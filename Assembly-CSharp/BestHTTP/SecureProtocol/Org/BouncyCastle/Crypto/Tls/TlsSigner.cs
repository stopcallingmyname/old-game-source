using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000484 RID: 1156
	public interface TlsSigner
	{
		// Token: 0x06002D2B RID: 11563
		void Init(TlsContext context);

		// Token: 0x06002D2C RID: 11564
		byte[] GenerateRawSignature(AsymmetricKeyParameter privateKey, byte[] md5AndSha1);

		// Token: 0x06002D2D RID: 11565
		byte[] GenerateRawSignature(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey, byte[] hash);

		// Token: 0x06002D2E RID: 11566
		bool VerifyRawSignature(byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] md5AndSha1);

		// Token: 0x06002D2F RID: 11567
		bool VerifyRawSignature(SignatureAndHashAlgorithm algorithm, byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] hash);

		// Token: 0x06002D30 RID: 11568
		ISigner CreateSigner(AsymmetricKeyParameter privateKey);

		// Token: 0x06002D31 RID: 11569
		ISigner CreateSigner(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey);

		// Token: 0x06002D32 RID: 11570
		ISigner CreateVerifyer(AsymmetricKeyParameter publicKey);

		// Token: 0x06002D33 RID: 11571
		ISigner CreateVerifyer(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter publicKey);

		// Token: 0x06002D34 RID: 11572
		bool IsValidPublicKey(AsymmetricKeyParameter publicKey);
	}
}
