using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000021 RID: 33
public class ServerList : MonoBehaviour
{
	// Token: 0x060000F2 RID: 242 RVA: 0x00012BFE File Offset: 0x00010DFE
	private void Awake()
	{
		ServerList.THIS = this;
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00012C08 File Offset: 0x00010E08
	private void myGlobalInit()
	{
		for (int i = 0; i < 15; i++)
		{
			this.gamemode[i] = false;
		}
		this.gamemode[0] = true;
		this.gamemode[1] = true;
		this.gamemode[2] = true;
		this.gamemode[3] = true;
		this.gamemode[4] = true;
		this.gamemode[5] = true;
		this.gamemode[6] = true;
		this.gamemode[7] = true;
		this.gamemode[8] = true;
		this.gamemode[13] = true;
		this.filtercountry[0] = false;
		this.filtercountry[1] = false;
		this.filtercountry[2] = false;
		this.filtercountryBTN[0] = false;
		this.filtercountryBTN[1] = false;
		this.filtercountryBTN[2] = false;
		if (PlayerProfile.network == NETWORK.ST)
		{
			this.filtercountryBTN[0] = true;
			this.filtercountryBTN[1] = true;
		}
		for (int j = 0; j < 15; j++)
		{
			this.hoverleftmenu[j] = false;
		}
		base.StartCoroutine(this.update_stats());
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00012CF9 File Offset: 0x00010EF9
	private IEnumerator update_stats()
	{
		yield break;
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00012D01 File Offset: 0x00010F01
	public void refresh_servers()
	{
		this.lastupdate = Time.time;
		base.StartCoroutine(this.get_server_stats());
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00012D1B File Offset: 0x00010F1B
	private IEnumerator get_server_stats()
	{
		while (PlayerProfile.level <= 0)
		{
			yield return null;
		}
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER_LIST,
			"4&time",
			DateTime.Now.Second,
			"&LVL=",
			PlayerProfile.level
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			this.srvlist.Clear();
			string[] array = www.text.Split(new char[]
			{
				'^'
			});
			for (int i = 0; i < array.Length - 1; i++)
			{
				if (!(array[i] == ""))
				{
					string[] array2 = array[i].Split(new char[]
					{
						'|'
					});
					int num;
					int.TryParse(array2[0], out num);
					int num2;
					int.TryParse(array2[1], out num2);
					string ip = array2[2];
					int port;
					int.TryParse(array2[3], out port);
					int players;
					int.TryParse(array2[4], out players);
					int maxplayers;
					int.TryParse(array2[5], out maxplayers);
					string map = array2[6];
					ulong adminid;
					ulong.TryParse(array2[7], out adminid);
					int password;
					int.TryParse(array2[8], out password);
					int country_id;
					int.TryParse(array2[9], out country_id);
					int lvl;
					if (array2.Length > 10)
					{
						int.TryParse(array2[10], out lvl);
					}
					else
					{
						lvl = 0;
					}
					this.srvlist.Add(new CServerData(num, num2, players, maxplayers, map, adminid, ip, port, password, country_id, lvl));
				}
			}
			this.srvlist.Sort(delegate(CServerData x, CServerData y)
			{
				if (x.avaliable_by_lvl != y.avaliable_by_lvl)
				{
					return y.avaliable_by_lvl.CompareTo(x.avaliable_by_lvl);
				}
				if (x.players != y.players)
				{
					return y.players.CompareTo(x.players);
				}
				if (x.port != y.port)
				{
					return x.port.CompareTo(y.port);
				}
				return x.gamemode.CompareTo(y.gamemode);
			});
			this.next_update = Time.time + (float)CONST.CFG.SERVER_UPDATE_TIMEOUT;
			yield return new WaitForSeconds((float)CONST.CFG.SERVER_UPDATE_TIMEOUT);
			base.StartCoroutine(this.get_server_stats());
		}
		else
		{
			Debug.Log(www.error);
			this.next_update = Time.time + (float)CONST.CFG.SERVER_UPDATE_TIMEOUT / 2f;
			yield return new WaitForSeconds((float)CONST.CFG.SERVER_UPDATE_TIMEOUT / 2f);
			base.StartCoroutine(this.get_server_stats());
		}
		if (this._get_stats)
		{
			yield break;
		}
		base.StartCoroutine(this.get_stats());
		yield break;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00012D2A File Offset: 0x00010F2A
	private IEnumerator get_stats()
	{
		if (this._get_stats)
		{
			yield break;
		}
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"0&id=",
			PlayerProfile.id,
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
			int i;
			int.TryParse(www.text.Split(new char[]
			{
				'|'
			})[2], out i);
			int num = 1;
			int num2 = 1;
			while (i >= (num * (num + 1) * (num + 2) + 15 * num) * 10)
			{
				num++;
				num2++;
			}
			PlayerProfile.level = num2;
			this._get_stats = true;
			base.StartCoroutine(this.get_bonus_lvl());
		}
		yield break;
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00012D39 File Offset: 0x00010F39
	private IEnumerator get_bonus_lvl()
	{
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"102&id=",
			PlayerProfile.id,
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
			if (www.text.Split(new char[]
			{
				'^'
			})[0] == "OK")
			{
				PopUp.THIS.bonus_text = www.text;
				PopUp.ShowBonus(6, PlayerProfile.level);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00012D44 File Offset: 0x00010F44
	private void OnGUI()
	{
		if (!this.Active)
		{
			return;
		}
		if (GameController.STATE != GAME_STATES.MAINMENU)
		{
			return;
		}
		if (GM.currGUIState != GUIGS.SERVERLIST)
		{
			return;
		}
		GUI.Window(900, new Rect((float)Screen.width / 2f - 300f, 199f, 600f, (float)(Screen.height - 180)), new GUI.WindowFunction(this.DoWindow), "", GUIManager.gs_empty);
		float num = this.next_update - Time.time;
		if (num < 0f)
		{
			num = 0f;
		}
		GUIManager.DrawText(new Rect(10f, (float)(Screen.height - 40), (float)(Screen.width - 20), 30f), Lang.GetLabel(443) + ((int)num).ToString() + "...", 12, TextAnchor.LowerLeft, 8);
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00012E20 File Offset: 0x00011020
	private void DoWindow(int windowID)
	{
		GUI.DrawTexture(new Rect(0f, 0f, 600f, 32f), GUIManager.tex_panel);
		GUI.DrawTexture(new Rect(0f, 32f, 600f, (float)(Screen.height - 180)), GUIManager.tex_half_black);
		this.DrawMode(Lang.GetLabel(44), 108, 0, 0);
		this.DrawMode(Lang.GetLabel(45), 236, 0, 1);
		this.DrawMode(Lang.GetLabel(486), 364, 0, 2);
		GUI.DrawTexture(new Rect(0f, 32f, 160f, 26f), GUIManager.tex_menubar);
		GUIManager.DrawText(new Rect(0f, 32f, 160f, 26f), Lang.GetLabel(46), 16, TextAnchor.MiddleCenter, 8);
		Rect r = new Rect(0f, 58f, 160f, 32f);
		if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(47), null, 255 == this.drawmode, false, 0))
		{
			this.drawmode = 255;
		}
		r.y += 32f;
		if (this.gamemode[8])
		{
			if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(838), GUIManager.gm8, 8 == this.drawmode, 255 == this.drawmode, 8))
			{
				this.drawmode = 8;
			}
			r.y += 32f;
		}
		if (this.gamemode[1])
		{
			if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(831), GUIManager.gm1, 1 == this.drawmode, 8 == this.drawmode, 12))
			{
				this.drawmode = 1;
			}
			r.y += 32f;
		}
		if (this.gamemode[0])
		{
			if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(830), GUIManager.gm0, this.drawmode == 0, 1 == this.drawmode, 1))
			{
				this.drawmode = 0;
			}
			r.y += 32f;
		}
		if (this.gamemode[2])
		{
			if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(832), GUIManager.gm2, 2 == this.drawmode, this.drawmode == 0, 2))
			{
				this.drawmode = 2;
			}
			r.y += 32f;
		}
		if (this.gamemode[3])
		{
			if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(833), GUIManager.gm3, 3 == this.drawmode, 2 == this.drawmode, 3))
			{
				this.drawmode = 3;
			}
			r.y += 32f;
		}
		if (this.gamemode[4])
		{
			if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(834), GUIManager.gm4, 4 == this.drawmode, 3 == this.drawmode, 4))
			{
				this.drawmode = 4;
			}
			r.y += 32f;
		}
		if (this.gamemode[5])
		{
			if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(835), GUIManager.gm5, 5 == this.drawmode, 4 == this.drawmode, 5))
			{
				this.drawmode = 5;
			}
			r.y += 32f;
		}
		if (this.gamemode[6])
		{
			if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(836), GUIManager.gm6, 6 == this.drawmode, 5 == this.drawmode, 6))
			{
				this.drawmode = 6;
			}
			r.y += 32f;
		}
		if (this.gamemode[7])
		{
			if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(837), GUIManager.gm7, 7 == this.drawmode, 6 == this.drawmode, 7))
			{
				this.drawmode = 7;
			}
			r.y += 32f;
		}
		if (this.gamemode[9])
		{
			if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(839), GUIManager.gm10, 10 == this.drawmode, 9 == this.drawmode, 10))
			{
				this.drawmode = 10;
			}
			r.y += 32f;
		}
		if (this.gamemode[11])
		{
			if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(841), GUIManager.gm11, 11 == this.drawmode, 10 == this.drawmode, 11))
			{
				this.drawmode = 11;
			}
			r.y += 32f;
		}
		if (this.gamemode[13])
		{
			if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(843), GUIManager.gm13, 13 == this.drawmode, 11 == this.drawmode, 13))
			{
				this.drawmode = 13;
			}
			r.y += 32f;
		}
		GUI.DrawTexture(new Rect(r.x, r.y, r.width, 26f), GUIManager.tex_menubar);
		GUIManager.DrawText(new Rect(r.x, r.y, r.width, 26f), Lang.GetLabel(55), 16, TextAnchor.MiddleCenter, 8);
		r.y += 26f;
		if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(56), null, 255 == this.drawfilter, false, 5))
		{
			this.drawfilter = 255;
			this.drawcountryfilter = 0;
		}
		r.y += 32f;
		if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(57), null, this.drawfilter == 0, 255 == this.drawfilter, 6))
		{
			this.drawfilter = 0;
		}
		r.y += 32f;
		if (this.filtercountryBTN[0])
		{
			if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(342), GUIManager.tex_flags_filter[0], 1 == this.drawcountryfilter, this.drawfilter == 0, 12))
			{
				this.drawcountryfilter = 1;
			}
			r.y += 32f;
		}
		if (this.filtercountryBTN[1])
		{
			if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(343), GUIManager.tex_flags_filter[1], 2 == this.drawcountryfilter, 1 == this.drawcountryfilter, 13))
			{
				this.drawcountryfilter = 2;
			}
			r.y += 32f;
		}
		if (this.filtercountryBTN[2])
		{
			if (this.DrawButton(r, GUIManager.tex_button_mode, GUIManager.select_glow, Lang.GetLabel(344), GUIManager.tex_flags_filter[2], 3 == this.drawcountryfilter, 2 == this.drawcountryfilter, 14))
			{
				this.drawcountryfilter = 3;
			}
			r.y += 32f;
		}
		GUI.DrawTexture(new Rect(0f, r.y, 160f, (float)(Screen.height - 180 - 52 + 320)), GUIManager.tex_half_black);
		this.y_pos = 10;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.srvlist.Count; i++)
		{
			if (this.srvlist[i].type == this.type)
			{
				if (this.type == 1 && this.srvlist[i].adminid == 0UL)
				{
					num2++;
				}
				else if ((this.drawfilter == 255 || this.drawfilter != 0 || (this.srvlist[i].players != this.srvlist[i].maxplayers && this.srvlist[i].avaliable_by_lvl)) && (this.drawmode == 255 || this.srvlist[i].gamemode == this.drawmode) && (this.drawcountryfilter == 0 || this.srvlist[i].country_id == this.drawcountryfilter))
				{
					num++;
				}
			}
		}
		float num3 = (float)(num * 36 + 16);
		if (this.type == 1)
		{
			num3 += 40f;
		}
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, 32f, 600f, GUIManager.YRES(768f) - 180f - 32f - 32f), this.scrollViewVector, new Rect(0f, 0f, 0f, num3));
		if (PlayerProfile.loh > 0)
		{
			GUI.DrawTexture(new Rect(178f, 10f, 384f, 32f), GUIManager.tex_warning);
			GUIManager.DrawText(new Rect(210f, 10f, 352f, 32f), Lang.GetLabel(58), 16, TextAnchor.MiddleCenter, 8);
		}
		else
		{
			if (this.type == 1)
			{
				this.DrawCreateServer();
				this.y_pos += 40;
			}
			if (this.type == 2)
			{
				if (PlayerProfile.friendsOnlineServers.Length != 0 && this.online_friends_loaded)
				{
					for (int j = 1; j < PlayerProfile.friendsOnlineServers.Length; j++)
					{
						if (PlayerProfile.friendsOnlineServers[j] != "")
						{
							string[] array = PlayerProfile.friendsOnlineServers[j].Split(new char[]
							{
								'|'
							});
							int num4 = 0;
							int.TryParse(array[2], out num4);
							int num5 = 0;
							int.TryParse(array[3], out num5);
							int num6 = 0;
							int.TryParse(array[4], out num6);
							if ((num6 != 999 || PlayerProfile.level >= 6) && num6 >= PlayerProfile.level)
							{
								for (int k = 0; k < this.srvlist.Count; k++)
								{
									if (this.srvlist[k].port == num4 && !(this.srvlist[k].ip != array[1]) && (this.drawfilter == 255 || this.drawfilter != 0 || (this.srvlist[k].players != this.srvlist[k].maxplayers && this.srvlist[k].avaliable_by_lvl)) && (this.drawmode == 255 || this.srvlist[k].gamemode == this.drawmode))
									{
										this.DrawFriendServer(array, this.srvlist[k]);
										break;
									}
								}
							}
						}
					}
				}
				else if (!this.online_friends_loaded)
				{
					GUI.DrawTexture(new Rect(178f, 10f, 384f, 32f), GUIManager.tex_warning);
					GUIManager.DrawText(new Rect(210f, 10f, 352f, 32f), Lang.GetLabel(487), 16, TextAnchor.MiddleCenter, 8);
				}
				else if (this.online_friends_loaded)
				{
					GUI.DrawTexture(new Rect(178f, 10f, 384f, 32f), GUIManager.tex_warning);
					GUIManager.DrawText(new Rect(210f, 10f, 352f, 32f), Lang.GetLabel(850), 16, TextAnchor.MiddleCenter, 8);
				}
			}
			else if (this.drawmode == 12 && PlayerProfile.skin != 59 && PlayerProfile.skin != 56 && PlayerProfile.skin != 57 && PlayerProfile.skin != 58 && PlayerProfile.skin != 148 && PlayerProfile.skin != 227 && PlayerProfile.skin != 228 && PlayerProfile.skin != 229 && PlayerProfile.skin != 149 && PlayerProfile.skin != 150 && PlayerProfile.skin != 253 && PlayerProfile.skin != 312)
			{
				GUI.DrawTexture(new Rect(178f, 10f, 384f, 54f), GUIManager.tex_warning2);
				GUIManager.DrawText(new Rect(210f, 10f, 352f, 54f), Lang.GetLabel(540), 16, TextAnchor.MiddleCenter, 8);
				this.y_pos += 72;
			}
			else
			{
				for (int l = 0; l < this.srvlist.Count; l++)
				{
					if (this.gamemode[this.srvlist[l].gamemode] && this.srvlist[l].type == this.type && (this.type != 1 || this.srvlist[l].adminid != 0UL) && (this.drawfilter == 255 || this.drawfilter != 0 || (this.srvlist[l].players != this.srvlist[l].maxplayers && this.srvlist[l].avaliable_by_lvl)) && (this.drawmode == 255 || this.srvlist[l].gamemode == this.drawmode) && (this.drawcountryfilter == 0 || this.srvlist[l].country_id == this.drawcountryfilter) && this.srvlist[l].players != 0)
					{
						this.DrawServer(this.srvlist[l]);
					}
				}
				for (int m = 0; m < this.srvlist.Count; m++)
				{
					if (this.gamemode[this.srvlist[m].gamemode] && this.srvlist[m].type == this.type && (this.type != 1 || this.srvlist[m].adminid != 0UL) && (this.drawfilter == 255 || this.drawfilter != 0 || (this.srvlist[m].players != this.srvlist[m].maxplayers && this.srvlist[m].avaliable_by_lvl)) && (this.drawmode == 255 || this.srvlist[m].gamemode == this.drawmode) && (this.drawcountryfilter == 0 || this.srvlist[m].country_id == this.drawcountryfilter) && this.srvlist[m].players <= 0)
					{
						this.DrawServer(this.srvlist[m]);
					}
				}
			}
		}
		if (this.type == 0 && this.drawmode == 2)
		{
			GUI.DrawTexture(new Rect(178f, 10f, 384f, 32f), GUIManager.tex_warning);
			GUIManager.DrawText(new Rect(210f, 10f, 352f, 32f), Lang.GetLabel(59), 16, TextAnchor.MiddleCenter, 8);
		}
		else if (this.type == 0)
		{
			int num7 = this.drawmode;
		}
		GUIManager.EndScrollView();
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00013E48 File Offset: 0x00012048
	private void DrawServer(CServerData server)
	{
		float num = Input.mousePosition.x;
		float num2 = (float)Screen.height - Input.mousePosition.y;
		num -= (float)Screen.width / 2f - 300f;
		num2 -= (float)(192 - (int)this.scrollViewVector.y + 32 + 10);
		Rect position = new Rect(178f, (float)this.y_pos, 384f, 32f);
		Rect position2 = new Rect(193f, (float)this.y_pos, 32f, 32f);
		Rect rect = new Rect(232f, (float)this.y_pos, 200f, 32f);
		Rect r = new Rect(470f, (float)this.y_pos, 80f, 32f);
		if (!PopUp.Active)
		{
			if (position.Contains(new Vector2(num, num2)))
			{
				if (!server.hover)
				{
					server.hover = true;
				}
			}
			else if (server.hover)
			{
				server.hover = false;
			}
			bool hover = server.hover;
			if (GUI.Button(position, "", GUIManager.gs_empty) && server.avaliable_by_lvl)
			{
				MainMenu.MainAS.PlayOneShot(SoundManager.SoundClick, AudioListener.volume);
				if (server.password > 0)
				{
					base.gameObject.GetComponent<PasswordGUI>().Show(true, server);
				}
				else
				{
					ConnectionInfo.PRIVATE = (server.type == 1);
					ConnectionInfo.IP = server.ip;
					ConnectionInfo.PORT = server.port;
					ConnectionInfo.HOSTNAME = server.name;
					ConnectionInfo.mode = server.gamemode;
					GameController.STATE = GAME_STATES.GAME;
					GM.currExtState = GAME_STATES.NULL;
					SceneManager.LoadScene(1);
				}
			}
		}
		if (server.password > 0)
		{
			GUI.color = new Color(1f, 0.7f, 0.7f, 1f);
		}
		else if (!server.avaliable_by_lvl)
		{
			GUI.color = new Color(0.5f, 0.5f, 0.5f, 1f);
		}
		if (server.hover)
		{
			GUI.DrawTexture(position, GUIManager.tex_server_hover);
		}
		else
		{
			GUI.DrawTexture(position, GUIManager.tex_server);
		}
		GUI.color = new Color(1f, 1f, 1f, 1f);
		if (server.gamemode == 0)
		{
			GUI.DrawTexture(position2, GUIManager.gm0);
		}
		else if (server.gamemode == 1)
		{
			GUI.DrawTexture(position2, GUIManager.gm1);
		}
		else if (server.gamemode == 2)
		{
			GUI.DrawTexture(position2, GUIManager.gm2);
		}
		else if (server.gamemode == 3)
		{
			GUI.DrawTexture(position2, GUIManager.gm3);
		}
		else if (server.gamemode == 4)
		{
			GUI.DrawTexture(position2, GUIManager.gm4);
		}
		else if (server.gamemode == 5)
		{
			GUI.DrawTexture(position2, GUIManager.gm5);
		}
		else if (server.gamemode == 6)
		{
			GUI.DrawTexture(position2, GUIManager.gm6);
		}
		else if (server.gamemode == 7)
		{
			GUI.DrawTexture(position2, GUIManager.gm7);
		}
		else if (server.gamemode == 8)
		{
			GUI.DrawTexture(position2, GUIManager.gm8);
		}
		else if (server.gamemode == 9)
		{
			GUI.DrawTexture(position2, GUIManager.gm9);
		}
		else if (server.gamemode == 10)
		{
			GUI.DrawTexture(position2, GUIManager.gm10);
		}
		else if (server.gamemode == 11)
		{
			GUI.DrawTexture(position2, GUIManager.gm11);
		}
		else if (server.gamemode == 12)
		{
			GUI.DrawTexture(position2, GUIManager.gm12);
		}
		else if (server.gamemode == 13)
		{
			GUI.DrawTexture(position2, GUIManager.gm13);
		}
		int num3 = server.country_id - 1;
		if (num3 < 0 || num3 >= GUIManager.tex_flags_filter.Length)
		{
			num3 = 0;
		}
		GUI.DrawTexture(new Rect(rect.x, rect.y + 6f, 20f, 20f), GUIManager.tex_flags_filter[num3]);
		GUIManager.DrawText(new Rect(rect.x + 24f, rect.y, rect.width - 18f, rect.height), server.name, 16, TextAnchor.MiddleLeft, 8);
		GUI.color = new Color(0.8f, 0.8f, 0.8f, 1f);
		GUIManager.DrawText(r, server.players + "/" + server.maxplayers, 20, TextAnchor.MiddleCenter, 8);
		if (!server.avaliable_by_lvl)
		{
			GUI.color = new Color(1f, 0f, 0f, 1f);
			GUI.DrawTexture(new Rect(r.x - 20f, r.y + 8f, 16f, 16f), GUIManager.tex_padlock);
		}
		GUI.color = new Color(1f, 1f, 1f, 1f);
		this.y_pos += 36;
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00014328 File Offset: 0x00012528
	private void DrawFriendServer(string[] _fInfo, CServerData server)
	{
		float num = Input.mousePosition.x;
		float num2 = (float)Screen.height - Input.mousePosition.y;
		num -= (float)Screen.width / 2f - 300f;
		num2 -= (float)(192 - (int)this.scrollViewVector.y + 44 + 10);
		Rect position = new Rect(178f, (float)this.y_pos, 384f, 44f);
		Rect position2 = new Rect(193f, (float)(this.y_pos + 6), 32f, 32f);
		Rect rect = new Rect(232f, (float)this.y_pos, 200f, 44f);
		Rect r = new Rect(470f, (float)(this.y_pos + 6), 80f, 32f);
		if (!PopUp.Active)
		{
			if (position.Contains(new Vector2(num, num2)))
			{
				if (!server.hover)
				{
					server.hover = true;
				}
			}
			else if (server.hover)
			{
				server.hover = false;
			}
			bool hover = server.hover;
			if (GUI.Button(position, "", GUIManager.gs_empty) && server.avaliable_by_lvl)
			{
				MainMenu.MainAS.PlayOneShot(SoundManager.SoundClick, AudioListener.volume);
				if (server.password > 0)
				{
					base.gameObject.GetComponent<PasswordGUI>().Show(true, server);
				}
				else
				{
					ConnectionInfo.PRIVATE = (server.type == 1);
					ConnectionInfo.IP = server.ip;
					ConnectionInfo.PORT = server.port;
					ConnectionInfo.HOSTNAME = server.name;
					ConnectionInfo.mode = server.gamemode;
					GameController.STATE = GAME_STATES.GAME;
					GM.currExtState = GAME_STATES.NULL;
					SceneManager.LoadScene(1);
				}
			}
		}
		if (server.password > 0)
		{
			GUI.color = new Color(1f, 0.7f, 0.7f, 1f);
		}
		if (server.hover)
		{
			GUI.DrawTexture(position, GUIManager.tex_server_hover);
		}
		else
		{
			GUI.DrawTexture(position, GUIManager.tex_server);
		}
		GUI.color = new Color(1f, 1f, 1f, 1f);
		if (server.gamemode == 0)
		{
			GUI.DrawTexture(position2, GUIManager.gm0);
		}
		else if (server.gamemode == 1)
		{
			GUI.DrawTexture(position2, GUIManager.gm1);
		}
		else if (server.gamemode == 2)
		{
			GUI.DrawTexture(position2, GUIManager.gm2);
		}
		else if (server.gamemode == 3)
		{
			GUI.DrawTexture(position2, GUIManager.gm3);
		}
		else if (server.gamemode == 4)
		{
			GUI.DrawTexture(position2, GUIManager.gm4);
		}
		else if (server.gamemode == 5)
		{
			GUI.DrawTexture(position2, GUIManager.gm5);
		}
		else if (server.gamemode == 6)
		{
			GUI.DrawTexture(position2, GUIManager.gm6);
		}
		else if (server.gamemode == 7)
		{
			GUI.DrawTexture(position2, GUIManager.gm7);
		}
		else if (server.gamemode == 8)
		{
			GUI.DrawTexture(position2, GUIManager.gm8);
		}
		else if (server.gamemode == 9)
		{
			GUI.DrawTexture(position2, GUIManager.gm9);
		}
		else if (server.gamemode == 10)
		{
			GUI.DrawTexture(position2, GUIManager.gm10);
		}
		else if (server.gamemode == 11)
		{
			GUI.DrawTexture(position2, GUIManager.gm11);
		}
		GUI.DrawTexture(new Rect(rect.x, rect.y + 22f, 20f, 20f), GUIManager.tex_flags_filter[server.country_id - 1]);
		GUIManager.DrawText(new Rect(rect.x + 24f, rect.y, rect.width - 18f, rect.height), PlayerProfile.friendsOnline[_fInfo[0]] + "\n" + server.name, 16, TextAnchor.MiddleLeft, 8);
		GUI.color = new Color(0.8f, 0.8f, 0.8f, 1f);
		GUIManager.DrawText(r, server.players + "/" + server.maxplayers, 20, TextAnchor.MiddleCenter, 8);
		GUI.color = new Color(1f, 1f, 1f, 1f);
		this.y_pos += 50;
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00014754 File Offset: 0x00012954
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
			this.drawmode = 255;
			if (this.type == 2)
			{
				this.online_friends_loaded = false;
				PlayerProfile.friendsOnline.Clear();
				this.online_friends_loaded = SteamHandler.GetFriends();
				base.StartCoroutine(Handler.GetOnlineFriends());
			}
			if (this.lastupdate + (float)CONST.CFG.SERVER_UPDATE_TIMEOUT < Time.time)
			{
				this.refresh_servers();
			}
		}
	}

	// Token: 0x060000FE RID: 254 RVA: 0x000148EC File Offset: 0x00012AEC
	private void DrawCreateServer()
	{
		float num = Input.mousePosition.x;
		float num2 = (float)Screen.height - Input.mousePosition.y;
		num -= (float)Screen.width / 2f - 300f;
		num2 -= (float)(192 - (int)this.scrollViewVector.y + 39);
		Rect rect = new Rect(280f, 4f, 192f, 32f);
		if (rect.Contains(new Vector2(num, num2)))
		{
			if (!this.bh)
			{
				this.bh = true;
				MainMenu.MainAS.PlayOneShot(SoundManager.SoundHover, AudioListener.volume);
			}
		}
		else if (this.bh)
		{
			this.bh = false;
		}
		if (this.bh)
		{
			rect.y -= 2f;
		}
		GUI.DrawTexture(rect, GUIManager.tex_button_hover);
		GUIManager.DrawText(rect, Lang.GetLabel(61), 16, TextAnchor.MiddleCenter, 8);
		if (Time.time - this.request < 5f)
		{
			return;
		}
		if (GUI.Button(rect, "", GUIManager.gs_empty))
		{
			this.request = Time.time;
			if (PlayerProfile.premium == 1)
			{
				base.StartCoroutine(this.create_server());
				return;
			}
			PopUp.ShowBonus(4, 0);
		}
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00014A28 File Offset: 0x00012C28
	private IEnumerator create_server()
	{
		while (PlayerProfile.level <= 0)
		{
			yield return null;
		}
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER_LIST,
			"16&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&time=",
			DateTime.Now.Second,
			"&LVL=",
			PlayerProfile.level
		});
		WWW www = new WWW(url, null, new Dictionary<string, string>
		{
			{
				"User-Agent",
				"Mozilla / 5.0(Windows NT 10.0; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 55.0.2883.87 Safari / 537.36"
			}
		});
		yield return www;
		if (www.error == null)
		{
			string[] array = www.text.Split(new char[]
			{
				'|'
			});
			if (array.Length <= 1)
			{
				yield break;
			}
			int port = 0;
			int mode = 0;
			int.TryParse(array[1], out port);
			int.TryParse(array[2], out mode);
			MainMenu.MainAS.PlayOneShot(SoundManager.SoundClick, AudioListener.volume);
			ConnectionInfo.PRIVATE = true;
			ConnectionInfo.IP = array[0];
			ConnectionInfo.PORT = port;
			ConnectionInfo.HOSTNAME = "";
			ConnectionInfo.mode = mode;
			GameController.STATE = GAME_STATES.GAME;
			GM.currExtState = GAME_STATES.NULL;
			SceneManager.LoadScene(1);
		}
		yield break;
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00014A30 File Offset: 0x00012C30
	private bool DrawButton(Rect r, Texture2D tex, Texture2D tex2, string text, Texture2D icon, bool sel, bool presel, int id)
	{
		bool result = false;
		if (GUI.Button(r, "", GUIManager.gs_style3))
		{
			result = true;
		}
		if (sel)
		{
			tex = tex2;
			GUI.DrawTexture(new Rect(r.x - 15f, r.y - 16f, 190f, 64f), tex);
		}
		else
		{
			GUI.DrawTexture(r, tex);
		}
		if (presel)
		{
			GUI.DrawTexture(new Rect(r.x - 15f, r.y, 190f, 16f), GUIManager.hover_part_glow);
		}
		if (!sel)
		{
			float num = Input.mousePosition.x;
			float num2 = (float)Screen.height - Input.mousePosition.y;
			num -= (float)Screen.width / 2f - 300f;
			num2 -= 199f;
			if (r.Contains(new Vector2(num, num2)))
			{
				if (!this.hoverleftmenu[id])
				{
					this.hoverleftmenu[id] = true;
				}
			}
			else if (this.hoverleftmenu[id])
			{
				this.hoverleftmenu[id] = false;
			}
			if (this.hoverleftmenu[id])
			{
				GUI.DrawTexture(new Rect(r.x - 28f, r.y - 16f, 64f, 64f), GUIManager.hover_glow);
				r.x += 8f;
			}
		}
		if (icon)
		{
			if (id == 8)
			{
				GUI.color = Color.red;
			}
			GUI.DrawTexture(new Rect(r.x + 4f, r.y, 32f, 32f), icon);
			GUI.color = Color.white;
			GUIManager.DrawText(new Rect(r.x + 40f, r.y, r.width - 40f, r.height), text, 16, TextAnchor.MiddleLeft, 8);
		}
		else
		{
			GUIManager.DrawText(new Rect(r.x + 12f, r.y, r.width - 12f, r.height), text, 16, TextAnchor.MiddleLeft, 8);
		}
		return result;
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00014C50 File Offset: 0x00012E50
	public void ShuffleList<T>(List<T> list)
	{
		Random random = new Random();
		for (int i = 0; i < list.Count; i++)
		{
			T item = list[i];
			list.RemoveAt(i);
			list.Insert(random.Next(0, list.Count), item);
		}
	}

	// Token: 0x0400010F RID: 271
	public static ServerList THIS;

	// Token: 0x04000110 RID: 272
	public bool Active = true;

	// Token: 0x04000111 RID: 273
	private int drawmode = 255;

	// Token: 0x04000112 RID: 274
	private int drawfilter = 255;

	// Token: 0x04000113 RID: 275
	private int drawcountryfilter;

	// Token: 0x04000114 RID: 276
	private int type;

	// Token: 0x04000115 RID: 277
	private bool[] hovermode = new bool[3];

	// Token: 0x04000116 RID: 278
	private bool[] hoverleftmenu = new bool[15];

	// Token: 0x04000117 RID: 279
	private bool[] filtercountry = new bool[3];

	// Token: 0x04000118 RID: 280
	private bool[] filtercountryBTN = new bool[3];

	// Token: 0x04000119 RID: 281
	private List<CServerData> srvlist = new List<CServerData>();

	// Token: 0x0400011A RID: 282
	private float lastupdate;

	// Token: 0x0400011B RID: 283
	private bool _get_stats;

	// Token: 0x0400011C RID: 284
	private float next_update;

	// Token: 0x0400011D RID: 285
	private bool[] gamemode = new bool[15];

	// Token: 0x0400011E RID: 286
	private bool online_friends_loaded;

	// Token: 0x0400011F RID: 287
	private int x_pos;

	// Token: 0x04000120 RID: 288
	private int y_pos;

	// Token: 0x04000121 RID: 289
	private Vector2 scrollViewVector = Vector2.zero;

	// Token: 0x04000122 RID: 290
	private bool bh;

	// Token: 0x04000123 RID: 291
	private float request = -5f;
}
