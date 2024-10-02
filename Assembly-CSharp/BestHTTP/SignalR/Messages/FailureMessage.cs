using System;
using System.Collections.Generic;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x0200021D RID: 541
	public sealed class FailureMessage : IServerMessage, IHubMessage
	{
		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060013A6 RID: 5030 RVA: 0x000A8A3C File Offset: 0x000A6C3C
		MessageTypes IServerMessage.Type
		{
			get
			{
				return MessageTypes.Failure;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060013A7 RID: 5031 RVA: 0x000A8A3F File Offset: 0x000A6C3F
		// (set) Token: 0x060013A8 RID: 5032 RVA: 0x000A8A47 File Offset: 0x000A6C47
		public ulong InvocationId { get; private set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060013A9 RID: 5033 RVA: 0x000A8A50 File Offset: 0x000A6C50
		// (set) Token: 0x060013AA RID: 5034 RVA: 0x000A8A58 File Offset: 0x000A6C58
		public bool IsHubError { get; private set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060013AB RID: 5035 RVA: 0x000A8A61 File Offset: 0x000A6C61
		// (set) Token: 0x060013AC RID: 5036 RVA: 0x000A8A69 File Offset: 0x000A6C69
		public string ErrorMessage { get; private set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060013AD RID: 5037 RVA: 0x000A8A72 File Offset: 0x000A6C72
		// (set) Token: 0x060013AE RID: 5038 RVA: 0x000A8A7A File Offset: 0x000A6C7A
		public IDictionary<string, object> AdditionalData { get; private set; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060013AF RID: 5039 RVA: 0x000A8A83 File Offset: 0x000A6C83
		// (set) Token: 0x060013B0 RID: 5040 RVA: 0x000A8A8B File Offset: 0x000A6C8B
		public string StackTrace { get; private set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060013B1 RID: 5041 RVA: 0x000A8A94 File Offset: 0x000A6C94
		// (set) Token: 0x060013B2 RID: 5042 RVA: 0x000A8A9C File Offset: 0x000A6C9C
		public IDictionary<string, object> State { get; private set; }

		// Token: 0x060013B3 RID: 5043 RVA: 0x000A8AA8 File Offset: 0x000A6CA8
		void IServerMessage.Parse(object data)
		{
			IDictionary<string, object> dictionary = data as IDictionary<string, object>;
			this.InvocationId = ulong.Parse(dictionary["I"].ToString());
			object obj;
			if (dictionary.TryGetValue("E", out obj))
			{
				this.ErrorMessage = obj.ToString();
			}
			if (dictionary.TryGetValue("H", out obj))
			{
				this.IsHubError = (int.Parse(obj.ToString()) == 1);
			}
			if (dictionary.TryGetValue("D", out obj))
			{
				this.AdditionalData = (obj as IDictionary<string, object>);
			}
			if (dictionary.TryGetValue("T", out obj))
			{
				this.StackTrace = obj.ToString();
			}
			if (dictionary.TryGetValue("S", out obj))
			{
				this.State = (obj as IDictionary<string, object>);
			}
		}
	}
}
