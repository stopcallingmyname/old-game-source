using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200047A RID: 1146
	public class TlsPskKeyExchange : AbstractTlsKeyExchange
	{
		// Token: 0x06002CD4 RID: 11476 RVA: 0x00119CD4 File Offset: 0x00117ED4
		[Obsolete("Use constructor that takes a TlsDHVerifier")]
		public TlsPskKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, TlsPskIdentity pskIdentity, TlsPskIdentityManager pskIdentityManager, DHParameters dhParameters, int[] namedCurves, byte[] clientECPointFormats, byte[] serverECPointFormats) : this(keyExchange, supportedSignatureAlgorithms, pskIdentity, pskIdentityManager, new DefaultTlsDHVerifier(), dhParameters, namedCurves, clientECPointFormats, serverECPointFormats)
		{
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x00119CFC File Offset: 0x00117EFC
		public TlsPskKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, TlsPskIdentity pskIdentity, TlsPskIdentityManager pskIdentityManager, TlsDHVerifier dhVerifier, DHParameters dhParameters, int[] namedCurves, byte[] clientECPointFormats, byte[] serverECPointFormats) : base(keyExchange, supportedSignatureAlgorithms)
		{
			if (keyExchange - 13 > 2 && keyExchange != 24)
			{
				throw new InvalidOperationException("unsupported key exchange algorithm");
			}
			this.mPskIdentity = pskIdentity;
			this.mPskIdentityManager = pskIdentityManager;
			this.mDHVerifier = dhVerifier;
			this.mDHParameters = dhParameters;
			this.mNamedCurves = namedCurves;
			this.mClientECPointFormats = clientECPointFormats;
			this.mServerECPointFormats = serverECPointFormats;
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x00119D5F File Offset: 0x00117F5F
		public override void SkipServerCredentials()
		{
			if (this.mKeyExchange == 15)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x00119D73 File Offset: 0x00117F73
		public override void ProcessServerCredentials(TlsCredentials serverCredentials)
		{
			if (!(serverCredentials is TlsEncryptionCredentials))
			{
				throw new TlsFatalAlert(80);
			}
			this.ProcessServerCertificate(serverCredentials.Certificate);
			this.mServerCredentials = (TlsEncryptionCredentials)serverCredentials;
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x00119DA0 File Offset: 0x00117FA0
		public override byte[] GenerateServerKeyExchange()
		{
			this.mPskIdentityHint = this.mPskIdentityManager.GetHint();
			if (this.mPskIdentityHint == null && !this.RequiresServerKeyExchange)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream();
			if (this.mPskIdentityHint == null)
			{
				TlsUtilities.WriteOpaque16(TlsUtilities.EmptyBytes, memoryStream);
			}
			else
			{
				TlsUtilities.WriteOpaque16(this.mPskIdentityHint, memoryStream);
			}
			if (this.mKeyExchange == 14)
			{
				if (this.mDHParameters == null)
				{
					throw new TlsFatalAlert(80);
				}
				this.mDHAgreePrivateKey = TlsDHUtilities.GenerateEphemeralServerKeyExchange(this.mContext.SecureRandom, this.mDHParameters, memoryStream);
			}
			else if (this.mKeyExchange == 24)
			{
				this.mECAgreePrivateKey = TlsEccUtilities.GenerateEphemeralServerKeyExchange(this.mContext.SecureRandom, this.mNamedCurves, this.mClientECPointFormats, memoryStream);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x00119E64 File Offset: 0x00118064
		public override void ProcessServerCertificate(Certificate serverCertificate)
		{
			if (this.mKeyExchange != 15)
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
			if (this.mServerPublicKey.IsPrivate)
			{
				throw new TlsFatalAlert(80);
			}
			this.mRsaServerPublicKey = this.ValidateRsaPublicKey((RsaKeyParameters)this.mServerPublicKey);
			TlsUtilities.ValidateKeyUsage(certificateAt, 32);
			base.ProcessServerCertificate(serverCertificate);
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06002CDA RID: 11482 RVA: 0x00119F08 File Offset: 0x00118108
		public override bool RequiresServerKeyExchange
		{
			get
			{
				int mKeyExchange = this.mKeyExchange;
				return mKeyExchange == 14 || mKeyExchange == 24;
			}
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x00119F2C File Offset: 0x0011812C
		public override void ProcessServerKeyExchange(Stream input)
		{
			this.mPskIdentityHint = TlsUtilities.ReadOpaque16(input);
			if (this.mKeyExchange == 14)
			{
				this.mDHParameters = TlsDHUtilities.ReceiveDHParameters(this.mDHVerifier, input);
				this.mDHAgreePublicKey = new DHPublicKeyParameters(TlsDHUtilities.ReadDHParameter(input), this.mDHParameters);
				return;
			}
			if (this.mKeyExchange == 24)
			{
				ECDomainParameters curve_params = TlsEccUtilities.ReadECParameters(this.mNamedCurves, this.mClientECPointFormats, input);
				byte[] encoding = TlsUtilities.ReadOpaque8(input);
				this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey(TlsEccUtilities.DeserializeECPublicKey(this.mClientECPointFormats, curve_params, encoding));
			}
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x00119FB5 File Offset: 0x001181B5
		public override void ValidateCertificateRequest(CertificateRequest certificateRequest)
		{
			throw new TlsFatalAlert(10);
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x0010D2C7 File Offset: 0x0010B4C7
		public override void ProcessClientCredentials(TlsCredentials clientCredentials)
		{
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x00119FC0 File Offset: 0x001181C0
		public override void GenerateClientKeyExchange(Stream output)
		{
			if (this.mPskIdentityHint == null)
			{
				this.mPskIdentity.SkipIdentityHint();
			}
			else
			{
				this.mPskIdentity.NotifyIdentityHint(this.mPskIdentityHint);
			}
			byte[] pskIdentity = this.mPskIdentity.GetPskIdentity();
			if (pskIdentity == null)
			{
				throw new TlsFatalAlert(80);
			}
			this.mPsk = this.mPskIdentity.GetPsk();
			if (this.mPsk == null)
			{
				throw new TlsFatalAlert(80);
			}
			TlsUtilities.WriteOpaque16(pskIdentity, output);
			this.mContext.SecurityParameters.pskIdentity = pskIdentity;
			if (this.mKeyExchange == 14)
			{
				this.mDHAgreePrivateKey = TlsDHUtilities.GenerateEphemeralClientKeyExchange(this.mContext.SecureRandom, this.mDHParameters, output);
				return;
			}
			if (this.mKeyExchange == 24)
			{
				this.mECAgreePrivateKey = TlsEccUtilities.GenerateEphemeralClientKeyExchange(this.mContext.SecureRandom, this.mServerECPointFormats, this.mECAgreePublicKey.Parameters, output);
				return;
			}
			if (this.mKeyExchange == 15)
			{
				this.mPremasterSecret = TlsRsaUtilities.GenerateEncryptedPreMasterSecret(this.mContext, this.mRsaServerPublicKey, output);
			}
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x0011A0C0 File Offset: 0x001182C0
		public override void ProcessClientKeyExchange(Stream input)
		{
			byte[] array = TlsUtilities.ReadOpaque16(input);
			this.mPsk = this.mPskIdentityManager.GetPsk(array);
			if (this.mPsk == null)
			{
				throw new TlsFatalAlert(115);
			}
			this.mContext.SecurityParameters.pskIdentity = array;
			if (this.mKeyExchange == 14)
			{
				this.mDHAgreePublicKey = new DHPublicKeyParameters(TlsDHUtilities.ReadDHParameter(input), this.mDHParameters);
				return;
			}
			if (this.mKeyExchange == 24)
			{
				byte[] encoding = TlsUtilities.ReadOpaque8(input);
				ECDomainParameters parameters = this.mECAgreePrivateKey.Parameters;
				this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey(TlsEccUtilities.DeserializeECPublicKey(this.mServerECPointFormats, parameters, encoding));
				return;
			}
			if (this.mKeyExchange == 15)
			{
				byte[] encryptedPreMasterSecret;
				if (TlsUtilities.IsSsl(this.mContext))
				{
					encryptedPreMasterSecret = Streams.ReadAll(input);
				}
				else
				{
					encryptedPreMasterSecret = TlsUtilities.ReadOpaque16(input);
				}
				this.mPremasterSecret = this.mServerCredentials.DecryptPreMasterSecret(encryptedPreMasterSecret);
			}
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x0011A198 File Offset: 0x00118398
		public override byte[] GeneratePremasterSecret()
		{
			byte[] array = this.GenerateOtherSecret(this.mPsk.Length);
			MemoryStream memoryStream = new MemoryStream(4 + array.Length + this.mPsk.Length);
			TlsUtilities.WriteOpaque16(array, memoryStream);
			TlsUtilities.WriteOpaque16(this.mPsk, memoryStream);
			Arrays.Fill(this.mPsk, 0);
			this.mPsk = null;
			return memoryStream.ToArray();
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x0011A1F4 File Offset: 0x001183F4
		protected virtual byte[] GenerateOtherSecret(int pskLength)
		{
			if (this.mKeyExchange == 14)
			{
				if (this.mDHAgreePrivateKey != null)
				{
					return TlsDHUtilities.CalculateDHBasicAgreement(this.mDHAgreePublicKey, this.mDHAgreePrivateKey);
				}
				throw new TlsFatalAlert(80);
			}
			else if (this.mKeyExchange == 24)
			{
				if (this.mECAgreePrivateKey != null)
				{
					return TlsEccUtilities.CalculateECDHBasicAgreement(this.mECAgreePublicKey, this.mECAgreePrivateKey);
				}
				throw new TlsFatalAlert(80);
			}
			else
			{
				if (this.mKeyExchange == 15)
				{
					return this.mPremasterSecret;
				}
				return new byte[pskLength];
			}
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x0011A270 File Offset: 0x00118470
		protected virtual RsaKeyParameters ValidateRsaPublicKey(RsaKeyParameters key)
		{
			if (!key.Exponent.IsProbablePrime(2))
			{
				throw new TlsFatalAlert(47);
			}
			return key;
		}

		// Token: 0x04001E7F RID: 7807
		protected TlsPskIdentity mPskIdentity;

		// Token: 0x04001E80 RID: 7808
		protected TlsPskIdentityManager mPskIdentityManager;

		// Token: 0x04001E81 RID: 7809
		protected TlsDHVerifier mDHVerifier;

		// Token: 0x04001E82 RID: 7810
		protected DHParameters mDHParameters;

		// Token: 0x04001E83 RID: 7811
		protected int[] mNamedCurves;

		// Token: 0x04001E84 RID: 7812
		protected byte[] mClientECPointFormats;

		// Token: 0x04001E85 RID: 7813
		protected byte[] mServerECPointFormats;

		// Token: 0x04001E86 RID: 7814
		protected byte[] mPskIdentityHint;

		// Token: 0x04001E87 RID: 7815
		protected byte[] mPsk;

		// Token: 0x04001E88 RID: 7816
		protected DHPrivateKeyParameters mDHAgreePrivateKey;

		// Token: 0x04001E89 RID: 7817
		protected DHPublicKeyParameters mDHAgreePublicKey;

		// Token: 0x04001E8A RID: 7818
		protected ECPrivateKeyParameters mECAgreePrivateKey;

		// Token: 0x04001E8B RID: 7819
		protected ECPublicKeyParameters mECAgreePublicKey;

		// Token: 0x04001E8C RID: 7820
		protected AsymmetricKeyParameter mServerPublicKey;

		// Token: 0x04001E8D RID: 7821
		protected RsaKeyParameters mRsaServerPublicKey;

		// Token: 0x04001E8E RID: 7822
		protected TlsEncryptionCredentials mServerCredentials;

		// Token: 0x04001E8F RID: 7823
		protected byte[] mPremasterSecret;
	}
}
