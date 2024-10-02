using System;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class vp_SimpleCrosshair : MonoBehaviour
{
	// Token: 0x06000890 RID: 2192 RVA: 0x0007BAB0 File Offset: 0x00079CB0
	private void OnGUI()
	{
		if (this.m_ImageCrosshair != null)
		{
			GUI.color = new Color(1f, 1f, 1f, 0.8f);
			GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)this.m_ImageCrosshair.width * 0.5f, (float)Screen.height * 0.5f - (float)this.m_ImageCrosshair.height * 0.5f, (float)this.m_ImageCrosshair.width, (float)this.m_ImageCrosshair.height), this.m_ImageCrosshair);
			GUI.color = Color.white;
		}
	}

	// Token: 0x04000F14 RID: 3860
	public Texture m_ImageCrosshair;
}
