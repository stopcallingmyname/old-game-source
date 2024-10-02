using System;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x02000218 RID: 536
	public sealed class KeepAliveMessage : IServerMessage
	{
		// Token: 0x17000224 RID: 548
		// (get) Token: 0x0600137B RID: 4987 RVA: 0x0007D96F File Offset: 0x0007BB6F
		MessageTypes IServerMessage.Type
		{
			get
			{
				return MessageTypes.KeepAlive;
			}
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x0000248C File Offset: 0x0000068C
		void IServerMessage.Parse(object data)
		{
		}
	}
}
