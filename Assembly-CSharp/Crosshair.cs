using System;
using UnityEngine;

// Token: 0x0200007B RID: 123
public class Crosshair : MonoBehaviour
{
	// Token: 0x06000370 RID: 880 RVA: 0x0000248C File Offset: 0x0000068C
	private void Awake()
	{
	}

	// Token: 0x06000371 RID: 881 RVA: 0x0003C320 File Offset: 0x0003A520
	private void OnGUI()
	{
		if (!this.active || MainGUI.ForceCursor)
		{
			return;
		}
		float num = this.ShootTime - Time.time;
		if (num <= 0f)
		{
			num = 0f;
		}
		num *= 1.2f;
		Rect position = new Rect(0f, 0f, 32f + 32f * num, 32f + 32f * num);
		position.center = new Vector2((float)Screen.width, (float)Screen.height) / 2f;
		GUI.color = PlayerProfile.crossColor;
		GUI.DrawTexture(position, PlayerProfile.crossList[Config.cross]);
		if (Config.dot > 0)
		{
			GUI.DrawTexture(position, PlayerProfile.crossDot[Config.dot]);
		}
		GUI.color = Color.white;
	}

	// Token: 0x06000372 RID: 882 RVA: 0x0003C3F3 File Offset: 0x0003A5F3
	public void Shoot(float interval)
	{
		this.ShootTime = Time.time + interval;
	}

	// Token: 0x06000373 RID: 883 RVA: 0x0003C402 File Offset: 0x0003A602
	public void SetActive(bool val)
	{
		this.active = val;
	}

	// Token: 0x0400066A RID: 1642
	private float ShootTime;

	// Token: 0x0400066B RID: 1643
	public bool active = true;
}
