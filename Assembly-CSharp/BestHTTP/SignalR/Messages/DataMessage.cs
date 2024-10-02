using System;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x0200021A RID: 538
	public sealed class DataMessage : IServerMessage
	{
		// Token: 0x1700022C RID: 556
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x0006AE98 File Offset: 0x00069098
		MessageTypes IServerMessage.Type
		{
			get
			{
				return MessageTypes.Data;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x0600138E RID: 5006 RVA: 0x000A8880 File Offset: 0x000A6A80
		// (set) Token: 0x0600138F RID: 5007 RVA: 0x000A8888 File Offset: 0x000A6A88
		public object Data { get; private set; }

		// Token: 0x06001390 RID: 5008 RVA: 0x000A8891 File Offset: 0x000A6A91
		void IServerMessage.Parse(object data)
		{
			this.Data = data;
		}
	}
}
