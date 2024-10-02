using System;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class Ammo : MonoBehaviour
{
	// Token: 0x06000364 RID: 868 RVA: 0x0003B368 File Offset: 0x00039568
	private void Awake()
	{
		this.ammo_block = ContentLoader.LoadTexture("ammo_block");
		this.ammo_shotgun = ContentLoader.LoadTexture("ammo_m3");
		this.ammo_machinegun = ContentLoader.LoadTexture("ammo_mp5");
		this.ammo_rifle = ContentLoader.LoadTexture("ammo_m14");
		this.ammo_m61 = ContentLoader.LoadTexture("ammo_m61");
		this.ammo_shmel = ContentLoader.LoadTexture("ammo_shmel");
		this.ammo_medkit = ContentLoader.LoadTexture("ammo_medkit");
		this.ammo_tnt = ContentLoader.LoadTexture("ammo_tnt");
		this.ammo_gp = ContentLoader.LoadTexture("ammo_gp");
		this.ammo_rpg = ContentLoader.LoadTexture("ammo_shmel");
		this.ammo_zbk18m = ContentLoader.LoadTexture("ammo_shmel");
		this.ammo_zof26 = ContentLoader.LoadTexture("ammo_shmel");
		this.ammo_snaryad = ContentLoader.LoadTexture("ammo_shmel");
		this.ammo_repair_kit = ContentLoader.LoadTexture("ammo_shmel");
		this.ammo_arrows = ContentLoader.LoadTexture("ammo_arrows");
		this.ammo_flamethrower = ContentLoader.LoadTexture("fire");
		this.ammo_gp_hud = ContentLoader.LoadTexture("ammo_gp_hud");
		this.ammo_javelin_hud = ContentLoader.LoadTexture("ammo_javelin_hud");
		this.ammo_minefly_hud = ContentLoader.LoadTexture("ammo_minefly_hud");
		this.ammo_rpg_hud = ContentLoader.LoadTexture("ammo_rpg_hud");
		this.ammo_shmel_hud = ContentLoader.LoadTexture("ammo_shmel_hud");
		this.ammo_snowball_hud = ContentLoader.LoadTexture("ammo_snowball_hud");
		this.gui_style = new GUIStyle();
		this.gui_style.font = FontManager.font[0];
		this.gui_style.alignment = TextAnchor.UpperRight;
		this.gui_style.normal.textColor = new Color(255f, 255f, 255f, 255f);
	}

	// Token: 0x06000365 RID: 869 RVA: 0x0003B528 File Offset: 0x00039728
	private void OnResize()
	{
		this.r_ammo_gun = new Rect((float)(Screen.width - 40), (float)(Screen.height - 40), 32f, 32f);
		this.r_ammo_m61 = new Rect((float)(Screen.width - 40), (float)(Screen.height - 100), 32f, 32f);
		this.r_ammo_shmel = new Rect((float)(Screen.width - 40), (float)(Screen.height - 140), 32f, 32f);
		this.r_ammo_tnt = new Rect((float)(Screen.width - 40), (float)(Screen.height - 180), 32f, 32f);
		this.r_ammo_gp = new Rect((float)(Screen.width - 120), (float)(Screen.height - 40), 32f, 32f);
		this.r_ammo_rpg = new Rect((float)(Screen.width - 40), (float)(Screen.height - 140), 32f, 32f);
		this.r_ammo_zbk18m = new Rect((float)(Screen.width - 40), (float)(Screen.height - 140), 32f, 32f);
		this.r_ammo_zof26 = new Rect((float)(Screen.width - 40), (float)(Screen.height - 140), 32f, 32f);
		this.r_ammo_snaryad = new Rect((float)(Screen.width - 40), (float)(Screen.height - 140), 32f, 32f);
		this.r_ammo_repair_kit = new Rect((float)(Screen.width - 40), (float)(Screen.height - 140), 32f, 32f);
		this.r_ammo_snowball = new Rect((float)(Screen.width - 40), (float)(Screen.height - 40), 32f, 32f);
	}

	// Token: 0x06000366 RID: 870 RVA: 0x0003B6F8 File Offset: 0x000398F8
	private void OnGUI()
	{
		if (!this.draw)
		{
			return;
		}
		if (!ItemsDB.CheckItem(this.weaponid) && this.weaponid != 0)
		{
			return;
		}
		this.OnResize();
		if (this.g1count > 0 || this.g2count > 0)
		{
			GUI.DrawTexture(this.r_ammo_m61, this.ammo_m61);
		}
		if (this.a1count > 0 || this.a3count > 0)
		{
			GUI.DrawTexture(this.r_ammo_shmel, this.ammo_shmel);
		}
		if (this.a2count > 0)
		{
			GUI.DrawTexture(this.r_ammo_tnt, this.ammo_tnt);
		}
		if (this.weaponid == 0)
		{
			GUI.DrawTexture(this.r_ammo_gun, this.ammo_block);
		}
		else if (ItemsDB.Items[this.weaponid].Category == 5)
		{
			GUI.DrawTexture(this.r_ammo_gun, this.ammo_rifle);
		}
		else if (ItemsDB.Items[this.weaponid].Category == 6)
		{
			GUI.DrawTexture(this.r_ammo_gun, this.ammo_shotgun);
		}
		else if (ItemsDB.Items[this.weaponid].Category == 19)
		{
			GUI.DrawTexture(this.r_ammo_gun, this.ammo_medkit);
		}
		else if (this.weaponid == 10 || this.weaponid == 185)
		{
			GUI.DrawTexture(this.r_ammo_gun, this.ammo_shmel_hud);
		}
		else if (this.weaponid == 62)
		{
			GUI.DrawTexture(this.r_ammo_gun, this.ammo_minefly_hud);
		}
		else if (this.weaponid == 77)
		{
			GUI.DrawTexture(this.r_ammo_gun, this.ammo_gp_hud);
		}
		else if (this.weaponid == 100)
		{
			GUI.DrawTexture(this.r_ammo_gun, this.ammo_rpg_hud);
		}
		else if (this.weaponid == 138)
		{
			GUI.DrawTexture(this.r_ammo_gun, this.ammo_javelin_hud);
		}
		else if (this.weaponid == 161)
		{
			GUI.DrawTexture(this.r_ammo_gun, this.ammo_arrows);
		}
		else if (this.weaponid == 315)
		{
			GUI.DrawTexture(this.r_ammo_gun, this.ammo_flamethrower);
		}
		else
		{
			if (ItemsDB.Items[this.weaponid].Type != 1)
			{
				return;
			}
			GUI.DrawTexture(this.r_ammo_gun, this.ammo_machinegun);
		}
		int num;
		if (this.weaponid == 10 || this.weaponid == 62 || this.weaponid == 185)
		{
			num = this.a1count;
		}
		else if (this.weaponid == 100 || this.weaponid == 138)
		{
			num = this.a3count;
		}
		else
		{
			num = this.clip;
		}
		this.gui_style.fontSize = 40;
		this.gui_style.normal.textColor = GUIManager.c[9];
		GUI.Label(new Rect((float)(Screen.width - 42 + 2), (float)(Screen.height - 34 + 2), 0f, 0f), num.ToString(), this.gui_style);
		this.gui_style.normal.textColor = GUIManager.c[8];
		GUI.Label(new Rect((float)(Screen.width - 42), (float)(Screen.height - 34), 0f, 0f), num.ToString(), this.gui_style);
		if (this.weaponid == 0 || this.weaponid == 38 || this.weaponid == 37 || this.weaponid == 36 || this.weaponid == 10 || this.weaponid == 62 || this.weaponid == 100 || this.weaponid == 138 || this.weaponid == 185 || this.weaponid == 315)
		{
			return;
		}
		this.gui_style.fontSize = 20;
		this.gui_style.normal.textColor = GUIManager.c[9];
		GUI.Label(new Rect((float)(Screen.width - 10 + 2), (float)(Screen.height - 50 + 2), 0f, 0f), this.backpack.ToString(), this.gui_style);
		this.gui_style.normal.textColor = GUIManager.c[8];
		GUI.Label(new Rect((float)(Screen.width - 10), (float)(Screen.height - 50), 0f, 0f), this.backpack.ToString(), this.gui_style);
		if (this.weaponid == 79 || this.weaponid == 80 || this.weaponid == 208)
		{
			GUI.DrawTexture(this.r_ammo_gp, this.ammo_gp);
			this.gui_style.fontSize = 40;
			this.gui_style.normal.textColor = GUIManager.c[9];
			GUI.Label(new Rect((float)(Screen.width - 112 + 2), (float)(Screen.height - 34 + 2), 0f, 0f), this.gp.ToString(), this.gui_style);
			this.gui_style.normal.textColor = GUIManager.c[8];
			GUI.Label(new Rect((float)(Screen.width - 112), (float)(Screen.height - 34), 0f, 0f), this.gp.ToString(), this.gui_style);
		}
	}

	// Token: 0x06000367 RID: 871 RVA: 0x0003BC48 File Offset: 0x00039E48
	public void SetWeapon(int _weaponid, int _clip, int _backpack)
	{
		this.weaponid = _weaponid;
		if (this.weaponid != 201 && this.weaponid != 202 && this.weaponid != 203 && this.weaponid != 204)
		{
			this.clip = _clip;
			this.backpack = _backpack;
			if (this.clip > 999 && this.weaponid != 315)
			{
				this.clip = 999;
			}
		}
	}

	// Token: 0x06000368 RID: 872 RVA: 0x0003BCC3 File Offset: 0x00039EC3
	public void SetPrimaryAmmo(int _ammo_clip)
	{
		this.clip = _ammo_clip;
	}

	// Token: 0x06000369 RID: 873 RVA: 0x0003BCCC File Offset: 0x00039ECC
	public void SetAmmo(int _g1, int _g2, int _a1, int _a2, int _a3, int _gp, int _zbk18m, int _zof26, int _snaryad)
	{
		this.g1count = _g1;
		this.g2count = _g2;
		this.a1count = _a1;
		this.a2count = _a2;
		this.a3count = _a3;
		this.gp = _gp;
		this.zbk18m = _zbk18m;
		this.zof26 = _zof26;
		this.snaryad = _snaryad;
	}

	// Token: 0x04000628 RID: 1576
	private Texture ammo_block;

	// Token: 0x04000629 RID: 1577
	private Texture ammo_shotgun;

	// Token: 0x0400062A RID: 1578
	private Texture ammo_machinegun;

	// Token: 0x0400062B RID: 1579
	private Texture ammo_rifle;

	// Token: 0x0400062C RID: 1580
	private Texture ammo_m61;

	// Token: 0x0400062D RID: 1581
	private Texture ammo_shmel;

	// Token: 0x0400062E RID: 1582
	private Texture ammo_medkit;

	// Token: 0x0400062F RID: 1583
	private Texture ammo_tnt;

	// Token: 0x04000630 RID: 1584
	private Texture ammo_gp;

	// Token: 0x04000631 RID: 1585
	private Texture ammo_rpg;

	// Token: 0x04000632 RID: 1586
	private Texture ammo_zbk18m;

	// Token: 0x04000633 RID: 1587
	private Texture ammo_zof26;

	// Token: 0x04000634 RID: 1588
	private Texture ammo_snaryad;

	// Token: 0x04000635 RID: 1589
	private Texture ammo_repair_kit;

	// Token: 0x04000636 RID: 1590
	private Texture ammo_arrows;

	// Token: 0x04000637 RID: 1591
	private Texture ammo_flamethrower;

	// Token: 0x04000638 RID: 1592
	private Texture ammo_gp_hud;

	// Token: 0x04000639 RID: 1593
	private Texture ammo_javelin_hud;

	// Token: 0x0400063A RID: 1594
	private Texture ammo_minefly_hud;

	// Token: 0x0400063B RID: 1595
	private Texture ammo_rpg_hud;

	// Token: 0x0400063C RID: 1596
	private Texture ammo_shmel_hud;

	// Token: 0x0400063D RID: 1597
	private Texture ammo_snowball_hud;

	// Token: 0x0400063E RID: 1598
	private Rect r_ammo_gun;

	// Token: 0x0400063F RID: 1599
	private Rect r_ammo_m61;

	// Token: 0x04000640 RID: 1600
	private Rect r_ammo_shmel;

	// Token: 0x04000641 RID: 1601
	private Rect r_ammo_tnt;

	// Token: 0x04000642 RID: 1602
	private Rect r_ammo_gp;

	// Token: 0x04000643 RID: 1603
	private Rect r_ammo_rpg;

	// Token: 0x04000644 RID: 1604
	private Rect r_ammo_zbk18m;

	// Token: 0x04000645 RID: 1605
	private Rect r_ammo_zof26;

	// Token: 0x04000646 RID: 1606
	private Rect r_ammo_snaryad;

	// Token: 0x04000647 RID: 1607
	private Rect r_ammo_repair_kit;

	// Token: 0x04000648 RID: 1608
	private Rect r_ammo_snowball;

	// Token: 0x04000649 RID: 1609
	private int weaponid;

	// Token: 0x0400064A RID: 1610
	private int clip;

	// Token: 0x0400064B RID: 1611
	private int backpack;

	// Token: 0x0400064C RID: 1612
	private int g1count;

	// Token: 0x0400064D RID: 1613
	private int g2count;

	// Token: 0x0400064E RID: 1614
	private int a1count;

	// Token: 0x0400064F RID: 1615
	private int a2count;

	// Token: 0x04000650 RID: 1616
	private int a3count;

	// Token: 0x04000651 RID: 1617
	private int gp;

	// Token: 0x04000652 RID: 1618
	private int zbk18m;

	// Token: 0x04000653 RID: 1619
	private int zof26;

	// Token: 0x04000654 RID: 1620
	private int snaryad;

	// Token: 0x04000655 RID: 1621
	private int repair_kit;

	// Token: 0x04000656 RID: 1622
	private GUIStyle gui_style;

	// Token: 0x04000657 RID: 1623
	public bool draw = true;

	// Token: 0x04000658 RID: 1624
	private bool initialized;
}
