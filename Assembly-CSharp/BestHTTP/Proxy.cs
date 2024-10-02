using System;
using System.IO;
using BestHTTP.Authentication;

namespace BestHTTP
{
	// Token: 0x0200017D RID: 381
	public abstract class Proxy
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000D77 RID: 3447 RVA: 0x00092CD6 File Offset: 0x00090ED6
		// (set) Token: 0x06000D78 RID: 3448 RVA: 0x00092CDE File Offset: 0x00090EDE
		public Uri Address { get; set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000D79 RID: 3449 RVA: 0x00092CE7 File Offset: 0x00090EE7
		// (set) Token: 0x06000D7A RID: 3450 RVA: 0x00092CEF File Offset: 0x00090EEF
		public Credentials Credentials { get; set; }

		// Token: 0x06000D7B RID: 3451 RVA: 0x00092CF8 File Offset: 0x00090EF8
		internal Proxy(Uri address, Credentials credentials)
		{
			this.Address = address;
			this.Credentials = credentials;
		}

		// Token: 0x06000D7C RID: 3452
		internal abstract void Connect(Stream stream, HTTPRequest request);

		// Token: 0x06000D7D RID: 3453
		internal abstract string GetRequestPath(Uri uri);
	}
}
