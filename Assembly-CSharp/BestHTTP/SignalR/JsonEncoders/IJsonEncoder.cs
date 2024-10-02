using System;
using System.Collections.Generic;

namespace BestHTTP.SignalR.JsonEncoders
{
	// Token: 0x02000220 RID: 544
	public interface IJsonEncoder
	{
		// Token: 0x060013BF RID: 5055
		string Encode(object obj);

		// Token: 0x060013C0 RID: 5056
		IDictionary<string, object> DecodeMessage(string json);
	}
}
