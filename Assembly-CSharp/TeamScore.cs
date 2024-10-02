using System;
using UnityEngine;

// Token: 0x0200008C RID: 140
public class TeamScore : MonoBehaviour
{
	// Token: 0x060003F6 RID: 1014 RVA: 0x0004D6D4 File Offset: 0x0004B8D4
	private void Awake()
	{
		this.gui_style = new GUIStyle();
		this.gui_style.font = FontManager.font[2];
		this.gui_style.fontSize = 16;
		this.gui_style.normal.textColor = new Color(255f, 255f, 255f, 255f);
		this.gui_style.alignment = TextAnchor.UpperLeft;
		this.csradar = base.GetComponent<Radar>();
		this.tex_limit = (Resources.Load("GUI/limit") as Texture2D);
		this.tex_dot = (Resources.Load("GUI/ct/dot") as Texture2D);
		this.tex_score = (Resources.Load("GUI/ct/score") as Texture2D);
		this.tex_topbar = (Resources.Load("GUI/topbar") as Texture2D);
		this.tex_topbar2 = (Resources.Load("GUI/topbar2") as Texture2D);
		this.tex_topbar3 = (Resources.Load("GUI/topbar3") as Texture2D);
		this.tex_evacuation = (Resources.Load("GUI/evacuation") as Texture2D);
		this.Flags = new Texture2D[3];
		this.Flags[0] = (Resources.Load("GUI/flag_blue") as Texture2D);
		this.Flags[1] = (Resources.Load("GUI/flag_red") as Texture2D);
		this.Flags[2] = (Resources.Load("GUI/flag_black") as Texture2D);
		this.map = Object.FindObjectOfType<Map>();
		for (int i = 0; i < 4; i++)
		{
			this.c[i] = Color.white;
			this.oldFlagScores[i] = 150;
			this.mig[i] = false;
			this.score[i] = 0;
		}
		this.OnResize();
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x0004D880 File Offset: 0x0004BA80
	private void OnGUI()
	{
		this.g_mode = CONST.GetGameMode();
		if (this.g_mode == MODE.BATTLE || this.g_mode == MODE.CLASSIC)
		{
			this.DrawBattle();
			return;
		}
		if (this.g_mode == MODE.ZOMBIE)
		{
			this.DrawZombie();
			return;
		}
		if (this.g_mode == MODE.CAPTURE)
		{
			this.DrawCT();
			return;
		}
		if (this.g_mode == MODE.CONTRA || this.g_mode == MODE.TANK || this.g_mode == MODE.SNOWBALLS || this.g_mode == MODE.M1945)
		{
			this.DrawCS();
			return;
		}
		if (this.g_mode == MODE.MELEE)
		{
			this.DrawML();
			return;
		}
		if (this.g_mode == MODE.FFA)
		{
			this.DrawFFA();
			return;
		}
		if (this.g_mode == MODE.PRORIV)
		{
			this.DrawPR();
		}
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x0004D930 File Offset: 0x0004BB30
	public void OnResize()
	{
		this.x_pos = Screen.width - 60;
		this.y_pos = 12;
		this.battle_rect[0] = new Rect((float)(this.x_pos + 2), (float)(this.y_pos + 2), 10f, 10f);
		this.y_pos += 28;
		this.battle_rect[1] = new Rect((float)(this.x_pos + 2), (float)(this.y_pos + 2), 10f, 10f);
		this.y_pos += 28;
		this.battle_rect[2] = new Rect((float)(this.x_pos + 2), (float)(this.y_pos + 2), 10f, 10f);
		this.y_pos += 28;
		this.battle_rect[3] = new Rect((float)(this.x_pos + 2), (float)(this.y_pos + 2), 10f, 10f);
		this.y_pos += 28;
		this.y_pos -= 112;
		this.battle_rect[4] = new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f);
		this.y_pos += 28;
		this.battle_rect[5] = new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f);
		this.y_pos += 28;
		this.battle_rect[6] = new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f);
		this.y_pos += 28;
		this.battle_rect[7] = new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f);
		this.y_pos += 28;
		this.x_pos = Screen.width - 40;
		this.y_pos -= 110;
		this.battle_rect[8] = new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f);
		this.y_pos += 28;
		this.battle_rect[9] = new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f);
		this.y_pos += 28;
		this.battle_rect[10] = new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f);
		this.y_pos += 28;
		this.battle_rect[11] = new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f);
		this.y_pos += 28;
		this.x_pos = Screen.width - 60;
		this.y_pos = 36;
		this.proriv_rect[0] = new Rect((float)(this.x_pos + 2), (float)(this.y_pos + 2), 10f, 10f);
		this.y_pos += 28;
		this.proriv_rect[1] = new Rect((float)(this.x_pos + 2), (float)(this.y_pos + 2), 10f, 10f);
		this.y_pos += 28;
		this.proriv_rect[2] = new Rect((float)(this.x_pos + 2), (float)(this.y_pos + 2), 10f, 10f);
		this.y_pos += 28;
		this.proriv_rect[3] = new Rect((float)(this.x_pos + 2), (float)(this.y_pos + 2), 10f, 10f);
		this.y_pos += 28;
		this.proriv_rect[12] = new Rect((float)(this.x_pos - 20 + 2 - 3), (float)(this.y_pos + 2 - 3), 16f, 16f);
		this.y_pos -= 112;
		this.proriv_rect[4] = new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f);
		this.y_pos += 28;
		this.proriv_rect[5] = new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f);
		this.y_pos += 28;
		this.proriv_rect[6] = new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f);
		this.y_pos += 28;
		this.proriv_rect[7] = new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f);
		this.y_pos += 28;
		this.proriv_rect[13] = new Rect((float)(this.x_pos - 20 - 3), (float)(this.y_pos - 3), 16f, 16f);
		this.x_pos = Screen.width - 40;
		this.y_pos -= 110;
		this.proriv_rect[8] = new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f);
		this.y_pos += 28;
		this.proriv_rect[9] = new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f);
		this.y_pos += 28;
		this.proriv_rect[10] = new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f);
		this.y_pos += 28;
		this.proriv_rect[11] = new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f);
		this.y_pos += 28;
		this.proriv_rect[14] = new Rect((float)(this.x_pos - 20), (float)this.y_pos, 10f, 10f);
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x0004DFBC File Offset: 0x0004C1BC
	private void DrawBattle()
	{
		GUI.DrawTexture(this.battle_rect[0], GUIManager.tex_black);
		GUI.DrawTexture(this.battle_rect[1], GUIManager.tex_black);
		GUI.DrawTexture(this.battle_rect[2], GUIManager.tex_black);
		GUI.DrawTexture(this.battle_rect[3], GUIManager.tex_black);
		GUI.DrawTexture(this.battle_rect[4], GUIManager.tex_blue);
		GUI.DrawTexture(this.battle_rect[5], GUIManager.tex_red);
		GUI.DrawTexture(this.battle_rect[6], GUIManager.tex_green);
		GUI.DrawTexture(this.battle_rect[7], GUIManager.tex_yellow);
		GUIManager.DrawText(this.battle_rect[8], this.score[0].ToString(), 18, TextAnchor.MiddleLeft, 8);
		GUIManager.DrawText(this.battle_rect[9], this.score[1].ToString(), 18, TextAnchor.MiddleLeft, 8);
		GUIManager.DrawText(this.battle_rect[10], this.score[2].ToString(), 18, TextAnchor.MiddleLeft, 8);
		GUIManager.DrawText(this.battle_rect[11], this.score[3].ToString(), 18, TextAnchor.MiddleLeft, 8);
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x0004E114 File Offset: 0x0004C314
	private void DrawZombie()
	{
		this.x_pos = Screen.width - 60;
		this.y_pos = 12;
		this.DrawTimer();
		GUI.DrawTexture(new Rect((float)(this.x_pos + 2), (float)(this.y_pos + 2), 10f, 10f), GUIManager.tex_black);
		this.y_pos += 28;
		GUI.DrawTexture(new Rect((float)(this.x_pos + 2), (float)(this.y_pos + 2), 10f, 10f), GUIManager.tex_black);
		this.y_pos += 28;
		this.y_pos -= 56;
		GUI.DrawTexture(new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f), GUIManager.tex_blue);
		this.y_pos += 28;
		GUI.DrawTexture(new Rect((float)this.x_pos, (float)this.y_pos, 10f, 10f), GUIManager.tex_red);
		this.y_pos += 28;
		this.x_pos = Screen.width - 40;
		this.y_pos = 40;
		this.DrawText(this.score[0].ToString());
		this.y_pos += 28;
		this.DrawText(this.score[1].ToString());
		this.y_pos += 28;
		GUI.color = new Color(0f, 0f, 0f, 0.5f);
		GUI.DrawTexture(new Rect((float)(Screen.width - 62), (float)(this.y_pos - 1), 62f, 19f), GUIManager.tex_black);
		GUI.color = new Color(1f, 1f, 1f, 1f);
		GUI.DrawTexture(new Rect((float)(Screen.width - 60), (float)(this.y_pos + 1), 16f, 16f), this.tex_limit);
		this.DrawText("10");
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x0004E32C File Offset: 0x0004C52C
	private void DrawText(string text)
	{
		this.gui_style.normal.textColor = new Color(0f, 0f, 0f, 255f);
		GUI.Label(new Rect((float)(this.x_pos + 1), (float)(this.y_pos + 1), 294f, 20f), text, this.gui_style);
		this.gui_style.normal.textColor = new Color(1f, 1f, 1f, 1f);
		GUI.Label(new Rect((float)this.x_pos, (float)this.y_pos, 294f, 20f), text, this.gui_style);
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x0004E3E1 File Offset: 0x0004C5E1
	public void UpdateScore(int score1, int score2, int score3, int score4, int _timer)
	{
		this.score[0] = score1;
		this.score[1] = score2;
		this.score[2] = score3;
		this.score[3] = score4;
		this.SetTimer(_timer);
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x0004E410 File Offset: 0x0004C610
	public void SetTimer(int val)
	{
		this.timerset = Time.time;
		this.timer = val;
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x0004E424 File Offset: 0x0004C624
	private void DrawCT()
	{
		this.gui_style.alignment = TextAnchor.UpperRight;
		int x = Screen.width / 2 - 84 - this.csradar.team_dot_all * 8;
		x = this.DrawScoreCT(0, x, 2, this.csradar.team_dot[0]);
		x = this.DrawScoreCT(1, x, 2, this.csradar.team_dot[1]);
		x = this.DrawScoreCT(2, x, 2, this.csradar.team_dot[2]);
		x = this.DrawScoreCT(3, x, 2, this.csradar.team_dot[3]);
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x0004E4B4 File Offset: 0x0004C6B4
	private void DrawPR()
	{
		this.DrawTimer();
		if (this.map == null)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			if (this.map.flags[i].inited)
			{
				int num = 1;
				int num2 = this.map.flags[i].timer[1];
				if (this.map.flags[i].timer[0] > this.map.flags[i].timer[1])
				{
					num = 0;
					num2 = this.map.flags[i].timer[0];
				}
				if (this.oldFlagScores[i] != num2)
				{
					this.mig[i] = true;
				}
				if (this.mig[i])
				{
					if (this.oldFlagScores[i] != num2)
					{
						this.c[i] = new Color(this.c[i].r, this.c[i].g, this.c[i].b, this.c[i].a - 0.02f);
					}
					else
					{
						this.c[i] = new Color(this.c[i].r, this.c[i].g, this.c[i].b, this.c[i].a + 0.02f);
					}
					if (this.c[i].a <= 0.5f)
					{
						this.oldFlagScores[i] = num2;
					}
					if (this.c[i].a >= 1f)
					{
						this.c[i].a = 1f;
						this.mig[i] = false;
					}
				}
				GUI.color = this.c[i];
				GUI.DrawTexture(this.proriv_rect[i], this.Flags[2]);
				GUI.DrawTexture(this.proriv_rect[i + 4], this.Flags[num]);
				GUIManager.DrawText(this.proriv_rect[i + 8], num2.ToString(), 18, TextAnchor.MiddleLeft, 8);
				if (this.map.flags[i].FP != null)
				{
					this.map.flags[i].FP.MyGUI();
				}
				GUI.color = Color.white;
			}
		}
		GUI.color = Color.black;
		GUI.DrawTexture(this.proriv_rect[12], this.tex_evacuation);
		GUI.color = Color.white;
		GUI.DrawTexture(this.proriv_rect[13], this.tex_evacuation);
		GUIManager.DrawText(this.proriv_rect[14], this.score[0].ToString() + "/50", 18, TextAnchor.MiddleLeft, 8);
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x0004E7A8 File Offset: 0x0004C9A8
	private int DrawScoreCT(int team, int x, int y, int dots)
	{
		if (team == 0)
		{
			GUI.color = new Color(0f, 0f, 1f, 1f);
		}
		else if (team == 1)
		{
			GUI.color = new Color(1f, 0f, 0f, 1f);
		}
		else if (team == 2)
		{
			GUI.color = new Color(0f, 1f, 0f, 1f);
		}
		else if (team == 3)
		{
			GUI.color = new Color(1f, 1f, 0f, 1f);
		}
		GUI.DrawTexture(new Rect((float)x, (float)y, 64f, 32f), this.tex_score);
		x += 42;
		this.x_pos = x - 296;
		this.y_pos = 4;
		for (int i = 0; i < dots; i++)
		{
			GUI.DrawTexture(new Rect((float)x, (float)y, 32f, 32f), this.tex_dot);
			x += 16;
		}
		GUI.DrawTexture(new Rect((float)x, (float)y, 1f, 32f), this.tex_score);
		x++;
		GUI.color = new Color(1f, 1f, 1f, 1f);
		this.DrawText(this.score[team].ToString());
		return x;
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x0004E904 File Offset: 0x0004CB04
	private void DrawCS()
	{
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - 90f, 0f, 180f, 32f), this.tex_topbar);
		this.gui_style.alignment = TextAnchor.UpperRight;
		this.x_pos = Screen.width / 2 - 36 - 294;
		this.y_pos = 0;
		this.DrawText(this.score[0].ToString());
		this.gui_style.alignment = TextAnchor.UpperLeft;
		this.x_pos = Screen.width / 2 + 38;
		this.y_pos = 0;
		this.DrawText(this.score[1].ToString());
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x0004E9BC File Offset: 0x0004CBBC
	private void DrawML()
	{
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - 90f, 0f, 180f, 32f), this.tex_topbar2);
		this.gui_style.alignment = TextAnchor.UpperRight;
		this.x_pos = Screen.width / 2 - 36 - 294;
		this.y_pos = 0;
		this.DrawText(this.score[0].ToString());
		this.gui_style.alignment = TextAnchor.UpperLeft;
		this.x_pos = Screen.width / 2 + 38;
		this.y_pos = 0;
		this.DrawText(this.score[1].ToString());
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x0004EA74 File Offset: 0x0004CC74
	private void DrawFFA()
	{
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - 64f, 0f, 128f, 32f), this.tex_topbar3);
		this.gui_style.alignment = TextAnchor.UpperLeft;
		this.x_pos = Screen.width / 2 - 44;
		this.y_pos = 0;
		this.DrawText(this.score[0].ToString());
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x0004EAEC File Offset: 0x0004CCEC
	private void DrawTimer()
	{
		this.x_pos = Screen.width - 60;
		this.y_pos = 12;
		GUI.color = new Color(0f, 0f, 0f, 0.5f);
		GUI.DrawTexture(new Rect((float)(Screen.width - 64), (float)(this.y_pos - 1), 64f, 19f), GUIManager.tex_black);
		GUI.color = new Color(1f, 1f, 1f, 1f);
		int num = this.timer - (int)(Time.time - this.timerset);
		if (num < 0)
		{
			num = 0;
		}
		int num2 = num / 60;
		int num3 = num - num2 * 60;
		GUI.DrawTexture(new Rect((float)(this.x_pos - 3), (float)(this.y_pos + 1), 16f, 16f), GUIManager.tex_clock);
		this.gui_style.normal.textColor = new Color(0f, 0f, 0f, 255f);
		GUI.Label(new Rect((float)(Screen.width - 40 + 1), (float)(this.y_pos + 1), 294f, 20f), num2.ToString() + ":" + num3.ToString("00"), this.gui_style);
		this.gui_style.normal.textColor = new Color(255f, 255f, 255f, 255f);
		GUI.Label(new Rect((float)(Screen.width - 40), (float)this.y_pos, 294f, 20f), num2.ToString() + ":" + num3.ToString("00"), this.gui_style);
		this.y_pos += 32;
	}

	// Token: 0x0400088C RID: 2188
	private PlayerControl pc;

	// Token: 0x0400088D RID: 2189
	private Radar csradar;

	// Token: 0x0400088E RID: 2190
	private GUIStyle gui_style;

	// Token: 0x0400088F RID: 2191
	private int y_pos;

	// Token: 0x04000890 RID: 2192
	private int x_pos;

	// Token: 0x04000891 RID: 2193
	private int[] score = new int[4];

	// Token: 0x04000892 RID: 2194
	private float timerset;

	// Token: 0x04000893 RID: 2195
	private int timer;

	// Token: 0x04000894 RID: 2196
	private Texture2D tex_limit;

	// Token: 0x04000895 RID: 2197
	private Texture2D tex_dot;

	// Token: 0x04000896 RID: 2198
	private Texture2D tex_score;

	// Token: 0x04000897 RID: 2199
	private Texture2D tex_topbar;

	// Token: 0x04000898 RID: 2200
	private Texture2D tex_topbar2;

	// Token: 0x04000899 RID: 2201
	private Texture2D tex_topbar3;

	// Token: 0x0400089A RID: 2202
	private Texture2D tex_evacuation;

	// Token: 0x0400089B RID: 2203
	public Texture2D[] Flags;

	// Token: 0x0400089C RID: 2204
	private Rect[] battle_rect = new Rect[16];

	// Token: 0x0400089D RID: 2205
	private Rect[] proriv_rect = new Rect[16];

	// Token: 0x0400089E RID: 2206
	private Map map;

	// Token: 0x0400089F RID: 2207
	private Color[] c = new Color[4];

	// Token: 0x040008A0 RID: 2208
	private int[] oldFlagScores = new int[4];

	// Token: 0x040008A1 RID: 2209
	private bool[] mig = new bool[4];

	// Token: 0x040008A2 RID: 2210
	private MODE g_mode;
}
