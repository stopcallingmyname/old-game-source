using System;
using System.Collections.Generic;
using BestHTTP.Extensions;

namespace BestHTTP.ServerSentEvents
{
	// Token: 0x02000235 RID: 565
	public class EventSource : IHeartbeat
	{
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x0600143B RID: 5179 RVA: 0x000A97CB File Offset: 0x000A79CB
		// (set) Token: 0x0600143C RID: 5180 RVA: 0x000A97D3 File Offset: 0x000A79D3
		public Uri Uri { get; private set; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600143D RID: 5181 RVA: 0x000A97DC File Offset: 0x000A79DC
		// (set) Token: 0x0600143E RID: 5182 RVA: 0x000A97E4 File Offset: 0x000A79E4
		public States State
		{
			get
			{
				return this._state;
			}
			private set
			{
				States state = this._state;
				this._state = value;
				if (this.OnStateChanged != null)
				{
					try
					{
						this.OnStateChanged(this, state, this._state);
					}
					catch (Exception ex)
					{
						HTTPManager.Logger.Exception("EventSource", "OnStateChanged", ex);
					}
				}
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x0600143F RID: 5183 RVA: 0x000A9844 File Offset: 0x000A7A44
		// (set) Token: 0x06001440 RID: 5184 RVA: 0x000A984C File Offset: 0x000A7A4C
		public TimeSpan ReconnectionTime { get; set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06001441 RID: 5185 RVA: 0x000A9855 File Offset: 0x000A7A55
		// (set) Token: 0x06001442 RID: 5186 RVA: 0x000A985D File Offset: 0x000A7A5D
		public string LastEventId { get; private set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06001443 RID: 5187 RVA: 0x000A9866 File Offset: 0x000A7A66
		// (set) Token: 0x06001444 RID: 5188 RVA: 0x000A986E File Offset: 0x000A7A6E
		public HTTPRequest InternalRequest { get; private set; }

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06001445 RID: 5189 RVA: 0x000A9878 File Offset: 0x000A7A78
		// (remove) Token: 0x06001446 RID: 5190 RVA: 0x000A98B0 File Offset: 0x000A7AB0
		public event OnGeneralEventDelegate OnOpen;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06001447 RID: 5191 RVA: 0x000A98E8 File Offset: 0x000A7AE8
		// (remove) Token: 0x06001448 RID: 5192 RVA: 0x000A9920 File Offset: 0x000A7B20
		public event OnMessageDelegate OnMessage;

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06001449 RID: 5193 RVA: 0x000A9958 File Offset: 0x000A7B58
		// (remove) Token: 0x0600144A RID: 5194 RVA: 0x000A9990 File Offset: 0x000A7B90
		public event OnErrorDelegate OnError;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x0600144B RID: 5195 RVA: 0x000A99C8 File Offset: 0x000A7BC8
		// (remove) Token: 0x0600144C RID: 5196 RVA: 0x000A9A00 File Offset: 0x000A7C00
		public event OnRetryDelegate OnRetry;

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x0600144D RID: 5197 RVA: 0x000A9A38 File Offset: 0x000A7C38
		// (remove) Token: 0x0600144E RID: 5198 RVA: 0x000A9A70 File Offset: 0x000A7C70
		public event OnGeneralEventDelegate OnClosed;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x0600144F RID: 5199 RVA: 0x000A9AA8 File Offset: 0x000A7CA8
		// (remove) Token: 0x06001450 RID: 5200 RVA: 0x000A9AE0 File Offset: 0x000A7CE0
		public event OnStateChangedDelegate OnStateChanged;

		// Token: 0x06001451 RID: 5201 RVA: 0x000A9B18 File Offset: 0x000A7D18
		public EventSource(Uri uri)
		{
			this.Uri = uri;
			this.ReconnectionTime = TimeSpan.FromMilliseconds(2000.0);
			this.InternalRequest = new HTTPRequest(this.Uri, HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnRequestFinished));
			this.InternalRequest.SetHeader("Accept", "text/event-stream");
			this.InternalRequest.SetHeader("Cache-Control", "no-cache");
			this.InternalRequest.SetHeader("Accept-Encoding", "identity");
			this.InternalRequest.ProtocolHandler = SupportedProtocols.ServerSentEvents;
			this.InternalRequest.OnUpgraded = new OnRequestFinishedDelegate(this.OnUpgraded);
			this.InternalRequest.DisableRetry = true;
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x000A9BD4 File Offset: 0x000A7DD4
		public void Open()
		{
			if (this.State != States.Initial && this.State != States.Retrying && this.State != States.Closed)
			{
				return;
			}
			this.State = States.Connecting;
			if (!string.IsNullOrEmpty(this.LastEventId))
			{
				this.InternalRequest.SetHeader("Last-Event-ID", this.LastEventId);
			}
			this.InternalRequest.Send();
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x000A9C32 File Offset: 0x000A7E32
		public void Close()
		{
			if (this.State == States.Closing || this.State == States.Closed)
			{
				return;
			}
			this.State = States.Closing;
			if (this.InternalRequest != null)
			{
				this.InternalRequest.Abort();
				return;
			}
			this.State = States.Closed;
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x000A9C69 File Offset: 0x000A7E69
		public void On(string eventName, OnEventDelegate action)
		{
			if (this.EventTable == null)
			{
				this.EventTable = new Dictionary<string, OnEventDelegate>();
			}
			this.EventTable[eventName] = action;
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x000A9C8B File Offset: 0x000A7E8B
		public void Off(string eventName)
		{
			if (eventName == null || this.EventTable == null)
			{
				return;
			}
			this.EventTable.Remove(eventName);
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x000A9CA8 File Offset: 0x000A7EA8
		private void CallOnError(string error, string msg)
		{
			if (this.OnError != null)
			{
				try
				{
					this.OnError(this, error);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("EventSource", msg + " - OnError", ex);
				}
			}
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x000A9CFC File Offset: 0x000A7EFC
		private bool CallOnRetry()
		{
			if (this.OnRetry != null)
			{
				try
				{
					return this.OnRetry(this);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("EventSource", "CallOnRetry", ex);
				}
				return true;
			}
			return true;
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x000A9D4C File Offset: 0x000A7F4C
		private void SetClosed(string msg)
		{
			this.State = States.Closed;
			if (this.OnClosed != null)
			{
				try
				{
					this.OnClosed(this);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("EventSource", msg + " - OnClosed", ex);
				}
			}
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x000A9DA4 File Offset: 0x000A7FA4
		private void Retry()
		{
			if (this.RetryCount > 0 || !this.CallOnRetry())
			{
				this.SetClosed("Retry");
				return;
			}
			this.RetryCount += 1;
			this.RetryCalled = DateTime.UtcNow;
			HTTPManager.Heartbeats.Subscribe(this);
			this.State = States.Retrying;
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x000A9DFC File Offset: 0x000A7FFC
		private void OnUpgraded(HTTPRequest originalRequest, HTTPResponse response)
		{
			EventSourceResponse eventSourceResponse = response as EventSourceResponse;
			if (eventSourceResponse == null)
			{
				this.CallOnError("Not an EventSourceResponse!", "OnUpgraded");
				return;
			}
			if (this.OnOpen != null)
			{
				try
				{
					this.OnOpen(this);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("EventSource", "OnOpen", ex);
				}
			}
			EventSourceResponse eventSourceResponse2 = eventSourceResponse;
			eventSourceResponse2.OnMessage = (Action<EventSourceResponse, Message>)Delegate.Combine(eventSourceResponse2.OnMessage, new Action<EventSourceResponse, Message>(this.OnMessageReceived));
			eventSourceResponse.StartReceive();
			this.RetryCount = 0;
			this.State = States.Open;
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x000A9E98 File Offset: 0x000A8098
		private void OnRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			if (this.State == States.Closed)
			{
				return;
			}
			if (this.State == States.Closing || req.State == HTTPRequestStates.Aborted)
			{
				this.SetClosed("OnRequestFinished");
				return;
			}
			string text = string.Empty;
			bool flag = true;
			switch (req.State)
			{
			case HTTPRequestStates.Processing:
				flag = !resp.HasHeader("content-length");
				break;
			case HTTPRequestStates.Finished:
				if (resp.StatusCode == 200 && !resp.HasHeaderWithValue("content-type", "text/event-stream"))
				{
					text = "No Content-Type header with value 'text/event-stream' present.";
					flag = false;
				}
				if (flag && resp.StatusCode != 500 && resp.StatusCode != 502 && resp.StatusCode != 503 && resp.StatusCode != 504)
				{
					flag = false;
					text = string.Format("Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText);
				}
				break;
			case HTTPRequestStates.Error:
				text = "Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
				break;
			case HTTPRequestStates.Aborted:
				text = "OnRequestFinished - Aborted without request. EventSource's State: " + this.State;
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = "Connection Timed Out!";
				break;
			case HTTPRequestStates.TimedOut:
				text = "Processing the request Timed Out!";
				break;
			}
			if (this.State >= States.Closing)
			{
				this.SetClosed("OnRequestFinished");
				return;
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.CallOnError(text, "OnRequestFinished");
			}
			if (flag)
			{
				this.Retry();
				return;
			}
			this.SetClosed("OnRequestFinished");
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x000AA048 File Offset: 0x000A8248
		private void OnMessageReceived(EventSourceResponse resp, Message message)
		{
			if (this.State >= States.Closing)
			{
				return;
			}
			if (message.Id != null)
			{
				this.LastEventId = message.Id;
			}
			if (message.Retry.TotalMilliseconds > 0.0)
			{
				this.ReconnectionTime = message.Retry;
			}
			if (string.IsNullOrEmpty(message.Data))
			{
				return;
			}
			if (this.OnMessage != null)
			{
				try
				{
					this.OnMessage(this, message);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("EventSource", "OnMessageReceived - OnMessage", ex);
				}
			}
			OnEventDelegate onEventDelegate;
			if (this.EventTable != null && !string.IsNullOrEmpty(message.Event) && this.EventTable.TryGetValue(message.Event, out onEventDelegate) && onEventDelegate != null)
			{
				try
				{
					onEventDelegate(this, message);
				}
				catch (Exception ex2)
				{
					HTTPManager.Logger.Exception("EventSource", "OnMessageReceived - action", ex2);
				}
			}
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x000AA140 File Offset: 0x000A8340
		void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
		{
			if (this.State != States.Retrying)
			{
				HTTPManager.Heartbeats.Unsubscribe(this);
				return;
			}
			if (DateTime.UtcNow - this.RetryCalled >= this.ReconnectionTime)
			{
				this.Open();
				if (this.State != States.Connecting)
				{
					this.SetClosed("OnHeartbeatUpdate");
				}
				HTTPManager.Heartbeats.Unsubscribe(this);
			}
		}

		// Token: 0x040015FD RID: 5629
		private States _state;

		// Token: 0x04001607 RID: 5639
		private Dictionary<string, OnEventDelegate> EventTable;

		// Token: 0x04001608 RID: 5640
		private byte RetryCount;

		// Token: 0x04001609 RID: 5641
		private DateTime RetryCalled;
	}
}
