using System;
using System.Security.Cryptography;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x020004AD RID: 1197
	public class CryptoApiEntropySourceProvider : IEntropySourceProvider
	{
		// Token: 0x06002EEB RID: 12011 RVA: 0x001234A6 File Offset: 0x001216A6
		public CryptoApiEntropySourceProvider() : this(RandomNumberGenerator.Create(), true)
		{
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x001234B4 File Offset: 0x001216B4
		public CryptoApiEntropySourceProvider(RandomNumberGenerator rng, bool isPredictionResistant)
		{
			if (rng == null)
			{
				throw new ArgumentNullException("rng");
			}
			this.mRng = rng;
			this.mPredictionResistant = isPredictionResistant;
		}

		// Token: 0x06002EED RID: 12013 RVA: 0x001234D8 File Offset: 0x001216D8
		public IEntropySource Get(int bitsRequired)
		{
			return new CryptoApiEntropySourceProvider.CryptoApiEntropySource(this.mRng, this.mPredictionResistant, bitsRequired);
		}

		// Token: 0x04001F5C RID: 8028
		private readonly RandomNumberGenerator mRng;

		// Token: 0x04001F5D RID: 8029
		private readonly bool mPredictionResistant;

		// Token: 0x0200094F RID: 2383
		private class CryptoApiEntropySource : IEntropySource
		{
			// Token: 0x06004F03 RID: 20227 RVA: 0x001B3A0C File Offset: 0x001B1C0C
			internal CryptoApiEntropySource(RandomNumberGenerator rng, bool predictionResistant, int entropySize)
			{
				this.mRng = rng;
				this.mPredictionResistant = predictionResistant;
				this.mEntropySize = entropySize;
			}

			// Token: 0x17000C5A RID: 3162
			// (get) Token: 0x06004F04 RID: 20228 RVA: 0x001B3A29 File Offset: 0x001B1C29
			bool IEntropySource.IsPredictionResistant
			{
				get
				{
					return this.mPredictionResistant;
				}
			}

			// Token: 0x06004F05 RID: 20229 RVA: 0x001B3A34 File Offset: 0x001B1C34
			byte[] IEntropySource.GetEntropy()
			{
				byte[] array = new byte[(this.mEntropySize + 7) / 8];
				this.mRng.GetBytes(array);
				return array;
			}

			// Token: 0x17000C5B RID: 3163
			// (get) Token: 0x06004F06 RID: 20230 RVA: 0x001B3A5E File Offset: 0x001B1C5E
			int IEntropySource.EntropySize
			{
				get
				{
					return this.mEntropySize;
				}
			}

			// Token: 0x04003624 RID: 13860
			private readonly RandomNumberGenerator mRng;

			// Token: 0x04003625 RID: 13861
			private readonly bool mPredictionResistant;

			// Token: 0x04003626 RID: 13862
			private readonly int mEntropySize;
		}
	}
}
