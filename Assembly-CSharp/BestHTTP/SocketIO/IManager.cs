using System;
using BestHTTP.SocketIO.Transports;

namespace BestHTTP.SocketIO
{
	// Token: 0x020001C7 RID: 455
	public interface IManager
	{
		// Token: 0x060010B2 RID: 4274
		void Remove(Socket socket);

		// Token: 0x060010B3 RID: 4275
		void Close(bool removeSockets = true);

		// Token: 0x060010B4 RID: 4276
		void TryToReconnect();

		// Token: 0x060010B5 RID: 4277
		bool OnTransportConnected(ITransport transport);

		// Token: 0x060010B6 RID: 4278
		void OnTransportError(ITransport trans, string err);

		// Token: 0x060010B7 RID: 4279
		void OnTransportProbed(ITransport trans);

		// Token: 0x060010B8 RID: 4280
		void SendPacket(Packet packet);

		// Token: 0x060010B9 RID: 4281
		void OnPacket(Packet packet);

		// Token: 0x060010BA RID: 4282
		void EmitEvent(string eventName, params object[] args);

		// Token: 0x060010BB RID: 4283
		void EmitEvent(SocketIOEventTypes type, params object[] args);

		// Token: 0x060010BC RID: 4284
		void EmitError(SocketIOErrors errCode, string msg);

		// Token: 0x060010BD RID: 4285
		void EmitAll(string eventName, params object[] args);
	}
}
