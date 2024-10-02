using System;
using UnityEngine;

// Token: 0x02000090 RID: 144
public class New_Select_Team : MonoBehaviour
{
	// Token: 0x0600043C RID: 1084 RVA: 0x0000248C File Offset: 0x0000068C
	public void OpenMenu()
	{
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x0000248C File Offset: 0x0000068C
	public void CloseMenu()
	{
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x00052DE0 File Offset: 0x00050FE0
	private void Awake()
	{
		this.EM = base.GetComponent<E_Menu>();
		this.New_Select_Teams_Icons = new Texture[5];
		this.New_Select_Teams_Icons_Background = new Texture[4];
		this.New_Select_Teams_Icons_Tab = new Texture[4];
		this.New_Select_Teams_Names = new string[4];
		this.Title_Background = GUI3.GetTexture2D(Color.black, 100f);
		this.Icon_Background = GUI3.GetTexture2D(Color.black, 60f);
		this.List_Background = GUI3.GetTexture2D(Color.black, 75f);
		this.New_Select_Teams_Icons_Background[0] = GUI3.GetTexture2D(Color.blue, 60f);
		this.New_Select_Teams_Icons_Background[1] = GUI3.GetTexture2D(Color.red, 60f);
		this.New_Select_Teams_Icons_Background[2] = GUI3.GetTexture2D(Color.green, 60f);
		this.New_Select_Teams_Icons_Background[3] = GUI3.GetTexture2D(Color.yellow, 60f);
		this.yellow_tex = GUI3.GetTexture2D(Color.yellow, 50f);
		this.New_Select_Teams_Icons[0] = (Resources.Load("NewMenu/TeamSelect/team_blue") as Texture);
		this.New_Select_Teams_Icons[1] = (Resources.Load("NewMenu/TeamSelect/team_red") as Texture);
		this.New_Select_Teams_Icons[2] = (Resources.Load("NewMenu/TeamSelect/team_green") as Texture);
		this.New_Select_Teams_Icons[3] = (Resources.Load("NewMenu/TeamSelect/team_yellow") as Texture);
		this.New_Select_Teams_Icons[4] = (Resources.Load("NewMenu/TeamSelect/team_zombie") as Texture);
		this.New_Select_Teams_Icons_Tab[0] = GUI3.GetTexture2D(Color.blue, 100f);
		this.New_Select_Teams_Icons_Tab[1] = GUI3.GetTexture2D(Color.red, 100f);
		this.New_Select_Teams_Icons_Tab[2] = GUI3.GetTexture2D(Color.green, 100f);
		this.New_Select_Teams_Icons_Tab[3] = GUI3.GetTexture2D(Color.yellow, 100f);
		this.New_Select_Teams_Names[0] = Lang.GetLabel(400);
		this.New_Select_Teams_Names[1] = Lang.GetLabel(401);
		this.New_Select_Teams_Names[2] = Lang.GetLabel(402);
		this.New_Select_Teams_Names[3] = Lang.GetLabel(403);
		this.Play_Button_Normal = (Resources.Load("NewMenu/Play_Button_Normal") as Texture);
		this.Play_Button_Hover = (Resources.Load("NewMenu/Play_Button_Hover") as Texture);
		this.New_Select_Teams = new Rect[4];
		this.New_Select_Teams_List = new Rect[4];
		this.Map = GameObject.Find("Map");
		this.gui_style = new GUIStyle();
		this.gui_style.font = ContentLoader.LoadFont("E_Menu_Font4");
		this.gui_style.normal.textColor = Color.white;
		this.MGUI = (MainGUI)Object.FindObjectOfType(typeof(MainGUI));
		this.tex_premium = (Resources.Load("GUI/premium_game") as Texture2D);
		this.sorted = new New_Select_Team.CSortedPlayer[32];
		for (int i = 0; i < 32; i++)
		{
			this.sorted[i] = new New_Select_Team.CSortedPlayer();
		}
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x0000248C File Offset: 0x0000068C
	private void Update()
	{
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x000530D0 File Offset: 0x000512D0
	public void Draw_New_Select_Team()
	{
		if (this.tc == null)
		{
			this.tc = (TankController)Object.FindObjectOfType(typeof(TankController));
		}
		if (this.tc != null && this.tc.enabled)
		{
			return;
		}
		if (this.cc == null)
		{
			this.cc = (CarController)Object.FindObjectOfType(typeof(CarController));
		}
		if (this.cc != null && this.cc.enabled)
		{
			return;
		}
		if (CONST.GetGameMode() == MODE.BUILD || CONST.GetGameMode() == MODE.CLEAR)
		{
			return;
		}
		int num;
		if (CONST.GetGameMode() == MODE.SURVIVAL || CONST.GetGameMode() == MODE.ZOMBIE || CONST.GetGameMode() == MODE.FFA)
		{
			num = 2;
		}
		else if (CONST.GetGameMode() == MODE.BATTLE || CONST.GetGameMode() == MODE.CAPTURE || CONST.GetGameMode() == MODE.CLASSIC)
		{
			num = 0;
		}
		else
		{
			num = 1;
		}
		this.x = Screen.width / 2 - 300;
		this.y = Screen.height / 2 - (Screen.height - 220) / 2;
		this.Play_Button_Rect = new Rect((float)(Screen.width / 2 - 86), 50f, 172f, 42f);
		if (num == 2)
		{
			this.Title_Rect = new Rect((float)this.x, (float)(this.y - 42), 600f, 22f);
		}
		else
		{
			this.Title_Rect = new Rect((float)this.x, (float)this.y, 600f, 22f);
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
		if (num != 2)
		{
			this.Draw_Play_Button();
		}
		GUI.DrawTexture(this.Title_Rect, this.Title_Background);
		string label = Lang.GetLabel(404);
		if (num == 2)
		{
			label = Lang.GetLabel(405);
		}
		this.gui_style.fontSize = 18;
		this.gui_style.alignment = TextAnchor.MiddleCenter;
		this.gui_style.normal.textColor = Color.black;
		this.gui_style.padding.bottom = 0;
		GUI.Label(new Rect(this.Title_Rect.x + 1f, this.Title_Rect.y + 1f, this.Title_Rect.width, 22f), label, this.gui_style);
		this.gui_style.normal.textColor = Color.yellow;
		GUI.Label(this.Title_Rect, label, this.gui_style);
		if (num == 0)
		{
			this.Draw_Battle();
		}
		else if (num == 1)
		{
			this.Draw_Contra();
		}
		else if (num == 2)
		{
			this.Draw_Zombie();
		}
		if (this.last_time < Time.time)
		{
			this.SortPlayers();
			this.last_time += 1f;
		}
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x000533C0 File Offset: 0x000515C0
	private void Draw_New_Select_Zombie_Tab(int i)
	{
		GUI.DrawTexture(this.New_Select_Teams[i], this.New_Select_Teams_Icons_Background[i]);
		if (i == 0)
		{
			GUI.DrawTexture(this.New_Select_Teams[i], this.New_Select_Teams_Icons[i]);
		}
		else
		{
			GUI.DrawTexture(this.New_Select_Teams[i], this.New_Select_Teams_Icons[4]);
		}
		GUI.DrawTexture(new Rect(this.New_Select_Teams[i].x, this.New_Select_Teams[i].yMax - 20f, this.New_Select_Teams[i].width, 20f), this.New_Select_Teams_Icons_Tab[i]);
		GUI.DrawTexture(this.New_Select_Teams_List[i], this.List_Background);
		this.gui_style.fontSize = 14;
		this.gui_style.alignment = TextAnchor.MiddleCenter;
		this.gui_style.normal.textColor = Color.black;
		this.gui_style.padding.bottom = 0;
		this.New_Select_Teams_Names[0] = Lang.GetLabel(406);
		if (CONST.GetGameMode() == MODE.SURVIVAL)
		{
			this.New_Select_Teams_Names[1] = Lang.GetLabel(407);
		}
		else
		{
			this.New_Select_Teams_Names[1] = Lang.GetLabel(408);
		}
		GUI.Label(new Rect(this.New_Select_Teams[i].x + 1f, this.New_Select_Teams[i].yMax - 20f + 1f, this.New_Select_Teams[i].width, 20f), this.New_Select_Teams_Names[i], this.gui_style);
		this.gui_style.normal.textColor = Color.white;
		GUI.Label(new Rect(this.New_Select_Teams[i].x, this.New_Select_Teams[i].yMax - 20f, this.New_Select_Teams[i].width, 20f), this.New_Select_Teams_Names[i], this.gui_style);
		int num = (int)this.New_Select_Teams_List[i].x + 5;
		int num2 = (int)this.New_Select_Teams_List[i].y + 5;
		this.gui_style.alignment = TextAnchor.MiddleLeft;
		this.DrawPlayers(num, num2, i, 20);
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x00053614 File Offset: 0x00051814
	private void Draw_Tab(int i)
	{
		if (this.New_Select_Teams[i].Contains(this.mousePos) || this.New_Select_Teams_List[i].Contains(this.mousePos))
		{
			GUI.DrawTexture(this.New_Select_Teams[i], this.New_Select_Teams_Icons_Background[i]);
		}
		else
		{
			GUI.DrawTexture(this.New_Select_Teams[i], this.Icon_Background);
		}
		GUI.DrawTexture(this.New_Select_Teams[i], this.New_Select_Teams_Icons[i]);
		GUI.DrawTexture(new Rect(this.New_Select_Teams[i].x, this.New_Select_Teams[i].yMax - 20f, this.New_Select_Teams[i].width, 20f), this.New_Select_Teams_Icons_Tab[i]);
		GUI.DrawTexture(this.New_Select_Teams_List[i], this.List_Background);
		this.gui_style.fontSize = 14;
		this.gui_style.alignment = TextAnchor.MiddleCenter;
		this.gui_style.normal.textColor = Color.black;
		this.gui_style.padding.bottom = 0;
		GUI.Label(new Rect(this.New_Select_Teams[i].x + 1f, this.New_Select_Teams[i].yMax - 20f + 1f, this.New_Select_Teams[i].width, 20f), this.New_Select_Teams_Names[i], this.gui_style);
		this.gui_style.normal.textColor = Color.white;
		GUI.Label(new Rect(this.New_Select_Teams[i].x, this.New_Select_Teams[i].yMax - 20f, this.New_Select_Teams[i].width, 20f), this.New_Select_Teams_Names[i], this.gui_style);
		int num = (int)this.New_Select_Teams_List[i].x + 5;
		int num2 = (int)this.New_Select_Teams_List[i].y + 5;
		this.gui_style.alignment = TextAnchor.MiddleLeft;
		this.DrawPlayers(num, num2, i, 20);
		if (GUI.Button(this.New_Select_Teams[i], "", this.gui_style) || GUI.Button(this.New_Select_Teams_List[i], "", this.gui_style))
		{
			if (this.cscl == null)
			{
				this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
			}
			this.cscl.send_jointeamclass((byte)i, 0);
		}
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x000538C4 File Offset: 0x00051AC4
	private void Draw_Battle()
	{
		this.New_Select_Teams[0] = new Rect((float)this.x, (float)(this.y + 22), 150f, 170f);
		this.New_Select_Teams[1] = new Rect((float)(this.x + 150), (float)(this.y + 22), 150f, 170f);
		this.New_Select_Teams[2] = new Rect((float)(this.x + 300), (float)(this.y + 22), 150f, 170f);
		this.New_Select_Teams[3] = new Rect((float)(this.x + 450), (float)(this.y + 22), 150f, 170f);
		this.New_Select_Teams_List[0] = new Rect((float)this.x, (float)(this.y + 22 + 170), 150f, 170f);
		this.New_Select_Teams_List[1] = new Rect((float)(this.x + 150), (float)(this.y + 22 + 170), 150f, 170f);
		this.New_Select_Teams_List[2] = new Rect((float)(this.x + 300), (float)(this.y + 22 + 170), 150f, 170f);
		this.New_Select_Teams_List[3] = new Rect((float)(this.x + 450), (float)(this.y + 22 + 170), 150f, 170f);
		for (int i = 0; i < 4; i++)
		{
			this.Draw_Tab(i);
			if (i == 0 || i == 1 || i == 2)
			{
				this.EM.Draw_Borders(this.New_Select_Teams[i], true, false, false, true);
				this.EM.Draw_Borders(this.New_Select_Teams_List[i], true, false, false, true);
			}
			if (i == 3)
			{
				this.EM.Draw_Borders(this.New_Select_Teams[i], true, false, true, true);
				this.EM.Draw_Borders(this.New_Select_Teams_List[i], true, false, false, true);
			}
		}
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x00053B04 File Offset: 0x00051D04
	private void Draw_Contra()
	{
		this.New_Select_Teams[0] = new Rect((float)this.x, (float)(this.y + 22), 150f, 170f);
		this.New_Select_Teams[1] = new Rect((float)(this.x + 450), (float)(this.y + 22), 150f, 170f);
		this.New_Select_Teams[2] = new Rect(0f, 0f, 0f, 0f);
		this.New_Select_Teams[3] = new Rect(0f, 0f, 0f, 0f);
		this.New_Select_Teams_List[0] = new Rect((float)(this.x + 150), (float)(this.y + 22), 150f, 330f);
		this.New_Select_Teams_List[1] = new Rect((float)(this.x + 300), (float)(this.y + 22), 150f, 330f);
		this.New_Select_Teams_List[2] = new Rect(0f, 0f, 0f, 0f);
		this.New_Select_Teams_List[3] = new Rect(0f, 0f, 0f, 0f);
		for (int i = 0; i < 2; i++)
		{
			this.Draw_Tab(i);
			if (i == 0)
			{
				this.EM.Draw_Borders(this.New_Select_Teams[i], true, false, false, true);
				this.EM.Draw_Borders(this.New_Select_Teams_List[i], true, false, false, true);
			}
			if (i == 1)
			{
				this.EM.Draw_Borders(this.New_Select_Teams[i], false, false, true, true);
				this.EM.Draw_Borders(this.New_Select_Teams_List[i], true, false, true, true);
			}
		}
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x00053CF0 File Offset: 0x00051EF0
	private void Draw_Zombie()
	{
		this.New_Select_Teams[0] = new Rect((float)this.x, (float)(this.y + 22 - 42), 150f, 170f);
		this.New_Select_Teams[1] = new Rect((float)(this.x + 450), (float)(this.y + 22 - 42), 150f, 170f);
		this.New_Select_Teams[2] = new Rect(0f, 0f, 0f, 0f);
		this.New_Select_Teams[3] = new Rect(0f, 0f, 0f, 0f);
		this.New_Select_Teams_List[0] = new Rect((float)(this.x + 150), (float)(this.y + 22 - 42), 150f, 650f);
		this.New_Select_Teams_List[1] = new Rect((float)(this.x + 300), (float)(this.y + 22 - 42), 150f, 650f);
		this.New_Select_Teams_List[2] = new Rect(0f, 0f, 0f, 0f);
		this.New_Select_Teams_List[3] = new Rect(0f, 0f, 0f, 0f);
		this.Draw_New_Select_Zombie_Tab(0);
		this.Draw_New_Select_Zombie_Tab(1);
		this.EM.Draw_Borders(this.New_Select_Teams[0], true, false, false, true);
		this.EM.Draw_Borders(this.New_Select_Teams_List[0], true, false, false, true);
		this.EM.Draw_Borders(this.New_Select_Teams[1], false, false, true, true);
		this.EM.Draw_Borders(this.New_Select_Teams_List[1], true, false, true, true);
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x00053EDC File Offset: 0x000520DC
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
		GUI.Label(new Rect(this.Play_Button_Rect.x + 2f, this.Play_Button_Rect.y + 2f, this.Play_Button_Rect.width, this.Play_Button_Rect.height), Lang.GetLabel(409), this.gui_style);
		this.gui_style.normal.textColor = Color.white;
		GUI.Label(this.Play_Button_Rect, Lang.GetLabel(409), this.gui_style);
		if (GUI.Button(this.Play_Button_Rect, "", this.gui_style))
		{
			if (this.cscl == null)
			{
				this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
			}
			this.cscl.send_auto_jointeamclass();
		}
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x00054024 File Offset: 0x00052224
	private void DrawPlayers(int x, int y, int squadid, int offset = 20)
	{
		if (this.cscl == null)
		{
			this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		for (int i = 0; i < this.sortedCount; i++)
		{
			int id = this.sorted[i].id;
			if ((int)RemotePlayersUpdater.Instance.Bots[id].Team == squadid)
			{
				this.gui_style.fontSize = 12;
				this.gui_style.alignment = TextAnchor.MiddleLeft;
				if (RemotePlayersUpdater.Instance.Bots[id].Dead > 0)
				{
					this.gui_style.normal.textColor = Color.gray;
				}
				else
				{
					this.gui_style.normal.textColor = Color.white;
				}
				if (RemotePlayersUpdater.Instance.Bots[id].Item[6] == 1)
				{
					GUI.DrawTexture(new Rect((float)(x - 5), (float)y, 150f, (float)offset), this.yellow_tex);
					GUI.DrawTexture(new Rect((float)(x + 140 - 16), (float)y, 16f, 16f), this.tex_premium);
				}
				string text = RemotePlayersUpdater.Instance.Bots[id].Name;
				if (text.Length > 12)
				{
					text = text.Substring(0, 12);
				}
				if (id == this.cscl.myindex)
				{
					if (RemotePlayersUpdater.Instance.Bots[id].Item[6] == 0)
					{
						GUI.DrawTexture(new Rect((float)(x - 5), (float)y, 150f, (float)offset), this.yellow_tex);
					}
					GUI.Label(new Rect((float)(x + 18), (float)y, 102f, (float)offset), text, this.gui_style);
				}
				else
				{
					GUI.Label(new Rect((float)(x + 18), (float)y, 102f, (float)offset), text, this.gui_style);
				}
				if ((int)RemotePlayersUpdater.Instance.Bots[id].CountryID < GUIManager.tex_flags.Length && !(GUIManager.tex_flags[(int)RemotePlayersUpdater.Instance.Bots[id].CountryID] == null))
				{
					GUI.DrawTexture(new Rect((float)x, (float)(y + 5), 16f, 12f), GUIManager.tex_flags[(int)RemotePlayersUpdater.Instance.Bots[id].CountryID]);
				}
				y += offset;
			}
		}
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x00054264 File Offset: 0x00052464
	private void SortPlayers()
	{
		this.sortedCount = 0;
		for (int i = 0; i < 32; i++)
		{
			if (RemotePlayersUpdater.Instance.Bots[i].Active)
			{
				this.sorted[this.sortedCount].id = i;
				this.sorted[this.sortedCount].frags = RemotePlayersUpdater.Instance.Bots[i].Stats_Kills;
				this.sortedCount++;
			}
		}
		if (this.sortedCount <= 1)
		{
			return;
		}
		bool flag = false;
		while (!flag)
		{
			flag = true;
			for (int j = 1; j < this.sortedCount; j++)
			{
				if (this.sorted[j].frags > this.sorted[j - 1].frags)
				{
					int id = this.sorted[j - 1].id;
					int frags = this.sorted[j - 1].frags;
					this.sorted[j - 1].id = this.sorted[j].id;
					this.sorted[j - 1].frags = this.sorted[j].frags;
					this.sorted[j].id = id;
					this.sorted[j].frags = frags;
					flag = false;
				}
			}
		}
	}

	// Token: 0x040008F5 RID: 2293
	private E_Menu EM;

	// Token: 0x040008F6 RID: 2294
	public Texture yellow_tex;

	// Token: 0x040008F7 RID: 2295
	public Texture Title_Background;

	// Token: 0x040008F8 RID: 2296
	public Texture Icon_Background;

	// Token: 0x040008F9 RID: 2297
	public Texture List_Background;

	// Token: 0x040008FA RID: 2298
	public Texture[] New_Select_Teams_Icons_Background;

	// Token: 0x040008FB RID: 2299
	public Texture[] New_Select_Teams_Icons;

	// Token: 0x040008FC RID: 2300
	public Texture[] New_Select_Teams_Icons_Tab;

	// Token: 0x040008FD RID: 2301
	public string[] New_Select_Teams_Names;

	// Token: 0x040008FE RID: 2302
	public Rect Title_Rect;

	// Token: 0x040008FF RID: 2303
	public Rect Icon_Rect;

	// Token: 0x04000900 RID: 2304
	public Rect[] New_Select_Teams;

	// Token: 0x04000901 RID: 2305
	public Rect[] New_Select_Teams_List;

	// Token: 0x04000902 RID: 2306
	public Texture Play_Button_Normal;

	// Token: 0x04000903 RID: 2307
	public Texture Play_Button_Hover;

	// Token: 0x04000904 RID: 2308
	public Texture2D Border;

	// Token: 0x04000905 RID: 2309
	public Rect Play_Button_Rect;

	// Token: 0x04000906 RID: 2310
	private WeaponSystem csws;

	// Token: 0x04000907 RID: 2311
	private PlayerControl cspc;

	// Token: 0x04000908 RID: 2312
	private Client cscl;

	// Token: 0x04000909 RID: 2313
	private GameObject Map;

	// Token: 0x0400090A RID: 2314
	private MainGUI MGUI;

	// Token: 0x0400090B RID: 2315
	private Texture2D tex_premium;

	// Token: 0x0400090C RID: 2316
	private GUIStyle gui_style;

	// Token: 0x0400090D RID: 2317
	private New_Select_Team.CSortedPlayer[] sorted;

	// Token: 0x0400090E RID: 2318
	private int sortedCount;

	// Token: 0x0400090F RID: 2319
	private float showtime;

	// Token: 0x04000910 RID: 2320
	private TankController tc;

	// Token: 0x04000911 RID: 2321
	private CarController cc;

	// Token: 0x04000912 RID: 2322
	private bool can_team_select;

	// Token: 0x04000913 RID: 2323
	private int x;

	// Token: 0x04000914 RID: 2324
	private int y;

	// Token: 0x04000915 RID: 2325
	private float last_time;

	// Token: 0x04000916 RID: 2326
	private bool In_Play_Button_Rect;

	// Token: 0x04000917 RID: 2327
	private Vector2 mousePos;

	// Token: 0x02000871 RID: 2161
	public class CSortedPlayer
	{
		// Token: 0x040032DC RID: 13020
		public int id;

		// Token: 0x040032DD RID: 13021
		public int frags;
	}
}
