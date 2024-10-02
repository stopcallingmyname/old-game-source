using System;
using BestHTTP.Extensions;

namespace BestHTTP.WebSocket.Frames
{
	// Token: 0x020001BA RID: 442
	public struct RawFrameData : IDisposable
	{
		// Token: 0x0600105D RID: 4189 RVA: 0x0009E258 File Offset: 0x0009C458
		public RawFrameData(byte[] data, int length)
		{
			this.Data = data;
			this.Length = length;
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x0009E268 File Offset: 0x0009C468
		public void Dispose()
		{
			VariableSizedBufferPool.Release(this.Data);
			this.Data = null;
		}

		// Token: 0x04001437 RID: 5175
		public byte[] Data;

		// Token: 0x04001438 RID: 5176
		public int Length;
	}
}
