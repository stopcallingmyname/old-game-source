using System;
using BestHTTP.SignalR.Messages;
using BestHTTP.WebSocket;

namespace BestHTTP.SignalR.Transports
{
	// Token: 0x02000214 RID: 532
	public sealed class WebSocketTransport : TransportBase
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x0600136B RID: 4971 RVA: 0x0006AE98 File Offset: 0x00069098
		public override bool SupportsKeepAlive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x0600136C RID: 4972 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override TransportTypes Type
		{
			get
			{
				return TransportTypes.WebSocket;
			}
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x000A8358 File Offset: 0x000A6558
		public WebSocketTransport(Connection connection) : base("webSockets", connection)
		{
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x000A8368 File Offset: 0x000A6568
		public override void Connect()
		{
			if (this.wSocket != null)
			{
				HTTPManager.Logger.Warning("WebSocketTransport", "Start - WebSocket already created!");
				return;
			}
			if (base.State != TransportStates.Reconnecting)
			{
				base.State = TransportStates.Connecting;
			}
			RequestTypes type = (base.State == TransportStates.Reconnecting) ? RequestTypes.Reconnect : RequestTypes.Connect;
			Uri uri = base.Connection.BuildUri(type, this);
			this.wSocket = new WebSocket(uri);
			WebSocket webSocket = this.wSocket;
			webSocket.OnOpen = (OnWebSocketOpenDelegate)Delegate.Combine(webSocket.OnOpen, new OnWebSocketOpenDelegate(this.WSocket_OnOpen));
			WebSocket webSocket2 = this.wSocket;
			webSocket2.OnMessage = (OnWebSocketMessageDelegate)Delegate.Combine(webSocket2.OnMessage, new OnWebSocketMessageDelegate(this.WSocket_OnMessage));
			WebSocket webSocket3 = this.wSocket;
			webSocket3.OnClosed = (OnWebSocketClosedDelegate)Delegate.Combine(webSocket3.OnClosed, new OnWebSocketClosedDelegate(this.WSocket_OnClosed));
			WebSocket webSocket4 = this.wSocket;
			webSocket4.OnErrorDesc = (OnWebSocketErrorDescriptionDelegate)Delegate.Combine(webSocket4.OnErrorDesc, new OnWebSocketErrorDescriptionDelegate(this.WSocket_OnError));
			base.Connection.PrepareRequest(this.wSocket.InternalRequest, type);
			this.wSocket.Open();
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x000A8489 File Offset: 0x000A6689
		protected override void SendImpl(string json)
		{
			if (this.wSocket != null && this.wSocket.IsOpen)
			{
				this.wSocket.Send(json);
			}
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x000A84AC File Offset: 0x000A66AC
		public override void Stop()
		{
			if (this.wSocket != null)
			{
				this.wSocket.OnOpen = null;
				this.wSocket.OnMessage = null;
				this.wSocket.OnClosed = null;
				this.wSocket.OnErrorDesc = null;
				this.wSocket.Close();
				this.wSocket = null;
			}
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x0000248C File Offset: 0x0000068C
		protected override void Started()
		{
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x000A8503 File Offset: 0x000A6703
		protected override void Aborted()
		{
			if (this.wSocket != null && this.wSocket.IsOpen)
			{
				this.wSocket.Close();
				this.wSocket = null;
			}
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x000A852C File Offset: 0x000A672C
		private void WSocket_OnOpen(WebSocket webSocket)
		{
			if (webSocket != this.wSocket)
			{
				return;
			}
			HTTPManager.Logger.Information("WebSocketTransport", "WSocket_OnOpen");
			base.OnConnected();
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x000A8554 File Offset: 0x000A6754
		private void WSocket_OnMessage(WebSocket webSocket, string message)
		{
			if (webSocket != this.wSocket)
			{
				return;
			}
			IServerMessage serverMessage = TransportBase.Parse(base.Connection.JsonEncoder, message);
			if (serverMessage != null)
			{
				base.Connection.OnMessage(serverMessage);
			}
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x000A858C File Offset: 0x000A678C
		private void WSocket_OnClosed(WebSocket webSocket, ushort code, string message)
		{
			if (webSocket != this.wSocket)
			{
				return;
			}
			string text = code.ToString() + " : " + message;
			HTTPManager.Logger.Information("WebSocketTransport", "WSocket_OnClosed " + text);
			if (base.State == TransportStates.Closing)
			{
				base.State = TransportStates.Closed;
				return;
			}
			base.Connection.Error(text);
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x000A85F0 File Offset: 0x000A67F0
		private void WSocket_OnError(WebSocket webSocket, string reason)
		{
			if (webSocket != this.wSocket)
			{
				return;
			}
			if (base.State == TransportStates.Closing || base.State == TransportStates.Closed)
			{
				base.AbortFinished();
				return;
			}
			HTTPManager.Logger.Error("WebSocketTransport", "WSocket_OnError " + reason);
			base.State = TransportStates.Closed;
			base.Connection.Error(reason);
		}

		// Token: 0x040015C3 RID: 5571
		private WebSocket wSocket;
	}
}
