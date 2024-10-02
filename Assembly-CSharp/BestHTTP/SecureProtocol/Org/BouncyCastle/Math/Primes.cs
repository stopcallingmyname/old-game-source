using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math
{
	// Token: 0x02000306 RID: 774
	public abstract class Primes
	{
		// Token: 0x06001C62 RID: 7266 RVA: 0x000D581C File Offset: 0x000D3A1C
		public static Primes.STOutput GenerateSTRandomPrime(IDigest hash, int length, byte[] inputSeed)
		{
			if (hash == null)
			{
				throw new ArgumentNullException("hash");
			}
			if (length < 2)
			{
				throw new ArgumentException("must be >= 2", "length");
			}
			if (inputSeed == null)
			{
				throw new ArgumentNullException("inputSeed");
			}
			if (inputSeed.Length == 0)
			{
				throw new ArgumentException("cannot be empty", "inputSeed");
			}
			return Primes.ImplSTRandomPrime(hash, length, Arrays.Clone(inputSeed));
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x000D587C File Offset: 0x000D3A7C
		public static Primes.MROutput EnhancedMRProbablePrimeTest(BigInteger candidate, SecureRandom random, int iterations)
		{
			Primes.CheckCandidate(candidate, "candidate");
			if (random == null)
			{
				throw new ArgumentNullException("random");
			}
			if (iterations < 1)
			{
				throw new ArgumentException("must be > 0", "iterations");
			}
			if (candidate.BitLength == 2)
			{
				return Primes.MROutput.ProbablyPrime();
			}
			if (!candidate.TestBit(0))
			{
				return Primes.MROutput.ProvablyCompositeWithFactor(Primes.Two);
			}
			BigInteger bigInteger = candidate.Subtract(Primes.One);
			BigInteger max = candidate.Subtract(Primes.Two);
			int lowestSetBit = bigInteger.GetLowestSetBit();
			BigInteger e = bigInteger.ShiftRight(lowestSetBit);
			for (int i = 0; i < iterations; i++)
			{
				BigInteger bigInteger2 = BigIntegers.CreateRandomInRange(Primes.Two, max, random);
				BigInteger bigInteger3 = bigInteger2.Gcd(candidate);
				if (bigInteger3.CompareTo(Primes.One) > 0)
				{
					return Primes.MROutput.ProvablyCompositeWithFactor(bigInteger3);
				}
				BigInteger bigInteger4 = bigInteger2.ModPow(e, candidate);
				if (!bigInteger4.Equals(Primes.One) && !bigInteger4.Equals(bigInteger))
				{
					bool flag = false;
					BigInteger bigInteger5 = bigInteger4;
					for (int j = 1; j < lowestSetBit; j++)
					{
						bigInteger4 = bigInteger4.ModPow(Primes.Two, candidate);
						if (bigInteger4.Equals(bigInteger))
						{
							flag = true;
							break;
						}
						if (bigInteger4.Equals(Primes.One))
						{
							break;
						}
						bigInteger5 = bigInteger4;
					}
					if (!flag)
					{
						if (!bigInteger4.Equals(Primes.One))
						{
							bigInteger5 = bigInteger4;
							bigInteger4 = bigInteger4.ModPow(Primes.Two, candidate);
							if (!bigInteger4.Equals(Primes.One))
							{
								bigInteger5 = bigInteger4;
							}
						}
						bigInteger3 = bigInteger5.Subtract(Primes.One).Gcd(candidate);
						if (bigInteger3.CompareTo(Primes.One) > 0)
						{
							return Primes.MROutput.ProvablyCompositeWithFactor(bigInteger3);
						}
						return Primes.MROutput.ProvablyCompositeNotPrimePower();
					}
				}
			}
			return Primes.MROutput.ProbablyPrime();
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x000D5A26 File Offset: 0x000D3C26
		public static bool HasAnySmallFactors(BigInteger candidate)
		{
			Primes.CheckCandidate(candidate, "candidate");
			return Primes.ImplHasAnySmallFactors(candidate);
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x000D5A3C File Offset: 0x000D3C3C
		public static bool IsMRProbablePrime(BigInteger candidate, SecureRandom random, int iterations)
		{
			Primes.CheckCandidate(candidate, "candidate");
			if (random == null)
			{
				throw new ArgumentException("cannot be null", "random");
			}
			if (iterations < 1)
			{
				throw new ArgumentException("must be > 0", "iterations");
			}
			if (candidate.BitLength == 2)
			{
				return true;
			}
			if (!candidate.TestBit(0))
			{
				return false;
			}
			BigInteger bigInteger = candidate.Subtract(Primes.One);
			BigInteger max = candidate.Subtract(Primes.Two);
			int lowestSetBit = bigInteger.GetLowestSetBit();
			BigInteger m = bigInteger.ShiftRight(lowestSetBit);
			for (int i = 0; i < iterations; i++)
			{
				BigInteger b = BigIntegers.CreateRandomInRange(Primes.Two, max, random);
				if (!Primes.ImplMRProbablePrimeToBase(candidate, bigInteger, m, lowestSetBit, b))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x000D5AEC File Offset: 0x000D3CEC
		public static bool IsMRProbablePrimeToBase(BigInteger candidate, BigInteger baseValue)
		{
			Primes.CheckCandidate(candidate, "candidate");
			Primes.CheckCandidate(baseValue, "baseValue");
			if (baseValue.CompareTo(candidate.Subtract(Primes.One)) >= 0)
			{
				throw new ArgumentException("must be < ('candidate' - 1)", "baseValue");
			}
			if (candidate.BitLength == 2)
			{
				return true;
			}
			BigInteger bigInteger = candidate.Subtract(Primes.One);
			int lowestSetBit = bigInteger.GetLowestSetBit();
			BigInteger m = bigInteger.ShiftRight(lowestSetBit);
			return Primes.ImplMRProbablePrimeToBase(candidate, bigInteger, m, lowestSetBit, baseValue);
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x000D5B63 File Offset: 0x000D3D63
		private static void CheckCandidate(BigInteger n, string name)
		{
			if (n == null || n.SignValue < 1 || n.BitLength < 2)
			{
				throw new ArgumentException("must be non-null and >= 2", name);
			}
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000D5B88 File Offset: 0x000D3D88
		private static bool ImplHasAnySmallFactors(BigInteger x)
		{
			int num = 223092870;
			int intValue = x.Mod(BigInteger.ValueOf((long)num)).IntValue;
			if (intValue % 2 == 0 || intValue % 3 == 0 || intValue % 5 == 0 || intValue % 7 == 0 || intValue % 11 == 0 || intValue % 13 == 0 || intValue % 17 == 0 || intValue % 19 == 0 || intValue % 23 == 0)
			{
				return true;
			}
			num = 58642669;
			intValue = x.Mod(BigInteger.ValueOf((long)num)).IntValue;
			if (intValue % 29 == 0 || intValue % 31 == 0 || intValue % 37 == 0 || intValue % 41 == 0 || intValue % 43 == 0)
			{
				return true;
			}
			num = 600662303;
			intValue = x.Mod(BigInteger.ValueOf((long)num)).IntValue;
			if (intValue % 47 == 0 || intValue % 53 == 0 || intValue % 59 == 0 || intValue % 61 == 0 || intValue % 67 == 0)
			{
				return true;
			}
			num = 33984931;
			intValue = x.Mod(BigInteger.ValueOf((long)num)).IntValue;
			if (intValue % 71 == 0 || intValue % 73 == 0 || intValue % 79 == 0 || intValue % 83 == 0)
			{
				return true;
			}
			num = 89809099;
			intValue = x.Mod(BigInteger.ValueOf((long)num)).IntValue;
			if (intValue % 89 == 0 || intValue % 97 == 0 || intValue % 101 == 0 || intValue % 103 == 0)
			{
				return true;
			}
			num = 167375713;
			intValue = x.Mod(BigInteger.ValueOf((long)num)).IntValue;
			if (intValue % 107 == 0 || intValue % 109 == 0 || intValue % 113 == 0 || intValue % 127 == 0)
			{
				return true;
			}
			num = 371700317;
			intValue = x.Mod(BigInteger.ValueOf((long)num)).IntValue;
			if (intValue % 131 == 0 || intValue % 137 == 0 || intValue % 139 == 0 || intValue % 149 == 0)
			{
				return true;
			}
			num = 645328247;
			intValue = x.Mod(BigInteger.ValueOf((long)num)).IntValue;
			if (intValue % 151 == 0 || intValue % 157 == 0 || intValue % 163 == 0 || intValue % 167 == 0)
			{
				return true;
			}
			num = 1070560157;
			intValue = x.Mod(BigInteger.ValueOf((long)num)).IntValue;
			if (intValue % 173 == 0 || intValue % 179 == 0 || intValue % 181 == 0 || intValue % 191 == 0)
			{
				return true;
			}
			num = 1596463769;
			intValue = x.Mod(BigInteger.ValueOf((long)num)).IntValue;
			return intValue % 193 == 0 || intValue % 197 == 0 || intValue % 199 == 0 || intValue % 211 == 0;
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x000D5DEC File Offset: 0x000D3FEC
		private static bool ImplMRProbablePrimeToBase(BigInteger w, BigInteger wSubOne, BigInteger m, int a, BigInteger b)
		{
			BigInteger bigInteger = b.ModPow(m, w);
			if (bigInteger.Equals(Primes.One) || bigInteger.Equals(wSubOne))
			{
				return true;
			}
			bool result = false;
			for (int i = 1; i < a; i++)
			{
				bigInteger = bigInteger.ModPow(Primes.Two, w);
				if (bigInteger.Equals(wSubOne))
				{
					result = true;
					break;
				}
				if (bigInteger.Equals(Primes.One))
				{
					return false;
				}
			}
			return result;
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000D5E54 File Offset: 0x000D4054
		private static Primes.STOutput ImplSTRandomPrime(IDigest d, int length, byte[] primeSeed)
		{
			int digestSize = d.GetDigestSize();
			if (length < 33)
			{
				int num = 0;
				byte[] array = new byte[digestSize];
				byte[] array2 = new byte[digestSize];
				uint num2;
				for (;;)
				{
					Primes.Hash(d, primeSeed, array, 0);
					Primes.Inc(primeSeed, 1);
					Primes.Hash(d, primeSeed, array2, 0);
					Primes.Inc(primeSeed, 1);
					num2 = (Primes.Extract32(array) ^ Primes.Extract32(array2));
					num2 &= uint.MaxValue >> 32 - length;
					num2 |= (1U << length - 1 | 1U);
					num++;
					if (Primes.IsPrime32(num2))
					{
						break;
					}
					if (num > 4 * length)
					{
						goto Block_3;
					}
				}
				return new Primes.STOutput(BigInteger.ValueOf((long)((ulong)num2)), primeSeed, num);
				Block_3:
				throw new InvalidOperationException("Too many iterations in Shawe-Taylor Random_Prime Routine");
			}
			Primes.STOutput stoutput = Primes.ImplSTRandomPrime(d, (length + 3) / 2, primeSeed);
			BigInteger prime = stoutput.Prime;
			primeSeed = stoutput.PrimeSeed;
			int num3 = stoutput.PrimeGenCounter;
			int num4 = 8 * digestSize;
			int num5 = (length - 1) / num4;
			int num6 = num3;
			BigInteger bigInteger = Primes.HashGen(d, primeSeed, num5 + 1).Mod(Primes.One.ShiftLeft(length - 1)).SetBit(length - 1);
			BigInteger bigInteger2 = prime.ShiftLeft(1);
			BigInteger bigInteger3 = bigInteger.Subtract(Primes.One).Divide(bigInteger2).Add(Primes.One).ShiftLeft(1);
			int num7 = 0;
			BigInteger bigInteger4 = bigInteger3.Multiply(prime).Add(Primes.One);
			for (;;)
			{
				if (bigInteger4.BitLength > length)
				{
					bigInteger3 = Primes.One.ShiftLeft(length - 1).Subtract(Primes.One).Divide(bigInteger2).Add(Primes.One).ShiftLeft(1);
					bigInteger4 = bigInteger3.Multiply(prime).Add(Primes.One);
				}
				num3++;
				if (!Primes.ImplHasAnySmallFactors(bigInteger4))
				{
					BigInteger bigInteger5 = Primes.HashGen(d, primeSeed, num5 + 1).Mod(bigInteger4.Subtract(Primes.Three)).Add(Primes.Two);
					bigInteger3 = bigInteger3.Add(BigInteger.ValueOf((long)num7));
					num7 = 0;
					BigInteger bigInteger6 = bigInteger5.ModPow(bigInteger3, bigInteger4);
					if (bigInteger4.Gcd(bigInteger6.Subtract(Primes.One)).Equals(Primes.One) && bigInteger6.ModPow(prime, bigInteger4).Equals(Primes.One))
					{
						break;
					}
				}
				else
				{
					Primes.Inc(primeSeed, num5 + 1);
				}
				if (num3 >= 4 * length + num6)
				{
					goto Block_8;
				}
				num7 += 2;
				bigInteger4 = bigInteger4.Add(bigInteger2);
			}
			return new Primes.STOutput(bigInteger4, primeSeed, num3);
			Block_8:
			throw new InvalidOperationException("Too many iterations in Shawe-Taylor Random_Prime Routine");
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x000D60B4 File Offset: 0x000D42B4
		private static uint Extract32(byte[] bs)
		{
			uint num = 0U;
			int num2 = Math.Min(4, bs.Length);
			for (int i = 0; i < num2; i++)
			{
				uint num3 = (uint)bs[bs.Length - (i + 1)];
				num |= num3 << 8 * i;
			}
			return num;
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x000D60EF File Offset: 0x000D42EF
		private static void Hash(IDigest d, byte[] input, byte[] output, int outPos)
		{
			d.BlockUpdate(input, 0, input.Length);
			d.DoFinal(output, outPos);
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000D6108 File Offset: 0x000D4308
		private static BigInteger HashGen(IDigest d, byte[] seed, int count)
		{
			int digestSize = d.GetDigestSize();
			int num = count * digestSize;
			byte[] array = new byte[num];
			for (int i = 0; i < count; i++)
			{
				num -= digestSize;
				Primes.Hash(d, seed, array, num);
				Primes.Inc(seed, 1);
			}
			return new BigInteger(1, array);
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x000D6150 File Offset: 0x000D4350
		private static void Inc(byte[] seed, int c)
		{
			int num = seed.Length;
			while (c > 0 && --num >= 0)
			{
				c += (int)seed[num];
				seed[num] = (byte)c;
				c >>= 8;
			}
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x000D6180 File Offset: 0x000D4380
		private static bool IsPrime32(uint x)
		{
			if (x <= 5U)
			{
				return x == 2U || x == 3U || x == 5U;
			}
			if ((x & 1U) == 0U || x % 3U == 0U || x % 5U == 0U)
			{
				return false;
			}
			uint[] array = new uint[]
			{
				1U,
				7U,
				11U,
				13U,
				17U,
				19U,
				23U,
				29U
			};
			uint num = 0U;
			int num2 = 1;
			for (;;)
			{
				if (num2 >= array.Length)
				{
					num += 30U;
					if (num >> 16 != 0U || num * num >= x)
					{
						return true;
					}
					num2 = 0;
				}
				else
				{
					uint num3 = num + array[num2];
					if (x % num3 == 0U)
					{
						break;
					}
					num2++;
				}
			}
			return x < 30U;
		}

		// Token: 0x04001939 RID: 6457
		public static readonly int SmallFactorLimit = 211;

		// Token: 0x0400193A RID: 6458
		private static readonly BigInteger One = BigInteger.One;

		// Token: 0x0400193B RID: 6459
		private static readonly BigInteger Two = BigInteger.Two;

		// Token: 0x0400193C RID: 6460
		private static readonly BigInteger Three = BigInteger.Three;

		// Token: 0x0200090B RID: 2315
		public class MROutput
		{
			// Token: 0x06004E3C RID: 20028 RVA: 0x001B10A6 File Offset: 0x001AF2A6
			internal static Primes.MROutput ProbablyPrime()
			{
				return new Primes.MROutput(false, null);
			}

			// Token: 0x06004E3D RID: 20029 RVA: 0x001B10AF File Offset: 0x001AF2AF
			internal static Primes.MROutput ProvablyCompositeWithFactor(BigInteger factor)
			{
				return new Primes.MROutput(true, factor);
			}

			// Token: 0x06004E3E RID: 20030 RVA: 0x001B10B8 File Offset: 0x001AF2B8
			internal static Primes.MROutput ProvablyCompositeNotPrimePower()
			{
				return new Primes.MROutput(true, null);
			}

			// Token: 0x06004E3F RID: 20031 RVA: 0x001B10C1 File Offset: 0x001AF2C1
			private MROutput(bool provablyComposite, BigInteger factor)
			{
				this.mProvablyComposite = provablyComposite;
				this.mFactor = factor;
			}

			// Token: 0x17000C2B RID: 3115
			// (get) Token: 0x06004E40 RID: 20032 RVA: 0x001B10D7 File Offset: 0x001AF2D7
			public BigInteger Factor
			{
				get
				{
					return this.mFactor;
				}
			}

			// Token: 0x17000C2C RID: 3116
			// (get) Token: 0x06004E41 RID: 20033 RVA: 0x001B10DF File Offset: 0x001AF2DF
			public bool IsProvablyComposite
			{
				get
				{
					return this.mProvablyComposite;
				}
			}

			// Token: 0x17000C2D RID: 3117
			// (get) Token: 0x06004E42 RID: 20034 RVA: 0x001B10E7 File Offset: 0x001AF2E7
			public bool IsNotPrimePower
			{
				get
				{
					return this.mProvablyComposite && this.mFactor == null;
				}
			}

			// Token: 0x04003557 RID: 13655
			private readonly bool mProvablyComposite;

			// Token: 0x04003558 RID: 13656
			private readonly BigInteger mFactor;
		}

		// Token: 0x0200090C RID: 2316
		public class STOutput
		{
			// Token: 0x06004E43 RID: 20035 RVA: 0x001B10FC File Offset: 0x001AF2FC
			internal STOutput(BigInteger prime, byte[] primeSeed, int primeGenCounter)
			{
				this.mPrime = prime;
				this.mPrimeSeed = primeSeed;
				this.mPrimeGenCounter = primeGenCounter;
			}

			// Token: 0x17000C2E RID: 3118
			// (get) Token: 0x06004E44 RID: 20036 RVA: 0x001B1119 File Offset: 0x001AF319
			public BigInteger Prime
			{
				get
				{
					return this.mPrime;
				}
			}

			// Token: 0x17000C2F RID: 3119
			// (get) Token: 0x06004E45 RID: 20037 RVA: 0x001B1121 File Offset: 0x001AF321
			public byte[] PrimeSeed
			{
				get
				{
					return this.mPrimeSeed;
				}
			}

			// Token: 0x17000C30 RID: 3120
			// (get) Token: 0x06004E46 RID: 20038 RVA: 0x001B1129 File Offset: 0x001AF329
			public int PrimeGenCounter
			{
				get
				{
					return this.mPrimeGenCounter;
				}
			}

			// Token: 0x04003559 RID: 13657
			private readonly BigInteger mPrime;

			// Token: 0x0400355A RID: 13658
			private readonly byte[] mPrimeSeed;

			// Token: 0x0400355B RID: 13659
			private readonly int mPrimeGenCounter;
		}
	}
}
