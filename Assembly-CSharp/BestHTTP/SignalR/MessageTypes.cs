using System;

namespace BestHTTP.SignalR
{
	// Token: 0x0200020A RID: 522
	public enum MessageTypes
	{
		// Token: 0x04001589 RID: 5513
		KeepAlive,
		// Token: 0x0400158A RID: 5514
		Data,
		// Token: 0x0400158B RID: 5515
		Multiple,
		// Token: 0x0400158C RID: 5516
		Result,
		// Token: 0x0400158D RID: 5517
		Failure,
		// Token: 0x0400158E RID: 5518
		MethodCall,
		// Token: 0x0400158F RID: 5519
		Progress
	}
}
