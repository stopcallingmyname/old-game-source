using System;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class Sky : MonoBehaviour
{
	// Token: 0x06000153 RID: 339 RVA: 0x00019794 File Offset: 0x00017994
	public void SetSky(int skyid, bool prazd = false)
	{
		if (prazd)
		{
			RenderSettings.skybox = this.skybox[2];
		}
		else
		{
			RenderSettings.skybox = this.skybox[skyid];
		}
		if (skyid == 1)
		{
			this.SetFog(0f, 12f, new Color(0.05f, 0.05f, 0.05f, 1f));
		}
		else
		{
			this.SetFog(40f, 80f, new Color(0.58f, 0.68f, 0.72f, 1f));
		}
		if (prazd)
		{
			this.SetFog(40f, 80f, new Color(0.05f, 0.05f, 0.05f, 1f));
		}
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00019844 File Offset: 0x00017A44
	public void SetFog(float start, float end, Color color)
	{
		RenderSettings.fogDensity = 0f;
		RenderSettings.fogMode = FogMode.Linear;
		RenderSettings.fogStartDistance = start;
		RenderSettings.fogEndDistance = end;
		RenderSettings.fogColor = color;
	}

	// Token: 0x04000166 RID: 358
	public Material[] skybox = new Material[3];
}
