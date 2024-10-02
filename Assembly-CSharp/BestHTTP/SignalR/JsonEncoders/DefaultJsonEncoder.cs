using System;
using System.Collections.Generic;
using BestHTTP.JSON;

namespace BestHTTP.SignalR.JsonEncoders
{
	// Token: 0x0200021F RID: 543
	public sealed class DefaultJsonEncoder : IJsonEncoder
	{
		// Token: 0x060013BC RID: 5052 RVA: 0x000A2383 File Offset: 0x000A0583
		public string Encode(object obj)
		{
			return Json.Encode(obj);
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x000A8BE8 File Offset: 0x000A6DE8
		public IDictionary<string, object> DecodeMessage(string json)
		{
			bool flag = false;
			IDictionary<string, object> result = Json.Decode(json, ref flag) as IDictionary<string, object>;
			if (!flag)
			{
				return null;
			}
			return result;
		}
	}
}
