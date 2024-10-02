using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000564 RID: 1380
	public class X25519KeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		// Token: 0x060033CD RID: 13261 RVA: 0x00137297 File Offset: 0x00135497
		public virtual void Init(KeyGenerationParameters parameters)
		{
			this.random = parameters.Random;
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x001372A8 File Offset: 0x001354A8
		public virtual AsymmetricCipherKeyPair GenerateKeyPair()
		{
			X25519PrivateKeyParameters x25519PrivateKeyParameters = new X25519PrivateKeyParameters(this.random);
			return new AsymmetricCipherKeyPair(x25519PrivateKeyParameters.GeneratePublicKey(), x25519PrivateKeyParameters);
		}

		// Token: 0x040021D4 RID: 8660
		private SecureRandom random;
	}
}
