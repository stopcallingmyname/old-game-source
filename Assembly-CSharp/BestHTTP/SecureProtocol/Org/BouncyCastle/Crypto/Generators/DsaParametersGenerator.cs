using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x0200054E RID: 1358
	public class DsaParametersGenerator
	{
		// Token: 0x0600334C RID: 13132 RVA: 0x00133B89 File Offset: 0x00131D89
		public DsaParametersGenerator() : this(new Sha1Digest())
		{
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x00133B96 File Offset: 0x00131D96
		public DsaParametersGenerator(IDigest digest)
		{
			this.digest = digest;
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x00133BA8 File Offset: 0x00131DA8
		public virtual void Init(int size, int certainty, SecureRandom random)
		{
			if (!DsaParametersGenerator.IsValidDsaStrength(size))
			{
				throw new ArgumentException("size must be from 512 - 1024 and a multiple of 64", "size");
			}
			this.use186_3 = false;
			this.L = size;
			this.N = DsaParametersGenerator.GetDefaultN(size);
			this.certainty = certainty;
			this.random = random;
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x00133BF8 File Offset: 0x00131DF8
		public virtual void Init(DsaParameterGenerationParameters parameters)
		{
			this.use186_3 = true;
			this.L = parameters.L;
			this.N = parameters.N;
			this.certainty = parameters.Certainty;
			this.random = parameters.Random;
			this.usageIndex = parameters.UsageIndex;
			if (this.L < 1024 || this.L > 3072 || this.L % 1024 != 0)
			{
				throw new ArgumentException("Values must be between 1024 and 3072 and a multiple of 1024", "L");
			}
			if (this.L == 1024 && this.N != 160)
			{
				throw new ArgumentException("N must be 160 for L = 1024");
			}
			if (this.L == 2048 && this.N != 224 && this.N != 256)
			{
				throw new ArgumentException("N must be 224 or 256 for L = 2048");
			}
			if (this.L == 3072 && this.N != 256)
			{
				throw new ArgumentException("N must be 256 for L = 3072");
			}
			if (this.digest.GetDigestSize() * 8 < this.N)
			{
				throw new InvalidOperationException("Digest output size too small for value of N");
			}
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x00133D1C File Offset: 0x00131F1C
		public virtual DsaParameters GenerateParameters()
		{
			if (!this.use186_3)
			{
				return this.GenerateParameters_FIPS186_2();
			}
			return this.GenerateParameters_FIPS186_3();
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x00133D34 File Offset: 0x00131F34
		protected virtual DsaParameters GenerateParameters_FIPS186_2()
		{
			byte[] array = new byte[20];
			byte[] array2 = new byte[20];
			byte[] array3 = new byte[20];
			byte[] array4 = new byte[20];
			int num = (this.L - 1) / 160;
			byte[] array5 = new byte[this.L / 8];
			if (!(this.digest is Sha1Digest))
			{
				throw new InvalidOperationException("can only use SHA-1 for generating FIPS 186-2 parameters");
			}
			BigInteger bigInteger;
			int i;
			BigInteger bigInteger4;
			for (;;)
			{
				this.random.NextBytes(array);
				DsaParametersGenerator.Hash(this.digest, array, array2);
				Array.Copy(array, 0, array3, 0, array.Length);
				DsaParametersGenerator.Inc(array3);
				DsaParametersGenerator.Hash(this.digest, array3, array3);
				for (int num2 = 0; num2 != array4.Length; num2++)
				{
					array4[num2] = (array2[num2] ^ array3[num2]);
				}
				byte[] array6 = array4;
				int num3 = 0;
				array6[num3] |= 128;
				byte[] array7 = array4;
				int num4 = 19;
				array7[num4] |= 1;
				bigInteger = new BigInteger(1, array4);
				if (bigInteger.IsProbablePrime(this.certainty))
				{
					byte[] array8 = Arrays.Clone(array);
					DsaParametersGenerator.Inc(array8);
					for (i = 0; i < 4096; i++)
					{
						for (int j = 0; j < num; j++)
						{
							DsaParametersGenerator.Inc(array8);
							DsaParametersGenerator.Hash(this.digest, array8, array2);
							Array.Copy(array2, 0, array5, array5.Length - (j + 1) * array2.Length, array2.Length);
						}
						DsaParametersGenerator.Inc(array8);
						DsaParametersGenerator.Hash(this.digest, array8, array2);
						Array.Copy(array2, array2.Length - (array5.Length - num * array2.Length), array5, 0, array5.Length - num * array2.Length);
						byte[] array9 = array5;
						int num5 = 0;
						array9[num5] |= 128;
						BigInteger bigInteger2 = new BigInteger(1, array5);
						BigInteger bigInteger3 = bigInteger2.Mod(bigInteger.ShiftLeft(1));
						bigInteger4 = bigInteger2.Subtract(bigInteger3.Subtract(BigInteger.One));
						if (bigInteger4.BitLength == this.L && bigInteger4.IsProbablePrime(this.certainty))
						{
							goto Block_6;
						}
					}
				}
			}
			Block_6:
			BigInteger g = this.CalculateGenerator_FIPS186_2(bigInteger4, bigInteger, this.random);
			return new DsaParameters(bigInteger4, bigInteger, g, new DsaValidationParameters(array, i));
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x00133F50 File Offset: 0x00132150
		protected virtual BigInteger CalculateGenerator_FIPS186_2(BigInteger p, BigInteger q, SecureRandom r)
		{
			BigInteger e = p.Subtract(BigInteger.One).Divide(q);
			BigInteger max = p.Subtract(BigInteger.Two);
			BigInteger bigInteger;
			do
			{
				bigInteger = BigIntegers.CreateRandomInRange(BigInteger.Two, max, r).ModPow(e, p);
			}
			while (bigInteger.BitLength <= 1);
			return bigInteger;
		}

		// Token: 0x06003353 RID: 13139 RVA: 0x00133F9C File Offset: 0x0013219C
		protected virtual DsaParameters GenerateParameters_FIPS186_3()
		{
			IDigest digest = this.digest;
			int num = digest.GetDigestSize() * 8;
			byte[] array = new byte[this.N / 8];
			int num2 = (this.L - 1) / num;
			int n = (this.L - 1) % num;
			byte[] array2 = new byte[digest.GetDigestSize()];
			BigInteger bigInteger;
			int i;
			BigInteger bigInteger6;
			for (;;)
			{
				this.random.NextBytes(array);
				DsaParametersGenerator.Hash(digest, array, array2);
				bigInteger = new BigInteger(1, array2).Mod(BigInteger.One.ShiftLeft(this.N - 1)).SetBit(0).SetBit(this.N - 1);
				if (bigInteger.IsProbablePrime(this.certainty))
				{
					byte[] array3 = Arrays.Clone(array);
					int num3 = 4 * this.L;
					for (i = 0; i < num3; i++)
					{
						BigInteger bigInteger2 = BigInteger.Zero;
						int j = 0;
						int num4 = 0;
						while (j <= num2)
						{
							DsaParametersGenerator.Inc(array3);
							DsaParametersGenerator.Hash(digest, array3, array2);
							BigInteger bigInteger3 = new BigInteger(1, array2);
							if (j == num2)
							{
								bigInteger3 = bigInteger3.Mod(BigInteger.One.ShiftLeft(n));
							}
							bigInteger2 = bigInteger2.Add(bigInteger3.ShiftLeft(num4));
							j++;
							num4 += num;
						}
						BigInteger bigInteger4 = bigInteger2.Add(BigInteger.One.ShiftLeft(this.L - 1));
						BigInteger bigInteger5 = bigInteger4.Mod(bigInteger.ShiftLeft(1));
						bigInteger6 = bigInteger4.Subtract(bigInteger5.Subtract(BigInteger.One));
						if (bigInteger6.BitLength == this.L && bigInteger6.IsProbablePrime(this.certainty))
						{
							goto Block_5;
						}
					}
				}
			}
			Block_5:
			if (this.usageIndex >= 0)
			{
				BigInteger bigInteger7 = this.CalculateGenerator_FIPS186_3_Verifiable(digest, bigInteger6, bigInteger, array, this.usageIndex);
				if (bigInteger7 != null)
				{
					return new DsaParameters(bigInteger6, bigInteger, bigInteger7, new DsaValidationParameters(array, i, this.usageIndex));
				}
			}
			BigInteger g = this.CalculateGenerator_FIPS186_3_Unverifiable(bigInteger6, bigInteger, this.random);
			return new DsaParameters(bigInteger6, bigInteger, g, new DsaValidationParameters(array, i));
		}

		// Token: 0x06003354 RID: 13140 RVA: 0x00134192 File Offset: 0x00132392
		protected virtual BigInteger CalculateGenerator_FIPS186_3_Unverifiable(BigInteger p, BigInteger q, SecureRandom r)
		{
			return this.CalculateGenerator_FIPS186_2(p, q, r);
		}

		// Token: 0x06003355 RID: 13141 RVA: 0x001341A0 File Offset: 0x001323A0
		protected virtual BigInteger CalculateGenerator_FIPS186_3_Verifiable(IDigest d, BigInteger p, BigInteger q, byte[] seed, int index)
		{
			BigInteger e = p.Subtract(BigInteger.One).Divide(q);
			byte[] array = Hex.Decode("6767656E");
			byte[] array2 = new byte[seed.Length + array.Length + 1 + 2];
			Array.Copy(seed, 0, array2, 0, seed.Length);
			Array.Copy(array, 0, array2, seed.Length, array.Length);
			array2[array2.Length - 3] = (byte)index;
			byte[] array3 = new byte[d.GetDigestSize()];
			for (int i = 1; i < 65536; i++)
			{
				DsaParametersGenerator.Inc(array2);
				DsaParametersGenerator.Hash(d, array2, array3);
				BigInteger bigInteger = new BigInteger(1, array3).ModPow(e, p);
				if (bigInteger.CompareTo(BigInteger.Two) >= 0)
				{
					return bigInteger;
				}
			}
			return null;
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x00134254 File Offset: 0x00132454
		private static bool IsValidDsaStrength(int strength)
		{
			return strength >= 512 && strength <= 1024 && strength % 64 == 0;
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x0013426F File Offset: 0x0013246F
		protected static void Hash(IDigest d, byte[] input, byte[] output)
		{
			d.BlockUpdate(input, 0, input.Length);
			d.DoFinal(output, 0);
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x00134285 File Offset: 0x00132485
		private static int GetDefaultN(int L)
		{
			if (L <= 1024)
			{
				return 160;
			}
			return 256;
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x0013429C File Offset: 0x0013249C
		protected static void Inc(byte[] buf)
		{
			for (int i = buf.Length - 1; i >= 0; i--)
			{
				byte b = buf[i] + 1;
				buf[i] = b;
				if (b != 0)
				{
					break;
				}
			}
		}

		// Token: 0x0400219C RID: 8604
		private IDigest digest;

		// Token: 0x0400219D RID: 8605
		private int L;

		// Token: 0x0400219E RID: 8606
		private int N;

		// Token: 0x0400219F RID: 8607
		private int certainty;

		// Token: 0x040021A0 RID: 8608
		private SecureRandom random;

		// Token: 0x040021A1 RID: 8609
		private bool use186_3;

		// Token: 0x040021A2 RID: 8610
		private int usageIndex;
	}
}
