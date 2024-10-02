using System;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x02000216 RID: 534
	public interface IServerMessage
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06001378 RID: 4984
		MessageTypes Type { get; }

		// Token: 0x06001379 RID: 4985
		void Parse(object data);
	}
}
