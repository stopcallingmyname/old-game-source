using System;
using System.Collections.Generic;
using BestHTTP.SignalRCore.Messages;

namespace BestHTTP.SignalRCore
{
	// Token: 0x020001EA RID: 490
	public interface IProtocol
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06001224 RID: 4644
		TransferModes Type { get; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06001225 RID: 4645
		IEncoder Encoder { get; }

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06001226 RID: 4646
		// (set) Token: 0x06001227 RID: 4647
		HubConnection Connection { get; set; }

		// Token: 0x06001228 RID: 4648
		void ParseMessages(string data, ref List<Message> messages);

		// Token: 0x06001229 RID: 4649
		void ParseMessages(byte[] data, ref List<Message> messages);

		// Token: 0x0600122A RID: 4650
		byte[] EncodeMessage(Message message);

		// Token: 0x0600122B RID: 4651
		object[] GetRealArguments(Type[] argTypes, object[] arguments);

		// Token: 0x0600122C RID: 4652
		object ConvertTo(Type toType, object obj);
	}
}
