using System;

namespace BestHTTP.SignalRCore.Messages
{
	// Token: 0x020001F3 RID: 499
	public struct CompletionWithResult
	{
		// Token: 0x04001527 RID: 5415
		public MessageTypes type;

		// Token: 0x04001528 RID: 5416
		public string invocationId;

		// Token: 0x04001529 RID: 5417
		public object result;
	}
}
