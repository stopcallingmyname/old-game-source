using System;
using UnityEngine;

// Token: 0x0200008A RID: 138
public class SelectTeam : MonoBehaviour
{
	// Token: 0x060003E3 RID: 995 RVA: 0x0004C2BC File Offset: 0x0004A4BC
	public void OpenMenu()
	{
		if (CONST.GetGameMode() == MODE.SURVIVAL || CONST.GetGameMode() == MODE.CLEAR || CONST.GetGameMode() == MODE.FFA)
		{
			return;
		}
		this.SortPlayers();
		this.show = true;
		this.showtime = Time.time;
		this.show_teamselect = true;
		if (this.csws == null)
		{
			this.csws = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		MainGUI.ForceCursor = true;
		this.csws.HideWeapons(true);
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x0004C33D File Offset: 0x0004A53D
	public void CloseMenu()
	{
		this.show = false;
		MainGUI.ForceCursor = this.show;
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x0004C354 File Offset: 0x0004A554
	private void Awake()
	{
		this.Map = GameObject.Find("Map");
		this.gui_style = new GUIStyle();
		this.gui_style.font = FontManager.font[2];
		this.gui_style.fontSize = 16;
		this.gui_style.normal.textColor = new Color(255f, 255f, 255f, 255f);
		this.tex_tabmenu = (Resources.Load("GUI/tabmenu") as Texture2D);
		this.tex_tabmenu2 = (Resources.Load("GUI/tabmenu2") as Texture2D);
		this.tex_tabmenu3 = (Resources.Load("GUI/tabmenuz") as Texture2D);
		this.tex_tabmenu2team = (Resources.Load("GUI/tabmenucs") as Texture2D);
		if (Lang.current == 1)
		{
			this.tex_class_mp5 = (Resources.Load("GUI/eng/assault") as Texture2D);
			this.tex_class_m3 = (Resources.Load("GUI/eng/support") as Texture2D);
			this.tex_class_m14 = (Resources.Load("GUI/eng/sniper") as Texture2D);
		}
		else
		{
			this.tex_class_mp5 = (Resources.Load("GUI/rus/assault") as Texture2D);
			this.tex_class_m3 = (Resources.Load("GUI/rus/support") as Texture2D);
			this.tex_class_m14 = (Resources.Load("GUI/rus/sniper") as Texture2D);
		}
		this.blueTexture = new Texture2D(1, 1);
		this.blueTexture.SetPixel(0, 0, new Color(0.03f, 0.23f, 0.76f, 1f));
		this.blueTexture.Apply();
		this.redTexture = new Texture2D(1, 1);
		this.redTexture.SetPixel(0, 0, new Color(0.65f, 0.18f, 0.14f, 1f));
		this.redTexture.Apply();
		this.greenTexture = new Texture2D(1, 1);
		this.greenTexture.SetPixel(0, 0, new Color(0.27f, 0.78f, 0.09f, 1f));
		this.greenTexture.Apply();
		this.yellowTexture = new Texture2D(1, 1);
		this.yellowTexture.SetPixel(0, 0, new Color(0.87f, 0.85f, 0f, 1f));
		this.yellowTexture.Apply();
		this.tex_premium = (Resources.Load("GUI/premium_game") as Texture2D);
		this.sorted = new SelectTeam.CSortedPlayer[32];
		for (int i = 0; i < 32; i++)
		{
			this.sorted[i] = new SelectTeam.CSortedPlayer();
		}
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x0004C5D4 File Offset: 0x0004A7D4
	private void Update()
	{
		if (this.tc == null)
		{
			this.tc = (TankController)Object.FindObjectOfType(typeof(TankController));
		}
		if (this.cc == null)
		{
			this.cc = (CarController)Object.FindObjectOfType(typeof(CarController));
		}
		if (this.tc != null && this.cc != null)
		{
			if (this.tc.enabled || this.cc.enabled)
			{
				this.can_team_select = false;
			}
			else
			{
				this.can_team_select = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			this.SortPlayers();
			this.show = true;
			this.show_teamselect = false;
		}
		if (Input.GetKeyUp(KeyCode.Tab))
		{
			this.show = false;
		}
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x0004C6A4 File Offset: 0x0004A8A4
	private void OnGUI()
	{
		if (this.show)
		{
			if (this.cspc == null)
			{
				this.cspc = (PlayerControl)Object.FindObjectOfType(typeof(PlayerControl));
			}
			if (this.sortedCount == 0)
			{
				this.SortPlayers();
			}
			if (Time.time > this.showtime + 2f)
			{
				this.showtime = Time.time;
				this.SortPlayers();
			}
			if (CONST.GetGameMode() == MODE.BATTLE || CONST.GetGameMode() == MODE.CAPTURE || CONST.GetGameMode() == MODE.CLASSIC)
			{
				this.r = new Rect(0f, 0f, 640f, 420f);
				this.r.center = new Vector2((float)Screen.width, (float)Screen.height) / 2f;
				GUI.Window(900, this.r, new GUI.WindowFunction(this.TabBattle), "", this.gui_style);
				return;
			}
			if (CONST.GetGameMode() == MODE.BUILD || CONST.GetGameMode() == MODE.SURVIVAL || CONST.GetGameMode() == MODE.CLEAR || CONST.GetGameMode() == MODE.FFA)
			{
				this.r = new Rect(0f, 0f, 320f, 671f);
				this.r.center = new Vector2((float)Screen.width, (float)Screen.height) / 2f;
				GUI.Window(900, this.r, new GUI.WindowFunction(this.TabBuild), "", this.gui_style);
				return;
			}
			if (CONST.GetGameMode() == MODE.ZOMBIE)
			{
				this.r = new Rect(0f, 0f, 640f, 620f);
				this.r.center = new Vector2((float)Screen.width, (float)Screen.height) / 2f;
				GUI.Window(900, this.r, new GUI.WindowFunction(this.TabZombie), "", this.gui_style);
				return;
			}
			if (CONST.GetGameMode() == MODE.CONTRA || CONST.GetGameMode() == MODE.SNOWBALLS || CONST.GetGameMode() == MODE.M1945 || CONST.GetGameMode() == MODE.PRORIV || CONST.GetGameMode() == MODE.TANK)
			{
				this.r = new Rect(0f, 0f, 640f, 420f);
				this.r.center = new Vector2((float)Screen.width, (float)Screen.height) / 2f;
				GUI.Window(900, this.r, new GUI.WindowFunction(this.TabContra), "", this.gui_style);
				return;
			}
			if (CONST.GetGameMode() == MODE.MELEE)
			{
				this.r = new Rect(0f, 0f, 640f, 420f);
				this.r.center = new Vector2((float)Screen.width, (float)Screen.height) / 2f;
				GUI.Window(900, this.r, new GUI.WindowFunction(this.TabContra), "", this.gui_style);
			}
		}
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x0004C9B0 File Offset: 0x0004ABB0
	private void TabBattle(int windowID)
	{
		GUI.DrawTexture(new Rect(0f, 0f, 640f, 420f), this.tex_tabmenu);
		GUIManager.DrawText(new Rect(0f, 0f, 600f, 28f), ConnectionInfo.HOSTNAME, 16, TextAnchor.MiddleCenter, 8);
		this.DrawSquad(1, 30, Lang.GetLabel(146), 0, false);
		this.DrawSquad(320, 30, Lang.GetLabel(147), 1, false);
		this.DrawSquad(1, 224, Lang.GetLabel(148), 2, false);
		this.DrawSquad(320, 224, Lang.GetLabel(149), 3, false);
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x0004CA6C File Offset: 0x0004AC6C
	private void TabBuild(int windowID)
	{
		GUI.DrawTexture(new Rect(0f, 0f, 320f, 600f), this.tex_tabmenu2);
		GUIManager.DrawText(new Rect(0f, 0f, 320f, 28f), ConnectionInfo.HOSTNAME, 16, TextAnchor.MiddleCenter, 8);
		this.DrawPlayers(1, 30, 0, false, 20);
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x0004CAD4 File Offset: 0x0004ACD4
	private void TabZombie(int windowID)
	{
		GUI.DrawTexture(new Rect(0f, 0f, 640f, 620f), this.tex_tabmenu3);
		GUIManager.DrawText(new Rect(0f, 0f, 640f, 28f), ConnectionInfo.HOSTNAME, 16, TextAnchor.MiddleCenter, 8);
		GUIManager.DrawText(new Rect(1f, 30f, 318f, 22f), Lang.GetLabel(150), 16, TextAnchor.MiddleCenter, 8);
		GUIManager.DrawText(new Rect(320f, 30f, 318f, 22f), Lang.GetLabel(50), 16, TextAnchor.MiddleCenter, 8);
		this.DrawPlayers(1, 58, 0, false, 17);
		this.DrawPlayers(320, 58, 1, false, 17);
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x0004CBA0 File Offset: 0x0004ADA0
	private void TabContra(int windowID)
	{
		GUI.DrawTexture(new Rect(0f, 0f, 640f, 420f), this.tex_tabmenu2team);
		GUIManager.DrawText(new Rect(0f, 0f, 600f, 28f), ConnectionInfo.HOSTNAME, 16, TextAnchor.MiddleCenter, 8);
		this.DrawSquad(1, 30, Lang.GetLabel(146), 0, false);
		this.DrawSquad(320, 30, Lang.GetLabel(147), 1, false);
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0004CC28 File Offset: 0x0004AE28
	private void DrawSquad(int x, int y, string text, byte squadid, bool by_admin = false)
	{
		this.x_pos = x;
		this.y_pos = y;
		GUIManager.DrawText(new Rect((float)x, (float)y, 318f, 22f), text, 16, TextAnchor.MiddleCenter, 8);
		this.y_pos += 28;
		this.DrawPlayers(this.x_pos, this.y_pos, (int)squadid, by_admin, 20);
		if (this.show_teamselect)
		{
			this.DrawClasses(squadid);
		}
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x0004CC98 File Offset: 0x0004AE98
	private void DrawClasses(byte squadid)
	{
		Texture2D image;
		if (squadid == 0)
		{
			image = this.blueTexture;
		}
		else if (squadid == 1)
		{
			image = this.redTexture;
		}
		else if (squadid == 2)
		{
			image = this.greenTexture;
		}
		else if (squadid == 3)
		{
			image = this.yellowTexture;
		}
		else
		{
			image = this.blueTexture;
		}
		int num = 29;
		int num2 = 12;
		float num3 = Input.mousePosition.x;
		float num4 = (float)Screen.height - Input.mousePosition.y;
		num3 -= (float)(Screen.width - 600) / 2f;
		num4 -= (float)(Screen.height - 400) / 2f;
		Rect position = new Rect((float)(this.x_pos + num), (float)(this.y_pos + num2), 84f, 120f);
		Rect position2 = new Rect((float)(this.x_pos + num + 84 + 2), (float)(this.y_pos + num2), 84f, 120f);
		Rect position3 = new Rect((float)(this.x_pos + num + 168 + 4), (float)(this.y_pos + num2), 84f, 120f);
		Rect position4 = new Rect((float)(this.x_pos + num), (float)(this.y_pos + num2 + 120 + 4), 256f, 18f);
		Rect position5 = new Rect((float)(this.x_pos + num + 1), (float)(this.y_pos + num2 + 1), 82f, 18f);
		Rect position6 = new Rect((float)(this.x_pos + num + 84 + 2 + 1), (float)(this.y_pos + num2 + 1), 82f, 18f);
		Rect position7 = new Rect((float)(this.x_pos + num + 168 + 4 + 1), (float)(this.y_pos + num2 + 1), 82f, 18f);
		Rect position8 = new Rect((float)(this.x_pos + num + 1), (float)(this.y_pos + num2 + 120 + 4 + 1), 254f, 16f);
		if (position.Contains(new Vector2(num3, num4)))
		{
			if (!this.hover[0])
			{
				this.hover[0] = true;
			}
		}
		else if (this.hover[0])
		{
			this.hover[0] = false;
		}
		if (position2.Contains(new Vector2(num3, num4)))
		{
			if (!this.hover[1])
			{
				this.hover[1] = true;
			}
		}
		else if (this.hover[1])
		{
			this.hover[1] = false;
		}
		if (position3.Contains(new Vector2(num3, num4)))
		{
			if (!this.hover[2])
			{
				this.hover[2] = true;
			}
		}
		else if (this.hover[2])
		{
			this.hover[2] = false;
		}
		if (position4.Contains(new Vector2(num3, num4)))
		{
			if (!this.hover[3])
			{
				this.hover[3] = true;
			}
		}
		else if (this.hover[3])
		{
			this.hover[3] = false;
		}
		GUI.DrawTexture(position5, image);
		GUI.DrawTexture(position6, image);
		GUI.DrawTexture(position7, image);
		GUI.DrawTexture(position8, image);
		if (this.hover[0])
		{
			GUI.DrawTexture(position, image);
		}
		else if (this.hover[1])
		{
			GUI.DrawTexture(position2, image);
		}
		else if (this.hover[2])
		{
			GUI.DrawTexture(position3, image);
		}
		else if (this.hover[3])
		{
			GUI.DrawTexture(position4, image);
		}
		GUI.DrawTexture(new Rect((float)(this.x_pos + num), (float)(this.y_pos + num2), 128f, 128f), this.tex_class_mp5);
		GUI.DrawTexture(new Rect((float)(this.x_pos + num + 84 + 2), (float)(this.y_pos + num2), 128f, 128f), this.tex_class_m3);
		GUI.DrawTexture(new Rect((float)(this.x_pos + num + 168 + 4), (float)(this.y_pos + num2), 128f, 128f), this.tex_class_m14);
		if (this.cscl == null)
		{
			this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		if (GUI.Button(position, "", this.gui_style))
		{
			this.cscl.send_jointeamclass(squadid, 0);
		}
		if (GUI.Button(position2, "", this.gui_style))
		{
			this.cscl.send_jointeamclass(squadid, 1);
		}
		if (GUI.Button(position3, "", this.gui_style))
		{
			this.cscl.send_jointeamclass(squadid, 2);
		}
		this.gui_style.alignment = TextAnchor.UpperCenter;
		if (GUI.Button(position4, Lang.GetLabel(221), this.gui_style))
		{
			this.cscl.send_last_config(squadid);
		}
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x0004D118 File Offset: 0x0004B318
	private void DrawPlayers(int x, int y, int squadid, bool by_admin = false, int offset = 20)
	{
		if (this.cscl == null)
		{
			this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		for (int i = 0; i < this.sortedCount; i++)
		{
			int id = this.sorted[i].id;
			if (ConnectionInfo.mode == 2 || (int)RemotePlayersUpdater.Instance.Bots[id].Team == squadid)
			{
				float num = (float)(offset - 16) / 2f;
				if (RemotePlayersUpdater.Instance.Bots[id].Item[6] == 1)
				{
					GUI.color = new Color(1f, 1f, 1f, 0.1f);
					GUI.DrawTexture(new Rect((float)x, (float)y, 318f, (float)offset), GUIManager.tex_yellow);
					GUI.color = new Color(1f, 1f, 1f, 1f);
					GUI.DrawTexture(new Rect((float)(x + 160), (float)y + num, 16f, 16f), this.tex_premium);
				}
				if (id == this.cscl.myindex)
				{
					if (RemotePlayersUpdater.Instance.Bots[id].Item[6] == 0)
					{
						GUI.color = new Color(1f, 0f, 0f, 0.25f);
						GUI.DrawTexture(new Rect((float)x, (float)y, 318f, (float)offset), GUIManager.tex_yellow);
						GUI.color = new Color(1f, 1f, 1f, 1f);
					}
					Color gray = new Color(1f, 0.7f, 0f, 1f);
					if (RemotePlayersUpdater.Instance.Bots[id].Dead > 0)
					{
						gray = Color.gray;
					}
					GUIManager.DrawText2(new Rect((float)(x + 4 + 20), (float)y, 200f, (float)offset), RemotePlayersUpdater.Instance.Bots[id].Name, 14, TextAnchor.MiddleLeft, gray);
				}
				else
				{
					Color gray2 = new Color(1f, 1f, 1f, 1f);
					if (RemotePlayersUpdater.Instance.Bots[id].Dead > 0)
					{
						gray2 = Color.gray;
					}
					GUIManager.DrawText2(new Rect((float)(x + 4 + 20), (float)y, 200f, (float)offset), RemotePlayersUpdater.Instance.Bots[id].Name, 14, TextAnchor.MiddleLeft, gray2);
				}
				if ((int)RemotePlayersUpdater.Instance.Bots[id].CountryID < GUIManager.tex_flags.Length && !(GUIManager.tex_flags[(int)RemotePlayersUpdater.Instance.Bots[id].CountryID] == null))
				{
					GUI.DrawTexture(new Rect((float)(x + 4), (float)y + num + 2f, 16f, 12f), GUIManager.tex_flags[(int)RemotePlayersUpdater.Instance.Bots[id].CountryID]);
				}
				GUIManager.DrawText2(new Rect((float)(x + 180), (float)y, 200f, (float)offset), RemotePlayersUpdater.Instance.Bots[id].ClanName, 14, TextAnchor.MiddleLeft, new Color(1f, 1f, 0f, 1f));
				GUIManager.DrawText2(new Rect((float)(x + 235), (float)y, 60f, (float)offset), RemotePlayersUpdater.Instance.Bots[id].Stats_Kills.ToString(), 14, TextAnchor.MiddleCenter, new Color(0.49f, 0.74f, 1f, 1f));
				GUIManager.DrawText2(new Rect((float)(x + 265), (float)y, 60f, (float)offset), RemotePlayersUpdater.Instance.Bots[id].Stats_Deads.ToString(), 14, TextAnchor.MiddleCenter, new Color(0.56f, 0.56f, 0.56f, 1f));
				y += offset;
				GUIManager.gs_style2.normal.textColor = new Color(1f, 1f, 1f, 1f);
			}
		}
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0004D504 File Offset: 0x0004B704
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

	// Token: 0x0400086A RID: 2154
	private bool show;

	// Token: 0x0400086B RID: 2155
	private bool show_teamselect;

	// Token: 0x0400086C RID: 2156
	private WeaponSystem csws;

	// Token: 0x0400086D RID: 2157
	private PlayerControl cspc;

	// Token: 0x0400086E RID: 2158
	private Client cscl;

	// Token: 0x0400086F RID: 2159
	private GameObject Map;

	// Token: 0x04000870 RID: 2160
	private Rect r;

	// Token: 0x04000871 RID: 2161
	private Texture2D tex_tabmenu;

	// Token: 0x04000872 RID: 2162
	private Texture2D tex_tabmenu2;

	// Token: 0x04000873 RID: 2163
	private Texture2D tex_tabmenu3;

	// Token: 0x04000874 RID: 2164
	private Texture2D tex_class_mp5;

	// Token: 0x04000875 RID: 2165
	private Texture2D tex_class_m3;

	// Token: 0x04000876 RID: 2166
	private Texture2D tex_class_m14;

	// Token: 0x04000877 RID: 2167
	private Texture2D tex_tabmenu2team;

	// Token: 0x04000878 RID: 2168
	private Texture2D tex_premium;

	// Token: 0x04000879 RID: 2169
	private Texture2D tex_border;

	// Token: 0x0400087A RID: 2170
	private GUIStyle gui_style;

	// Token: 0x0400087B RID: 2171
	private int y_pos;

	// Token: 0x0400087C RID: 2172
	private int x_pos;

	// Token: 0x0400087D RID: 2173
	private bool[] hover = new bool[4];

	// Token: 0x0400087E RID: 2174
	private Texture2D redTexture;

	// Token: 0x0400087F RID: 2175
	private Texture2D blueTexture;

	// Token: 0x04000880 RID: 2176
	private Texture2D greenTexture;

	// Token: 0x04000881 RID: 2177
	private Texture2D yellowTexture;

	// Token: 0x04000882 RID: 2178
	private SelectTeam.CSortedPlayer[] sorted;

	// Token: 0x04000883 RID: 2179
	private int sortedCount;

	// Token: 0x04000884 RID: 2180
	private float showtime;

	// Token: 0x04000885 RID: 2181
	private TankController tc;

	// Token: 0x04000886 RID: 2182
	private CarController cc;

	// Token: 0x04000887 RID: 2183
	private bool can_team_select;

	// Token: 0x02000870 RID: 2160
	public class CSortedPlayer
	{
		// Token: 0x040032DA RID: 13018
		public int id;

		// Token: 0x040032DB RID: 13019
		public int frags;
	}
}
