using System;

namespace BestHTTP.WebSocket.Frames
{
	// Token: 0x020001BD RID: 445
	public enum WebSocketFrameTypes : byte
	{
		// Token: 0x04001447 RID: 5191
		Continuation,
		// Token: 0x04001448 RID: 5192
		Text,
		// Token: 0x04001449 RID: 5193
		Binary,
		// Token: 0x0400144A RID: 5194
		ConnectionClose = 8,
		// Token: 0x0400144B RID: 5195
		Ping,
		// Token: 0x0400144C RID: 5196
		Pong
	}
}
