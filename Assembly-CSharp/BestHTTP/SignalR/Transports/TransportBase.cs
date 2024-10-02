using System;
using System.Collections.Generic;
using BestHTTP.SignalR.JsonEncoders;
using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Transports
{
	// Token: 0x02000213 RID: 531
	public abstract class TransportBase
	{
		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06001352 RID: 4946 RVA: 0x000A7CB8 File Offset: 0x000A5EB8
		// (set) Token: 0x06001353 RID: 4947 RVA: 0x000A7CC0 File Offset: 0x000A5EC0
		public string Name { get; protected set; }

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06001354 RID: 4948
		public abstract bool SupportsKeepAlive { get; }

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06001355 RID: 4949
		public abstract TransportTypes Type { get; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06001356 RID: 4950 RVA: 0x000A7CC9 File Offset: 0x000A5EC9
		// (set) Token: 0x06001357 RID: 4951 RVA: 0x000A7CD1 File Offset: 0x000A5ED1
		public IConnection Connection { get; protected set; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06001358 RID: 4952 RVA: 0x000A7CDA File Offset: 0x000A5EDA
		// (set) Token: 0x06001359 RID: 4953 RVA: 0x000A7CE4 File Offset: 0x000A5EE4
		public TransportStates State
		{
			get
			{
				return this._state;
			}
			protected set
			{
				TransportStates state = this._state;
				this._state = value;
				if (this.OnStateChanged != null)
				{
					this.OnStateChanged(this, state, this._state);
				}
			}
		}

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x0600135A RID: 4954 RVA: 0x000A7D1C File Offset: 0x000A5F1C
		// (remove) Token: 0x0600135B RID: 4955 RVA: 0x000A7D54 File Offset: 0x000A5F54
		public event OnTransportStateChangedDelegate OnStateChanged;

		// Token: 0x0600135C RID: 4956 RVA: 0x000A7D89 File Offset: 0x000A5F89
		public TransportBase(string name, Connection connection)
		{
			this.Name = name;
			this.Connection = connection;
			this.State = TransportStates.Initial;
		}

		// Token: 0x0600135D RID: 4957
		public abstract void Connect();

		// Token: 0x0600135E RID: 4958
		public abstract void Stop();

		// Token: 0x0600135F RID: 4959
		protected abstract void SendImpl(string json);

		// Token: 0x06001360 RID: 4960
		protected abstract void Started();

		// Token: 0x06001361 RID: 4961
		protected abstract void Aborted();

		// Token: 0x06001362 RID: 4962 RVA: 0x000A7DA6 File Offset: 0x000A5FA6
		protected void OnConnected()
		{
			if (this.State != TransportStates.Reconnecting)
			{
				this.Start();
				return;
			}
			this.Connection.TransportReconnected();
			this.Started();
			this.State = TransportStates.Started;
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x000A7DD0 File Offset: 0x000A5FD0
		protected void Start()
		{
			HTTPManager.Logger.Information("Transport - " + this.Name, "Sending Start Request");
			this.State = TransportStates.Starting;
			if (this.Connection.Protocol > ProtocolVersions.Protocol_2_0)
			{
				HTTPRequest httprequest = new HTTPRequest(this.Connection.BuildUri(RequestTypes.Start, this), HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnStartRequestFinished));
				httprequest.Tag = 0;
				httprequest.DisableRetry = true;
				httprequest.Timeout = this.Connection.NegotiationResult.ConnectionTimeout + TimeSpan.FromSeconds(10.0);
				this.Connection.PrepareRequest(httprequest, RequestTypes.Start);
				httprequest.Send();
				return;
			}
			this.State = TransportStates.Started;
			this.Started();
			this.Connection.TransportStarted();
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x000A7EA0 File Offset: 0x000A60A0
		private void OnStartRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			HTTPRequestStates state = req.State;
			if (state == HTTPRequestStates.Finished)
			{
				if (resp.IsSuccess)
				{
					HTTPManager.Logger.Information("Transport - " + this.Name, "Start - Returned: " + resp.DataAsText);
					string text = this.Connection.ParseResponse(resp.DataAsText);
					if (text != "started")
					{
						this.Connection.Error(string.Format("Expected 'started' response, but '{0}' found!", text));
						return;
					}
					this.State = TransportStates.Started;
					this.Started();
					this.Connection.TransportStarted();
					return;
				}
				else
				{
					HTTPManager.Logger.Warning("Transport - " + this.Name, string.Format("Start - request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2} Uri: {3}", new object[]
					{
						resp.StatusCode,
						resp.Message,
						resp.DataAsText,
						req.CurrentUri
					}));
				}
			}
			HTTPManager.Logger.Information("Transport - " + this.Name, "Start request state: " + req.State.ToString());
			int num = (int)req.Tag;
			if (num++ < 5)
			{
				req.Tag = num;
				req.Send();
				return;
			}
			this.Connection.Error("Failed to send Start request.");
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x000A8000 File Offset: 0x000A6200
		public virtual void Abort()
		{
			if (this.State != TransportStates.Started)
			{
				return;
			}
			this.State = TransportStates.Closing;
			HTTPRequest httprequest = new HTTPRequest(this.Connection.BuildUri(RequestTypes.Abort, this), HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnAbortRequestFinished));
			httprequest.Tag = 0;
			httprequest.DisableRetry = true;
			this.Connection.PrepareRequest(httprequest, RequestTypes.Abort);
			httprequest.Send();
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x000A8068 File Offset: 0x000A6268
		protected void AbortFinished()
		{
			this.State = TransportStates.Closed;
			this.Connection.TransportAborted();
			this.Aborted();
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x000A8084 File Offset: 0x000A6284
		private void OnAbortRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			HTTPRequestStates state = req.State;
			if (state == HTTPRequestStates.Finished)
			{
				if (resp.IsSuccess)
				{
					HTTPManager.Logger.Information("Transport - " + this.Name, "Abort - Returned: " + resp.DataAsText);
					if (this.State == TransportStates.Closing)
					{
						this.AbortFinished();
						return;
					}
					return;
				}
				else
				{
					HTTPManager.Logger.Warning("Transport - " + this.Name, string.Format("Abort - Handshake request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2} Uri: {3}", new object[]
					{
						resp.StatusCode,
						resp.Message,
						resp.DataAsText,
						req.CurrentUri
					}));
				}
			}
			HTTPManager.Logger.Information("Transport - " + this.Name, "Abort request state: " + req.State.ToString());
			int num = (int)req.Tag;
			if (num++ < 5)
			{
				req.Tag = num;
				req.Send();
				return;
			}
			this.Connection.Error("Failed to send Abort request!");
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x000A81A8 File Offset: 0x000A63A8
		public void Send(string jsonStr)
		{
			try
			{
				HTTPManager.Logger.Information("Transport - " + this.Name, "Sending: " + jsonStr);
				this.SendImpl(jsonStr);
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("Transport - " + this.Name, "Send", ex);
			}
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x000A8218 File Offset: 0x000A6418
		public void Reconnect()
		{
			HTTPManager.Logger.Information("Transport - " + this.Name, "Reconnecting");
			this.Stop();
			this.State = TransportStates.Reconnecting;
			this.Connect();
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x000A824C File Offset: 0x000A644C
		public static IServerMessage Parse(IJsonEncoder encoder, string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				HTTPManager.Logger.Error("MessageFactory", "Parse - called with empty or null string!");
				return null;
			}
			if (json.Length == 2 && json == "{}")
			{
				return new KeepAliveMessage();
			}
			IDictionary<string, object> dictionary = null;
			try
			{
				dictionary = encoder.DecodeMessage(json);
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("MessageFactory", "Parse - encoder.DecodeMessage", ex);
				return null;
			}
			if (dictionary == null)
			{
				HTTPManager.Logger.Error("MessageFactory", "Parse - Json Decode failed for json string: \"" + json + "\"");
				return null;
			}
			IServerMessage serverMessage = null;
			if (!dictionary.ContainsKey("C"))
			{
				if (!dictionary.ContainsKey("E"))
				{
					serverMessage = new ResultMessage();
				}
				else
				{
					serverMessage = new FailureMessage();
				}
			}
			else
			{
				serverMessage = new MultiMessage();
			}
			try
			{
				serverMessage.Parse(dictionary);
			}
			catch
			{
				HTTPManager.Logger.Error("MessageFactory", "Can't parse msg: " + json);
				throw;
			}
			return serverMessage;
		}

		// Token: 0x040015BE RID: 5566
		private const int MaxRetryCount = 5;

		// Token: 0x040015C1 RID: 5569
		public TransportStates _state;
	}
}
