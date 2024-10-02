using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000552 RID: 1362
	public class ElGamalKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		// Token: 0x06003367 RID: 13159 RVA: 0x001345AD File Offset: 0x001327AD
		public void Init(KeyGenerationParameters parameters)
		{
			this.param = (ElGamalKeyGenerationParameters)parameters;
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x001345BC File Offset: 0x001327BC
		public AsymmetricCipherKeyPair GenerateKeyPair()
		{
			DHKeyGeneratorHelper instance = DHKeyGeneratorHelper.Instance;
			ElGamalParameters parameters = this.param.Parameters;
			DHParameters dhParams = new DHParameters(parameters.P, parameters.G, null, 0, parameters.L);
			BigInteger x = instance.CalculatePrivate(dhParams, this.param.Random);
			return new AsymmetricCipherKeyPair(new ElGamalPublicKeyParameters(instance.CalculatePublic(dhParams, x), parameters), new ElGamalPrivateKeyParameters(x, parameters));
		}

		// Token: 0x040021A9 RID: 8617
		private ElGamalKeyGenerationParameters param;
	}
}
