using System;

namespace Facebook.Unity.Example
{
	// Token: 0x02000145 RID: 325
	internal class AppLinks : MenuBase
	{
		// Token: 0x06000B26 RID: 2854 RVA: 0x0008A32C File Offset: 0x0008852C
		protected override void GetGui()
		{
			if (base.Button("Get App Link"))
			{
				FB.GetAppLink(new FacebookDelegate<IAppLinkResult>(base.HandleResult));
			}
			if (Constants.IsMobile && base.Button("Fetch Deferred App Link"))
			{
				FB.Mobile.FetchDeferredAppLinkData(new FacebookDelegate<IAppLinkResult>(base.HandleResult));
			}
		}
	}
}
