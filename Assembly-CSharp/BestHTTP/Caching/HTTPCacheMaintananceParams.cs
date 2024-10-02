using System;

namespace BestHTTP.Caching
{
	// Token: 0x02000817 RID: 2071
	public sealed class HTTPCacheMaintananceParams
	{
		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x060049D5 RID: 18901 RVA: 0x001A3535 File Offset: 0x001A1735
		// (set) Token: 0x060049D6 RID: 18902 RVA: 0x001A353D File Offset: 0x001A173D
		public TimeSpan DeleteOlder { get; private set; }

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x060049D7 RID: 18903 RVA: 0x001A3546 File Offset: 0x001A1746
		// (set) Token: 0x060049D8 RID: 18904 RVA: 0x001A354E File Offset: 0x001A174E
		public ulong MaxCacheSize { get; private set; }

		// Token: 0x060049D9 RID: 18905 RVA: 0x001A3557 File Offset: 0x001A1757
		public HTTPCacheMaintananceParams(TimeSpan deleteOlder, ulong maxCacheSize)
		{
			this.DeleteOlder = deleteOlder;
			this.MaxCacheSize = maxCacheSize;
		}
	}
}
