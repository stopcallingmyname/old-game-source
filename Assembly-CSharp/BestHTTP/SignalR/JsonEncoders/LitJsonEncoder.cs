using System;
using System.Collections.Generic;
using LitJson;

namespace BestHTTP.SignalR.JsonEncoders
{
	// Token: 0x02000221 RID: 545
	public sealed class LitJsonEncoder : IJsonEncoder
	{
		// Token: 0x060013C1 RID: 5057 RVA: 0x000A8C0C File Offset: 0x000A6E0C
		public string Encode(object obj)
		{
			JsonWriter jsonWriter = new JsonWriter();
			JsonMapper.ToJson(obj, jsonWriter);
			return jsonWriter.ToString();
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x000A8C2C File Offset: 0x000A6E2C
		public IDictionary<string, object> DecodeMessage(string json)
		{
			return JsonMapper.ToObject<Dictionary<string, object>>(new JsonReader(json));
		}
	}
}
