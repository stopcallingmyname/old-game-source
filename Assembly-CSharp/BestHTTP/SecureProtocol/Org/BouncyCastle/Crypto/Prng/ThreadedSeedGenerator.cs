using System;
using System.Threading;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x020004B6 RID: 1206
	public class ThreadedSeedGenerator
	{
		// Token: 0x06002F1B RID: 12059 RVA: 0x00123C92 File Offset: 0x00121E92
		public byte[] GenerateSeed(int numBytes, bool fast)
		{
			return new ThreadedSeedGenerator.SeedGenerator().GenerateSeed(numBytes, fast);
		}

		// Token: 0x02000953 RID: 2387
		private class SeedGenerator
		{
			// Token: 0x06004F0D RID: 20237 RVA: 0x001B3B43 File Offset: 0x001B1D43
			private void Run(object ignored)
			{
				while (!this.stop)
				{
					this.counter++;
				}
			}

			// Token: 0x06004F0E RID: 20238 RVA: 0x001B3B64 File Offset: 0x001B1D64
			public byte[] GenerateSeed(int numBytes, bool fast)
			{
				ThreadPriority priority = Thread.CurrentThread.Priority;
				byte[] result;
				try
				{
					Thread.CurrentThread.Priority = ThreadPriority.Normal;
					result = this.DoGenerateSeed(numBytes, fast);
				}
				finally
				{
					Thread.CurrentThread.Priority = priority;
				}
				return result;
			}

			// Token: 0x06004F0F RID: 20239 RVA: 0x001B3BB0 File Offset: 0x001B1DB0
			private byte[] DoGenerateSeed(int numBytes, bool fast)
			{
				this.counter = 0;
				this.stop = false;
				byte[] array = new byte[numBytes];
				int num = 0;
				int num2 = fast ? numBytes : (numBytes * 8);
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.Run));
				for (int i = 0; i < num2; i++)
				{
					while (this.counter == num)
					{
						try
						{
							Thread.Sleep(1);
						}
						catch (Exception)
						{
						}
					}
					num = this.counter;
					if (fast)
					{
						array[i] = (byte)num;
					}
					else
					{
						int num3 = i / 8;
						array[num3] = (byte)((int)array[num3] << 1 | (num & 1));
					}
				}
				this.stop = true;
				return array;
			}

			// Token: 0x04003634 RID: 13876
			private volatile int counter;

			// Token: 0x04003635 RID: 13877
			private volatile bool stop;
		}
	}
}
