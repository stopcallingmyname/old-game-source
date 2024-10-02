using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200047B RID: 1147
	public class TlsRsaKeyExchange : AbstractTlsKeyExchange
	{
		// Token: 0x06002CE3 RID: 11491 RVA: 0x0011A289 File Offset: 0x00118489
		public TlsRsaKeyExchange(IList supportedSignatureAlgorithms) : base(1, supportedSignatureAlgorithms)
		{
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x00119FB5 File Offset: 0x001181B5
		public override void SkipServerCredentials()
		{
			throw new TlsFatalAlert(10);
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x0011A293 File Offset: 0x00118493
		public override void ProcessServerCredentials(TlsCredentials serverCredentials)
		{
			if (!(serverCredentials is TlsEncryptionCredentials))
			{
				throw new TlsFatalAlert(80);
			}
			this.ProcessServerCertificate(serverCredentials.Certificate);
			this.mServerCredentials = (TlsEncryptionCredentials)serverCredentials;
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x0011A2C0 File Offset: 0x001184C0
		public override void ProcessServerCertificate(Certificate serverCertificate)
		{
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

		// Token: 0x06002CE7 RID: 11495 RVA: 0x0011A350 File Offset: 0x00118550
		public override void ValidateCertificateRequest(CertificateRequest certificateRequest)
		{
			foreach (byte b in certificateRequest.CertificateTypes)
			{
				if (b - 1 > 1 && b != 64)
				{
					throw new TlsFatalAlert(47);
				}
			}
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x00117B45 File Offset: 0x00115D45
		public override void ProcessClientCredentials(TlsCredentials clientCredentials)
		{
			if (!(clientCredentials is TlsSignerCredentials))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x0011A389 File Offset: 0x00118589
		public override void GenerateClientKeyExchange(Stream output)
		{
			this.mPremasterSecret = TlsRsaUtilities.GenerateEncryptedPreMasterSecret(this.mContext, this.mRsaServerPublicKey, output);
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x0011A3A4 File Offset: 0x001185A4
		public override void ProcessClientKeyExchange(Stream input)
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

		// Token: 0x06002CEB RID: 11499 RVA: 0x0011A3E0 File Offset: 0x001185E0
		public override byte[] GeneratePremasterSecret()
		{
			if (this.mPremasterSecret == null)
			{
				throw new TlsFatalAlert(80);
			}
			byte[] result = this.mPremasterSecret;
			this.mPremasterSecret = null;
			return result;
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x0011A270 File Offset: 0x00118470
		protected virtual RsaKeyParameters ValidateRsaPublicKey(RsaKeyParameters key)
		{
			if (!key.Exponent.IsProbablePrime(2))
			{
				throw new TlsFatalAlert(47);
			}
			return key;
		}

		// Token: 0x04001E90 RID: 7824
		protected AsymmetricKeyParameter mServerPublicKey;

		// Token: 0x04001E91 RID: 7825
		protected RsaKeyParameters mRsaServerPublicKey;

		// Token: 0x04001E92 RID: 7826
		protected TlsEncryptionCredentials mServerCredentials;

		// Token: 0x04001E93 RID: 7827
		protected byte[] mPremasterSecret;
	}
}
