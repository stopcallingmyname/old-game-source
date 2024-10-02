using System;

namespace BestHTTP.SignalRCore.Messages
{
	// Token: 0x020001F9 RID: 505
	public struct PingMessage
	{
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06001280 RID: 4736 RVA: 0x000A4E21 File Offset: 0x000A3021
		public MessageTypes type
		{
			get
			{
				return MessageTypes.Ping;
			}
		}
	}
}
