using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class Inv : MonoBehaviour
{
	// Token: 0x0600006F RID: 111 RVA: 0x00007E8F File Offset: 0x0000608F
	private void myGlobalInit()
	{
		this.Active = false;
		Inv.needRefresh = true;
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00007EA0 File Offset: 0x000060A0
	private void OnGUI()
	{
		if (GameController.STATE != GAME_STATES.MAINMENU)
		{
			return;
		}
		if (GM.currGUIState != GUIGS.INVENTORY)
		{
			return;
		}
		GUI.Window(904, new Rect((float)Screen.width / 2f - 300f, 199f, 600f, GUIManager.YRES(768f) - 180f), new GUI.WindowFunction(this.DoWindow), "", GUIManager.gs_style1);
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00007F12 File Offset: 0x00006112
	private void ResetPos()
	{
		this.x_pos = 0;
		this.y_pos = 0;
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00007F24 File Offset: 0x00006124
	private void DrawCategory(ITEMS_CATEGORY cat)
	{
		int num = 0;
		int num2 = this.y_pos;
		int num3 = this.x_pos;
		this.y_pos += 30;
		this.x_pos = 184;
		this.icount = 0;
		foreach (ItemData itemData in ItemsDB.Items)
		{
			if (itemData != null && itemData.Category == (int)cat && itemData.LastCount != 0 && itemData.ItemID != 35 && (itemData.Theme != 5 || itemData.LastCount != 0))
			{
				ItemsDrawer.THIS.DrawItem(itemData.ItemID, ITEMS_THEME.STANDART, new Rect((float)this.x_pos, (float)this.y_pos, 128f, 64f), true);
				this.NextPos(false);
				num++;
			}
		}
		if (num > 0)
		{
			string label = Lang.GetLabel((int)(575 + cat));
			GUI.DrawTexture(new Rect(181f, (float)num2, 400f, 26f), GUIManager.tex_category);
			GUIManager.DrawText(new Rect(197f, (float)num2, 400f, 26f), label, 16, TextAnchor.MiddleLeft, 8);
			this.NextPos(true);
			return;
		}
		this.y_pos = num2;
		this.x_pos = num3;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x0000805C File Offset: 0x0000625C
	private void DoWindow(int windowID)
	{
		if (!this.Active)
		{
			return;
		}
		GUI.DrawTexture(new Rect(0f, 0f, 600f, 32f), GUIManager.tex_panel);
		GUI.DrawTexture(new Rect(0f, 32f, 600f, GUIManager.YRES(768f) - 180f), GUIManager.tex_half_black);
		this.DrawMode(Lang.GetLabel(173), 40, 0, ITEMS_TYPE.WEAPONS);
		this.DrawMode(Lang.GetLabel(175), 170, 0, ITEMS_TYPE.AMMUNITION);
		this.DrawMode(Lang.GetLabel(176), 300, 0, ITEMS_TYPE.CUSTOMIZATION);
		this.DrawMode(Lang.GetLabel(177), 430, 0, ITEMS_TYPE.OTHER);
		ItemsDrawer.THIS.DrawPlayer();
		this.y_cat_ofs = 36;
		if (this.type == ITEMS_TYPE.WEAPONS)
		{
			this.DrawCategory0();
			return;
		}
		if (this.type == ITEMS_TYPE.AMMUNITION)
		{
			this.DrawCategory1();
			return;
		}
		if (this.type == ITEMS_TYPE.CUSTOMIZATION)
		{
			this.DrawCategory2();
			return;
		}
		if (this.type == ITEMS_TYPE.OTHER)
		{
			this.DrawCategory3();
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00008170 File Offset: 0x00006370
	private void DrawCategory0()
	{
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, (float)this.y_cat_ofs, 598f, GUIManager.YRES(768f) - 199f), this.scrollViewVector, new Rect(0f, 0f, 0f, this.sh + (float)this.y_cat_ofs));
		this.ResetPos();
		this.DrawCategory(ITEMS_CATEGORY.PISTOLS);
		this.DrawCategory(ITEMS_CATEGORY.PP);
		this.DrawCategory(ITEMS_CATEGORY.AUTOMATS);
		this.DrawCategory(ITEMS_CATEGORY.MACHINEGUNS);
		this.DrawCategory(ITEMS_CATEGORY.SNIPERS);
		this.DrawCategory(ITEMS_CATEGORY.SHOTGUNS);
		this.DrawCategory(ITEMS_CATEGORY.MELEE);
		this.DrawCategory(ITEMS_CATEGORY.REST);
		GUIManager.EndScrollView();
		this.sh = (float)(this.y_pos + 8);
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00008228 File Offset: 0x00006428
	private void DrawCategory1()
	{
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, (float)this.y_cat_ofs, 598f, GUIManager.YRES(768f) - 199f), this.scrollViewVector, new Rect(0f, 0f, 0f, this.sh + (float)this.y_cat_ofs));
		this.ResetPos();
		this.DrawCategory(ITEMS_CATEGORY.ARMOR);
		this.DrawCategory(ITEMS_CATEGORY.MEDS);
		this.DrawCategory(ITEMS_CATEGORY.GRENS);
		this.DrawCategory(ITEMS_CATEGORY.LAUNCHERS);
		this.DrawCategory(ITEMS_CATEGORY.EXPLOSIVES);
		this.DrawCategory(ITEMS_CATEGORY.BARRICADES);
		GUIManager.EndScrollView();
		this.sh = (float)(this.y_pos + 8);
	}

	// Token: 0x06000076 RID: 118 RVA: 0x000082D8 File Offset: 0x000064D8
	private void DrawCategory2()
	{
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, (float)this.y_cat_ofs, 598f, GUIManager.YRES(768f) - 199f), this.scrollViewVector, new Rect(0f, 0f, 0f, this.sh + (float)this.y_cat_ofs));
		this.ResetPos();
		this.DrawCategory(ITEMS_CATEGORY.PLAYER_SKINS);
		this.DrawCategory(ITEMS_CATEGORY.BADGES);
		this.DrawCategory(ITEMS_CATEGORY.CAPS);
		this.DrawCategory(ITEMS_CATEGORY.VEHICLE_SKINS);
		GUIManager.EndScrollView();
		this.sh = (float)(this.y_pos + 8);
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00008378 File Offset: 0x00006578
	private void DrawCategory3()
	{
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, (float)this.y_cat_ofs, 598f, GUIManager.YRES(768f) - 199f), this.scrollViewVector, new Rect(0f, 0f, 0f, this.sh + (float)this.y_cat_ofs));
		this.ResetPos();
		this.DrawCategory(ITEMS_CATEGORY.GIFTS);
		this.DrawCategory(ITEMS_CATEGORY.TANKS);
		this.DrawCategory(ITEMS_CATEGORY.CARS);
		this.DrawCategory(ITEMS_CATEGORY.PREMIUM);
		this.DrawCategory(ITEMS_CATEGORY.MODES);
		this.DrawCategory(ITEMS_CATEGORY.MAPS);
		this.DrawCategory(ITEMS_CATEGORY.CLAN);
		if (PlayerProfile.network == NETWORK.ST || PlayerProfile.network == NETWORK.KR)
		{
			this.DrawCategory(ITEMS_CATEGORY.MEGAPACKS);
		}
		GUIManager.EndScrollView();
		this.sh = (float)(this.y_pos + 8);
	}

	// Token: 0x06000078 RID: 120 RVA: 0x0000844C File Offset: 0x0000664C
	private void NextPos(bool end = false)
	{
		if (end)
		{
			this.x_pos = 184;
			if (this.icount > 0)
			{
				this.y_pos += 68;
			}
			return;
		}
		this.x_pos += 132;
		this.icount++;
		if (this.icount == 3)
		{
			this.icount = 0;
			this.x_pos = 184;
			this.y_pos += 68;
		}
	}

	// Token: 0x06000079 RID: 121 RVA: 0x000084CA File Offset: 0x000066CA
	private void Update()
	{
		if (Inv.needRefresh)
		{
			base.StartCoroutine(this.get_inv());
			Inv.needRefresh = false;
		}
	}

	// Token: 0x0600007A RID: 122 RVA: 0x000084E8 File Offset: 0x000066E8
	public void onActive()
	{
		Inv.needRefresh = true;
		ItemsDrawer.THIS.GetInvSkin();
		this.type = ITEMS_TYPE.WEAPONS;
		Shop.THIS.currItem = null;
		if (PlayerProfile.tykva > 0)
		{
			ItemsDrawer.THIS.EquipHeadAttach(211, true, false);
		}
		else
		{
			ItemsDrawer.THIS.EquipHeadAttach(211, false, true);
		}
		if (PlayerProfile.kolpak > 0)
		{
			ItemsDrawer.THIS.EquipHeadAttach(222, true, false);
		}
		else
		{
			ItemsDrawer.THIS.EquipHeadAttach(222, false, true);
		}
		if (PlayerProfile.roga > 0)
		{
			ItemsDrawer.THIS.EquipHeadAttach(223, true, false);
		}
		else
		{
			ItemsDrawer.THIS.EquipHeadAttach(223, false, true);
		}
		if (PlayerProfile.mask_bear > 0)
		{
			ItemsDrawer.THIS.EquipHeadAttach(224, true, false);
		}
		else
		{
			ItemsDrawer.THIS.EquipHeadAttach(224, false, true);
		}
		if (PlayerProfile.mask_fox > 0)
		{
			ItemsDrawer.THIS.EquipHeadAttach(225, true, false);
		}
		else
		{
			ItemsDrawer.THIS.EquipHeadAttach(225, false, true);
		}
		if (PlayerProfile.mask_rabbit > 0)
		{
			ItemsDrawer.THIS.EquipHeadAttach(226, true, false);
			return;
		}
		ItemsDrawer.THIS.EquipHeadAttach(226, false, true);
	}

	// Token: 0x0600007B RID: 123 RVA: 0x0000861E File Offset: 0x0000681E
	private IEnumerator get_inv()
	{
		yield return base.StartCoroutine(Handler.reload_inv());
		string[] array = PlayerProfile.myInventory.Split(new char[]
		{
			'^'
		});
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array2 = array[i].Split(new char[]
				{
					'|'
				});
				int num2;
				int.TryParse(array2[0], out num2);
				int dateEnd;
				int.TryParse(array2[1], out dateEnd);
				int lastCount;
				int.TryParse(array2[2], out lastCount);
				if (ItemsDB.CheckItem(num2))
				{
					if (ItemsDB.Items[num2].Type == 1 || ItemsDB.Items[num2].Type == 2 || ItemsDB.Items[num2].Type == 3 || (ItemsDB.Items[num2].Category == 18 && num2 != 211) || ItemsDB.Items[num2].Category == 25)
					{
						ItemsDB.Items[num2].LastCount = 1;
					}
					else
					{
						ItemsDB.Items[num2].LastCount = lastCount;
					}
					ItemsDB.Items[num2].DateEnd = dateEnd;
				}
			}
			i++;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600007C RID: 124 RVA: 0x0000862D File Offset: 0x0000682D
	private IEnumerator get_gift()
	{
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"23&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&time=",
			DateTime.Now.Second
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null && !(www.text == "NO") && www.text == "OK")
		{
			PlayerProfile.money += 25;
			Inv.needRefresh = true;
		}
		yield break;
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00008638 File Offset: 0x00006838
	private void DrawMode(string name, int x, int y, ITEMS_TYPE id)
	{
		float num = Input.mousePosition.x;
		float num2 = (float)Screen.height - Input.mousePosition.y;
		num -= (float)Screen.width / 2f - 300f;
		num2 -= 192f;
		Rect rect = new Rect((float)x, (float)y, 128f, 32f);
		if (this.type != id)
		{
			if (rect.Contains(new Vector2(num, num2)))
			{
				if (!this.hovermode[(int)id])
				{
					this.hovermode[(int)id] = true;
					MainMenu.MainAS.PlayOneShot(SoundManager.SoundHover, AudioListener.volume);
				}
			}
			else if (this.hovermode[(int)id])
			{
				this.hovermode[(int)id] = false;
			}
		}
		if (this.type == id)
		{
			GUI.DrawTexture(new Rect((float)x, (float)y, 128f, 32f), GUIManager.tex_select);
		}
		else if (this.hovermode[(int)id])
		{
			GUI.DrawTexture(new Rect((float)x, (float)y, 128f, 32f), GUIManager.tex_hover);
		}
		GUIManager.DrawText(rect, name, 17, TextAnchor.MiddleCenter, 8);
		if (GUI.Button(rect, "", GUIManager.gs_style1))
		{
			MainMenu.MainAS.PlayOneShot(SoundManager.SoundClick, AudioListener.volume);
			this.type = id;
		}
	}

	// Token: 0x04000065 RID: 101
	public bool Active;

	// Token: 0x04000066 RID: 102
	public static bool needRefresh;

	// Token: 0x04000067 RID: 103
	private float lastupdate = -5f;

	// Token: 0x04000068 RID: 104
	private bool giftlock;

	// Token: 0x04000069 RID: 105
	private int x_pos;

	// Token: 0x0400006A RID: 106
	private int y_pos;

	// Token: 0x0400006B RID: 107
	private int icount;

	// Token: 0x0400006C RID: 108
	private float sh;

	// Token: 0x0400006D RID: 109
	private int y_cat_ofs;

	// Token: 0x0400006E RID: 110
	private ITEMS_TYPE type = ITEMS_TYPE.WEAPONS;

	// Token: 0x0400006F RID: 111
	private bool[] hovermode = new bool[6];

	// Token: 0x04000070 RID: 112
	public Texture rt;

	// Token: 0x04000071 RID: 113
	private Vector2 scrollViewVector = Vector2.zero;
}
