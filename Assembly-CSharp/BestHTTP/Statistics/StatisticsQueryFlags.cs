using System;

namespace BestHTTP.Statistics
{
	// Token: 0x020001C0 RID: 448
	[Flags]
	public enum StatisticsQueryFlags : byte
	{
		// Token: 0x0400145B RID: 5211
		Connections = 1,
		// Token: 0x0400145C RID: 5212
		Cache = 2,
		// Token: 0x0400145D RID: 5213
		Cookies = 4,
		// Token: 0x0400145E RID: 5214
		All = 255
	}
}
