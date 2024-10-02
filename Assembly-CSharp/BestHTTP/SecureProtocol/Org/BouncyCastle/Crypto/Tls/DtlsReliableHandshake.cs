using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000425 RID: 1061
	internal class DtlsReliableHandshake
	{
		// Token: 0x06002A66 RID: 10854 RVA: 0x00111CB0 File Offset: 0x0010FEB0
		internal DtlsReliableHandshake(TlsContext context, DtlsRecordLayer transport)
		{
			this.mRecordLayer = transport;
			this.mHandshakeHash = new DeferredHash();
			this.mHandshakeHash.Init(context);
		}

		// Token: 0x06002A67 RID: 10855 RVA: 0x00111CFE File Offset: 0x0010FEFE
		internal void NotifyHelloComplete()
		{
			this.mHandshakeHash = this.mHandshakeHash.NotifyPrfDetermined();
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06002A68 RID: 10856 RVA: 0x00111D11 File Offset: 0x0010FF11
		internal TlsHandshakeHash HandshakeHash
		{
			get
			{
				return this.mHandshakeHash;
			}
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x00111D19 File Offset: 0x0010FF19
		internal TlsHandshakeHash PrepareToFinish()
		{
			TlsHandshakeHash result = this.mHandshakeHash;
			this.mHandshakeHash = this.mHandshakeHash.StopTracking();
			return result;
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x00111D34 File Offset: 0x0010FF34
		internal void SendMessage(byte msg_type, byte[] body)
		{
			TlsUtilities.CheckUint24(body.Length);
			if (!this.mSending)
			{
				this.CheckInboundFlight();
				this.mSending = true;
				this.mOutboundFlight.Clear();
			}
			int num = this.mMessageSeq;
			this.mMessageSeq = num + 1;
			DtlsReliableHandshake.Message message = new DtlsReliableHandshake.Message(num, msg_type, body);
			this.mOutboundFlight.Add(message);
			this.WriteMessage(message);
			this.UpdateHandshakeMessagesDigest(message);
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x00111D9E File Offset: 0x0010FF9E
		internal byte[] ReceiveMessageBody(byte msg_type)
		{
			DtlsReliableHandshake.Message message = this.ReceiveMessage();
			if (message.Type != msg_type)
			{
				throw new TlsFatalAlert(10);
			}
			return message.Body;
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x00111DBC File Offset: 0x0010FFBC
		internal DtlsReliableHandshake.Message ReceiveMessage()
		{
			if (this.mSending)
			{
				this.mSending = false;
				this.PrepareInboundFlight(Platform.CreateHashtable());
			}
			byte[] array = null;
			int num = 1000;
			DtlsReliableHandshake.Message result;
			for (;;)
			{
				try
				{
					DtlsReliableHandshake.Message pendingMessage;
					for (;;)
					{
						pendingMessage = this.GetPendingMessage();
						if (pendingMessage != null)
						{
							break;
						}
						int receiveLimit = this.mRecordLayer.GetReceiveLimit();
						if (array == null || array.Length < receiveLimit)
						{
							array = new byte[receiveLimit];
						}
						int num2 = this.mRecordLayer.Receive(array, 0, receiveLimit, num);
						if (num2 < 0)
						{
							goto IL_87;
						}
						if (this.ProcessRecord(16, this.mRecordLayer.ReadEpoch, array, 0, num2))
						{
							num = this.BackOff(num);
						}
					}
					result = pendingMessage;
					break;
					IL_87:;
				}
				catch (IOException)
				{
				}
				this.ResendOutboundFlight();
				num = this.BackOff(num);
			}
			return result;
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x00111E78 File Offset: 0x00110078
		internal void Finish()
		{
			DtlsHandshakeRetransmit retransmit = null;
			if (!this.mSending)
			{
				this.CheckInboundFlight();
			}
			else
			{
				this.PrepareInboundFlight(null);
				if (this.mPreviousInboundFlight != null)
				{
					retransmit = new DtlsReliableHandshake.Retransmit(this);
				}
			}
			this.mRecordLayer.HandshakeSuccessful(retransmit);
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x00111EB9 File Offset: 0x001100B9
		internal void ResetHandshakeMessagesDigest()
		{
			this.mHandshakeHash.Reset();
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x00111EC6 File Offset: 0x001100C6
		private int BackOff(int timeoutMillis)
		{
			return Math.Min(timeoutMillis * 2, 60000);
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x00111ED8 File Offset: 0x001100D8
		private void CheckInboundFlight()
		{
			foreach (object obj in this.mCurrentInboundFlight.Keys)
			{
				int num = (int)obj;
				int num2 = this.mNextReceiveSeq;
			}
		}

		// Token: 0x06002A71 RID: 10865 RVA: 0x00111F38 File Offset: 0x00110138
		private DtlsReliableHandshake.Message GetPendingMessage()
		{
			DtlsReassembler dtlsReassembler = (DtlsReassembler)this.mCurrentInboundFlight[this.mNextReceiveSeq];
			if (dtlsReassembler != null)
			{
				byte[] bodyIfComplete = dtlsReassembler.GetBodyIfComplete();
				if (bodyIfComplete != null)
				{
					this.mPreviousInboundFlight = null;
					int num = this.mNextReceiveSeq;
					this.mNextReceiveSeq = num + 1;
					return this.UpdateHandshakeMessagesDigest(new DtlsReliableHandshake.Message(num, dtlsReassembler.MsgType, bodyIfComplete));
				}
			}
			return null;
		}

		// Token: 0x06002A72 RID: 10866 RVA: 0x00111F9A File Offset: 0x0011019A
		private void PrepareInboundFlight(IDictionary nextFlight)
		{
			DtlsReliableHandshake.ResetAll(this.mCurrentInboundFlight);
			this.mPreviousInboundFlight = this.mCurrentInboundFlight;
			this.mCurrentInboundFlight = nextFlight;
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x00111FBC File Offset: 0x001101BC
		private bool ProcessRecord(int windowSize, int epoch, byte[] buf, int off, int len)
		{
			bool flag = false;
			while (len >= 12)
			{
				int num = TlsUtilities.ReadUint24(buf, off + 9);
				int num2 = num + 12;
				if (len < num2)
				{
					break;
				}
				int num3 = TlsUtilities.ReadUint24(buf, off + 1);
				int num4 = TlsUtilities.ReadUint24(buf, off + 6);
				if (num4 + num > num3)
				{
					break;
				}
				byte b = TlsUtilities.ReadUint8(buf, off);
				int num5 = (b == 20) ? 1 : 0;
				if (epoch != num5)
				{
					break;
				}
				int num6 = TlsUtilities.ReadUint16(buf, off + 4);
				if (num6 < this.mNextReceiveSeq + windowSize)
				{
					if (num6 >= this.mNextReceiveSeq)
					{
						DtlsReassembler dtlsReassembler = (DtlsReassembler)this.mCurrentInboundFlight[num6];
						if (dtlsReassembler == null)
						{
							dtlsReassembler = new DtlsReassembler(b, num3);
							this.mCurrentInboundFlight[num6] = dtlsReassembler;
						}
						dtlsReassembler.ContributeFragment(b, num3, buf, off + 12, num4, num);
					}
					else if (this.mPreviousInboundFlight != null)
					{
						DtlsReassembler dtlsReassembler2 = (DtlsReassembler)this.mPreviousInboundFlight[num6];
						if (dtlsReassembler2 != null)
						{
							dtlsReassembler2.ContributeFragment(b, num3, buf, off + 12, num4, num);
							flag = true;
						}
					}
				}
				off += num2;
				len -= num2;
			}
			bool flag2 = flag && DtlsReliableHandshake.CheckAll(this.mPreviousInboundFlight);
			if (flag2)
			{
				this.ResendOutboundFlight();
				DtlsReliableHandshake.ResetAll(this.mPreviousInboundFlight);
			}
			return flag2;
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x00112114 File Offset: 0x00110314
		private void ResendOutboundFlight()
		{
			this.mRecordLayer.ResetWriteEpoch();
			for (int i = 0; i < this.mOutboundFlight.Count; i++)
			{
				this.WriteMessage((DtlsReliableHandshake.Message)this.mOutboundFlight[i]);
			}
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x0011215C File Offset: 0x0011035C
		private DtlsReliableHandshake.Message UpdateHandshakeMessagesDigest(DtlsReliableHandshake.Message message)
		{
			if (message.Type != 0)
			{
				byte[] body = message.Body;
				byte[] array = new byte[12];
				TlsUtilities.WriteUint8(message.Type, array, 0);
				TlsUtilities.WriteUint24(body.Length, array, 1);
				TlsUtilities.WriteUint16(message.Seq, array, 4);
				TlsUtilities.WriteUint24(0, array, 6);
				TlsUtilities.WriteUint24(body.Length, array, 9);
				this.mHandshakeHash.BlockUpdate(array, 0, array.Length);
				this.mHandshakeHash.BlockUpdate(body, 0, body.Length);
			}
			return message;
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x001121D8 File Offset: 0x001103D8
		private void WriteMessage(DtlsReliableHandshake.Message message)
		{
			int num = this.mRecordLayer.GetSendLimit() - 12;
			if (num < 1)
			{
				throw new TlsFatalAlert(80);
			}
			int num2 = message.Body.Length;
			int num3 = 0;
			do
			{
				int num4 = Math.Min(num2 - num3, num);
				this.WriteHandshakeFragment(message, num3, num4);
				num3 += num4;
			}
			while (num3 < num2);
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x00112228 File Offset: 0x00110428
		private void WriteHandshakeFragment(DtlsReliableHandshake.Message message, int fragment_offset, int fragment_length)
		{
			DtlsReliableHandshake.RecordLayerBuffer recordLayerBuffer = new DtlsReliableHandshake.RecordLayerBuffer(12 + fragment_length);
			TlsUtilities.WriteUint8(message.Type, recordLayerBuffer);
			TlsUtilities.WriteUint24(message.Body.Length, recordLayerBuffer);
			TlsUtilities.WriteUint16(message.Seq, recordLayerBuffer);
			TlsUtilities.WriteUint24(fragment_offset, recordLayerBuffer);
			TlsUtilities.WriteUint24(fragment_length, recordLayerBuffer);
			recordLayerBuffer.Write(message.Body, fragment_offset, fragment_length);
			recordLayerBuffer.SendToRecordLayer(this.mRecordLayer);
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x00112290 File Offset: 0x00110490
		private static bool CheckAll(IDictionary inboundFlight)
		{
			using (IEnumerator enumerator = inboundFlight.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((DtlsReassembler)enumerator.Current).GetBodyIfComplete() == null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x001122F0 File Offset: 0x001104F0
		private static void ResetAll(IDictionary inboundFlight)
		{
			foreach (object obj in inboundFlight.Values)
			{
				((DtlsReassembler)obj).Reset();
			}
		}

		// Token: 0x04001CDE RID: 7390
		private const int MaxReceiveAhead = 16;

		// Token: 0x04001CDF RID: 7391
		private const int MessageHeaderLength = 12;

		// Token: 0x04001CE0 RID: 7392
		private readonly DtlsRecordLayer mRecordLayer;

		// Token: 0x04001CE1 RID: 7393
		private TlsHandshakeHash mHandshakeHash;

		// Token: 0x04001CE2 RID: 7394
		private IDictionary mCurrentInboundFlight = Platform.CreateHashtable();

		// Token: 0x04001CE3 RID: 7395
		private IDictionary mPreviousInboundFlight;

		// Token: 0x04001CE4 RID: 7396
		private IList mOutboundFlight = Platform.CreateArrayList();

		// Token: 0x04001CE5 RID: 7397
		private bool mSending = true;

		// Token: 0x04001CE6 RID: 7398
		private int mMessageSeq;

		// Token: 0x04001CE7 RID: 7399
		private int mNextReceiveSeq;

		// Token: 0x02000940 RID: 2368
		internal class Message
		{
			// Token: 0x06004ED0 RID: 20176 RVA: 0x001B334A File Offset: 0x001B154A
			internal Message(int message_seq, byte msg_type, byte[] body)
			{
				this.mMessageSeq = message_seq;
				this.mMsgType = msg_type;
				this.mBody = body;
			}

			// Token: 0x17000C55 RID: 3157
			// (get) Token: 0x06004ED1 RID: 20177 RVA: 0x001B3367 File Offset: 0x001B1567
			public int Seq
			{
				get
				{
					return this.mMessageSeq;
				}
			}

			// Token: 0x17000C56 RID: 3158
			// (get) Token: 0x06004ED2 RID: 20178 RVA: 0x001B336F File Offset: 0x001B156F
			public byte Type
			{
				get
				{
					return this.mMsgType;
				}
			}

			// Token: 0x17000C57 RID: 3159
			// (get) Token: 0x06004ED3 RID: 20179 RVA: 0x001B3377 File Offset: 0x001B1577
			public byte[] Body
			{
				get
				{
					return this.mBody;
				}
			}

			// Token: 0x040035FF RID: 13823
			private readonly int mMessageSeq;

			// Token: 0x04003600 RID: 13824
			private readonly byte mMsgType;

			// Token: 0x04003601 RID: 13825
			private readonly byte[] mBody;
		}

		// Token: 0x02000941 RID: 2369
		internal class RecordLayerBuffer : MemoryStream
		{
			// Token: 0x06004ED4 RID: 20180 RVA: 0x001B337F File Offset: 0x001B157F
			internal RecordLayerBuffer(int size) : base(size)
			{
			}

			// Token: 0x06004ED5 RID: 20181 RVA: 0x001B3388 File Offset: 0x001B1588
			internal void SendToRecordLayer(DtlsRecordLayer recordLayer)
			{
				byte[] buffer = this.GetBuffer();
				int len = (int)this.Length;
				recordLayer.Send(buffer, 0, len);
				Platform.Dispose(this);
			}
		}

		// Token: 0x02000942 RID: 2370
		internal class Retransmit : DtlsHandshakeRetransmit
		{
			// Token: 0x06004ED6 RID: 20182 RVA: 0x001B33B3 File Offset: 0x001B15B3
			internal Retransmit(DtlsReliableHandshake outer)
			{
				this.mOuter = outer;
			}

			// Token: 0x06004ED7 RID: 20183 RVA: 0x001B33C2 File Offset: 0x001B15C2
			public void ReceivedHandshakeRecord(int epoch, byte[] buf, int off, int len)
			{
				this.mOuter.ProcessRecord(0, epoch, buf, off, len);
			}

			// Token: 0x04003602 RID: 13826
			private readonly DtlsReliableHandshake mOuter;
		}
	}
}
