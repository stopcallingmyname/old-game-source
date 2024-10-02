using System;
using UnityEngine;

// Token: 0x02000080 RID: 128
public class Health : MonoBehaviour
{
	// Token: 0x06000396 RID: 918 RVA: 0x0004410C File Offset: 0x0004230C
	private void Awake()
	{
		if (!this.tex)
		{
			this.tex = (Resources.Load("GUI/health") as Texture);
		}
		this.texHelmet = (Resources.Load("GUI/helmet") as Texture);
		this.texArmor = (Resources.Load("GUI/armor") as Texture);
		this.gasMask = (Resources.Load("GUI/gasmask2") as Texture);
		this.gui_style = new GUIStyle();
		this.gui_style.font = FontManager.font[0];
		this.gui_style.fontSize = 52;
		GameObject.Find("Map");
		if (!this.tex_indicator)
		{
			this.tex_indicator = (Resources.Load("GUI/damage") as Texture);
		}
	}

	// Token: 0x06000397 RID: 919 RVA: 0x000441D4 File Offset: 0x000423D4
	private void OnGUI()
	{
		if (!this.init)
		{
			this.init = true;
		}
		GUI.depth = -1;
		if (this.tc == null)
		{
			this.tc = (TankController)Object.FindObjectOfType(typeof(TankController));
		}
		if (this.tc != null)
		{
			this.activeTC = this.tc.enabled;
		}
		if (this.cc == null)
		{
			this.cc = (CarController)Object.FindObjectOfType(typeof(CarController));
		}
		if (this.cc != null)
		{
			this.activeCC = this.cc.enabled;
		}
		if (!this.activeTC && !this.activeCC)
		{
			this.gui_style.normal.textColor = GUIManager.c[9];
			GUI.Label(new Rect(GUIManager.XRES(512f) + 2f, GUIManager.YRES(768f) - 39f + 2f, 0f, 0f), this.health.ToString(), this.gui_style);
			if (this.health < 30)
			{
				this.gui_style.normal.textColor = GUIManager.c[1];
			}
			else if (this.health <= 60)
			{
				this.gui_style.normal.textColor = GUIManager.c[3];
			}
			else
			{
				this.gui_style.normal.textColor = GUIManager.c[8];
			}
			GUI.Label(new Rect(GUIManager.XRES(512f), GUIManager.YRES(768f) - 39f, 0f, 0f), this.health.ToString(), this.gui_style);
			GUI.DrawTexture(new Rect(GUIManager.XRES(512f) - 40f, GUIManager.YRES(768f) - 40f, 32f, 32f), this.tex);
			if (this.helmet)
			{
				GUI.DrawTexture(new Rect(GUIManager.XRES(512f) - 112f, GUIManager.YRES(768f) - 40f, 32f, 32f), this.texHelmet);
			}
			if (this.armor)
			{
				GUI.DrawTexture(new Rect(GUIManager.XRES(512f) - 76f, GUIManager.YRES(768f) - 40f, 32f, 32f), this.texArmor);
			}
			if (this.mask)
			{
				GUI.DrawTexture(new Rect(GUIManager.XRES(512f) - 148f, GUIManager.YRES(768f) - 40f, 32f, 32f), this.gasMask);
			}
		}
		if (this.IndicatorTime < Time.time)
		{
			return;
		}
		float angle = Health.AngleSigned(Camera.main.transform.forward, this.DamagePos - this.goPlayer.transform.position, this.goPlayer.transform.up);
		Matrix4x4 matrix = GUI.matrix;
		GUIUtility.RotateAroundPivot(angle, new Vector2((float)Screen.width, (float)Screen.height) / 2f);
		GUI.DrawTexture(new Rect(0f, 0f, GUIManager.XRES(1024f), GUIManager.YRES(768f)), this.tex_indicator);
		GUI.matrix = matrix;
	}

	// Token: 0x06000398 RID: 920 RVA: 0x00044550 File Offset: 0x00042750
	public void SetHealth(int _health)
	{
		this.health = _health;
	}

	// Token: 0x06000399 RID: 921 RVA: 0x00044559 File Offset: 0x00042759
	public int GetHealth()
	{
		return this.health;
	}

	// Token: 0x0600039A RID: 922 RVA: 0x00044561 File Offset: 0x00042761
	public void SetHelmet(bool val)
	{
		this.helmet = val;
	}

	// Token: 0x0600039B RID: 923 RVA: 0x0004456A File Offset: 0x0004276A
	public void SetArmor(bool val)
	{
		this.armor = val;
	}

	// Token: 0x0600039C RID: 924 RVA: 0x00044573 File Offset: 0x00042773
	public void SetMask(bool val)
	{
		this.mask = val;
	}

	// Token: 0x0600039D RID: 925 RVA: 0x0004457C File Offset: 0x0004277C
	public void SetDamageIndicator(int attackerid)
	{
		if (attackerid >= 32)
		{
			return;
		}
		this.IndicatorTime = Time.time + 1f;
		this.aid = attackerid;
		this.DamagePos = RemotePlayersUpdater.Instance.Bots[this.aid].position;
		if (!this.goPlayer)
		{
			this.goPlayer = GameObject.Find("Player");
		}
	}

	// Token: 0x0600039E RID: 926 RVA: 0x000445E0 File Offset: 0x000427E0
	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
	{
		return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * 57.29578f;
	}

	// Token: 0x0400079D RID: 1949
	private bool init;

	// Token: 0x0400079E RID: 1950
	private GUIStyle gui_style;

	// Token: 0x0400079F RID: 1951
	private Texture tex;

	// Token: 0x040007A0 RID: 1952
	private Texture texHelmet;

	// Token: 0x040007A1 RID: 1953
	private Texture texArmor;

	// Token: 0x040007A2 RID: 1954
	private Texture gasMask;

	// Token: 0x040007A3 RID: 1955
	private int health = 100;

	// Token: 0x040007A4 RID: 1956
	private bool helmet;

	// Token: 0x040007A5 RID: 1957
	private bool armor;

	// Token: 0x040007A6 RID: 1958
	private bool mask;

	// Token: 0x040007A7 RID: 1959
	private float IndicatorTime;

	// Token: 0x040007A8 RID: 1960
	private Texture tex_indicator;

	// Token: 0x040007A9 RID: 1961
	private int aid;

	// Token: 0x040007AA RID: 1962
	private Vector3 DamagePos = Vector3.zero;

	// Token: 0x040007AB RID: 1963
	private GameObject goPlayer;

	// Token: 0x040007AC RID: 1964
	private TankController tc;

	// Token: 0x040007AD RID: 1965
	private bool activeTC;

	// Token: 0x040007AE RID: 1966
	private CarController cc;

	// Token: 0x040007AF RID: 1967
	private bool activeCC;
}
