using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using Facebook.Unity;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class Gold : MonoBehaviour
{
	// Token: 0x0600005D RID: 93 RVA: 0x000047B4 File Offset: 0x000029B4
	private void myGlobalInit()
	{
		this.Active = false;
		Gold.WaitingForResponce = false;
		Gold.WaitingTimer = 0f;
		this.history_loaded = false;
		if ((PlayerProfile.network == NETWORK.KR || PlayerProfile.network == NETWORK.ST) && DateTime.UtcNow >= CONST.EXT.TIME_START && DateTime.UtcNow < CONST.EXT.TIME_END)
		{
			this.type = 0;
			this.ActionItems = new List<ActionItem>();
			this.ActionItems.Add(new ActionItem(-1, 1050));
		}
		Gold.Purchases.Clear();
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00004844 File Offset: 0x00002A44
	private void OnGUI()
	{
		if (GameController.STATE != GAME_STATES.MAINMENU)
		{
			return;
		}
		if (GM.currGUIState != GUIGS.GOLD)
		{
			return;
		}
		GUI.Window(903, new Rect((float)Screen.width / 2f - 300f, 199f, 600f, GUIManager.YRES(768f) - 180f), new GUI.WindowFunction(this.DoWindow), "", GUIManager.gs_style3);
	}

	// Token: 0x0600005F RID: 95 RVA: 0x000048B7 File Offset: 0x00002AB7
	private void Update()
	{
		if (Gold.WaitingForResponce && Gold.WaitingTimer < Time.time)
		{
			Gold.WaitingForResponce = false;
			Gold.WaitingTimer = 0f;
		}
	}

	// Token: 0x06000060 RID: 96 RVA: 0x000048DC File Offset: 0x00002ADC
	private void DoWindow(int windowID)
	{
		if (!this.Active)
		{
			return;
		}
		GUI.DrawTexture(new Rect(0f, 0f, 600f, 32f), GUIManager.tex_panel);
		GUI.DrawTexture(new Rect(0f, 32f, 600f, GUIManager.YRES(768f) - 180f), GUIManager.tex_half_black);
		if ((PlayerProfile.network == NETWORK.KR || PlayerProfile.network == NETWORK.ST) && DateTime.UtcNow >= CONST.EXT.TIME_START && DateTime.UtcNow < CONST.EXT.TIME_END)
		{
			this.DrawMode(Lang.GetLabel(855), 108, 0, 0);
		}
		this.DrawMode(Lang.GetLabel(856), 236, 0, 1);
		this.DrawMode(Lang.GetLabel(857), 364, 0, 2);
		if (this.type == 0)
		{
			this.DrawActions();
			return;
		}
		if (this.type == 1)
		{
			this.DrawCoins();
			return;
		}
		if (this.type == 2)
		{
			this.DrawHistory();
		}
	}

	// Token: 0x06000061 RID: 97 RVA: 0x000049E8 File Offset: 0x00002BE8
	private void DrawBuyButton(int itemID, int offY)
	{
		GUIManager.gs_style3.fontSize = 44;
		GUIManager.gs_style3.alignment = TextAnchor.MiddleCenter;
		Rect position = new Rect(30f, (float)(offY + 45), 200f, 110f);
		Rect position2 = new Rect(31f, (float)(offY + 46), 200f, 110f);
		GUI.DrawTexture(position, GUIManager.tex_button_red);
		GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
		GUI.Label(position2, Lang.GetLabel(180), GUIManager.gs_style3);
		GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
		GUI.Label(position, Lang.GetLabel(180), GUIManager.gs_style3);
		if (GUI.Button(position2, "", GUIManager.gs_style3))
		{
			if (Gold.WaitingForResponce)
			{
				return;
			}
			Gold.WaitingTimer = Time.time + 2.5f;
			Gold.WaitingForResponce = true;
			if (PlayerProfile.network == NETWORK.ST)
			{
				WEB_HANDLER.STEAM_BUY_ITEM((ulong)((long)itemID), 1, new OnRequestFinishedDelegate(Gold.OnSteamBuyItem));
			}
			NETWORK network = PlayerProfile.network;
		}
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00004B1C File Offset: 0x00002D1C
	private void DrawActions()
	{
		TextAnchor alignment = GUIManager.gs_style3.alignment;
		Color textColor = GUIManager.gs_style3.normal.textColor;
		int fontSize = GUIManager.gs_style3.fontSize;
		int num = 0;
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, 32f, 600f, GUIManager.YRES(768f) - 230f), this.scrollViewVector, new Rect(0f, 0f, 0f, (float)this.sh));
		if (DateTime.UtcNow >= CONST.EXT.TIME_START && DateTime.UtcNow < CONST.EXT.TIME_END)
		{
			GUI.DrawTexture(new Rect(0f, (float)num, 600f, 360f), GUIManager.tex_action_banner);
			num += 180;
			foreach (ActionItem actionItem in this.ActionItems)
			{
				if (actionItem.ItemID <= 0 || (ItemsDB.CheckItem(actionItem.ItemID) && ItemsDB.Items[actionItem.ItemID].LastCount <= 0))
				{
					if (actionItem.ActionText != null)
					{
						GUI.DrawTexture(new Rect(0f, (float)num, 600f, 90f), actionItem.ActionText);
					}
					this.DrawBuyButton(actionItem.ActionID, num);
					num += 180;
				}
			}
			num += 10;
			GUIManager.gs_style3.fontSize = 16;
			GUIManager.gs_style3.alignment = TextAnchor.UpperCenter;
			Rect position = new Rect(0f, (float)num, 600f, 20f);
			Rect position2 = new Rect(1f, (float)(num + 1), 600f, 20f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position2, Lang.GetLabel(514) + CONST.EXT.TIME_END.ToString(), GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 0f, 0f, 1f);
			GUI.Label(position, Lang.GetLabel(514) + CONST.EXT.TIME_END.ToString(), GUIManager.gs_style3);
			num += 40;
		}
		GUIManager.EndScrollView();
		this.sh = num;
		GUIManager.gs_style3.alignment = alignment;
		GUIManager.gs_style3.normal.textColor = textColor;
		GUIManager.gs_style3.fontSize = fontSize;
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00004DC0 File Offset: 0x00002FC0
	private void DrawCoins()
	{
		TextAnchor alignment = GUIManager.gs_style3.alignment;
		Color textColor = GUIManager.gs_style3.normal.textColor;
		int fontSize = GUIManager.gs_style3.fontSize;
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, 32f, 600f, GUIManager.YRES(768f) - 180f), this.scrollViewVector, new Rect(0f, 0f, 0f, 368f));
		int num = PlayerProfile.moneypay / 30 + 1;
		if (num > 100)
		{
			num = 100;
		}
		GUIManager.gs_style3.fontSize = 22;
		GUIManager.gs_style3.alignment = TextAnchor.UpperLeft;
		int num2 = 8;
		if (PlayerProfile.network == NETWORK.VK)
		{
			if (DateTime.UtcNow >= CONST.EXT.TIME_START && DateTime.UtcNow < CONST.EXT.TIME_END)
			{
				this.DrawLine(1f, 6, 0, 8, num, 305, 0, PlayerProfile.network);
				this.DrawLine(5f, 30, 0, 40, num, 300, 1, PlayerProfile.network);
				this.DrawLine(10f, 60, 0, 80, num, 301, 2, PlayerProfile.network);
				this.DrawLine(30f, 180, 10, 240, num, 302, 3, PlayerProfile.network);
				this.DrawLine(50f, 300, 20, 400, num, 303, 4, PlayerProfile.network);
				this.DrawLine(100f, 600, 60, 800, num, 304, 5, PlayerProfile.network);
				this.DrawLine(500f, 3000, 500, 4000, num, 306, 6, PlayerProfile.network);
				this.DrawLine(1000f, 6000, 3000, 8000, num, 307, 7, PlayerProfile.network);
			}
			else
			{
				this.DrawLine(1f, 0, 0, 6, num, 205, 0, PlayerProfile.network);
				this.DrawLine(5f, 0, 0, 30, num, 200, 1, PlayerProfile.network);
				this.DrawLine(10f, 0, 0, 60, num, 201, 2, PlayerProfile.network);
				this.DrawLine(30f, 0, 10, 180, num, 202, 3, PlayerProfile.network);
				this.DrawLine(50f, 0, 20, 300, num, 203, 4, PlayerProfile.network);
				this.DrawLine(100f, 0, 60, 600, num, 204, 5, PlayerProfile.network);
				this.DrawLine(500f, 0, 500, 3000, num, 206, 6, PlayerProfile.network);
				this.DrawLine(1000f, 0, 3000, 6000, num, 207, 7, PlayerProfile.network);
			}
		}
		else if (PlayerProfile.network == NETWORK.OK || PlayerProfile.network == NETWORK.MM)
		{
			if (DateTime.UtcNow >= CONST.EXT.TIME_START && DateTime.UtcNow < CONST.EXT.TIME_END)
			{
				this.DrawLine(1f, 1, 0, 2, num, 11, 1, PlayerProfile.network);
				this.DrawLine(5f, 5, 1, 10, num, 12, 2, PlayerProfile.network);
				this.DrawLine(10f, 10, 3, 20, num, 13, 3, PlayerProfile.network);
				this.DrawLine(50f, 50, 10, 100, num, 14, 4, PlayerProfile.network);
				this.DrawLine(100f, 100, 25, 200, num, 15, 5, PlayerProfile.network);
				this.DrawLine(250f, 250, 80, 500, num, 16, 6, PlayerProfile.network);
				this.DrawLine(500f, 500, 250, 1000, num, 17, 7, PlayerProfile.network);
				this.DrawLine(1000f, 1000, 1000, 2000, num, 18, 8, PlayerProfile.network);
			}
			else
			{
				this.DrawLine(1f, 0, 0, 1, num, 1, 1, PlayerProfile.network);
				this.DrawLine(5f, 0, 1, 5, num, 2, 2, PlayerProfile.network);
				this.DrawLine(10f, 0, 3, 10, num, 3, 3, PlayerProfile.network);
				this.DrawLine(50f, 0, 10, 50, num, 4, 4, PlayerProfile.network);
				this.DrawLine(100f, 0, 25, 100, num, 5, 5, PlayerProfile.network);
				this.DrawLine(250f, 0, 80, 250, num, 6, 6, PlayerProfile.network);
				this.DrawLine(500f, 0, 250, 500, num, 7, 7, PlayerProfile.network);
				this.DrawLine(1000f, 0, 1000, 1000, num, 8, 8, PlayerProfile.network);
			}
			num2 = 10;
		}
		else if (PlayerProfile.network == NETWORK.ST || PlayerProfile.network == NETWORK.KR || PlayerProfile.network == NETWORK.FB)
		{
			if (DateTime.UtcNow >= CONST.EXT.TIME_START && DateTime.UtcNow < CONST.EXT.TIME_END)
			{
				if (PlayerProfile.network == NETWORK.ST)
				{
					this.DrawLine(0.1f, 6, 0, 12, num, 11, 1, PlayerProfile.network);
					this.DrawLine(0.5f, 30, 0, 60, num, 12, 2, PlayerProfile.network);
				}
				this.DrawLine(1f, 60, 20, 120, num, 13, 3, PlayerProfile.network);
				this.DrawLine(3f, 180, 60, 360, num, 14, 4, PlayerProfile.network);
				this.DrawLine(5f, 300, 160, 600, num, 15, 5, PlayerProfile.network);
				this.DrawLine(10f, 600, 400, 1200, num, 16, 6, PlayerProfile.network);
				this.DrawLine(50f, 3000, 4000, 6000, num, 17, 7, PlayerProfile.network);
				this.DrawLine(100f, 6000, 12000, 12000, num, 18, 8, PlayerProfile.network);
			}
			else
			{
				if (PlayerProfile.network == NETWORK.ST)
				{
					this.DrawLine(0.1f, 0, 0, 6, num, 1, 1, PlayerProfile.network);
					this.DrawLine(0.5f, 0, 0, 30, num, 2, 2, PlayerProfile.network);
				}
				this.DrawLine(1f, 0, 0, 60, num, 3, 3, PlayerProfile.network);
				this.DrawLine(3f, 0, 10, 180, num, 4, 4, PlayerProfile.network);
				this.DrawLine(5f, 0, 20, 300, num, 5, 5, PlayerProfile.network);
				this.DrawLine(10f, 0, 60, 600, num, 6, 6, PlayerProfile.network);
				this.DrawLine(50f, 0, 500, 3000, num, 7, 7, PlayerProfile.network);
				this.DrawLine(100f, 0, 3000, 6000, num, 8, 8, PlayerProfile.network);
			}
			num2 = 10;
		}
		else if (PlayerProfile.network == NETWORK.KG)
		{
			if (DateTime.UtcNow >= CONST.EXT.TIME_START && DateTime.UtcNow < CONST.EXT.TIME_END)
			{
				this.DrawLine(1f, 6, 0, 12, num, 11, 1, PlayerProfile.network);
				this.DrawLine(5f, 30, 0, 60, num, 12, 2, PlayerProfile.network);
				this.DrawLine(10f, 60, 20, 120, num, 13, 3, PlayerProfile.network);
				this.DrawLine(30f, 180, 60, 360, num, 14, 4, PlayerProfile.network);
				this.DrawLine(50f, 300, 160, 600, num, 15, 5, PlayerProfile.network);
				this.DrawLine(100f, 600, 400, 1200, num, 16, 6, PlayerProfile.network);
				this.DrawLine(500f, 3000, 4000, 6000, num, 17, 7, PlayerProfile.network);
			}
			else
			{
				this.DrawLine(1f, 0, 0, 6, num, 1, 1, PlayerProfile.network);
				this.DrawLine(5f, 0, 0, 30, num, 2, 2, PlayerProfile.network);
				this.DrawLine(10f, 0, 0, 60, num, 3, 3, PlayerProfile.network);
				this.DrawLine(30f, 0, 10, 180, num, 4, 4, PlayerProfile.network);
				this.DrawLine(50f, 0, 20, 300, num, 5, 5, PlayerProfile.network);
				this.DrawLine(100f, 0, 60, 600, num, 6, 6, PlayerProfile.network);
				this.DrawLine(500f, 0, 500, 3000, num, 7, 7, PlayerProfile.network);
			}
			num2 = 10;
		}
		if (DateTime.UtcNow >= CONST.EXT.TIME_START && DateTime.UtcNow < CONST.EXT.TIME_END)
		{
			GUIManager.gs_style3.fontSize = 16;
			GUIManager.gs_style3.alignment = TextAnchor.UpperCenter;
			Rect position = new Rect(0f, (float)(40 * (num2 - 1)), 600f, 32f);
			Rect position2 = new Rect(1f, (float)(40 * (num2 - 1) + 1), 600f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position2, Lang.GetLabel(514) + CONST.EXT.TIME_END.ToString(), GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 0f, 0f, 1f);
			GUI.Label(position, Lang.GetLabel(514) + CONST.EXT.TIME_END.ToString(), GUIManager.gs_style3);
		}
		GUIManager.gs_style3.fontSize = 16;
		int num3 = 0;
		GUIManager.gs_style3.normal.textColor = new Color(0f, 1f, 0f, 1f);
		GUIManager.gs_style3.alignment = TextAnchor.UpperCenter;
		GUI.Label(new Rect(0f, (float)(40 * num2 + num3), 600f, 32f), Lang.GetLabel(23), GUIManager.gs_style3);
		GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
		GUIManager.EndScrollView();
		GUIManager.gs_style3.alignment = alignment;
		GUIManager.gs_style3.normal.textColor = textColor;
		GUIManager.gs_style3.fontSize = fontSize;
	}

	// Token: 0x06000064 RID: 100 RVA: 0x0000585C File Offset: 0x00003A5C
	private void DrawHistory()
	{
		TextAnchor alignment = GUIManager.gs_style3.alignment;
		Color textColor = GUIManager.gs_style3.normal.textColor;
		int fontSize = GUIManager.gs_style3.fontSize;
		int num = 20;
		int num2 = 40;
		GUIManager.Toggle();
		GUIManager.DrawText(new Rect(170f, (float)num2, 600f, 30f), Lang.GetLabel(870), 16, TextAnchor.MiddleLeft, 8);
		this._show_canceled = GUI.Toggle(new Rect(146f, (float)(num2 + 4), 16f, 16f), this._show_canceled, "");
		if (this.show_canceled != this._show_canceled)
		{
			this.show_canceled = this._show_canceled;
			this.history_loaded = false;
			Gold.Purchases.Clear();
			WEB_HANDLER.GET_PURCHASE_HISTORY(this.show_canceled, new OnRequestFinishedDelegate(this.OnPurchaseHistory));
		}
		num2 += 32;
		if (Gold.Purchases.Count == 0 && !this.history_loaded)
		{
			GUI.DrawTexture(new Rect(108f, (float)(num2 - 4), 384f, 32f), GUIManager.tex_warning);
			GUIManager.DrawText(new Rect(140f, (float)(num2 - 4), 352f, 32f), Lang.GetLabel(868), 16, TextAnchor.MiddleCenter, 8);
			return;
		}
		if (Gold.Purchases.Count == 0 && this.history_loaded)
		{
			GUI.DrawTexture(new Rect(108f, (float)(num2 - 4), 384f, 32f), GUIManager.tex_warning);
			GUIManager.DrawText(new Rect(140f, (float)(num2 - 4), 352f, 32f), Lang.GetLabel(871), 16, TextAnchor.MiddleCenter, 8);
			return;
		}
		new Rect(0f, (float)(num2 - 4), 490f, 32f);
		GUIManager.gs_style3.alignment = TextAnchor.UpperLeft;
		GUIManager.gs_style3.fontSize = 20;
		Rect position = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
		Rect position2 = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
		GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
		GUI.Label(position2, Lang.GetLabel(864), GUIManager.gs_style3);
		GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
		GUI.Label(position, Lang.GetLabel(864), GUIManager.gs_style3);
		num += 170;
		Rect position3 = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
		Rect position4 = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
		GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
		GUI.Label(position4, Lang.GetLabel(865), GUIManager.gs_style3);
		GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
		GUI.Label(position3, Lang.GetLabel(865), GUIManager.gs_style3);
		num += 36;
		num += 130;
		Rect position5 = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
		Rect position6 = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
		GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
		GUI.Label(position6, Lang.GetLabel(866), GUIManager.gs_style3);
		GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
		GUI.Label(position5, Lang.GetLabel(866), GUIManager.gs_style3);
		num += 130;
		Rect position7 = new Rect((float)num, (float)(num2 + 5), 128f, 32f);
		Rect position8 = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 128f, 32f);
		GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
		GUI.Label(position8, Lang.GetLabel(867), GUIManager.gs_style3);
		GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
		GUI.Label(position7, Lang.GetLabel(867), GUIManager.gs_style3);
		GUIManager.gs_style3.fontSize = fontSize;
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, 96f, 600f, GUIManager.YRES(472f)), this.scrollViewVector, new Rect(0f, 0f, 0f, (float)(8 + 40 * Gold.Purchases.Count)));
		int i = 0;
		int count = Gold.Purchases.Count;
		while (i < count)
		{
			this.DrawHistoryLine(i);
			i++;
		}
		GUIManager.EndScrollView();
		GUIManager.gs_style3.alignment = alignment;
		GUIManager.gs_style3.normal.textColor = textColor;
		GUIManager.gs_style3.fontSize = fontSize;
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00005DA4 File Offset: 0x00003FA4
	private void DrawMode(string name, int x, int y, int id)
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
			GUI.DrawTexture(new Rect((float)x, (float)y, 128f, 32f), GUIManager.tex_select);
		}
		else if (this.hovermode[id])
		{
			GUI.DrawTexture(new Rect((float)x, (float)y, 128f, 32f), GUIManager.tex_hover);
		}
		GUIManager.DrawText(rect, name, 20, TextAnchor.MiddleCenter, 8);
		if (GUI.Button(rect, "", GUIManager.gs_empty))
		{
			MainMenu.MainAS.PlayOneShot(SoundManager.SoundClick, AudioListener.volume);
			this.type = id;
			if (id == 2)
			{
				this.history_loaded = false;
				Gold.Purchases.Clear();
				WEB_HANDLER.GET_PURCHASE_HISTORY(this.show_canceled, new OnRequestFinishedDelegate(this.OnPurchaseHistory));
			}
		}
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00005F14 File Offset: 0x00004114
	private void DrawLine(float golos, int money, int bonus, int newmoney, int discount, int itemid, int id, NETWORK network)
	{
		float num = Input.mousePosition.x;
		float num2 = (float)Screen.height - Input.mousePosition.y;
		num -= (float)Screen.width / 2f - 300f;
		num2 -= 192f - this.scrollViewVector.y + 39f;
		int num3 = 54;
		int num4 = 40 * id + 8;
		Rect position = new Rect(0f, (float)(num4 - 4), 490f, 32f);
		if (position.Contains(new Vector2(num, num2)))
		{
			if (!this.hover[id])
			{
				this.hover[id] = true;
				MainMenu.MainAS.PlayOneShot(SoundManager.SoundHover, AudioListener.volume);
			}
		}
		else if (this.hover[id])
		{
			this.hover[id] = false;
		}
		if (this.hover[id])
		{
			GUI.DrawTexture(new Rect(0f, (float)(num4 - 4), 600f, 32f), GUIManager.tex_half_yellow);
			GUI.DrawTexture(new Rect(0f, (float)(num4 + 32 - 4), 600f, 1f), GUIManager.tex_half_black);
		}
		GUIManager.gs_style3.alignment = TextAnchor.UpperRight;
		Rect position2 = new Rect((float)num3, (float)(num4 + 5), 45f, 32f);
		Rect position3 = new Rect((float)(num3 + 1), (float)(num4 + 1 + 5), 45f, 32f);
		GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
		GUI.Label(position3, golos.ToString(), GUIManager.gs_style3);
		GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
		GUI.Label(position2, golos.ToString(), GUIManager.gs_style3);
		GUIManager.gs_style3.alignment = TextAnchor.UpperLeft;
		string text = "";
		if (network == NETWORK.VK)
		{
			text = "голосов";
			if (id == 0)
			{
				text = "голос";
			}
		}
		else if (network == NETWORK.OK)
		{
			text = "ок";
		}
		else if (network == NETWORK.MM)
		{
			text = "мэйликов";
			if (id == 0)
			{
				text = "мэйлика";
			}
		}
		else if (network == NETWORK.FB)
		{
			text = "USD";
		}
		else if (network == NETWORK.ST || network == NETWORK.KR)
		{
			text = "USD";
		}
		else if (network == NETWORK.KG)
		{
			text = "KREDS";
		}
		Rect position4 = new Rect((float)(num3 + 48), (float)(num4 + 8), 256f, 32f);
		Rect position5 = new Rect((float)(num3 + 48 + 1), (float)(num4 + 1 + 8), 256f, 32f);
		GUIManager.gs_style3.fontSize = 12;
		GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
		GUI.Label(position5, text, GUIManager.gs_style3);
		GUIManager.gs_style3.normal.textColor = new Color(0.35f, 0.75f, 1f, 1f);
		GUI.Label(position4, text, GUIManager.gs_style3);
		GUIManager.gs_style3.fontSize = 22;
		GUIManager.gs_style3.alignment = TextAnchor.UpperCenter;
		num3 += 100;
		GUI.DrawTexture(new Rect((float)num3, (float)(num4 - 4), 32f, 32f), GUIManager.tex_arrow);
		if (money > 0)
		{
			num3 += 30;
			Rect position6 = new Rect((float)num3, (float)(num4 + 5), 55f, 32f);
			Rect position7 = new Rect((float)(num3 + 1), (float)(num4 + 1 + 5), 55f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position7, money.ToString(), GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(0.5f, 0.5f, 0.5f, 1f);
			GUI.Label(position6, money.ToString(), GUIManager.gs_style3);
			GUI.DrawTexture(new Rect((float)(num3 - 4), (float)(num4 + 2), 64f, 16f), GUIManager.tex_crossline);
		}
		GUIManager.gs_style3.alignment = TextAnchor.UpperRight;
		num3 += 60;
		Rect position8 = new Rect((float)num3, (float)(num4 + 5), 52f, 32f);
		Rect position9 = new Rect((float)(num3 + 1), (float)(num4 + 1 + 5), 52f, 32f);
		GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
		GUI.Label(position9, newmoney.ToString(), GUIManager.gs_style3);
		GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
		GUI.Label(position8, newmoney.ToString(), GUIManager.gs_style3);
		num3 += 55;
		GUI.DrawTexture(new Rect((float)num3, (float)(num4 - 4), 32f, 32f), GUIManager.tex_coin);
		if (bonus > 0)
		{
			num3 += 34;
			if (network == NETWORK.OK)
			{
				num3 += 20;
			}
			Rect position10 = new Rect((float)num3, (float)(num4 + 5), 65f, 32f);
			Rect position11 = new Rect((float)(num3 + 1), (float)(num4 + 1 + 5), 65f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position11, "+" + bonus.ToString(), GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(position10, "+" + bonus.ToString(), GUIManager.gs_style3);
			num3 += 68;
			GUI.DrawTexture(new Rect((float)num3, (float)(num4 - 4), 32f, 32f), GUIManager.tex_bonus);
			num3 += 35;
		}
		else
		{
			num3 += 137;
		}
		if (discount > 0)
		{
			int num5 = newmoney / 100 * discount;
			if (num5 > 0)
			{
				Rect position12 = new Rect((float)num3, (float)(num4 + 5), 55f, 32f);
				Rect position13 = new Rect((float)(num3 + 1), (float)(num4 + 1 + 5), 55f, 32f);
				GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
				GUI.Label(position13, "+" + num5.ToString(), GUIManager.gs_style3);
				GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
				GUI.Label(position12, "+" + num5.ToString(), GUIManager.gs_style3);
				num3 += 42;
				GUI.DrawTexture(new Rect((float)num3, (float)(num4 - 4), 64f, 32f), GUIManager.tex_discount);
			}
		}
		if (GUI.Button(position, "", GUIManager.gs_style3))
		{
			if (Gold.WaitingForResponce)
			{
				return;
			}
			Gold.WaitingTimer = Time.time + 2.5f;
			Gold.WaitingForResponce = true;
			if (network == NETWORK.FB)
			{
				FB.Canvas.Pay("https://blockade3d.com/fb_coins/item" + itemid.ToString() + ".html", "purchaseitem", 1, null, null, null, null, null, delegate(IPayResult response)
				{
					base.StartCoroutine(this.ReInit());
				});
				return;
			}
			if (network == NETWORK.ST)
			{
				WEB_HANDLER.STEAM_BUY_ITEM((ulong)((long)itemid), 1, new OnRequestFinishedDelegate(Gold.OnSteamBuyItem));
				return;
			}
			if (network != NETWORK.KR)
			{
				PlayerProfile.get_player_stats = false;
				Application.ExternalCall("order", new object[]
				{
					"item" + itemid.ToString()
				});
			}
		}
	}

	// Token: 0x06000067 RID: 103 RVA: 0x0000669C File Offset: 0x0000489C
	private void DrawHistoryLine(int _pos)
	{
		DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds((long)Gold.Purchases[_pos].PTime).DateTime.ToLocalTime();
		int num = 20;
		int num2 = 40 * _pos + 8;
		new Rect(0f, (float)(num2 - 4), 490f, 32f);
		if (Gold.Purchases[_pos].PStatus == 1 || Gold.Purchases[_pos].PStatus == 6)
		{
			GUI.DrawTexture(new Rect(0f, (float)(num2 - 4), 600f, 32f), GUIManager.tex_half_green);
		}
		else if (Gold.Purchases[_pos].PStatus == 0)
		{
			GUI.DrawTexture(new Rect(0f, (float)(num2 - 4), 600f, 32f), GUIManager.tex_half_red);
		}
		else
		{
			GUI.DrawTexture(new Rect(0f, (float)(num2 - 4), 600f, 32f), GUIManager.tex_half_yellow);
		}
		GUIManager.gs_style3.alignment = TextAnchor.UpperLeft;
		Rect rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
		Rect position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
		GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
		GUI.Label(position, dateTime.ToString(), GUIManager.gs_style3);
		GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
		GUI.Label(rect, dateTime.ToString(), GUIManager.gs_style3);
		num += 170;
		if (Gold.Purchases[_pos].PItem == 187)
		{
			rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
			position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, Lang.GetLabel(860), GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, Lang.GetLabel(860), GUIManager.gs_style3);
			num += 36;
		}
		else if (Gold.Purchases[_pos].PItem == 1002)
		{
			rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
			position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, "DPM [ACTION]", GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, "DPM [ACTION]", GUIManager.gs_style3);
			num += 36;
		}
		else if (Gold.Purchases[_pos].PItem == 1032)
		{
			rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
			position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, "KAR98K [ACTION]", GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, "KAR98K [ACTION]", GUIManager.gs_style3);
			num += 36;
		}
		else if (Gold.Purchases[_pos].PItem == 1001)
		{
			rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
			position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, "M1A1 [ACTION]", GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, "M1A1 [ACTION]", GUIManager.gs_style3);
			num += 36;
		}
		else if (Gold.Purchases[_pos].PItem == 1003)
		{
			rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
			position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, "M1924 [ACTION]", GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, "M1924 [ACTION]", GUIManager.gs_style3);
			num += 36;
		}
		else if (Gold.Purchases[_pos].PItem == 1004)
		{
			rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
			position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, "MAUSER [ACTION]", GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, "MAUSER [ACTION]", GUIManager.gs_style3);
			num += 36;
		}
		else if (Gold.Purchases[_pos].PItem == 1006)
		{
			rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
			position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, "MG42 [ACTION]", GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, "MG42 [ACTION]", GUIManager.gs_style3);
			num += 36;
		}
		else if (Gold.Purchases[_pos].PItem == 1005)
		{
			rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
			position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, "MOSIN [ACTION]", GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, "MOSIN [ACTION]", GUIManager.gs_style3);
			num += 36;
		}
		else if (Gold.Purchases[_pos].PItem == 1008)
		{
			rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
			position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, "MP40 [ACTION]", GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, "MP40 [ACTION]", GUIManager.gs_style3);
			num += 36;
		}
		else if (Gold.Purchases[_pos].PItem == 1007)
		{
			rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
			position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, "PPSH [ACTION]", GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, "PPSH [ACTION]", GUIManager.gs_style3);
			num += 36;
		}
		else if (Gold.Purchases[_pos].PItem == 1010)
		{
			rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
			position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, "STEN MK2 [ACTION]", GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, "STEN MK2 [ACTION]", GUIManager.gs_style3);
			num += 36;
		}
		else if (Gold.Purchases[_pos].PItem == 1009)
		{
			rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
			position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, "TYPE99 [ACTION]", GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, "TYPE99 [ACTION]", GUIManager.gs_style3);
			num += 36;
		}
		else if (Gold.Purchases[_pos].PItem == 1033)
		{
			rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
			position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, "MG13 [ACTION]", GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, "MG13 [ACTION]", GUIManager.gs_style3);
			num += 36;
		}
		else if (Gold.Purchases[_pos].PItem == 1034)
		{
			rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
			position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, "RPD [ACTION]", GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, "RPD [ACTION]", GUIManager.gs_style3);
			num += 36;
		}
		else if (Gold.Purchases[_pos].PItem == 1035)
		{
			rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
			position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, "TT [ACTION]", GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, "TT [ACTION]", GUIManager.gs_style3);
			num += 36;
		}
		else
		{
			GUI.DrawTexture(new Rect((float)num, (float)(num2 - 4), 32f, 32f), GUIManager.tex_coin);
			num += 36;
			rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
			position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, Gold.Purchases[_pos].PMoney.ToString(), GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, Gold.Purchases[_pos].PMoney.ToString(), GUIManager.gs_style3);
		}
		string str = "";
		if (PlayerProfile.network == NETWORK.VK)
		{
			str = " голосов";
			if (Gold.Purchases[_pos].PCost == 1)
			{
				str = " голос";
			}
		}
		else if (PlayerProfile.network == NETWORK.OK)
		{
			str = " OK";
		}
		else if (PlayerProfile.network == NETWORK.MM)
		{
			str = " мэйликов";
			if (Gold.Purchases[_pos].PCost == 1)
			{
				str = " мэйлика";
			}
		}
		else if (PlayerProfile.network == NETWORK.FB || PlayerProfile.network == NETWORK.ST || PlayerProfile.network == NETWORK.KR)
		{
			str = " USD";
		}
		else if (PlayerProfile.network == NETWORK.KG)
		{
			str = " KREDS";
		}
		num += 130;
		rect = new Rect((float)num, (float)(num2 + 5), 45f, 32f);
		position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 45f, 32f);
		float num3 = (float)Gold.Purchases[_pos].PCost;
		if (PlayerProfile.network == NETWORK.ST || PlayerProfile.network == NETWORK.FB || PlayerProfile.network == NETWORK.KR)
		{
			num3 /= 100f;
		}
		GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
		GUI.Label(position, num3.ToString() + str, GUIManager.gs_style3);
		GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
		GUI.Label(rect, num3.ToString() + str, GUIManager.gs_style3);
		num += 130;
		rect = new Rect((float)num, (float)(num2 + 5), 128f, 32f);
		position = new Rect((float)(num + 1), (float)(num2 + 1 + 5), 128f, 32f);
		if (PlayerProfile.network != NETWORK.KR)
		{
			if (Gold.Purchases[_pos].PStatus == 0)
			{
				rect = new Rect((float)(num - 24), (float)(num2 - 4), 128f, 32f);
				position = new Rect((float)(num + 1 - 24), (float)(num2 + 1 - 4), 128f, 32f);
				if (Gold.Purchases[_pos].PUpdating)
				{
					GUIManager.DrawText(rect, Lang.GetLabel(869), 20, TextAnchor.MiddleCenter, 8);
				}
				else
				{
					GUI.DrawTexture(rect, GUIManager.tex_select);
					GUIManager.DrawText(rect, Lang.GetLabel(863), 20, TextAnchor.MiddleCenter, 8);
					if (GUI.Button(rect, "", GUIManager.gs_empty))
					{
						Gold.Purchases[_pos].PUpdating = true;
						MainMenu.MainAS.PlayOneShot(SoundManager.SoundClick, AudioListener.volume);
						WEB_HANDLER.STEAM_BUY_ITEM((ulong)((long)Gold.Purchases[_pos].PID), 2, new OnRequestFinishedDelegate(Gold.OnSteamBuyItemFinish));
					}
				}
			}
			else if (Gold.Purchases[_pos].PStatus == 1 || Gold.Purchases[_pos].PStatus == 6)
			{
				GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
				GUI.Label(position, Lang.GetLabel(861), GUIManager.gs_style3);
				GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
				GUI.Label(rect, Lang.GetLabel(861), GUIManager.gs_style3);
			}
			else
			{
				GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
				GUI.Label(position, Lang.GetLabel(862), GUIManager.gs_style3);
				GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
				GUI.Label(rect, Lang.GetLabel(862), GUIManager.gs_style3);
			}
		}
		else if (Gold.Purchases[_pos].PStatus == 1)
		{
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, Lang.GetLabel(861), GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, Lang.GetLabel(861), GUIManager.gs_style3);
		}
		else
		{
			GUIManager.gs_style3.normal.textColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(position, Lang.GetLabel(862), GUIManager.gs_style3);
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUI.Label(rect, Lang.GetLabel(862), GUIManager.gs_style3);
		}
		GUIManager.gs_style3.alignment = TextAnchor.UpperCenter;
	}

	// Token: 0x06000068 RID: 104 RVA: 0x000079FB File Offset: 0x00005BFB
	private IEnumerator ReInit()
	{
		yield return new WaitForSeconds(2f);
		PlayerProfile.get_player_stats = false;
		yield break;
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00007A04 File Offset: 0x00005C04
	public void OnPurchaseHistory(HTTPRequest req, HTTPResponse resp)
	{
		this.history_loaded = true;
		switch (req.State)
		{
		case HTTPRequestStates.Finished:
		{
			if (!resp.IsSuccess)
			{
				Debug.LogWarning(string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText));
				return;
			}
			if (resp.DataAsText.Contains("API_ERROR"))
			{
				WEB_HANDLER.CHECK_ERROR(resp.DataAsText);
				return;
			}
			Gold.Purchases.Clear();
			UTILS.BEGIN_READ(resp.Data, 0);
			int num = UTILS.READ_LONG();
			for (int i = 0; i < num; i++)
			{
				Gold.Purchases.Add(new Purchase(UTILS.READ_LONG(), UTILS.READ_LONG(), UTILS.READ_LONG(), UTILS.READ_LONG(), UTILS.READ_LONG(), UTILS.READ_LONG()));
			}
			return;
		}
		case HTTPRequestStates.Error:
			Debug.LogError("Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
			return;
		case HTTPRequestStates.Aborted:
			Debug.LogWarning("Request Aborted!");
			return;
		case HTTPRequestStates.ConnectionTimedOut:
			Debug.LogError("Connection Timed Out!");
			return;
		case HTTPRequestStates.TimedOut:
			Debug.LogError("Processing the request Timed Out!");
			return;
		default:
			return;
		}
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00007B40 File Offset: 0x00005D40
	public static void OnSteamBuyItem(HTTPRequest req, HTTPResponse resp)
	{
		Gold.WaitingTimer = 0f;
		Gold.WaitingForResponce = false;
		switch (req.State)
		{
		case HTTPRequestStates.Finished:
			if (!resp.IsSuccess)
			{
				Debug.LogWarning(string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText));
				return;
			}
			if (resp.DataAsText.Contains("API_ERROR"))
			{
				WEB_HANDLER.CHECK_ERROR(resp.DataAsText);
				return;
			}
			if (resp.DataAsText.Contains("DUBLICATE"))
			{
				Shop.THIS.message = Lang.GetLabel(108);
				return;
			}
			break;
		case HTTPRequestStates.Error:
			Debug.LogError("Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
			return;
		case HTTPRequestStates.Aborted:
			Debug.LogWarning("Request Aborted!");
			return;
		case HTTPRequestStates.ConnectionTimedOut:
			Debug.LogError("Connection Timed Out!");
			return;
		case HTTPRequestStates.TimedOut:
			Debug.LogError("Processing the request Timed Out!");
			break;
		default:
			return;
		}
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00007C58 File Offset: 0x00005E58
	public static void OnSteamBuyItemFinish(HTTPRequest req, HTTPResponse resp)
	{
		Gold.WaitingTimer = 0f;
		Gold.WaitingForResponce = false;
		switch (req.State)
		{
		case HTTPRequestStates.Finished:
		{
			if (!resp.IsSuccess)
			{
				Debug.LogWarning(string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText));
				return;
			}
			string dataAsText = resp.DataAsText;
			if (dataAsText.Contains("ERR"))
			{
				WEB_HANDLER.CHECK_ERROR(resp.DataAsText);
				return;
			}
			string[] array = dataAsText.Split(new char[]
			{
				'|'
			});
			if (array[0].Equals("OK"))
			{
				int.Parse(array[1]);
				int num = int.Parse(array[2]);
				int num2 = int.Parse(array[3]);
				if (Gold.Purchases.Count > 0)
				{
					int i = 0;
					int count = Gold.Purchases.Count;
					while (i < count)
					{
						if (Gold.Purchases[i].PID == num)
						{
							Gold.Purchases[i].PStatus = num2;
							Gold.Purchases[i].PUpdating = false;
							break;
						}
						i++;
					}
				}
				if (num2 == 1 || num2 == 6)
				{
					PlayerProfile.get_player_stats = false;
					GameController.THIS.playerRefresh();
					Inv.needRefresh = true;
					Shop.THIS.message = Lang.GetLabel(107);
					return;
				}
			}
			break;
		}
		case HTTPRequestStates.Error:
			Debug.LogError("Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
			return;
		case HTTPRequestStates.Aborted:
			Debug.LogWarning("Request Aborted!");
			return;
		case HTTPRequestStates.ConnectionTimedOut:
			Debug.LogError("Connection Timed Out!");
			return;
		case HTTPRequestStates.TimedOut:
			Debug.LogError("Processing the request Timed Out!");
			break;
		default:
			return;
		}
	}

	// Token: 0x04000058 RID: 88
	public bool Active;

	// Token: 0x04000059 RID: 89
	private int type = 1;

	// Token: 0x0400005A RID: 90
	private bool[] hovermode = new bool[3];

	// Token: 0x0400005B RID: 91
	private bool[] hover = new bool[10];

	// Token: 0x0400005C RID: 92
	private List<ActionItem> ActionItems;

	// Token: 0x0400005D RID: 93
	private static List<Purchase> Purchases = new List<Purchase>();

	// Token: 0x0400005E RID: 94
	private static bool WaitingForResponce;

	// Token: 0x0400005F RID: 95
	private static float WaitingTimer;

	// Token: 0x04000060 RID: 96
	private bool _show_canceled = true;

	// Token: 0x04000061 RID: 97
	private bool show_canceled = true;

	// Token: 0x04000062 RID: 98
	private bool history_loaded;

	// Token: 0x04000063 RID: 99
	private Vector2 scrollViewVector = Vector2.zero;

	// Token: 0x04000064 RID: 100
	private int sh;
}
