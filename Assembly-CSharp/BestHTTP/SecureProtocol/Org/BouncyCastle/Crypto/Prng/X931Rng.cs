using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x020004B8 RID: 1208
	internal class X931Rng
	{
		// Token: 0x06002F22 RID: 12066 RVA: 0x00123EB4 File Offset: 0x001220B4
		internal X931Rng(IBlockCipher engine, byte[] dateTimeVector, IEntropySource entropySource)
		{
			this.mEngine = engine;
			this.mEntropySource = entropySource;
			this.mDT = new byte[engine.GetBlockSize()];
			Array.Copy(dateTimeVector, 0, this.mDT, 0, this.mDT.Length);
			this.mI = new byte[engine.GetBlockSize()];
			this.mR = new byte[engine.GetBlockSize()];
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x00123F28 File Offset: 0x00122128
		internal int Generate(byte[] output, bool predictionResistant)
		{
			if (this.mR.Length == 8)
			{
				if (this.mReseedCounter > 32768L)
				{
					return -1;
				}
				if (X931Rng.IsTooLarge(output, 512))
				{
					throw new ArgumentException("Number of bits per request limited to " + 4096, "output");
				}
			}
			else
			{
				if (this.mReseedCounter > 8388608L)
				{
					return -1;
				}
				if (X931Rng.IsTooLarge(output, 32768))
				{
					throw new ArgumentException("Number of bits per request limited to " + 262144, "output");
				}
			}
			if (predictionResistant || this.mV == null)
			{
				this.mV = this.mEntropySource.GetEntropy();
				if (this.mV.Length != this.mEngine.GetBlockSize())
				{
					throw new InvalidOperationException("Insufficient entropy returned");
				}
			}
			int num = output.Length / this.mR.Length;
			for (int i = 0; i < num; i++)
			{
				this.mEngine.ProcessBlock(this.mDT, 0, this.mI, 0);
				this.Process(this.mR, this.mI, this.mV);
				this.Process(this.mV, this.mR, this.mI);
				Array.Copy(this.mR, 0, output, i * this.mR.Length, this.mR.Length);
				this.Increment(this.mDT);
			}
			int num2 = output.Length - num * this.mR.Length;
			if (num2 > 0)
			{
				this.mEngine.ProcessBlock(this.mDT, 0, this.mI, 0);
				this.Process(this.mR, this.mI, this.mV);
				this.Process(this.mV, this.mR, this.mI);
				Array.Copy(this.mR, 0, output, num * this.mR.Length, num2);
				this.Increment(this.mDT);
			}
			this.mReseedCounter += 1L;
			return output.Length;
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x00124115 File Offset: 0x00122315
		internal void Reseed()
		{
			this.mV = this.mEntropySource.GetEntropy();
			if (this.mV.Length != this.mEngine.GetBlockSize())
			{
				throw new InvalidOperationException("Insufficient entropy returned");
			}
			this.mReseedCounter = 1L;
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06002F25 RID: 12069 RVA: 0x00124150 File Offset: 0x00122350
		internal IEntropySource EntropySource
		{
			get
			{
				return this.mEntropySource;
			}
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x00124158 File Offset: 0x00122358
		private void Process(byte[] res, byte[] a, byte[] b)
		{
			for (int num = 0; num != res.Length; num++)
			{
				res[num] = (a[num] ^ b[num]);
			}
			this.mEngine.ProcessBlock(res, 0, res, 0);
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x00124190 File Offset: 0x00122390
		private void Increment(byte[] val)
		{
			for (int i = val.Length - 1; i >= 0; i--)
			{
				int num = i;
				byte b = val[num] + 1;
				val[num] = b;
				if (b != 0)
				{
					break;
				}
			}
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x001241BF File Offset: 0x001223BF
		private static bool IsTooLarge(byte[] bytes, int maxBytes)
		{
			return bytes != null && bytes.Length > maxBytes;
		}

		// Token: 0x04001F75 RID: 8053
		private const long BLOCK64_RESEED_MAX = 32768L;

		// Token: 0x04001F76 RID: 8054
		private const long BLOCK128_RESEED_MAX = 8388608L;

		// Token: 0x04001F77 RID: 8055
		private const int BLOCK64_MAX_BITS_REQUEST = 4096;

		// Token: 0x04001F78 RID: 8056
		private const int BLOCK128_MAX_BITS_REQUEST = 262144;

		// Token: 0x04001F79 RID: 8057
		private readonly IBlockCipher mEngine;

		// Token: 0x04001F7A RID: 8058
		private readonly IEntropySource mEntropySource;

		// Token: 0x04001F7B RID: 8059
		private readonly byte[] mDT;

		// Token: 0x04001F7C RID: 8060
		private readonly byte[] mI;

		// Token: 0x04001F7D RID: 8061
		private readonly byte[] mR;

		// Token: 0x04001F7E RID: 8062
		private byte[] mV;

		// Token: 0x04001F7F RID: 8063
		private long mReseedCounter = 1L;
	}
}
