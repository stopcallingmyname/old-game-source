using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x0200054D RID: 1357
	public class DsaKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		// Token: 0x06003346 RID: 13126 RVA: 0x00133ACC File Offset: 0x00131CCC
		public void Init(KeyGenerationParameters parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			this.param = (DsaKeyGenerationParameters)parameters;
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x00133AE8 File Offset: 0x00131CE8
		public AsymmetricCipherKeyPair GenerateKeyPair()
		{
			DsaParameters parameters = this.param.Parameters;
			BigInteger x = DsaKeyPairGenerator.GeneratePrivateKey(parameters.Q, this.param.Random);
			return new AsymmetricCipherKeyPair(new DsaPublicKeyParameters(DsaKeyPairGenerator.CalculatePublicKey(parameters.P, parameters.G, x), parameters), new DsaPrivateKeyParameters(x, parameters));
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x00133B3C File Offset: 0x00131D3C
		private static BigInteger GeneratePrivateKey(BigInteger q, SecureRandom random)
		{
			int num = q.BitLength >> 2;
			BigInteger bigInteger;
			do
			{
				bigInteger = BigIntegers.CreateRandomInRange(DsaKeyPairGenerator.One, q.Subtract(DsaKeyPairGenerator.One), random);
			}
			while (WNafUtilities.GetNafWeight(bigInteger) < num);
			return bigInteger;
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x00133B73 File Offset: 0x00131D73
		private static BigInteger CalculatePublicKey(BigInteger p, BigInteger g, BigInteger x)
		{
			return g.ModPow(x, p);
		}

		// Token: 0x0400219A RID: 8602
		private static readonly BigInteger One = BigInteger.One;

		// Token: 0x0400219B RID: 8603
		private DsaKeyGenerationParameters param;
	}
}
