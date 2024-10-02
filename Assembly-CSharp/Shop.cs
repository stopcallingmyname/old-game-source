using System;
using System.Collections;
using BestHTTP;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class Shop : MonoBehaviour
{
	// Token: 0x06000103 RID: 259 RVA: 0x00014D27 File Offset: 0x00012F27
	private void myGlobalInit()
	{
		this.Active = false;
		Shop.THIS = this;
		ItemsDrawer.THIS.GetInvSkin();
		this.message = "";
		this.type = 0;
		this.currItem = null;
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00014D5C File Offset: 0x00012F5C
	public void onActive()
	{
		Shop.THIS = this;
		if (ItemsDrawer.THIS == null)
		{
			return;
		}
		this.rand = Random.Range(0, 100);
		ItemsDrawer.THIS.GetInvSkin();
		this.message = "";
		this.type = 0;
		this.currItem = null;
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00014DB0 File Offset: 0x00012FB0
	private void OnGUI()
	{
		if (GameController.STATE != GAME_STATES.MAINMENU)
		{
			return;
		}
		if (GM.currGUIState != GUIGS.SHOP)
		{
			return;
		}
		GUI.Window(903, new Rect((float)Screen.width / 2f - 300f, 199f, 600f, GUIManager.YRES(768f) - 199f), new GUI.WindowFunction(this.DoWindow), "", GUIManager.gs_style1);
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00014E24 File Offset: 0x00013024
	private void DoWindow(int windowID)
	{
		if (!this.Active)
		{
			return;
		}
		GUI.DrawTexture(new Rect(0f, 0f, 600f, 32f), GUIManager.tex_panel);
		GUI.DrawTexture(new Rect(0f, 32f, 600f, GUIManager.YRES(768f) - 199f - 32f), GUIManager.tex_half_black);
		this.DrawMode(Lang.GetLabel(173), 7, 0, 0, 113);
		this.DrawMode(Lang.GetLabel(174), 125, 0, 4, 113);
		this.DrawMode(Lang.GetLabel(175), 243, 0, 1, 113);
		this.DrawMode(Lang.GetLabel(176), 361, 0, 2, 113);
		this.DrawMode(Lang.GetLabel(177), 479, 0, 3, 113);
		ItemsDrawer.THIS.DrawPlayer();
		if (this.currItem != null)
		{
			this.DrawActiveItem();
		}
		else if (this.rand > 50)
		{
			this.DrawPremuim();
		}
		else if (PlayerProfile.network == NETWORK.ST || PlayerProfile.network == NETWORK.KR)
		{
			this.DrawMegapack();
		}
		else
		{
			this.DrawPremuim();
		}
		if (this.message != "")
		{
			this.DrawMessage();
		}
		else
		{
			this.y_cat_ofs = 0;
		}
		if (this.type == 0)
		{
			this.DrawCategory0();
			return;
		}
		if (this.type == 1)
		{
			this.DrawCategory1();
			return;
		}
		if (this.type == 2)
		{
			this.DrawCategory2();
			return;
		}
		if (this.type == 3)
		{
			this.DrawCategory3();
			return;
		}
		if (this.type == 4)
		{
			this.DrawCategory4();
		}
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00014FC4 File Offset: 0x000131C4
	public void DrawPremuim()
	{
		GUI.DrawTexture(new Rect(190f, 46f, 400f, 80f), GUIManager.tex_premium_big);
		if (GUI.Button(new Rect(190f, 46f, 400f, 80f), "", GUIManager.gs_style1))
		{
			this.type = 3;
		}
	}

	// Token: 0x06000108 RID: 264 RVA: 0x00015028 File Offset: 0x00013228
	public void DrawMegapack()
	{
		GUI.DrawTexture(new Rect(190f, 46f, 400f, 80f), GUIManager.tex_megapack);
		if (GUI.Button(new Rect(190f, 46f, 400f, 80f), "", GUIManager.gs_style1))
		{
			this.type = 3;
		}
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00015089 File Offset: 0x00013289
	private void ResetPos()
	{
		this.x_pos = 0;
		this.y_pos = 0;
	}

	// Token: 0x0600010A RID: 266 RVA: 0x0001509C File Offset: 0x0001329C
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
			if (itemData != null && itemData.Category == (int)cat && itemData.ItemID != 35 && itemData.ItemID != 211 && itemData.ShowStatus <= 1 && itemData.Theme <= 1)
			{
				ItemsDrawer.THIS.DrawItem(itemData.ItemID, ITEMS_THEME.STANDART, new Rect((float)this.x_pos, (float)this.y_pos, 128f, 64f), false);
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

	// Token: 0x0600010B RID: 267 RVA: 0x000151E0 File Offset: 0x000133E0
	private void DrawTheme(ITEMS_THEME theme, ITEMS_TYPE t)
	{
		int num = 0;
		int num2 = this.y_pos;
		int num3 = this.x_pos;
		this.y_pos += 30;
		this.x_pos = 184;
		this.icount = 0;
		foreach (ItemData itemData in ItemsDB.Items)
		{
			if (itemData != null && itemData.Theme == (int)theme && itemData.Type == (int)t && itemData.ShowStatus <= 1 && itemData.ItemID != 35 && itemData.ItemID != 211)
			{
				ItemsDrawer.THIS.DrawItem(itemData.ItemID, theme, new Rect((float)this.x_pos, (float)this.y_pos, 128f, 64f), false);
				this.NextPos(false);
				num++;
			}
		}
		if (num > 0)
		{
			string label = Lang.GetLabel((int)(620 + theme));
			if (theme == ITEMS_THEME.HALLOWEEN)
			{
				GUI.DrawTexture(new Rect(181f, (float)num2, 400f, 26f), GUIManager.tex_prazd_HL);
			}
			else if (theme == ITEMS_THEME.LADY)
			{
				GUI.DrawTexture(new Rect(181f, (float)num2, 400f, 26f), GUIManager.tex_prazd_LADY);
			}
			else if (theme == ITEMS_THEME.NY)
			{
				GUI.DrawTexture(new Rect(181f, (float)num2, 400f, 26f), GUIManager.tex_prazd_NY);
			}
			else if (theme == ITEMS_THEME.WWII)
			{
				GUI.DrawTexture(new Rect(181f, (float)num2, 400f, 26f), GUIManager.tex_prazd_WWII);
			}
			else if (theme == ITEMS_THEME.ZOMBIE)
			{
				GUI.DrawTexture(new Rect(181f, (float)num2, 400f, 26f), GUIManager.tex_prazd_ZM);
			}
			else
			{
				GUI.DrawTexture(new Rect(181f, (float)num2, 400f, 26f), GUIManager.tex_category);
			}
			GUIManager.DrawText(new Rect(197f, (float)num2, 400f, 26f), label, 16, TextAnchor.MiddleLeft, 8);
			this.NextPos(true);
			return;
		}
		this.y_pos = num2;
		this.x_pos = num3;
	}

	// Token: 0x0600010C RID: 268 RVA: 0x000153E8 File Offset: 0x000135E8
	private void DrawMessage()
	{
		GUI.DrawTexture(new Rect(184f, 138f, 400f, 26f), GUIManager.tex_half_black);
		GUIManager.DrawText2(new Rect(184f, 138f, 400f, 26f), this.message, 14, TextAnchor.MiddleCenter, Color.yellow);
		this.y_cat_ofs = 30;
	}

	// Token: 0x0600010D RID: 269 RVA: 0x0001544C File Offset: 0x0001364C
	private void DrawActiveItem()
	{
		float x = Input.mousePosition.x;
		float num = (float)Screen.height - Input.mousePosition.y;
		float num2 = (float)Screen.width / 2f;
		num -= 160f;
		GUI.DrawTexture(new Rect(184f, 36f, 412f, 100f), GUIManager.tex_half_black);
		if (this.currItem.icon.width >= 128)
		{
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			GUI.DrawTexture(new Rect(464f, 40f, 128f, 64f), GUIManager.tex_weaponback);
			GUI.color = Color.white;
			GUI.DrawTexture(new Rect(464f, 40f, 128f, 64f), this.currItem.icon);
		}
		else
		{
			GUI.DrawTexture(new Rect(496f, 40f, 64f, 64f), this.currItem.icon);
		}
		Rect r = new Rect(188f, 36f, 64f, 32f);
		ITEM itemID = (ITEM)this.currItem.ItemID;
		GUIManager.DrawText2(r, itemID.ToString(), 14, TextAnchor.UpperLeft, Color.yellow);
		if (this.currItem.canUpgrade)
		{
			this.DrawItemData();
			GUIManager.DrawText2(new Rect(188f, 120f, 64f, 32f), this.currItem.StoreDesc, 10, TextAnchor.UpperLeft, Color.yellow);
		}
		else
		{
			GUIManager.DrawText2(new Rect(188f, 56f, 64f, 32f), this.currItem.StoreDesc, 14, TextAnchor.UpperLeft, Color.white);
		}
		if (PlayerProfile.level < this.currItem.Lvl)
		{
			GUIManager.DrawText(new Rect(394f, 84f, 256f, 64f), Lang.GetLabel(341) + this.currItem.Lvl.ToString(), 16, TextAnchor.MiddleCenter, 3);
			return;
		}
		if ((PlayerProfile.money < this.currItem.CostGold && this.currItem.Category != 29) || this.buytime > Time.time || !this.can_buy)
		{
			GUI.DrawTexture(new Rect(394f, 84f, 256f, 64f), GUIManager.tex_buy_blocked);
		}
		else
		{
			GUI.DrawTexture(new Rect(394f, 84f, 256f, 64f), GUIManager.tex_buy_active);
			if (GUI.Button(new Rect(452f, 100f, 140f, 32f), "", GUIManager.gs_empty))
			{
				MainMenu.MainAS.PlayOneShot(SoundManager.SoundClick, AudioListener.volume);
				if (this.currItem.Category == 29)
				{
					WEB_HANDLER.STEAM_BUY_ITEM((ulong)((long)this.currItem.ItemID), 1, new OnRequestFinishedDelegate(Gold.OnSteamBuyItem));
				}
				else
				{
					base.StartCoroutine(this.get_buy(this.currItem.ItemID));
				}
				this.buytime = Time.time + 2.5f;
			}
		}
		GUIManager.DrawText(new Rect(460f, 100f, 256f, 32f), Lang.GetLabel(180), 17, TextAnchor.MiddleLeft, 8);
		string text;
		if (this.currItem.Category == 29)
		{
			text = "$" + (this.currItem.CostSocial / 100f).ToString();
			GUIManager.DrawText(new Rect(462f, 100f, 100f, 32f), text, 17, TextAnchor.MiddleRight, 8);
			GUI.DrawTexture(new Rect(564f, 106f, 20f, 20f), GUIManager.tex_st);
			return;
		}
		text = this.currItem.CostGold.ToString();
		GUI.DrawTexture(new Rect(564f, 106f, 20f, 20f), GUIManager.tex_coin);
		GUIManager.DrawText(new Rect(462f, 100f, 100f, 32f), text, 17, TextAnchor.MiddleRight, 8);
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00015888 File Offset: 0x00013A88
	private IEnumerator get_buy(int itemid)
	{
		this.can_buy = false;
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"6&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&itemid=",
			itemid.ToString(),
			"&time=",
			DateTime.Now.Second
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			this.can_buy = true;
			string[] array = www.text.Split(new char[]
			{
				'|'
			});
			if (array[0] == "OK")
			{
				int num;
				int.TryParse(array[1], out num);
				PlayerProfile.money -= num;
				this.message = Lang.GetLabel(107);
				if (itemid == 15)
				{
					PlayerProfile.premium = 1;
				}
				base.StartCoroutine(Handler.reload_inv());
			}
			else if (array[0] == "DUBLICATE")
			{
				this.message = Lang.GetLabel(108);
			}
			else if (array[0] == "LOWLEVEL")
			{
				this.message = Lang.GetLabel(109);
			}
		}
		this.can_buy = true;
		yield break;
	}

	// Token: 0x0600010F RID: 271 RVA: 0x000158A0 File Offset: 0x00013AA0
	private void DrawBar(int x, int y, string text, float procent, int color)
	{
		if (procent == 0f)
		{
			procent = 0.1f;
		}
		GUIManager.DrawText(new Rect((float)x, (float)y, 200f, 12f), text, 16, TextAnchor.MiddleLeft, 8);
		x += 100;
		float num = 160f * procent;
		if (color == 0)
		{
			GUI.DrawTextureWithTexCoords(new Rect((float)x, (float)y, 160f, 12f), GUIManager.tex_bars, new Rect(0f, 0.6666666f, 1f, 0.16666667f));
			GUI.DrawTextureWithTexCoords(new Rect((float)x, (float)y, num, 12f), GUIManager.tex_bars, new Rect(0f, 0.8333333f, 1f * procent, 0.16666667f));
			if (num > 1f)
			{
				GUI.DrawTextureWithTexCoords(new Rect((float)x + num - 1f, (float)y, 2f, 12f), GUIManager.tex_bars, new Rect(0.9875f, 0.8333333f, 0.0125f, 0.16666667f));
				return;
			}
		}
		else
		{
			GUI.DrawTextureWithTexCoords(new Rect((float)x, (float)y, 160f, 12f), GUIManager.tex_bars, new Rect(0f, 0.3333333f, 1f, 0.16666667f));
			GUI.DrawTextureWithTexCoords(new Rect((float)x, (float)y, num, 12f), GUIManager.tex_bars, new Rect(0f, 0.5f, 1f * procent, 0.16666667f));
			if (num > 1f)
			{
				GUI.DrawTextureWithTexCoords(new Rect((float)x + num - 1f, (float)y, 2f, 12f), GUIManager.tex_bars, new Rect(0.9875f, 0.5f, 0.0125f, 0.16666667f));
			}
		}
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00015A58 File Offset: 0x00013C58
	private void DrawItemData()
	{
		int x = 188;
		int num = 22;
		if (this.currItem.Type == 1)
		{
			this.DrawBar(x, num + 32, Lang.GetLabel(110), (float)this.currItem.Upgrades[1][0].Val / (float)ItemsDB.Items[1000].Upgrades[1][0].Val, 1);
			this.DrawBar(x, num + 45, Lang.GetLabel(111), (float)this.currItem.Upgrades[4][0].Val / (float)ItemsDB.Items[1000].Upgrades[4][0].Val, 0);
			this.DrawBar(x, num + 58, Lang.GetLabel(112), (float)this.currItem.Upgrades[2][0].Val / (float)ItemsDB.Items[1000].Upgrades[2][0].Val, 0);
			this.DrawBar(x, num + 71, Lang.GetLabel(113), (float)this.currItem.Upgrades[3][0].Val / (float)ItemsDB.Items[1000].Upgrades[3][0].Val, 0);
			this.DrawBar(x, num + 84, Lang.GetLabel(114), (float)this.currItem.Upgrades[5][0].Val / (float)ItemsDB.Items[1000].Upgrades[5][0].Val, 0);
			return;
		}
		if (this.currItem.Type == 2)
		{
			this.DrawBar(x, num + 32, Lang.GetLabel(186), (float)this.currItem.Upgrades[1][0].Val / (float)ItemsDB.Items[1001].Upgrades[1][0].Val, 1);
			this.DrawBar(x, num + 45, Lang.GetLabel(187), (float)this.currItem.Upgrades[4][0].Val / (float)ItemsDB.Items[1001].Upgrades[4][0].Val, 0);
			this.DrawBar(x, num + 58, Lang.GetLabel(188), (float)this.currItem.Upgrades[2][0].Val / (float)ItemsDB.Items[1001].Upgrades[2][0].Val, 0);
			this.DrawBar(x, num + 71, Lang.GetLabel(189), (300f - (float)this.currItem.Upgrades[3][0].Val) / (float)ItemsDB.Items[1001].Upgrades[3][0].Val, 0);
			this.DrawBar(x, num + 84, Lang.GetLabel(190), (300f - (float)this.currItem.Upgrades[5][0].Val) / (float)ItemsDB.Items[1001].Upgrades[5][0].Val, 0);
		}
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00015D4C File Offset: 0x00013F4C
	private void DrawMode(string name, int x, int y, int id, int length)
	{
		float num = Input.mousePosition.x;
		float num2 = (float)Screen.height - Input.mousePosition.y;
		num -= (float)Screen.width / 2f - 300f;
		num2 -= 192f;
		Rect rect = new Rect((float)x, (float)y, (float)length, 32f);
		if (this.type != id)
		{
			if (rect.Contains(new Vector2(num, num2)))
			{
				if (!this.hovermode[id])
				{
					this.hovermode[id] = true;
					MainMenu.MainAS.PlayOneShot(SoundManager.SoundHover, AudioListener.volume);
				}
			}
			else if (this.hovermode[id])
			{
				this.hovermode[id] = false;
			}
		}
		if (this.type == id)
		{
			GUI.DrawTexture(new Rect((float)x, (float)y, (float)length, 32f), GUIManager.tex_select);
		}
		else if (this.hovermode[id])
		{
			GUI.DrawTexture(new Rect((float)x, (float)y, (float)length, 32f), GUIManager.tex_hover);
		}
		GUIManager.DrawText(rect, name, 17, TextAnchor.MiddleCenter, 8);
		if (GUI.Button(rect, "", GUIManager.gs_style1))
		{
			MainMenu.MainAS.PlayOneShot(SoundManager.SoundClick, AudioListener.volume);
			this.type = id;
		}
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00015E88 File Offset: 0x00014088
	private void DrawCategory0()
	{
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, (float)(140 + this.y_cat_ofs), 598f, GUIManager.YRES(768f) - 199f - 112f - 20f), this.scrollViewVector, new Rect(0f, 0f, 0f, this.sh + (float)this.y_cat_ofs));
		this.ResetPos();
		this.DrawCategory(ITEMS_CATEGORY.PISTOLS);
		this.DrawCategory(ITEMS_CATEGORY.PP);
		this.DrawCategory(ITEMS_CATEGORY.AUTOMATS);
		this.DrawCategory(ITEMS_CATEGORY.MACHINEGUNS);
		this.DrawCategory(ITEMS_CATEGORY.SNIPERS);
		this.DrawCategory(ITEMS_CATEGORY.SHOTGUNS);
		this.DrawCategory(ITEMS_CATEGORY.MELEE);
		this.DrawCategory(ITEMS_CATEGORY.REST);
		this.DrawTheme(ITEMS_THEME.LADY, ITEMS_TYPE.WEAPONS);
		this.DrawTheme(ITEMS_THEME.ZOMBIE, ITEMS_TYPE.WEAPONS);
		GUIManager.EndScrollView();
		this.sh = (float)(this.y_pos + 8);
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00015F64 File Offset: 0x00014164
	private void DrawCategory1()
	{
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, (float)(140 + this.y_cat_ofs), 598f, GUIManager.YRES(768f) - 199f - 112f - 20f), this.scrollViewVector, new Rect(0f, 0f, 0f, this.sh + (float)this.y_cat_ofs));
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

	// Token: 0x06000114 RID: 276 RVA: 0x00016028 File Offset: 0x00014228
	private void DrawCategory2()
	{
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, (float)(140 + this.y_cat_ofs), 598f, GUIManager.YRES(768f) - 199f - 112f - 20f), this.scrollViewVector, new Rect(0f, 0f, 0f, this.sh + (float)this.y_cat_ofs));
		this.ResetPos();
		this.DrawCategory(ITEMS_CATEGORY.PLAYER_SKINS);
		this.DrawCategory(ITEMS_CATEGORY.BADGES);
		this.DrawCategory(ITEMS_CATEGORY.VEHICLE_SKINS);
		GUIManager.EndScrollView();
		this.sh = (float)(this.y_pos + 8);
	}

	// Token: 0x06000115 RID: 277 RVA: 0x000160D4 File Offset: 0x000142D4
	private void DrawCategory3()
	{
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, (float)(140 + this.y_cat_ofs), 598f, GUIManager.YRES(768f) - 199f - 112f - 20f), this.scrollViewVector, new Rect(0f, 0f, 0f, this.sh + (float)this.y_cat_ofs));
		this.ResetPos();
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

	// Token: 0x06000116 RID: 278 RVA: 0x000161A0 File Offset: 0x000143A0
	private void DrawCategory4()
	{
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, (float)(140 + this.y_cat_ofs), 598f, GUIManager.YRES(768f) - 199f - 112f - 20f), this.scrollViewVector, new Rect(0f, 0f, 0f, this.sh + (float)this.y_cat_ofs));
		this.ResetPos();
		this.DrawCategory(ITEMS_CATEGORY.TANKS);
		this.DrawCategory(ITEMS_CATEGORY.CARS);
		this.DrawCategory(ITEMS_CATEGORY.VEHICLES_AMMO);
		GUIManager.EndScrollView();
		this.sh = (float)(this.y_pos + 8);
	}

	// Token: 0x06000117 RID: 279 RVA: 0x0001624C File Offset: 0x0001444C
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

	// Token: 0x06000118 RID: 280 RVA: 0x000162CC File Offset: 0x000144CC
	public void OnSelectItem(ItemData item)
	{
		this.message = "";
		if (item.Type == 1 || item.Type == 3 || item.ItemID == 211)
		{
			ItemsDrawer.THIS.EquipWeapon(item);
		}
		else if (item.ItemID == 1 || item.ItemID == 146)
		{
			ItemsDrawer.THIS.EquipHelmet();
		}
		else if (item.Type == 2)
		{
			ItemsDrawer.THIS.ShowVehicle(item);
		}
		this.currItem = item;
		if (!this.can_buy)
		{
			this.can_buy = ((PlayerProfile.money >= this.currItem.CostGold && this.currItem.Category != 29 && this.buytime < Time.time) || this.currItem.Category == 29);
		}
	}

	// Token: 0x04000124 RID: 292
	public static Shop THIS;

	// Token: 0x04000125 RID: 293
	public bool Active;

	// Token: 0x04000126 RID: 294
	private bool can_buy = true;

	// Token: 0x04000127 RID: 295
	private int type;

	// Token: 0x04000128 RID: 296
	private bool[] hovermode = new bool[6];

	// Token: 0x04000129 RID: 297
	public string message = "";

	// Token: 0x0400012A RID: 298
	private string otvet = "";

	// Token: 0x0400012B RID: 299
	private bool orderWaiting;

	// Token: 0x0400012C RID: 300
	private float orderWaitingTime;

	// Token: 0x0400012D RID: 301
	private ulong orderID;

	// Token: 0x0400012E RID: 302
	private float runCallback;

	// Token: 0x0400012F RID: 303
	public ItemData currItem;

	// Token: 0x04000130 RID: 304
	private int x_pos;

	// Token: 0x04000131 RID: 305
	private int y_pos;

	// Token: 0x04000132 RID: 306
	private int icount;

	// Token: 0x04000133 RID: 307
	private float sh;

	// Token: 0x04000134 RID: 308
	private int y_cat_ofs;

	// Token: 0x04000135 RID: 309
	private float buytime;

	// Token: 0x04000136 RID: 310
	private int rand;

	// Token: 0x04000137 RID: 311
	private Vector2 scrollViewVector;
}
