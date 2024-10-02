using System;
using System.Collections.Generic;
using BestHTTP.JSON;
using BestHTTP.SocketIO.Events;

namespace BestHTTP.SocketIO
{
	// Token: 0x020001CA RID: 458
	public sealed class Socket : ISocket
	{
		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060010EC RID: 4332 RVA: 0x0009FC72 File Offset: 0x0009DE72
		// (set) Token: 0x060010ED RID: 4333 RVA: 0x0009FC7A File Offset: 0x0009DE7A
		public SocketManager Manager { get; private set; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x0009FC83 File Offset: 0x0009DE83
		// (set) Token: 0x060010EF RID: 4335 RVA: 0x0009FC8B File Offset: 0x0009DE8B
		public string Namespace { get; private set; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x0009FC94 File Offset: 0x0009DE94
		// (set) Token: 0x060010F1 RID: 4337 RVA: 0x0009FC9C File Offset: 0x0009DE9C
		public string Id { get; private set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x0009FCA5 File Offset: 0x0009DEA5
		// (set) Token: 0x060010F3 RID: 4339 RVA: 0x0009FCAD File Offset: 0x0009DEAD
		public bool IsOpen { get; private set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060010F4 RID: 4340 RVA: 0x0009FCB6 File Offset: 0x0009DEB6
		// (set) Token: 0x060010F5 RID: 4341 RVA: 0x0009FCBE File Offset: 0x0009DEBE
		public bool AutoDecodePayload { get; set; }

		// Token: 0x060010F6 RID: 4342 RVA: 0x0009FCC7 File Offset: 0x0009DEC7
		internal Socket(string nsp, SocketManager manager)
		{
			this.Namespace = nsp;
			this.Manager = manager;
			this.IsOpen = false;
			this.AutoDecodePayload = true;
			this.EventCallbacks = new EventTable(this);
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0009FD04 File Offset: 0x0009DF04
		void ISocket.Open()
		{
			if (this.Manager.State == SocketManager.States.Open)
			{
				this.OnTransportOpen(this.Manager.Socket, null, Array.Empty<object>());
				return;
			}
			this.Manager.Socket.Off("connect", new SocketIOCallback(this.OnTransportOpen));
			this.Manager.Socket.On("connect", new SocketIOCallback(this.OnTransportOpen));
			if (this.Manager.Options.AutoConnect && this.Manager.State == SocketManager.States.Initial)
			{
				this.Manager.Open();
			}
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0009FDA3 File Offset: 0x0009DFA3
		public void Disconnect()
		{
			((ISocket)this).Disconnect(true);
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0009FDAC File Offset: 0x0009DFAC
		void ISocket.Disconnect(bool remove)
		{
			if (this.IsOpen)
			{
				Packet packet = new Packet(TransportEventTypes.Message, SocketIOEventTypes.Disconnect, this.Namespace, string.Empty, 0, 0);
				((IManager)this.Manager).SendPacket(packet);
				this.IsOpen = false;
				((ISocket)this).OnPacket(packet);
			}
			if (this.AckCallbacks != null)
			{
				this.AckCallbacks.Clear();
			}
			if (remove)
			{
				this.EventCallbacks.Clear();
				((IManager)this.Manager).Remove(this);
			}
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x0009FE1D File Offset: 0x0009E01D
		public Socket Emit(string eventName, params object[] args)
		{
			return this.Emit(eventName, null, args);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0009FE28 File Offset: 0x0009E028
		public Socket Emit(string eventName, SocketIOAckCallback callback, params object[] args)
		{
			if (EventNames.IsBlacklisted(eventName))
			{
				throw new ArgumentException("Blacklisted event: " + eventName);
			}
			this.arguments.Clear();
			this.arguments.Add(eventName);
			List<byte[]> list = null;
			if (args != null && args.Length != 0)
			{
				int num = 0;
				for (int i = 0; i < args.Length; i++)
				{
					byte[] array = args[i] as byte[];
					if (array != null)
					{
						if (list == null)
						{
							list = new List<byte[]>();
						}
						Dictionary<string, object> dictionary = new Dictionary<string, object>(2);
						dictionary.Add("_placeholder", true);
						dictionary.Add("num", num++);
						this.arguments.Add(dictionary);
						list.Add(array);
					}
					else
					{
						this.arguments.Add(args[i]);
					}
				}
			}
			string text = null;
			try
			{
				text = this.Manager.Encoder.Encode(this.arguments);
			}
			catch (Exception ex)
			{
				((ISocket)this).EmitError(SocketIOErrors.Internal, "Error while encoding payload: " + ex.Message + " " + ex.StackTrace);
				return this;
			}
			this.arguments.Clear();
			if (text == null)
			{
				throw new ArgumentException("Encoding the arguments to JSON failed!");
			}
			int num2 = 0;
			if (callback != null)
			{
				num2 = this.Manager.NextAckId;
				if (this.AckCallbacks == null)
				{
					this.AckCallbacks = new Dictionary<int, SocketIOAckCallback>();
				}
				this.AckCallbacks[num2] = callback;
			}
			Packet packet = new Packet(TransportEventTypes.Message, (list == null) ? SocketIOEventTypes.Event : SocketIOEventTypes.BinaryEvent, this.Namespace, text, 0, num2);
			if (list != null)
			{
				packet.Attachments = list;
			}
			((IManager)this.Manager).SendPacket(packet);
			return this;
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0009FFCC File Offset: 0x0009E1CC
		public Socket EmitAck(Packet originalPacket, params object[] args)
		{
			if (originalPacket == null)
			{
				throw new ArgumentNullException("originalPacket == null!");
			}
			if (originalPacket.SocketIOEvent != SocketIOEventTypes.Event && originalPacket.SocketIOEvent != SocketIOEventTypes.BinaryEvent)
			{
				throw new ArgumentException("Wrong packet - you can't send an Ack for a packet with id == 0 and SocketIOEvent != Event or SocketIOEvent != BinaryEvent!");
			}
			this.arguments.Clear();
			if (args != null && args.Length != 0)
			{
				this.arguments.AddRange(args);
			}
			string text = null;
			try
			{
				text = this.Manager.Encoder.Encode(this.arguments);
			}
			catch (Exception ex)
			{
				((ISocket)this).EmitError(SocketIOErrors.Internal, "Error while encoding payload: " + ex.Message + " " + ex.StackTrace);
				return this;
			}
			if (text == null)
			{
				throw new ArgumentException("Encoding the arguments to JSON failed!");
			}
			Packet packet = new Packet(TransportEventTypes.Message, (originalPacket.SocketIOEvent == SocketIOEventTypes.Event) ? SocketIOEventTypes.Ack : SocketIOEventTypes.BinaryAck, this.Namespace, text, 0, originalPacket.Id);
			((IManager)this.Manager).SendPacket(packet);
			return this;
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x000A00B4 File Offset: 0x0009E2B4
		public void On(string eventName, SocketIOCallback callback)
		{
			this.EventCallbacks.Register(eventName, callback, false, this.AutoDecodePayload);
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x000A00CC File Offset: 0x0009E2CC
		public void On(SocketIOEventTypes type, SocketIOCallback callback)
		{
			string nameFor = EventNames.GetNameFor(type);
			this.EventCallbacks.Register(nameFor, callback, false, this.AutoDecodePayload);
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x000A00F4 File Offset: 0x0009E2F4
		public void On(string eventName, SocketIOCallback callback, bool autoDecodePayload)
		{
			this.EventCallbacks.Register(eventName, callback, false, autoDecodePayload);
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x000A0108 File Offset: 0x0009E308
		public void On(SocketIOEventTypes type, SocketIOCallback callback, bool autoDecodePayload)
		{
			string nameFor = EventNames.GetNameFor(type);
			this.EventCallbacks.Register(nameFor, callback, false, autoDecodePayload);
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x000A012B File Offset: 0x0009E32B
		public void Once(string eventName, SocketIOCallback callback)
		{
			this.EventCallbacks.Register(eventName, callback, true, this.AutoDecodePayload);
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x000A0141 File Offset: 0x0009E341
		public void Once(SocketIOEventTypes type, SocketIOCallback callback)
		{
			this.EventCallbacks.Register(EventNames.GetNameFor(type), callback, true, this.AutoDecodePayload);
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x000A015C File Offset: 0x0009E35C
		public void Once(string eventName, SocketIOCallback callback, bool autoDecodePayload)
		{
			this.EventCallbacks.Register(eventName, callback, true, autoDecodePayload);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x000A016D File Offset: 0x0009E36D
		public void Once(SocketIOEventTypes type, SocketIOCallback callback, bool autoDecodePayload)
		{
			this.EventCallbacks.Register(EventNames.GetNameFor(type), callback, true, autoDecodePayload);
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x000A0183 File Offset: 0x0009E383
		public void Off()
		{
			this.EventCallbacks.Clear();
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x000A0190 File Offset: 0x0009E390
		public void Off(string eventName)
		{
			this.EventCallbacks.Unregister(eventName);
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x000A019E File Offset: 0x0009E39E
		public void Off(SocketIOEventTypes type)
		{
			this.Off(EventNames.GetNameFor(type));
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x000A01AC File Offset: 0x0009E3AC
		public void Off(string eventName, SocketIOCallback callback)
		{
			this.EventCallbacks.Unregister(eventName, callback);
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x000A01BB File Offset: 0x0009E3BB
		public void Off(SocketIOEventTypes type, SocketIOCallback callback)
		{
			this.EventCallbacks.Unregister(EventNames.GetNameFor(type), callback);
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x000A01D0 File Offset: 0x0009E3D0
		void ISocket.OnPacket(Packet packet)
		{
			switch (packet.SocketIOEvent)
			{
			case SocketIOEventTypes.Connect:
				this.Id = ((this.Namespace != "/") ? (this.Namespace + "#" + this.Manager.Handshake.Sid) : this.Manager.Handshake.Sid);
				break;
			case SocketIOEventTypes.Disconnect:
				if (this.IsOpen)
				{
					this.IsOpen = false;
					this.EventCallbacks.Call(EventNames.GetNameFor(SocketIOEventTypes.Disconnect), packet, Array.Empty<object>());
					this.Disconnect();
				}
				break;
			case SocketIOEventTypes.Error:
			{
				bool flag = false;
				object obj = Json.Decode(packet.Payload, ref flag);
				if (flag)
				{
					Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
					Error error;
					if (dictionary != null && dictionary.ContainsKey("code"))
					{
						error = new Error((SocketIOErrors)Convert.ToInt32(dictionary["code"]), dictionary["message"] as string);
					}
					else
					{
						error = new Error(SocketIOErrors.Custom, packet.Payload);
					}
					this.EventCallbacks.Call(EventNames.GetNameFor(SocketIOEventTypes.Error), packet, new object[]
					{
						error
					});
					return;
				}
				break;
			}
			}
			this.EventCallbacks.Call(packet);
			if ((packet.SocketIOEvent == SocketIOEventTypes.Ack || packet.SocketIOEvent == SocketIOEventTypes.BinaryAck) && this.AckCallbacks != null)
			{
				SocketIOAckCallback socketIOAckCallback = null;
				if (this.AckCallbacks.TryGetValue(packet.Id, out socketIOAckCallback) && socketIOAckCallback != null)
				{
					try
					{
						socketIOAckCallback(this, packet, this.AutoDecodePayload ? packet.Decode(this.Manager.Encoder) : null);
					}
					catch (Exception ex)
					{
						HTTPManager.Logger.Exception("Socket", "ackCallback", ex);
					}
				}
				this.AckCallbacks.Remove(packet.Id);
			}
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x000A03A8 File Offset: 0x0009E5A8
		void ISocket.EmitEvent(SocketIOEventTypes type, params object[] args)
		{
			((ISocket)this).EmitEvent(EventNames.GetNameFor(type), args);
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x000A03B7 File Offset: 0x0009E5B7
		void ISocket.EmitEvent(string eventName, params object[] args)
		{
			if (!string.IsNullOrEmpty(eventName))
			{
				this.EventCallbacks.Call(eventName, null, args);
			}
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x000A03CF File Offset: 0x0009E5CF
		void ISocket.EmitError(SocketIOErrors errCode, string msg)
		{
			((ISocket)this).EmitEvent(SocketIOEventTypes.Error, new object[]
			{
				new Error(errCode, msg)
			});
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x000A03E8 File Offset: 0x0009E5E8
		private void OnTransportOpen(Socket socket, Packet packet, params object[] args)
		{
			if (this.Namespace != "/")
			{
				((IManager)this.Manager).SendPacket(new Packet(TransportEventTypes.Message, SocketIOEventTypes.Connect, this.Namespace, string.Empty, 0, 0));
			}
			this.IsOpen = true;
		}

		// Token: 0x04001499 RID: 5273
		private Dictionary<int, SocketIOAckCallback> AckCallbacks;

		// Token: 0x0400149A RID: 5274
		private EventTable EventCallbacks;

		// Token: 0x0400149B RID: 5275
		private List<object> arguments = new List<object>();
	}
}
