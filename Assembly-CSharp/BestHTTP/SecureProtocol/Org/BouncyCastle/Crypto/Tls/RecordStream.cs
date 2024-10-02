using System;
using System.IO;
using BestHTTP.Extensions;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000441 RID: 1089
	internal sealed class RecordStream
	{
		// Token: 0x06002AE6 RID: 10982 RVA: 0x00113A00 File Offset: 0x00111C00
		internal RecordStream(TlsProtocol handler, Stream input, Stream output)
		{
			this.mHandler = handler;
			this.mInput = input;
			this.mOutput = output;
			this.mReadCompression = new TlsNullCompression();
			this.mWriteCompression = this.mReadCompression;
			this.mHandshakeHashUpdater = new RecordStream.HandshakeHashUpdateStream(this);
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x00113A73 File Offset: 0x00111C73
		internal void Init(TlsContext context)
		{
			this.mReadCipher = new TlsNullCipher(context);
			this.mWriteCipher = this.mReadCipher;
			this.mHandshakeHash = new DeferredHash();
			this.mHandshakeHash.Init(context);
			this.SetPlaintextLimit(16384);
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x00113AAF File Offset: 0x00111CAF
		internal int GetPlaintextLimit()
		{
			return this.mPlaintextLimit;
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x00113AB7 File Offset: 0x00111CB7
		internal void SetPlaintextLimit(int plaintextLimit)
		{
			this.mPlaintextLimit = plaintextLimit;
			this.mCompressedLimit = this.mPlaintextLimit + 1024;
			this.mCiphertextLimit = this.mCompressedLimit + 1024;
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06002AEA RID: 10986 RVA: 0x00113AE4 File Offset: 0x00111CE4
		// (set) Token: 0x06002AEB RID: 10987 RVA: 0x00113AEC File Offset: 0x00111CEC
		internal ProtocolVersion ReadVersion
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

		// Token: 0x06002AEC RID: 10988 RVA: 0x00113AF5 File Offset: 0x00111CF5
		internal void SetWriteVersion(ProtocolVersion writeVersion)
		{
			this.mWriteVersion = writeVersion;
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x00113AFE File Offset: 0x00111CFE
		internal void SetRestrictReadVersion(bool enabled)
		{
			this.mRestrictReadVersion = enabled;
		}

		// Token: 0x06002AEE RID: 10990 RVA: 0x00113B07 File Offset: 0x00111D07
		internal void SetPendingConnectionState(TlsCompression tlsCompression, TlsCipher tlsCipher)
		{
			this.mPendingCompression = tlsCompression;
			this.mPendingCipher = tlsCipher;
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x00113B17 File Offset: 0x00111D17
		internal void SentWriteCipherSpec()
		{
			if (this.mPendingCompression == null || this.mPendingCipher == null)
			{
				throw new TlsFatalAlert(40);
			}
			this.mWriteCompression = this.mPendingCompression;
			this.mWriteCipher = this.mPendingCipher;
			this.mWriteSeqNo = new RecordStream.SequenceNumber();
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x00113B54 File Offset: 0x00111D54
		internal void ReceivedReadCipherSpec()
		{
			if (this.mPendingCompression == null || this.mPendingCipher == null)
			{
				throw new TlsFatalAlert(40);
			}
			this.mReadCompression = this.mPendingCompression;
			this.mReadCipher = this.mPendingCipher;
			this.mReadSeqNo = new RecordStream.SequenceNumber();
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x00113B94 File Offset: 0x00111D94
		internal void FinaliseHandshake()
		{
			if (this.mReadCompression != this.mPendingCompression || this.mWriteCompression != this.mPendingCompression || this.mReadCipher != this.mPendingCipher || this.mWriteCipher != this.mPendingCipher)
			{
				throw new TlsFatalAlert(40);
			}
			this.mPendingCompression = null;
			this.mPendingCipher = null;
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x00113BF0 File Offset: 0x00111DF0
		internal void CheckRecordHeader(byte[] recordHeader)
		{
			RecordStream.CheckType(TlsUtilities.ReadUint8(recordHeader, 0), 10);
			if (!this.mRestrictReadVersion)
			{
				if (((long)TlsUtilities.ReadVersionRaw(recordHeader, 1) & (long)((ulong)-256)) != 768L)
				{
					throw new TlsFatalAlert(47);
				}
			}
			else
			{
				ProtocolVersion protocolVersion = TlsUtilities.ReadVersion(recordHeader, 1);
				if (this.mReadVersion != null && !protocolVersion.Equals(this.mReadVersion))
				{
					throw new TlsFatalAlert(47);
				}
			}
			RecordStream.CheckLength(TlsUtilities.ReadUint16(recordHeader, 3), this.mCiphertextLimit, 22);
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x00113C6C File Offset: 0x00111E6C
		internal bool ReadRecord()
		{
			byte[] array = TlsUtilities.ReadAllOrNothing(5, this.mInput);
			if (array == null)
			{
				return false;
			}
			byte b = TlsUtilities.ReadUint8(array, 0);
			RecordStream.CheckType(b, 10);
			if (!this.mRestrictReadVersion)
			{
				if (((long)TlsUtilities.ReadVersionRaw(array, 1) & (long)((ulong)-256)) != 768L)
				{
					throw new TlsFatalAlert(47);
				}
			}
			else
			{
				ProtocolVersion protocolVersion = TlsUtilities.ReadVersion(array, 1);
				if (this.mReadVersion == null)
				{
					this.mReadVersion = protocolVersion;
				}
				else if (!protocolVersion.Equals(this.mReadVersion))
				{
					throw new TlsFatalAlert(47);
				}
			}
			int num = TlsUtilities.ReadUint16(array, 3);
			RecordStream.CheckLength(num, this.mCiphertextLimit, 22);
			byte[] array2 = this.DecodeAndVerify(b, this.mInput, num);
			this.mHandler.ProcessRecord(b, array2, 0, array2.Length);
			return true;
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x00113D2C File Offset: 0x00111F2C
		internal byte[] DecodeAndVerify(byte type, Stream input, int len)
		{
			byte[] array = TlsUtilities.ReadFully(len, input);
			long seqNo = this.mReadSeqNo.NextValue(10);
			byte[] array2 = this.mReadCipher.DecodeCiphertext(seqNo, type, array, 0, array.Length);
			RecordStream.CheckLength(array2.Length, this.mCompressedLimit, 22);
			Stream stream = this.mReadCompression.Decompress(this.mBuffer);
			if (stream != this.mBuffer)
			{
				stream.Write(array2, 0, array2.Length);
				stream.Flush();
				array2 = this.GetBufferContents();
			}
			RecordStream.CheckLength(array2.Length, this.mPlaintextLimit, 30);
			if (array2.Length < 1 && type != 23)
			{
				throw new TlsFatalAlert(47);
			}
			return array2;
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x00113DCC File Offset: 0x00111FCC
		internal void WriteRecord(byte type, byte[] plaintext, int plaintextOffset, int plaintextLength)
		{
			if (this.mWriteVersion == null)
			{
				return;
			}
			RecordStream.CheckType(type, 80);
			RecordStream.CheckLength(plaintextLength, this.mPlaintextLimit, 80);
			if (plaintextLength < 1 && type != 23)
			{
				throw new TlsFatalAlert(80);
			}
			Stream stream = this.mWriteCompression.Compress(this.mBuffer);
			long seqNo = this.mWriteSeqNo.NextValue(80);
			byte[] array;
			if (stream == this.mBuffer)
			{
				array = this.mWriteCipher.EncodePlaintext(seqNo, type, plaintext, plaintextOffset, plaintextLength);
			}
			else
			{
				stream.Write(plaintext, plaintextOffset, plaintextLength);
				stream.Flush();
				byte[] bufferContents = this.GetBufferContents();
				RecordStream.CheckLength(bufferContents.Length, plaintextLength + 1024, 80);
				array = this.mWriteCipher.EncodePlaintext(seqNo, type, bufferContents, 0, bufferContents.Length);
			}
			RecordStream.CheckLength(array.Length, this.mCiphertextLimit, 80);
			int num = array.Length + 5;
			byte[] array2 = VariableSizedBufferPool.Get((long)num, true);
			TlsUtilities.WriteUint8(type, array2, 0);
			TlsUtilities.WriteVersion(this.mWriteVersion, array2, 1);
			TlsUtilities.WriteUint16(array.Length, array2, 3);
			Array.Copy(array, 0, array2, 5, array.Length);
			this.mOutput.Write(array2, 0, num);
			VariableSizedBufferPool.Release(array2);
			this.mOutput.Flush();
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x00113EF8 File Offset: 0x001120F8
		internal void NotifyHelloComplete()
		{
			this.mHandshakeHash = this.mHandshakeHash.NotifyPrfDetermined();
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06002AF7 RID: 10999 RVA: 0x00113F0B File Offset: 0x0011210B
		internal TlsHandshakeHash HandshakeHash
		{
			get
			{
				return this.mHandshakeHash;
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06002AF8 RID: 11000 RVA: 0x00113F13 File Offset: 0x00112113
		internal Stream HandshakeHashUpdater
		{
			get
			{
				return this.mHandshakeHashUpdater;
			}
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x00113F1B File Offset: 0x0011211B
		internal TlsHandshakeHash PrepareToFinish()
		{
			TlsHandshakeHash result = this.mHandshakeHash;
			this.mHandshakeHash = this.mHandshakeHash.StopTracking();
			return result;
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x00113F34 File Offset: 0x00112134
		internal void SafeClose()
		{
			try
			{
				Platform.Dispose(this.mInput);
			}
			catch (IOException)
			{
			}
			try
			{
				Platform.Dispose(this.mOutput);
			}
			catch (IOException)
			{
			}
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x00113F80 File Offset: 0x00112180
		internal void Flush()
		{
			this.mOutput.Flush();
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x00113F8D File Offset: 0x0011218D
		private byte[] GetBufferContents()
		{
			byte[] result = this.mBuffer.ToArray();
			this.mBuffer.SetLength(0L);
			return result;
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x00113FA7 File Offset: 0x001121A7
		private static void CheckType(byte type, byte alertDescription)
		{
			if (type - 20 > 3)
			{
				throw new TlsFatalAlert(alertDescription);
			}
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x00113FB7 File Offset: 0x001121B7
		private static void CheckLength(int length, int limit, byte alertDescription)
		{
			if (length > limit)
			{
				throw new TlsFatalAlert(alertDescription);
			}
		}

		// Token: 0x04001DAE RID: 7598
		private const int DEFAULT_PLAINTEXT_LIMIT = 16384;

		// Token: 0x04001DAF RID: 7599
		internal const int TLS_HEADER_SIZE = 5;

		// Token: 0x04001DB0 RID: 7600
		internal const int TLS_HEADER_TYPE_OFFSET = 0;

		// Token: 0x04001DB1 RID: 7601
		internal const int TLS_HEADER_VERSION_OFFSET = 1;

		// Token: 0x04001DB2 RID: 7602
		internal const int TLS_HEADER_LENGTH_OFFSET = 3;

		// Token: 0x04001DB3 RID: 7603
		private TlsProtocol mHandler;

		// Token: 0x04001DB4 RID: 7604
		private Stream mInput;

		// Token: 0x04001DB5 RID: 7605
		private Stream mOutput;

		// Token: 0x04001DB6 RID: 7606
		private TlsCompression mPendingCompression;

		// Token: 0x04001DB7 RID: 7607
		private TlsCompression mReadCompression;

		// Token: 0x04001DB8 RID: 7608
		private TlsCompression mWriteCompression;

		// Token: 0x04001DB9 RID: 7609
		private TlsCipher mPendingCipher;

		// Token: 0x04001DBA RID: 7610
		private TlsCipher mReadCipher;

		// Token: 0x04001DBB RID: 7611
		private TlsCipher mWriteCipher;

		// Token: 0x04001DBC RID: 7612
		private RecordStream.SequenceNumber mReadSeqNo = new RecordStream.SequenceNumber();

		// Token: 0x04001DBD RID: 7613
		private RecordStream.SequenceNumber mWriteSeqNo = new RecordStream.SequenceNumber();

		// Token: 0x04001DBE RID: 7614
		private MemoryStream mBuffer = new MemoryStream();

		// Token: 0x04001DBF RID: 7615
		private TlsHandshakeHash mHandshakeHash;

		// Token: 0x04001DC0 RID: 7616
		private readonly BaseOutputStream mHandshakeHashUpdater;

		// Token: 0x04001DC1 RID: 7617
		private ProtocolVersion mReadVersion;

		// Token: 0x04001DC2 RID: 7618
		private ProtocolVersion mWriteVersion;

		// Token: 0x04001DC3 RID: 7619
		private bool mRestrictReadVersion = true;

		// Token: 0x04001DC4 RID: 7620
		private int mPlaintextLimit;

		// Token: 0x04001DC5 RID: 7621
		private int mCompressedLimit;

		// Token: 0x04001DC6 RID: 7622
		private int mCiphertextLimit;

		// Token: 0x02000945 RID: 2373
		private class HandshakeHashUpdateStream : BaseOutputStream
		{
			// Token: 0x06004EDB RID: 20187 RVA: 0x001B3412 File Offset: 0x001B1612
			public HandshakeHashUpdateStream(RecordStream mOuter)
			{
				this.mOuter = mOuter;
			}

			// Token: 0x06004EDC RID: 20188 RVA: 0x001B3421 File Offset: 0x001B1621
			public override void Write(byte[] buf, int off, int len)
			{
				this.mOuter.mHandshakeHash.BlockUpdate(buf, off, len);
			}

			// Token: 0x04003615 RID: 13845
			private readonly RecordStream mOuter;
		}

		// Token: 0x02000946 RID: 2374
		private class SequenceNumber
		{
			// Token: 0x06004EDD RID: 20189 RVA: 0x001B3438 File Offset: 0x001B1638
			internal long NextValue(byte alertDescription)
			{
				if (this.exhausted)
				{
					throw new TlsFatalAlert(alertDescription);
				}
				long result = this.value;
				long num = this.value + 1L;
				this.value = num;
				if (num == 0L)
				{
					this.exhausted = true;
				}
				return result;
			}

			// Token: 0x04003616 RID: 13846
			private long value;

			// Token: 0x04003617 RID: 13847
			private bool exhausted;
		}
	}
}
