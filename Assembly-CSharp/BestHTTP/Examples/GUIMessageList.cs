using System;
using System.Collections.Generic;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x02000192 RID: 402
	public class GUIMessageList
	{
		// Token: 0x06000EA3 RID: 3747 RVA: 0x00096BA0 File Offset: 0x00094DA0
		public void Draw()
		{
			this.Draw((float)Screen.width, 0f);
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x00096BB4 File Offset: 0x00094DB4
		public void Draw(float minWidth, float minHeight)
		{
			this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, new GUILayoutOption[]
			{
				GUILayout.MinHeight(minHeight)
			});
			for (int i = 0; i < this.messages.Count; i++)
			{
				GUILayout.Label(this.messages[i], new GUILayoutOption[]
				{
					GUILayout.MinWidth(minWidth)
				});
			}
			GUILayout.EndScrollView();
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x00096C1E File Offset: 0x00094E1E
		public void Add(string msg)
		{
			this.messages.Add(msg);
			this.scrollPos = new Vector2(this.scrollPos.x, float.MaxValue);
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00096C47 File Offset: 0x00094E47
		public void Clear()
		{
			this.messages.Clear();
		}

		// Token: 0x0400136A RID: 4970
		private List<string> messages = new List<string>();

		// Token: 0x0400136B RID: 4971
		private Vector2 scrollPos;
	}
}
