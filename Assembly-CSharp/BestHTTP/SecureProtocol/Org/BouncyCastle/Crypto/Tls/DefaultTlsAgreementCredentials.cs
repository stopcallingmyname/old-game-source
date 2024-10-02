using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000414 RID: 1044
	public class DefaultTlsAgreementCredentials : AbstractTlsAgreementCredentials
	{
		// Token: 0x060029C4 RID: 10692 RVA: 0x0010F0FC File Offset: 0x0010D2FC
		public DefaultTlsAgreementCredentials(Certificate certificate, AsymmetricKeyParameter privateKey)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			if (certificate.IsEmpty)
			{
				throw new ArgumentException("cannot be empty", "certificate");
			}
			if (privateKey == null)
			{
				throw new ArgumentNullException("privateKey");
			}
			if (!privateKey.IsPrivate)
			{
				throw new ArgumentException("must be private", "privateKey");
			}
			if (privateKey is DHPrivateKeyParameters)
			{
				this.mBasicAgreement = new DHBasicAgreement();
				this.mTruncateAgreement = true;
			}
			else
			{
				if (!(privateKey is ECPrivateKeyParameters))
				{
					throw new ArgumentException("type not supported: " + Platform.GetTypeName(privateKey), "privateKey");
				}
				this.mBasicAgreement = new ECDHBasicAgreement();
				this.mTruncateAgreement = false;
			}
			this.mCertificate = certificate;
			this.mPrivateKey = privateKey;
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x060029C5 RID: 10693 RVA: 0x0010F1BC File Offset: 0x0010D3BC
		public override Certificate Certificate
		{
			get
			{
				return this.mCertificate;
			}
		}

		// Token: 0x060029C6 RID: 10694 RVA: 0x0010F1C4 File Offset: 0x0010D3C4
		public override byte[] GenerateAgreement(AsymmetricKeyParameter peerPublicKey)
		{
			this.mBasicAgreement.Init(this.mPrivateKey);
			BigInteger n = this.mBasicAgreement.CalculateAgreement(peerPublicKey);
			if (this.mTruncateAgreement)
			{
				return BigIntegers.AsUnsignedByteArray(n);
			}
			return BigIntegers.AsUnsignedByteArray(this.mBasicAgreement.GetFieldSize(), n);
		}

		// Token: 0x04001CA7 RID: 7335
		protected readonly Certificate mCertificate;

		// Token: 0x04001CA8 RID: 7336
		protected readonly AsymmetricKeyParameter mPrivateKey;

		// Token: 0x04001CA9 RID: 7337
		protected readonly IBasicAgreement mBasicAgreement;

		// Token: 0x04001CAA RID: 7338
		protected readonly bool mTruncateAgreement;
	}
}
