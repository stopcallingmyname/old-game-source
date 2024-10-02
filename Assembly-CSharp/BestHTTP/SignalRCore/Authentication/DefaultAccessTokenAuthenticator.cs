using System;

namespace BestHTTP.SignalRCore.Authentication
{
	// Token: 0x020001FE RID: 510
	public sealed class DefaultAccessTokenAuthenticator : IAuthenticationProvider
	{
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06001291 RID: 4753 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool IsPreAuthRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06001292 RID: 4754 RVA: 0x000A5248 File Offset: 0x000A3448
		// (remove) Token: 0x06001293 RID: 4755 RVA: 0x000A5280 File Offset: 0x000A3480
		public event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06001294 RID: 4756 RVA: 0x000A52B8 File Offset: 0x000A34B8
		// (remove) Token: 0x06001295 RID: 4757 RVA: 0x000A52F0 File Offset: 0x000A34F0
		public event OnAuthenticationFailedDelegate OnAuthenticationFailed;

		// Token: 0x06001296 RID: 4758 RVA: 0x000A5325 File Offset: 0x000A3525
		public DefaultAccessTokenAuthenticator(HubConnection connection)
		{
			this._connection = connection;
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x0000248C File Offset: 0x0000068C
		public void StartAuthentication()
		{
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x000A5334 File Offset: 0x000A3534
		public void PrepareRequest(HTTPRequest request)
		{
			if (HTTPProtocolFactory.GetProtocolFromUri(request.CurrentUri) == SupportedProtocols.HTTP)
			{
				request.Uri = this.PrepareUri(request.Uri);
			}
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x000A5358 File Offset: 0x000A3558
		public Uri PrepareUri(Uri uri)
		{
			if (this._connection.NegotiationResult != null && !string.IsNullOrEmpty(this._connection.NegotiationResult.AccessToken))
			{
				string str = string.IsNullOrEmpty(uri.Query) ? "?" : (uri.Query + "&");
				return new UriBuilder(uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath, str + "access_token=" + this._connection.NegotiationResult.AccessToken).Uri;
			}
			return uri;
		}

		// Token: 0x04001556 RID: 5462
		private HubConnection _connection;
	}
}
