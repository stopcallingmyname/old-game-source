using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000550 RID: 1360
	public class Ed25519KeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		// Token: 0x06003361 RID: 13153 RVA: 0x00134543 File Offset: 0x00132743
		public virtual void Init(KeyGenerationParameters parameters)
		{
			this.random = parameters.Random;
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x00134554 File Offset: 0x00132754
		public virtual AsymmetricCipherKeyPair GenerateKeyPair()
		{
			Ed25519PrivateKeyParameters ed25519PrivateKeyParameters = new Ed25519PrivateKeyParameters(this.random);
			return new AsymmetricCipherKeyPair(ed25519PrivateKeyParameters.GeneratePublicKey(), ed25519PrivateKeyParameters);
		}

		// Token: 0x040021A7 RID: 8615
		private SecureRandom random;
	}
}
