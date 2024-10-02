using System;

namespace BestHTTP.Extensions
{
	// Token: 0x020007F0 RID: 2032
	public interface IHeartbeat
	{
		// Token: 0x06004854 RID: 18516
		void OnHeartbeatUpdate(TimeSpan dif);
	}
}
