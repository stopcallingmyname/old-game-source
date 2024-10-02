using System;

namespace BestHTTP.Statistics
{
	// Token: 0x020001C1 RID: 449
	public struct GeneralStatistics
	{
		// Token: 0x0400145F RID: 5215
		public StatisticsQueryFlags QueryFlags;

		// Token: 0x04001460 RID: 5216
		public int Connections;

		// Token: 0x04001461 RID: 5217
		public int ActiveConnections;

		// Token: 0x04001462 RID: 5218
		public int FreeConnections;

		// Token: 0x04001463 RID: 5219
		public int RecycledConnections;

		// Token: 0x04001464 RID: 5220
		public int RequestsInQueue;

		// Token: 0x04001465 RID: 5221
		public int CacheEntityCount;

		// Token: 0x04001466 RID: 5222
		public ulong CacheSize;

		// Token: 0x04001467 RID: 5223
		public int CookieCount;

		// Token: 0x04001468 RID: 5224
		public uint CookieJarSize;
	}
}
