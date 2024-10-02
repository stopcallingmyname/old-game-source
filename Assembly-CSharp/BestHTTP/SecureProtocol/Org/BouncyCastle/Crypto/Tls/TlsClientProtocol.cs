using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200045B RID: 1115
	public class TlsClientProtocol : TlsProtocol
	{
		// Token: 0x06002B8B RID: 11147 RVA: 0x001155DD File Offset: 0x001137DD
		public TlsClientProtocol(Stream stream, SecureRandom secureRandom) : base(stream, secureRandom)
		{
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x001155E7 File Offset: 0x001137E7
		public TlsClientProtocol(Stream input, Stream output, SecureRandom secureRandom) : base(input, output, secureRandom)
		{
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x001155F2 File Offset: 0x001137F2
		public TlsClientProtocol(SecureRandom secureRandom) : base(secureRandom)
		{
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x001155FC File Offset: 0x001137FC
		public virtual void Connect(TlsClient tlsClient)
		{
			if (tlsClient == null)
			{
				throw new ArgumentNullException("tlsClient");
			}
			if (this.mTlsClient != null)
			{
				throw new InvalidOperationException("'Connect' can only be called once");
			}
			this.mTlsClient = tlsClient;
			this.mSecurityParameters = new SecurityParameters();
			this.mSecurityParameters.entity = 1;
			this.mTlsClientContext = new TlsClientContextImpl(this.mSecureRandom, this.mSecurityParameters);
			this.mSecurityParameters.clientRandom = TlsProtocol.CreateRandomBlock(tlsClient.ShouldUseGmtUnixTime(), this.mTlsClientContext.NonceRandomGenerator);
			this.mTlsClient.Init(this.mTlsClientContext);
			this.mRecordStream.Init(this.mTlsClientContext);
			TlsSession sessionToResume = tlsClient.GetSessionToResume();
			if (sessionToResume != null && sessionToResume.IsResumable)
			{
				SessionParameters sessionParameters = sessionToResume.ExportSessionParameters();
				if (sessionParameters != null && sessionParameters.IsExtendedMasterSecret)
				{
					this.mTlsSession = sessionToResume;
					this.mSessionParameters = sessionParameters;
				}
			}
			this.SendClientHelloMessage();
			this.mConnectionState = 1;
			this.BlockForHandshake();
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x001156E7 File Offset: 0x001138E7
		protected override void CleanupHandshake()
		{
			base.CleanupHandshake();
			this.mSelectedSessionID = null;
			this.mKeyExchange = null;
			this.mAuthentication = null;
			this.mCertificateStatus = null;
			this.mCertificateRequest = null;
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06002B90 RID: 11152 RVA: 0x00115712 File Offset: 0x00113912
		protected override TlsContext Context
		{
			get
			{
				return this.mTlsClientContext;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06002B91 RID: 11153 RVA: 0x00115712 File Offset: 0x00113912
		internal override AbstractTlsContext ContextAdmin
		{
			get
			{
				return this.mTlsClientContext;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06002B92 RID: 11154 RVA: 0x0011571A File Offset: 0x0011391A
		protected override TlsPeer Peer
		{
			get
			{
				return this.mTlsClient;
			}
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x00115724 File Offset: 0x00113924
		protected override void HandleHandshakeMessage(byte type, MemoryStream buf)
		{
			if (!this.mResumedSession)
			{
				switch (type)
				{
				case 0:
					TlsProtocol.AssertEmpty(buf);
					if (this.mConnectionState == 16)
					{
						this.RefuseRenegotiation();
						return;
					}
					return;
				case 2:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState != 1)
					{
						throw new TlsFatalAlert(10);
					}
					this.ReceiveServerHelloMessage(buf);
					this.mConnectionState = 2;
					this.mRecordStream.NotifyHelloComplete();
					this.ApplyMaxFragmentLengthExtension();
					if (this.mResumedSession)
					{
						this.mSecurityParameters.masterSecret = Arrays.Clone(this.mSessionParameters.MasterSecret);
						this.mRecordStream.SetPendingConnectionState(this.Peer.GetCompression(), this.Peer.GetCipher());
						return;
					}
					this.InvalidateSession();
					if (this.mSelectedSessionID.Length != 0)
					{
						this.mTlsSession = new TlsSessionImpl(this.mSelectedSessionID, null);
						return;
					}
					return;
				}
				case 4:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState != 13)
					{
						throw new TlsFatalAlert(10);
					}
					if (!this.mExpectSessionTicket)
					{
						throw new TlsFatalAlert(10);
					}
					this.InvalidateSession();
					this.ReceiveNewSessionTicketMessage(buf);
					this.mConnectionState = 14;
					return;
				}
				case 11:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState - 2 <= 1)
					{
						if (this.mConnectionState == 2)
						{
							this.HandleSupplementalData(null);
						}
						this.mPeerCertificate = Certificate.Parse(buf);
						TlsProtocol.AssertEmpty(buf);
						if (this.mPeerCertificate == null || this.mPeerCertificate.IsEmpty)
						{
							this.mAllowCertificateStatus = false;
						}
						this.mKeyExchange.ProcessServerCertificate(this.mPeerCertificate);
						this.mAuthentication = this.mTlsClient.GetAuthentication();
						this.mAuthentication.NotifyServerCertificate(this.mPeerCertificate);
						this.mConnectionState = 4;
						return;
					}
					throw new TlsFatalAlert(10);
				}
				case 12:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState - 2 <= 3)
					{
						if (this.mConnectionState < 3)
						{
							this.HandleSupplementalData(null);
						}
						if (this.mConnectionState < 4)
						{
							this.mKeyExchange.SkipServerCredentials();
							this.mAuthentication = null;
						}
						this.mKeyExchange.ProcessServerKeyExchange(buf);
						TlsProtocol.AssertEmpty(buf);
						this.mConnectionState = 6;
						return;
					}
					throw new TlsFatalAlert(10);
				}
				case 13:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState - 4 > 2)
					{
						throw new TlsFatalAlert(10);
					}
					if (this.mConnectionState != 6)
					{
						this.mKeyExchange.SkipServerKeyExchange();
					}
					if (this.mAuthentication == null)
					{
						throw new TlsFatalAlert(40);
					}
					this.mCertificateRequest = CertificateRequest.Parse(this.Context, buf);
					TlsProtocol.AssertEmpty(buf);
					this.mKeyExchange.ValidateCertificateRequest(this.mCertificateRequest);
					TlsUtilities.TrackHashAlgorithms(this.mRecordStream.HandshakeHash, this.mCertificateRequest.SupportedSignatureAlgorithms);
					this.mConnectionState = 7;
					return;
				}
				case 14:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState - 2 <= 5)
					{
						if (this.mConnectionState < 3)
						{
							this.HandleSupplementalData(null);
						}
						if (this.mConnectionState < 4)
						{
							this.mKeyExchange.SkipServerCredentials();
							this.mAuthentication = null;
						}
						if (this.mConnectionState < 6)
						{
							this.mKeyExchange.SkipServerKeyExchange();
						}
						TlsProtocol.AssertEmpty(buf);
						this.mConnectionState = 8;
						this.mRecordStream.HandshakeHash.SealHashAlgorithms();
						IList clientSupplementalData = this.mTlsClient.GetClientSupplementalData();
						if (clientSupplementalData != null)
						{
							this.SendSupplementalDataMessage(clientSupplementalData);
						}
						this.mConnectionState = 9;
						TlsCredentials tlsCredentials = null;
						if (this.mCertificateRequest == null)
						{
							this.mKeyExchange.SkipClientCredentials();
						}
						else
						{
							tlsCredentials = this.mAuthentication.GetClientCredentials(this.Context, this.mCertificateRequest);
							if (tlsCredentials == null)
							{
								this.mKeyExchange.SkipClientCredentials();
								this.SendCertificateMessage(Certificate.EmptyChain);
							}
							else
							{
								this.mKeyExchange.ProcessClientCredentials(tlsCredentials);
								this.SendCertificateMessage(tlsCredentials.Certificate);
							}
						}
						this.mConnectionState = 10;
						this.SendClientKeyExchangeMessage();
						this.mConnectionState = 11;
						if (TlsUtilities.IsSsl(this.Context))
						{
							TlsProtocol.EstablishMasterSecret(this.Context, this.mKeyExchange);
						}
						TlsHandshakeHash tlsHandshakeHash = this.mRecordStream.PrepareToFinish();
						this.mSecurityParameters.sessionHash = TlsProtocol.GetCurrentPrfHash(this.Context, tlsHandshakeHash, null);
						if (!TlsUtilities.IsSsl(this.Context))
						{
							TlsProtocol.EstablishMasterSecret(this.Context, this.mKeyExchange);
						}
						this.mRecordStream.SetPendingConnectionState(this.Peer.GetCompression(), this.Peer.GetCipher());
						if (tlsCredentials != null && tlsCredentials is TlsSignerCredentials)
						{
							TlsSignerCredentials tlsSignerCredentials = (TlsSignerCredentials)tlsCredentials;
							SignatureAndHashAlgorithm signatureAndHashAlgorithm = TlsUtilities.GetSignatureAndHashAlgorithm(this.Context, tlsSignerCredentials);
							byte[] hash;
							if (signatureAndHashAlgorithm == null)
							{
								hash = this.mSecurityParameters.SessionHash;
							}
							else
							{
								hash = tlsHandshakeHash.GetFinalHash(signatureAndHashAlgorithm.Hash);
							}
							byte[] signature = tlsSignerCredentials.GenerateCertificateSignature(hash);
							DigitallySigned certificateVerify = new DigitallySigned(signatureAndHashAlgorithm, signature);
							this.SendCertificateVerifyMessage(certificateVerify);
							this.mConnectionState = 12;
						}
						this.SendChangeCipherSpecMessage();
						this.SendFinishedMessage();
						this.mConnectionState = 13;
						return;
					}
					throw new TlsFatalAlert(10);
				}
				case 20:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState - 13 > 1)
					{
						throw new TlsFatalAlert(10);
					}
					if (this.mConnectionState == 13 && this.mExpectSessionTicket)
					{
						throw new TlsFatalAlert(10);
					}
					this.ProcessFinishedMessage(buf);
					this.mConnectionState = 15;
					this.CompleteHandshake();
					return;
				}
				case 22:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState != 4)
					{
						throw new TlsFatalAlert(10);
					}
					if (!this.mAllowCertificateStatus)
					{
						throw new TlsFatalAlert(10);
					}
					this.mCertificateStatus = CertificateStatus.Parse(buf);
					TlsProtocol.AssertEmpty(buf);
					this.mConnectionState = 5;
					return;
				}
				case 23:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState == 2)
					{
						this.HandleSupplementalData(TlsProtocol.ReadSupplementalDataMessage(buf));
						return;
					}
					throw new TlsFatalAlert(10);
				}
				}
				throw new TlsFatalAlert(10);
			}
			if (type != 20 || this.mConnectionState != 2)
			{
				throw new TlsFatalAlert(10);
			}
			this.ProcessFinishedMessage(buf);
			this.mConnectionState = 15;
			this.SendChangeCipherSpecMessage();
			this.SendFinishedMessage();
			this.mConnectionState = 13;
			this.CompleteHandshake();
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x00115D00 File Offset: 0x00113F00
		protected virtual void HandleSupplementalData(IList serverSupplementalData)
		{
			this.mTlsClient.ProcessServerSupplementalData(serverSupplementalData);
			this.mConnectionState = 3;
			this.mKeyExchange = this.mTlsClient.GetKeyExchange();
			this.mKeyExchange.Init(this.Context);
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x00115D38 File Offset: 0x00113F38
		protected virtual void ReceiveNewSessionTicketMessage(MemoryStream buf)
		{
			NewSessionTicket newSessionTicket = NewSessionTicket.Parse(buf);
			TlsProtocol.AssertEmpty(buf);
			this.mTlsClient.NotifyNewSessionTicket(newSessionTicket);
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x00115D60 File Offset: 0x00113F60
		protected virtual void ReceiveServerHelloMessage(MemoryStream buf)
		{
			ProtocolVersion protocolVersion = TlsUtilities.ReadVersion(buf);
			if (protocolVersion.IsDtls)
			{
				throw new TlsFatalAlert(47);
			}
			if (!protocolVersion.Equals(this.mRecordStream.ReadVersion))
			{
				throw new TlsFatalAlert(47);
			}
			ProtocolVersion clientVersion = this.Context.ClientVersion;
			if (!protocolVersion.IsEqualOrEarlierVersionOf(clientVersion))
			{
				throw new TlsFatalAlert(47);
			}
			this.mRecordStream.SetWriteVersion(protocolVersion);
			this.ContextAdmin.SetServerVersion(protocolVersion);
			this.mTlsClient.NotifyServerVersion(protocolVersion);
			this.mSecurityParameters.serverRandom = TlsUtilities.ReadFully(32, buf);
			this.mSelectedSessionID = TlsUtilities.ReadOpaque8(buf);
			if (this.mSelectedSessionID.Length > 32)
			{
				throw new TlsFatalAlert(47);
			}
			this.mTlsClient.NotifySessionID(this.mSelectedSessionID);
			this.mResumedSession = (this.mSelectedSessionID.Length != 0 && this.mTlsSession != null && Arrays.AreEqual(this.mSelectedSessionID, this.mTlsSession.SessionID));
			int num = TlsUtilities.ReadUint16(buf);
			if (!Arrays.Contains(this.mOfferedCipherSuites, num) || num == 0 || CipherSuite.IsScsv(num) || !TlsUtilities.IsValidCipherSuiteForVersion(num, this.Context.ServerVersion))
			{
				throw new TlsFatalAlert(47);
			}
			this.mTlsClient.NotifySelectedCipherSuite(num);
			byte b = TlsUtilities.ReadUint8(buf);
			if (!Arrays.Contains(this.mOfferedCompressionMethods, b))
			{
				throw new TlsFatalAlert(47);
			}
			this.mTlsClient.NotifySelectedCompressionMethod(b);
			this.mServerExtensions = TlsProtocol.ReadExtensions(buf);
			this.mSecurityParameters.extendedMasterSecret = (!TlsUtilities.IsSsl(this.mTlsClientContext) && TlsExtensionsUtilities.HasExtendedMasterSecretExtension(this.mServerExtensions));
			if (!this.mSecurityParameters.IsExtendedMasterSecret && (this.mResumedSession || this.mTlsClient.RequiresExtendedMasterSecret()))
			{
				throw new TlsFatalAlert(40);
			}
			if (this.mServerExtensions != null)
			{
				foreach (object obj in this.mServerExtensions.Keys)
				{
					int num2 = (int)obj;
					if (num2 != 65281)
					{
						if (TlsUtilities.GetExtensionData(this.mClientExtensions, num2) == null)
						{
							throw new TlsFatalAlert(110);
						}
						bool mResumedSession = this.mResumedSession;
					}
				}
			}
			byte[] extensionData = TlsUtilities.GetExtensionData(this.mServerExtensions, 65281);
			if (extensionData != null)
			{
				this.mSecureRenegotiation = true;
				if (!Arrays.ConstantTimeAreEqual(extensionData, TlsProtocol.CreateRenegotiationInfo(TlsUtilities.EmptyBytes)))
				{
					throw new TlsFatalAlert(40);
				}
			}
			this.mTlsClient.NotifySecureRenegotiation(this.mSecureRenegotiation);
			IDictionary dictionary = this.mClientExtensions;
			IDictionary dictionary2 = this.mServerExtensions;
			if (this.mResumedSession)
			{
				if (num != this.mSessionParameters.CipherSuite || b != this.mSessionParameters.CompressionAlgorithm)
				{
					throw new TlsFatalAlert(47);
				}
				dictionary = null;
				dictionary2 = this.mSessionParameters.ReadServerExtensions();
			}
			this.mSecurityParameters.cipherSuite = num;
			this.mSecurityParameters.compressionAlgorithm = b;
			if (dictionary2 != null && dictionary2.Count > 0)
			{
				bool flag = TlsExtensionsUtilities.HasEncryptThenMacExtension(dictionary2);
				if (flag && !TlsUtilities.IsBlockCipherSuite(num))
				{
					throw new TlsFatalAlert(47);
				}
				this.mSecurityParameters.encryptThenMac = flag;
				this.mSecurityParameters.maxFragmentLength = this.ProcessMaxFragmentLengthExtension(dictionary, dictionary2, 47);
				this.mSecurityParameters.truncatedHMac = TlsExtensionsUtilities.HasTruncatedHMacExtension(dictionary2);
				this.mAllowCertificateStatus = (!this.mResumedSession && TlsUtilities.HasExpectedEmptyExtensionData(dictionary2, 5, 47));
				this.mExpectSessionTicket = (!this.mResumedSession && TlsUtilities.HasExpectedEmptyExtensionData(dictionary2, 35, 47));
			}
			if (dictionary != null)
			{
				this.mTlsClient.ProcessServerExtensions(dictionary2);
			}
			this.mSecurityParameters.prfAlgorithm = TlsProtocol.GetPrfAlgorithm(this.Context, this.mSecurityParameters.CipherSuite);
			this.mSecurityParameters.verifyDataLength = 12;
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x00116128 File Offset: 0x00114328
		protected virtual void SendCertificateVerifyMessage(DigitallySigned certificateVerify)
		{
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(15);
			certificateVerify.Encode(handshakeMessage);
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x0011614C File Offset: 0x0011434C
		protected virtual void SendClientHelloMessage()
		{
			this.mRecordStream.SetWriteVersion(this.mTlsClient.ClientHelloRecordLayerVersion);
			ProtocolVersion clientVersion = this.mTlsClient.ClientVersion;
			if (clientVersion.IsDtls)
			{
				throw new TlsFatalAlert(80);
			}
			this.ContextAdmin.SetClientVersion(clientVersion);
			byte[] array = TlsUtilities.EmptyBytes;
			if (this.mTlsSession != null)
			{
				array = this.mTlsSession.SessionID;
				if (array == null || array.Length > 32)
				{
					array = TlsUtilities.EmptyBytes;
				}
			}
			bool isFallback = this.mTlsClient.IsFallback;
			this.mOfferedCipherSuites = this.mTlsClient.GetCipherSuites();
			this.mOfferedCompressionMethods = this.mTlsClient.GetCompressionMethods();
			if (array.Length != 0 && this.mSessionParameters != null && (!this.mSessionParameters.IsExtendedMasterSecret || !Arrays.Contains(this.mOfferedCipherSuites, this.mSessionParameters.CipherSuite) || !Arrays.Contains(this.mOfferedCompressionMethods, this.mSessionParameters.CompressionAlgorithm)))
			{
				array = TlsUtilities.EmptyBytes;
			}
			this.mClientExtensions = TlsExtensionsUtilities.EnsureExtensionsInitialised(this.mTlsClient.GetClientExtensions());
			if (!clientVersion.IsSsl)
			{
				TlsExtensionsUtilities.AddExtendedMasterSecretExtension(this.mClientExtensions);
			}
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(1);
			TlsUtilities.WriteVersion(clientVersion, handshakeMessage);
			handshakeMessage.Write(this.mSecurityParameters.ClientRandom);
			TlsUtilities.WriteOpaque8(array, handshakeMessage);
			byte[] extensionData = TlsUtilities.GetExtensionData(this.mClientExtensions, 65281);
			bool flag = extensionData == null;
			bool flag2 = !Arrays.Contains(this.mOfferedCipherSuites, 255);
			if (flag && flag2)
			{
				this.mOfferedCipherSuites = Arrays.Append(this.mOfferedCipherSuites, 255);
			}
			if (isFallback && !Arrays.Contains(this.mOfferedCipherSuites, 22016))
			{
				this.mOfferedCipherSuites = Arrays.Append(this.mOfferedCipherSuites, 22016);
			}
			TlsUtilities.WriteUint16ArrayWithUint16Length(this.mOfferedCipherSuites, handshakeMessage);
			TlsUtilities.WriteUint8ArrayWithUint8Length(this.mOfferedCompressionMethods, handshakeMessage);
			TlsProtocol.WriteExtensions(handshakeMessage, this.mClientExtensions);
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x00116328 File Offset: 0x00114528
		protected virtual void SendClientKeyExchangeMessage()
		{
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(16);
			this.mKeyExchange.GenerateClientKeyExchange(handshakeMessage);
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x04001E19 RID: 7705
		protected TlsClient mTlsClient;

		// Token: 0x04001E1A RID: 7706
		internal TlsClientContextImpl mTlsClientContext;

		// Token: 0x04001E1B RID: 7707
		protected byte[] mSelectedSessionID;

		// Token: 0x04001E1C RID: 7708
		protected TlsKeyExchange mKeyExchange;

		// Token: 0x04001E1D RID: 7709
		protected TlsAuthentication mAuthentication;

		// Token: 0x04001E1E RID: 7710
		protected CertificateStatus mCertificateStatus;

		// Token: 0x04001E1F RID: 7711
		protected CertificateRequest mCertificateRequest;
	}
}
