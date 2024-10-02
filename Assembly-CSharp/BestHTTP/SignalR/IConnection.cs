using System;
using BestHTTP.SignalR.JsonEncoders;
using BestHTTP.SignalR.Messages;
using BestHTTP.SignalR.Transports;

namespace BestHTTP.SignalR
{
	// Token: 0x02000206 RID: 518
	public interface IConnection
	{
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060012BB RID: 4795
		ProtocolVersions Protocol { get; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060012BC RID: 4796
		NegotiationData NegotiationResult { get; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060012BD RID: 4797
		// (set) Token: 0x060012BE RID: 4798
		IJsonEncoder JsonEncoder { get; set; }

		// Token: 0x060012BF RID: 4799
		void OnMessage(IServerMessage msg);

		// Token: 0x060012C0 RID: 4800
		void TransportStarted();

		// Token: 0x060012C1 RID: 4801
		void TransportReconnected();

		// Token: 0x060012C2 RID: 4802
		void TransportAborted();

		// Token: 0x060012C3 RID: 4803
		void Error(string reason);

		// Token: 0x060012C4 RID: 4804
		Uri BuildUri(RequestTypes type);

		// Token: 0x060012C5 RID: 4805
		Uri BuildUri(RequestTypes type, TransportBase transport);

		// Token: 0x060012C6 RID: 4806
		HTTPRequest PrepareRequest(HTTPRequest req, RequestTypes type);

		// Token: 0x060012C7 RID: 4807
		string ParseResponse(string responseStr);
	}
}
