using System;
using UnityEngine;

// Token: 0x02000086 RID: 134
public class Overlay : MonoBehaviour
{
	// Token: 0x060003C5 RID: 965 RVA: 0x00049D89 File Offset: 0x00047F89
	private void Awake()
	{
		this.tex_heart = (Resources.Load("GUI/heart") as Texture2D);
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x00049DA0 File Offset: 0x00047FA0
	public void SetActive(bool val)
	{
		this.active = val;
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x00049DAC File Offset: 0x00047FAC
	private void OnGUI()
	{
		if (!this.active)
		{
			return;
		}
		GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.tex_heart);
		GUI.color = this.color1;
		GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), GUIManager.tex_red);
		GUI.color = this.color2;
	}

	// Token: 0x04000839 RID: 2105
	private bool active;

	// Token: 0x0400083A RID: 2106
	private Texture2D tex_heart;

	// Token: 0x0400083B RID: 2107
	private Color color1 = new Color(1f, 1f, 1f, 0.25f);

	// Token: 0x0400083C RID: 2108
	private Color color2 = new Color(1f, 1f, 1f, 1f);
}
