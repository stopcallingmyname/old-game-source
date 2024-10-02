using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000427 RID: 1063
	public class DtlsServerProtocol : DtlsProtocol
	{
		// Token: 0x06002A7E RID: 10878 RVA: 0x00112450 File Offset: 0x00110650
		public DtlsServerProtocol(SecureRandom secureRandom) : base(secureRandom)
		{
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06002A7F RID: 10879 RVA: 0x00112460 File Offset: 0x00110660
		// (set) Token: 0x06002A80 RID: 10880 RVA: 0x00112468 File Offset: 0x00110668
		public virtual bool VerifyRequests
		{
			get
			{
				return this.mVerifyRequests;
			}
			set
			{
				this.mVerifyRequests = value;
			}
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x00112474 File Offset: 0x00110674
		public virtual DtlsTransport Accept(TlsServer server, DatagramTransport transport)
		{
			if (server == null)
			{
				throw new ArgumentNullException("server");
			}
			if (transport == null)
			{
				throw new ArgumentNullException("transport");
			}
			SecurityParameters securityParameters = new SecurityParameters();
			securityParameters.entity = 0;
			DtlsServerProtocol.ServerHandshakeState serverHandshakeState = new DtlsServerProtocol.ServerHandshakeState();
			serverHandshakeState.server = server;
			serverHandshakeState.serverContext = new TlsServerContextImpl(this.mSecureRandom, securityParameters);
			securityParameters.serverRandom = TlsProtocol.CreateRandomBlock(server.ShouldUseGmtUnixTime(), serverHandshakeState.serverContext.NonceRandomGenerator);
			server.Init(serverHandshakeState.serverContext);
			DtlsRecordLayer recordLayer = new DtlsRecordLayer(transport, serverHandshakeState.serverContext, server, 22);
			DtlsTransport result;
			try
			{
				result = this.ServerHandshake(serverHandshakeState, recordLayer);
			}
			catch (TlsFatalAlert tlsFatalAlert)
			{
				this.AbortServerHandshake(serverHandshakeState, recordLayer, tlsFatalAlert.AlertDescription);
				throw tlsFatalAlert;
			}
			catch (IOException ex)
			{
				this.AbortServerHandshake(serverHandshakeState, recordLayer, 80);
				throw ex;
			}
			catch (Exception alertCause)
			{
				this.AbortServerHandshake(serverHandshakeState, recordLayer, 80);
				throw new TlsFatalAlert(80, alertCause);
			}
			finally
			{
				securityParameters.Clear();
			}
			return result;
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x00112580 File Offset: 0x00110780
		internal virtual void AbortServerHandshake(DtlsServerProtocol.ServerHandshakeState state, DtlsRecordLayer recordLayer, byte alertDescription)
		{
			recordLayer.Fail(alertDescription);
			this.InvalidateSession(state);
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x00112590 File Offset: 0x00110790
		internal virtual DtlsTransport ServerHandshake(DtlsServerProtocol.ServerHandshakeState state, DtlsRecordLayer recordLayer)
		{
			SecurityParameters securityParameters = state.serverContext.SecurityParameters;
			DtlsReliableHandshake dtlsReliableHandshake = new DtlsReliableHandshake(state.serverContext, recordLayer);
			DtlsReliableHandshake.Message message = dtlsReliableHandshake.ReceiveMessage();
			if (message.Type != 1)
			{
				throw new TlsFatalAlert(10);
			}
			this.ProcessClientHello(state, message.Body);
			byte[] body = this.GenerateServerHello(state);
			DtlsProtocol.ApplyMaxFragmentLengthExtension(recordLayer, securityParameters.maxFragmentLength);
			ProtocolVersion serverVersion = state.serverContext.ServerVersion;
			recordLayer.ReadVersion = serverVersion;
			recordLayer.SetWriteVersion(serverVersion);
			dtlsReliableHandshake.SendMessage(2, body);
			dtlsReliableHandshake.NotifyHelloComplete();
			IList serverSupplementalData = state.server.GetServerSupplementalData();
			if (serverSupplementalData != null)
			{
				byte[] body2 = DtlsProtocol.GenerateSupplementalData(serverSupplementalData);
				dtlsReliableHandshake.SendMessage(23, body2);
			}
			state.keyExchange = state.server.GetKeyExchange();
			state.keyExchange.Init(state.serverContext);
			state.serverCredentials = state.server.GetCredentials();
			Certificate certificate = null;
			if (state.serverCredentials == null)
			{
				state.keyExchange.SkipServerCredentials();
			}
			else
			{
				state.keyExchange.ProcessServerCredentials(state.serverCredentials);
				certificate = state.serverCredentials.Certificate;
				byte[] body3 = DtlsProtocol.GenerateCertificate(certificate);
				dtlsReliableHandshake.SendMessage(11, body3);
			}
			if (certificate == null || certificate.IsEmpty)
			{
				state.allowCertificateStatus = false;
			}
			if (state.allowCertificateStatus)
			{
				CertificateStatus certificateStatus = state.server.GetCertificateStatus();
				if (certificateStatus != null)
				{
					byte[] body4 = this.GenerateCertificateStatus(state, certificateStatus);
					dtlsReliableHandshake.SendMessage(22, body4);
				}
			}
			byte[] array = state.keyExchange.GenerateServerKeyExchange();
			if (array != null)
			{
				dtlsReliableHandshake.SendMessage(12, array);
			}
			if (state.serverCredentials != null)
			{
				state.certificateRequest = state.server.GetCertificateRequest();
				if (state.certificateRequest != null)
				{
					if (TlsUtilities.IsTlsV12(state.serverContext) != (state.certificateRequest.SupportedSignatureAlgorithms != null))
					{
						throw new TlsFatalAlert(80);
					}
					state.keyExchange.ValidateCertificateRequest(state.certificateRequest);
					byte[] body5 = this.GenerateCertificateRequest(state, state.certificateRequest);
					dtlsReliableHandshake.SendMessage(13, body5);
					TlsUtilities.TrackHashAlgorithms(dtlsReliableHandshake.HandshakeHash, state.certificateRequest.SupportedSignatureAlgorithms);
				}
			}
			dtlsReliableHandshake.SendMessage(14, TlsUtilities.EmptyBytes);
			dtlsReliableHandshake.HandshakeHash.SealHashAlgorithms();
			message = dtlsReliableHandshake.ReceiveMessage();
			if (message.Type == 23)
			{
				this.ProcessClientSupplementalData(state, message.Body);
				message = dtlsReliableHandshake.ReceiveMessage();
			}
			else
			{
				state.server.ProcessClientSupplementalData(null);
			}
			if (state.certificateRequest == null)
			{
				state.keyExchange.SkipClientCredentials();
			}
			else if (message.Type == 11)
			{
				this.ProcessClientCertificate(state, message.Body);
				message = dtlsReliableHandshake.ReceiveMessage();
			}
			else
			{
				if (TlsUtilities.IsTlsV12(state.serverContext))
				{
					throw new TlsFatalAlert(10);
				}
				this.NotifyClientCertificate(state, Certificate.EmptyChain);
			}
			if (message.Type == 16)
			{
				this.ProcessClientKeyExchange(state, message.Body);
				TlsHandshakeHash tlsHandshakeHash = dtlsReliableHandshake.PrepareToFinish();
				securityParameters.sessionHash = TlsProtocol.GetCurrentPrfHash(state.serverContext, tlsHandshakeHash, null);
				TlsProtocol.EstablishMasterSecret(state.serverContext, state.keyExchange);
				recordLayer.InitPendingEpoch(state.server.GetCipher());
				if (this.ExpectCertificateVerifyMessage(state))
				{
					byte[] body6 = dtlsReliableHandshake.ReceiveMessageBody(15);
					this.ProcessCertificateVerify(state, body6, tlsHandshakeHash);
				}
				byte[] expected_verify_data = TlsUtilities.CalculateVerifyData(state.serverContext, "client finished", TlsProtocol.GetCurrentPrfHash(state.serverContext, dtlsReliableHandshake.HandshakeHash, null));
				this.ProcessFinished(dtlsReliableHandshake.ReceiveMessageBody(20), expected_verify_data);
				if (state.expectSessionTicket)
				{
					NewSessionTicket newSessionTicket = state.server.GetNewSessionTicket();
					byte[] body7 = this.GenerateNewSessionTicket(state, newSessionTicket);
					dtlsReliableHandshake.SendMessage(4, body7);
				}
				byte[] body8 = TlsUtilities.CalculateVerifyData(state.serverContext, "server finished", TlsProtocol.GetCurrentPrfHash(state.serverContext, dtlsReliableHandshake.HandshakeHash, null));
				dtlsReliableHandshake.SendMessage(20, body8);
				dtlsReliableHandshake.Finish();
				state.server.NotifyHandshakeComplete();
				return new DtlsTransport(recordLayer);
			}
			throw new TlsFatalAlert(10);
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x0011295C File Offset: 0x00110B5C
		protected virtual void InvalidateSession(DtlsServerProtocol.ServerHandshakeState state)
		{
			if (state.sessionParameters != null)
			{
				state.sessionParameters.Clear();
				state.sessionParameters = null;
			}
			if (state.tlsSession != null)
			{
				state.tlsSession.Invalidate();
				state.tlsSession = null;
			}
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x00112994 File Offset: 0x00110B94
		protected virtual byte[] GenerateCertificateRequest(DtlsServerProtocol.ServerHandshakeState state, CertificateRequest certificateRequest)
		{
			MemoryStream memoryStream = new MemoryStream();
			certificateRequest.Encode(memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x001129B4 File Offset: 0x00110BB4
		protected virtual byte[] GenerateCertificateStatus(DtlsServerProtocol.ServerHandshakeState state, CertificateStatus certificateStatus)
		{
			MemoryStream memoryStream = new MemoryStream();
			certificateStatus.Encode(memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x001129D4 File Offset: 0x00110BD4
		protected virtual byte[] GenerateNewSessionTicket(DtlsServerProtocol.ServerHandshakeState state, NewSessionTicket newSessionTicket)
		{
			MemoryStream memoryStream = new MemoryStream();
			newSessionTicket.Encode(memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x001129F4 File Offset: 0x00110BF4
		protected virtual byte[] GenerateServerHello(DtlsServerProtocol.ServerHandshakeState state)
		{
			SecurityParameters securityParameters = state.serverContext.SecurityParameters;
			MemoryStream memoryStream = new MemoryStream();
			ProtocolVersion serverVersion = state.server.GetServerVersion();
			if (!serverVersion.IsEqualOrEarlierVersionOf(state.serverContext.ClientVersion))
			{
				throw new TlsFatalAlert(80);
			}
			state.serverContext.SetServerVersion(serverVersion);
			TlsUtilities.WriteVersion(state.serverContext.ServerVersion, memoryStream);
			memoryStream.Write(securityParameters.ServerRandom, 0, securityParameters.ServerRandom.Length);
			TlsUtilities.WriteOpaque8(TlsUtilities.EmptyBytes, memoryStream);
			int selectedCipherSuite = state.server.GetSelectedCipherSuite();
			if (!Arrays.Contains(state.offeredCipherSuites, selectedCipherSuite) || selectedCipherSuite == 0 || CipherSuite.IsScsv(selectedCipherSuite) || !TlsUtilities.IsValidCipherSuiteForVersion(selectedCipherSuite, state.serverContext.ServerVersion))
			{
				throw new TlsFatalAlert(80);
			}
			DtlsProtocol.ValidateSelectedCipherSuite(selectedCipherSuite, 80);
			securityParameters.cipherSuite = selectedCipherSuite;
			byte selectedCompressionMethod = state.server.GetSelectedCompressionMethod();
			if (!Arrays.Contains(state.offeredCompressionMethods, selectedCompressionMethod))
			{
				throw new TlsFatalAlert(80);
			}
			securityParameters.compressionAlgorithm = selectedCompressionMethod;
			TlsUtilities.WriteUint16(selectedCipherSuite, memoryStream);
			TlsUtilities.WriteUint8(selectedCompressionMethod, memoryStream);
			state.serverExtensions = TlsExtensionsUtilities.EnsureExtensionsInitialised(state.server.GetServerExtensions());
			if (state.secure_renegotiation)
			{
				byte[] extensionData = TlsUtilities.GetExtensionData(state.serverExtensions, 65281);
				if (extensionData == null)
				{
					state.serverExtensions[65281] = TlsProtocol.CreateRenegotiationInfo(TlsUtilities.EmptyBytes);
				}
			}
			if (securityParameters.IsExtendedMasterSecret)
			{
				TlsExtensionsUtilities.AddExtendedMasterSecretExtension(state.serverExtensions);
			}
			if (state.serverExtensions.Count > 0)
			{
				securityParameters.encryptThenMac = TlsExtensionsUtilities.HasEncryptThenMacExtension(state.serverExtensions);
				securityParameters.maxFragmentLength = DtlsProtocol.EvaluateMaxFragmentLengthExtension(state.resumedSession, state.clientExtensions, state.serverExtensions, 80);
				securityParameters.truncatedHMac = TlsExtensionsUtilities.HasTruncatedHMacExtension(state.serverExtensions);
				state.allowCertificateStatus = (!state.resumedSession && TlsUtilities.HasExpectedEmptyExtensionData(state.serverExtensions, 5, 80));
				state.expectSessionTicket = (!state.resumedSession && TlsUtilities.HasExpectedEmptyExtensionData(state.serverExtensions, 35, 80));
				TlsProtocol.WriteExtensions(memoryStream, state.serverExtensions);
			}
			securityParameters.prfAlgorithm = TlsProtocol.GetPrfAlgorithm(state.serverContext, securityParameters.CipherSuite);
			securityParameters.verifyDataLength = 12;
			return memoryStream.ToArray();
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x00112C2C File Offset: 0x00110E2C
		protected virtual void NotifyClientCertificate(DtlsServerProtocol.ServerHandshakeState state, Certificate clientCertificate)
		{
			if (state.certificateRequest == null)
			{
				throw new InvalidOperationException();
			}
			if (state.clientCertificate != null)
			{
				throw new TlsFatalAlert(10);
			}
			state.clientCertificate = clientCertificate;
			if (clientCertificate.IsEmpty)
			{
				state.keyExchange.SkipClientCredentials();
			}
			else
			{
				state.clientCertificateType = TlsUtilities.GetClientCertificateType(clientCertificate, state.serverCredentials.Certificate);
				state.keyExchange.ProcessClientCertificate(clientCertificate);
			}
			state.server.NotifyClientCertificate(clientCertificate);
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x00112CA4 File Offset: 0x00110EA4
		protected virtual void ProcessClientCertificate(DtlsServerProtocol.ServerHandshakeState state, byte[] body)
		{
			MemoryStream memoryStream = new MemoryStream(body, false);
			Certificate clientCertificate = Certificate.Parse(memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			this.NotifyClientCertificate(state, clientCertificate);
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x00112CCC File Offset: 0x00110ECC
		protected virtual void ProcessCertificateVerify(DtlsServerProtocol.ServerHandshakeState state, byte[] body, TlsHandshakeHash prepareFinishHash)
		{
			if (state.certificateRequest == null)
			{
				throw new InvalidOperationException();
			}
			MemoryStream memoryStream = new MemoryStream(body, false);
			TlsServerContextImpl serverContext = state.serverContext;
			DigitallySigned digitallySigned = DigitallySigned.Parse(serverContext, memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			try
			{
				SignatureAndHashAlgorithm algorithm = digitallySigned.Algorithm;
				byte[] hash;
				if (TlsUtilities.IsTlsV12(serverContext))
				{
					TlsUtilities.VerifySupportedSignatureAlgorithm(state.certificateRequest.SupportedSignatureAlgorithms, algorithm);
					hash = prepareFinishHash.GetFinalHash(algorithm.Hash);
				}
				else
				{
					hash = serverContext.SecurityParameters.SessionHash;
				}
				AsymmetricKeyParameter publicKey = PublicKeyFactory.CreateKey(state.clientCertificate.GetCertificateAt(0).SubjectPublicKeyInfo);
				TlsSigner tlsSigner = TlsUtilities.CreateTlsSigner((byte)state.clientCertificateType);
				tlsSigner.Init(serverContext);
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

		// Token: 0x06002A8C RID: 10892 RVA: 0x00112DB4 File Offset: 0x00110FB4
		protected virtual void ProcessClientHello(DtlsServerProtocol.ServerHandshakeState state, byte[] body)
		{
			MemoryStream input = new MemoryStream(body, false);
			ProtocolVersion protocolVersion = TlsUtilities.ReadVersion(input);
			if (!protocolVersion.IsDtls)
			{
				throw new TlsFatalAlert(47);
			}
			byte[] clientRandom = TlsUtilities.ReadFully(32, input);
			if (TlsUtilities.ReadOpaque8(input).Length > 32)
			{
				throw new TlsFatalAlert(47);
			}
			TlsUtilities.ReadOpaque8(input);
			int num = TlsUtilities.ReadUint16(input);
			if (num < 2 || (num & 1) != 0)
			{
				throw new TlsFatalAlert(50);
			}
			state.offeredCipherSuites = TlsUtilities.ReadUint16Array(num / 2, input);
			int num2 = (int)TlsUtilities.ReadUint8(input);
			if (num2 < 1)
			{
				throw new TlsFatalAlert(47);
			}
			state.offeredCompressionMethods = TlsUtilities.ReadUint8Array(num2, input);
			state.clientExtensions = TlsProtocol.ReadExtensions(input);
			TlsServerContextImpl serverContext = state.serverContext;
			SecurityParameters securityParameters = serverContext.SecurityParameters;
			securityParameters.extendedMasterSecret = TlsExtensionsUtilities.HasExtendedMasterSecretExtension(state.clientExtensions);
			if (!securityParameters.IsExtendedMasterSecret && state.server.RequiresExtendedMasterSecret())
			{
				throw new TlsFatalAlert(40);
			}
			serverContext.SetClientVersion(protocolVersion);
			state.server.NotifyClientVersion(protocolVersion);
			state.server.NotifyFallback(Arrays.Contains(state.offeredCipherSuites, 22016));
			securityParameters.clientRandom = clientRandom;
			state.server.NotifyOfferedCipherSuites(state.offeredCipherSuites);
			state.server.NotifyOfferedCompressionMethods(state.offeredCompressionMethods);
			if (Arrays.Contains(state.offeredCipherSuites, 255))
			{
				state.secure_renegotiation = true;
			}
			byte[] extensionData = TlsUtilities.GetExtensionData(state.clientExtensions, 65281);
			if (extensionData != null)
			{
				state.secure_renegotiation = true;
				if (!Arrays.ConstantTimeAreEqual(extensionData, TlsProtocol.CreateRenegotiationInfo(TlsUtilities.EmptyBytes)))
				{
					throw new TlsFatalAlert(40);
				}
			}
			state.server.NotifySecureRenegotiation(state.secure_renegotiation);
			if (state.clientExtensions != null)
			{
				TlsExtensionsUtilities.GetPaddingExtension(state.clientExtensions);
				state.server.ProcessClientExtensions(state.clientExtensions);
			}
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x00112F78 File Offset: 0x00111178
		protected virtual void ProcessClientKeyExchange(DtlsServerProtocol.ServerHandshakeState state, byte[] body)
		{
			MemoryStream memoryStream = new MemoryStream(body, false);
			state.keyExchange.ProcessClientKeyExchange(memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x00112FA0 File Offset: 0x001111A0
		protected virtual void ProcessClientSupplementalData(DtlsServerProtocol.ServerHandshakeState state, byte[] body)
		{
			IList clientSupplementalData = TlsProtocol.ReadSupplementalDataMessage(new MemoryStream(body, false));
			state.server.ProcessClientSupplementalData(clientSupplementalData);
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x00112FC6 File Offset: 0x001111C6
		protected virtual bool ExpectCertificateVerifyMessage(DtlsServerProtocol.ServerHandshakeState state)
		{
			return state.clientCertificateType >= 0 && TlsUtilities.HasSigningCapability((byte)state.clientCertificateType);
		}

		// Token: 0x04001CEC RID: 7404
		protected bool mVerifyRequests = true;

		// Token: 0x02000943 RID: 2371
		protected internal class ServerHandshakeState
		{
			// Token: 0x04003603 RID: 13827
			internal TlsServer server;

			// Token: 0x04003604 RID: 13828
			internal TlsServerContextImpl serverContext;

			// Token: 0x04003605 RID: 13829
			internal TlsSession tlsSession;

			// Token: 0x04003606 RID: 13830
			internal SessionParameters sessionParameters;

			// Token: 0x04003607 RID: 13831
			internal SessionParameters.Builder sessionParametersBuilder;

			// Token: 0x04003608 RID: 13832
			internal int[] offeredCipherSuites;

			// Token: 0x04003609 RID: 13833
			internal byte[] offeredCompressionMethods;

			// Token: 0x0400360A RID: 13834
			internal IDictionary clientExtensions;

			// Token: 0x0400360B RID: 13835
			internal IDictionary serverExtensions;

			// Token: 0x0400360C RID: 13836
			internal bool resumedSession;

			// Token: 0x0400360D RID: 13837
			internal bool secure_renegotiation;

			// Token: 0x0400360E RID: 13838
			internal bool allowCertificateStatus;

			// Token: 0x0400360F RID: 13839
			internal bool expectSessionTicket;

			// Token: 0x04003610 RID: 13840
			internal TlsKeyExchange keyExchange;

			// Token: 0x04003611 RID: 13841
			internal TlsCredentials serverCredentials;

			// Token: 0x04003612 RID: 13842
			internal CertificateRequest certificateRequest;

			// Token: 0x04003613 RID: 13843
			internal short clientCertificateType = -1;

			// Token: 0x04003614 RID: 13844
			internal Certificate clientCertificate;
		}
	}
}
