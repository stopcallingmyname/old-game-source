using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F9 RID: 1017
	public abstract class AbstractTlsSigner : TlsSigner
	{
		// Token: 0x06002938 RID: 10552 RVA: 0x0010DCA6 File Offset: 0x0010BEA6
		public virtual void Init(TlsContext context)
		{
			this.mContext = context;
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x0010DCAF File Offset: 0x0010BEAF
		public virtual byte[] GenerateRawSignature(AsymmetricKeyParameter privateKey, byte[] md5AndSha1)
		{
			return this.GenerateRawSignature(null, privateKey, md5AndSha1);
		}

		// Token: 0x0600293A RID: 10554
		public abstract byte[] GenerateRawSignature(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey, byte[] hash);

		// Token: 0x0600293B RID: 10555 RVA: 0x0010DCBA File Offset: 0x0010BEBA
		public virtual bool VerifyRawSignature(byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] md5AndSha1)
		{
			return this.VerifyRawSignature(null, sigBytes, publicKey, md5AndSha1);
		}

		// Token: 0x0600293C RID: 10556
		public abstract bool VerifyRawSignature(SignatureAndHashAlgorithm algorithm, byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] hash);

		// Token: 0x0600293D RID: 10557 RVA: 0x0010DCC6 File Offset: 0x0010BEC6
		public virtual ISigner CreateSigner(AsymmetricKeyParameter privateKey)
		{
			return this.CreateSigner(null, privateKey);
		}

		// Token: 0x0600293E RID: 10558
		public abstract ISigner CreateSigner(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey);

		// Token: 0x0600293F RID: 10559 RVA: 0x0010DCD0 File Offset: 0x0010BED0
		public virtual ISigner CreateVerifyer(AsymmetricKeyParameter publicKey)
		{
			return this.CreateVerifyer(null, publicKey);
		}

		// Token: 0x06002940 RID: 10560
		public abstract ISigner CreateVerifyer(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter publicKey);

		// Token: 0x06002941 RID: 10561
		public abstract bool IsValidPublicKey(AsymmetricKeyParameter publicKey);

		// Token: 0x04001B33 RID: 6963
		protected TlsContext mContext;
	}
}
