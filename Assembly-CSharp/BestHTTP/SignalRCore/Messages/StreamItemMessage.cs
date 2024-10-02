using System;

namespace BestHTTP.SignalRCore.Messages
{
	// Token: 0x020001F5 RID: 501
	public struct StreamItemMessage
	{
		// Token: 0x0400152D RID: 5421
		public MessageTypes type;

		// Token: 0x0400152E RID: 5422
		public string invocationId;

		// Token: 0x0400152F RID: 5423
		public object item;
	}
}
