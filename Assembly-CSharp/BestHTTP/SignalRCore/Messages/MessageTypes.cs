using System;

namespace BestHTTP.SignalRCore.Messages
{
	// Token: 0x020001FA RID: 506
	public enum MessageTypes
	{
		// Token: 0x0400153D RID: 5437
		Handshake,
		// Token: 0x0400153E RID: 5438
		Invocation,
		// Token: 0x0400153F RID: 5439
		StreamItem,
		// Token: 0x04001540 RID: 5440
		Completion,
		// Token: 0x04001541 RID: 5441
		StreamInvocation,
		// Token: 0x04001542 RID: 5442
		CancelInvocation,
		// Token: 0x04001543 RID: 5443
		Ping,
		// Token: 0x04001544 RID: 5444
		Close
	}
}
