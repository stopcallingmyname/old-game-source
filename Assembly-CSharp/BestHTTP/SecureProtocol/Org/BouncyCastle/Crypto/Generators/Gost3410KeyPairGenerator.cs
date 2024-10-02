using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000554 RID: 1364
	public class Gost3410KeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		// Token: 0x0600336D RID: 13165 RVA: 0x00134678 File Offset: 0x00132878
		public void Init(KeyGenerationParameters parameters)
		{
			if (parameters is Gost3410KeyGenerationParameters)
			{
				this.param = (Gost3410KeyGenerationParameters)parameters;
				return;
			}
			Gost3410KeyGenerationParameters gost3410KeyGenerationParameters = new Gost3410KeyGenerationParameters(parameters.Random, CryptoProObjectIdentifiers.GostR3410x94CryptoProA);
			int strength = parameters.Strength;
			int num = gost3410KeyGenerationParameters.Parameters.P.BitLength - 1;
			this.param = gost3410KeyGenerationParameters;
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x001346CC File Offset: 0x001328CC
		public AsymmetricCipherKeyPair GenerateKeyPair()
		{
			SecureRandom random = this.param.Random;
			Gost3410Parameters parameters = this.param.Parameters;
			BigInteger q = parameters.Q;
			int num = 64;
			BigInteger bigInteger;
			do
			{
				bigInteger = new BigInteger(256, random);
			}
			while (bigInteger.SignValue < 1 || bigInteger.CompareTo(q) >= 0 || WNafUtilities.GetNafWeight(bigInteger) < num);
			BigInteger p = parameters.P;
			BigInteger y = parameters.A.ModPow(bigInteger, p);
			if (this.param.PublicKeyParamSet != null)
			{
				return new AsymmetricCipherKeyPair(new Gost3410PublicKeyParameters(y, this.param.PublicKeyParamSet), new Gost3410PrivateKeyParameters(bigInteger, this.param.PublicKeyParamSet));
			}
			return new AsymmetricCipherKeyPair(new Gost3410PublicKeyParameters(y, parameters), new Gost3410PrivateKeyParameters(bigInteger, parameters));
		}

		// Token: 0x040021AD RID: 8621
		private Gost3410KeyGenerationParameters param;
	}
}
