using System;

namespace BestHTTP.SignalR.Authentication
{
	// Token: 0x0200022B RID: 555
	public interface IAuthenticationProvider
	{
		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060013FE RID: 5118
		bool IsPreAuthRequired { get; }

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x060013FF RID: 5119
		// (remove) Token: 0x06001400 RID: 5120
		event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06001401 RID: 5121
		// (remove) Token: 0x06001402 RID: 5122
		event OnAuthenticationFailedDelegate OnAuthenticationFailed;

		// Token: 0x06001403 RID: 5123
		void StartAuthentication();

		// Token: 0x06001404 RID: 5124
		void PrepareRequest(HTTPRequest request, RequestTypes type);
	}
}
