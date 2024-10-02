using System;
using System.Linq;
using UnityEngine;

namespace Facebook.Unity.Example
{
	// Token: 0x02000141 RID: 321
	internal abstract class MenuBase : ConsoleBase
	{
		// Token: 0x06000B15 RID: 2837
		protected abstract void GetGui();

		// Token: 0x06000B16 RID: 2838 RVA: 0x0007D96F File Offset: 0x0007BB6F
		protected virtual bool ShowDialogModeSelector()
		{
			return false;
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0006AE98 File Offset: 0x00069098
		protected virtual bool ShowBackButton()
		{
			return true;
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00089EAC File Offset: 0x000880AC
		protected void HandleResult(IResult result)
		{
			if (result == null)
			{
				base.LastResponse = "Null Response\n";
				LogView.AddLog(base.LastResponse);
				return;
			}
			base.LastResponseTexture = null;
			if (!string.IsNullOrEmpty(result.Error))
			{
				base.Status = "Error - Check log for details";
				base.LastResponse = "Error Response:\n" + result.Error;
			}
			else if (result.Cancelled)
			{
				base.Status = "Cancelled - Check log for details";
				base.LastResponse = "Cancelled Response:\n" + result.RawResult;
			}
			else if (!string.IsNullOrEmpty(result.RawResult))
			{
				base.Status = "Success - Check log for details";
				base.LastResponse = "Success Response:\n" + result.RawResult;
			}
			else
			{
				base.LastResponse = "Empty Response\n";
			}
			LogView.AddLog(result.ToString());
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00089F7C File Offset: 0x0008817C
		protected void OnGUI()
		{
			if (base.IsHorizontalLayout())
			{
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			}
			GUILayout.Label(base.GetType().Name, base.LabelStyle, Array.Empty<GUILayoutOption>());
			this.AddStatus();
			base.ScrollPosition = GUILayout.BeginScrollView(base.ScrollPosition, new GUILayoutOption[]
			{
				GUILayout.MinWidth((float)ConsoleBase.MainWindowFullWidth)
			});
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (this.ShowBackButton())
			{
				this.AddBackButton();
			}
			this.AddLogButton();
			if (this.ShowBackButton())
			{
				GUILayout.Label(GUIContent.none, new GUILayoutOption[]
				{
					GUILayout.MinWidth((float)ConsoleBase.MarginFix)
				});
			}
			GUILayout.EndHorizontal();
			if (this.ShowDialogModeSelector())
			{
				this.AddDialogModeButtons();
			}
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			this.GetGui();
			GUILayout.Space(10f);
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0008A066 File Offset: 0x00088266
		private void AddStatus()
		{
			GUILayout.Space(5f);
			GUILayout.Box("Status: " + base.Status, base.TextStyle, new GUILayoutOption[]
			{
				GUILayout.MinWidth((float)ConsoleBase.MainWindowWidth)
			});
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0008A0A1 File Offset: 0x000882A1
		private void AddBackButton()
		{
			GUI.enabled = ConsoleBase.MenuStack.Any<string>();
			if (base.Button("Back"))
			{
				base.GoBack();
			}
			GUI.enabled = true;
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0008A0CB File Offset: 0x000882CB
		private void AddLogButton()
		{
			if (base.Button("Log"))
			{
				base.SwitchMenu(typeof(LogView));
			}
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0008A0EC File Offset: 0x000882EC
		private void AddDialogModeButtons()
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			foreach (object obj in Enum.GetValues(typeof(ShareDialogMode)))
			{
				this.AddDialogModeButton((ShareDialogMode)obj);
			}
			GUILayout.EndHorizontal();
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0008A160 File Offset: 0x00088360
		private void AddDialogModeButton(ShareDialogMode mode)
		{
			bool enabled = GUI.enabled;
			GUI.enabled = (enabled && mode != MenuBase.shareDialogMode);
			if (base.Button(mode.ToString()))
			{
				MenuBase.shareDialogMode = mode;
				FB.Mobile.ShareDialogMode = mode;
			}
			GUI.enabled = enabled;
		}

		// Token: 0x040011A1 RID: 4513
		private static ShareDialogMode shareDialogMode;
	}
}
