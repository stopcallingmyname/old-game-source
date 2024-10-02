using System;
using System.Collections.Generic;
using BestHTTP.JSON;

namespace BestHTTP.SocketIO.JsonEncoders
{
	// Token: 0x020001D2 RID: 466
	public sealed class DefaultJSonEncoder : IJsonEncoder
	{
		// Token: 0x06001185 RID: 4485 RVA: 0x000A2376 File Offset: 0x000A0576
		public List<object> Decode(string json)
		{
			return Json.Decode(json) as List<object>;
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x000A2383 File Offset: 0x000A0583
		public string Encode(List<object> obj)
		{
			return Json.Encode(obj);
		}
	}
}
