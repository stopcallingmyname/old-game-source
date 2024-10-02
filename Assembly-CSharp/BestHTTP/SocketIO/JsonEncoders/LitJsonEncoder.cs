using System;
using System.Collections.Generic;
using LitJson;

namespace BestHTTP.SocketIO.JsonEncoders
{
	// Token: 0x020001D4 RID: 468
	public sealed class LitJsonEncoder : IJsonEncoder
	{
		// Token: 0x0600118A RID: 4490 RVA: 0x000A238B File Offset: 0x000A058B
		public List<object> Decode(string json)
		{
			return JsonMapper.ToObject<List<object>>(new JsonReader(json));
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x000A2398 File Offset: 0x000A0598
		public string Encode(List<object> obj)
		{
			JsonWriter jsonWriter = new JsonWriter();
			JsonMapper.ToJson(obj, jsonWriter);
			return jsonWriter.ToString();
		}
	}
}
