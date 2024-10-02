using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000562 RID: 1378
	public class RsaKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		// Token: 0x060033BE RID: 13246 RVA: 0x00136CC5 File Offset: 0x00134EC5
		public virtual void Init(KeyGenerationParameters parameters)
		{
			if (parameters is RsaKeyGenerationParameters)
			{
				this.parameters = (RsaKeyGenerationParameters)parameters;
				return;
			}
			this.parameters = new RsaKeyGenerationParameters(RsaKeyPairGenerator.DefaultPublicExponent, parameters.Random, parameters.Strength, 100);
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x00136CFC File Offset: 0x00134EFC
		public virtual AsymmetricCipherKeyPair GenerateKeyPair()
		{
			int num2;
			BigInteger publicExponent;
			BigInteger bigInteger;
			BigInteger bigInteger2;
			BigInteger bigInteger3;
			BigInteger bigInteger5;
			BigInteger bigInteger6;
			BigInteger bigInteger7;
			do
			{
				int strength = this.parameters.Strength;
				int num = (strength + 1) / 2;
				num2 = strength - num;
				int num3 = strength / 3;
				int num4 = strength >> 2;
				publicExponent = this.parameters.PublicExponent;
				bigInteger = this.ChooseRandomPrime(num, publicExponent);
				for (;;)
				{
					bigInteger2 = this.ChooseRandomPrime(num2, publicExponent);
					if (bigInteger2.Subtract(bigInteger).Abs().BitLength >= num3)
					{
						bigInteger3 = bigInteger.Multiply(bigInteger2);
						if (bigInteger3.BitLength != strength)
						{
							bigInteger = bigInteger.Max(bigInteger2);
						}
						else
						{
							if (WNafUtilities.GetNafWeight(bigInteger3) >= num4)
							{
								break;
							}
							bigInteger = this.ChooseRandomPrime(num, publicExponent);
						}
					}
				}
				if (bigInteger.CompareTo(bigInteger2) < 0)
				{
					BigInteger bigInteger4 = bigInteger;
					bigInteger = bigInteger2;
					bigInteger2 = bigInteger4;
				}
				bigInteger5 = bigInteger.Subtract(RsaKeyPairGenerator.One);
				bigInteger6 = bigInteger2.Subtract(RsaKeyPairGenerator.One);
				BigInteger val = bigInteger5.Gcd(bigInteger6);
				BigInteger m = bigInteger5.Divide(val).Multiply(bigInteger6);
				bigInteger7 = publicExponent.ModInverse(m);
			}
			while (bigInteger7.BitLength <= num2);
			BigInteger dP = bigInteger7.Remainder(bigInteger5);
			BigInteger dQ = bigInteger7.Remainder(bigInteger6);
			BigInteger qInv = bigInteger2.ModInverse(bigInteger);
			return new AsymmetricCipherKeyPair(new RsaKeyParameters(false, bigInteger3, publicExponent), new RsaPrivateCrtKeyParameters(bigInteger3, publicExponent, bigInteger7, bigInteger, bigInteger2, dP, dQ, qInv));
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x00136E48 File Offset: 0x00135048
		protected virtual BigInteger ChooseRandomPrime(int bitlength, BigInteger e)
		{
			bool flag = e.BitLength <= RsaKeyPairGenerator.SPECIAL_E_BITS && Arrays.Contains(RsaKeyPairGenerator.SPECIAL_E_VALUES, e.IntValue);
			BigInteger bigInteger;
			do
			{
				bigInteger = new BigInteger(bitlength, 1, this.parameters.Random);
			}
			while (bigInteger.Mod(e).Equals(RsaKeyPairGenerator.One) || !bigInteger.IsProbablePrime(this.parameters.Certainty, true) || (!flag && !e.Gcd(bigInteger.Subtract(RsaKeyPairGenerator.One)).Equals(RsaKeyPairGenerator.One)));
			return bigInteger;
		}

		// Token: 0x040021CD RID: 8653
		private static readonly int[] SPECIAL_E_VALUES = new int[]
		{
			3,
			5,
			17,
			257,
			65537
		};

		// Token: 0x040021CE RID: 8654
		private static readonly int SPECIAL_E_HIGHEST = RsaKeyPairGenerator.SPECIAL_E_VALUES[RsaKeyPairGenerator.SPECIAL_E_VALUES.Length - 1];

		// Token: 0x040021CF RID: 8655
		private static readonly int SPECIAL_E_BITS = BigInteger.ValueOf((long)RsaKeyPairGenerator.SPECIAL_E_HIGHEST).BitLength;

		// Token: 0x040021D0 RID: 8656
		protected static readonly BigInteger One = BigInteger.One;

		// Token: 0x040021D1 RID: 8657
		protected static readonly BigInteger DefaultPublicExponent = BigInteger.ValueOf(65537L);

		// Token: 0x040021D2 RID: 8658
		protected const int DefaultTests = 100;

		// Token: 0x040021D3 RID: 8659
		protected RsaKeyGenerationParameters parameters;
	}
}
