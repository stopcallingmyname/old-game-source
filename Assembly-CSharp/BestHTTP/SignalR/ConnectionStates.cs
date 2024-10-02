using System;

namespace BestHTTP.SignalR
{
	// Token: 0x0200020B RID: 523
	public enum ConnectionStates
	{
		// Token: 0x04001591 RID: 5521
		Initial,
		// Token: 0x04001592 RID: 5522
		Authenticating,
		// Token: 0x04001593 RID: 5523
		Negotiating,
		// Token: 0x04001594 RID: 5524
		Connecting,
		// Token: 0x04001595 RID: 5525
		Connected,
		// Token: 0x04001596 RID: 5526
		Reconnecting,
		// Token: 0x04001597 RID: 5527
		Closed
	}
}
