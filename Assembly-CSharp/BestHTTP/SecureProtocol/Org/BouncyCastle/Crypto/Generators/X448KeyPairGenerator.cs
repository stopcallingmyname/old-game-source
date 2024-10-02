using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000565 RID: 1381
	public class X448KeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		// Token: 0x060033D0 RID: 13264 RVA: 0x001372CD File Offset: 0x001354CD
		public virtual void Init(KeyGenerationParameters parameters)
		{
			this.random = parameters.Random;
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x001372DC File Offset: 0x001354DC
		public virtual AsymmetricCipherKeyPair GenerateKeyPair()
		{
			X448PrivateKeyParameters x448PrivateKeyParameters = new X448PrivateKeyParameters(this.random);
			return new AsymmetricCipherKeyPair(x448PrivateKeyParameters.GeneratePublicKey(), x448PrivateKeyParameters);
		}

		// Token: 0x040021D5 RID: 8661
		private SecureRandom random;
	}
}
