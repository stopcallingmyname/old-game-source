using System;
using BestHTTP.SignalRCore;

namespace BestHTTP.Examples
{
	// Token: 0x020001A8 RID: 424
	public sealed class RedirectLoggerAccessTokenAuthenticator : IAuthenticationProvider
	{
		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool IsPreAuthRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000F89 RID: 3977 RVA: 0x0009AC28 File Offset: 0x00098E28
		// (remove) Token: 0x06000F8A RID: 3978 RVA: 0x0009AC60 File Offset: 0x00098E60
		public event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000F8B RID: 3979 RVA: 0x0009AC98 File Offset: 0x00098E98
		// (remove) Token: 0x06000F8C RID: 3980 RVA: 0x0009ACD0 File Offset: 0x00098ED0
		public event OnAuthenticationFailedDelegate OnAuthenticationFailed;

		// Token: 0x06000F8D RID: 3981 RVA: 0x0009AD05 File Offset: 0x00098F05
		public RedirectLoggerAccessTokenAuthenticator(HubConnection connection)
		{
			this._connection = connection;
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0000248C File Offset: 0x0000068C
		public void StartAuthentication()
		{
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0009AD14 File Offset: 0x00098F14
		public void PrepareRequest(HTTPRequest request)
		{
			request.SetHeader("x-redirect-count", this._connection.RedirectCount.ToString());
			if (HTTPProtocolFactory.GetProtocolFromUri(request.CurrentUri) == SupportedProtocols.HTTP)
			{
				request.Uri = this.PrepareUri(request.Uri);
			}
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x0009AD60 File Offset: 0x00098F60
		public Uri PrepareUri(Uri uri)
		{
			if (this._connection.NegotiationResult != null && !string.IsNullOrEmpty(this._connection.NegotiationResult.AccessToken))
			{
				string str = string.IsNullOrEmpty(uri.Query) ? "?" : (uri.Query + "&");
				return new UriBuilder(uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath, str + "access_token=" + this._connection.NegotiationResult.AccessToken).Uri;
			}
			return uri;
		}

		// Token: 0x040013D8 RID: 5080
		private HubConnection _connection;
	}
}
