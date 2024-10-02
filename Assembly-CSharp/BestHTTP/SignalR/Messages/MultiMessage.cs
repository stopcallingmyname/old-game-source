using System;
using System.Collections;
using System.Collections.Generic;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x02000219 RID: 537
	public sealed class MultiMessage : IServerMessage
	{
		// Token: 0x17000225 RID: 549
		// (get) Token: 0x0600137E RID: 4990 RVA: 0x000A7398 File Offset: 0x000A5598
		MessageTypes IServerMessage.Type
		{
			get
			{
				return MessageTypes.Multiple;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x000A8684 File Offset: 0x000A6884
		// (set) Token: 0x06001380 RID: 4992 RVA: 0x000A868C File Offset: 0x000A688C
		public string MessageId { get; private set; }

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06001381 RID: 4993 RVA: 0x000A8695 File Offset: 0x000A6895
		// (set) Token: 0x06001382 RID: 4994 RVA: 0x000A869D File Offset: 0x000A689D
		public bool IsInitialization { get; private set; }

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06001383 RID: 4995 RVA: 0x000A86A6 File Offset: 0x000A68A6
		// (set) Token: 0x06001384 RID: 4996 RVA: 0x000A86AE File Offset: 0x000A68AE
		public string GroupsToken { get; private set; }

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06001385 RID: 4997 RVA: 0x000A86B7 File Offset: 0x000A68B7
		// (set) Token: 0x06001386 RID: 4998 RVA: 0x000A86BF File Offset: 0x000A68BF
		public bool ShouldReconnect { get; private set; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06001387 RID: 4999 RVA: 0x000A86C8 File Offset: 0x000A68C8
		// (set) Token: 0x06001388 RID: 5000 RVA: 0x000A86D0 File Offset: 0x000A68D0
		public TimeSpan? PollDelay { get; private set; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06001389 RID: 5001 RVA: 0x000A86D9 File Offset: 0x000A68D9
		// (set) Token: 0x0600138A RID: 5002 RVA: 0x000A86E1 File Offset: 0x000A68E1
		public List<IServerMessage> Data { get; private set; }

		// Token: 0x0600138B RID: 5003 RVA: 0x000A86EC File Offset: 0x000A68EC
		void IServerMessage.Parse(object data)
		{
			IDictionary<string, object> dictionary = data as IDictionary<string, object>;
			this.MessageId = dictionary["C"].ToString();
			object obj;
			if (dictionary.TryGetValue("S", out obj))
			{
				this.IsInitialization = (int.Parse(obj.ToString()) == 1);
			}
			else
			{
				this.IsInitialization = false;
			}
			if (dictionary.TryGetValue("G", out obj))
			{
				this.GroupsToken = obj.ToString();
			}
			if (dictionary.TryGetValue("T", out obj))
			{
				this.ShouldReconnect = (int.Parse(obj.ToString()) == 1);
			}
			else
			{
				this.ShouldReconnect = false;
			}
			if (dictionary.TryGetValue("L", out obj))
			{
				this.PollDelay = new TimeSpan?(TimeSpan.FromMilliseconds(double.Parse(obj.ToString())));
			}
			IEnumerable enumerable = dictionary["M"] as IEnumerable;
			if (enumerable != null)
			{
				this.Data = new List<IServerMessage>();
				foreach (object obj2 in enumerable)
				{
					IDictionary<string, object> dictionary2 = obj2 as IDictionary<string, object>;
					IServerMessage serverMessage;
					if (dictionary2 != null)
					{
						if (dictionary2.ContainsKey("H"))
						{
							serverMessage = new MethodCallMessage();
						}
						else if (dictionary2.ContainsKey("I"))
						{
							serverMessage = new ProgressMessage();
						}
						else
						{
							serverMessage = new DataMessage();
						}
					}
					else
					{
						serverMessage = new DataMessage();
					}
					serverMessage.Parse(obj2);
					this.Data.Add(serverMessage);
				}
			}
		}
	}
}
