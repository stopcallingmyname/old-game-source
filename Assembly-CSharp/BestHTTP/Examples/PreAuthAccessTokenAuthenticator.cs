using System;
using BestHTTP.SignalRCore;

namespace BestHTTP.Examples
{
	// Token: 0x020001A6 RID: 422
	public sealed class PreAuthAccessTokenAuthenticator : IAuthenticationProvider
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000F70 RID: 3952 RVA: 0x0006AE98 File Offset: 0x00069098
		public bool IsPreAuthRequired
		{
			get
			{
				return true;
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000F71 RID: 3953 RVA: 0x0009A758 File Offset: 0x00098958
		// (remove) Token: 0x06000F72 RID: 3954 RVA: 0x0009A790 File Offset: 0x00098990
		public event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000F73 RID: 3955 RVA: 0x0009A7C8 File Offset: 0x000989C8
		// (remove) Token: 0x06000F74 RID: 3956 RVA: 0x0009A800 File Offset: 0x00098A00
		public event OnAuthenticationFailedDelegate OnAuthenticationFailed;

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000F75 RID: 3957 RVA: 0x0009A835 File Offset: 0x00098A35
		// (set) Token: 0x06000F76 RID: 3958 RVA: 0x0009A83D File Offset: 0x00098A3D
		public string Token { get; private set; }

		// Token: 0x06000F77 RID: 3959 RVA: 0x0009A846 File Offset: 0x00098A46
		public PreAuthAccessTokenAuthenticator(Uri authUri)
		{
			this.authenticationUri = authUri;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0009A855 File Offset: 0x00098A55
		public void StartAuthentication()
		{
			new HTTPRequest(this.authenticationUri, new OnRequestFinishedDelegate(this.OnAuthenticationRequestFinished)).Send();
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0009A874 File Offset: 0x00098A74
		private void OnAuthenticationRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (!resp.IsSuccess)
				{
					this.AuthenticationFailed(string.Format("Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText));
					return;
				}
				this.Token = resp.DataAsText;
				if (this.OnAuthenticationSucceded != null)
				{
					this.OnAuthenticationSucceded(this);
					return;
				}
				break;
			case HTTPRequestStates.Error:
				this.AuthenticationFailed("Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
				return;
			case HTTPRequestStates.Aborted:
				this.AuthenticationFailed("Request Aborted!");
				return;
			case HTTPRequestStates.ConnectionTimedOut:
				this.AuthenticationFailed("Connection Timed Out!");
				return;
			case HTTPRequestStates.TimedOut:
				this.AuthenticationFailed("Processing the request Timed Out!");
				break;
			default:
				return;
			}
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0009A95C File Offset: 0x00098B5C
		private void AuthenticationFailed(string reason)
		{
			if (this.OnAuthenticationFailed != null)
			{
				this.OnAuthenticationFailed(this, reason);
			}
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0009A973 File Offset: 0x00098B73
		public void PrepareRequest(HTTPRequest request)
		{
			if (HTTPProtocolFactory.GetProtocolFromUri(request.CurrentUri) == SupportedProtocols.HTTP)
			{
				request.Uri = this.PrepareUri(request.Uri);
			}
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0009A998 File Offset: 0x00098B98
		public Uri PrepareUri(Uri uri)
		{
			if (!string.IsNullOrEmpty(this.Token))
			{
				string str = string.IsNullOrEmpty(uri.Query) ? "?" : (uri.Query + "&");
				return new UriBuilder(uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath, str + "access_token=" + this.Token).Uri;
			}
			return uri;
		}

		// Token: 0x040013D0 RID: 5072
		private Uri authenticationUri;
	}
}
