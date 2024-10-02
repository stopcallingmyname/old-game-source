using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x020004AC RID: 1196
	public class BasicEntropySourceProvider : IEntropySourceProvider
	{
		// Token: 0x06002EE9 RID: 12009 RVA: 0x0012347C File Offset: 0x0012167C
		public BasicEntropySourceProvider(SecureRandom secureRandom, bool isPredictionResistant)
		{
			this.mSecureRandom = secureRandom;
			this.mPredictionResistant = isPredictionResistant;
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x00123492 File Offset: 0x00121692
		public IEntropySource Get(int bitsRequired)
		{
			return new BasicEntropySourceProvider.BasicEntropySource(this.mSecureRandom, this.mPredictionResistant, bitsRequired);
		}

		// Token: 0x04001F5A RID: 8026
		private readonly SecureRandom mSecureRandom;

		// Token: 0x04001F5B RID: 8027
		private readonly bool mPredictionResistant;

		// Token: 0x0200094E RID: 2382
		private class BasicEntropySource : IEntropySource
		{
			// Token: 0x06004EFF RID: 20223 RVA: 0x001B39C8 File Offset: 0x001B1BC8
			internal BasicEntropySource(SecureRandom secureRandom, bool predictionResistant, int entropySize)
			{
				this.mSecureRandom = secureRandom;
				this.mPredictionResistant = predictionResistant;
				this.mEntropySize = entropySize;
			}

			// Token: 0x17000C58 RID: 3160
			// (get) Token: 0x06004F00 RID: 20224 RVA: 0x001B39E5 File Offset: 0x001B1BE5
			bool IEntropySource.IsPredictionResistant
			{
				get
				{
					return this.mPredictionResistant;
				}
			}

			// Token: 0x06004F01 RID: 20225 RVA: 0x001B39ED File Offset: 0x001B1BED
			byte[] IEntropySource.GetEntropy()
			{
				return SecureRandom.GetNextBytes(this.mSecureRandom, (this.mEntropySize + 7) / 8);
			}

			// Token: 0x17000C59 RID: 3161
			// (get) Token: 0x06004F02 RID: 20226 RVA: 0x001B3A04 File Offset: 0x001B1C04
			int IEntropySource.EntropySize
			{
				get
				{
					return this.mEntropySize;
				}
			}

			// Token: 0x04003621 RID: 13857
			private readonly SecureRandom mSecureRandom;

			// Token: 0x04003622 RID: 13858
			private readonly bool mPredictionResistant;

			// Token: 0x04003623 RID: 13859
			private readonly int mEntropySize;
		}
	}
}
