using System;
using System.Collections.Generic;
using System.Threading;
using BestHTTP.Extensions;
using BestHTTP.SocketIO.Events;
using BestHTTP.SocketIO.JsonEncoders;
using BestHTTP.SocketIO.Transports;

namespace BestHTTP.SocketIO
{
	// Token: 0x020001CB RID: 459
	public sealed class SocketManager : IHeartbeat, IManager
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600110F RID: 4367 RVA: 0x000A0422 File Offset: 0x0009E622
		// (set) Token: 0x06001110 RID: 4368 RVA: 0x000A042A File Offset: 0x0009E62A
		public SocketManager.States State
		{
			get
			{
				return this.state;
			}
			private set
			{
				this.PreviousState = this.state;
				this.state = value;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x000A043F File Offset: 0x0009E63F
		// (set) Token: 0x06001112 RID: 4370 RVA: 0x000A0447 File Offset: 0x0009E647
		public SocketOptions Options { get; private set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x000A0450 File Offset: 0x0009E650
		// (set) Token: 0x06001114 RID: 4372 RVA: 0x000A0458 File Offset: 0x0009E658
		public Uri Uri { get; private set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06001115 RID: 4373 RVA: 0x000A0461 File Offset: 0x0009E661
		// (set) Token: 0x06001116 RID: 4374 RVA: 0x000A0469 File Offset: 0x0009E669
		public HandshakeData Handshake { get; private set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06001117 RID: 4375 RVA: 0x000A0472 File Offset: 0x0009E672
		// (set) Token: 0x06001118 RID: 4376 RVA: 0x000A047A File Offset: 0x0009E67A
		public ITransport Transport { get; private set; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06001119 RID: 4377 RVA: 0x000A0483 File Offset: 0x0009E683
		// (set) Token: 0x0600111A RID: 4378 RVA: 0x000A048B File Offset: 0x0009E68B
		public ulong RequestCounter { get; internal set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600111B RID: 4379 RVA: 0x000A0494 File Offset: 0x0009E694
		public Socket Socket
		{
			get
			{
				return this.GetSocket();
			}
		}

		// Token: 0x1700019E RID: 414
		public Socket this[string nsp]
		{
			get
			{
				return this.GetSocket(nsp);
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600111D RID: 4381 RVA: 0x000A04A5 File Offset: 0x0009E6A5
		// (set) Token: 0x0600111E RID: 4382 RVA: 0x000A04AD File Offset: 0x0009E6AD
		public int ReconnectAttempts { get; private set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600111F RID: 4383 RVA: 0x000A04B6 File Offset: 0x0009E6B6
		// (set) Token: 0x06001120 RID: 4384 RVA: 0x000A04BE File Offset: 0x0009E6BE
		public IJsonEncoder Encoder { get; set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06001121 RID: 4385 RVA: 0x000A04C8 File Offset: 0x0009E6C8
		internal uint Timestamp
		{
			get
			{
				return (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06001122 RID: 4386 RVA: 0x000A04F7 File Offset: 0x0009E6F7
		internal int NextAckId
		{
			get
			{
				return Interlocked.Increment(ref this.nextAckId);
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06001123 RID: 4387 RVA: 0x000A0504 File Offset: 0x0009E704
		// (set) Token: 0x06001124 RID: 4388 RVA: 0x000A050C File Offset: 0x0009E70C
		internal SocketManager.States PreviousState { get; private set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06001125 RID: 4389 RVA: 0x000A0515 File Offset: 0x0009E715
		// (set) Token: 0x06001126 RID: 4390 RVA: 0x000A051D File Offset: 0x0009E71D
		internal ITransport UpgradingTransport { get; set; }

		// Token: 0x06001127 RID: 4391 RVA: 0x000A0526 File Offset: 0x0009E726
		public SocketManager(Uri uri) : this(uri, new SocketOptions())
		{
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x000A0534 File Offset: 0x0009E734
		public SocketManager(Uri uri, SocketOptions options)
		{
			this.Uri = uri;
			this.Options = options;
			this.State = SocketManager.States.Initial;
			this.PreviousState = SocketManager.States.Initial;
			this.Encoder = SocketManager.DefaultEncoder;
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x000A058F File Offset: 0x0009E78F
		public Socket GetSocket()
		{
			return this.GetSocket("/");
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x000A059C File Offset: 0x0009E79C
		public Socket GetSocket(string nsp)
		{
			if (string.IsNullOrEmpty(nsp))
			{
				throw new ArgumentNullException("Namespace parameter is null or empty!");
			}
			Socket socket = null;
			if (!this.Namespaces.TryGetValue(nsp, out socket))
			{
				socket = new Socket(nsp, this);
				this.Namespaces.Add(nsp, socket);
				this.Sockets.Add(socket);
				((ISocket)socket).Open();
			}
			return socket;
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x000A05F6 File Offset: 0x0009E7F6
		void IManager.Remove(Socket socket)
		{
			this.Namespaces.Remove(socket.Namespace);
			this.Sockets.Remove(socket);
			if (this.Sockets.Count == 0)
			{
				this.Close();
			}
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x000A062C File Offset: 0x0009E82C
		public void Open()
		{
			if (this.State != SocketManager.States.Initial && this.State != SocketManager.States.Closed && this.State != SocketManager.States.Reconnecting)
			{
				return;
			}
			HTTPManager.Logger.Information("SocketManager", "Opening");
			this.ReconnectAt = DateTime.MinValue;
			TransportTypes connectWith = this.Options.ConnectWith;
			if (connectWith != TransportTypes.Polling)
			{
				if (connectWith == TransportTypes.WebSocket)
				{
					this.Transport = new WebSocketTransport(this);
				}
			}
			else
			{
				this.Transport = new PollingTransport(this);
			}
			this.Transport.Open();
			((IManager)this).EmitEvent("connecting", Array.Empty<object>());
			this.State = SocketManager.States.Opening;
			this.ConnectionStarted = DateTime.UtcNow;
			HTTPManager.Heartbeats.Subscribe(this);
			this.GetSocket("/");
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x000A06E6 File Offset: 0x0009E8E6
		public void Close()
		{
			((IManager)this).Close(true);
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x000A06F0 File Offset: 0x0009E8F0
		void IManager.Close(bool removeSockets)
		{
			if (this.State == SocketManager.States.Closed || this.closing)
			{
				return;
			}
			this.closing = true;
			HTTPManager.Logger.Information("SocketManager", "Closing");
			HTTPManager.Heartbeats.Unsubscribe(this);
			if (removeSockets)
			{
				while (this.Sockets.Count > 0)
				{
					((ISocket)this.Sockets[this.Sockets.Count - 1]).Disconnect(removeSockets);
				}
			}
			else
			{
				for (int i = 0; i < this.Sockets.Count; i++)
				{
					((ISocket)this.Sockets[i]).Disconnect(removeSockets);
				}
			}
			this.State = SocketManager.States.Closed;
			this.LastHeartbeat = DateTime.MinValue;
			if (this.OfflinePackets != null)
			{
				this.OfflinePackets.Clear();
			}
			if (removeSockets)
			{
				this.Namespaces.Clear();
			}
			this.Handshake = null;
			if (this.Transport != null)
			{
				this.Transport.Close();
			}
			this.Transport = null;
			this.closing = false;
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x000A07EC File Offset: 0x0009E9EC
		void IManager.TryToReconnect()
		{
			if (this.State == SocketManager.States.Reconnecting || this.State == SocketManager.States.Closed)
			{
				return;
			}
			if (!this.Options.Reconnection || HTTPManager.IsQuitting)
			{
				this.Close();
				return;
			}
			int num = this.ReconnectAttempts + 1;
			this.ReconnectAttempts = num;
			if (num >= this.Options.ReconnectionAttempts)
			{
				((IManager)this).EmitEvent("reconnect_failed", Array.Empty<object>());
				this.Close();
				return;
			}
			Random random = new Random();
			int num2 = (int)this.Options.ReconnectionDelay.TotalMilliseconds * this.ReconnectAttempts;
			this.ReconnectAt = DateTime.UtcNow + TimeSpan.FromMilliseconds((double)Math.Min(random.Next((int)((float)num2 - (float)num2 * this.Options.RandomizationFactor), (int)((float)num2 + (float)num2 * this.Options.RandomizationFactor)), (int)this.Options.ReconnectionDelayMax.TotalMilliseconds));
			((IManager)this).Close(false);
			this.State = SocketManager.States.Reconnecting;
			for (int i = 0; i < this.Sockets.Count; i++)
			{
				((ISocket)this.Sockets[i]).Open();
			}
			HTTPManager.Heartbeats.Subscribe(this);
			HTTPManager.Logger.Information("SocketManager", "Reconnecting");
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x000A0930 File Offset: 0x0009EB30
		bool IManager.OnTransportConnected(ITransport trans)
		{
			if (this.State != SocketManager.States.Opening)
			{
				return false;
			}
			if (this.PreviousState == SocketManager.States.Reconnecting)
			{
				((IManager)this).EmitEvent("reconnect", Array.Empty<object>());
			}
			this.State = SocketManager.States.Open;
			this.ReconnectAttempts = 0;
			this.SendOfflinePackets();
			HTTPManager.Logger.Information("SocketManager", "Open");
			if (this.Transport.Type != TransportTypes.WebSocket && this.Handshake.Upgrades.Contains("websocket"))
			{
				this.UpgradingTransport = new WebSocketTransport(this);
				this.UpgradingTransport.Open();
			}
			return true;
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x000A09C6 File Offset: 0x0009EBC6
		void IManager.OnTransportError(ITransport trans, string err)
		{
			((IManager)this).EmitError(SocketIOErrors.Internal, err);
			trans.Close();
			((IManager)this).TryToReconnect();
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x000A09DC File Offset: 0x0009EBDC
		void IManager.OnTransportProbed(ITransport trans)
		{
			HTTPManager.Logger.Information("SocketManager", "\"probe\" packet received");
			this.Options.ConnectWith = trans.Type;
			this.State = SocketManager.States.Paused;
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x000A0A0A File Offset: 0x0009EC0A
		private ITransport SelectTransport()
		{
			if (this.State != SocketManager.States.Open || this.Transport == null)
			{
				return null;
			}
			if (!this.Transport.IsRequestInProgress)
			{
				return this.Transport;
			}
			return null;
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x000A0A34 File Offset: 0x0009EC34
		private void SendOfflinePackets()
		{
			ITransport transport = this.SelectTransport();
			if (this.OfflinePackets != null && this.OfflinePackets.Count > 0 && transport != null)
			{
				transport.Send(this.OfflinePackets);
				this.OfflinePackets.Clear();
			}
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x000A0A78 File Offset: 0x0009EC78
		void IManager.SendPacket(Packet packet)
		{
			ITransport transport = this.SelectTransport();
			if (transport != null)
			{
				try
				{
					transport.Send(packet);
					return;
				}
				catch (Exception ex)
				{
					((IManager)this).EmitError(SocketIOErrors.Internal, ex.Message + " " + ex.StackTrace);
					return;
				}
			}
			if (this.OfflinePackets == null)
			{
				this.OfflinePackets = new List<Packet>();
			}
			this.OfflinePackets.Add(packet.Clone());
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x000A0AEC File Offset: 0x0009ECEC
		void IManager.OnPacket(Packet packet)
		{
			if (this.State == SocketManager.States.Closed)
			{
				return;
			}
			switch (packet.TransportEvent)
			{
			case TransportEventTypes.Open:
				if (this.Handshake == null)
				{
					this.Handshake = new HandshakeData();
					if (!this.Handshake.Parse(packet.Payload))
					{
						HTTPManager.Logger.Warning("SocketManager", "Expected handshake data, but wasn't able to pars. Payload: " + packet.Payload);
					}
					((IManager)this).OnTransportConnected(this.Transport);
					return;
				}
				break;
			case TransportEventTypes.Ping:
				((IManager)this).SendPacket(new Packet(TransportEventTypes.Pong, SocketIOEventTypes.Unknown, "/", string.Empty, 0, 0));
				break;
			case TransportEventTypes.Pong:
				this.IsWaitingPong = false;
				break;
			}
			Socket socket = null;
			if (this.Namespaces.TryGetValue(packet.Namespace, out socket))
			{
				((ISocket)socket).OnPacket(packet);
				return;
			}
			HTTPManager.Logger.Warning("SocketManager", "Namespace \"" + packet.Namespace + "\" not found!");
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x000A0BDC File Offset: 0x0009EDDC
		public void EmitAll(string eventName, params object[] args)
		{
			for (int i = 0; i < this.Sockets.Count; i++)
			{
				this.Sockets[i].Emit(eventName, args);
			}
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x000A0C14 File Offset: 0x0009EE14
		void IManager.EmitEvent(string eventName, params object[] args)
		{
			Socket socket = null;
			if (this.Namespaces.TryGetValue("/", out socket))
			{
				((ISocket)socket).EmitEvent(eventName, args);
			}
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x000A0C3F File Offset: 0x0009EE3F
		void IManager.EmitEvent(SocketIOEventTypes type, params object[] args)
		{
			((IManager)this).EmitEvent(EventNames.GetNameFor(type), args);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x000A0C4E File Offset: 0x0009EE4E
		void IManager.EmitError(SocketIOErrors errCode, string msg)
		{
			((IManager)this).EmitEvent(SocketIOEventTypes.Error, new object[]
			{
				new Error(errCode, msg)
			});
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x000A0C68 File Offset: 0x0009EE68
		void IManager.EmitAll(string eventName, params object[] args)
		{
			for (int i = 0; i < this.Sockets.Count; i++)
			{
				((ISocket)this.Sockets[i]).EmitEvent(eventName, args);
			}
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x000A0CA0 File Offset: 0x0009EEA0
		void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
		{
			switch (this.State)
			{
			case SocketManager.States.Opening:
				if (DateTime.UtcNow - this.ConnectionStarted >= this.Options.Timeout)
				{
					((IManager)this).EmitError(SocketIOErrors.Internal, "Connection timed out!");
					((IManager)this).EmitEvent("connect_error", Array.Empty<object>());
					((IManager)this).EmitEvent("connect_timeout", Array.Empty<object>());
					((IManager)this).TryToReconnect();
					return;
				}
				return;
			case SocketManager.States.Open:
				break;
			case SocketManager.States.Paused:
				if (this.Transport.IsRequestInProgress || this.Transport.IsPollingInProgress)
				{
					return;
				}
				this.State = SocketManager.States.Open;
				this.Transport.Close();
				this.Transport = this.UpgradingTransport;
				this.UpgradingTransport = null;
				this.Transport.Send(new Packet(TransportEventTypes.Upgrade, SocketIOEventTypes.Unknown, "/", string.Empty, 0, 0));
				break;
			case SocketManager.States.Reconnecting:
				if (this.ReconnectAt != DateTime.MinValue && DateTime.UtcNow >= this.ReconnectAt)
				{
					((IManager)this).EmitEvent("reconnect_attempt", Array.Empty<object>());
					((IManager)this).EmitEvent("reconnecting", Array.Empty<object>());
					this.Open();
					return;
				}
				return;
			default:
				return;
			}
			ITransport transport = null;
			if (this.Transport != null && this.Transport.State == TransportStates.Open)
			{
				transport = this.Transport;
			}
			if (transport == null || transport.State != TransportStates.Open)
			{
				return;
			}
			transport.Poll();
			this.SendOfflinePackets();
			if (this.LastHeartbeat == DateTime.MinValue)
			{
				this.LastHeartbeat = DateTime.UtcNow;
				return;
			}
			if (!this.IsWaitingPong && DateTime.UtcNow - this.LastHeartbeat > this.Handshake.PingInterval)
			{
				((IManager)this).SendPacket(new Packet(TransportEventTypes.Ping, SocketIOEventTypes.Unknown, "/", string.Empty, 0, 0));
				this.LastHeartbeat = DateTime.UtcNow;
				this.IsWaitingPong = true;
			}
			if (this.IsWaitingPong && DateTime.UtcNow - this.LastHeartbeat > this.Handshake.PingTimeout)
			{
				this.IsWaitingPong = false;
				((IManager)this).TryToReconnect();
			}
		}

		// Token: 0x0400149C RID: 5276
		public static IJsonEncoder DefaultEncoder = new DefaultJSonEncoder();

		// Token: 0x0400149D RID: 5277
		public const int MinProtocolVersion = 4;

		// Token: 0x0400149E RID: 5278
		private SocketManager.States state;

		// Token: 0x040014A6 RID: 5286
		private int nextAckId;

		// Token: 0x040014A9 RID: 5289
		private Dictionary<string, Socket> Namespaces = new Dictionary<string, Socket>();

		// Token: 0x040014AA RID: 5290
		private List<Socket> Sockets = new List<Socket>();

		// Token: 0x040014AB RID: 5291
		private List<Packet> OfflinePackets;

		// Token: 0x040014AC RID: 5292
		private DateTime LastHeartbeat = DateTime.MinValue;

		// Token: 0x040014AD RID: 5293
		private DateTime ReconnectAt;

		// Token: 0x040014AE RID: 5294
		private DateTime ConnectionStarted;

		// Token: 0x040014AF RID: 5295
		private bool closing;

		// Token: 0x040014B0 RID: 5296
		private bool IsWaitingPong;

		// Token: 0x020008EC RID: 2284
		public enum States
		{
			// Token: 0x04003486 RID: 13446
			Initial,
			// Token: 0x04003487 RID: 13447
			Closed,
			// Token: 0x04003488 RID: 13448
			Opening,
			// Token: 0x04003489 RID: 13449
			Open,
			// Token: 0x0400348A RID: 13450
			Paused,
			// Token: 0x0400348B RID: 13451
			Reconnecting
		}
	}
}
