using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000481 RID: 1153
	public class TlsServerProtocol : TlsProtocol
	{
		// Token: 0x06002D0C RID: 11532 RVA: 0x0011A68C File Offset: 0x0011888C
		public TlsServerProtocol(Stream stream, SecureRandom secureRandom) : base(stream, secureRandom)
		{
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x0011A69D File Offset: 0x0011889D
		public TlsServerProtocol(Stream input, Stream output, SecureRandom secureRandom) : base(input, output, secureRandom)
		{
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x0011A6AF File Offset: 0x001188AF
		public TlsServerProtocol(SecureRandom secureRandom) : base(secureRandom)
		{
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x0011A6C0 File Offset: 0x001188C0
		public virtual void Accept(TlsServer tlsServer)
		{
			if (tlsServer == null)
			{
				throw new ArgumentNullException("tlsServer");
			}
			if (this.mTlsServer != null)
			{
				throw new InvalidOperationException("'Accept' can only be called once");
			}
			this.mTlsServer = tlsServer;
			this.mSecurityParameters = new SecurityParameters();
			this.mSecurityParameters.entity = 0;
			this.mTlsServerContext = new TlsServerContextImpl(this.mSecureRandom, this.mSecurityParameters);
			this.mSecurityParameters.serverRandom = TlsProtocol.CreateRandomBlock(tlsServer.ShouldUseGmtUnixTime(), this.mTlsServerContext.NonceRandomGenerator);
			this.mTlsServer.Init(this.mTlsServerContext);
			this.mRecordStream.Init(this.mTlsServerContext);
			this.mRecordStream.SetRestrictReadVersion(false);
			this.BlockForHandshake();
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x0011A778 File Offset: 0x00118978
		protected override void CleanupHandshake()
		{
			base.CleanupHandshake();
			this.mKeyExchange = null;
			this.mServerCredentials = null;
			this.mCertificateRequest = null;
			this.mPrepareFinishHash = null;
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06002D11 RID: 11537 RVA: 0x0011A79C File Offset: 0x0011899C
		protected override TlsContext Context
		{
			get
			{
				return this.mTlsServerContext;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06002D12 RID: 11538 RVA: 0x0011A79C File Offset: 0x0011899C
		internal override AbstractTlsContext ContextAdmin
		{
			get
			{
				return this.mTlsServerContext;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06002D13 RID: 11539 RVA: 0x0011A7A4 File Offset: 0x001189A4
		protected override TlsPeer Peer
		{
			get
			{
				return this.mTlsServer;
			}
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x0011A7AC File Offset: 0x001189AC
		protected override void HandleHandshakeMessage(byte type, MemoryStream buf)
		{
			switch (type)
			{
			case 1:
			{
				short mConnectionState = this.mConnectionState;
				if (mConnectionState == 0)
				{
					this.ReceiveClientHelloMessage(buf);
					this.mConnectionState = 1;
					this.SendServerHelloMessage();
					this.mConnectionState = 2;
					this.mRecordStream.NotifyHelloComplete();
					IList serverSupplementalData = this.mTlsServer.GetServerSupplementalData();
					if (serverSupplementalData != null)
					{
						this.SendSupplementalDataMessage(serverSupplementalData);
					}
					this.mConnectionState = 3;
					this.mKeyExchange = this.mTlsServer.GetKeyExchange();
					this.mKeyExchange.Init(this.Context);
					this.mServerCredentials = this.mTlsServer.GetCredentials();
					Certificate certificate = null;
					if (this.mServerCredentials == null)
					{
						this.mKeyExchange.SkipServerCredentials();
					}
					else
					{
						this.mKeyExchange.ProcessServerCredentials(this.mServerCredentials);
						certificate = this.mServerCredentials.Certificate;
						this.SendCertificateMessage(certificate);
					}
					this.mConnectionState = 4;
					if (certificate == null || certificate.IsEmpty)
					{
						this.mAllowCertificateStatus = false;
					}
					if (this.mAllowCertificateStatus)
					{
						CertificateStatus certificateStatus = this.mTlsServer.GetCertificateStatus();
						if (certificateStatus != null)
						{
							this.SendCertificateStatusMessage(certificateStatus);
						}
					}
					this.mConnectionState = 5;
					byte[] array = this.mKeyExchange.GenerateServerKeyExchange();
					if (array != null)
					{
						this.SendServerKeyExchangeMessage(array);
					}
					this.mConnectionState = 6;
					if (this.mServerCredentials != null)
					{
						this.mCertificateRequest = this.mTlsServer.GetCertificateRequest();
						if (this.mCertificateRequest != null)
						{
							if (TlsUtilities.IsTlsV12(this.Context) != (this.mCertificateRequest.SupportedSignatureAlgorithms != null))
							{
								throw new TlsFatalAlert(80);
							}
							this.mKeyExchange.ValidateCertificateRequest(this.mCertificateRequest);
							this.SendCertificateRequestMessage(this.mCertificateRequest);
							TlsUtilities.TrackHashAlgorithms(this.mRecordStream.HandshakeHash, this.mCertificateRequest.SupportedSignatureAlgorithms);
						}
					}
					this.mConnectionState = 7;
					this.SendServerHelloDoneMessage();
					this.mConnectionState = 8;
					this.mRecordStream.HandshakeHash.SealHashAlgorithms();
					return;
				}
				if (mConnectionState != 16)
				{
					throw new TlsFatalAlert(10);
				}
				this.RefuseRenegotiation();
				return;
			}
			case 11:
			{
				short mConnectionState = this.mConnectionState;
				if (mConnectionState - 8 > 1)
				{
					throw new TlsFatalAlert(10);
				}
				if (this.mConnectionState < 9)
				{
					this.mTlsServer.ProcessClientSupplementalData(null);
				}
				if (this.mCertificateRequest == null)
				{
					throw new TlsFatalAlert(10);
				}
				this.ReceiveCertificateMessage(buf);
				this.mConnectionState = 10;
				return;
			}
			case 15:
			{
				short mConnectionState = this.mConnectionState;
				if (mConnectionState != 11)
				{
					throw new TlsFatalAlert(10);
				}
				if (!this.ExpectCertificateVerifyMessage())
				{
					throw new TlsFatalAlert(10);
				}
				this.ReceiveCertificateVerifyMessage(buf);
				this.mConnectionState = 12;
				return;
			}
			case 16:
			{
				short mConnectionState = this.mConnectionState;
				if (mConnectionState - 8 <= 2)
				{
					if (this.mConnectionState < 9)
					{
						this.mTlsServer.ProcessClientSupplementalData(null);
					}
					if (this.mConnectionState < 10)
					{
						if (this.mCertificateRequest == null)
						{
							this.mKeyExchange.SkipClientCredentials();
						}
						else
						{
							if (TlsUtilities.IsTlsV12(this.Context))
							{
								throw new TlsFatalAlert(10);
							}
							if (TlsUtilities.IsSsl(this.Context))
							{
								if (this.mPeerCertificate == null)
								{
									throw new TlsFatalAlert(10);
								}
							}
							else
							{
								this.NotifyClientCertificate(Certificate.EmptyChain);
							}
						}
					}
					this.ReceiveClientKeyExchangeMessage(buf);
					this.mConnectionState = 11;
					return;
				}
				throw new TlsFatalAlert(10);
			}
			case 20:
			{
				short mConnectionState = this.mConnectionState;
				if (mConnectionState - 11 > 1)
				{
					throw new TlsFatalAlert(10);
				}
				if (this.mConnectionState < 12 && this.ExpectCertificateVerifyMessage())
				{
					throw new TlsFatalAlert(10);
				}
				this.ProcessFinishedMessage(buf);
				this.mConnectionState = 13;
				if (this.mExpectSessionTicket)
				{
					this.SendNewSessionTicketMessage(this.mTlsServer.GetNewSessionTicket());
				}
				this.mConnectionState = 14;
				this.SendChangeCipherSpecMessage();
				this.SendFinishedMessage();
				this.mConnectionState = 15;
				this.CompleteHandshake();
				return;
			}
			case 23:
			{
				short mConnectionState = this.mConnectionState;
				if (mConnectionState == 8)
				{
					this.mTlsServer.ProcessClientSupplementalData(TlsProtocol.ReadSupplementalDataMessage(buf));
					this.mConnectionState = 9;
					return;
				}
				throw new TlsFatalAlert(10);
			}
			}
			throw new TlsFatalAlert(10);
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x0011ABC4 File Offset: 0x00118DC4
		protected override void HandleAlertWarningMessage(byte alertDescription)
		{
			base.HandleAlertWarningMessage(alertDescription);
			if (alertDescription == 41)
			{
				if (TlsUtilities.IsSsl(this.Context) && this.mCertificateRequest != null)
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState - 8 <= 1)
					{
						if (this.mConnectionState < 9)
						{
							this.mTlsServer.ProcessClientSupplementalData(null);
						}
						this.NotifyClientCertificate(Certificate.EmptyChain);
						this.mConnectionState = 10;
						return;
					}
				}
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x0011AC34 File Offset: 0x00118E34
		protected virtual void NotifyClientCertificate(Certificate clientCertificate)
		{
			if (this.mCertificateRequest == null)
			{
				throw new InvalidOperationException();
			}
			if (this.mPeerCertificate != null)
			{
				throw new TlsFatalAlert(10);
			}
			this.mPeerCertificate = clientCertificate;
			if (clientCertificate.IsEmpty)
			{
				this.mKeyExchange.SkipClientCredentials();
			}
			else
			{
				this.mClientCertificateType = TlsUtilities.GetClientCertificateType(clientCertificate, this.mServerCredentials.Certificate);
				this.mKeyExchange.ProcessClientCertificate(clientCertificate);
			}
			this.mTlsServer.NotifyClientCertificate(clientCertificate);
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x0011ACAC File Offset: 0x00118EAC
		protected virtual void ReceiveCertificateMessage(MemoryStream buf)
		{
			Certificate clientCertificate = Certificate.Parse(buf);
			TlsProtocol.AssertEmpty(buf);
			this.NotifyClientCertificate(clientCertificate);
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x0011ACD0 File Offset: 0x00118ED0
		protected virtual void ReceiveCertificateVerifyMessage(MemoryStream buf)
		{
			if (this.mCertificateRequest == null)
			{
				throw new InvalidOperationException();
			}
			DigitallySigned digitallySigned = DigitallySigned.Parse(this.Context, buf);
			TlsProtocol.AssertEmpty(buf);
			try
			{
				SignatureAndHashAlgorithm algorithm = digitallySigned.Algorithm;
				byte[] hash;
				if (TlsUtilities.IsTlsV12(this.Context))
				{
					TlsUtilities.VerifySupportedSignatureAlgorithm(this.mCertificateRequest.SupportedSignatureAlgorithms, algorithm);
					hash = this.mPrepareFinishHash.GetFinalHash(algorithm.Hash);
				}
				else
				{
					hash = this.mSecurityParameters.SessionHash;
				}
				AsymmetricKeyParameter publicKey = PublicKeyFactory.CreateKey(this.mPeerCertificate.GetCertificateAt(0).SubjectPublicKeyInfo);
				TlsSigner tlsSigner = TlsUtilities.CreateTlsSigner((byte)this.mClientCertificateType);
				tlsSigner.Init(this.Context);
				if (!tlsSigner.VerifyRawSignature(algorithm, digitallySigned.Signature, publicKey, hash))
				{
					throw new TlsFatalAlert(51);
				}
			}
			catch (TlsFatalAlert tlsFatalAlert)
			{
				throw tlsFatalAlert;
			}
			catch (Exception alertCause)
			{
				throw new TlsFatalAlert(51, alertCause);
			}
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x0011ADB8 File Offset: 0x00118FB8
		protected virtual void ReceiveClientHelloMessage(MemoryStream buf)
		{
			ProtocolVersion protocolVersion = TlsUtilities.ReadVersion(buf);
			this.mRecordStream.SetWriteVersion(protocolVersion);
			if (protocolVersion.IsDtls)
			{
				throw new TlsFatalAlert(47);
			}
			byte[] clientRandom = TlsUtilities.ReadFully(32, buf);
			if (TlsUtilities.ReadOpaque8(buf).Length > 32)
			{
				throw new TlsFatalAlert(47);
			}
			int num = TlsUtilities.ReadUint16(buf);
			if (num < 2 || (num & 1) != 0)
			{
				throw new TlsFatalAlert(50);
			}
			this.mOfferedCipherSuites = TlsUtilities.ReadUint16Array(num / 2, buf);
			int num2 = (int)TlsUtilities.ReadUint8(buf);
			if (num2 < 1)
			{
				throw new TlsFatalAlert(47);
			}
			this.mOfferedCompressionMethods = TlsUtilities.ReadUint8Array(num2, buf);
			this.mClientExtensions = TlsProtocol.ReadExtensions(buf);
			this.mSecurityParameters.extendedMasterSecret = TlsExtensionsUtilities.HasExtendedMasterSecretExtension(this.mClientExtensions);
			if (!this.mSecurityParameters.IsExtendedMasterSecret && this.mTlsServer.RequiresExtendedMasterSecret())
			{
				throw new TlsFatalAlert(40);
			}
			this.ContextAdmin.SetClientVersion(protocolVersion);
			this.mTlsServer.NotifyClientVersion(protocolVersion);
			this.mTlsServer.NotifyFallback(Arrays.Contains(this.mOfferedCipherSuites, 22016));
			this.mSecurityParameters.clientRandom = clientRandom;
			this.mTlsServer.NotifyOfferedCipherSuites(this.mOfferedCipherSuites);
			this.mTlsServer.NotifyOfferedCompressionMethods(this.mOfferedCompressionMethods);
			if (Arrays.Contains(this.mOfferedCipherSuites, 255))
			{
				this.mSecureRenegotiation = true;
			}
			byte[] extensionData = TlsUtilities.GetExtensionData(this.mClientExtensions, 65281);
			if (extensionData != null)
			{
				this.mSecureRenegotiation = true;
				if (!Arrays.ConstantTimeAreEqual(extensionData, TlsProtocol.CreateRenegotiationInfo(TlsUtilities.EmptyBytes)))
				{
					throw new TlsFatalAlert(40);
				}
			}
			this.mTlsServer.NotifySecureRenegotiation(this.mSecureRenegotiation);
			if (this.mClientExtensions != null)
			{
				TlsExtensionsUtilities.GetPaddingExtension(this.mClientExtensions);
				this.mTlsServer.ProcessClientExtensions(this.mClientExtensions);
			}
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x0011AF78 File Offset: 0x00119178
		protected virtual void ReceiveClientKeyExchangeMessage(MemoryStream buf)
		{
			this.mKeyExchange.ProcessClientKeyExchange(buf);
			TlsProtocol.AssertEmpty(buf);
			if (TlsUtilities.IsSsl(this.Context))
			{
				TlsProtocol.EstablishMasterSecret(this.Context, this.mKeyExchange);
			}
			this.mPrepareFinishHash = this.mRecordStream.PrepareToFinish();
			this.mSecurityParameters.sessionHash = TlsProtocol.GetCurrentPrfHash(this.Context, this.mPrepareFinishHash, null);
			if (!TlsUtilities.IsSsl(this.Context))
			{
				TlsProtocol.EstablishMasterSecret(this.Context, this.mKeyExchange);
			}
			this.mRecordStream.SetPendingConnectionState(this.Peer.GetCompression(), this.Peer.GetCipher());
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x0011B024 File Offset: 0x00119224
		protected virtual void SendCertificateRequestMessage(CertificateRequest certificateRequest)
		{
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(13);
			certificateRequest.Encode(handshakeMessage);
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x0011B048 File Offset: 0x00119248
		protected virtual void SendCertificateStatusMessage(CertificateStatus certificateStatus)
		{
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(22);
			certificateStatus.Encode(handshakeMessage);
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x0011B06C File Offset: 0x0011926C
		protected virtual void SendNewSessionTicketMessage(NewSessionTicket newSessionTicket)
		{
			if (newSessionTicket == null)
			{
				throw new TlsFatalAlert(80);
			}
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(4);
			newSessionTicket.Encode(handshakeMessage);
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x0011B09C File Offset: 0x0011929C
		protected virtual void SendServerHelloMessage()
		{
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(2);
			ProtocolVersion serverVersion = this.mTlsServer.GetServerVersion();
			if (!serverVersion.IsEqualOrEarlierVersionOf(this.Context.ClientVersion))
			{
				throw new TlsFatalAlert(80);
			}
			this.mRecordStream.ReadVersion = serverVersion;
			this.mRecordStream.SetWriteVersion(serverVersion);
			this.mRecordStream.SetRestrictReadVersion(true);
			this.ContextAdmin.SetServerVersion(serverVersion);
			TlsUtilities.WriteVersion(serverVersion, handshakeMessage);
			handshakeMessage.Write(this.mSecurityParameters.serverRandom);
			TlsUtilities.WriteOpaque8(TlsUtilities.EmptyBytes, handshakeMessage);
			int selectedCipherSuite = this.mTlsServer.GetSelectedCipherSuite();
			if (!Arrays.Contains(this.mOfferedCipherSuites, selectedCipherSuite) || selectedCipherSuite == 0 || CipherSuite.IsScsv(selectedCipherSuite) || !TlsUtilities.IsValidCipherSuiteForVersion(selectedCipherSuite, this.Context.ServerVersion))
			{
				throw new TlsFatalAlert(80);
			}
			this.mSecurityParameters.cipherSuite = selectedCipherSuite;
			byte selectedCompressionMethod = this.mTlsServer.GetSelectedCompressionMethod();
			if (!Arrays.Contains(this.mOfferedCompressionMethods, selectedCompressionMethod))
			{
				throw new TlsFatalAlert(80);
			}
			this.mSecurityParameters.compressionAlgorithm = selectedCompressionMethod;
			TlsUtilities.WriteUint16(selectedCipherSuite, handshakeMessage);
			TlsUtilities.WriteUint8(selectedCompressionMethod, handshakeMessage);
			this.mServerExtensions = TlsExtensionsUtilities.EnsureExtensionsInitialised(this.mTlsServer.GetServerExtensions());
			if (this.mSecureRenegotiation)
			{
				byte[] extensionData = TlsUtilities.GetExtensionData(this.mServerExtensions, 65281);
				if (extensionData == null)
				{
					this.mServerExtensions[65281] = TlsProtocol.CreateRenegotiationInfo(TlsUtilities.EmptyBytes);
				}
			}
			if (TlsUtilities.IsSsl(this.mTlsServerContext))
			{
				this.mSecurityParameters.extendedMasterSecret = false;
			}
			else if (this.mSecurityParameters.IsExtendedMasterSecret)
			{
				TlsExtensionsUtilities.AddExtendedMasterSecretExtension(this.mServerExtensions);
			}
			if (this.mServerExtensions.Count > 0)
			{
				this.mSecurityParameters.encryptThenMac = TlsExtensionsUtilities.HasEncryptThenMacExtension(this.mServerExtensions);
				this.mSecurityParameters.maxFragmentLength = this.ProcessMaxFragmentLengthExtension(this.mClientExtensions, this.mServerExtensions, 80);
				this.mSecurityParameters.truncatedHMac = TlsExtensionsUtilities.HasTruncatedHMacExtension(this.mServerExtensions);
				this.mAllowCertificateStatus = (!this.mResumedSession && TlsUtilities.HasExpectedEmptyExtensionData(this.mServerExtensions, 5, 80));
				this.mExpectSessionTicket = (!this.mResumedSession && TlsUtilities.HasExpectedEmptyExtensionData(this.mServerExtensions, 35, 80));
				TlsProtocol.WriteExtensions(handshakeMessage, this.mServerExtensions);
			}
			this.mSecurityParameters.prfAlgorithm = TlsProtocol.GetPrfAlgorithm(this.Context, this.mSecurityParameters.CipherSuite);
			this.mSecurityParameters.verifyDataLength = 12;
			this.ApplyMaxFragmentLengthExtension();
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x0011B320 File Offset: 0x00119520
		protected virtual void SendServerHelloDoneMessage()
		{
			byte[] array = new byte[4];
			TlsUtilities.WriteUint8(14, array, 0);
			TlsUtilities.WriteUint24(0, array, 1);
			this.WriteHandshakeMessage(array, 0, array.Length);
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x0011B350 File Offset: 0x00119550
		protected virtual void SendServerKeyExchangeMessage(byte[] serverKeyExchange)
		{
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(12, serverKeyExchange.Length);
			handshakeMessage.Write(serverKeyExchange);
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x0011B369 File Offset: 0x00119569
		protected virtual bool ExpectCertificateVerifyMessage()
		{
			return this.mClientCertificateType >= 0 && TlsUtilities.HasSigningCapability((byte)this.mClientCertificateType);
		}

		// Token: 0x04001E94 RID: 7828
		protected TlsServer mTlsServer;

		// Token: 0x04001E95 RID: 7829
		internal TlsServerContextImpl mTlsServerContext;

		// Token: 0x04001E96 RID: 7830
		protected TlsKeyExchange mKeyExchange;

		// Token: 0x04001E97 RID: 7831
		protected TlsCredentials mServerCredentials;

		// Token: 0x04001E98 RID: 7832
		protected CertificateRequest mCertificateRequest;

		// Token: 0x04001E99 RID: 7833
		protected short mClientCertificateType = -1;

		// Token: 0x04001E9A RID: 7834
		protected TlsHandshakeHash mPrepareFinishHash;
	}
}
