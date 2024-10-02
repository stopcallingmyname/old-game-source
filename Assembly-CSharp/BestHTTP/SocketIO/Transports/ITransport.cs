using System;
using System.Collections.Generic;

namespace BestHTTP.SocketIO.Transports
{
	// Token: 0x020001CF RID: 463
	public interface ITransport
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06001155 RID: 4437
		TransportTypes Type { get; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06001156 RID: 4438
		TransportStates State { get; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06001157 RID: 4439
		SocketManager Manager { get; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06001158 RID: 4440
		bool IsRequestInProgress { get; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06001159 RID: 4441
		bool IsPollingInProgress { get; }

		// Token: 0x0600115A RID: 4442
		void Open();

		// Token: 0x0600115B RID: 4443
		void Poll();

		// Token: 0x0600115C RID: 4444
		void Send(Packet packet);

		// Token: 0x0600115D RID: 4445
		void Send(List<Packet> packets);

		// Token: 0x0600115E RID: 4446
		void Close();
	}
}
