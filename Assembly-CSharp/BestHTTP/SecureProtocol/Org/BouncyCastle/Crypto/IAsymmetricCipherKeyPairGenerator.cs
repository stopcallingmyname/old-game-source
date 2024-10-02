using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003CF RID: 975
	public interface IAsymmetricCipherKeyPairGenerator
	{
		// Token: 0x0600281C RID: 10268
		void Init(KeyGenerationParameters parameters);

		// Token: 0x0600281D RID: 10269
		AsymmetricCipherKeyPair GenerateKeyPair();
	}
}
