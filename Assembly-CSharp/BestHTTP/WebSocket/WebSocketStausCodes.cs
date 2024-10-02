using System;

namespace BestHTTP.WebSocket
{
	// Token: 0x020001B9 RID: 441
	public enum WebSocketStausCodes : ushort
	{
		// Token: 0x0400142A RID: 5162
		NormalClosure = 1000,
		// Token: 0x0400142B RID: 5163
		GoingAway,
		// Token: 0x0400142C RID: 5164
		ProtocolError,
		// Token: 0x0400142D RID: 5165
		WrongDataType,
		// Token: 0x0400142E RID: 5166
		Reserved,
		// Token: 0x0400142F RID: 5167
		NoStatusCode,
		// Token: 0x04001430 RID: 5168
		ClosedAbnormally,
		// Token: 0x04001431 RID: 5169
		DataError,
		// Token: 0x04001432 RID: 5170
		PolicyError,
		// Token: 0x04001433 RID: 5171
		TooBigMessage,
		// Token: 0x04001434 RID: 5172
		ExtensionExpected,
		// Token: 0x04001435 RID: 5173
		WrongRequest,
		// Token: 0x04001436 RID: 5174
		TLSHandshakeError = 1015
	}
}
