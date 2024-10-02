using System;

namespace BestHTTP.SocketIO
{
	// Token: 0x020001C3 RID: 451
	public enum SocketIOEventTypes
	{
		// Token: 0x04001473 RID: 5235
		Unknown = -1,
		// Token: 0x04001474 RID: 5236
		Connect,
		// Token: 0x04001475 RID: 5237
		Disconnect,
		// Token: 0x04001476 RID: 5238
		Event,
		// Token: 0x04001477 RID: 5239
		Ack,
		// Token: 0x04001478 RID: 5240
		Error,
		// Token: 0x04001479 RID: 5241
		BinaryEvent,
		// Token: 0x0400147A RID: 5242
		BinaryAck
	}
}
