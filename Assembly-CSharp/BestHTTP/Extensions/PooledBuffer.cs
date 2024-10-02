using System;

namespace BestHTTP.Extensions
{
	// Token: 0x020007F5 RID: 2037
	public struct PooledBuffer : IDisposable
	{
		// Token: 0x0600487A RID: 18554 RVA: 0x00199325 File Offset: 0x00197525
		public void Dispose()
		{
			if (this.Data != null)
			{
				VariableSizedBufferPool.Release(this.Data);
			}
			this.Data = null;
		}

		// Token: 0x04002F0E RID: 12046
		public byte[] Data;

		// Token: 0x04002F0F RID: 12047
		public int Length;
	}
}
