using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Facebook.Unity.Example
{
	// Token: 0x02000140 RID: 320
	internal class LogView : ConsoleBase
	{
		// Token: 0x06000B11 RID: 2833 RVA: 0x00089DC0 File Offset: 0x00087FC0
		public static void AddLog(string log)
		{
			LogView.events.Insert(0, string.Format("{0}\n{1}\n", DateTime.Now.ToString(LogView.datePatt), log));
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x00089DF8 File Offset: 0x00087FF8
		protected void OnGUI()
		{
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			if (base.Button("Back"))
			{
				base.GoBack();
			}
			base.ScrollPosition = GUILayout.BeginScrollView(base.ScrollPosition, new GUILayoutOption[]
			{
				GUILayout.MinWidth((float)ConsoleBase.MainWindowFullWidth)
			});
			GUILayout.TextArea(string.Join("\n", LogView.events.ToArray<string>()), base.TextStyle, new GUILayoutOption[]
			{
				GUILayout.ExpandHeight(true),
				GUILayout.MaxWidth((float)ConsoleBase.MainWindowWidth)
			});
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
		}

		// Token: 0x0400119F RID: 4511
		private static string datePatt = "M/d/yyyy hh:mm:ss tt";

		// Token: 0x040011A0 RID: 4512
		private static IList<string> events = new List<string>();
	}
}
