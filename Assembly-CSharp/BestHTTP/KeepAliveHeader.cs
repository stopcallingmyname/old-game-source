using System;
using System.Collections.Generic;
using BestHTTP.Extensions;

namespace BestHTTP
{
	// Token: 0x02000175 RID: 373
	internal sealed class KeepAliveHeader
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x00090BAA File Offset: 0x0008EDAA
		// (set) Token: 0x06000D28 RID: 3368 RVA: 0x00090BB2 File Offset: 0x0008EDB2
		public TimeSpan TimeOut { get; private set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000D29 RID: 3369 RVA: 0x00090BBB File Offset: 0x0008EDBB
		// (set) Token: 0x06000D2A RID: 3370 RVA: 0x00090BC3 File Offset: 0x0008EDC3
		public int MaxRequests { get; private set; }

		// Token: 0x06000D2B RID: 3371 RVA: 0x00090BCC File Offset: 0x0008EDCC
		public void Parse(List<string> headerValues)
		{
			HeaderParser headerParser = new HeaderParser(headerValues[0]);
			HeaderValue headerValue;
			if (headerParser.TryGet("timeout", out headerValue) && headerValue.HasValue)
			{
				int num = 0;
				if (int.TryParse(headerValue.Value, out num))
				{
					this.TimeOut = TimeSpan.FromSeconds((double)num);
				}
				else
				{
					this.TimeOut = TimeSpan.MaxValue;
				}
			}
			if (headerParser.TryGet("max", out headerValue) && headerValue.HasValue)
			{
				int maxRequests = 0;
				if (int.TryParse("max", out maxRequests))
				{
					this.MaxRequests = maxRequests;
					return;
				}
				this.MaxRequests = int.MaxValue;
			}
		}
	}
}
