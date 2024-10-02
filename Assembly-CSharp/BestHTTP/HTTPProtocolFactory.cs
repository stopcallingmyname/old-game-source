using System;
using System.IO;
using BestHTTP.Extensions;
using BestHTTP.ServerSentEvents;
using BestHTTP.WebSocket;

namespace BestHTTP
{
	// Token: 0x0200017C RID: 380
	public static class HTTPProtocolFactory
	{
		// Token: 0x06000D74 RID: 3444 RVA: 0x00092BFC File Offset: 0x00090DFC
		public static HTTPResponse Get(SupportedProtocols protocol, HTTPRequest request, Stream stream, bool isStreamed, bool isFromCache)
		{
			if (protocol == SupportedProtocols.WebSocket)
			{
				return new WebSocketResponse(request, stream, isStreamed, isFromCache);
			}
			if (protocol != SupportedProtocols.ServerSentEvents)
			{
				return new HTTPResponse(request, new ReadOnlyBufferedStream(stream), isStreamed, isFromCache);
			}
			return new EventSourceResponse(request, stream, isStreamed, isFromCache);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00092C30 File Offset: 0x00090E30
		public static SupportedProtocols GetProtocolFromUri(Uri uri)
		{
			if (uri == null || uri.Scheme == null)
			{
				throw new Exception("Malformed URI in GetProtocolFromUri");
			}
			string a = uri.Scheme.ToLowerInvariant();
			if (a == "ws" || a == "wss")
			{
				return SupportedProtocols.WebSocket;
			}
			return SupportedProtocols.HTTP;
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x00092C84 File Offset: 0x00090E84
		public static bool IsSecureProtocol(Uri uri)
		{
			if (uri == null || uri.Scheme == null)
			{
				throw new Exception("Malformed URI in IsSecureProtocol");
			}
			string a = uri.Scheme.ToLowerInvariant();
			return a == "https" || a == "wss";
		}
	}
}
