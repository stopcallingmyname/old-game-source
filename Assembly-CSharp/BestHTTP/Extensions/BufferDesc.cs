using System;

namespace BestHTTP.Extensions
{
	// Token: 0x020007F7 RID: 2039
	internal struct BufferDesc
	{
		// Token: 0x0600487D RID: 18557 RVA: 0x0019936F File Offset: 0x0019756F
		public BufferDesc(byte[] buff)
		{
			this.buffer = buff;
			this.released = DateTime.UtcNow;
		}

		// Token: 0x04002F12 RID: 12050
		public static readonly BufferDesc Empty = new BufferDesc(null);

		// Token: 0x04002F13 RID: 12051
		public byte[] buffer;

		// Token: 0x04002F14 RID: 12052
		public DateTime released;
	}
}
