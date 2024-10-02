using System;

namespace BestHTTP.SignalRCore.Authentication
{
	// Token: 0x020001FF RID: 511
	public sealed class HeaderAuthenticator : IAuthenticationProvider
	{
		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600129A RID: 4762 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool IsPreAuthRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x0600129B RID: 4763 RVA: 0x000A53F0 File Offset: 0x000A35F0
		// (remove) Token: 0x0600129C RID: 4764 RVA: 0x000A5428 File Offset: 0x000A3628
		public event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x0600129D RID: 4765 RVA: 0x000A5460 File Offset: 0x000A3660
		// (remove) Token: 0x0600129E RID: 4766 RVA: 0x000A5498 File Offset: 0x000A3698
		public event OnAuthenticationFailedDelegate OnAuthenticationFailed;

		// Token: 0x0600129F RID: 4767 RVA: 0x000A54CD File Offset: 0x000A36CD
		public HeaderAuthenticator(string credentials)
		{
			this._credentials = credentials;
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x0000248C File Offset: 0x0000068C
		public void StartAuthentication()
		{
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x000A54DC File Offset: 0x000A36DC
		public void PrepareRequest(HTTPRequest request)
		{
			request.SetHeader("Authorization", "Bearer " + this._credentials);
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x000A54F9 File Offset: 0x000A36F9
		public Uri PrepareUri(Uri uri)
		{
			return uri;
		}

		// Token: 0x04001559 RID: 5465
		private string _credentials;
	}
}
