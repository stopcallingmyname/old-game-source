using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x020004B9 RID: 1209
	public class X931SecureRandom : SecureRandom
	{
		// Token: 0x06002F29 RID: 12073 RVA: 0x001241CC File Offset: 0x001223CC
		internal X931SecureRandom(SecureRandom randomSource, X931Rng drbg, bool predictionResistant) : base(null)
		{
			this.mRandomSource = randomSource;
			this.mDrbg = drbg;
			this.mPredictionResistant = predictionResistant;
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x001241EC File Offset: 0x001223EC
		public override void SetSeed(byte[] seed)
		{
			lock (this)
			{
				if (this.mRandomSource != null)
				{
					this.mRandomSource.SetSeed(seed);
				}
			}
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x00124238 File Offset: 0x00122438
		public override void SetSeed(long seed)
		{
			lock (this)
			{
				if (this.mRandomSource != null)
				{
					this.mRandomSource.SetSeed(seed);
				}
			}
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x00124284 File Offset: 0x00122484
		public override void NextBytes(byte[] bytes)
		{
			lock (this)
			{
				if (this.mDrbg.Generate(bytes, this.mPredictionResistant) < 0)
				{
					this.mDrbg.Reseed();
					this.mDrbg.Generate(bytes, this.mPredictionResistant);
				}
			}
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x001242EC File Offset: 0x001224EC
		public override void NextBytes(byte[] buf, int off, int len)
		{
			byte[] array = new byte[len];
			this.NextBytes(array);
			Array.Copy(array, 0, buf, off, len);
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x00124311 File Offset: 0x00122511
		public override byte[] GenerateSeed(int numBytes)
		{
			return EntropyUtilities.GenerateSeed(this.mDrbg.EntropySource, numBytes);
		}

		// Token: 0x04001F80 RID: 8064
		private readonly bool mPredictionResistant;

		// Token: 0x04001F81 RID: 8065
		private readonly SecureRandom mRandomSource;

		// Token: 0x04001F82 RID: 8066
		private readonly X931Rng mDrbg;
	}
}
