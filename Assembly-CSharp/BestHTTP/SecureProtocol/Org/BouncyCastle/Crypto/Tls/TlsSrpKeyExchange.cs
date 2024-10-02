using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.Srp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000488 RID: 1160
	public class TlsSrpKeyExchange : AbstractTlsKeyExchange
	{
		// Token: 0x06002D39 RID: 11577 RVA: 0x0011B4F4 File Offset: 0x001196F4
		protected static TlsSigner CreateSigner(int keyExchange)
		{
			switch (keyExchange)
			{
			case 21:
				return null;
			case 22:
				return new TlsDssSigner();
			case 23:
				return new TlsRsaSigner();
			default:
				throw new ArgumentException("unsupported key exchange algorithm");
			}
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x0011B525 File Offset: 0x00119725
		[Obsolete("Use constructor taking an explicit 'groupVerifier' argument")]
		public TlsSrpKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, byte[] identity, byte[] password) : this(keyExchange, supportedSignatureAlgorithms, new DefaultTlsSrpGroupVerifier(), identity, password)
		{
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x0011B537 File Offset: 0x00119737
		public TlsSrpKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, TlsSrpGroupVerifier groupVerifier, byte[] identity, byte[] password) : base(keyExchange, supportedSignatureAlgorithms)
		{
			this.mTlsSigner = TlsSrpKeyExchange.CreateSigner(keyExchange);
			this.mGroupVerifier = groupVerifier;
			this.mIdentity = identity;
			this.mPassword = password;
			this.mSrpClient = new Srp6Client();
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x0011B570 File Offset: 0x00119770
		public TlsSrpKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, byte[] identity, TlsSrpLoginParameters loginParameters) : base(keyExchange, supportedSignatureAlgorithms)
		{
			this.mTlsSigner = TlsSrpKeyExchange.CreateSigner(keyExchange);
			this.mIdentity = identity;
			this.mSrpServer = new Srp6Server();
			this.mSrpGroup = loginParameters.Group;
			this.mSrpVerifier = loginParameters.Verifier;
			this.mSrpSalt = loginParameters.Salt;
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x0011B5CA File Offset: 0x001197CA
		public override void Init(TlsContext context)
		{
			base.Init(context);
			if (this.mTlsSigner != null)
			{
				this.mTlsSigner.Init(context);
			}
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x0011B5E7 File Offset: 0x001197E7
		public override void SkipServerCredentials()
		{
			if (this.mTlsSigner != null)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x0011B5FC File Offset: 0x001197FC
		public override void ProcessServerCertificate(Certificate serverCertificate)
		{
			if (this.mTlsSigner == null)
			{
				throw new TlsFatalAlert(10);
			}
			if (serverCertificate.IsEmpty)
			{
				throw new TlsFatalAlert(42);
			}
			X509CertificateStructure certificateAt = serverCertificate.GetCertificateAt(0);
			SubjectPublicKeyInfo subjectPublicKeyInfo = certificateAt.SubjectPublicKeyInfo;
			try
			{
				this.mServerPublicKey = PublicKeyFactory.CreateKey(subjectPublicKeyInfo);
			}
			catch (Exception alertCause)
			{
				throw new TlsFatalAlert(43, alertCause);
			}
			if (!this.mTlsSigner.IsValidPublicKey(this.mServerPublicKey))
			{
				throw new TlsFatalAlert(46);
			}
			TlsUtilities.ValidateKeyUsage(certificateAt, 128);
			base.ProcessServerCertificate(serverCertificate);
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x0011B690 File Offset: 0x00119890
		public override void ProcessServerCredentials(TlsCredentials serverCredentials)
		{
			if (this.mKeyExchange == 21 || !(serverCredentials is TlsSignerCredentials))
			{
				throw new TlsFatalAlert(80);
			}
			this.ProcessServerCertificate(serverCredentials.Certificate);
			this.mServerCredentials = (TlsSignerCredentials)serverCredentials;
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06002D41 RID: 11585 RVA: 0x0006AE98 File Offset: 0x00069098
		public override bool RequiresServerKeyExchange
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x0011B6C4 File Offset: 0x001198C4
		public override byte[] GenerateServerKeyExchange()
		{
			this.mSrpServer.Init(this.mSrpGroup, this.mSrpVerifier, TlsUtilities.CreateHash(2), this.mContext.SecureRandom);
			BigInteger b = this.mSrpServer.GenerateServerCredentials();
			ServerSrpParams serverSrpParams = new ServerSrpParams(this.mSrpGroup.N, this.mSrpGroup.G, this.mSrpSalt, b);
			DigestInputBuffer digestInputBuffer = new DigestInputBuffer();
			serverSrpParams.Encode(digestInputBuffer);
			if (this.mServerCredentials != null)
			{
				SignatureAndHashAlgorithm signatureAndHashAlgorithm = TlsUtilities.GetSignatureAndHashAlgorithm(this.mContext, this.mServerCredentials);
				IDigest digest = TlsUtilities.CreateHash(signatureAndHashAlgorithm);
				SecurityParameters securityParameters = this.mContext.SecurityParameters;
				digest.BlockUpdate(securityParameters.clientRandom, 0, securityParameters.clientRandom.Length);
				digest.BlockUpdate(securityParameters.serverRandom, 0, securityParameters.serverRandom.Length);
				digestInputBuffer.UpdateDigest(digest);
				byte[] array = new byte[digest.GetDigestSize()];
				digest.DoFinal(array, 0);
				byte[] signature = this.mServerCredentials.GenerateCertificateSignature(array);
				new DigitallySigned(signatureAndHashAlgorithm, signature).Encode(digestInputBuffer);
			}
			return digestInputBuffer.ToArray();
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x0011B7CC File Offset: 0x001199CC
		public override void ProcessServerKeyExchange(Stream input)
		{
			SecurityParameters securityParameters = this.mContext.SecurityParameters;
			SignerInputBuffer signerInputBuffer = null;
			Stream input2 = input;
			if (this.mTlsSigner != null)
			{
				signerInputBuffer = new SignerInputBuffer();
				input2 = new TeeInputStream(input, signerInputBuffer);
			}
			ServerSrpParams serverSrpParams = ServerSrpParams.Parse(input2);
			if (signerInputBuffer != null)
			{
				DigitallySigned digitallySigned = this.ParseSignature(input);
				ISigner signer = this.InitVerifyer(this.mTlsSigner, digitallySigned.Algorithm, securityParameters);
				signerInputBuffer.UpdateSigner(signer);
				if (!signer.VerifySignature(digitallySigned.Signature))
				{
					throw new TlsFatalAlert(51);
				}
			}
			this.mSrpGroup = new Srp6GroupParameters(serverSrpParams.N, serverSrpParams.G);
			if (!this.mGroupVerifier.Accept(this.mSrpGroup))
			{
				throw new TlsFatalAlert(71);
			}
			this.mSrpSalt = serverSrpParams.S;
			try
			{
				this.mSrpPeerCredentials = Srp6Utilities.ValidatePublicValue(this.mSrpGroup.N, serverSrpParams.B);
			}
			catch (CryptoException alertCause)
			{
				throw new TlsFatalAlert(47, alertCause);
			}
			this.mSrpClient.Init(this.mSrpGroup, TlsUtilities.CreateHash(2), this.mContext.SecureRandom);
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x00119FB5 File Offset: 0x001181B5
		public override void ValidateCertificateRequest(CertificateRequest certificateRequest)
		{
			throw new TlsFatalAlert(10);
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x0010D2C7 File Offset: 0x0010B4C7
		public override void ProcessClientCredentials(TlsCredentials clientCredentials)
		{
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x0011B8E4 File Offset: 0x00119AE4
		public override void GenerateClientKeyExchange(Stream output)
		{
			TlsSrpUtilities.WriteSrpParameter(this.mSrpClient.GenerateClientCredentials(this.mSrpSalt, this.mIdentity, this.mPassword), output);
			this.mContext.SecurityParameters.srpIdentity = Arrays.Clone(this.mIdentity);
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x0011B924 File Offset: 0x00119B24
		public override void ProcessClientKeyExchange(Stream input)
		{
			try
			{
				this.mSrpPeerCredentials = Srp6Utilities.ValidatePublicValue(this.mSrpGroup.N, TlsSrpUtilities.ReadSrpParameter(input));
			}
			catch (CryptoException alertCause)
			{
				throw new TlsFatalAlert(47, alertCause);
			}
			this.mContext.SecurityParameters.srpIdentity = Arrays.Clone(this.mIdentity);
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x0011B984 File Offset: 0x00119B84
		public override byte[] GeneratePremasterSecret()
		{
			byte[] result;
			try
			{
				result = BigIntegers.AsUnsignedByteArray((this.mSrpServer != null) ? this.mSrpServer.CalculateSecret(this.mSrpPeerCredentials) : this.mSrpClient.CalculateSecret(this.mSrpPeerCredentials));
			}
			catch (CryptoException alertCause)
			{
				throw new TlsFatalAlert(47, alertCause);
			}
			return result;
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x0011B9E0 File Offset: 0x00119BE0
		protected virtual ISigner InitVerifyer(TlsSigner tlsSigner, SignatureAndHashAlgorithm algorithm, SecurityParameters securityParameters)
		{
			ISigner signer = tlsSigner.CreateVerifyer(algorithm, this.mServerPublicKey);
			signer.BlockUpdate(securityParameters.clientRandom, 0, securityParameters.clientRandom.Length);
			signer.BlockUpdate(securityParameters.serverRandom, 0, securityParameters.serverRandom.Length);
			return signer;
		}

		// Token: 0x04001E9E RID: 7838
		protected TlsSigner mTlsSigner;

		// Token: 0x04001E9F RID: 7839
		protected TlsSrpGroupVerifier mGroupVerifier;

		// Token: 0x04001EA0 RID: 7840
		protected byte[] mIdentity;

		// Token: 0x04001EA1 RID: 7841
		protected byte[] mPassword;

		// Token: 0x04001EA2 RID: 7842
		protected AsymmetricKeyParameter mServerPublicKey;

		// Token: 0x04001EA3 RID: 7843
		protected Srp6GroupParameters mSrpGroup;

		// Token: 0x04001EA4 RID: 7844
		protected Srp6Client mSrpClient;

		// Token: 0x04001EA5 RID: 7845
		protected Srp6Server mSrpServer;

		// Token: 0x04001EA6 RID: 7846
		protected BigInteger mSrpPeerCredentials;

		// Token: 0x04001EA7 RID: 7847
		protected BigInteger mSrpVerifier;

		// Token: 0x04001EA8 RID: 7848
		protected byte[] mSrpSalt;

		// Token: 0x04001EA9 RID: 7849
		protected TlsSignerCredentials mServerCredentials;
	}
}
