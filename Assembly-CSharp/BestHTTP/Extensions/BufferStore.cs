using System;
using System.Collections.Generic;

namespace BestHTTP.Extensions
{
	// Token: 0x020007F6 RID: 2038
	internal struct BufferStore
	{
		// Token: 0x0600487B RID: 18555 RVA: 0x00199341 File Offset: 0x00197541
		public BufferStore(long size)
		{
			this.Size = size;
			this.buffers = new List<BufferDesc>();
		}

		// Token: 0x0600487C RID: 18556 RVA: 0x00199355 File Offset: 0x00197555
		public BufferStore(long size, byte[] buffer)
		{
			this = new BufferStore(size);
			this.buffers.Add(new BufferDesc(buffer));
		}

		// Token: 0x04002F10 RID: 12048
		public readonly long Size;

		// Token: 0x04002F11 RID: 12049
		public List<BufferDesc> buffers;
	}
}
