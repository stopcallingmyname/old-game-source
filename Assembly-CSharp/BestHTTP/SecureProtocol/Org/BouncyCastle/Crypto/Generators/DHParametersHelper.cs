﻿using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x0200054C RID: 1356
	internal class DHParametersHelper
	{
		// Token: 0x06003341 RID: 13121 RVA: 0x001338B0 File Offset: 0x00131AB0
		private static BigInteger[] ConstructBigPrimeProducts(int[] primeProducts)
		{
			BigInteger[] array = new BigInteger[primeProducts.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = BigInteger.ValueOf((long)primeProducts[i]);
			}
			return array;
		}

		// Token: 0x06003342 RID: 13122 RVA: 0x001338E4 File Offset: 0x00131AE4
		internal static BigInteger[] GenerateSafePrimes(int size, int certainty, SecureRandom random)
		{
			int num = size - 1;
			int num2 = size >> 2;
			BigInteger bigInteger;
			BigInteger bigInteger2;
			if (size <= 32)
			{
				for (;;)
				{
					bigInteger = new BigInteger(num, 2, random);
					bigInteger2 = bigInteger.ShiftLeft(1).Add(BigInteger.One);
					if (bigInteger2.IsProbablePrime(certainty, true))
					{
						if (certainty <= 2 || bigInteger.IsProbablePrime(certainty, true))
						{
							break;
						}
					}
				}
			}
			else
			{
				for (;;)
				{
					bigInteger = new BigInteger(num, 0, random);
					for (;;)
					{
						IL_51:
						for (int i = 0; i < DHParametersHelper.primeLists.Length; i++)
						{
							int num3 = bigInteger.Remainder(DHParametersHelper.BigPrimeProducts[i]).IntValue;
							if (i == 0)
							{
								int num4 = num3 % 3;
								if (num4 != 2)
								{
									int num5 = 2 * num4 + 2;
									bigInteger = bigInteger.Add(BigInteger.ValueOf((long)num5));
									num3 = (num3 + num5) % DHParametersHelper.primeProducts[i];
								}
							}
							foreach (int num6 in DHParametersHelper.primeLists[i])
							{
								int num7 = num3 % num6;
								if (num7 == 0 || num7 == num6 >> 1)
								{
									bigInteger = bigInteger.Add(DHParametersHelper.Six);
									goto IL_51;
								}
							}
						}
						break;
					}
					if (bigInteger.BitLength == num && bigInteger.RabinMillerTest(2, random, true))
					{
						bigInteger2 = bigInteger.ShiftLeft(1).Add(BigInteger.One);
						if (bigInteger2.RabinMillerTest(certainty, random, true) && (certainty <= 2 || bigInteger.RabinMillerTest(certainty - 2, random, true)) && WNafUtilities.GetNafWeight(bigInteger2) >= num2)
						{
							break;
						}
					}
				}
			}
			return new BigInteger[]
			{
				bigInteger2,
				bigInteger
			};
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x00133A5C File Offset: 0x00131C5C
		internal static BigInteger SelectGenerator(BigInteger p, BigInteger q, SecureRandom random)
		{
			BigInteger max = p.Subtract(BigInteger.Two);
			BigInteger bigInteger;
			do
			{
				bigInteger = BigIntegers.CreateRandomInRange(BigInteger.Two, max, random).ModPow(BigInteger.Two, p);
			}
			while (bigInteger.Equals(BigInteger.One));
			return bigInteger;
		}

		// Token: 0x04002196 RID: 8598
		private static readonly BigInteger Six = BigInteger.ValueOf(6L);

		// Token: 0x04002197 RID: 8599
		private static readonly int[][] primeLists = BigInteger.primeLists;

		// Token: 0x04002198 RID: 8600
		private static readonly int[] primeProducts = BigInteger.primeProducts;

		// Token: 0x04002199 RID: 8601
		private static readonly BigInteger[] BigPrimeProducts = DHParametersHelper.ConstructBigPrimeProducts(DHParametersHelper.primeProducts);
	}
}
