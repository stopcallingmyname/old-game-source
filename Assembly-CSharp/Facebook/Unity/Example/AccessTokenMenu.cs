using System;

namespace Facebook.Unity.Example
{
	// Token: 0x02000142 RID: 322
	internal class AccessTokenMenu : MenuBase
	{
		// Token: 0x06000B20 RID: 2848 RVA: 0x0008A1AE File Offset: 0x000883AE
		protected override void GetGui()
		{
			if (base.Button("Refresh Access Token"))
			{
				FB.Mobile.RefreshCurrentAccessToken(new FacebookDelegate<IAccessTokenRefreshResult>(base.HandleResult));
			}
		}
	}
}
