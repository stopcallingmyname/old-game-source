using System;
using System.Collections.Generic;

namespace BestHTTP.SocketIO.JsonEncoders
{
	// Token: 0x020001D3 RID: 467
	public interface IJsonEncoder
	{
		// Token: 0x06001188 RID: 4488
		List<object> Decode(string json);

		// Token: 0x06001189 RID: 4489
		string Encode(List<object> obj);
	}
}
