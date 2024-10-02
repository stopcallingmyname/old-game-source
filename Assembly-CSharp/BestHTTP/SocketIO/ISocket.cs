using System;

namespace BestHTTP.SocketIO
{
	// Token: 0x020001C8 RID: 456
	public interface ISocket
	{
		// Token: 0x060010BE RID: 4286
		void Open();

		// Token: 0x060010BF RID: 4287
		void Disconnect(bool remove);

		// Token: 0x060010C0 RID: 4288
		void OnPacket(Packet packet);

		// Token: 0x060010C1 RID: 4289
		void EmitEvent(SocketIOEventTypes type, params object[] args);

		// Token: 0x060010C2 RID: 4290
		void EmitEvent(string eventName, params object[] args);

		// Token: 0x060010C3 RID: 4291
		void EmitError(SocketIOErrors errCode, string msg);
	}
}
