using System;
using System.Collections.Generic;

namespace BestHTTP.SocketIO.Events
{
	// Token: 0x020001D7 RID: 471
	internal sealed class EventDescriptor
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06001195 RID: 4501 RVA: 0x000A23B8 File Offset: 0x000A05B8
		// (set) Token: 0x06001196 RID: 4502 RVA: 0x000A23C0 File Offset: 0x000A05C0
		public List<SocketIOCallback> Callbacks { get; private set; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06001197 RID: 4503 RVA: 0x000A23C9 File Offset: 0x000A05C9
		// (set) Token: 0x06001198 RID: 4504 RVA: 0x000A23D1 File Offset: 0x000A05D1
		public bool OnlyOnce { get; private set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06001199 RID: 4505 RVA: 0x000A23DA File Offset: 0x000A05DA
		// (set) Token: 0x0600119A RID: 4506 RVA: 0x000A23E2 File Offset: 0x000A05E2
		public bool AutoDecodePayload { get; private set; }

		// Token: 0x0600119B RID: 4507 RVA: 0x000A23EB File Offset: 0x000A05EB
		public EventDescriptor(bool onlyOnce, bool autoDecodePayload, SocketIOCallback callback)
		{
			this.OnlyOnce = onlyOnce;
			this.AutoDecodePayload = autoDecodePayload;
			this.Callbacks = new List<SocketIOCallback>(1);
			if (callback != null)
			{
				this.Callbacks.Add(callback);
			}
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x000A241C File Offset: 0x000A061C
		public void Call(Socket socket, Packet packet, params object[] args)
		{
			int count = this.Callbacks.Count;
			if (this.CallbackArray == null || this.CallbackArray.Length < count)
			{
				Array.Resize<SocketIOCallback>(ref this.CallbackArray, count);
			}
			this.Callbacks.CopyTo(this.CallbackArray);
			for (int i = 0; i < count; i++)
			{
				try
				{
					SocketIOCallback socketIOCallback = this.CallbackArray[i];
					if (socketIOCallback != null)
					{
						socketIOCallback(socket, packet, args);
					}
				}
				catch (Exception ex)
				{
					if (args == null || args.Length == 0 || !(args[0] is Error))
					{
						((ISocket)socket).EmitError(SocketIOErrors.User, ex.Message + " " + ex.StackTrace);
					}
					HTTPManager.Logger.Exception("EventDescriptor", "Call", ex);
				}
				if (this.OnlyOnce)
				{
					this.Callbacks.Remove(this.CallbackArray[i]);
				}
				this.CallbackArray[i] = null;
			}
		}

		// Token: 0x040014D3 RID: 5331
		private SocketIOCallback[] CallbackArray;
	}
}
