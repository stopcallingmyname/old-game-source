using System;
using System.Collections.Generic;
using UnityEngine;

namespace Facebook.Unity.Example
{
	// Token: 0x02000149 RID: 329
	internal sealed class MainMenu : MenuBase
	{
		// Token: 0x06000B32 RID: 2866 RVA: 0x0007D96F File Offset: 0x0007BB6F
		protected override bool ShowBackButton()
		{
			return false;
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0008AC00 File Offset: 0x00088E00
		protected override void GetGui()
		{
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			bool enabled = GUI.enabled;
			if (base.Button("FB.Init"))
			{
				FB.Init(new InitDelegate(this.OnInitComplete), new HideUnityDelegate(this.OnHideUnity), null);
				base.Status = "FB.Init() called with " + FB.AppId;
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUI.enabled = (enabled && FB.IsInitialized);
			if (base.Button("Login"))
			{
				this.CallFBLogin();
				base.Status = "Login called";
			}
			GUI.enabled = FB.IsLoggedIn;
			if (base.Button("Get publish_actions"))
			{
				this.CallFBLoginForPublish();
				base.Status = "Login (for publish_actions) called";
			}
			GUILayout.Label(GUIContent.none, new GUILayoutOption[]
			{
				GUILayout.MinWidth((float)ConsoleBase.MarginFix)
			});
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label(GUIContent.none, new GUILayoutOption[]
			{
				GUILayout.MinWidth((float)ConsoleBase.MarginFix)
			});
			GUILayout.EndHorizontal();
			if (base.Button("Logout"))
			{
				this.CallFBLogout();
				base.Status = "Logout called";
			}
			GUI.enabled = (enabled && FB.IsInitialized);
			if (base.Button("Share Dialog"))
			{
				base.SwitchMenu(typeof(DialogShare));
			}
			if (base.Button("App Requests"))
			{
				base.SwitchMenu(typeof(AppRequests));
			}
			if (base.Button("Graph Request"))
			{
				base.SwitchMenu(typeof(GraphRequest));
			}
			if (Constants.IsWeb && base.Button("Pay"))
			{
				base.SwitchMenu(typeof(Pay));
			}
			if (base.Button("App Events"))
			{
				base.SwitchMenu(typeof(AppEvents));
			}
			if (base.Button("App Links"))
			{
				base.SwitchMenu(typeof(AppLinks));
			}
			if (Constants.IsMobile && base.Button("App Invites"))
			{
				base.SwitchMenu(typeof(AppInvites));
			}
			if (Constants.IsMobile && base.Button("Access Token"))
			{
				base.SwitchMenu(typeof(AccessTokenMenu));
			}
			GUILayout.EndVertical();
			GUI.enabled = enabled;
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0008AE43 File Offset: 0x00089043
		private void CallFBLogin()
		{
			FB.LogInWithReadPermissions(new List<string>
			{
				"public_profile",
				"email",
				"user_friends"
			}, new FacebookDelegate<ILoginResult>(base.HandleResult));
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0008AE7C File Offset: 0x0008907C
		private void CallFBLoginForPublish()
		{
			FB.LogInWithPublishPermissions(new List<string>
			{
				"publish_actions"
			}, new FacebookDelegate<ILoginResult>(base.HandleResult));
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0008AE9F File Offset: 0x0008909F
		private void CallFBLogout()
		{
			FB.LogOut();
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0008AEA8 File Offset: 0x000890A8
		private void OnInitComplete()
		{
			base.Status = "Success - Check log for details";
			base.LastResponse = "Success Response: OnInitComplete Called\n";
			LogView.AddLog(string.Format("OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'", FB.IsLoggedIn, FB.IsInitialized));
			if (AccessToken.CurrentAccessToken != null)
			{
				LogView.AddLog(AccessToken.CurrentAccessToken.ToString());
			}
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0008AF04 File Offset: 0x00089104
		private void OnHideUnity(bool isGameShown)
		{
			base.Status = "Success - Check log for details";
			base.LastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
			LogView.AddLog("Is game shown: " + isGameShown.ToString());
		}
	}
}
