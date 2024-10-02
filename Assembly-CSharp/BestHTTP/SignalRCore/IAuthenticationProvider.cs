using System;

namespace BestHTTP.SignalRCore
{
	// Token: 0x020001E9 RID: 489
	public interface IAuthenticationProvider
	{
		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600121C RID: 4636
		bool IsPreAuthRequired { get; }

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600121D RID: 4637
		// (remove) Token: 0x0600121E RID: 4638
		event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600121F RID: 4639
		// (remove) Token: 0x06001220 RID: 4640
		event OnAuthenticationFailedDelegate OnAuthenticationFailed;

		// Token: 0x06001221 RID: 4641
		void StartAuthentication();

		// Token: 0x06001222 RID: 4642
		void PrepareRequest(HTTPRequest request);

		// Token: 0x06001223 RID: 4643
		Uri PrepareUri(Uri uri);
	}
}
