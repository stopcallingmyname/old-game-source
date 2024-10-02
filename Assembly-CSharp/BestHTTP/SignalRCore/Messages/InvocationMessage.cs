using System;

namespace BestHTTP.SignalRCore.Messages
{
	// Token: 0x020001F6 RID: 502
	public struct InvocationMessage
	{
		// Token: 0x04001530 RID: 5424
		public MessageTypes type;

		// Token: 0x04001531 RID: 5425
		public string invocationId;

		// Token: 0x04001532 RID: 5426
		public bool nonblocking;

		// Token: 0x04001533 RID: 5427
		public string target;

		// Token: 0x04001534 RID: 5428
		public object[] arguments;
	}
}
