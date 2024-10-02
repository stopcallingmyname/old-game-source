using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x020004B3 RID: 1203
	public class ReversedWindowGenerator : IRandomGenerator
	{
		// Token: 0x06002F05 RID: 12037 RVA: 0x00123818 File Offset: 0x00121A18
		public ReversedWindowGenerator(IRandomGenerator generator, int windowSize)
		{
			if (generator == null)
			{
				throw new ArgumentNullException("generator");
			}
			if (windowSize < 2)
			{
				throw new ArgumentException("Window size must be at least 2", "windowSize");
			}
			this.generator = generator;
			this.window = new byte[windowSize];
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x00123858 File Offset: 0x00121A58
		public virtual void AddSeedMaterial(byte[] seed)
		{
			lock (this)
			{
				this.windowCount = 0;
				this.generator.AddSeedMaterial(seed);
			}
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x001238A0 File Offset: 0x00121AA0
		public virtual void AddSeedMaterial(long seed)
		{
			lock (this)
			{
				this.windowCount = 0;
				this.generator.AddSeedMaterial(seed);
			}
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x001238E8 File Offset: 0x00121AE8
		public virtual void NextBytes(byte[] bytes)
		{
			this.doNextBytes(bytes, 0, bytes.Length);
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x001238F5 File Offset: 0x00121AF5
		public virtual void NextBytes(byte[] bytes, int start, int len)
		{
			this.doNextBytes(bytes, start, len);
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x00123900 File Offset: 0x00121B00
		private void doNextBytes(byte[] bytes, int start, int len)
		{
			lock (this)
			{
				int i = 0;
				while (i < len)
				{
					if (this.windowCount < 1)
					{
						this.generator.NextBytes(this.window, 0, this.window.Length);
						this.windowCount = this.window.Length;
					}
					int num = start + i++;
					byte[] array = this.window;
					int num2 = this.windowCount - 1;
					this.windowCount = num2;
					bytes[num] = array[num2];
				}
			}
		}

		// Token: 0x04001F65 RID: 8037
		private readonly IRandomGenerator generator;

		// Token: 0x04001F66 RID: 8038
		private byte[] window;

		// Token: 0x04001F67 RID: 8039
		private int windowCount;
	}
}
