using System;

namespace BestHTTP.SignalRCore.Messages
{
	// Token: 0x020001F7 RID: 503
	public struct UploadInvocationMessage
	{
		// Token: 0x04001535 RID: 5429
		public MessageTypes type;

		// Token: 0x04001536 RID: 5430
		public string invocationId;

		// Token: 0x04001537 RID: 5431
		public bool nonblocking;

		// Token: 0x04001538 RID: 5432
		public string target;

		// Token: 0x04001539 RID: 5433
		public object[] arguments;

		// Token: 0x0400153A RID: 5434
		public int[] streamIds;
	}
}
