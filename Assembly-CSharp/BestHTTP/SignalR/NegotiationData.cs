using System;
using System.Collections.Generic;
using BestHTTP.JSON;

namespace BestHTTP.SignalR
{
	// Token: 0x0200020E RID: 526
	public sealed class NegotiationData
	{
		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06001313 RID: 4883 RVA: 0x000A6E7C File Offset: 0x000A507C
		// (set) Token: 0x06001314 RID: 4884 RVA: 0x000A6E84 File Offset: 0x000A5084
		public string Url { get; private set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06001315 RID: 4885 RVA: 0x000A6E8D File Offset: 0x000A508D
		// (set) Token: 0x06001316 RID: 4886 RVA: 0x000A6E95 File Offset: 0x000A5095
		public string WebSocketServerUrl { get; private set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06001317 RID: 4887 RVA: 0x000A6E9E File Offset: 0x000A509E
		// (set) Token: 0x06001318 RID: 4888 RVA: 0x000A6EA6 File Offset: 0x000A50A6
		public string ConnectionToken { get; private set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06001319 RID: 4889 RVA: 0x000A6EAF File Offset: 0x000A50AF
		// (set) Token: 0x0600131A RID: 4890 RVA: 0x000A6EB7 File Offset: 0x000A50B7
		public string ConnectionId { get; private set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600131B RID: 4891 RVA: 0x000A6EC0 File Offset: 0x000A50C0
		// (set) Token: 0x0600131C RID: 4892 RVA: 0x000A6EC8 File Offset: 0x000A50C8
		public TimeSpan? KeepAliveTimeout { get; private set; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600131D RID: 4893 RVA: 0x000A6ED1 File Offset: 0x000A50D1
		// (set) Token: 0x0600131E RID: 4894 RVA: 0x000A6ED9 File Offset: 0x000A50D9
		public TimeSpan DisconnectTimeout { get; private set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x0600131F RID: 4895 RVA: 0x000A6EE2 File Offset: 0x000A50E2
		// (set) Token: 0x06001320 RID: 4896 RVA: 0x000A6EEA File Offset: 0x000A50EA
		public TimeSpan ConnectionTimeout { get; private set; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06001321 RID: 4897 RVA: 0x000A6EF3 File Offset: 0x000A50F3
		// (set) Token: 0x06001322 RID: 4898 RVA: 0x000A6EFB File Offset: 0x000A50FB
		public bool TryWebSockets { get; private set; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06001323 RID: 4899 RVA: 0x000A6F04 File Offset: 0x000A5104
		// (set) Token: 0x06001324 RID: 4900 RVA: 0x000A6F0C File Offset: 0x000A510C
		public string ProtocolVersion { get; private set; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06001325 RID: 4901 RVA: 0x000A6F15 File Offset: 0x000A5115
		// (set) Token: 0x06001326 RID: 4902 RVA: 0x000A6F1D File Offset: 0x000A511D
		public TimeSpan TransportConnectTimeout { get; private set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06001327 RID: 4903 RVA: 0x000A6F26 File Offset: 0x000A5126
		// (set) Token: 0x06001328 RID: 4904 RVA: 0x000A6F2E File Offset: 0x000A512E
		public TimeSpan LongPollDelay { get; private set; }

		// Token: 0x06001329 RID: 4905 RVA: 0x000A6F37 File Offset: 0x000A5137
		public NegotiationData(Connection connection)
		{
			this.Connection = connection;
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x000A6F48 File Offset: 0x000A5148
		public void Start()
		{
			this.NegotiationRequest = new HTTPRequest(this.Connection.BuildUri(RequestTypes.Negotiate), HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnNegotiationRequestFinished));
			this.Connection.PrepareRequest(this.NegotiationRequest, RequestTypes.Negotiate);
			this.NegotiationRequest.Send();
			HTTPManager.Logger.Information("NegotiationData", "Negotiation request sent");
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x000A6FAE File Offset: 0x000A51AE
		public void Abort()
		{
			if (this.NegotiationRequest != null)
			{
				this.OnReceived = null;
				this.OnError = null;
				this.NegotiationRequest.Abort();
			}
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x000A6FD4 File Offset: 0x000A51D4
		private void OnNegotiationRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			this.NegotiationRequest = null;
			HTTPRequestStates state = req.State;
			if (state != HTTPRequestStates.Finished)
			{
				if (state == HTTPRequestStates.Error)
				{
					this.RaiseOnError((req.Exception != null) ? (req.Exception.Message + " " + req.Exception.StackTrace) : string.Empty);
					return;
				}
				this.RaiseOnError(req.State.ToString());
			}
			else
			{
				if (!resp.IsSuccess)
				{
					this.RaiseOnError(string.Format("Negotiation request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2} Uri: {3}", new object[]
					{
						resp.StatusCode,
						resp.Message,
						resp.DataAsText,
						req.CurrentUri
					}));
					return;
				}
				HTTPManager.Logger.Information("NegotiationData", "Negotiation data arrived: " + resp.DataAsText);
				int num = resp.DataAsText.IndexOf("{");
				if (num < 0)
				{
					this.RaiseOnError("Invalid negotiation text: " + resp.DataAsText);
					return;
				}
				if (this.Parse(resp.DataAsText.Substring(num)) == null)
				{
					this.RaiseOnError("Parsing Negotiation data failed: " + resp.DataAsText);
					return;
				}
				if (this.OnReceived != null)
				{
					this.OnReceived(this);
					this.OnReceived = null;
					return;
				}
			}
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x000A712F File Offset: 0x000A532F
		private void RaiseOnError(string err)
		{
			HTTPManager.Logger.Error("NegotiationData", "Negotiation request failed with error: " + err);
			if (this.OnError != null)
			{
				this.OnError(this, err);
				this.OnError = null;
			}
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x000A7168 File Offset: 0x000A5368
		private NegotiationData Parse(string str)
		{
			bool flag = false;
			Dictionary<string, object> dictionary = Json.Decode(str, ref flag) as Dictionary<string, object>;
			if (!flag)
			{
				return null;
			}
			try
			{
				this.Url = NegotiationData.GetString(dictionary, "Url");
				if (dictionary.ContainsKey("webSocketServerUrl"))
				{
					this.WebSocketServerUrl = NegotiationData.GetString(dictionary, "webSocketServerUrl");
				}
				this.ConnectionToken = Uri.EscapeDataString(NegotiationData.GetString(dictionary, "ConnectionToken"));
				this.ConnectionId = NegotiationData.GetString(dictionary, "ConnectionId");
				if (dictionary.ContainsKey("KeepAliveTimeout"))
				{
					this.KeepAliveTimeout = new TimeSpan?(TimeSpan.FromSeconds(NegotiationData.GetDouble(dictionary, "KeepAliveTimeout")));
				}
				this.DisconnectTimeout = TimeSpan.FromSeconds(NegotiationData.GetDouble(dictionary, "DisconnectTimeout"));
				if (dictionary.ContainsKey("ConnectionTimeout"))
				{
					this.ConnectionTimeout = TimeSpan.FromSeconds(NegotiationData.GetDouble(dictionary, "ConnectionTimeout"));
				}
				else
				{
					this.ConnectionTimeout = TimeSpan.FromSeconds(120.0);
				}
				this.TryWebSockets = (bool)NegotiationData.Get(dictionary, "TryWebSockets");
				this.ProtocolVersion = NegotiationData.GetString(dictionary, "ProtocolVersion");
				this.TransportConnectTimeout = TimeSpan.FromSeconds(NegotiationData.GetDouble(dictionary, "TransportConnectTimeout"));
				if (dictionary.ContainsKey("LongPollDelay"))
				{
					this.LongPollDelay = TimeSpan.FromSeconds(NegotiationData.GetDouble(dictionary, "LongPollDelay"));
				}
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("NegotiationData", "Parse", ex);
				return null;
			}
			return this;
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x000A72F4 File Offset: 0x000A54F4
		private static object Get(Dictionary<string, object> from, string key)
		{
			object result;
			if (!from.TryGetValue(key, out result))
			{
				throw new Exception(string.Format("Can't get {0} from Negotiation data!", key));
			}
			return result;
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x000A731E File Offset: 0x000A551E
		private static string GetString(Dictionary<string, object> from, string key)
		{
			return NegotiationData.Get(from, key) as string;
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x000A732C File Offset: 0x000A552C
		private static List<string> GetStringList(Dictionary<string, object> from, string key)
		{
			List<object> list = NegotiationData.Get(from, key) as List<object>;
			List<string> list2 = new List<string>(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				string text = list[i] as string;
				if (text != null)
				{
					list2.Add(text);
				}
			}
			return list2;
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x000A737B File Offset: 0x000A557B
		private static int GetInt(Dictionary<string, object> from, string key)
		{
			return (int)((double)NegotiationData.Get(from, key));
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x000A738A File Offset: 0x000A558A
		private static double GetDouble(Dictionary<string, object> from, string key)
		{
			return (double)NegotiationData.Get(from, key);
		}

		// Token: 0x040015B4 RID: 5556
		public Action<NegotiationData> OnReceived;

		// Token: 0x040015B5 RID: 5557
		public Action<NegotiationData, string> OnError;

		// Token: 0x040015B6 RID: 5558
		private HTTPRequest NegotiationRequest;

		// Token: 0x040015B7 RID: 5559
		private IConnection Connection;
	}
}
