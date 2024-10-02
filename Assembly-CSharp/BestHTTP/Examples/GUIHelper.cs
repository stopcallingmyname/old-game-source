using System;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x02000191 RID: 401
	public static class GUIHelper
	{
		// Token: 0x06000E9E RID: 3742 RVA: 0x00096A88 File Offset: 0x00094C88
		private static void Setup()
		{
			if (GUIHelper.centerAlignedLabel == null)
			{
				GUIHelper.centerAlignedLabel = new GUIStyle(GUI.skin.label);
				GUIHelper.centerAlignedLabel.alignment = TextAnchor.MiddleCenter;
				GUIHelper.rightAlignedLabel = new GUIStyle(GUI.skin.label);
				GUIHelper.rightAlignedLabel.alignment = TextAnchor.MiddleRight;
			}
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x00096ADC File Offset: 0x00094CDC
		public static void DrawArea(Rect area, bool drawHeader, Action action)
		{
			GUIHelper.Setup();
			GUI.Box(area, string.Empty);
			GUILayout.BeginArea(area);
			if (drawHeader)
			{
				GUIHelper.DrawCenteredText(SampleSelector.SelectedSample.DisplayName);
				GUILayout.Space(5f);
			}
			if (action != null)
			{
				action();
			}
			GUILayout.EndArea();
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00096B29 File Offset: 0x00094D29
		public static void DrawCenteredText(string msg)
		{
			GUIHelper.Setup();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.Label(msg, GUIHelper.centerAlignedLabel, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x00096B59 File Offset: 0x00094D59
		public static void DrawRow(string key, string value)
		{
			GUIHelper.Setup();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label(key, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.Label(value, GUIHelper.rightAlignedLabel, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		// Token: 0x04001366 RID: 4966
		public static string BaseURL = "https://besthttpdemosite.azurewebsites.net";

		// Token: 0x04001367 RID: 4967
		private static GUIStyle centerAlignedLabel;

		// Token: 0x04001368 RID: 4968
		private static GUIStyle rightAlignedLabel;

		// Token: 0x04001369 RID: 4969
		public static Rect ClientArea;
	}
}
