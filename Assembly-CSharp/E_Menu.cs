using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008D RID: 141
public class E_Menu : MonoBehaviour
{
	// Token: 0x06000406 RID: 1030 RVA: 0x0004ED1C File Offset: 0x0004CF1C
	public void Start()
	{
		this.Tabs = new Texture[6];
		this.Tabs_Rects = new Rect[6];
		this.Title_Background = GUI3.GetTexture2D(Color.black, 100f);
		this.Tab_Background = GUI3.GetTexture2D(Color.black, 75f);
		this.Active_Tab_Background = GUI3.GetTexture2D(Color.black, 60f);
		this.Tab_Background_Hover = GUI3.GetTexture2D(new Color(1f, 0.4f, 0f), 100f);
		this.Tab_Background_Active = GUI3.GetTexture2D(new Color(0.38f, 0.38f, 0.38f), 75f);
		this.Item_Background = GUI3.GetTexture2D(Color.gray, 35f);
		this.Item_Name_Background = GUI3.GetTexture2D(Color.black, 35f);
		this.Category_Background = GUI3.GetTexture2D(new Color(0.38f, 0.38f, 0.38f), 75f);
		this.Item_Background_Hover = GUI3.GetTexture2D(new Color(1f, 0.4f, 0f), 100f);
		this.Play_Button_Normal = (Resources.Load("NewMenu/Play_Button_Normal") as Texture);
		this.Play_Button_Hover = (Resources.Load("NewMenu/Play_Button_Hover") as Texture);
		this.Item_Bar_Red = (Resources.Load("NewMenu/Item_Bar_Red") as Texture);
		this.Item_Bar_Blue = (Resources.Load("NewMenu/Item_Bar_Blue") as Texture);
		this.Item_Bar_Sharp = (Resources.Load("NewMenu/Item_Bar_Sharp") as Texture);
		this.Tabs[0] = (Resources.Load("NewMenu/E_Menu_Tab_Melee") as Texture);
		this.Tabs[1] = (Resources.Load("NewMenu/E_Menu_Tab_Primary") as Texture);
		this.Tabs[2] = (Resources.Load("NewMenu/E_Menu_Tab_Secondary") as Texture);
		this.Tabs[3] = (Resources.Load("NewMenu/E_Menu_Tab_Equip") as Texture);
		this.Tabs[4] = (Resources.Load("NewMenu/E_Menu_Tab_Equip") as Texture);
		this.Tabs[5] = (Resources.Load("NewMenu/E_Menu_Tab_Vehicles") as Texture);
		this.Border = GUI3.GetTexture2D(Color.black, 100f);
		this.TabsActive = new bool[6];
		this.TabsHover = new bool[6];
		this.SelectedItem.Add(0, new int[1]);
		this.SelectedItem.Add(1, new int[1]);
		this.SelectedItem.Add(2, new int[1]);
		this.SelectedItem.Add(3, new int[1]);
		this.SelectedItem.Add(4, new int[5]);
		this.SelectedItem.Add(5, new int[1]);
		this.TabsActive[0] = true;
		this.TabsActive[1] = true;
		this.TabsActive[2] = true;
		this.TabsActive[3] = false;
		this.TabsActive[4] = true;
		if (CONST.GetGameMode() == MODE.TANK)
		{
			this.TabsActive[5] = true;
		}
		else
		{
			this.TabsActive[5] = false;
		}
		this.TabsHover[0] = false;
		this.TabsHover[1] = false;
		this.TabsHover[2] = false;
		this.TabsHover[3] = false;
		this.TabsHover[4] = false;
		this.TabsHover[5] = false;
		this.Active_Item_PodIndex = 0;
		this.SelectedItem[0][0] = 33;
		this.SelectedItem[1][0] = 44;
		this.SelectedItem[2][0] = 46;
		this.SelectedItem[3][0] = 0;
		this.SelectedItem[4][0] = 0;
		this.SelectedItem[4][1] = 0;
		this.SelectedItem[4][2] = 0;
		this.SelectedItem[4][3] = 0;
		this.SelectedItem[4][4] = 0;
		this.SelectedItem[5][0] = 0;
		this.Inactive_Tabs_Space_X = this.x;
		this.TabsNames = new string[6];
		this.WeaponBarsNames = new string[6];
		this.VehicleBarsNames = new string[5];
		this.WeaponsCategoryNames = new string[14];
		this.TabsNames[0] = Lang.GetLabel(410);
		this.TabsNames[1] = Lang.GetLabel(411);
		this.TabsNames[2] = Lang.GetLabel(412);
		this.TabsNames[3] = Lang.GetLabel(413);
		this.TabsNames[4] = Lang.GetLabel(413);
		this.TabsNames[5] = Lang.GetLabel(414);
		this.WeaponBarsNames[1] = Lang.GetLabel(415);
		this.WeaponBarsNames[2] = Lang.GetLabel(417);
		this.WeaponBarsNames[3] = Lang.GetLabel(418);
		this.WeaponBarsNames[4] = Lang.GetLabel(416);
		this.WeaponBarsNames[5] = Lang.GetLabel(419);
		this.VehicleBarsNames[0] = Lang.GetLabel(420);
		this.VehicleBarsNames[1] = Lang.GetLabel(421);
		this.VehicleBarsNames[2] = Lang.GetLabel(422);
		this.VehicleBarsNames[3] = Lang.GetLabel(423);
		this.VehicleBarsNames[4] = Lang.GetLabel(424);
		for (int i = 1; i < 14; i++)
		{
			this.WeaponsCategoryNames[i] = Lang.GetLabel(575 + i);
		}
		this.gui_style = new GUIStyle();
		this.gui_style.font = ContentLoader.LoadFont("E_Menu_Font4");
		this.gui_style.normal.textColor = Color.white;
		this.Init();
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x0004F2A0 File Offset: 0x0004D4A0
	public void Init()
	{
		if (PlayerPrefs.HasKey("PWID"))
		{
			this.SelectedItem[1][0] = this.GetWeaponByID(PlayerPrefs.GetInt("PWID"), this.SelectedItem[1][0]);
		}
		if (PlayerPrefs.HasKey("SWID"))
		{
			this.SelectedItem[2][0] = this.GetWeaponByID(PlayerPrefs.GetInt("SWID"), this.SelectedItem[2][0]);
		}
		if (PlayerPrefs.HasKey("MWID"))
		{
			this.SelectedItem[0][0] = this.GetWeaponByID(PlayerPrefs.GetInt("MWID"), this.SelectedItem[0][0]);
		}
		if (PlayerPrefs.HasKey("A1WID"))
		{
			this.SelectedItem[4][0] = this.GetWeaponByID(PlayerPrefs.GetInt("A1WID"), this.SelectedItem[4][0]);
		}
		if (PlayerPrefs.HasKey("A2WID"))
		{
			this.SelectedItem[4][1] = this.GetWeaponByID(PlayerPrefs.GetInt("A2WID"), this.SelectedItem[4][1]);
		}
		if (PlayerPrefs.HasKey("A3WID"))
		{
			this.SelectedItem[4][2] = this.GetWeaponByID(PlayerPrefs.GetInt("A3WID"), this.SelectedItem[4][2]);
		}
		if (PlayerPrefs.HasKey("G1WID"))
		{
			this.SelectedItem[4][3] = this.GetWeaponByID(PlayerPrefs.GetInt("G1WID"), this.SelectedItem[4][3]);
		}
		if (PlayerPrefs.HasKey("G2WID"))
		{
			this.SelectedItem[4][4] = this.GetWeaponByID(PlayerPrefs.GetInt("G2WID"), this.SelectedItem[4][4]);
		}
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x0004F470 File Offset: 0x0004D670
	public void Draw_E_Menu()
	{
		this.TabsActive[0] = true;
		this.TabsActive[1] = true;
		this.TabsActive[2] = true;
		this.TabsActive[3] = false;
		this.TabsActive[4] = true;
		if (CONST.GetGameMode() == MODE.TANK)
		{
			this.TabsActive[5] = true;
		}
		else
		{
			this.TabsActive[5] = false;
		}
		this.Play_Button_Rect = new Rect((float)(Screen.width / 2 - 86), 50f, 172f, 42f);
		this.x = Screen.width / 2 - 300;
		this.y = Screen.height / 2 - (Screen.height - 220) / 2;
		int num = Screen.height - 390;
		if (num < 60)
		{
			num = 60;
		}
		this.Title_Background_Rect = new Rect((float)this.x, (float)this.y, 600f, 22f);
		this.Tabs_Rect = new Rect((float)this.x, (float)(this.y + 22), 600f, 50f);
		this.Active_Tab_Rect = new Rect((float)this.x, (float)(this.y + 72), 600f, (float)num);
		this.Active_Item_Rect = new Rect((float)this.x, (float)(this.y + num + 72), 600f, 80f);
		this.Active_Item_Rect1 = new Rect((float)(this.x + 3), this.Active_Item_Rect.y + 3f, 194f, 74f);
		if (this.Active_Tab_Index == 1 || this.Active_Tab_Index == 2 || this.Active_Tab_Index == 5)
		{
			this.Active_Item_Rect2 = new Rect((float)(this.x + 200), this.Active_Item_Rect.y, 200f, 80f);
			this.Active_Item_Rect3 = new Rect((float)(this.x + 400), this.Active_Item_Rect.y, 200f, 80f);
		}
		else
		{
			this.Active_Item_Rect2 = new Rect(0f, 0f, 0f, 0f);
			this.Active_Item_Rect3 = new Rect((float)(this.x + 200), this.Active_Item_Rect.y, 400f, 80f);
		}
		this.mousePos = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
		if (this.Play_Button_Rect.Contains(this.mousePos))
		{
			this.In_Play_Button_Rect = true;
		}
		else
		{
			this.In_Play_Button_Rect = false;
		}
		this.Tabs_Rects = new Rect[6];
		int num2 = this.x;
		int num3 = this.y + 22;
		int num4 = 120;
		int num5 = 600;
		for (int i = 0; i < this.TabsActive.Length; i++)
		{
			if (this.TabsActive[i])
			{
				this.Tabs_Rects[i] = new Rect((float)num2, (float)num3, (float)num4, (float)this.Tabs[i].height);
				if (this.Tabs_Rects[i].Contains(this.mousePos))
				{
					this.TabsHover[i] = true;
				}
				else
				{
					this.TabsHover[i] = false;
				}
				num2 += num4;
				num5 -= num4;
			}
		}
		this.Draw_Background();
		this.Draw_Play_Button();
		this.Draw_Tabs();
		this.Draw_Active_Tab();
		if (this.Active_Tab_Index == 5)
		{
			this.Draw_Active_Vehicle();
		}
		else
		{
			this.Draw_Active_Item();
		}
		this.Draw_Borders(this.Title_Background_Rect, true, true, true, true);
		this.Draw_Borders(this.Tabs_Rect, true, false, true, false);
		this.Draw_Borders(this.Active_Tab_Rect, true, false, true, false);
		this.Draw_Borders(this.Active_Item_Rect, true, true, true, true);
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x0004F818 File Offset: 0x0004DA18
	public void Draw_Background()
	{
		GUI.DrawTexture(this.Title_Background_Rect, this.Title_Background);
		GUI.DrawTexture(this.Tabs_Rect, this.Tab_Background);
		GUI.DrawTexture(this.Active_Tab_Rect, this.Active_Tab_Background);
		GUI.DrawTexture(this.Active_Item_Rect, this.Tab_Background);
		this.gui_style.fontSize = 18;
		this.gui_style.alignment = TextAnchor.MiddleCenter;
		this.gui_style.normal.textColor = Color.black;
		this.gui_style.padding.bottom = 0;
		GUI.Label(new Rect(this.Title_Background_Rect.x + 1f, this.Title_Background_Rect.y + 1f, this.Title_Background_Rect.width, 22f), Lang.GetLabel(428), this.gui_style);
		this.gui_style.normal.textColor = Color.yellow;
		GUI.Label(this.Title_Background_Rect, Lang.GetLabel(428), this.gui_style);
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x0004F924 File Offset: 0x0004DB24
	public void Draw_Borders(Rect _rect, bool left, bool top, bool right, bool bottom)
	{
		if (left)
		{
			GUI.DrawTexture(new Rect(_rect.x, _rect.y, 2f, _rect.height), this.Border);
		}
		if (top)
		{
			GUI.DrawTexture(new Rect(_rect.x, _rect.y, _rect.width, 2f), this.Border);
		}
		if (right)
		{
			GUI.DrawTexture(new Rect(_rect.xMax - 2f, _rect.y, 2f, _rect.height), this.Border);
		}
		if (bottom)
		{
			GUI.DrawTexture(new Rect(_rect.x, _rect.yMax - 2f, _rect.width, 2f), this.Border);
		}
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x0004F9F4 File Offset: 0x0004DBF4
	public void Draw_Play_Button()
	{
		if (this.In_Play_Button_Rect)
		{
			GUI.DrawTexture(this.Play_Button_Rect, this.Play_Button_Hover);
		}
		else
		{
			GUI.DrawTexture(this.Play_Button_Rect, this.Play_Button_Normal);
		}
		this.gui_style.fontSize = 22;
		this.gui_style.alignment = TextAnchor.MiddleCenter;
		this.gui_style.normal.textColor = Color.black;
		this.gui_style.padding.bottom = 0;
		GUI.Label(new Rect(this.Play_Button_Rect.x + 2f, this.Play_Button_Rect.y + 2f, this.Play_Button_Rect.width, this.Play_Button_Rect.height), Lang.GetLabel(429), this.gui_style);
		this.gui_style.normal.textColor = Color.white;
		GUI.Label(this.Play_Button_Rect, Lang.GetLabel(429), this.gui_style);
		if (GUI.Button(this.Play_Button_Rect, "", this.gui_style))
		{
			if (this.cscl == null)
			{
				this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
			}
			this.cscl.send_new_config(this.SelectedItem[0][0], this.SelectedItem[1][0], this.SelectedItem[2][0], this.SelectedItem[4][0], this.SelectedItem[4][1], this.SelectedItem[4][2], this.SelectedItem[4][3], this.SelectedItem[4][4]);
			if (this.SelectedItem[5][0] > 0)
			{
				this.cscl.send_spawn_my_vehicle(this.SelectedItem[5][0]);
				this.SelectedItem[5][0] = 0;
			}
		}
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x0004FBE8 File Offset: 0x0004DDE8
	public void Draw_Tabs()
	{
		for (int i = 0; i < this.TabsActive.Length; i++)
		{
			if (this.TabsActive[i])
			{
				if (this.Active_Tab_Index == i)
				{
					GUI.DrawTexture(this.Tabs_Rects[i], this.Tab_Background_Active);
				}
				else if (this.TabsHover[i])
				{
					GUI.DrawTexture(this.Tabs_Rects[i], this.Tab_Background_Hover);
					if (GUI.Button(this.Tabs_Rects[i], "", this.gui_style))
					{
						this.Active_Tab_Index = i;
						this.Active_Item_PodIndex = 0;
					}
				}
				GUI.DrawTexture(this.Tabs_Rects[i], this.Tabs[i]);
				this.gui_style.fontSize = 12;
				this.gui_style.alignment = TextAnchor.LowerCenter;
				this.gui_style.padding.bottom = 3;
				this.gui_style.normal.textColor = Color.black;
				GUI.Label(new Rect(this.Tabs_Rects[i].x + 1f, this.Tabs_Rects[i].y + 1f, this.Tabs_Rects[i].width, this.Tabs_Rects[i].height), this.TabsNames[i], this.gui_style);
				this.gui_style.normal.textColor = Color.white;
				GUI.Label(this.Tabs_Rects[i], this.TabsNames[i], this.gui_style);
			}
			if (this.Active_Tab_Index == i)
			{
				this.Draw_Borders(this.Tabs_Rects[i], true, false, true, false);
			}
			else
			{
				this.Draw_Borders(this.Tabs_Rects[i], false, false, false, true);
			}
		}
		GUI.DrawTexture(this.Inactive_Space_Rect, this.Tab_Background);
		this.Draw_Borders(this.Inactive_Space_Rect, false, false, false, true);
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x0004FDD8 File Offset: 0x0004DFD8
	public void Draw_Active_Tab()
	{
		this.ResetPos();
		this.scrollViewVector = GUI3.BeginScrollView(this.Active_Tab_Rect, this.scrollViewVector, new Rect(0f, 0f, 0f, this.sh), false);
		switch (this.Active_Tab_Index)
		{
		case 0:
			this.Draw_Melee();
			goto IL_8D;
		case 1:
			this.Draw_Primary();
			goto IL_8D;
		case 2:
			this.Draw_Secondary();
			goto IL_8D;
		case 4:
			this.Draw_Ammunition();
			goto IL_8D;
		case 5:
			this.Draw_Vehicles();
			goto IL_8D;
		}
		this.Draw_Melee();
		IL_8D:
		GUI3.EndScrollView();
		this.sh = (float)this.y_pos;
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x0004FE84 File Offset: 0x0004E084
	public void Draw_Melee()
	{
		if (ItemsDB.Items.Length == 0)
		{
			return;
		}
		this.DrawCategory(Lang.GetLabel(524), 5, true);
		for (int i = 0; i < 500; i++)
		{
			if (ItemsDB.CheckItem(i) && ItemsDB.Items[i].Category == 7 && ItemsDB.Items[i].ItemID != 35 && (i <= 0 || RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[i] != 0))
			{
				this.DrawItem(i, 0);
				this.NextPos(false);
			}
		}
		this.NextPos(true);
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x0004FF18 File Offset: 0x0004E118
	public void Draw_Primary()
	{
		if (ItemsDB.Items.Length == 0)
		{
			return;
		}
		for (int i = 2; i < 9; i++)
		{
			if (i != 7)
			{
				this.DrawCategory(this.WeaponsCategoryNames[i], 5, true);
				for (int j = 0; j < 500; j++)
				{
					if (ItemsDB.CheckItem(j) && ItemsDB.Items[j].Category == i && RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[j] != 0)
					{
						this.DrawItem(j, 0);
						this.NextPos(false);
					}
				}
				this.NextPos(true);
			}
		}
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x0004FFA8 File Offset: 0x0004E1A8
	public void Draw_Secondary()
	{
		if (ItemsDB.Items.Length == 0)
		{
			return;
		}
		this.DrawCategory(Lang.GetLabel(525), 5, true);
		for (int i = -10; i < 500; i++)
		{
			if (ItemsDB.CheckItem(i) && ItemsDB.Items[i].Category == 1 && (i <= 0 || RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[i] != 0))
			{
				this.DrawItem(i, 0);
				this.NextPos(false);
			}
		}
		this.NextPos(true);
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x0005002C File Offset: 0x0004E22C
	public void Draw_Ammunition()
	{
		if (ItemsDB.Items.Length == 0)
		{
			return;
		}
		this.DrawCategory(Lang.GetLabel(430), 1, false);
		this.DrawCategory(Lang.GetLabel(431), 1, false);
		this.DrawCategory(Lang.GetLabel(432), 1, false);
		this.DrawCategory(Lang.GetLabel(433), 2, true);
		int val = this.y_pos;
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[62] != 0)
		{
			this.DrawItem(62, 0);
			this.NextYPos(false);
			val = Math.Max(val, this.y_pos);
		}
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[10] != 0)
		{
			this.DrawItem(10, 0);
			this.NextYPos(false);
			val = Math.Max(val, this.y_pos);
		}
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[185] != 0)
		{
			this.DrawItem(185, 0);
			this.NextYPos(false);
			val = Math.Max(val, this.y_pos);
		}
		this.NextYPos(true);
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[55] != 0)
		{
			this.DrawItem(55, 1);
			this.NextYPos(false);
			val = Math.Max(val, this.y_pos);
		}
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[171] != 0)
		{
			this.DrawItem(171, 1);
			this.NextYPos(false);
			val = Math.Max(val, this.y_pos);
		}
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[172] != 0)
		{
			this.DrawItem(172, 1);
			this.NextYPos(false);
			val = Math.Max(val, this.y_pos);
		}
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[183] != 0)
		{
			this.DrawItem(183, 1);
			this.NextYPos(false);
			val = Math.Max(val, this.y_pos);
		}
		this.NextYPos(true);
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[138] != 0)
		{
			this.DrawItem(138, 2);
			this.NextYPos(false);
			val = Math.Max(val, this.y_pos);
		}
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[100] != 0)
		{
			this.DrawItem(100, 2);
			this.NextYPos(false);
			val = Math.Max(val, this.y_pos);
		}
		this.NextYPos(true);
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[7] != 0)
		{
			this.DrawItem(7, 3);
			this.NextYPos(false);
			val = Math.Max(val, this.y_pos);
		}
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[168] != 0)
		{
			this.DrawItem(168, 3);
			this.NextYPos(false);
			val = Math.Max(val, this.y_pos);
		}
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[170] != 0)
		{
			this.DrawItem(170, 3);
			this.NextYPos(false);
			val = Math.Max(val, this.y_pos);
		}
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[156] != 0)
		{
			this.DrawItem(156, 3);
			this.NextYPos(false);
			val = Math.Max(val, this.y_pos);
		}
		this.NextYPos(true);
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[169] != 0)
		{
			this.DrawItem(169, 4);
			this.NextYPos(false);
			val = Math.Max(val, this.y_pos);
		}
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[184] != 0)
		{
			this.DrawItem(184, 4);
			this.NextYPos(false);
			val = Math.Max(val, this.y_pos);
		}
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[186] != 0)
		{
			this.DrawItem(186, 4);
			this.NextYPos(false);
			val = Math.Max(val, this.y_pos);
		}
		this.NextYPos(true);
		this.y_pos = val;
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x0005046C File Offset: 0x0004E66C
	public void Draw_Vehicles()
	{
		this.DrawCategory(Lang.GetLabel(434), 5, true);
		for (int i = 200; i < 204; i++)
		{
			this.DrawVehicle(i, 0);
			this.NextPos(false);
		}
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x000504B0 File Offset: 0x0004E6B0
	private void NextPos(bool end = false)
	{
		if (end)
		{
			if (this.icount != 0 && this.icount != 5)
			{
				this.y_pos += 59;
			}
			this.x_pos = 2;
			return;
		}
		this.x_pos += 117;
		this.icount++;
		if (this.icount == 5)
		{
			this.icount = 0;
			this.x_pos = 2;
			this.y_pos += 59;
		}
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x0005052B File Offset: 0x0004E72B
	private void NextYPos(bool end = false)
	{
		if (end)
		{
			this.x_pos += 117;
			this.y_pos = 17;
			return;
		}
		this.y_pos += 59;
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x00050557 File Offset: 0x0004E757
	private void ResetPos()
	{
		this.x_pos = 2;
		this.y_pos = 0;
		this.icount = 0;
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x00050570 File Offset: 0x0004E770
	private void DrawCategory(string _text, int _cols, bool _ending)
	{
		GUI.DrawTexture(new Rect((float)this.x_pos, (float)this.y_pos, (float)(116 * _cols + _cols - 1), 16f), this.Category_Background);
		this.gui_style.fontSize = 12;
		this.gui_style.alignment = TextAnchor.MiddleCenter;
		this.gui_style.normal.textColor = Color.black;
		this.gui_style.padding.bottom = 0;
		GUI.Label(new Rect((float)this.x_pos, (float)(this.y_pos + 1), (float)(116 * _cols + _cols - 1), 16f), _text, this.gui_style);
		this.gui_style.normal.textColor = Color.white;
		GUI.Label(new Rect((float)this.x_pos, (float)this.y_pos, (float)(116 * _cols + _cols - 1), 16f), _text, this.gui_style);
		this.x_pos += 117 * _cols;
		if (_ending)
		{
			this.y_pos += 17;
			this.x_pos = 2;
			this.icount = 0;
		}
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x0005068C File Offset: 0x0004E88C
	private void DrawItem(int index, int _Active_Item_PodIndex)
	{
		Rect rect = new Rect((float)this.x_pos, (float)this.y_pos, 116f, 58f);
		Rect position = new Rect((float)this.x_pos, (float)this.y_pos, 116f, 15f);
		if (this.SelectedItem[this.Active_Tab_Index][_Active_Item_PodIndex] == index || (rect.Contains(Event.current.mousePosition) && this.Active_Tab_Rect.Contains(this.mousePos)))
		{
			GUI.DrawTexture(rect, this.Item_Background_Hover);
		}
		else
		{
			GUI.DrawTexture(rect, this.Item_Background);
		}
		GUI.DrawTexture(position, this.Item_Name_Background);
		GUI.DrawTexture(rect, ItemsDB.Items[index].icon);
		this.gui_style.fontSize = 10;
		this.gui_style.alignment = TextAnchor.UpperLeft;
		this.gui_style.normal.textColor = Color.black;
		this.SetPadding(2, 0, 0, 2);
		Rect position2 = new Rect(rect.x + 1f, rect.y + 1f, rect.width, rect.height);
		ITEM item = (ITEM)index;
		GUI.Label(position2, item.ToString(), this.gui_style);
		this.gui_style.normal.textColor = Color.white;
		Rect position3 = rect;
		item = (ITEM)index;
		GUI.Label(position3, item.ToString(), this.gui_style);
		this.SetPadding(0, 0, 0, 0);
		if (GUI.Button(rect, "", this.gui_style))
		{
			this.SelectedItem[this.Active_Tab_Index][_Active_Item_PodIndex] = index;
			this.Active_Item_PodIndex = _Active_Item_PodIndex;
		}
		if (this.Active_Tab_Index == 3)
		{
			this.gui_style.fontSize = 10;
			this.gui_style.alignment = TextAnchor.LowerRight;
			this.gui_style.normal.textColor = Color.black;
			this.SetPadding(2, 5, 2, 0);
			GUI.Label(new Rect(rect.x + 1f, rect.y + 1f, rect.width, rect.height), RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[index].ToString(), this.gui_style);
			this.gui_style.normal.textColor = Color.white;
			GUI.Label(rect, RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Item[index].ToString(), this.gui_style);
			this.SetPadding(0, 0, 0, 0);
		}
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x00050918 File Offset: 0x0004EB18
	private void DrawVehicle(int index, int _Active_Item_PodIndex)
	{
		Rect position = new Rect((float)this.x_pos, (float)this.y_pos, 116f, 58f);
		Rect position2 = new Rect((float)this.x_pos, (float)this.y_pos, 116f, 15f);
		if (this.SelectedItem[this.Active_Tab_Index][_Active_Item_PodIndex] == index || (position.Contains(Event.current.mousePosition) && this.Active_Tab_Rect.Contains(this.mousePos)))
		{
			GUI.DrawTexture(position, this.Item_Background_Hover);
		}
		else
		{
			GUI.DrawTexture(position, this.Item_Background);
		}
		GUI.DrawTexture(position2, this.Item_Name_Background);
		this.gui_style.fontSize = 10;
		this.gui_style.alignment = TextAnchor.UpperLeft;
		this.gui_style.normal.textColor = Color.black;
		this.SetPadding(2, 0, 0, 2);
		this.gui_style.normal.textColor = Color.white;
		this.SetPadding(0, 0, 0, 0);
		if (GUI.Button(position, "", this.gui_style))
		{
			this.SelectedItem[this.Active_Tab_Index][_Active_Item_PodIndex] = index;
			this.Active_Item_PodIndex = _Active_Item_PodIndex;
		}
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x00050A48 File Offset: 0x0004EC48
	public void Draw_Active_Item()
	{
		if (ItemsDB.Items.Length == 0)
		{
			return;
		}
		if (!ItemsDB.CheckItem(this.SelectedItem[this.Active_Tab_Index][this.Active_Item_PodIndex]))
		{
			return;
		}
		if (this.SelectedItem[this.Active_Tab_Index][this.Active_Item_PodIndex] == 0)
		{
			return;
		}
		GUI.DrawTexture(this.Active_Item_Rect1, this.Item_Background);
		GUI.DrawTexture(new Rect(this.Active_Item_Rect1.x, this.Active_Item_Rect1.y, this.Active_Item_Rect1.width, 18f), this.Item_Name_Background);
		GUI.DrawTexture(new Rect(this.Active_Item_Rect1.x + 10f, this.Active_Item_Rect1.y, this.Active_Item_Rect1.height * 2f, this.Active_Item_Rect1.height), ItemsDB.Items[this.SelectedItem[this.Active_Tab_Index][this.Active_Item_PodIndex]].icon);
		if ((this.Active_Tab_Index == 1 || this.Active_Tab_Index == 2) && this.SelectedItem[this.Active_Tab_Index][this.Active_Item_PodIndex] != 174 && this.SelectedItem[this.Active_Tab_Index][this.Active_Item_PodIndex] != 304)
		{
			for (int i = 1; i < 6; i++)
			{
				this.DrawBar(i);
			}
		}
		this.gui_style.fontSize = 14;
		this.gui_style.alignment = TextAnchor.UpperLeft;
		this.gui_style.normal.textColor = Color.black;
		this.SetPadding(2, 0, 0, 10);
		Rect position = new Rect(this.Active_Item_Rect1.x + 1f, this.Active_Item_Rect1.y + 1f, this.Active_Item_Rect1.width, this.Active_Item_Rect1.height);
		ITEM item = (ITEM)this.SelectedItem[this.Active_Tab_Index][this.Active_Item_PodIndex];
		GUI.Label(position, item.ToString(), this.gui_style);
		this.gui_style.normal.textColor = Color.white;
		Rect active_Item_Rect = this.Active_Item_Rect1;
		item = (ITEM)this.SelectedItem[this.Active_Tab_Index][this.Active_Item_PodIndex];
		GUI.Label(active_Item_Rect, item.ToString(), this.gui_style);
		this.SetPadding(0, 0, 0, 0);
		this.gui_style.fontSize = 14;
		this.gui_style.alignment = TextAnchor.UpperCenter;
		this.gui_style.normal.textColor = Color.black;
		this.SetPadding(2, 0, 0, 0);
		GUI.Label(new Rect(this.Active_Item_Rect3.x + 1f, this.Active_Item_Rect3.y + 1f, this.Active_Item_Rect3.width, this.Active_Item_Rect3.height), Lang.GetLabel(435), this.gui_style);
		this.gui_style.normal.textColor = Color.white;
		GUI.Label(this.Active_Item_Rect3, Lang.GetLabel(435), this.gui_style);
		this.SetPadding(0, 0, 0, 0);
		this.gui_style.wordWrap = true;
		Rect viewzone = new Rect(this.Active_Item_Rect3.x, this.Active_Item_Rect3.y + 20f, this.Active_Item_Rect3.width, this.Active_Item_Rect3.height - 10f);
		this.gui_style.fontSize = 12;
		this.gui_style.alignment = TextAnchor.UpperCenter;
		this.gui_style.normal.textColor = Color.black;
		this.SetPadding(5, 5, 5, 5);
		item = (ITEM)this.SelectedItem[this.Active_Tab_Index][this.Active_Item_PodIndex];
		GUIContent content = new GUIContent(item.ToString());
		float height = this.gui_style.CalcHeight(content, viewzone.width - 11f);
		Rect rect = new Rect(0f, 0f, viewzone.width - 11f, height);
		this.scrollViewVector2 = GUI3.BeginScrollView(viewzone, this.scrollViewVector2, rect, false);
		Rect position2 = new Rect(rect.x + 1f, rect.y + 1f, rect.width, rect.height);
		item = (ITEM)this.SelectedItem[this.Active_Tab_Index][this.Active_Item_PodIndex];
		GUI.Label(position2, item.ToString(), this.gui_style);
		this.gui_style.normal.textColor = Color.white;
		Rect position3 = rect;
		item = (ITEM)this.SelectedItem[this.Active_Tab_Index][this.Active_Item_PodIndex];
		GUI.Label(position3, item.ToString(), this.gui_style);
		this.SetPadding(0, 0, 0, 0);
		this.gui_style.wordWrap = false;
		GUI3.EndScrollView();
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x0000248C File Offset: 0x0000068C
	public void Draw_Active_Vehicle()
	{
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x00050F30 File Offset: 0x0004F130
	public void DrawBar(int i)
	{
		int num = (int)this.Active_Item_Rect2.x + 3;
		int num2 = (int)this.Active_Item_Rect2.y + 4 + 14 * (i - 1);
		int num3 = (int)this.Active_Item_Rect2.xMax - 1 - (num + 1 + 90 + 80);
		int num4 = ItemsDB.Items[this.SelectedItem[this.Active_Tab_Index][this.Active_Item_PodIndex]].Upgrades[i][0].Val * 71 / ItemsDB.Items[1000].Upgrades[i][0].Val;
		float num5 = (float)ItemsDB.Items[this.SelectedItem[this.Active_Tab_Index][this.Active_Item_PodIndex]].Upgrades[i][0].Val;
		this.gui_style.fontSize = 10;
		this.gui_style.normal.textColor = Color.black;
		this.SetPadding(0, 0, 0, 0);
		this.gui_style.alignment = TextAnchor.MiddleLeft;
		GUI.Label(new Rect((float)(num + 1), (float)(num2 + 1), 80f, 14f), this.WeaponBarsNames[i], this.gui_style);
		this.gui_style.alignment = TextAnchor.MiddleRight;
		GUI.Label(new Rect((float)(num + 1 + 90 + 80), (float)(num2 + 1), (float)num3, 14f), num5.ToString(), this.gui_style);
		this.gui_style.normal.textColor = Color.white;
		this.gui_style.alignment = TextAnchor.MiddleLeft;
		GUI.Label(new Rect((float)num, (float)num2, 80f, 14f), this.WeaponBarsNames[i], this.gui_style);
		this.gui_style.alignment = TextAnchor.MiddleRight;
		GUI.Label(new Rect((float)(num + 1 + 90 + 80), (float)num2, (float)num3, 14f), num5.ToString(), this.gui_style);
		this.SetPadding(0, 0, 0, 0);
		num += 90;
		if (num4 < 2)
		{
			num4 = 2;
		}
		if (i == 0)
		{
			GUI.DrawTexture(new Rect((float)num, (float)(num2 + 3), (float)num4, 8f), this.Item_Bar_Red);
		}
		else
		{
			GUI.DrawTexture(new Rect((float)num, (float)(num2 + 3), (float)num4, 8f), this.Item_Bar_Blue);
		}
		for (int j = 0; j < 10; j++)
		{
			GUI.DrawTexture(new Rect((float)num, (float)(num2 + 3), 8f, 8f), this.Item_Bar_Sharp);
			num += 7;
		}
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x000511A0 File Offset: 0x0004F3A0
	public void DrawVehicleBar(int i)
	{
		int num = (int)this.Active_Item_Rect2.x + 3;
		int num2 = (int)this.Active_Item_Rect2.y + 4 + 14 * i;
		int num3 = (int)this.Active_Item_Rect2.xMax - 1 - (num + 1 + 90 + 80);
		int num4 = 0;
		float num5 = 0f;
		this.gui_style.fontSize = 10;
		this.gui_style.normal.textColor = Color.black;
		this.SetPadding(0, 0, 0, 0);
		this.gui_style.alignment = TextAnchor.MiddleLeft;
		GUI.Label(new Rect((float)(num + 1), (float)(num2 + 1), 80f, 14f), this.VehicleBarsNames[i], this.gui_style);
		this.gui_style.alignment = TextAnchor.MiddleRight;
		GUI.Label(new Rect((float)(num + 1 + 90 + 80), (float)(num2 + 1), (float)num3, 14f), num5.ToString(), this.gui_style);
		this.gui_style.normal.textColor = Color.white;
		this.gui_style.alignment = TextAnchor.MiddleLeft;
		GUI.Label(new Rect((float)num, (float)num2, 80f, 14f), this.VehicleBarsNames[i], this.gui_style);
		this.gui_style.alignment = TextAnchor.MiddleRight;
		GUI.Label(new Rect((float)(num + 1 + 90 + 80), (float)num2, (float)num3, 14f), num5.ToString(), this.gui_style);
		this.SetPadding(0, 0, 0, 0);
		num += 90;
		if (num4 < 2)
		{
			num4 = 2;
		}
		if (i == 0)
		{
			GUI.DrawTexture(new Rect((float)num, (float)(num2 + 3), (float)num4, 8f), this.Item_Bar_Red);
		}
		else
		{
			GUI.DrawTexture(new Rect((float)num, (float)(num2 + 3), (float)num4, 8f), this.Item_Bar_Blue);
		}
		for (int j = 0; j < 10; j++)
		{
			GUI.DrawTexture(new Rect((float)num, (float)(num2 + 3), 8f, 8f), this.Item_Bar_Sharp);
			num += 7;
		}
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x00051394 File Offset: 0x0004F594
	private void SetPadding(int _top, int _right, int _bottom, int _left)
	{
		this.gui_style.padding.top = _top;
		this.gui_style.padding.left = _left;
		this.gui_style.padding.bottom = _bottom;
		this.gui_style.padding.right = _right;
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x000513E6 File Offset: 0x0004F5E6
	private int GetWeaponByID(int newID, int oldItemID)
	{
		if (!ItemsDB.CheckItem(newID))
		{
			return oldItemID;
		}
		return newID;
	}

	// Token: 0x040008A3 RID: 2211
	private GUIStyle gui_style;

	// Token: 0x040008A4 RID: 2212
	private Vector2 scrollViewVector;

	// Token: 0x040008A5 RID: 2213
	private Vector2 scrollViewVector2;

	// Token: 0x040008A6 RID: 2214
	public Texture Title_Background;

	// Token: 0x040008A7 RID: 2215
	public Texture Tab_Background;

	// Token: 0x040008A8 RID: 2216
	public Texture Active_Tab_Background;

	// Token: 0x040008A9 RID: 2217
	public Texture Tab_Background_Hover;

	// Token: 0x040008AA RID: 2218
	public Texture Tab_Background_Active;

	// Token: 0x040008AB RID: 2219
	public Texture Item_Background;

	// Token: 0x040008AC RID: 2220
	public Texture Item_Name_Background;

	// Token: 0x040008AD RID: 2221
	public Texture Category_Background;

	// Token: 0x040008AE RID: 2222
	public Texture Item_Background_Hover;

	// Token: 0x040008AF RID: 2223
	public Texture Item_Bar_Red;

	// Token: 0x040008B0 RID: 2224
	public Texture Item_Bar_Blue;

	// Token: 0x040008B1 RID: 2225
	public Texture Item_Bar_Sharp;

	// Token: 0x040008B2 RID: 2226
	public Texture Play_Button_Normal;

	// Token: 0x040008B3 RID: 2227
	public Texture Play_Button_Hover;

	// Token: 0x040008B4 RID: 2228
	public Texture2D Border;

	// Token: 0x040008B5 RID: 2229
	public Texture[] Tabs;

	// Token: 0x040008B6 RID: 2230
	public Rect Title_Background_Rect;

	// Token: 0x040008B7 RID: 2231
	public Rect Play_Button_Rect;

	// Token: 0x040008B8 RID: 2232
	public Rect Tabs_Rect;

	// Token: 0x040008B9 RID: 2233
	public Rect Active_Item_Rect;

	// Token: 0x040008BA RID: 2234
	public Rect Inactive_Space_Rect;

	// Token: 0x040008BB RID: 2235
	public Rect Active_Tab_Rect;

	// Token: 0x040008BC RID: 2236
	public Rect Active_Item_Rect1;

	// Token: 0x040008BD RID: 2237
	public Rect Active_Item_Rect2;

	// Token: 0x040008BE RID: 2238
	public Rect Active_Item_Rect3;

	// Token: 0x040008BF RID: 2239
	public Rect[] Tabs_Rects;

	// Token: 0x040008C0 RID: 2240
	public bool In_Play_Button_Rect;

	// Token: 0x040008C1 RID: 2241
	public bool In_Tab_Rect;

	// Token: 0x040008C2 RID: 2242
	public bool[] TabsActive;

	// Token: 0x040008C3 RID: 2243
	public bool[] TabsHover;

	// Token: 0x040008C4 RID: 2244
	public string[] TabsNames;

	// Token: 0x040008C5 RID: 2245
	public string[] WeaponBarsNames;

	// Token: 0x040008C6 RID: 2246
	public string[] VehicleBarsNames;

	// Token: 0x040008C7 RID: 2247
	public string[] WeaponsCategoryNames;

	// Token: 0x040008C8 RID: 2248
	private int x;

	// Token: 0x040008C9 RID: 2249
	private int y;

	// Token: 0x040008CA RID: 2250
	private int koef;

	// Token: 0x040008CB RID: 2251
	private int Inactive_Tabs_Space_X;

	// Token: 0x040008CC RID: 2252
	public int Active_Tab_Index;

	// Token: 0x040008CD RID: 2253
	public Dictionary<int, int[]> SelectedItem = new Dictionary<int, int[]>();

	// Token: 0x040008CE RID: 2254
	public int Active_Item_PodIndex;

	// Token: 0x040008CF RID: 2255
	private int x_pos;

	// Token: 0x040008D0 RID: 2256
	private int y_pos;

	// Token: 0x040008D1 RID: 2257
	private int icount;

	// Token: 0x040008D2 RID: 2258
	private float sh;

	// Token: 0x040008D3 RID: 2259
	private Vector2 mousePos;

	// Token: 0x040008D4 RID: 2260
	private Client cscl;

	// Token: 0x040008D5 RID: 2261
	private WeaponSystem m_WeaponSystem;
}
