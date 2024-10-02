using System;
using System.Collections.Generic;
using BestHTTP.Extensions;
using BestHTTP.Logger;
using BestHTTP.WebSocket;

namespace BestHTTP.SocketIO.Transports
{
	// Token: 0x020001D1 RID: 465
	internal sealed class WebSocketTransport : ITransport
	{
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06001170 RID: 4464 RVA: 0x0006AE98 File Offset: 0x00069098
		public TransportTypes Type
		{
			get
			{
				return TransportTypes.WebSocket;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06001171 RID: 4465 RVA: 0x000A1B00 File Offset: 0x0009FD00
		// (set) Token: 0x06001172 RID: 4466 RVA: 0x000A1B08 File Offset: 0x0009FD08
		public TransportStates State { get; private set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06001173 RID: 4467 RVA: 0x000A1B11 File Offset: 0x0009FD11
		// (set) Token: 0x06001174 RID: 4468 RVA: 0x000A1B19 File Offset: 0x0009FD19
		public SocketManager Manager { get; private set; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06001175 RID: 4469 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool IsRequestInProgress
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06001176 RID: 4470 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool IsPollingInProgress
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06001177 RID: 4471 RVA: 0x000A1B22 File Offset: 0x0009FD22
		// (set) Token: 0x06001178 RID: 4472 RVA: 0x000A1B2A File Offset: 0x0009FD2A
		public WebSocket Implementation { get; private set; }

		// Token: 0x06001179 RID: 4473 RVA: 0x000A1B33 File Offset: 0x0009FD33
		public WebSocketTransport(SocketManager manager)
		{
			this.State = TransportStates.Closed;
			this.Manager = manager;
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x000A1B4C File Offset: 0x0009FD4C
		public void Open()
		{
			if (this.State != TransportStates.Closed)
			{
				return;
			}
			string text = new UriBuilder(HTTPProtocolFactory.IsSecureProtocol(this.Manager.Uri) ? "wss" : "ws", this.Manager.Uri.Host, this.Manager.Uri.Port, this.Manager.Uri.GetRequestPathAndQueryURL()).Uri.ToString();
			string text2 = "{0}?EIO={1}&transport=websocket{3}";
			if (this.Manager.Handshake != null)
			{
				text2 += "&sid={2}";
			}
			bool flag = !this.Manager.Options.QueryParamsOnlyForHandshake || (this.Manager.Options.QueryParamsOnlyForHandshake && this.Manager.Handshake == null);
			Uri uri = new Uri(string.Format(text2, new object[]
			{
				text,
				4,
				(this.Manager.Handshake != null) ? this.Manager.Handshake.Sid : string.Empty,
				flag ? this.Manager.Options.BuildQueryParams() : string.Empty
			}));
			this.Implementation = new WebSocket(uri);
			this.Implementation.OnOpen = new OnWebSocketOpenDelegate(this.OnOpen);
			this.Implementation.OnMessage = new OnWebSocketMessageDelegate(this.OnMessage);
			this.Implementation.OnBinary = new OnWebSocketBinaryDelegate(this.OnBinary);
			this.Implementation.OnError = new OnWebSocketErrorDelegate(this.OnError);
			this.Implementation.OnClosed = new OnWebSocketClosedDelegate(this.OnClosed);
			this.Implementation.Open();
			this.State = TransportStates.Connecting;
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x000A1D10 File Offset: 0x0009FF10
		public void Close()
		{
			if (this.State == TransportStates.Closed)
			{
				return;
			}
			this.State = TransportStates.Closed;
			if (this.Implementation != null)
			{
				this.Implementation.Close();
			}
			else
			{
				HTTPManager.Logger.Warning("WebSocketTransport", "Close - WebSocket Implementation already null!");
			}
			this.Implementation = null;
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x0000248C File Offset: 0x0000068C
		public void Poll()
		{
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x000A1D60 File Offset: 0x0009FF60
		private void OnOpen(WebSocket ws)
		{
			if (ws != this.Implementation)
			{
				return;
			}
			HTTPManager.Logger.Information("WebSocketTransport", "OnOpen");
			this.State = TransportStates.Opening;
			if (this.Manager.UpgradingTransport == this)
			{
				this.Send(new Packet(TransportEventTypes.Ping, SocketIOEventTypes.Unknown, "/", "probe", 0, 0));
			}
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x000A1DBC File Offset: 0x0009FFBC
		private void OnMessage(WebSocket ws, string message)
		{
			if (ws != this.Implementation)
			{
				return;
			}
			if (HTTPManager.Logger.Level <= Loglevels.All)
			{
				HTTPManager.Logger.Verbose("WebSocketTransport", "OnMessage: " + message);
			}
			Packet packet = null;
			try
			{
				packet = new Packet(message);
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("WebSocketTransport", "OnMessage Packet parsing", ex);
			}
			if (packet == null)
			{
				HTTPManager.Logger.Error("WebSocketTransport", "Message parsing failed. Message: " + message);
				return;
			}
			try
			{
				if (packet.AttachmentCount == 0)
				{
					this.OnPacket(packet);
				}
				else
				{
					this.PacketWithAttachment = packet;
				}
			}
			catch (Exception ex2)
			{
				HTTPManager.Logger.Exception("WebSocketTransport", "OnMessage OnPacket", ex2);
			}
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x000A1E8C File Offset: 0x000A008C
		private void OnBinary(WebSocket ws, byte[] data)
		{
			if (ws != this.Implementation)
			{
				return;
			}
			if (HTTPManager.Logger.Level <= Loglevels.All)
			{
				HTTPManager.Logger.Verbose("WebSocketTransport", "OnBinary");
			}
			if (this.PacketWithAttachment != null)
			{
				this.PacketWithAttachment.AddAttachmentFromServer(data, false);
				if (this.PacketWithAttachment.HasAllAttachment)
				{
					try
					{
						this.OnPacket(this.PacketWithAttachment);
					}
					catch (Exception ex)
					{
						HTTPManager.Logger.Exception("WebSocketTransport", "OnBinary", ex);
					}
					finally
					{
						this.PacketWithAttachment = null;
					}
				}
			}
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x000A1F30 File Offset: 0x000A0130
		private void OnError(WebSocket ws, Exception ex)
		{
			if (ws != this.Implementation)
			{
				return;
			}
			string err = string.Empty;
			if (ex != null)
			{
				err = ex.Message + " " + ex.StackTrace;
			}
			else
			{
				switch (ws.InternalRequest.State)
				{
				case HTTPRequestStates.Finished:
					if (ws.InternalRequest.Response.IsSuccess || ws.InternalRequest.Response.StatusCode == 101)
					{
						err = string.Format("Request finished. Status Code: {0} Message: {1}", ws.InternalRequest.Response.StatusCode.ToString(), ws.InternalRequest.Response.Message);
					}
					else
					{
						err = string.Format("Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message, ws.InternalRequest.Response.DataAsText);
					}
					break;
				case HTTPRequestStates.Error:
					err = (("Request Finished with Error! : " + ws.InternalRequest.Exception != null) ? (ws.InternalRequest.Exception.Message + " " + ws.InternalRequest.Exception.StackTrace) : string.Empty);
					break;
				case HTTPRequestStates.Aborted:
					err = "Request Aborted!";
					break;
				case HTTPRequestStates.ConnectionTimedOut:
					err = "Connection Timed Out!";
					break;
				case HTTPRequestStates.TimedOut:
					err = "Processing the request Timed Out!";
					break;
				}
			}
			if (this.Manager.UpgradingTransport != this)
			{
				((IManager)this.Manager).OnTransportError(this, err);
				return;
			}
			this.Manager.UpgradingTransport = null;
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x000A20C4 File Offset: 0x000A02C4
		private void OnClosed(WebSocket ws, ushort code, string message)
		{
			if (ws != this.Implementation)
			{
				return;
			}
			HTTPManager.Logger.Information("WebSocketTransport", "OnClosed");
			this.Close();
			if (this.Manager.UpgradingTransport != this)
			{
				((IManager)this.Manager).TryToReconnect();
				return;
			}
			this.Manager.UpgradingTransport = null;
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x000A211C File Offset: 0x000A031C
		public void Send(Packet packet)
		{
			if (this.State == TransportStates.Closed || this.State == TransportStates.Paused)
			{
				return;
			}
			string text = packet.Encode();
			if (HTTPManager.Logger.Level <= Loglevels.All)
			{
				HTTPManager.Logger.Verbose("WebSocketTransport", "Send: " + text);
			}
			if (packet.AttachmentCount != 0 || (packet.Attachments != null && packet.Attachments.Count != 0))
			{
				if (packet.Attachments == null)
				{
					throw new ArgumentException("packet.Attachments are null!");
				}
				if (packet.AttachmentCount != packet.Attachments.Count)
				{
					throw new ArgumentException("packet.AttachmentCount != packet.Attachments.Count. Use the packet.AddAttachment function to add data to a packet!");
				}
			}
			this.Implementation.Send(text);
			if (packet.AttachmentCount != 0)
			{
				int num = packet.Attachments[0].Length + 1;
				for (int i = 1; i < packet.Attachments.Count; i++)
				{
					if (packet.Attachments[i].Length + 1 > num)
					{
						num = packet.Attachments[i].Length + 1;
					}
				}
				if (this.Buffer == null || this.Buffer.Length < num)
				{
					Array.Resize<byte>(ref this.Buffer, num);
				}
				for (int j = 0; j < packet.AttachmentCount; j++)
				{
					this.Buffer[0] = 4;
					Array.Copy(packet.Attachments[j], 0, this.Buffer, 1, packet.Attachments[j].Length);
					this.Implementation.Send(this.Buffer, 0UL, (ulong)((long)packet.Attachments[j].Length + 1L));
				}
			}
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x000A22A0 File Offset: 0x000A04A0
		public void Send(List<Packet> packets)
		{
			for (int i = 0; i < packets.Count; i++)
			{
				this.Send(packets[i]);
			}
			packets.Clear();
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x000A22D4 File Offset: 0x000A04D4
		private void OnPacket(Packet packet)
		{
			TransportEventTypes transportEvent = packet.TransportEvent;
			if (transportEvent != TransportEventTypes.Open)
			{
				if (transportEvent == TransportEventTypes.Pong)
				{
					if (packet.Payload == "probe")
					{
						this.State = TransportStates.Open;
						((IManager)this.Manager).OnTransportProbed(this);
					}
				}
			}
			else if (this.State != TransportStates.Opening)
			{
				HTTPManager.Logger.Warning("WebSocketTransport", "Received 'Open' packet while state is '" + this.State.ToString() + "'");
			}
			else
			{
				this.State = TransportStates.Open;
			}
			if (this.Manager.UpgradingTransport != this)
			{
				((IManager)this.Manager).OnPacket(packet);
			}
		}

		// Token: 0x040014CE RID: 5326
		private Packet PacketWithAttachment;

		// Token: 0x040014CF RID: 5327
		private byte[] Buffer;
	}
}
