using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000424 RID: 1060
	internal class DtlsRecordLayer : DatagramTransport
	{
		// Token: 0x06002A50 RID: 10832 RVA: 0x001114CC File Offset: 0x0010F6CC
		internal DtlsRecordLayer(DatagramTransport transport, TlsContext context, TlsPeer peer, byte contentType)
		{
			this.mTransport = transport;
			this.mContext = context;
			this.mPeer = peer;
			this.mInHandshake = true;
			this.mCurrentEpoch = new DtlsEpoch(0, new TlsNullCipher(context));
			this.mPendingEpoch = null;
			this.mReadEpoch = this.mCurrentEpoch;
			this.mWriteEpoch = this.mCurrentEpoch;
			this.SetPlaintextLimit(16384);
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x00111544 File Offset: 0x0010F744
		internal virtual void SetPlaintextLimit(int plaintextLimit)
		{
			this.mPlaintextLimit = plaintextLimit;
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06002A52 RID: 10834 RVA: 0x0011154F File Offset: 0x0010F74F
		internal virtual int ReadEpoch
		{
			get
			{
				return this.mReadEpoch.Epoch;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06002A53 RID: 10835 RVA: 0x0011155C File Offset: 0x0010F75C
		// (set) Token: 0x06002A54 RID: 10836 RVA: 0x00111566 File Offset: 0x0010F766
		internal virtual ProtocolVersion ReadVersion
		{
			get
			{
				return this.mReadVersion;
			}
			set
			{
				this.mReadVersion = value;
			}
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x00111571 File Offset: 0x0010F771
		internal virtual void SetWriteVersion(ProtocolVersion writeVersion)
		{
			this.mWriteVersion = writeVersion;
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x0011157C File Offset: 0x0010F77C
		internal virtual void InitPendingEpoch(TlsCipher pendingCipher)
		{
			if (this.mPendingEpoch != null)
			{
				throw new InvalidOperationException();
			}
			this.mPendingEpoch = new DtlsEpoch(this.mWriteEpoch.Epoch + 1, pendingCipher);
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x001115A8 File Offset: 0x0010F7A8
		internal virtual void HandshakeSuccessful(DtlsHandshakeRetransmit retransmit)
		{
			if (this.mReadEpoch == this.mCurrentEpoch || this.mWriteEpoch == this.mCurrentEpoch)
			{
				throw new InvalidOperationException();
			}
			if (retransmit != null)
			{
				this.mRetransmit = retransmit;
				this.mRetransmitEpoch = this.mCurrentEpoch;
				this.mRetransmitExpiry = DateTimeUtilities.CurrentUnixMs() + 240000L;
			}
			this.mInHandshake = false;
			this.mCurrentEpoch = this.mPendingEpoch;
			this.mPendingEpoch = null;
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x0011161B File Offset: 0x0010F81B
		internal virtual void ResetWriteEpoch()
		{
			if (this.mRetransmitEpoch != null)
			{
				this.mWriteEpoch = this.mRetransmitEpoch;
				return;
			}
			this.mWriteEpoch = this.mCurrentEpoch;
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x0011163E File Offset: 0x0010F83E
		public virtual int GetReceiveLimit()
		{
			return Math.Min(this.mPlaintextLimit, this.mReadEpoch.Cipher.GetPlaintextLimit(this.mTransport.GetReceiveLimit() - 13));
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x0011166B File Offset: 0x0010F86B
		public virtual int GetSendLimit()
		{
			return Math.Min(this.mPlaintextLimit, this.mWriteEpoch.Cipher.GetPlaintextLimit(this.mTransport.GetSendLimit() - 13));
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x00111698 File Offset: 0x0010F898
		public virtual int Receive(byte[] buf, int off, int len, int waitMillis)
		{
			byte[] array = null;
			int result;
			for (;;)
			{
				int num = Math.Min(len, this.GetReceiveLimit()) + 13;
				if (array == null || array.Length < num)
				{
					array = new byte[num];
				}
				try
				{
					if (this.mRetransmit != null && DateTimeUtilities.CurrentUnixMs() > this.mRetransmitExpiry)
					{
						this.mRetransmit = null;
						this.mRetransmitEpoch = null;
					}
					int num2 = this.ReceiveRecord(array, 0, num, waitMillis);
					if (num2 < 0)
					{
						result = num2;
					}
					else
					{
						if (num2 < 13)
						{
							continue;
						}
						int num3 = TlsUtilities.ReadUint16(array, 11);
						if (num2 != num3 + 13)
						{
							continue;
						}
						byte b = TlsUtilities.ReadUint8(array, 0);
						if (b - 20 > 4)
						{
							continue;
						}
						int num4 = TlsUtilities.ReadUint16(array, 3);
						DtlsEpoch dtlsEpoch = null;
						if (num4 == this.mReadEpoch.Epoch)
						{
							dtlsEpoch = this.mReadEpoch;
						}
						else if (b == 22 && this.mRetransmitEpoch != null && num4 == this.mRetransmitEpoch.Epoch)
						{
							dtlsEpoch = this.mRetransmitEpoch;
						}
						if (dtlsEpoch == null)
						{
							continue;
						}
						long num5 = TlsUtilities.ReadUint48(array, 5);
						if (dtlsEpoch.ReplayWindow.ShouldDiscard(num5))
						{
							continue;
						}
						ProtocolVersion protocolVersion = TlsUtilities.ReadVersion(array, 1);
						if (!protocolVersion.IsDtls)
						{
							continue;
						}
						if (this.mReadVersion != null && !this.mReadVersion.Equals(protocolVersion))
						{
							continue;
						}
						byte[] array2 = dtlsEpoch.Cipher.DecodeCiphertext(DtlsRecordLayer.GetMacSequenceNumber(dtlsEpoch.Epoch, num5), b, array, 13, num2 - 13);
						dtlsEpoch.ReplayWindow.ReportAuthenticated(num5);
						if (array2.Length > this.mPlaintextLimit)
						{
							continue;
						}
						if (this.mReadVersion == null)
						{
							this.mReadVersion = protocolVersion;
						}
						switch (b)
						{
						case 20:
							for (int i = 0; i < array2.Length; i++)
							{
								if (TlsUtilities.ReadUint8(array2, i) == 1 && this.mPendingEpoch != null)
								{
									this.mReadEpoch = this.mPendingEpoch;
								}
							}
							break;
						case 21:
							if (array2.Length == 2)
							{
								byte b2 = array2[0];
								byte b3 = array2[1];
								this.mPeer.NotifyAlertReceived(b2, b3);
								if (b2 == 2)
								{
									this.Failed();
									throw new TlsFatalAlert(b3);
								}
								if (b3 == 0)
								{
									this.CloseTransport();
								}
							}
							break;
						case 22:
							if (this.mInHandshake)
							{
								goto IL_268;
							}
							if (this.mRetransmit != null)
							{
								this.mRetransmit.ReceivedHandshakeRecord(num4, array2, 0, array2.Length);
							}
							break;
						case 23:
							if (!this.mInHandshake)
							{
								goto IL_268;
							}
							break;
						case 24:
							break;
						default:
							goto IL_268;
						}
						continue;
						IL_268:
						if (!this.mInHandshake && this.mRetransmit != null)
						{
							this.mRetransmit = null;
							this.mRetransmitEpoch = null;
						}
						Array.Copy(array2, 0, buf, off, array2.Length);
						result = array2.Length;
					}
				}
				catch (IOException ex)
				{
					throw ex;
				}
				break;
			}
			return result;
		}

		// Token: 0x06002A5C RID: 10844 RVA: 0x00111964 File Offset: 0x0010FB64
		public virtual void Send(byte[] buf, int off, int len)
		{
			byte contentType = 23;
			if (this.mInHandshake || this.mWriteEpoch == this.mRetransmitEpoch)
			{
				contentType = 22;
				if (TlsUtilities.ReadUint8(buf, off) == 20)
				{
					DtlsEpoch dtlsEpoch = null;
					if (this.mInHandshake)
					{
						dtlsEpoch = this.mPendingEpoch;
					}
					else if (this.mWriteEpoch == this.mRetransmitEpoch)
					{
						dtlsEpoch = this.mCurrentEpoch;
					}
					if (dtlsEpoch == null)
					{
						throw new InvalidOperationException();
					}
					byte[] array = new byte[]
					{
						1
					};
					this.SendRecord(20, array, 0, array.Length);
					this.mWriteEpoch = dtlsEpoch;
				}
			}
			this.SendRecord(contentType, buf, off, len);
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x001119F6 File Offset: 0x0010FBF6
		public virtual void Close()
		{
			if (!this.mClosed)
			{
				if (this.mInHandshake)
				{
					this.Warn(90, "User canceled handshake");
				}
				this.CloseTransport();
			}
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x00111A1F File Offset: 0x0010FC1F
		internal virtual void Failed()
		{
			if (!this.mClosed)
			{
				this.mFailed = true;
				this.CloseTransport();
			}
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x00111A3C File Offset: 0x0010FC3C
		internal virtual void Fail(byte alertDescription)
		{
			if (!this.mClosed)
			{
				try
				{
					this.RaiseAlert(2, alertDescription, null, null);
				}
				catch (Exception)
				{
				}
				this.mFailed = true;
				this.CloseTransport();
			}
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x00111A84 File Offset: 0x0010FC84
		internal virtual void Warn(byte alertDescription, string message)
		{
			this.RaiseAlert(1, alertDescription, message, null);
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x00111A90 File Offset: 0x0010FC90
		private void CloseTransport()
		{
			if (!this.mClosed)
			{
				try
				{
					if (!this.mFailed)
					{
						this.Warn(0, null);
					}
					this.mTransport.Close();
				}
				catch (Exception)
				{
				}
				this.mClosed = true;
			}
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x00111AE4 File Offset: 0x0010FCE4
		private void RaiseAlert(byte alertLevel, byte alertDescription, string message, Exception cause)
		{
			this.mPeer.NotifyAlertRaised(alertLevel, alertDescription, message, cause);
			this.SendRecord(21, new byte[]
			{
				alertLevel,
				alertDescription
			}, 0, 2);
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x00111B1C File Offset: 0x0010FD1C
		private int ReceiveRecord(byte[] buf, int off, int len, int waitMillis)
		{
			if (this.mRecordQueue.Available > 0)
			{
				int num = 0;
				if (this.mRecordQueue.Available >= 13)
				{
					byte[] buf2 = new byte[2];
					this.mRecordQueue.Read(buf2, 0, 2, 11);
					num = TlsUtilities.ReadUint16(buf2, 0);
				}
				int num2 = Math.Min(this.mRecordQueue.Available, 13 + num);
				this.mRecordQueue.RemoveData(buf, off, num2, 0);
				return num2;
			}
			int num3 = this.mTransport.Receive(buf, off, len, waitMillis);
			if (num3 >= 13)
			{
				int num4 = TlsUtilities.ReadUint16(buf, off + 11);
				int num5 = 13 + num4;
				if (num3 > num5)
				{
					this.mRecordQueue.AddData(buf, off + num5, num3 - num5);
					num3 = num5;
				}
			}
			return num3;
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x00111BD4 File Offset: 0x0010FDD4
		private void SendRecord(byte contentType, byte[] buf, int off, int len)
		{
			if (this.mWriteVersion == null)
			{
				return;
			}
			if (len > this.mPlaintextLimit)
			{
				throw new TlsFatalAlert(80);
			}
			if (len < 1 && contentType != 23)
			{
				throw new TlsFatalAlert(80);
			}
			int epoch = this.mWriteEpoch.Epoch;
			long num = this.mWriteEpoch.AllocateSequenceNumber();
			byte[] array = this.mWriteEpoch.Cipher.EncodePlaintext(DtlsRecordLayer.GetMacSequenceNumber(epoch, num), contentType, buf, off, len);
			byte[] array2 = new byte[array.Length + 13];
			TlsUtilities.WriteUint8(contentType, array2, 0);
			TlsUtilities.WriteVersion(this.mWriteVersion, array2, 1);
			TlsUtilities.WriteUint16(epoch, array2, 3);
			TlsUtilities.WriteUint48(num, array2, 5);
			TlsUtilities.WriteUint16(array.Length, array2, 11);
			Array.Copy(array, 0, array2, 13, array.Length);
			this.mTransport.Send(array2, 0, array2.Length);
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x00111CA2 File Offset: 0x0010FEA2
		private static long GetMacSequenceNumber(int epoch, long sequence_number)
		{
			return ((long)epoch & (long)((ulong)-1)) << 48 | sequence_number;
		}

		// Token: 0x04001CC9 RID: 7369
		private const int RECORD_HEADER_LENGTH = 13;

		// Token: 0x04001CCA RID: 7370
		private const int MAX_FRAGMENT_LENGTH = 16384;

		// Token: 0x04001CCB RID: 7371
		private const long TCP_MSL = 120000L;

		// Token: 0x04001CCC RID: 7372
		private const long RETRANSMIT_TIMEOUT = 240000L;

		// Token: 0x04001CCD RID: 7373
		private readonly DatagramTransport mTransport;

		// Token: 0x04001CCE RID: 7374
		private readonly TlsContext mContext;

		// Token: 0x04001CCF RID: 7375
		private readonly TlsPeer mPeer;

		// Token: 0x04001CD0 RID: 7376
		private readonly ByteQueue mRecordQueue = new ByteQueue();

		// Token: 0x04001CD1 RID: 7377
		private volatile bool mClosed;

		// Token: 0x04001CD2 RID: 7378
		private volatile bool mFailed;

		// Token: 0x04001CD3 RID: 7379
		private volatile ProtocolVersion mReadVersion;

		// Token: 0x04001CD4 RID: 7380
		private volatile ProtocolVersion mWriteVersion;

		// Token: 0x04001CD5 RID: 7381
		private volatile bool mInHandshake;

		// Token: 0x04001CD6 RID: 7382
		private volatile int mPlaintextLimit;

		// Token: 0x04001CD7 RID: 7383
		private DtlsEpoch mCurrentEpoch;

		// Token: 0x04001CD8 RID: 7384
		private DtlsEpoch mPendingEpoch;

		// Token: 0x04001CD9 RID: 7385
		private DtlsEpoch mReadEpoch;

		// Token: 0x04001CDA RID: 7386
		private DtlsEpoch mWriteEpoch;

		// Token: 0x04001CDB RID: 7387
		private DtlsHandshakeRetransmit mRetransmit;

		// Token: 0x04001CDC RID: 7388
		private DtlsEpoch mRetransmitEpoch;

		// Token: 0x04001CDD RID: 7389
		private long mRetransmitExpiry;
	}
}
