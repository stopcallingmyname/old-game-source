using System;
using UnityEngine;

// Token: 0x0200007C RID: 124
public class ScreenSplash
{
	// Token: 0x06000375 RID: 885 RVA: 0x0003C41C File Offset: 0x0003A61C
	public ScreenSplash()
	{
		this.splash = Resources.Load<Texture2D>("GUI/snowball_screen");
		float num = (float)Random.Range(Screen.height / 2, Screen.height);
		this.drawRect = new Rect(Random.Range(-1f * num / 2f, (float)Screen.width - num / 2f), Random.Range(-1f * num / 2f, (float)Screen.height - num / 2f), num, num);
		this.pivot = this.drawRect.center;
		this.rotateAngle = (float)Random.Range(0, 360);
		this.splashTime = Time.time + this.splashTimer;
	}

	// Token: 0x06000376 RID: 886 RVA: 0x0003C4EC File Offset: 0x0003A6EC
	public void Draw()
	{
		int depth = GUI.depth;
		Color color = GUI.color;
		Matrix4x4 matrix = GUI.matrix;
		GUI.depth = -1;
		this.a = this.splashTime - Time.time;
		GUI.color = new Color(1f, 1f, 1f, this.a);
		GUIUtility.RotateAroundPivot(this.rotateAngle, this.pivot);
		GUI.DrawTexture(this.drawRect, this.splash);
		GUI.matrix = matrix;
		GUI.color = color;
		GUI.depth = depth;
	}

	// Token: 0x0400066C RID: 1644
	private Texture2D splash;

	// Token: 0x0400066D RID: 1645
	private Rect drawRect;

	// Token: 0x0400066E RID: 1646
	private Vector2 pivot;

	// Token: 0x0400066F RID: 1647
	private float rotateAngle;

	// Token: 0x04000670 RID: 1648
	public float a = 1f;

	// Token: 0x04000671 RID: 1649
	private float splashTime;

	// Token: 0x04000672 RID: 1650
	private float splashTimer = 5f;
}
