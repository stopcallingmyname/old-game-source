using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng.Drbg;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x020004B4 RID: 1204
	public class SP800SecureRandom : SecureRandom
	{
		// Token: 0x06002F0B RID: 12043 RVA: 0x00123990 File Offset: 0x00121B90
		internal SP800SecureRandom(SecureRandom randomSource, IEntropySource entropySource, IDrbgProvider drbgProvider, bool predictionResistant) : base(null)
		{
			this.mRandomSource = randomSource;
			this.mEntropySource = entropySource;
			this.mDrbgProvider = drbgProvider;
			this.mPredictionResistant = predictionResistant;
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x001239B8 File Offset: 0x00121BB8
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

		// Token: 0x06002F0D RID: 12045 RVA: 0x00123A04 File Offset: 0x00121C04
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

		// Token: 0x06002F0E RID: 12046 RVA: 0x00123A50 File Offset: 0x00121C50
		public override void NextBytes(byte[] bytes)
		{
			lock (this)
			{
				if (this.mDrbg == null)
				{
					this.mDrbg = this.mDrbgProvider.Get(this.mEntropySource);
				}
				if (this.mDrbg.Generate(bytes, null, this.mPredictionResistant) < 0)
				{
					this.mDrbg.Reseed(null);
					this.mDrbg.Generate(bytes, null, this.mPredictionResistant);
				}
			}
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x00123ADC File Offset: 0x00121CDC
		public override void NextBytes(byte[] buf, int off, int len)
		{
			byte[] array = new byte[len];
			this.NextBytes(array);
			Array.Copy(array, 0, buf, off, len);
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x00123B01 File Offset: 0x00121D01
		public override byte[] GenerateSeed(int numBytes)
		{
			return EntropyUtilities.GenerateSeed(this.mEntropySource, numBytes);
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x00123B10 File Offset: 0x00121D10
		public virtual void Reseed(byte[] additionalInput)
		{
			lock (this)
			{
				if (this.mDrbg == null)
				{
					this.mDrbg = this.mDrbgProvider.Get(this.mEntropySource);
				}
				this.mDrbg.Reseed(additionalInput);
			}
		}

		// Token: 0x04001F68 RID: 8040
		private readonly IDrbgProvider mDrbgProvider;

		// Token: 0x04001F69 RID: 8041
		private readonly bool mPredictionResistant;

		// Token: 0x04001F6A RID: 8042
		private readonly SecureRandom mRandomSource;

		// Token: 0x04001F6B RID: 8043
		private readonly IEntropySource mEntropySource;

		// Token: 0x04001F6C RID: 8044
		private ISP80090Drbg mDrbg;
	}
}
