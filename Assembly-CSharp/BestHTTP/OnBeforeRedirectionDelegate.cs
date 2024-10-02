using System;

namespace BestHTTP
{
	// Token: 0x02000184 RID: 388
	// (Invoke) Token: 0x06000DA4 RID: 3492
	public delegate bool OnBeforeRedirectionDelegate(HTTPRequest originalRequest, HTTPResponse response, Uri redirectUri);
}
