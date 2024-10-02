using System;
using System.Collections.Generic;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x0200021C RID: 540
	public sealed class ResultMessage : IServerMessage, IHubMessage
	{
		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600139D RID: 5021 RVA: 0x000A89A4 File Offset: 0x000A6BA4
		MessageTypes IServerMessage.Type
		{
			get
			{
				return MessageTypes.Result;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600139E RID: 5022 RVA: 0x000A89A7 File Offset: 0x000A6BA7
		// (set) Token: 0x0600139F RID: 5023 RVA: 0x000A89AF File Offset: 0x000A6BAF
		public ulong InvocationId { get; private set; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060013A0 RID: 5024 RVA: 0x000A89B8 File Offset: 0x000A6BB8
		// (set) Token: 0x060013A1 RID: 5025 RVA: 0x000A89C0 File Offset: 0x000A6BC0
		public object ReturnValue { get; private set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060013A2 RID: 5026 RVA: 0x000A89C9 File Offset: 0x000A6BC9
		// (set) Token: 0x060013A3 RID: 5027 RVA: 0x000A89D1 File Offset: 0x000A6BD1
		public IDictionary<string, object> State { get; private set; }

		// Token: 0x060013A4 RID: 5028 RVA: 0x000A89DC File Offset: 0x000A6BDC
		void IServerMessage.Parse(object data)
		{
			IDictionary<string, object> dictionary = data as IDictionary<string, object>;
			this.InvocationId = ulong.Parse(dictionary["I"].ToString());
			object obj;
			if (dictionary.TryGetValue("R", out obj))
			{
				this.ReturnValue = obj;
			}
			if (dictionary.TryGetValue("S", out obj))
			{
				this.State = (obj as IDictionary<string, object>);
			}
		}
	}
}
