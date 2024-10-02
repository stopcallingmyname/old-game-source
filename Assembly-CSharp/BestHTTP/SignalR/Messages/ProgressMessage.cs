using System;
using System.Collections.Generic;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x0200021E RID: 542
	public sealed class ProgressMessage : IServerMessage, IHubMessage
	{
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060013B5 RID: 5045 RVA: 0x000A4E21 File Offset: 0x000A3021
		MessageTypes IServerMessage.Type
		{
			get
			{
				return MessageTypes.Progress;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060013B6 RID: 5046 RVA: 0x000A8B6A File Offset: 0x000A6D6A
		// (set) Token: 0x060013B7 RID: 5047 RVA: 0x000A8B72 File Offset: 0x000A6D72
		public ulong InvocationId { get; private set; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060013B8 RID: 5048 RVA: 0x000A8B7B File Offset: 0x000A6D7B
		// (set) Token: 0x060013B9 RID: 5049 RVA: 0x000A8B83 File Offset: 0x000A6D83
		public double Progress { get; private set; }

		// Token: 0x060013BA RID: 5050 RVA: 0x000A8B8C File Offset: 0x000A6D8C
		void IServerMessage.Parse(object data)
		{
			IDictionary<string, object> dictionary = (data as IDictionary<string, object>)["P"] as IDictionary<string, object>;
			this.InvocationId = ulong.Parse(dictionary["I"].ToString());
			this.Progress = double.Parse(dictionary["D"].ToString());
		}
	}
}
