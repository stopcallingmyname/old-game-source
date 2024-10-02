using System;
using BestHTTP.Extensions;
using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Transports
{
	// Token: 0x0200020F RID: 527
	public sealed class PollingTransport : PostSendTransportBase, IHeartbeat
	{
		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06001334 RID: 4916 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool SupportsKeepAlive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06001335 RID: 4917 RVA: 0x000A7398 File Offset: 0x000A5598
		public override TransportTypes Type
		{
			get
			{
				return TransportTypes.LongPoll;
			}
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x000A739B File Offset: 0x000A559B
		public PollingTransport(Connection connection) : base("longPolling", connection)
		{
			this.LastPoll = DateTime.MinValue;
			this.PollTimeout = connection.NegotiationResult.ConnectionTimeout + TimeSpan.FromSeconds(10.0);
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x000A73D8 File Offset: 0x000A55D8
		public override void Connect()
		{
			HTTPManager.Logger.Information("Transport - " + base.Name, "Sending Open Request");
			if (base.State != TransportStates.Reconnecting)
			{
				base.State = TransportStates.Connecting;
			}
			RequestTypes type = (base.State == TransportStates.Reconnecting) ? RequestTypes.Reconnect : RequestTypes.Connect;
			HTTPRequest httprequest = new HTTPRequest(base.Connection.BuildUri(type, this), HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnConnectRequestFinished));
			base.Connection.PrepareRequest(httprequest, type);
			httprequest.Send();
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x000A7459 File Offset: 0x000A5659
		public override void Stop()
		{
			HTTPManager.Heartbeats.Unsubscribe(this);
			if (this.pollRequest != null)
			{
				this.pollRequest.Abort();
				this.pollRequest = null;
			}
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x000A7480 File Offset: 0x000A5680
		protected override void Started()
		{
			this.LastPoll = DateTime.UtcNow;
			HTTPManager.Heartbeats.Subscribe(this);
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x000A7498 File Offset: 0x000A5698
		protected override void Aborted()
		{
			HTTPManager.Heartbeats.Unsubscribe(this);
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x000A74A8 File Offset: 0x000A56A8
		private void OnConnectRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			string text = string.Empty;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (resp.IsSuccess)
				{
					HTTPManager.Logger.Information("Transport - " + base.Name, "Connect - Request Finished Successfully! " + resp.DataAsText);
					base.OnConnected();
					IServerMessage serverMessage = TransportBase.Parse(base.Connection.JsonEncoder, resp.DataAsText);
					if (serverMessage != null)
					{
						base.Connection.OnMessage(serverMessage);
						MultiMessage multiMessage = serverMessage as MultiMessage;
						if (multiMessage != null && multiMessage.PollDelay != null)
						{
							this.PollDelay = multiMessage.PollDelay.Value;
						}
					}
				}
				else
				{
					text = string.Format("Connect - Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText);
				}
				break;
			case HTTPRequestStates.Error:
				text = "Connect - Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
				break;
			case HTTPRequestStates.Aborted:
				text = "Connect - Request Aborted!";
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = "Connect - Connection Timed Out!";
				break;
			case HTTPRequestStates.TimedOut:
				text = "Connect - Processing the request Timed Out!";
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				base.Connection.Error(text);
			}
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x000A760C File Offset: 0x000A580C
		private void OnPollRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			if (req.State == HTTPRequestStates.Aborted)
			{
				HTTPManager.Logger.Warning("Transport - " + base.Name, "Poll - Request Aborted!");
				return;
			}
			this.pollRequest = null;
			string text = string.Empty;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (resp.IsSuccess)
				{
					HTTPManager.Logger.Information("Transport - " + base.Name, "Poll - Request Finished Successfully! " + resp.DataAsText);
					IServerMessage serverMessage = TransportBase.Parse(base.Connection.JsonEncoder, resp.DataAsText);
					if (serverMessage != null)
					{
						base.Connection.OnMessage(serverMessage);
						MultiMessage multiMessage = serverMessage as MultiMessage;
						if (multiMessage != null && multiMessage.PollDelay != null)
						{
							this.PollDelay = multiMessage.PollDelay.Value;
						}
						this.LastPoll = DateTime.UtcNow;
					}
				}
				else
				{
					text = string.Format("Poll - Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText);
				}
				break;
			case HTTPRequestStates.Error:
				text = "Poll - Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = "Poll - Connection Timed Out!";
				break;
			case HTTPRequestStates.TimedOut:
				text = "Poll - Processing the request Timed Out!";
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				base.Connection.Error(text);
			}
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x000A7794 File Offset: 0x000A5994
		private void Poll()
		{
			this.pollRequest = new HTTPRequest(base.Connection.BuildUri(RequestTypes.Poll, this), HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnPollRequestFinished));
			base.Connection.PrepareRequest(this.pollRequest, RequestTypes.Poll);
			this.pollRequest.Timeout = this.PollTimeout;
			this.pollRequest.Send();
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x000A77F8 File Offset: 0x000A59F8
		void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
		{
			TransportStates state = base.State;
			if (state == TransportStates.Started && this.pollRequest == null && DateTime.UtcNow >= this.LastPoll + this.PollDelay + base.Connection.NegotiationResult.LongPollDelay)
			{
				this.Poll();
			}
		}

		// Token: 0x040015B8 RID: 5560
		private DateTime LastPoll;

		// Token: 0x040015B9 RID: 5561
		private TimeSpan PollDelay;

		// Token: 0x040015BA RID: 5562
		private TimeSpan PollTimeout;

		// Token: 0x040015BB RID: 5563
		private HTTPRequest pollRequest;
	}
}
