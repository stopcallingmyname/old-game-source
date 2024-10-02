using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000418 RID: 1048
	public class DefaultTlsEncryptionCredentials : AbstractTlsEncryptionCredentials
	{
		// Token: 0x060029F5 RID: 10741 RVA: 0x0010F86C File Offset: 0x0010DA6C
		public DefaultTlsEncryptionCredentials(TlsContext context, Certificate certificate, AsymmetricKeyParameter privateKey)
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
				throw new ArgumentNullException("'privateKey' cannot be null");
			}
			if (!privateKey.IsPrivate)
			{
				throw new ArgumentException("must be private", "privateKey");
			}
			if (!(privateKey is RsaKeyParameters))
			{
				throw new ArgumentException("type not supported: " + Platform.GetTypeName(privateKey), "privateKey");
			}
			this.mContext = context;
			this.mCertificate = certificate;
			this.mPrivateKey = privateKey;
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x060029F6 RID: 10742 RVA: 0x0010F903 File Offset: 0x0010DB03
		public override Certificate Certificate
		{
			get
			{
				return this.mCertificate;
			}
		}

		// Token: 0x060029F7 RID: 10743 RVA: 0x0010F90B File Offset: 0x0010DB0B
		public override byte[] DecryptPreMasterSecret(byte[] encryptedPreMasterSecret)
		{
			return TlsRsaUtilities.SafeDecryptPreMasterSecret(this.mContext, (RsaKeyParameters)this.mPrivateKey, encryptedPreMasterSecret);
		}

		// Token: 0x04001CB0 RID: 7344
		protected readonly TlsContext mContext;

		// Token: 0x04001CB1 RID: 7345
		protected readonly Certificate mCertificate;

		// Token: 0x04001CB2 RID: 7346
		protected readonly AsymmetricKeyParameter mPrivateKey;
	}
}
