using System;
using System.Collections.Generic;
using BestHTTP.JSON;

namespace BestHTTP.SignalRCore.Messages
{
	// Token: 0x020001FD RID: 509
	public sealed class NegotiationResult
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06001287 RID: 4743 RVA: 0x000A4FAA File Offset: 0x000A31AA
		// (set) Token: 0x06001288 RID: 4744 RVA: 0x000A4FB2 File Offset: 0x000A31B2
		public string ConnectionId { get; private set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06001289 RID: 4745 RVA: 0x000A4FBB File Offset: 0x000A31BB
		// (set) Token: 0x0600128A RID: 4746 RVA: 0x000A4FC3 File Offset: 0x000A31C3
		public List<SupportedTransport> SupportedTransports { get; private set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600128B RID: 4747 RVA: 0x000A4FCC File Offset: 0x000A31CC
		// (set) Token: 0x0600128C RID: 4748 RVA: 0x000A4FD4 File Offset: 0x000A31D4
		public Uri Url { get; private set; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600128D RID: 4749 RVA: 0x000A4FDD File Offset: 0x000A31DD
		// (set) Token: 0x0600128E RID: 4750 RVA: 0x000A4FE5 File Offset: 0x000A31E5
		public string AccessToken { get; private set; }

		// Token: 0x0600128F RID: 4751 RVA: 0x000A4FF0 File Offset: 0x000A31F0
		internal static NegotiationResult Parse(string json, out string error, HubConnection hub)
		{
			error = null;
			Dictionary<string, object> dictionary = Json.Decode(json) as Dictionary<string, object>;
			if (dictionary == null)
			{
				error = "Json decoding failed!";
				return null;
			}
			NegotiationResult result;
			try
			{
				NegotiationResult negotiationResult = new NegotiationResult();
				object obj;
				if (dictionary.TryGetValue("connectionId", out obj))
				{
					negotiationResult.ConnectionId = obj.ToString();
				}
				if (dictionary.TryGetValue("availableTransports", out obj))
				{
					List<object> list = obj as List<object>;
					if (list != null)
					{
						List<SupportedTransport> list2 = new List<SupportedTransport>(list.Count);
						foreach (object obj2 in list)
						{
							Dictionary<string, object> dictionary2 = (Dictionary<string, object>)obj2;
							string transportName = string.Empty;
							List<string> list3 = null;
							if (dictionary2.TryGetValue("transport", out obj))
							{
								transportName = obj.ToString();
							}
							if (dictionary2.TryGetValue("transferFormats", out obj))
							{
								List<object> list4 = obj as List<object>;
								if (list4 != null)
								{
									list3 = new List<string>(list4.Count);
									foreach (object obj3 in list4)
									{
										list3.Add(obj3.ToString());
									}
								}
							}
							list2.Add(new SupportedTransport(transportName, list3));
						}
						negotiationResult.SupportedTransports = list2;
					}
				}
				if (dictionary.TryGetValue("url", out obj))
				{
					string text = obj.ToString();
					Uri uri;
					if (!Uri.TryCreate(text, UriKind.RelativeOrAbsolute, out uri))
					{
						throw new Exception(string.Format("Couldn't parse url: '{0}'", text));
					}
					if (!uri.IsAbsoluteUri)
					{
						uri = new UriBuilder(hub.Uri)
						{
							Path = text
						}.Uri;
					}
					negotiationResult.Url = uri;
				}
				if (dictionary.TryGetValue("accessToken", out obj))
				{
					negotiationResult.AccessToken = obj.ToString();
				}
				else if (hub.NegotiationResult != null)
				{
					negotiationResult.AccessToken = hub.NegotiationResult.AccessToken;
				}
				result = negotiationResult;
			}
			catch (Exception ex)
			{
				error = "Error while parsing result: " + ex.Message + " StackTrace: " + ex.StackTrace;
				result = null;
			}
			return result;
		}
	}
}
