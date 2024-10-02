using System;
using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Hubs
{
	// Token: 0x02000228 RID: 552
	public interface IHub
	{
		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060013EF RID: 5103
		// (set) Token: 0x060013F0 RID: 5104
		Connection Connection { get; set; }

		// Token: 0x060013F1 RID: 5105
		bool Call(ClientMessage msg);

		// Token: 0x060013F2 RID: 5106
		bool HasSentMessageId(ulong id);

		// Token: 0x060013F3 RID: 5107
		void Close();

		// Token: 0x060013F4 RID: 5108
		void OnMethod(MethodCallMessage msg);

		// Token: 0x060013F5 RID: 5109
		void OnMessage(IServerMessage msg);
	}
}
