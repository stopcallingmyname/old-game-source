using System;
using System.Collections.Generic;

namespace Facebook.Unity.Example
{
	// Token: 0x02000143 RID: 323
	internal class AppEvents : MenuBase
	{
		// Token: 0x06000B22 RID: 2850 RVA: 0x0008A1D8 File Offset: 0x000883D8
		protected override void GetGui()
		{
			if (base.Button("Log FB App Event"))
			{
				base.Status = "Logged FB.AppEvent";
				FB.LogAppEvent("fb_mobile_achievement_unlocked", null, new Dictionary<string, object>
				{
					{
						"fb_description",
						"Clicked 'Log AppEvent' button"
					}
				});
				LogView.AddLog("You may see results showing up at https://www.facebook.com/analytics/" + FB.AppId);
			}
		}
	}
}
