using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000476 RID: 1142
	public abstract class TlsProtocol
	{
		// Token: 0x06002C8C RID: 11404 RVA: 0x001187AA File Offset: 0x001169AA
		public TlsProtocol(Stream stream, SecureRandom secureRandom) : this(stream, stream, secureRandom)
		{
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x001187B8 File Offset: 0x001169B8
		public TlsProtocol(Stream input, Stream output, SecureRandom secureRandom)
		{
			this.mApplicationDataQueue = new ByteQueue(0);
			this.mAlertQueue = new ByteQueue(2);
			this.mHandshakeQueue = new ByteQueue(0);
			this.mAppDataSplitEnabled = true;
			this.mBlocking = true;
			base..ctor();
			this.mRecordStream = new RecordStream(this, input, output);
			this.mSecureRandom = secureRandom;
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x00118814 File Offset: 0x00116A14
		public TlsProtocol(SecureRandom secureRandom)
		{
			this.mApplicationDataQueue = new ByteQueue(0);
			this.mAlertQueue = new ByteQueue(2);
			this.mHandshakeQueue = new ByteQueue(0);
			this.mAppDataSplitEnabled = true;
			this.mBlocking = true;
			base..ctor();
			this.mBlocking = false;
			this.mInputBuffers = new ByteQueueStream();
			this.mOutputBuffer = new ByteQueueStream();
			this.mRecordStream = new RecordStream(this, this.mInputBuffers, this.mOutputBuffer);
			this.mSecureRandom = secureRandom;
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06002C8F RID: 11407
		protected abstract TlsContext Context { get; }

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06002C90 RID: 11408
		internal abstract AbstractTlsContext ContextAdmin { get; }

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06002C91 RID: 11409
		protected abstract TlsPeer Peer { get; }

		// Token: 0x06002C92 RID: 11410 RVA: 0x00118897 File Offset: 0x00116A97
		protected virtual void HandleAlertMessage(byte alertLevel, byte alertDescription)
		{
			this.Peer.NotifyAlertReceived(alertLevel, alertDescription);
			if (alertLevel == 1)
			{
				this.HandleAlertWarningMessage(alertDescription);
				return;
			}
			this.HandleFailure();
			throw new TlsFatalAlertReceived(alertDescription);
		}

		// Token: 0x06002C93 RID: 11411 RVA: 0x001188BE File Offset: 0x00116ABE
		protected virtual void HandleAlertWarningMessage(byte alertDescription)
		{
			if (alertDescription == 0)
			{
				if (!this.mAppDataReady)
				{
					throw new TlsFatalAlert(40);
				}
				this.HandleClose(false);
			}
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x0000248C File Offset: 0x0000068C
		protected virtual void HandleChangeCipherSpecMessage()
		{
		}

		// Token: 0x06002C95 RID: 11413 RVA: 0x001188DC File Offset: 0x00116ADC
		protected virtual void HandleClose(bool user_canceled)
		{
			if (!this.mClosed)
			{
				this.mClosed = true;
				if (user_canceled && !this.mAppDataReady)
				{
					this.RaiseAlertWarning(90, "User canceled handshake");
				}
				this.RaiseAlertWarning(0, "Connection closed");
				this.mRecordStream.SafeClose();
				if (!this.mAppDataReady)
				{
					this.CleanupHandshake();
				}
			}
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x0011893D File Offset: 0x00116B3D
		protected virtual void HandleException(byte alertDescription, string message, Exception cause)
		{
			if (!this.mClosed)
			{
				this.RaiseAlertFatal(alertDescription, message, cause);
				this.HandleFailure();
			}
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x00118958 File Offset: 0x00116B58
		protected virtual void HandleFailure()
		{
			this.mClosed = true;
			this.mFailedWithError = true;
			this.InvalidateSession();
			this.mRecordStream.SafeClose();
			if (!this.mAppDataReady)
			{
				this.CleanupHandshake();
			}
		}

		// Token: 0x06002C98 RID: 11416
		protected abstract void HandleHandshakeMessage(byte type, MemoryStream buf);

		// Token: 0x06002C99 RID: 11417 RVA: 0x00118990 File Offset: 0x00116B90
		protected virtual void ApplyMaxFragmentLengthExtension()
		{
			if (this.mSecurityParameters.maxFragmentLength >= 0)
			{
				if (!MaxFragmentLength.IsValid((byte)this.mSecurityParameters.maxFragmentLength))
				{
					throw new TlsFatalAlert(80);
				}
				int plaintextLimit = 1 << (int)(8 + this.mSecurityParameters.maxFragmentLength);
				this.mRecordStream.SetPlaintextLimit(plaintextLimit);
			}
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x001189E5 File Offset: 0x00116BE5
		protected virtual void CheckReceivedChangeCipherSpec(bool expected)
		{
			if (expected != this.mReceivedChangeCipherSpec)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x001189F8 File Offset: 0x00116BF8
		protected virtual void CleanupHandshake()
		{
			if (this.mExpectedVerifyData != null)
			{
				Arrays.Fill(this.mExpectedVerifyData, 0);
				this.mExpectedVerifyData = null;
			}
			this.mSecurityParameters.Clear();
			this.mPeerCertificate = null;
			this.mOfferedCipherSuites = null;
			this.mOfferedCompressionMethods = null;
			this.mClientExtensions = null;
			this.mServerExtensions = null;
			this.mResumedSession = false;
			this.mReceivedChangeCipherSpec = false;
			this.mSecureRenegotiation = false;
			this.mAllowCertificateStatus = false;
			this.mExpectSessionTicket = false;
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x00118A71 File Offset: 0x00116C71
		protected virtual void BlockForHandshake()
		{
			if (this.mBlocking)
			{
				while (this.mConnectionState != 16)
				{
					if (this.mClosed)
					{
						throw new TlsFatalAlert(80);
					}
					this.SafeReadRecord();
				}
			}
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x00118AA0 File Offset: 0x00116CA0
		protected virtual void CompleteHandshake()
		{
			try
			{
				this.mConnectionState = 16;
				this.mAlertQueue.Shrink();
				this.mHandshakeQueue.Shrink();
				this.mRecordStream.FinaliseHandshake();
				this.mAppDataSplitEnabled = !TlsUtilities.IsTlsV11(this.Context);
				if (!this.mAppDataReady)
				{
					this.mAppDataReady = true;
					if (this.mBlocking)
					{
						this.mTlsStream = new TlsStream(this);
					}
				}
				if (this.mTlsSession != null)
				{
					if (this.mSessionParameters == null)
					{
						this.mSessionParameters = new SessionParameters.Builder().SetCipherSuite(this.mSecurityParameters.CipherSuite).SetCompressionAlgorithm(this.mSecurityParameters.CompressionAlgorithm).SetExtendedMasterSecret(this.mSecurityParameters.IsExtendedMasterSecret).SetMasterSecret(this.mSecurityParameters.MasterSecret).SetPeerCertificate(this.mPeerCertificate).SetPskIdentity(this.mSecurityParameters.PskIdentity).SetSrpIdentity(this.mSecurityParameters.SrpIdentity).SetServerExtensions(this.mServerExtensions).Build();
						this.mTlsSession = new TlsSessionImpl(this.mTlsSession.SessionID, this.mSessionParameters);
					}
					this.ContextAdmin.SetResumableSession(this.mTlsSession);
				}
				this.Peer.NotifyHandshakeComplete();
			}
			finally
			{
				this.CleanupHandshake();
			}
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x00118C0C File Offset: 0x00116E0C
		protected internal void ProcessRecord(byte protocol, byte[] buf, int off, int len)
		{
			switch (protocol)
			{
			case 20:
				this.ProcessChangeCipherSpec(buf, off, len);
				return;
			case 21:
				this.mAlertQueue.AddData(buf, off, len);
				this.ProcessAlertQueue();
				return;
			case 22:
			{
				if (this.mHandshakeQueue.Available > 0)
				{
					this.mHandshakeQueue.AddData(buf, off, len);
					this.ProcessHandshakeQueue(this.mHandshakeQueue);
					return;
				}
				ByteQueue byteQueue = new ByteQueue(buf, off, len);
				this.ProcessHandshakeQueue(byteQueue);
				int available = byteQueue.Available;
				if (available > 0)
				{
					this.mHandshakeQueue.AddData(buf, off + len - available, available);
					return;
				}
				return;
			}
			case 23:
				if (!this.mAppDataReady)
				{
					throw new TlsFatalAlert(10);
				}
				this.mApplicationDataQueue.AddData(buf, off, len);
				this.ProcessApplicationDataQueue();
				return;
			default:
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x00118CE4 File Offset: 0x00116EE4
		private void ProcessHandshakeQueue(ByteQueue queue)
		{
			while (queue.Available >= 4)
			{
				byte[] buf = new byte[4];
				queue.Read(buf, 0, 4, 0);
				byte b = TlsUtilities.ReadUint8(buf, 0);
				int num = TlsUtilities.ReadUint24(buf, 1);
				int num2 = 4 + num;
				if (queue.Available < num2)
				{
					break;
				}
				if (b != 0)
				{
					if (20 == b)
					{
						this.CheckReceivedChangeCipherSpec(true);
						TlsContext context = this.Context;
						if (this.mExpectedVerifyData == null && context.SecurityParameters.MasterSecret != null)
						{
							this.mExpectedVerifyData = this.CreateVerifyData(!context.IsServer);
						}
					}
					else
					{
						this.CheckReceivedChangeCipherSpec(this.mConnectionState == 16);
					}
					queue.CopyTo(this.mRecordStream.HandshakeHashUpdater, num2);
				}
				queue.RemoveData(4);
				MemoryStream buf2 = queue.ReadFrom(num);
				this.HandleHandshakeMessage(b, buf2);
			}
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x0000248C File Offset: 0x0000068C
		private void ProcessApplicationDataQueue()
		{
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x00118DB4 File Offset: 0x00116FB4
		private void ProcessAlertQueue()
		{
			while (this.mAlertQueue.Available >= 2)
			{
				byte[] array = this.mAlertQueue.RemoveData(2, 0);
				byte alertLevel = array[0];
				byte alertDescription = array[1];
				this.HandleAlertMessage(alertLevel, alertDescription);
			}
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x00118DF0 File Offset: 0x00116FF0
		private void ProcessChangeCipherSpec(byte[] buf, int off, int len)
		{
			for (int i = 0; i < len; i++)
			{
				if (TlsUtilities.ReadUint8(buf, off + i) != 1)
				{
					throw new TlsFatalAlert(50);
				}
				if (this.mReceivedChangeCipherSpec || this.mAlertQueue.Available > 0 || this.mHandshakeQueue.Available > 0)
				{
					throw new TlsFatalAlert(10);
				}
				this.mRecordStream.ReceivedReadCipherSpec();
				this.mReceivedChangeCipherSpec = true;
				this.HandleChangeCipherSpecMessage();
			}
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x00118E61 File Offset: 0x00117061
		protected internal virtual int ApplicationDataAvailable()
		{
			return this.mApplicationDataQueue.Available;
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x00118E70 File Offset: 0x00117070
		protected internal virtual int ReadApplicationData(byte[] buf, int offset, int len)
		{
			if (len < 1)
			{
				return 0;
			}
			while (this.mApplicationDataQueue.Available == 0)
			{
				if (this.mClosed)
				{
					if (this.mFailedWithError)
					{
						throw new IOException("Cannot read application data on failed TLS connection");
					}
					if (!this.mAppDataReady)
					{
						throw new InvalidOperationException("Cannot read application data until initial handshake completed.");
					}
					return 0;
				}
				else
				{
					this.SafeReadRecord();
				}
			}
			len = Math.Min(len, this.mApplicationDataQueue.Available);
			this.mApplicationDataQueue.RemoveData(buf, offset, len, 0);
			return len;
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x00118EF0 File Offset: 0x001170F0
		protected virtual void SafeCheckRecordHeader(byte[] recordHeader)
		{
			try
			{
				this.mRecordStream.CheckRecordHeader(recordHeader);
			}
			catch (TlsFatalAlert tlsFatalAlert)
			{
				this.HandleException(tlsFatalAlert.AlertDescription, "Failed to read record", tlsFatalAlert);
				throw tlsFatalAlert;
			}
			catch (IOException ex)
			{
				this.HandleException(80, "Failed to read record", ex);
				throw ex;
			}
			catch (Exception ex2)
			{
				this.HandleException(80, "Failed to read record", ex2);
				throw new TlsFatalAlert(80, ex2);
			}
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x00118F74 File Offset: 0x00117174
		protected virtual void SafeReadRecord()
		{
			try
			{
				if (this.mRecordStream.ReadRecord())
				{
					return;
				}
				if (!this.mAppDataReady)
				{
					throw new TlsFatalAlert(40);
				}
			}
			catch (TlsFatalAlertReceived tlsFatalAlertReceived)
			{
				throw tlsFatalAlertReceived;
			}
			catch (TlsFatalAlert tlsFatalAlert)
			{
				this.HandleException(tlsFatalAlert.AlertDescription, "Failed to read record", tlsFatalAlert);
				throw tlsFatalAlert;
			}
			catch (IOException ex)
			{
				this.HandleException(80, "Failed to read record", ex);
				throw ex;
			}
			catch (Exception ex2)
			{
				this.HandleException(80, "Failed to read record", ex2);
				throw new TlsFatalAlert(80, ex2);
			}
			this.HandleFailure();
			throw new TlsNoCloseNotifyException();
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x00119024 File Offset: 0x00117224
		protected virtual void SafeWriteRecord(byte type, byte[] buf, int offset, int len)
		{
			try
			{
				this.mRecordStream.WriteRecord(type, buf, offset, len);
			}
			catch (TlsFatalAlert tlsFatalAlert)
			{
				this.HandleException(tlsFatalAlert.AlertDescription, "Failed to write record", tlsFatalAlert);
				throw tlsFatalAlert;
			}
			catch (IOException ex)
			{
				this.HandleException(80, "Failed to write record", ex);
				throw ex;
			}
			catch (Exception ex2)
			{
				this.HandleException(80, "Failed to write record", ex2);
				throw new TlsFatalAlert(80, ex2);
			}
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x001190AC File Offset: 0x001172AC
		protected internal virtual void WriteData(byte[] buf, int offset, int len)
		{
			if (this.mClosed)
			{
				throw new IOException("Cannot write application data on closed/failed TLS connection");
			}
			while (len > 0)
			{
				if (this.mAppDataSplitEnabled)
				{
					switch (this.mAppDataSplitMode)
					{
					case 1:
						this.SafeWriteRecord(23, TlsUtilities.EmptyBytes, 0, 0);
						goto IL_7F;
					case 2:
						this.mAppDataSplitEnabled = false;
						this.SafeWriteRecord(23, TlsUtilities.EmptyBytes, 0, 0);
						goto IL_7F;
					}
					this.SafeWriteRecord(23, buf, offset, 1);
					offset++;
					len--;
				}
				IL_7F:
				if (len > 0)
				{
					int num = Math.Min(len, this.mRecordStream.GetPlaintextLimit());
					this.SafeWriteRecord(23, buf, offset, num);
					offset += num;
					len -= num;
				}
			}
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x0011916A File Offset: 0x0011736A
		protected virtual void SetAppDataSplitMode(int appDataSplitMode)
		{
			if (appDataSplitMode < 0 || appDataSplitMode > 2)
			{
				throw new ArgumentException("Illegal appDataSplitMode mode: " + appDataSplitMode, "appDataSplitMode");
			}
			this.mAppDataSplitMode = appDataSplitMode;
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x00119198 File Offset: 0x00117398
		protected virtual void WriteHandshakeMessage(byte[] buf, int off, int len)
		{
			if (len < 4)
			{
				throw new TlsFatalAlert(80);
			}
			if (TlsUtilities.ReadUint8(buf, off) != 0)
			{
				this.mRecordStream.HandshakeHashUpdater.Write(buf, off, len);
			}
			int num = 0;
			do
			{
				int num2 = Math.Min(len - num, this.mRecordStream.GetPlaintextLimit());
				this.SafeWriteRecord(22, buf, off + num, num2);
				num += num2;
			}
			while (num < len);
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06002CAB RID: 11435 RVA: 0x001191F8 File Offset: 0x001173F8
		public virtual Stream Stream
		{
			get
			{
				if (!this.mBlocking)
				{
					throw new InvalidOperationException("Cannot use Stream in non-blocking mode! Use OfferInput()/OfferOutput() instead.");
				}
				return this.mTlsStream;
			}
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x00119214 File Offset: 0x00117414
		public virtual void CloseInput()
		{
			if (this.mBlocking)
			{
				throw new InvalidOperationException("Cannot use CloseInput() in blocking mode!");
			}
			if (this.mClosed)
			{
				return;
			}
			if (this.mInputBuffers.Available > 0)
			{
				throw new EndOfStreamException();
			}
			if (!this.mAppDataReady)
			{
				throw new TlsFatalAlert(40);
			}
			throw new TlsNoCloseNotifyException();
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x0011926C File Offset: 0x0011746C
		public virtual void OfferInput(byte[] input)
		{
			if (this.mBlocking)
			{
				throw new InvalidOperationException("Cannot use OfferInput() in blocking mode! Use Stream instead.");
			}
			if (this.mClosed)
			{
				throw new IOException("Connection is closed, cannot accept any more input");
			}
			this.mInputBuffers.Write(input);
			while (this.mInputBuffers.Available >= 5)
			{
				byte[] array = new byte[5];
				this.mInputBuffers.Peek(array);
				int num = TlsUtilities.ReadUint16(array, 3) + 5;
				if (this.mInputBuffers.Available < num)
				{
					this.SafeCheckRecordHeader(array);
					return;
				}
				this.SafeReadRecord();
				if (this.mClosed)
				{
					if (this.mConnectionState != 16)
					{
						throw new TlsFatalAlert(80);
					}
					break;
				}
			}
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x00119313 File Offset: 0x00117513
		public virtual int GetAvailableInputBytes()
		{
			if (this.mBlocking)
			{
				throw new InvalidOperationException("Cannot use GetAvailableInputBytes() in blocking mode! Use ApplicationDataAvailable() instead.");
			}
			return this.ApplicationDataAvailable();
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x0011932E File Offset: 0x0011752E
		public virtual int ReadInput(byte[] buffer, int offset, int length)
		{
			if (this.mBlocking)
			{
				throw new InvalidOperationException("Cannot use ReadInput() in blocking mode! Use Stream instead.");
			}
			return this.ReadApplicationData(buffer, offset, Math.Min(length, this.ApplicationDataAvailable()));
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x00119357 File Offset: 0x00117557
		public virtual void OfferOutput(byte[] buffer, int offset, int length)
		{
			if (this.mBlocking)
			{
				throw new InvalidOperationException("Cannot use OfferOutput() in blocking mode! Use Stream instead.");
			}
			if (!this.mAppDataReady)
			{
				throw new IOException("Application data cannot be sent until the handshake is complete!");
			}
			this.WriteData(buffer, offset, length);
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x0011938A File Offset: 0x0011758A
		public virtual int GetAvailableOutputBytes()
		{
			if (this.mBlocking)
			{
				throw new InvalidOperationException("Cannot use GetAvailableOutputBytes() in blocking mode! Use Stream instead.");
			}
			return this.mOutputBuffer.Available;
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x001193AA File Offset: 0x001175AA
		public virtual int ReadOutput(byte[] buffer, int offset, int length)
		{
			if (this.mBlocking)
			{
				throw new InvalidOperationException("Cannot use ReadOutput() in blocking mode! Use Stream instead.");
			}
			return this.mOutputBuffer.Read(buffer, offset, length);
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x001193CD File Offset: 0x001175CD
		protected virtual void InvalidateSession()
		{
			if (this.mSessionParameters != null)
			{
				this.mSessionParameters.Clear();
				this.mSessionParameters = null;
			}
			if (this.mTlsSession != null)
			{
				this.mTlsSession.Invalidate();
				this.mTlsSession = null;
			}
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x00119404 File Offset: 0x00117604
		protected virtual void ProcessFinishedMessage(MemoryStream buf)
		{
			if (this.mExpectedVerifyData == null)
			{
				throw new TlsFatalAlert(80);
			}
			byte[] b = TlsUtilities.ReadFully(this.mExpectedVerifyData.Length, buf);
			TlsProtocol.AssertEmpty(buf);
			if (!Arrays.ConstantTimeAreEqual(this.mExpectedVerifyData, b))
			{
				throw new TlsFatalAlert(51);
			}
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x0011944C File Offset: 0x0011764C
		protected virtual void RaiseAlertFatal(byte alertDescription, string message, Exception cause)
		{
			this.Peer.NotifyAlertRaised(2, alertDescription, message, cause);
			byte[] plaintext = new byte[]
			{
				2,
				alertDescription
			};
			try
			{
				this.mRecordStream.WriteRecord(21, plaintext, 0, 2);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x0011949C File Offset: 0x0011769C
		protected virtual void RaiseAlertWarning(byte alertDescription, string message)
		{
			this.Peer.NotifyAlertRaised(1, alertDescription, message, null);
			byte[] buf = new byte[]
			{
				1,
				alertDescription
			};
			this.SafeWriteRecord(21, buf, 0, 2);
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x001194D4 File Offset: 0x001176D4
		protected virtual void SendCertificateMessage(Certificate certificate)
		{
			if (certificate == null)
			{
				certificate = Certificate.EmptyChain;
			}
			if (certificate.IsEmpty && !this.Context.IsServer)
			{
				ProtocolVersion serverVersion = this.Context.ServerVersion;
				if (serverVersion.IsSsl)
				{
					string message = serverVersion.ToString() + " client didn't provide credentials";
					this.RaiseAlertWarning(41, message);
					return;
				}
			}
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(11);
			certificate.Encode(handshakeMessage);
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x00119548 File Offset: 0x00117748
		protected virtual void SendChangeCipherSpecMessage()
		{
			byte[] array = new byte[]
			{
				1
			};
			this.SafeWriteRecord(20, array, 0, array.Length);
			this.mRecordStream.SentWriteCipherSpec();
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x00119578 File Offset: 0x00117778
		protected virtual void SendFinishedMessage()
		{
			byte[] array = this.CreateVerifyData(this.Context.IsServer);
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(20, array.Length);
			handshakeMessage.Write(array, 0, array.Length);
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x001195B2 File Offset: 0x001177B2
		protected virtual void SendSupplementalDataMessage(IList supplementalData)
		{
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(23);
			TlsProtocol.WriteSupplementalData(handshakeMessage, supplementalData);
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x001195C8 File Offset: 0x001177C8
		protected virtual byte[] CreateVerifyData(bool isServer)
		{
			TlsContext context = this.Context;
			string asciiLabel = isServer ? "server finished" : "client finished";
			byte[] sslSender = isServer ? TlsUtilities.SSL_SERVER : TlsUtilities.SSL_CLIENT;
			byte[] currentPrfHash = TlsProtocol.GetCurrentPrfHash(context, this.mRecordStream.HandshakeHash, sslSender);
			return TlsUtilities.CalculateVerifyData(context, asciiLabel, currentPrfHash);
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x00119615 File Offset: 0x00117815
		public virtual void Close()
		{
			this.HandleClose(true);
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x0011961E File Offset: 0x0011781E
		protected internal virtual void Flush()
		{
			this.mRecordStream.Flush();
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06002CBE RID: 11454 RVA: 0x0011962B File Offset: 0x0011782B
		public virtual bool IsClosed
		{
			get
			{
				return this.mClosed;
			}
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x00119638 File Offset: 0x00117838
		protected virtual short ProcessMaxFragmentLengthExtension(IDictionary clientExtensions, IDictionary serverExtensions, byte alertDescription)
		{
			short maxFragmentLengthExtension = TlsExtensionsUtilities.GetMaxFragmentLengthExtension(serverExtensions);
			if (maxFragmentLengthExtension >= 0 && (!MaxFragmentLength.IsValid((byte)maxFragmentLengthExtension) || (!this.mResumedSession && maxFragmentLengthExtension != TlsExtensionsUtilities.GetMaxFragmentLengthExtension(clientExtensions))))
			{
				throw new TlsFatalAlert(alertDescription);
			}
			return maxFragmentLengthExtension;
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x00119672 File Offset: 0x00117872
		protected virtual void RefuseRenegotiation()
		{
			if (TlsUtilities.IsSsl(this.Context))
			{
				throw new TlsFatalAlert(40);
			}
			this.RaiseAlertWarning(100, "Renegotiation not supported");
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x00119696 File Offset: 0x00117896
		protected internal static void AssertEmpty(MemoryStream buf)
		{
			if (buf.Position < buf.Length)
			{
				throw new TlsFatalAlert(50);
			}
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x001196B0 File Offset: 0x001178B0
		protected internal static byte[] CreateRandomBlock(bool useGmtUnixTime, IRandomGenerator randomGenerator)
		{
			byte[] array = new byte[32];
			randomGenerator.NextBytes(array);
			if (useGmtUnixTime)
			{
				TlsUtilities.WriteGmtUnixTime(array, 0);
			}
			return array;
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x001196D7 File Offset: 0x001178D7
		protected internal static byte[] CreateRenegotiationInfo(byte[] renegotiated_connection)
		{
			return TlsUtilities.EncodeOpaque8(renegotiated_connection);
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x001196E0 File Offset: 0x001178E0
		protected internal static void EstablishMasterSecret(TlsContext context, TlsKeyExchange keyExchange)
		{
			byte[] array = keyExchange.GeneratePremasterSecret();
			try
			{
				context.SecurityParameters.masterSecret = TlsUtilities.CalculateMasterSecret(context, array);
			}
			finally
			{
				if (array != null)
				{
					Arrays.Fill(array, 0);
				}
			}
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x00119724 File Offset: 0x00117924
		protected internal static byte[] GetCurrentPrfHash(TlsContext context, TlsHandshakeHash handshakeHash, byte[] sslSender)
		{
			IDigest digest = handshakeHash.ForkPrfHash();
			if (sslSender != null && TlsUtilities.IsSsl(context))
			{
				digest.BlockUpdate(sslSender, 0, sslSender.Length);
			}
			return DigestUtilities.DoFinal(digest);
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x00119754 File Offset: 0x00117954
		protected internal static IDictionary ReadExtensions(MemoryStream input)
		{
			if (input.Position >= input.Length)
			{
				return null;
			}
			byte[] buffer = TlsUtilities.ReadOpaque16(input);
			TlsProtocol.AssertEmpty(input);
			MemoryStream memoryStream = new MemoryStream(buffer, false);
			IDictionary dictionary = Platform.CreateHashtable();
			while (memoryStream.Position < memoryStream.Length)
			{
				int num = TlsUtilities.ReadUint16(memoryStream);
				byte[] value = TlsUtilities.ReadOpaque16(memoryStream);
				if (dictionary.Contains(num))
				{
					throw new TlsFatalAlert(47);
				}
				dictionary.Add(num, value);
			}
			return dictionary;
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x001197CC File Offset: 0x001179CC
		protected internal static IList ReadSupplementalDataMessage(MemoryStream input)
		{
			byte[] buffer = TlsUtilities.ReadOpaque24(input);
			TlsProtocol.AssertEmpty(input);
			MemoryStream memoryStream = new MemoryStream(buffer, false);
			IList list = Platform.CreateArrayList();
			while (memoryStream.Position < memoryStream.Length)
			{
				int dataType = TlsUtilities.ReadUint16(memoryStream);
				byte[] data = TlsUtilities.ReadOpaque16(memoryStream);
				list.Add(new SupplementalDataEntry(dataType, data));
			}
			return list;
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x0011981F File Offset: 0x00117A1F
		protected internal static void WriteExtensions(Stream output, IDictionary extensions)
		{
			MemoryStream memoryStream = new MemoryStream();
			TlsProtocol.WriteSelectedExtensions(memoryStream, extensions, true);
			TlsProtocol.WriteSelectedExtensions(memoryStream, extensions, false);
			TlsUtilities.WriteOpaque16(memoryStream.ToArray(), output);
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x00119844 File Offset: 0x00117A44
		protected internal static void WriteSelectedExtensions(Stream output, IDictionary extensions, bool selectEmpty)
		{
			foreach (object obj in extensions.Keys)
			{
				int num = (int)obj;
				byte[] array = (byte[])extensions[num];
				if (selectEmpty == (array.Length == 0))
				{
					TlsUtilities.CheckUint16(num);
					TlsUtilities.WriteUint16(num, output);
					TlsUtilities.WriteOpaque16(array, output);
				}
			}
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x001198C4 File Offset: 0x00117AC4
		protected internal static void WriteSupplementalData(Stream output, IList supplementalData)
		{
			MemoryStream memoryStream = new MemoryStream();
			foreach (object obj in supplementalData)
			{
				SupplementalDataEntry supplementalDataEntry = (SupplementalDataEntry)obj;
				int dataType = supplementalDataEntry.DataType;
				TlsUtilities.CheckUint16(dataType);
				TlsUtilities.WriteUint16(dataType, memoryStream);
				TlsUtilities.WriteOpaque16(supplementalDataEntry.Data, memoryStream);
			}
			TlsUtilities.WriteOpaque24(memoryStream.ToArray(), output);
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x00119940 File Offset: 0x00117B40
		protected internal static int GetPrfAlgorithm(TlsContext context, int ciphersuite)
		{
			bool flag = TlsUtilities.IsTlsV12(context);
			if (ciphersuite <= 197)
			{
				if (ciphersuite - 59 > 5 && ciphersuite - 103 > 6)
				{
					switch (ciphersuite)
					{
					case 156:
					case 158:
					case 160:
					case 162:
					case 164:
					case 166:
					case 168:
					case 170:
					case 172:
					case 186:
					case 187:
					case 188:
					case 189:
					case 190:
					case 191:
					case 192:
					case 193:
					case 194:
					case 195:
					case 196:
					case 197:
						break;
					case 157:
					case 159:
					case 161:
					case 163:
					case 165:
					case 167:
					case 169:
					case 171:
					case 173:
						goto IL_357;
					case 174:
					case 176:
					case 178:
					case 180:
					case 182:
					case 184:
						goto IL_36B;
					case 175:
					case 177:
					case 179:
					case 181:
					case 183:
					case 185:
						goto IL_364;
					default:
						goto IL_36B;
					}
				}
			}
			else if (ciphersuite <= 52398)
			{
				switch (ciphersuite)
				{
				case 49187:
				case 49189:
				case 49191:
				case 49193:
				case 49195:
				case 49197:
				case 49199:
				case 49201:
				case 49266:
				case 49268:
				case 49270:
				case 49272:
				case 49274:
				case 49276:
				case 49278:
				case 49280:
				case 49282:
				case 49284:
				case 49286:
				case 49288:
				case 49290:
				case 49292:
				case 49294:
				case 49296:
				case 49298:
				case 49308:
				case 49309:
				case 49310:
				case 49311:
				case 49312:
				case 49313:
				case 49314:
				case 49315:
				case 49316:
				case 49317:
				case 49318:
				case 49319:
				case 49320:
				case 49321:
				case 49322:
				case 49323:
				case 49324:
				case 49325:
				case 49326:
				case 49327:
					break;
				case 49188:
				case 49190:
				case 49192:
				case 49194:
				case 49196:
				case 49198:
				case 49200:
				case 49202:
				case 49267:
				case 49269:
				case 49271:
				case 49273:
				case 49275:
				case 49277:
				case 49279:
				case 49281:
				case 49283:
				case 49285:
				case 49287:
				case 49289:
				case 49291:
				case 49293:
				case 49295:
				case 49297:
				case 49299:
					goto IL_357;
				case 49203:
				case 49204:
				case 49205:
				case 49206:
				case 49207:
				case 49209:
				case 49210:
				case 49212:
				case 49213:
				case 49214:
				case 49215:
				case 49216:
				case 49217:
				case 49218:
				case 49219:
				case 49220:
				case 49221:
				case 49222:
				case 49223:
				case 49224:
				case 49225:
				case 49226:
				case 49227:
				case 49228:
				case 49229:
				case 49230:
				case 49231:
				case 49232:
				case 49233:
				case 49234:
				case 49235:
				case 49236:
				case 49237:
				case 49238:
				case 49239:
				case 49240:
				case 49241:
				case 49242:
				case 49243:
				case 49244:
				case 49245:
				case 49246:
				case 49247:
				case 49248:
				case 49249:
				case 49250:
				case 49251:
				case 49252:
				case 49253:
				case 49254:
				case 49255:
				case 49256:
				case 49257:
				case 49258:
				case 49259:
				case 49260:
				case 49261:
				case 49262:
				case 49263:
				case 49264:
				case 49265:
				case 49300:
				case 49302:
				case 49304:
				case 49306:
					goto IL_36B;
				case 49208:
				case 49211:
				case 49301:
				case 49303:
				case 49305:
				case 49307:
					goto IL_364;
				default:
					if (ciphersuite - 52392 > 6)
					{
						goto IL_36B;
					}
					break;
				}
			}
			else if (ciphersuite - 65280 > 5 && ciphersuite - 65296 > 5)
			{
				goto IL_36B;
			}
			if (flag)
			{
				return 1;
			}
			throw new TlsFatalAlert(47);
			IL_357:
			if (flag)
			{
				return 2;
			}
			throw new TlsFatalAlert(47);
			IL_364:
			if (flag)
			{
				return 2;
			}
			return 0;
			IL_36B:
			if (flag)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x04001E4E RID: 7758
		protected const short CS_START = 0;

		// Token: 0x04001E4F RID: 7759
		protected const short CS_CLIENT_HELLO = 1;

		// Token: 0x04001E50 RID: 7760
		protected const short CS_SERVER_HELLO = 2;

		// Token: 0x04001E51 RID: 7761
		protected const short CS_SERVER_SUPPLEMENTAL_DATA = 3;

		// Token: 0x04001E52 RID: 7762
		protected const short CS_SERVER_CERTIFICATE = 4;

		// Token: 0x04001E53 RID: 7763
		protected const short CS_CERTIFICATE_STATUS = 5;

		// Token: 0x04001E54 RID: 7764
		protected const short CS_SERVER_KEY_EXCHANGE = 6;

		// Token: 0x04001E55 RID: 7765
		protected const short CS_CERTIFICATE_REQUEST = 7;

		// Token: 0x04001E56 RID: 7766
		protected const short CS_SERVER_HELLO_DONE = 8;

		// Token: 0x04001E57 RID: 7767
		protected const short CS_CLIENT_SUPPLEMENTAL_DATA = 9;

		// Token: 0x04001E58 RID: 7768
		protected const short CS_CLIENT_CERTIFICATE = 10;

		// Token: 0x04001E59 RID: 7769
		protected const short CS_CLIENT_KEY_EXCHANGE = 11;

		// Token: 0x04001E5A RID: 7770
		protected const short CS_CERTIFICATE_VERIFY = 12;

		// Token: 0x04001E5B RID: 7771
		protected const short CS_CLIENT_FINISHED = 13;

		// Token: 0x04001E5C RID: 7772
		protected const short CS_SERVER_SESSION_TICKET = 14;

		// Token: 0x04001E5D RID: 7773
		protected const short CS_SERVER_FINISHED = 15;

		// Token: 0x04001E5E RID: 7774
		protected const short CS_END = 16;

		// Token: 0x04001E5F RID: 7775
		protected const short ADS_MODE_1_Nsub1 = 0;

		// Token: 0x04001E60 RID: 7776
		protected const short ADS_MODE_0_N = 1;

		// Token: 0x04001E61 RID: 7777
		protected const short ADS_MODE_0_N_FIRSTONLY = 2;

		// Token: 0x04001E62 RID: 7778
		private ByteQueue mApplicationDataQueue;

		// Token: 0x04001E63 RID: 7779
		private ByteQueue mAlertQueue;

		// Token: 0x04001E64 RID: 7780
		private ByteQueue mHandshakeQueue;

		// Token: 0x04001E65 RID: 7781
		internal RecordStream mRecordStream;

		// Token: 0x04001E66 RID: 7782
		protected SecureRandom mSecureRandom;

		// Token: 0x04001E67 RID: 7783
		private TlsStream mTlsStream;

		// Token: 0x04001E68 RID: 7784
		private volatile bool mClosed;

		// Token: 0x04001E69 RID: 7785
		private volatile bool mFailedWithError;

		// Token: 0x04001E6A RID: 7786
		private volatile bool mAppDataReady;

		// Token: 0x04001E6B RID: 7787
		private volatile bool mAppDataSplitEnabled;

		// Token: 0x04001E6C RID: 7788
		private volatile int mAppDataSplitMode;

		// Token: 0x04001E6D RID: 7789
		private byte[] mExpectedVerifyData;

		// Token: 0x04001E6E RID: 7790
		protected TlsSession mTlsSession;

		// Token: 0x04001E6F RID: 7791
		protected SessionParameters mSessionParameters;

		// Token: 0x04001E70 RID: 7792
		protected SecurityParameters mSecurityParameters;

		// Token: 0x04001E71 RID: 7793
		protected Certificate mPeerCertificate;

		// Token: 0x04001E72 RID: 7794
		protected int[] mOfferedCipherSuites;

		// Token: 0x04001E73 RID: 7795
		protected byte[] mOfferedCompressionMethods;

		// Token: 0x04001E74 RID: 7796
		protected IDictionary mClientExtensions;

		// Token: 0x04001E75 RID: 7797
		protected IDictionary mServerExtensions;

		// Token: 0x04001E76 RID: 7798
		protected short mConnectionState;

		// Token: 0x04001E77 RID: 7799
		protected bool mResumedSession;

		// Token: 0x04001E78 RID: 7800
		protected bool mReceivedChangeCipherSpec;

		// Token: 0x04001E79 RID: 7801
		protected bool mSecureRenegotiation;

		// Token: 0x04001E7A RID: 7802
		protected bool mAllowCertificateStatus;

		// Token: 0x04001E7B RID: 7803
		protected bool mExpectSessionTicket;

		// Token: 0x04001E7C RID: 7804
		protected bool mBlocking;

		// Token: 0x04001E7D RID: 7805
		protected ByteQueueStream mInputBuffers;

		// Token: 0x04001E7E RID: 7806
		protected ByteQueueStream mOutputBuffer;

		// Token: 0x0200094A RID: 2378
		internal class HandshakeMessage : MemoryStream
		{
			// Token: 0x06004EEF RID: 20207 RVA: 0x001B35EB File Offset: 0x001B17EB
			internal HandshakeMessage(byte handshakeType) : this(handshakeType, 60)
			{
			}

			// Token: 0x06004EF0 RID: 20208 RVA: 0x001B35F6 File Offset: 0x001B17F6
			internal HandshakeMessage(byte handshakeType, int length) : base(length + 4)
			{
				TlsUtilities.WriteUint8(handshakeType, this);
				TlsUtilities.WriteUint24(0, this);
			}

			// Token: 0x06004EF1 RID: 20209 RVA: 0x000B7E62 File Offset: 0x000B6062
			internal void Write(byte[] data)
			{
				this.Write(data, 0, data.Length);
			}

			// Token: 0x06004EF2 RID: 20210 RVA: 0x001B3610 File Offset: 0x001B1810
			internal void WriteToRecordStream(TlsProtocol protocol)
			{
				long num = this.Length - 4L;
				TlsUtilities.CheckUint24(num);
				this.Position = 1L;
				TlsUtilities.WriteUint24((int)num, this);
				byte[] buffer = this.GetBuffer();
				int len = (int)this.Length;
				protocol.WriteHandshakeMessage(buffer, 0, len);
				Platform.Dispose(this);
			}
		}
	}
}
