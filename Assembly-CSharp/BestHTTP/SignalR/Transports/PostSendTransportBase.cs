using System;
using System.Collections.Generic;
using BestHTTP.Forms;
using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Transports
{
	// Token: 0x02000210 RID: 528
	public abstract class PostSendTransportBase : TransportBase
	{
		// Token: 0x0600133F RID: 4927 RVA: 0x000A7850 File Offset: 0x000A5A50
		public PostSendTransportBase(string name, Connection con) : base(name, con)
		{
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x000A7868 File Offset: 0x000A5A68
		protected override void SendImpl(string json)
		{
			HTTPRequest httprequest = new HTTPRequest(base.Connection.BuildUri(RequestTypes.Send, this), HTTPMethods.Post, true, true, new OnRequestFinishedDelegate(this.OnSendRequestFinished));
			httprequest.FormUsage = HTTPFormUsage.UrlEncoded;
			httprequest.AddField("data", json);
			base.Connection.PrepareRequest(httprequest, RequestTypes.Send);
			httprequest.Priority = -1;
			httprequest.Send();
			this.sendRequestQueue.Add(httprequest);
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x000A78D4 File Offset: 0x000A5AD4
		private void OnSendRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			this.sendRequestQueue.Remove(req);
			string text = string.Empty;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (resp.IsSuccess)
				{
					HTTPManager.Logger.Information("Transport - " + base.Name, "Send - Request Finished Successfully! " + resp.DataAsText);
					if (!string.IsNullOrEmpty(resp.DataAsText))
					{
						IServerMessage serverMessage = TransportBase.Parse(base.Connection.JsonEncoder, resp.DataAsText);
						if (serverMessage != null)
						{
							base.Connection.OnMessage(serverMessage);
						}
					}
				}
				else
				{
					text = string.Format("Send - Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText);
				}
				break;
			case HTTPRequestStates.Error:
				text = "Send - Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
				break;
			case HTTPRequestStates.Aborted:
				text = "Send - Request Aborted!";
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = "Send - Connection Timed Out!";
				break;
			case HTTPRequestStates.TimedOut:
				text = "Send - Processing the request Timed Out!";
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				base.Connection.Error(text);
			}
		}

		// Token: 0x040015BC RID: 5564
		protected List<HTTPRequest> sendRequestQueue = new List<HTTPRequest>();
	}
}
