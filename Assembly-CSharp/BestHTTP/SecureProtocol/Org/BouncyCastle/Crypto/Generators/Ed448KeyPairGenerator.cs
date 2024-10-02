using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000551 RID: 1361
	public class Ed448KeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		// Token: 0x06003364 RID: 13156 RVA: 0x00134579 File Offset: 0x00132779
		public virtual void Init(KeyGenerationParameters parameters)
		{
			this.random = parameters.Random;
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x00134588 File Offset: 0x00132788
		public virtual AsymmetricCipherKeyPair GenerateKeyPair()
		{
			Ed448PrivateKeyParameters ed448PrivateKeyParameters = new Ed448PrivateKeyParameters(this.random);
			return new AsymmetricCipherKeyPair(ed448PrivateKeyParameters.GeneratePublicKey(), ed448PrivateKeyParameters);
		}

		// Token: 0x040021A8 RID: 8616
		private SecureRandom random;
	}
}
