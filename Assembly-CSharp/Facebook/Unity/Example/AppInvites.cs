using System;

namespace Facebook.Unity.Example
{
	// Token: 0x02000144 RID: 324
	internal class AppInvites : MenuBase
	{
		// Token: 0x06000B24 RID: 2852 RVA: 0x0008A23C File Offset: 0x0008843C
		protected override void GetGui()
		{
			if (base.Button("Android Invite"))
			{
				base.Status = "Logged FB.AppEvent";
				FB.Mobile.AppInvite(new Uri("https://fb.me/892708710750483"), null, new FacebookDelegate<IAppInviteResult>(base.HandleResult));
			}
			if (base.Button("Android Invite With Custom Image"))
			{
				base.Status = "Logged FB.AppEvent";
				FB.Mobile.AppInvite(new Uri("https://fb.me/892708710750483"), new Uri("http://i.imgur.com/zkYlB.jpg"), new FacebookDelegate<IAppInviteResult>(base.HandleResult));
			}
			if (base.Button("iOS Invite"))
			{
				base.Status = "Logged FB.AppEvent";
				FB.Mobile.AppInvite(new Uri("https://fb.me/810530068992919"), null, new FacebookDelegate<IAppInviteResult>(base.HandleResult));
			}
			if (base.Button("iOS Invite With Custom Image"))
			{
				base.Status = "Logged FB.AppEvent";
				FB.Mobile.AppInvite(new Uri("https://fb.me/810530068992919"), new Uri("http://i.imgur.com/zkYlB.jpg"), new FacebookDelegate<IAppInviteResult>(base.HandleResult));
			}
		}
	}
}
