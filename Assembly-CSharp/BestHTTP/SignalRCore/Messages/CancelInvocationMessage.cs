using System;

namespace BestHTTP.SignalRCore.Messages
{
	// Token: 0x020001F8 RID: 504
	public struct CancelInvocationMessage
	{
		// Token: 0x170001ED RID: 493
		// (get) Token: 0x0600127F RID: 4735 RVA: 0x000A4E1E File Offset: 0x000A301E
		public MessageTypes type
		{
			get
			{
				return MessageTypes.CancelInvocation;
			}
		}

		// Token: 0x0400153B RID: 5435
		public string invocationId;
	}
}
