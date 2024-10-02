using System;
using System.Collections.Generic;
using BestHTTP.Logger;

namespace BestHTTP.SocketIO.Events
{
	// Token: 0x020001D9 RID: 473
	internal sealed class EventTable
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060011A1 RID: 4513 RVA: 0x000A2658 File Offset: 0x000A0858
		// (set) Token: 0x060011A2 RID: 4514 RVA: 0x000A2660 File Offset: 0x000A0860
		private Socket Socket { get; set; }

		// Token: 0x060011A3 RID: 4515 RVA: 0x000A2669 File Offset: 0x000A0869
		public EventTable(Socket socket)
		{
			this.Socket = socket;
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x000A2684 File Offset: 0x000A0884
		public void Register(string eventName, SocketIOCallback callback, bool onlyOnce, bool autoDecodePayload)
		{
			List<EventDescriptor> list;
			if (!this.Table.TryGetValue(eventName, out list))
			{
				this.Table.Add(eventName, list = new List<EventDescriptor>(1));
			}
			EventDescriptor eventDescriptor = list.Find((EventDescriptor d) => d.OnlyOnce == onlyOnce && d.AutoDecodePayload == autoDecodePayload);
			if (eventDescriptor == null)
			{
				list.Add(new EventDescriptor(onlyOnce, autoDecodePayload, callback));
				return;
			}
			eventDescriptor.Callbacks.Add(callback);
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x000A2705 File Offset: 0x000A0905
		public void Unregister(string eventName)
		{
			this.Table.Remove(eventName);
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x000A2714 File Offset: 0x000A0914
		public void Unregister(string eventName, SocketIOCallback callback)
		{
			List<EventDescriptor> list;
			if (this.Table.TryGetValue(eventName, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					list[i].Callbacks.Remove(callback);
				}
			}
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x000A2758 File Offset: 0x000A0958
		public void Call(string eventName, Packet packet, params object[] args)
		{
			if (HTTPManager.Logger.Level <= Loglevels.All)
			{
				HTTPManager.Logger.Verbose("EventTable", "Call - " + eventName);
			}
			List<EventDescriptor> list;
			if (this.Table.TryGetValue(eventName, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					list[i].Call(this.Socket, packet, args);
				}
			}
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x000A27C4 File Offset: 0x000A09C4
		public void Call(Packet packet)
		{
			string text = packet.DecodeEventName();
			string text2 = (packet.SocketIOEvent != SocketIOEventTypes.Unknown) ? EventNames.GetNameFor(packet.SocketIOEvent) : EventNames.GetNameFor(packet.TransportEvent);
			object[] args = null;
			if (!this.HasSubsciber(text) && !this.HasSubsciber(text2))
			{
				return;
			}
			if (packet.TransportEvent == TransportEventTypes.Message && (packet.SocketIOEvent == SocketIOEventTypes.Event || packet.SocketIOEvent == SocketIOEventTypes.BinaryEvent) && this.ShouldDecodePayload(text))
			{
				args = packet.Decode(this.Socket.Manager.Encoder);
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.Call(text, packet, args);
			}
			if (!packet.IsDecoded && this.ShouldDecodePayload(text2))
			{
				args = packet.Decode(this.Socket.Manager.Encoder);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				this.Call(text2, packet, args);
			}
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x000A2894 File Offset: 0x000A0A94
		public void Clear()
		{
			this.Table.Clear();
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x000A28A4 File Offset: 0x000A0AA4
		private bool ShouldDecodePayload(string eventName)
		{
			List<EventDescriptor> list;
			if (this.Table.TryGetValue(eventName, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].AutoDecodePayload && list[i].Callbacks.Count > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x000A28F7 File Offset: 0x000A0AF7
		private bool HasSubsciber(string eventName)
		{
			return this.Table.ContainsKey(eventName);
		}

		// Token: 0x040014DF RID: 5343
		private Dictionary<string, List<EventDescriptor>> Table = new Dictionary<string, List<EventDescriptor>>();
	}
}
