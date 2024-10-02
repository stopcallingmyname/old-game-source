using System;
using System.Threading;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002E8 RID: 744
	public class SecureRandom : Random
	{
		// Token: 0x06001B39 RID: 6969 RVA: 0x000CDFEC File Offset: 0x000CC1EC
		private static long NextCounterValue()
		{
			return Interlocked.Increment(ref SecureRandom.counter);
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06001B3A RID: 6970 RVA: 0x000CDFF8 File Offset: 0x000CC1F8
		private static SecureRandom Master
		{
			get
			{
				return SecureRandom.master;
			}
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x000CE000 File Offset: 0x000CC200
		private static DigestRandomGenerator CreatePrng(string digestName, bool autoSeed)
		{
			IDigest digest = DigestUtilities.GetDigest(digestName);
			if (digest == null)
			{
				return null;
			}
			DigestRandomGenerator digestRandomGenerator = new DigestRandomGenerator(digest);
			if (autoSeed)
			{
				digestRandomGenerator.AddSeedMaterial(SecureRandom.NextCounterValue());
				digestRandomGenerator.AddSeedMaterial(SecureRandom.GetNextBytes(SecureRandom.Master, digest.GetDigestSize()));
			}
			return digestRandomGenerator;
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x000CE048 File Offset: 0x000CC248
		public static byte[] GetNextBytes(SecureRandom secureRandom, int length)
		{
			byte[] array = new byte[length];
			secureRandom.NextBytes(array);
			return array;
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x000CE064 File Offset: 0x000CC264
		public static SecureRandom GetInstance(string algorithm)
		{
			return SecureRandom.GetInstance(algorithm, true);
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x000CE070 File Offset: 0x000CC270
		public static SecureRandom GetInstance(string algorithm, bool autoSeed)
		{
			string text = Platform.ToUpperInvariant(algorithm);
			if (Platform.EndsWith(text, "PRNG"))
			{
				DigestRandomGenerator digestRandomGenerator = SecureRandom.CreatePrng(text.Substring(0, text.Length - "PRNG".Length), autoSeed);
				if (digestRandomGenerator != null)
				{
					return new SecureRandom(digestRandomGenerator);
				}
			}
			throw new ArgumentException("Unrecognised PRNG algorithm: " + algorithm, "algorithm");
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x000CE0CF File Offset: 0x000CC2CF
		[Obsolete("Call GenerateSeed() on a SecureRandom instance instead")]
		public static byte[] GetSeed(int length)
		{
			return SecureRandom.GetNextBytes(SecureRandom.Master, length);
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x000CE0DC File Offset: 0x000CC2DC
		public SecureRandom() : this(SecureRandom.CreatePrng("SHA256", true))
		{
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x000CE0EF File Offset: 0x000CC2EF
		[Obsolete("Use GetInstance/SetSeed instead")]
		public SecureRandom(byte[] seed) : this(SecureRandom.CreatePrng("SHA1", false))
		{
			this.SetSeed(seed);
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x000CE109 File Offset: 0x000CC309
		public SecureRandom(IRandomGenerator generator) : base(0)
		{
			this.generator = generator;
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x000CE119 File Offset: 0x000CC319
		public virtual byte[] GenerateSeed(int length)
		{
			return SecureRandom.GetNextBytes(SecureRandom.Master, length);
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x000CE126 File Offset: 0x000CC326
		public virtual void SetSeed(byte[] seed)
		{
			this.generator.AddSeedMaterial(seed);
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x000CE134 File Offset: 0x000CC334
		public virtual void SetSeed(long seed)
		{
			this.generator.AddSeedMaterial(seed);
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x000CE142 File Offset: 0x000CC342
		public override int Next()
		{
			return this.NextInt() & int.MaxValue;
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x000CE150 File Offset: 0x000CC350
		public override int Next(int maxValue)
		{
			if (maxValue < 2)
			{
				if (maxValue < 0)
				{
					throw new ArgumentOutOfRangeException("maxValue", "cannot be negative");
				}
				return 0;
			}
			else
			{
				int num;
				if ((maxValue & maxValue - 1) == 0)
				{
					num = (this.NextInt() & int.MaxValue);
					return (int)((long)num * (long)maxValue >> 31);
				}
				int num2;
				do
				{
					num = (this.NextInt() & int.MaxValue);
					num2 = num % maxValue;
				}
				while (num - num2 + (maxValue - 1) < 0);
				return num2;
			}
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x000CE1B4 File Offset: 0x000CC3B4
		public override int Next(int minValue, int maxValue)
		{
			if (maxValue <= minValue)
			{
				if (maxValue == minValue)
				{
					return minValue;
				}
				throw new ArgumentException("maxValue cannot be less than minValue");
			}
			else
			{
				int num = maxValue - minValue;
				if (num > 0)
				{
					return minValue + this.Next(num);
				}
				int num2;
				do
				{
					num2 = this.NextInt();
				}
				while (num2 < minValue || num2 >= maxValue);
				return num2;
			}
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x000CE1F8 File Offset: 0x000CC3F8
		public override void NextBytes(byte[] buf)
		{
			this.generator.NextBytes(buf);
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x000CE206 File Offset: 0x000CC406
		public virtual void NextBytes(byte[] buf, int off, int len)
		{
			this.generator.NextBytes(buf, off, len);
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x000CE216 File Offset: 0x000CC416
		public override double NextDouble()
		{
			return Convert.ToDouble((ulong)this.NextLong()) / SecureRandom.DoubleScale;
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x000CE22C File Offset: 0x000CC42C
		public virtual int NextInt()
		{
			byte[] array = new byte[4];
			this.NextBytes(array);
			return (((int)array[0] << 8 | (int)array[1]) << 8 | (int)array[2]) << 8 | (int)array[3];
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x000CE25C File Offset: 0x000CC45C
		public virtual long NextLong()
		{
			return (long)((ulong)this.NextInt() << 32 | (ulong)this.NextInt());
		}

		// Token: 0x040018EE RID: 6382
		private static long counter = Times.NanoTime();

		// Token: 0x040018EF RID: 6383
		private static readonly SecureRandom master = new SecureRandom(new CryptoApiRandomGenerator());

		// Token: 0x040018F0 RID: 6384
		protected readonly IRandomGenerator generator;

		// Token: 0x040018F1 RID: 6385
		private static readonly double DoubleScale = Math.Pow(2.0, 64.0);
	}
}
