using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class Clan : MonoBehaviour
{
	// Token: 0x06000045 RID: 69 RVA: 0x0000315A File Offset: 0x0000135A
	private void myGlobalInit()
	{
		this.Active = false;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00003163 File Offset: 0x00001363
	public void onActive()
	{
		if (PlayerProfile.id == "0")
		{
			return;
		}
		this.state = -1;
		base.StartCoroutine(this.get_clan(false));
	}

	// Token: 0x06000047 RID: 71 RVA: 0x0000318C File Offset: 0x0000138C
	private void OnGUI()
	{
		if (GameController.STATE != GAME_STATES.MAINMENU)
		{
			return;
		}
		if (GM.currGUIState != GUIGS.CLAN)
		{
			return;
		}
		GUI.Window(910, new Rect((float)Screen.width / 2f - 300f, 199f, 600f, GUIManager.YRES(768f) - 180f), new GUI.WindowFunction(this.DoWindow), "", GUIManager.gs_style1);
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00003200 File Offset: 0x00001400
	private void DoWindow(int windowID)
	{
		if (!this.Active)
		{
			return;
		}
		GUI.DrawTexture(new Rect(0f, 0f, 600f, 32f), GUIManager.tex_panel);
		GUI.DrawTexture(new Rect(0f, 32f, 600f, GUIManager.YRES(768f) - 180f), GUIManager.tex_half_black);
		if (this.state == 0)
		{
			this.DrawState0();
			return;
		}
		if (this.state == 1)
		{
			this.DrawState1();
			return;
		}
		if (this.state == 2)
		{
			this.DrawState2();
			return;
		}
		if (this.state == 3)
		{
			this.DrawState3();
			return;
		}
		if (this.state == 4)
		{
			this.DrawState4();
		}
	}

	// Token: 0x06000049 RID: 73 RVA: 0x000032B8 File Offset: 0x000014B8
	private void DrawState0()
	{
		Color textColor = GUIManager.gs_style1.normal.textColor;
		TextAnchor alignment = GUIManager.gs_style1.alignment;
		GUIManager.gs_style1.alignment = TextAnchor.MiddleCenter;
		GUIManager.gs_style1.normal.textColor = new Color(1f, 1f, 1f, 1f);
		Rect position = new Rect(0f, 50f, 600f, 32f);
		if (this.restore == 0)
		{
			GUI.Label(position, Lang.GetLabel(202), GUIManager.gs_style1);
			GUI.DrawTexture(new Rect(172f, 76f, 256f, 32f), GUIManager.tex_button);
			GUI.Label(new Rect(172f, 76f, 256f, 32f), Lang.GetLabel(0), GUIManager.gs_style1);
			if (GUI.Button(new Rect(172f, 76f, 256f, 32f), "", GUIManager.gs_style1))
			{
				this.state = 1;
			}
		}
		else
		{
			GUI.Label(position, Lang.GetLabel(1), GUIManager.gs_style1);
			GUI.DrawTexture(new Rect(172f, 76f, 256f, 32f), GUIManager.tex_button);
			GUI.Label(new Rect(172f, 76f, 256f, 32f), Lang.GetLabel(2), GUIManager.gs_style1);
			if (GUI.Button(new Rect(172f, 76f, 256f, 32f), "", GUIManager.gs_style1))
			{
				base.StartCoroutine(this.restore_clan());
			}
		}
		position = new Rect(0f, 130f, 600f, 32f);
		GUIManager.gs_style1.normal.textColor = new Color(1f, 1f, 1f, 1f);
		GUI.Label(position, Lang.GetLabel(3), GUIManager.gs_style1);
		GUI.DrawTexture(new Rect(172f, 160f, 256f, 32f), GUIManager.tex_button);
		GUI.Label(new Rect(172f, 160f, 256f, 32f), Lang.GetLabel(4), GUIManager.gs_style1);
		if (GUI.Button(new Rect(172f, 160f, 256f, 32f), "", GUIManager.gs_style1))
		{
			base.StartCoroutine(this.get_claninvite());
		}
		GUIManager.gs_style1.alignment = alignment;
		GUIManager.gs_style1.normal.textColor = textColor;
	}

	// Token: 0x0600004A RID: 74 RVA: 0x0000355C File Offset: 0x0000175C
	private void DrawState1()
	{
		Color textColor = GUIManager.gs_style1.normal.textColor;
		TextAnchor alignment = GUIManager.gs_style1.alignment;
		GUIManager.gs_style1.alignment = TextAnchor.MiddleLeft;
		GUI.Label(new Rect(128f, 64f, 128f, 22f), Lang.GetLabel(5), GUIManager.gs_style1);
		GUIManager.gs_style1.alignment = TextAnchor.MiddleCenter;
		GUI.DrawTexture(new Rect(255f, 64f, 1f, 22f), GUIManager.tex_half_yellow);
		GUI.DrawTexture(new Rect(352f, 64f, 1f, 22f), GUIManager.tex_half_yellow);
		GUI.DrawTexture(new Rect(255f, 63f, 98f, 1f), GUIManager.tex_half_yellow);
		GUI.DrawTexture(new Rect(255f, 86f, 98f, 1f), GUIManager.tex_half_yellow);
		GUI.DrawTexture(new Rect(256f, 64f, 96f, 22f), GUIManager.tex_half_black);
		this.clantag = GUI.TextField(new Rect(256f, 64f, 96f, 22f), this.clantag, 8, GUIManager.gs_style1);
		GUI.Label(new Rect(172f, 120f, 256f, 32f), Lang.GetLabel(6), GUIManager.gs_style1);
		GUI.DrawTexture(new Rect(172f, 160f, 256f, 32f), GUIManager.tex_button_hover);
		GUI.Label(new Rect(172f, 160f, 256f, 32f), Lang.GetLabel(0), GUIManager.gs_style1);
		if (GUI.Button(new Rect(172f, 160f, 256f, 32f), "", GUIManager.gs_style1))
		{
			base.StartCoroutine(this.create_clan());
		}
		GUIManager.gs_style1.alignment = alignment;
		GUIManager.gs_style1.normal.textColor = textColor;
	}

	// Token: 0x0600004B RID: 75 RVA: 0x0000376C File Offset: 0x0000196C
	private void DrawState2()
	{
		Color textColor = GUIManager.gs_style1.normal.textColor;
		TextAnchor alignment = GUIManager.gs_style1.alignment;
		if (this.claninvite == "")
		{
			GUI.Label(new Rect(0f, 32f, 600f, 32f), Lang.GetLabel(7), GUIManager.gs_style1);
		}
		else
		{
			GUI.Label(new Rect(0f, 32f, 600f, 32f), Lang.GetLabel(203) + this.claninvite, GUIManager.gs_style1);
			GUI.DrawTexture(new Rect(420f, 40f, 61f, 16f), GUIManager.tex_clan_decline);
			if (GUI.Button(new Rect(420f, 40f, 61f, 16f), "", GUIManager.gs_style1))
			{
				base.StartCoroutine(this.del_invite());
			}
		}
		if (this.claninvite != "")
		{
			GUI.Label(new Rect(0f, 64f, 600f, 32f), Lang.GetLabel(8), GUIManager.gs_style1);
		}
		else
		{
			GUI.Label(new Rect(100f, 64f, 128f, 22f), Lang.GetLabel(5), GUIManager.gs_style1);
			GUI.DrawTexture(new Rect(229f, 64f, 1f, 22f), GUIManager.tex_half_yellow);
			GUI.DrawTexture(new Rect(326f, 64f, 1f, 22f), GUIManager.tex_half_yellow);
			GUI.DrawTexture(new Rect(229f, 63f, 98f, 1f), GUIManager.tex_half_yellow);
			GUI.DrawTexture(new Rect(229f, 86f, 98f, 1f), GUIManager.tex_half_yellow);
			GUI.DrawTexture(new Rect(230f, 64f, 96f, 22f), GUIManager.tex_half_black);
			this.clantagfind = GUI.TextField(new Rect(230f, 64f, 96f, 22f), this.clantagfind, 8, GUIManager.gs_style1);
			GUI.DrawTexture(new Rect(332f, 60f, 128f, 32f), GUIManager.tex_clan_find);
			GUI.Label(new Rect(332f, 60f, 128f, 32f), Lang.GetLabel(4), GUIManager.gs_style1);
			if (GUI.Button(new Rect(332f, 60f, 128f, 32f), "", GUIManager.gs_style1))
			{
				base.StartCoroutine(this.get_clanfind());
			}
		}
		if (this.find_clanid < 0)
		{
			GUIManager.gs_style1.alignment = TextAnchor.MiddleCenter;
			GUI.Label(new Rect(0f, 100f, 600f, 22f), Lang.GetLabel(9), GUIManager.gs_style1);
		}
		else if (this.find_clanid > 0)
		{
			GUI.DrawTexture(new Rect(0f, 96f, 600f, 32f), GUIManager.tex_half_black);
			GUI.Label(new Rect(0f, 100f, 100f, 22f), Lang.GetLabel(10), GUIManager.gs_style1);
			GUI.Label(new Rect(100f, 100f, 100f, 22f), Lang.GetLabel(11), GUIManager.gs_style1);
			GUI.Label(new Rect(200f, 100f, 100f, 22f), Lang.GetLabel(12), GUIManager.gs_style1);
			GUI.Label(new Rect(300f, 100f, 100f, 22f), Lang.GetLabel(13), GUIManager.gs_style1);
			GUI.Label(new Rect(0f, 132f, 100f, 22f), this.find_clantag, GUIManager.gs_style1);
			GUI.Label(new Rect(100f, 132f, 100f, 22f), this.find_level, GUIManager.gs_style1);
			GUI.Label(new Rect(200f, 132f, 100f, 22f), this.find_capitan, GUIManager.gs_style1);
			GUI.Label(new Rect(300f, 132f, 100f, 22f), this.find_slots, GUIManager.gs_style1);
			if (this.find_full == 0)
			{
				GUI.DrawTexture(new Rect(400f, 132f, 88f, 16f), GUIManager.tex_clan_invite);
				if (GUI.Button(new Rect(400f, 132f, 88f, 16f), "", GUIManager.gs_style1))
				{
					base.StartCoroutine(this.send_invite());
				}
			}
		}
		GUIManager.gs_style1.alignment = alignment;
		GUIManager.gs_style1.normal.textColor = textColor;
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00003C6C File Offset: 0x00001E6C
	private void DrawState3()
	{
		Color textColor = GUIManager.gs_style1.normal.textColor;
		TextAnchor alignment = GUIManager.gs_style1.alignment;
		GUIManager.gs_style1.alignment = TextAnchor.MiddleCenter;
		Rect position = new Rect(0f, 32f, 600f, 32f);
		GUI.DrawTexture(position, GUIManager.tex_half_black);
		Rect position2 = new Rect(425f, 40f, 71f, 16f);
		GUI.DrawTexture(position2, GUIManager.tex_clan_manage);
		if (GUI.Button(position2, "", GUIManager.gs_style1))
		{
			this.state = 4;
		}
		GUIManager.gs_style1.normal.textColor = new Color(1f, 1f, 1f, 1f);
		GUIManager.gs_style1.alignment = TextAnchor.MiddleLeft;
		GUI.Label(new Rect(214f, 32f, 600f, 32f), Lang.GetLabel(204), GUIManager.gs_style1);
		if (!this.editName)
		{
			GUI.Label(new Rect(256f, 32f, 600f, 32f), this.info_tag, GUIManager.gs_style1);
			if (this.info_admin > 0)
			{
				Vector2 vector = GUIManager.gs_style1.CalcSize(new GUIContent(this.info_tag));
				int num;
				if (this.button_name_hover)
				{
					num = 2;
				}
				else
				{
					num = 0;
				}
				GUI.DrawTexture(new Rect(256f + vector.x + 4f, (float)(36 - num), 20f, 20f), GUIManager.tex_edit_icon);
				if (GUI.Button(new Rect(256f + vector.x + 4f, (float)(36 - num), 20f, 20f), "", GUIManager.gs_style1))
				{
					this.editName = true;
					MainMenu.MainAS.PlayOneShot(SoundManager.SoundClick, AudioListener.volume);
				}
			}
		}
		else
		{
			this.info_tag = GUI.TextField(new Rect(256f, 36f, 80f, 20f), this.info_tag, 8);
			int num;
			if (this.button_save_hover)
			{
				num = 2;
			}
			else
			{
				num = 0;
			}
			GUI.DrawTexture(new Rect(340f, (float)(36 - num), 32f, 32f), GUIManager.tex_save_icon);
			if (GUI.Button(new Rect(340f, (float)(36 - num), 20f, 20f), "", GUIManager.gs_style1))
			{
				this.editName = false;
				base.StartCoroutine(this.set_clanname());
			}
		}
		GUIManager.gs_style1.alignment = TextAnchor.MiddleCenter;
		position.y += 32f;
		GUI.Label(position, Lang.GetLabel(11) + ": " + this.info_level.ToString(), GUIManager.gs_style1);
		position.y += 26f;
		GUI.Label(position, Lang.GetLabel(14) + ": " + this.info_exp.ToString(), GUIManager.gs_style1);
		position.y += 26f;
		GUI.Label(position, string.Concat(new object[]
		{
			Lang.GetLabel(15),
			": ",
			this.info_progress,
			"%".ToString()
		}), GUIManager.gs_style1);
		position.y += 26f;
		GUI.Label(position, Lang.GetLabel(16) + ": " + this.info_adminname, GUIManager.gs_style1);
		position.y += 26f;
		GUI.Label(position, string.Concat(new string[]
		{
			Lang.GetLabel(17),
			": ",
			this.info_player_count.ToString(),
			"/",
			this.info_slots.ToString()
		}), GUIManager.gs_style1);
		position.y += 26f;
		GUI.Label(position, Lang.GetLabel(18) + ": " + this.info_timeparsed, GUIManager.gs_style1);
		position = new Rect(0f, 224f, 600f, 32f);
		GUI.DrawTexture(position, GUIManager.tex_half_black);
		GUIManager.gs_style1.normal.textColor = new Color(1f, 1f, 1f, 1f);
		GUI.Label(position, Lang.GetLabel(205), GUIManager.gs_style1);
		float height = GUIManager.YRES(768f) - 180f - 256f;
		this.scrollViewVector0 = GUIManager.BeginScrollView(new Rect(0f, position.y + 32f, 600f, height), this.scrollViewVector0, new Rect(0f, 0f, 0f, (float)(this.info_player_count * 20 + 10)));
		for (int i = 0; i < this.info_player_count; i++)
		{
			GUI.Label(new Rect(0f, (float)(i * 20), 600f, 20f), this.info_player_name[i], GUIManager.gs_style1);
		}
		GUIManager.EndScrollView();
		GUIManager.gs_style1.alignment = alignment;
		GUIManager.gs_style1.normal.textColor = textColor;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x000041B8 File Offset: 0x000023B8
	private void DrawState4()
	{
		Rect position = new Rect(204f, 36f, 192f, 32f);
		GUI.DrawTexture(position, GUIManager.tex_clan_exit);
		if (this.info_admin == 1)
		{
			GUI.Label(position, Lang.GetLabel(19), GUIManager.gs_style1);
		}
		else
		{
			GUI.Label(position, Lang.GetLabel(20), GUIManager.gs_style1);
		}
		if (GUI.Button(position, "", GUIManager.gs_style1))
		{
			base.StartCoroutine(this.leave_clan());
		}
		if (this.info_admin == 0)
		{
			return;
		}
		float num = (GUIManager.YRES(768f) - 180f - 136f) / 2f;
		position = new Rect(0f, 72f, 600f, 32f);
		GUI.DrawTexture(position, GUIManager.tex_half_black);
		GUI.Label(position, Lang.GetLabel(21), GUIManager.gs_style1);
		position.y += 32f;
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, position.y, 600f, num), this.scrollViewVector, new Rect(0f, 0f, 0f, (float)(this.info_player_count * 20 + 10)));
		position = new Rect(0f, 0f, 600f, 20f);
		for (int i = 0; i < this.info_player_count; i++)
		{
			if (PlayerProfile.id != this.info_player_vkid[i].ToString())
			{
				GUI.DrawTexture(new Rect(425f, (float)(i * 20 + 6), 61f, 16f), GUIManager.tex_clan_delete);
				if (GUI.Button(new Rect(425f, (float)(i * 20 + 6), 61f, 16f), "", GUIManager.gs_style1))
				{
					base.StartCoroutine(this.del_player(this.info_player_vkid[i]));
				}
			}
			GUI.Label(new Rect(0f, (float)(i * 20), 600f, 20f), this.info_player_name[i], GUIManager.gs_style1);
		}
		GUIManager.EndScrollView();
		position = new Rect(0f, num + 104f, 600f, 32f);
		GUI.DrawTexture(position, GUIManager.tex_half_black);
		GUI.Label(position, Lang.GetLabel(22), GUIManager.gs_style1);
		position.y += 32f;
		this.scrollViewVector2 = GUIManager.BeginScrollView(new Rect(0f, position.y, 600f, num), this.scrollViewVector2, new Rect(0f, 0f, 0f, (float)(this.inv_player_count * 20 + 10)));
		for (int j = 0; j < this.inv_player_count; j++)
		{
			GUI.DrawTexture(new Rect(365f, (float)(j * 20 + 6), 61f, 16f), GUIManager.tex_clan_accept);
			if (GUI.Button(new Rect(365f, (float)(j * 20 + 6), 61f, 16f), "", GUIManager.gs_style1))
			{
				base.StartCoroutine(this.accept_invite(this.inv_player_vkid[j]));
			}
			GUI.DrawTexture(new Rect(430f, (float)(j * 20 + 6), 61f, 16f), GUIManager.tex_clan_decline);
			if (GUI.Button(new Rect(430f, (float)(j * 20 + 6), 61f, 16f), "", GUIManager.gs_style1))
			{
				base.StartCoroutine(this.accept_decline(this.inv_player_vkid[j]));
			}
			GUI.Label(new Rect(0f, (float)(j * 20), 600f, 20f), this.inv_player_name[j], GUIManager.gs_style1);
			position.y += 20f;
		}
		GUIManager.EndScrollView();
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00004590 File Offset: 0x00002790
	private IEnumerator get_clan(bool noreset)
	{
		if (!noreset)
		{
			this.state = -1;
		}
		this.restore = 0;
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"1000&id=",
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
		if (www.error == null)
		{
			if (www.text == "NOCLAN")
			{
				this.state = 0;
			}
			else if (www.text == "RESTORECLAN")
			{
				this.restore = 1;
				this.state = 0;
			}
			else
			{
				string[] array = www.text.Split(new char[]
				{
					'^'
				});
				string[] array2 = array[0].Split(new char[]
				{
					'|'
				});
				this.info_tag = array2[0];
				int.TryParse(array2[1], out this.info_exp);
				int.TryParse(array2[2], out this.info_time);
				int.TryParse(array2[3], out this.info_slots);
				this.info_adminname = array2[4];
				int.TryParse(array2[5], out this.info_admin);
				if (!noreset)
				{
					this.state = 3;
				}
				int num = 1;
				this.info_level = 1;
				while (this.info_exp >= (num * (num + 1) * (num + 2) + 32 * num) * 10)
				{
					num++;
					this.info_level++;
				}
				num = this.info_level - 1;
				int num2 = (num * (num + 1) * (num + 2) + 32 * num) * 10;
				num = this.info_level;
				int num3 = (num * (num + 1) * (num + 2) + 32 * num) * 10;
				float num4 = (float)((this.info_exp - num2) * 100 / (num3 - num2));
				this.info_progress = (int)num4;
				this.info_timeparsed = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((double)this.info_time).ToString("dd/MM/yyyy");
				string[] array3 = array[1].Split(new char[]
				{
					'|'
				});
				int num5 = array3.Length - 1;
				for (int i = 0; i < num5; i++)
				{
					string[] array4 = array3[i].Split(new char[]
					{
						'*'
					});
					int j = 0;
					int.TryParse(array4[1], out j);
					this.info_player_vkid[i] = array4[2];
					int num6 = 1;
					int num7 = 1;
					while (j >= (num6 * (num6 + 1) * (num6 + 2) + 32 * num6) * 10)
					{
						num6++;
						num7++;
					}
					this.info_player_name[i] = "[" + num7.ToString() + "]" + array4[0];
				}
				this.info_player_count = num5;
				string[] array5 = array[2].Split(new char[]
				{
					'|'
				});
				this.inv_player_count = array5.Length - 1;
				if (array5[0] == "")
				{
					yield break;
				}
				for (int k = 0; k < this.inv_player_count; k++)
				{
					string[] array6 = array5[k].Split(new char[]
					{
						'*'
					});
					this.inv_player_vkid[k] = array6[0];
					this.inv_player_name[k] = "[" + array6[2] + "]" + array6[1];
				}
			}
		}
		yield break;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x000045A6 File Offset: 0x000027A6
	private IEnumerator create_clan()
	{
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"1001&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&name=",
			this.clantag,
			"&time=",
			DateTime.Now.Second
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			if (www.text == "NAMEBUSY")
			{
				this.clantag = "";
			}
			else if (www.text == "CREATED")
			{
				base.StartCoroutine(this.get_clan(false));
			}
		}
		yield break;
	}

	// Token: 0x06000050 RID: 80 RVA: 0x000045B5 File Offset: 0x000027B5
	private IEnumerator leave_clan()
	{
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"1002&id=",
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
		if (www.error == null)
		{
			if (www.text == "CLANOK")
			{
				base.StartCoroutine(this.get_clan(false));
			}
			else if (www.text == "USEROK")
			{
				base.StartCoroutine(this.get_clan(false));
			}
		}
		yield break;
	}

	// Token: 0x06000051 RID: 81 RVA: 0x000045C4 File Offset: 0x000027C4
	private IEnumerator restore_clan()
	{
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"1003&id=",
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
		if (www.error == null && www.text == "RESTORED")
		{
			base.StartCoroutine(this.get_clan(false));
		}
		yield break;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x000045D3 File Offset: 0x000027D3
	private IEnumerator get_claninvite()
	{
		this.state = 2;
		this.find_clanid = 0;
		this.claninvite = "";
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"1004&id=",
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
		if (www.error == null)
		{
			if (www.text == "NOINVITE")
			{
				this.claninvite = "";
			}
			else
			{
				this.claninvite = www.text;
			}
		}
		yield break;
	}

	// Token: 0x06000053 RID: 83 RVA: 0x000045E2 File Offset: 0x000027E2
	private IEnumerator get_clanfind()
	{
		this.find_clanid = 0;
		this.find_full = 0;
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"1005&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&name=",
			this.clantagfind,
			"&time=",
			DateTime.Now.Second
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			if (www.text == "NOTFOUND")
			{
				this.find_clanid = -1;
			}
			else
			{
				string[] array = www.text.Split(new char[]
				{
					'|'
				});
				int.TryParse(array[0], out this.find_clanid);
				this.find_clantag = array[1];
				this.find_level = array[2];
				this.find_capitan = "[" + array[4] + "]" + array[3];
				this.find_slots = array[5] + "/" + array[6];
				if (array[5] == array[6])
				{
					this.find_full = 1;
				}
			}
		}
		yield break;
	}

	// Token: 0x06000054 RID: 84 RVA: 0x000045F1 File Offset: 0x000027F1
	private IEnumerator send_invite()
	{
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"1006&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&clanid=",
			this.find_clanid.ToString(),
			"&time=",
			DateTime.Now.Second
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null && www.text == "OK")
		{
			base.StartCoroutine(this.get_claninvite());
		}
		yield break;
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00004600 File Offset: 0x00002800
	private IEnumerator del_invite()
	{
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"1007&id=",
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
		if (www.error == null && www.text == "OK")
		{
			base.StartCoroutine(this.get_claninvite());
		}
		yield break;
	}

	// Token: 0x06000056 RID: 86 RVA: 0x0000460F File Offset: 0x0000280F
	private IEnumerator accept_invite(string id)
	{
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"1008&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&mateid=",
			id.ToString(),
			"&time=",
			DateTime.Now.Second
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			base.StartCoroutine(this.get_clan(true));
		}
		yield break;
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00004625 File Offset: 0x00002825
	private IEnumerator accept_decline(string id)
	{
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"1009&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&mateid=",
			id.ToString(),
			"&time=",
			DateTime.Now.Second
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			base.StartCoroutine(this.get_clan(true));
		}
		yield break;
	}

	// Token: 0x06000058 RID: 88 RVA: 0x0000463B File Offset: 0x0000283B
	private IEnumerator del_player(string id)
	{
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"1010&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&mateid=",
			id.ToString(),
			"&time=",
			DateTime.Now.Second
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			base.StartCoroutine(this.get_clan(true));
		}
		yield break;
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00004651 File Offset: 0x00002851
	private IEnumerator set_clanname()
	{
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"1011&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&name=",
			this.info_tag,
			"&time=",
			DateTime.Now.Second
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			this.info_tag = www.text;
		}
		yield break;
	}

	// Token: 0x0400002D RID: 45
	public bool Active;

	// Token: 0x0400002E RID: 46
	private int state;

	// Token: 0x0400002F RID: 47
	private int restore;

	// Token: 0x04000030 RID: 48
	private string clantag = "";

	// Token: 0x04000031 RID: 49
	private string claninvite = "";

	// Token: 0x04000032 RID: 50
	private string clantagfind = "";

	// Token: 0x04000033 RID: 51
	private int find_clanid;

	// Token: 0x04000034 RID: 52
	private string find_clantag = "";

	// Token: 0x04000035 RID: 53
	private string find_level = "";

	// Token: 0x04000036 RID: 54
	private string find_capitan = "";

	// Token: 0x04000037 RID: 55
	private string find_slots = "";

	// Token: 0x04000038 RID: 56
	private int find_full;

	// Token: 0x04000039 RID: 57
	private string info_tag = "";

	// Token: 0x0400003A RID: 58
	private string info_adminname = "";

	// Token: 0x0400003B RID: 59
	private string info_timeparsed = "";

	// Token: 0x0400003C RID: 60
	private int info_exp;

	// Token: 0x0400003D RID: 61
	private int info_time;

	// Token: 0x0400003E RID: 62
	private int info_slots;

	// Token: 0x0400003F RID: 63
	private int info_admin;

	// Token: 0x04000040 RID: 64
	private int info_level;

	// Token: 0x04000041 RID: 65
	private int info_progress;

	// Token: 0x04000042 RID: 66
	private bool editName;

	// Token: 0x04000043 RID: 67
	private int info_player_count;

	// Token: 0x04000044 RID: 68
	private string[] info_player_vkid = new string[128];

	// Token: 0x04000045 RID: 69
	private string[] info_player_name = new string[128];

	// Token: 0x04000046 RID: 70
	private bool button_name_hover;

	// Token: 0x04000047 RID: 71
	private bool button_save_hover;

	// Token: 0x04000048 RID: 72
	private int inv_player_count;

	// Token: 0x04000049 RID: 73
	private string[] inv_player_vkid = new string[128];

	// Token: 0x0400004A RID: 74
	private string[] inv_player_name = new string[128];

	// Token: 0x0400004B RID: 75
	private Vector2 scrollViewVector0 = Vector2.zero;

	// Token: 0x0400004C RID: 76
	private Vector2 scrollViewVector = Vector2.zero;

	// Token: 0x0400004D RID: 77
	private Vector2 scrollViewVector2 = Vector2.zero;
}
