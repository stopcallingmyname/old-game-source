using System;

namespace BestHTTP.SignalRCore
{
	// Token: 0x020001DF RID: 479
	public enum ConnectionStates
	{
		// Token: 0x040014ED RID: 5357
		Initial,
		// Token: 0x040014EE RID: 5358
		Authenticating,
		// Token: 0x040014EF RID: 5359
		Negotiating,
		// Token: 0x040014F0 RID: 5360
		Redirected,
		// Token: 0x040014F1 RID: 5361
		Connected,
		// Token: 0x040014F2 RID: 5362
		CloseInitiated,
		// Token: 0x040014F3 RID: 5363
		Closed
	}
}
