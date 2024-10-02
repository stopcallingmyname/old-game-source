using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
public class Options : MonoBehaviour
{
	// Token: 0x060000B5 RID: 181 RVA: 0x0000CB0C File Offset: 0x0000AD0C
	private void myGlobalInit()
	{
		if (PlayerProfile.crossList.Count == 0)
		{
			PlayerProfile.crossList.Add(0, ContentLoader.LoadTexture("crosshair"));
			PlayerProfile.crossList.Add(1, ContentLoader.LoadTexture("crosshair_new_1"));
			PlayerProfile.crossList.Add(2, ContentLoader.LoadTexture("crosshair_new_2"));
			PlayerProfile.crossList.Add(3, ContentLoader.LoadTexture("crosshair_new_3"));
			PlayerProfile.crossList.Add(4, ContentLoader.LoadTexture("crosshair_new_4"));
			PlayerProfile.crossList.Add(5, ContentLoader.LoadTexture("crosshair_new_5"));
			PlayerProfile.crossList.Add(6, ContentLoader.LoadTexture("crosshair_new_6"));
			PlayerProfile.crossList.Add(7, ContentLoader.LoadTexture("crosshair_new_7"));
			PlayerProfile.crossList.Add(8, ContentLoader.LoadTexture("crosshair_new_8"));
			PlayerProfile.crossList.Add(9, ContentLoader.LoadTexture("crosshair_new_9"));
			PlayerProfile.crossList.Add(10, ContentLoader.LoadTexture("crosshair_new_10"));
			PlayerProfile.crossList.Add(11, ContentLoader.LoadTexture("crosshair_new_11"));
			PlayerProfile.crossList.Add(12, ContentLoader.LoadTexture("crosshair_new_12"));
			PlayerProfile.crossList.Add(13, ContentLoader.LoadTexture("crosshair_new_13"));
			PlayerProfile.crossList.Add(14, ContentLoader.LoadTexture("crosshair_new_14"));
			PlayerProfile.crossList.Add(15, ContentLoader.LoadTexture("crosshair_new_15"));
			PlayerProfile.crossList.Add(16, ContentLoader.LoadTexture("crosshair_new_16"));
			PlayerProfile.crossDot.Add(0, null);
			PlayerProfile.crossDot.Add(1, ContentLoader.LoadTexture("dot_1"));
			PlayerProfile.crossDot.Add(2, ContentLoader.LoadTexture("dot_2"));
			PlayerProfile.crossColor = new Color(Config.crossR, Config.crossG, Config.crossB);
		}
		this.Active = false;
		this.res = Screen.resolutions;
		for (int i = 0; i < this.res.Length; i++)
		{
			if (this.res[i].width >= 1024)
			{
				this.minRes = i;
				break;
			}
		}
		this.dist[0] = 168;
		this.dist[1] = 169;
		this.dist[2] = 170;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x0000CD54 File Offset: 0x0000AF54
	public void onActive()
	{
		this.local_tileset = Config.Tileset;
		this.local_sens = Config.Sensitivity;
		this.local_light = Config.Dlight;
		this.local_fullscreen = Config.Fscreen;
		if (this.local_light == 1)
		{
			this._local_light = true;
		}
		else
		{
			this._local_light = false;
		}
		if (this.local_fullscreen == 1)
		{
			this._local_fullscreen = true;
		}
		else
		{
			this._local_fullscreen = false;
		}
		this.local_crosshair_dot = Config.dot;
		this.local_crosshair_id = Config.cross;
		this.local_crossColor = new Color(Config.crossR, Config.crossG, Config.crossB);
		this.respos = Config.respos;
		if (this.respos < this.minRes)
		{
			this.respos = this.minRes;
		}
		this.distpos = Config.distpos;
		this.saved = false;
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x0000CE28 File Offset: 0x0000B028
	private void OnGUI()
	{
		if (GameController.STATE != GAME_STATES.MAINMENU)
		{
			return;
		}
		if (GM.currGUIState != GUIGS.OPTIONS)
		{
			return;
		}
		GUI.Window(901, new Rect((float)Screen.width / 2f - 300f, 199f, 600f, GUIManager.YRES(768f) - 199f), new GUI.WindowFunction(this.DoWindow), "", GUIManager.gs_style1);
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x0000CE9C File Offset: 0x0000B09C
	private void DoWindow(int windowID)
	{
		if (!this.Active)
		{
			return;
		}
		Vector2 mpos = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
		mpos.x -= (float)Screen.width / 2f - 300f;
		mpos.y -= 194f;
		GUI.DrawTexture(new Rect(0f, 0f, 600f, 32f), GUIManager.tex_panel);
		GUI.DrawTexture(new Rect(0f, 32f, 600f, GUIManager.YRES(768f) - 199f), GUIManager.tex_half_black);
		this.DrawSensitivity(new Rect(8f, 40f, 584f, 32f));
		this.DrawTileSet(new Rect(8f, 74f, 584f, 32f));
		this.DrawLight(new Rect(8f, 108f, 584f, 32f));
		this.DrawResolution(new Rect(8f, 142f, 584f, 32f), mpos);
		this.DrawDistance(new Rect(8f, 176f, 584f, 32f), mpos);
		this.DrawCrosshairDot(new Rect(8f, 210f, 516f, 32f), mpos);
		this.DrawCrosshairId(new Rect(8f, 244f, 516f, 32f), mpos);
		this.DrawCrosshairPrev(new Rect(526f, 210f, 66f, 66f));
		this.DrawCrosshairColor(new Rect(8f, 278f, 584f, 32f), mpos);
		this.DrawFullScreen(new Rect(8f, 312f, 584f, 32f));
		this.DrawLanguage(new Rect(8f, 346f, 584f, 32f));
		this.DrawSocialID(new Rect(8f, 380f, 584f, 32f));
		this.DrawButton();
		if (this.saved)
		{
			GUI.DrawTexture(new Rect(108f, GUIManager.YRES(768f) - 300f, 384f, 32f), GUIManager.tex_proceed);
			GUIManager.DrawText(new Rect(108f, GUIManager.YRES(768f) - 300f, 384f, 32f), Lang.GetLabel(28), 16, TextAnchor.MiddleCenter, 8);
		}
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x0000D13C File Offset: 0x0000B33C
	private void DrawSensitivity(Rect r)
	{
		GUIManager.HSlider();
		GUI.DrawTexture(r, GUIManager.tex_half_black);
		GUIManager.DrawText(new Rect(r.x + 100f, r.y, r.width, r.height), Lang.GetLabel(29), 16, TextAnchor.MiddleLeft, 8);
		GUILayout.BeginArea(new Rect(r.x + 360f, r.y + 9f, 512f, 512f));
		this.local_sens = GUILayout.HorizontalSlider(this.local_sens, 0.1f, 20f, new GUILayoutOption[]
		{
			GUILayout.Width(128f)
		});
		GUILayout.EndArea();
		this.local_sens = (float)((int)(this.local_sens * 10f)) / 10f;
		GUIManager.DrawText(new Rect(r.x + 312f, r.y, r.width, r.height), this.local_sens.ToString("0.00"), 16, TextAnchor.MiddleLeft, 8);
	}

	// Token: 0x060000BA RID: 186 RVA: 0x0000D24C File Offset: 0x0000B44C
	private void DrawTileSet(Rect r)
	{
		GUI.DrawTexture(r, GUIManager.tex_half_black);
		GUIManager.DrawText(new Rect(r.x + 100f, r.y, r.width, r.height), Lang.GetLabel(167), 16, TextAnchor.MiddleLeft, 8);
		if (this.local_tileset == 0)
		{
			GUI.DrawTexture(new Rect(320f, 78f, 25f, 25f), GUIManager.tex_half_yellow);
		}
		GUI.DrawTexture(new Rect(320f, 78f, 24f, 24f), GUIManager.tex_atlas1);
		if (GUI.Button(new Rect(320f, 78f, 24f, 24f), "", GUIManager.gs_style1))
		{
			this.local_tileset = 0;
		}
		if (this.local_tileset == 1)
		{
			GUI.DrawTexture(new Rect(356f, 78f, 25f, 25f), GUIManager.tex_half_yellow);
		}
		GUI.DrawTexture(new Rect(356f, 78f, 24f, 24f), GUIManager.tex_atlas2);
		if (GUI.Button(new Rect(356f, 78f, 24f, 24f), "", GUIManager.gs_style1))
		{
			this.local_tileset = 1;
		}
		if (this.local_tileset == 2)
		{
			GUI.DrawTexture(new Rect(392f, 78f, 25f, 25f), GUIManager.tex_half_yellow);
		}
		GUI.DrawTexture(new Rect(392f, 78f, 24f, 24f), GUIManager.tex_atlas3);
		if (GUI.Button(new Rect(392f, 78f, 24f, 24f), "", GUIManager.gs_style1))
		{
			this.local_tileset = 2;
		}
		if (this.local_tileset == 3)
		{
			GUI.DrawTexture(new Rect(428f, 78f, 25f, 25f), GUIManager.tex_half_yellow);
		}
		GUI.DrawTexture(new Rect(428f, 78f, 24f, 24f), GUIManager.tex_atlas4);
		if (GUI.Button(new Rect(428f, 78f, 24f, 24f), "", GUIManager.gs_style1))
		{
			this.local_tileset = 3;
		}
		if (this.local_tileset == 4)
		{
			GUI.DrawTexture(new Rect(464f, 78f, 25f, 25f), GUIManager.tex_half_yellow);
		}
		GUI.DrawTexture(new Rect(464f, 78f, 24f, 24f), GUIManager.tex_atlas5);
		if (GUI.Button(new Rect(464f, 78f, 24f, 24f), "", GUIManager.gs_style1))
		{
			this.local_tileset = 4;
		}
	}

	// Token: 0x060000BB RID: 187 RVA: 0x0000D520 File Offset: 0x0000B720
	private void DrawCrosshairDot(Rect r, Vector2 mpos)
	{
		GUI.DrawTexture(r, GUIManager.tex_half_black);
		GUIManager.DrawText(new Rect(r.x + 100f, r.y, r.width, r.height), Lang.GetLabel(515), 16, TextAnchor.MiddleLeft, 8);
		if (this.local_crosshair_dot < 0)
		{
			this.local_crosshair_dot = 0;
		}
		if (this.local_crosshair_dot > PlayerProfile.crossDot.Count - 1)
		{
			this.local_crosshair_dot = PlayerProfile.crossDot.Count - 1;
		}
		if (this.local_crosshair_dot > 0)
		{
			GUI.DrawTexture(new Rect(344f, 214f, 22f, 22f), PlayerProfile.crossDot[this.local_crosshair_dot]);
		}
		if (GUIManager.DrawButton(new Rect(320f, 214f, 20f, 22f), mpos, new Color(1f, 1f, 1f, 1f), "<"))
		{
			this.local_crosshair_dot--;
		}
		if (GUIManager.DrawButton(new Rect(368f, 214f, 20f, 22f), mpos, new Color(1f, 1f, 1f, 1f), ">"))
		{
			this.local_crosshair_dot++;
		}
	}

	// Token: 0x060000BC RID: 188 RVA: 0x0000D67C File Offset: 0x0000B87C
	private void DrawCrosshairId(Rect r, Vector2 mpos)
	{
		GUI.DrawTexture(r, GUIManager.tex_half_black);
		GUIManager.DrawText(new Rect(r.x + 100f, r.y, r.width, r.height), Lang.GetLabel(516), 16, TextAnchor.MiddleLeft, 8);
		if (this.local_crosshair_id < 0)
		{
			this.local_crosshair_id = 0;
		}
		if (this.local_crosshair_id > PlayerProfile.crossList.Count - 1)
		{
			this.local_crosshair_id = PlayerProfile.crossList.Count - 1;
		}
		GUI.DrawTexture(new Rect(344f, 246f, 22f, 22f), PlayerProfile.crossList[this.local_crosshair_id]);
		if (GUIManager.DrawButton(new Rect(320f, 246f, 20f, 22f), mpos, new Color(1f, 1f, 1f, 1f), "<"))
		{
			this.local_crosshair_id--;
		}
		if (GUIManager.DrawButton(new Rect(368f, 246f, 20f, 22f), mpos, new Color(1f, 1f, 1f, 1f), ">"))
		{
			this.local_crosshair_id++;
		}
	}

	// Token: 0x060000BD RID: 189 RVA: 0x0000D7CC File Offset: 0x0000B9CC
	private void DrawCrosshairPrev(Rect r)
	{
		GUI.DrawTexture(r, GUIManager.tex_crossPrevBackground);
		if (this.local_crosshair_dot < 0)
		{
			this.local_crosshair_dot = 0;
		}
		if (this.local_crosshair_dot > PlayerProfile.crossDot.Count - 1)
		{
			this.local_crosshair_dot = PlayerProfile.crossDot.Count - 1;
		}
		if (this.local_crosshair_id < 0)
		{
			this.local_crosshair_id = 0;
		}
		if (this.local_crosshair_id > PlayerProfile.crossList.Count - 1)
		{
			this.local_crosshair_id = PlayerProfile.crossList.Count - 1;
		}
		GUI.color = this.local_crossColor;
		GUI.DrawTexture(new Rect(r.x + 17f, r.y + 17f, 32f, 32f), PlayerProfile.crossList[this.local_crosshair_id]);
		if (this.local_crosshair_dot > 0)
		{
			GUI.DrawTexture(new Rect(r.x + 17f, r.y + 17f, 32f, 32f), PlayerProfile.crossDot[this.local_crosshair_dot]);
		}
		GUI.color = Color.white;
	}

	// Token: 0x060000BE RID: 190 RVA: 0x0000D8EC File Offset: 0x0000BAEC
	private void DrawCrosshairColor(Rect r, Vector2 mpos)
	{
		GUI.DrawTexture(r, GUIManager.tex_half_black);
		GUIManager.DrawText(new Rect(r.x + 100f, r.y, r.width, r.height), Lang.GetLabel(517), 16, TextAnchor.MiddleLeft, 8);
		Rect position = new Rect(320f, r.y + 4f, 242f, 22f);
		Color color = Color.white;
		GUI.DrawTexture(position, GUIManager.tex_crossPalette);
		bool flag = false;
		position.y += 6f;
		if (position.Contains(mpos))
		{
			Vector2 vector = new Vector2((float)((int)(mpos.x - position.x)), (float)(22 - (int)(mpos.y - position.y)));
			color = GUIManager.tex_crossPalette.GetPixel((int)vector.x, (int)vector.y);
			if (Input.GetMouseButtonDown(0))
			{
				this.local_crossColor = color;
			}
			flag = true;
		}
		if (flag)
		{
			GUI.color = color;
		}
		else
		{
			GUI.color = this.local_crossColor;
		}
		GUI.DrawTexture(new Rect(566f, r.y + 4f, 22f, 22f), GUI3.whiteTex);
		GUI.color = Color.white;
	}

	// Token: 0x060000BF RID: 191 RVA: 0x0000DA30 File Offset: 0x0000BC30
	private void DrawLight(Rect r)
	{
		GUIManager.Toggle();
		GUI.DrawTexture(r, GUIManager.tex_half_black);
		GUIManager.DrawText(new Rect(r.x + 100f, r.y, r.width, r.height), Lang.GetLabel(30), 16, TextAnchor.MiddleLeft, 8);
		this._local_light = GUI.Toggle(new Rect(320f, 112f, 16f, 16f), this._local_light, "");
		if (this._local_light)
		{
			this.local_light = 1;
			return;
		}
		this.local_light = 0;
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x0000DACC File Offset: 0x0000BCCC
	private void DrawResolution(Rect r, Vector2 mpos)
	{
		if (this.res == null)
		{
			return;
		}
		GUI.DrawTexture(r, GUIManager.tex_half_black);
		GUIManager.DrawText(new Rect(r.x + 100f, r.y, r.width, r.height), Lang.GetLabel(31), 16, TextAnchor.MiddleLeft, 8);
		if (this.respos < this.minRes)
		{
			this.respos = this.minRes;
		}
		if (this.respos > this.res.Length - 1)
		{
			this.respos = this.res.Length - 1;
		}
		GUIManager.DrawButton(new Rect(346f, 150f, 96f, 20f), mpos, new Color(1f, 1f, 1f, 1f), this.res[this.respos].width + "x" + this.res[this.respos].height);
		if (GUIManager.DrawButton(new Rect(320f, 150f, 22f, 20f), mpos, new Color(1f, 1f, 1f, 1f), "<"))
		{
			this.respos--;
		}
		if (GUIManager.DrawButton(new Rect(446f, 150f, 22f, 20f), mpos, new Color(1f, 1f, 1f, 1f), ">"))
		{
			this.respos++;
		}
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x0000DC74 File Offset: 0x0000BE74
	private void DrawDistance(Rect r, Vector2 mpos)
	{
		if (this.res == null)
		{
			return;
		}
		GUI.DrawTexture(r, GUIManager.tex_half_black);
		GUIManager.DrawText(new Rect(r.x + 100f, r.y, r.width, r.height), Lang.GetLabel(32), 16, TextAnchor.MiddleLeft, 8);
		if (this.distpos < 0)
		{
			this.distpos = 0;
		}
		if (this.distpos > 2)
		{
			this.distpos = 2;
		}
		GUIManager.DrawButton(new Rect(346f, 182f, 96f, 20f), mpos, new Color(1f, 1f, 1f, 1f), Lang.GetLabel(this.dist[this.distpos]));
		if (GUIManager.DrawButton(new Rect(320f, 182f, 22f, 20f), mpos, new Color(1f, 1f, 1f, 1f), "<"))
		{
			this.distpos--;
		}
		if (GUIManager.DrawButton(new Rect(446f, 182f, 22f, 20f), mpos, new Color(1f, 1f, 1f, 1f), ">"))
		{
			this.distpos++;
		}
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x0000DDD4 File Offset: 0x0000BFD4
	private void DrawFullScreen(Rect r)
	{
		GUIManager.Toggle();
		GUI.DrawTexture(r, GUIManager.tex_half_black);
		GUIManager.DrawText(new Rect(r.x + 100f, r.y, r.width, r.height), Lang.GetLabel(363), 16, TextAnchor.MiddleLeft, 8);
		this._local_fullscreen = GUI.Toggle(new Rect(320f, r.y + 4f, 16f, 16f), this._local_fullscreen, "");
		if (this._local_fullscreen)
		{
			this.local_fullscreen = 1;
			return;
		}
		this.local_fullscreen = 0;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x0000DE7C File Offset: 0x0000C07C
	private void DrawLanguage(Rect r)
	{
		GUI.DrawTexture(r, GUIManager.tex_half_black);
		GUIManager.DrawText(new Rect(r.x + 100f, r.y, r.width, r.height), Lang.GetLabel(826), 16, TextAnchor.MiddleLeft, 8);
		if (Lang.current == 0)
		{
			GUI.DrawTexture(new Rect(320f, r.y + 2f, 25f, 25f), GUIManager.tex_half_yellow);
		}
		GUI.DrawTexture(new Rect(320f, r.y + 2f, 24f, 24f), GUIManager.tex_flags_filter[0]);
		if (GUI.Button(new Rect(320f, r.y + 2f, 24f, 24f), "", GUIManager.gs_style1))
		{
			Lang.current = 0;
			GUIManager.Init(true);
			PlayerPrefs.SetInt(CONST.MD5("DefLanguage"), Lang.current);
			PlayerPrefs.Save();
		}
		if (Lang.current == 1)
		{
			GUI.DrawTexture(new Rect(356f, r.y + 2f, 25f, 25f), GUIManager.tex_half_yellow);
		}
		GUI.DrawTexture(new Rect(356f, r.y + 2f, 24f, 24f), GUIManager.tex_flags_filter[2]);
		if (GUI.Button(new Rect(356f, r.y + 2f, 24f, 24f), "", GUIManager.gs_style1))
		{
			Lang.current = 1;
			GUIManager.Init(true);
			PlayerPrefs.SetInt(CONST.MD5("DefLanguage"), Lang.current);
			PlayerPrefs.Save();
		}
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x0000E03C File Offset: 0x0000C23C
	private void DrawSocialID(Rect r)
	{
		GUIManager.Toggle();
		GUI.DrawTexture(r, GUIManager.tex_half_black);
		GUIManager.DrawText(new Rect(r.x + 100f, r.y, r.width, r.height), Lang.GetLabel(384), 16, TextAnchor.MiddleLeft, 8);
		TextAnchor alignment = GUIManager.gs_style1.alignment;
		GUIManager.gs_style1.alignment = TextAnchor.MiddleLeft;
		GUI.TextField(new Rect(320f, r.y, r.width, r.height), PlayerProfile.id, GUIManager.gs_style1);
		GUIManager.gs_style1.alignment = alignment;
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x0000E0E4 File Offset: 0x0000C2E4
	private void DrawButton()
	{
		float x = Input.mousePosition.x;
		float y = (float)Screen.height - Input.mousePosition.y;
		Rect rect = new Rect(236f + (float)Screen.width / 2f - 300f - 32f, GUIManager.YRES(768f) - 60f + 34f - 19f, 192f, 32f);
		if (rect.Contains(new Vector2(x, y)))
		{
			if (!this.hover_save)
			{
				this.hover_save = true;
				MainMenu.MainAS.PlayOneShot(SoundManager.SoundHover, AudioListener.volume);
			}
		}
		else if (this.hover_save)
		{
			this.hover_save = false;
		}
		int num = 0;
		if (this.hover_save)
		{
			num = 2;
		}
		GUI.DrawTexture(new Rect(204f, GUIManager.YRES(768f) - 220f - (float)num - 19f, 192f, 32f), GUIManager.tex_button);
		GUIManager.DrawText(new Rect(204f, GUIManager.YRES(768f) - 220f - (float)num - 19f, 192f, 32f), Lang.GetLabel(171), 16, TextAnchor.MiddleCenter, 8);
		if (GUI.Button(new Rect(204f, GUIManager.YRES(768f) - 220f - (float)num - 19f, 192f, 32f), "", GUIManager.gs_style1))
		{
			Config.Tileset = this.local_tileset;
			Config.Sensitivity = this.local_sens;
			Config.Dlight = this.local_light;
			if (this.respos < this.minRes)
			{
				this.respos = this.minRes;
			}
			Config.respos = this.respos;
			Config.distpos = this.distpos;
			Config.dot = this.local_crosshair_dot;
			Config.cross = this.local_crosshair_id;
			Config.Fscreen = this.local_fullscreen;
			PlayerPrefs.SetInt(CONST.MD5("Tileset"), this.local_tileset);
			PlayerPrefs.SetInt(CONST.MD5("Dlight"), this.local_light);
			PlayerPrefs.SetFloat(CONST.MD5("Sensitivity"), this.local_sens);
			PlayerPrefs.SetInt(CONST.MD5("Res"), this.respos);
			PlayerPrefs.SetInt(CONST.MD5("Dist"), this.distpos);
			PlayerPrefs.SetInt(CONST.MD5("CrosshairDot"), this.local_crosshair_dot);
			PlayerPrefs.SetInt(CONST.MD5("CrosshairID"), this.local_crosshair_id);
			PlayerPrefs.SetFloat(CONST.MD5("CrosshairR"), this.local_crossColor.r);
			PlayerPrefs.SetFloat(CONST.MD5("CrosshairG"), this.local_crossColor.g);
			PlayerPrefs.SetFloat(CONST.MD5("CrosshairB"), this.local_crossColor.b);
			PlayerProfile.crossColor = this.local_crossColor;
			PlayerPrefs.SetInt(CONST.MD5("Fscreen"), this.local_fullscreen);
			MainMenu.MainAS.PlayOneShot(SoundManager.SoundClick, AudioListener.volume);
			bool fullscreen = true;
			if (Config.Fscreen == 0)
			{
				fullscreen = false;
			}
			Screen.SetResolution(Screen.resolutions[this.respos].width, Screen.resolutions[this.respos].height, fullscreen);
			PlayerPrefs.Save();
			this.saved = true;
		}
	}

	// Token: 0x040000A7 RID: 167
	public bool Active;

	// Token: 0x040000A8 RID: 168
	private Resolution[] res;

	// Token: 0x040000A9 RID: 169
	private int minRes;

	// Token: 0x040000AA RID: 170
	private int[] dist = new int[3];

	// Token: 0x040000AB RID: 171
	private int respos = 1;

	// Token: 0x040000AC RID: 172
	private int distpos = 2;

	// Token: 0x040000AD RID: 173
	private int local_tileset;

	// Token: 0x040000AE RID: 174
	private float local_sens;

	// Token: 0x040000AF RID: 175
	private bool _local_light;

	// Token: 0x040000B0 RID: 176
	private int local_light;

	// Token: 0x040000B1 RID: 177
	private bool _local_fullscreen;

	// Token: 0x040000B2 RID: 178
	private int local_fullscreen;

	// Token: 0x040000B3 RID: 179
	private int local_crosshair_dot;

	// Token: 0x040000B4 RID: 180
	private int local_crosshair_id;

	// Token: 0x040000B5 RID: 181
	private Color local_crossColor = Color.white;

	// Token: 0x040000B6 RID: 182
	private bool saved;

	// Token: 0x040000B7 RID: 183
	private bool hover_save;
}
