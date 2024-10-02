using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200007D RID: 125
public class Crosshit : MonoBehaviour
{
	// Token: 0x06000377 RID: 887 RVA: 0x0003C570 File Offset: 0x0003A770
	private void Awake()
	{
		this.texhit = (Resources.Load("GUI/crosshit") as Texture);
		this.SpecDamag = (Resources.Load("GUI/SpecDamage") as Texture);
		this.indicatorGas = (Resources.Load("GUI/gas") as Texture);
		this.indicatorFire = (Resources.Load("GUI/fire") as Texture);
	}

	// Token: 0x06000378 RID: 888 RVA: 0x0003C5D4 File Offset: 0x0003A7D4
	private void OnGUI()
	{
		if (this.HitTime > 0f && this.HitTime > Time.time)
		{
			Rect position = new Rect(0f, 0f, 32f, 32f);
			position.center = new Vector2((float)Screen.width, (float)Screen.height) / 2f;
			GUI.depth = -1;
			GUI.DrawTexture(position, this.texhit);
		}
		if (this.FireHitTime > 0f && this.FireHitTime > Time.time)
		{
			Rect position2 = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
			GUI.depth = -1;
			this.c = GUI.color;
			this.a = this.FireHitTime - Time.time;
			GUI.color = new Color(1f, 0f, 0f, this.a);
			GUI.DrawTexture(position2, this.SpecDamag);
			GUI.DrawTexture(new Rect(GUIManager.XRES(512f) + 76f, GUIManager.YRES(768f) - 40f, 32f, 32f), this.indicatorFire);
			GUI.color = this.c;
		}
		if (this.GazHitTime > 0f && this.GazHitTime > Time.time)
		{
			Rect position3 = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
			GUI.depth = -1;
			this.c = GUI.color;
			this.a = this.GazHitTime - Time.time;
			GUI.color = new Color(0f, 1f, 0f, this.a);
			GUI.DrawTexture(position3, this.SpecDamag);
			GUI.color = this.c;
			GUI.DrawTexture(new Rect(GUIManager.XRES(512f) + 76f, GUIManager.YRES(768f) - 40f, 32f, 32f), this.indicatorGas);
		}
		if (this.Splashes.Count > 0)
		{
			int num = 0;
			foreach (ScreenSplash screenSplash in this.Splashes)
			{
				if (screenSplash.a > 0f)
				{
					screenSplash.Draw();
				}
				else
				{
					this.SplashesToDelete.Add(num);
				}
				num++;
			}
			if (this.SplashesToDelete.Count > 0)
			{
				foreach (int index in this.SplashesToDelete)
				{
					this.Splashes.RemoveAt(index);
				}
				this.SplashesToDelete.Clear();
			}
		}
	}

	// Token: 0x06000379 RID: 889 RVA: 0x0003C8C0 File Offset: 0x0003AAC0
	public void Hit()
	{
		this.HitTime = Time.time + 0.25f;
	}

	// Token: 0x0600037A RID: 890 RVA: 0x0003C8D3 File Offset: 0x0003AAD3
	public void FireHit()
	{
		this.FireHitTime = Time.time + 1f;
	}

	// Token: 0x0600037B RID: 891 RVA: 0x0003C8E6 File Offset: 0x0003AAE6
	public void GazHit()
	{
		this.GazHitTime = Time.time + 1f;
	}

	// Token: 0x0600037C RID: 892 RVA: 0x0003C8F9 File Offset: 0x0003AAF9
	public void SnowHit()
	{
		this.Splashes.Add(new ScreenSplash());
	}

	// Token: 0x04000673 RID: 1651
	private List<ScreenSplash> Splashes = new List<ScreenSplash>();

	// Token: 0x04000674 RID: 1652
	private List<int> SplashesToDelete = new List<int>();

	// Token: 0x04000675 RID: 1653
	private Texture texhit;

	// Token: 0x04000676 RID: 1654
	private Texture SpecDamag;

	// Token: 0x04000677 RID: 1655
	private float HitTime;

	// Token: 0x04000678 RID: 1656
	private float FireHitTime;

	// Token: 0x04000679 RID: 1657
	private float GazHitTime;

	// Token: 0x0400067A RID: 1658
	private Texture indicatorGas;

	// Token: 0x0400067B RID: 1659
	private Texture indicatorFire;

	// Token: 0x0400067C RID: 1660
	private float a;

	// Token: 0x0400067D RID: 1661
	private Color c;
}
