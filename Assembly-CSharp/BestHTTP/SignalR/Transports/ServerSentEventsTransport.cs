using System;
using BestHTTP.ServerSentEvents;
using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Transports
{
	// Token: 0x02000211 RID: 529
	public sealed class ServerSentEventsTransport : PostSendTransportBase
	{
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06001342 RID: 4930 RVA: 0x0006AE98 File Offset: 0x00069098
		public override bool SupportsKeepAlive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06001343 RID: 4931 RVA: 0x0006AE98 File Offset: 0x00069098
		public override TransportTypes Type
		{
			get
			{
				return TransportTypes.ServerSentEvents;
			}
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x000A7A13 File Offset: 0x000A5C13
		public ServerSentEventsTransport(Connection con) : base("serverSentEvents", con)
		{
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x000A7A24 File Offset: 0x000A5C24
		public override void Connect()
		{
			if (this.EventSource != null)
			{
				HTTPManager.Logger.Warning("ServerSentEventsTransport", "Start - EventSource already created!");
				return;
			}
			if (base.State != TransportStates.Reconnecting)
			{
				base.State = TransportStates.Connecting;
			}
			RequestTypes type = (base.State == TransportStates.Reconnecting) ? RequestTypes.Reconnect : RequestTypes.Connect;
			Uri uri = base.Connection.BuildUri(type, this);
			this.EventSource = new EventSource(uri);
			this.EventSource.OnOpen += this.OnEventSourceOpen;
			this.EventSource.OnMessage += this.OnEventSourceMessage;
			this.EventSource.OnError += this.OnEventSourceError;
			this.EventSource.OnClosed += this.OnEventSourceClosed;
			this.EventSource.OnRetry += ((EventSource es) => false);
			this.EventSource.Open();
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x000A7B18 File Offset: 0x000A5D18
		public override void Stop()
		{
			this.EventSource.OnOpen -= this.OnEventSourceOpen;
			this.EventSource.OnMessage -= this.OnEventSourceMessage;
			this.EventSource.OnError -= this.OnEventSourceError;
			this.EventSource.OnClosed -= this.OnEventSourceClosed;
			this.EventSource.Close();
			this.EventSource = null;
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0000248C File Offset: 0x0000068C
		protected override void Started()
		{
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x000A7B93 File Offset: 0x000A5D93
		public override void Abort()
		{
			base.Abort();
			this.EventSource.Close();
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x000A7BA6 File Offset: 0x000A5DA6
		protected override void Aborted()
		{
			if (base.State == TransportStates.Closing)
			{
				base.State = TransportStates.Closed;
			}
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x000A7BB8 File Offset: 0x000A5DB8
		private void OnEventSourceOpen(EventSource eventSource)
		{
			HTTPManager.Logger.Information("Transport - " + base.Name, "OnEventSourceOpen");
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x000A7BDC File Offset: 0x000A5DDC
		private void OnEventSourceMessage(EventSource eventSource, Message message)
		{
			if (message.Data.Equals("initialized"))
			{
				base.OnConnected();
				return;
			}
			IServerMessage serverMessage = TransportBase.Parse(base.Connection.JsonEncoder, message.Data);
			if (serverMessage != null)
			{
				base.Connection.OnMessage(serverMessage);
			}
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x000A7C28 File Offset: 0x000A5E28
		private void OnEventSourceError(EventSource eventSource, string error)
		{
			HTTPManager.Logger.Information("Transport - " + base.Name, "OnEventSourceError");
			if (base.State == TransportStates.Reconnecting)
			{
				this.Connect();
				return;
			}
			if (base.State == TransportStates.Closed)
			{
				return;
			}
			if (base.State == TransportStates.Closing)
			{
				base.State = TransportStates.Closed;
				return;
			}
			base.Connection.Error(error);
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x000A7C8B File Offset: 0x000A5E8B
		private void OnEventSourceClosed(EventSource eventSource)
		{
			HTTPManager.Logger.Information("Transport - " + base.Name, "OnEventSourceClosed");
			this.OnEventSourceError(eventSource, "EventSource Closed!");
		}

		// Token: 0x040015BD RID: 5565
		private EventSource EventSource;
	}
}
