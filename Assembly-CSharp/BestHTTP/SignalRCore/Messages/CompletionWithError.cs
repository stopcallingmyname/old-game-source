using System;

namespace BestHTTP.SignalRCore.Messages
{
	// Token: 0x020001F4 RID: 500
	public struct CompletionWithError
	{
		// Token: 0x0400152A RID: 5418
		public MessageTypes type;

		// Token: 0x0400152B RID: 5419
		public string invocationId;

		// Token: 0x0400152C RID: 5420
		public string error;
	}
}
