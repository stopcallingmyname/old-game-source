using System;
using BestHTTP.WebSocket.Frames;

namespace BestHTTP.WebSocket.Extensions
{
	// Token: 0x020001BE RID: 446
	public interface IExtension
	{
		// Token: 0x06001083 RID: 4227
		void AddNegotiation(HTTPRequest request);

		// Token: 0x06001084 RID: 4228
		bool ParseNegotiation(WebSocketResponse resp);

		// Token: 0x06001085 RID: 4229
		byte GetFrameHeader(WebSocketFrame writer, byte inFlag);

		// Token: 0x06001086 RID: 4230
		byte[] Encode(WebSocketFrame writer);

		// Token: 0x06001087 RID: 4231
		byte[] Decode(byte header, byte[] data, int length);
	}
}
