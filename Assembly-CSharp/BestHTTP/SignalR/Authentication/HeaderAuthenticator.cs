using System;

namespace BestHTTP.SignalR.Authentication
{
	// Token: 0x0200022D RID: 557
	internal class HeaderAuthenticator : IAuthenticationProvider
	{
		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x000A9692 File Offset: 0x000A7892
		// (set) Token: 0x06001418 RID: 5144 RVA: 0x000A969A File Offset: 0x000A789A
		public string User { get; private set; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x000A96A3 File Offset: 0x000A78A3
		// (set) Token: 0x0600141A RID: 5146 RVA: 0x000A96AB File Offset: 0x000A78AB
		public string Roles { get; private set; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool IsPreAuthRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x0600141C RID: 5148 RVA: 0x000A96B4 File Offset: 0x000A78B4
		// (remove) Token: 0x0600141D RID: 5149 RVA: 0x000A96EC File Offset: 0x000A78EC
		public event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x0600141E RID: 5150 RVA: 0x000A9724 File Offset: 0x000A7924
		// (remove) Token: 0x0600141F RID: 5151 RVA: 0x000A975C File Offset: 0x000A795C
		public event OnAuthenticationFailedDelegate OnAuthenticationFailed;

		// Token: 0x06001420 RID: 5152 RVA: 0x000A9791 File Offset: 0x000A7991
		public HeaderAuthenticator(string user, string roles)
		{
			this.User = user;
			this.Roles = roles;
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x0000248C File Offset: 0x0000068C
		public void StartAuthentication()
		{
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x000A97A7 File Offset: 0x000A79A7
		public void PrepareRequest(HTTPRequest request, RequestTypes type)
		{
			request.SetHeader("username", this.User);
			request.SetHeader("roles", this.Roles);
		}
	}
}
