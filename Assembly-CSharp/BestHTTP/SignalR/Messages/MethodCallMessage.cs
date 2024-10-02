using System;
using System.Collections;
using System.Collections.Generic;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x0200021B RID: 539
	public sealed class MethodCallMessage : IServerMessage
	{
		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06001392 RID: 5010 RVA: 0x000A4E1E File Offset: 0x000A301E
		MessageTypes IServerMessage.Type
		{
			get
			{
				return MessageTypes.MethodCall;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06001393 RID: 5011 RVA: 0x000A889A File Offset: 0x000A6A9A
		// (set) Token: 0x06001394 RID: 5012 RVA: 0x000A88A2 File Offset: 0x000A6AA2
		public string Hub { get; private set; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06001395 RID: 5013 RVA: 0x000A88AB File Offset: 0x000A6AAB
		// (set) Token: 0x06001396 RID: 5014 RVA: 0x000A88B3 File Offset: 0x000A6AB3
		public string Method { get; private set; }

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06001397 RID: 5015 RVA: 0x000A88BC File Offset: 0x000A6ABC
		// (set) Token: 0x06001398 RID: 5016 RVA: 0x000A88C4 File Offset: 0x000A6AC4
		public object[] Arguments { get; private set; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06001399 RID: 5017 RVA: 0x000A88CD File Offset: 0x000A6ACD
		// (set) Token: 0x0600139A RID: 5018 RVA: 0x000A88D5 File Offset: 0x000A6AD5
		public IDictionary<string, object> State { get; private set; }

		// Token: 0x0600139B RID: 5019 RVA: 0x000A88E0 File Offset: 0x000A6AE0
		void IServerMessage.Parse(object data)
		{
			IDictionary<string, object> dictionary = data as IDictionary<string, object>;
			this.Hub = dictionary["H"].ToString();
			this.Method = dictionary["M"].ToString();
			List<object> list = new List<object>();
			foreach (object item in (dictionary["A"] as IEnumerable))
			{
				list.Add(item);
			}
			this.Arguments = list.ToArray();
			object obj;
			if (dictionary.TryGetValue("S", out obj))
			{
				this.State = (obj as IDictionary<string, object>);
			}
		}
	}
}
