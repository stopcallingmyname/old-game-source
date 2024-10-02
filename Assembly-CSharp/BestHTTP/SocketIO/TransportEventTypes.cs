using System;

namespace BestHTTP.SocketIO
{
	// Token: 0x020001C2 RID: 450
	public enum TransportEventTypes
	{
		// Token: 0x0400146A RID: 5226
		Unknown = -1,
		// Token: 0x0400146B RID: 5227
		Open,
		// Token: 0x0400146C RID: 5228
		Close,
		// Token: 0x0400146D RID: 5229
		Ping,
		// Token: 0x0400146E RID: 5230
		Pong,
		// Token: 0x0400146F RID: 5231
		Message,
		// Token: 0x04001470 RID: 5232
		Upgrade,
		// Token: 0x04001471 RID: 5233
		Noop
	}
}
