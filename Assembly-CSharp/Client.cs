using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using AssemblyCSharp;
using Ionic.Zlib;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000094 RID: 148
public class Client : MonoBehaviour
{
	// Token: 0x0600045C RID: 1116 RVA: 0x00054F3C File Offset: 0x0005313C
	private void Awake()
	{
		Client.THIS = this;
		EntManager.Clear();
		this.LocalPlayer = base.gameObject;
		this.Player = (vp_FPPlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_FPPlayerEventHandler));
		this.active = false;
		this.goMap = GameObject.Find("Map");
		this.map = this.goMap.GetComponent<Map>();
		this.ziploader = this.goMap.GetComponent<ZipLoader>();
		this.csrm = this.goMap.GetComponent<RagDollManager>();
		this.cspm = (ParticleManager)Object.FindObjectOfType(typeof(ParticleManager));
		this.goGui = GameObject.Find("GUI");
		this.csTeamScore = this.goGui.GetComponent<TeamScore>();
		for (int i = 0; i < 256; i++)
		{
			this.net_stats_packet[i] = 0;
			this.net_stats_size[i] = 0;
		}
		base.StartCoroutine(this.LateConnect());
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x00055039 File Offset: 0x00053239
	private IEnumerator LateConnect()
	{
		MonoBehaviour.print("[ START CONNECT ]");
		yield return new WaitForSeconds(0f);
		if (ConnectionInfo.waitandreconnect)
		{
			this.goGui.GetComponent<LoadScreen>().need_rename = false;
			this.goGui.GetComponent<LoadScreen>().loadtext = Lang.GetLabel(151) + " 3";
			yield return new WaitForSeconds(1f);
			this.goGui.GetComponent<LoadScreen>().loadtext = Lang.GetLabel(151) + " 2";
			yield return new WaitForSeconds(1f);
			this.goGui.GetComponent<LoadScreen>().loadtext = Lang.GetLabel(151) + " 1";
			yield return new WaitForSeconds(1.25f);
			this.goGui.GetComponent<LoadScreen>().loadtext = Lang.GetLabel(152);
			this.goGui.GetComponent<LoadScreen>().need_rename = true;
		}
		ConnectionInfo.waitandreconnect = false;
		try
		{
			this.client = new TcpClient(ConnectionInfo.IP, ConnectionInfo.PORT);
			this.client.NoDelay = true;
			this.client.GetStream().BeginRead(this.readBuffer, 0, 30000, new AsyncCallback(this.DoRead), null);
			Debug.Log("connected");
			this.active = true;
		}
		catch
		{
			this.active = false;
			Debug.Log("Server is not active.");
		}
		if (!this.active)
		{
			this.active = true;
			this.Disconnect();
			yield break;
		}
		MonoBehaviour.print("[ END CONNECT ]");
		GM.currExtState = GAME_STATES.GAMEAUTH;
		base.StartCoroutine(this.LateDummy());
		yield break;
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x00055048 File Offset: 0x00053248
	private IEnumerator LateDummy()
	{
		yield return new WaitForSeconds(0.1f);
		this.send_dummy();
		base.StartCoroutine(this.LateAuth());
		yield break;
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x00055057 File Offset: 0x00053257
	private IEnumerator LateAuth()
	{
		yield return new WaitForSeconds(0.1f);
		this.send_auth();
		yield break;
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x00055066 File Offset: 0x00053266
	private void SendPacket()
	{
		this.client.GetStream().Write(this.sendbuffer, 0, this.WRITE_LEN());
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x00055088 File Offset: 0x00053288
	private void Update()
	{
		if (!this.active)
		{
			return;
		}
		if (this.inited && PlayerProfile.loh > 0 && !this.potratil)
		{
			this.potratil = this.send_bonus();
		}
		List<Client.RecvData> tlist = this.Tlist;
		lock (tlist)
		{
			for (int i = 0; i < this.Tlist.Count; i++)
			{
				this.ProcessData(this.Tlist[i].Buffer, this.Tlist[i].Len);
			}
			this.Tlist.Clear();
		}
		int tickCount = Environment.TickCount;
		if (tickCount < this.oldtime + 66)
		{
			return;
		}
		this.oldtime = tickCount;
		if (this.cspc == null)
		{
			this.cspc = this.LocalPlayer.GetComponent<PlayerControl>();
			return;
		}
		if (this.cspc.isSpectator())
		{
			return;
		}
		byte state = 0;
		if (this.Player.Walk.Active)
		{
			state = 1;
		}
		if (this.Player.Crouch.Active)
		{
			state = 2;
		}
		this.send_position(state);
		if (!this.inited)
		{
			int num = 0;
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				this.ass_list.Add(num, assembly.FullName);
				num++;
			}
			this.inited = true;
		}
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x0000248C File Offset: 0x0000068C
	private void OnApplicationQuit()
	{
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x0005520C File Offset: 0x0005340C
	private void DoRead(IAsyncResult ar)
	{
		try
		{
			this.BytesRead = this.client.GetStream().EndRead(ar);
			if (this.BytesRead < 1)
			{
				MonoBehaviour.print("BytesRead < 1");
				this.Disconnect();
			}
			else
			{
				this.SplitRead += this.BytesRead;
				while (this.SplitRead >= 4)
				{
					int num = this.DecodeShort(this.readBuffer, 2);
					if (this.SplitRead < num)
					{
						break;
					}
					List<Client.RecvData> tlist = this.Tlist;
					lock (tlist)
					{
						this.Tlist.Add(new Client.RecvData(this.readBuffer, num));
					}
					int num2 = 0;
					for (int i = num; i < this.SplitRead; i++)
					{
						this.readBuffer[num2] = this.readBuffer[i];
						num2++;
					}
					this.SplitRead -= num;
				}
				this.client.GetStream().BeginRead(this.readBuffer, this.SplitRead, 30000, new AsyncCallback(this.DoRead), null);
			}
		}
		catch (Exception ex)
		{
			MonoBehaviour.print("CATCH " + ex.Message);
		}
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x00055378 File Offset: 0x00053578
	private void Disconnect()
	{
		byte[] buffer = new byte[]
		{
			245,
			byte.MaxValue
		};
		List<Client.RecvData> tlist = this.Tlist;
		lock (tlist)
		{
			this.Tlist.Add(new Client.RecvData(buffer, 2));
		}
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x000553DC File Offset: 0x000535DC
	private void DisconnectByError()
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(16, true);
		this.WRITE_BYTE((byte)this.myindex);
		this.END_WRITE();
		List<Client.RecvData> tlist = this.Tlist;
		lock (tlist)
		{
			this.Tlist.Add(new Client.RecvData(this.sendbuffer, this.WRITE_LEN()));
		}
		this.active = false;
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x00055460 File Offset: 0x00053660
	private void ProcessData(byte[] buffer, int len)
	{
		if (len < 2)
		{
			return;
		}
		if (buffer[0] != 245)
		{
			Debug.Log("Incorrect packet header");
			return;
		}
		if (this.net_stats)
		{
			this.net_stats_packet[(int)buffer[1]]++;
			this.net_stats_size[(int)buffer[1]] += len;
		}
		byte b = buffer[1];
		switch (b)
		{
		case 0:
			this.recv_auth(buffer, len);
			return;
		case 1:
			this.recv_position(buffer, len);
			return;
		case 2:
			this.recv_playerinfo(buffer, len);
			return;
		case 3:
			this.recv_myinfo(buffer, len);
			return;
		case 4:
			this.recv_blockattack(buffer, len);
			return;
		case 5:
			this.recv_blockinfo(buffer, len);
			return;
		case 6:
			this.recv_block_destroy(buffer, len);
			return;
		case 7:
			this.recv_damage(buffer, len);
			return;
		case 8:
			this.recv_scores(buffer, len);
			return;
		case 9:
			this.recv_jointeamclass(buffer, len);
			return;
		case 10:
			this.recv_spawn(buffer, len);
			return;
		case 11:
			this.recv_attack_milk(buffer, len);
			return;
		case 12:
			this.recv_setblock(buffer, len);
			return;
		case 13:
			this.recv_chat(buffer, len);
			return;
		case 14:
			this.recv_stats(buffer, len);
			return;
		case 15:
			this.recv_currentweapon(buffer, len);
			return;
		case 16:
			this.recv_disconnect(buffer, len);
			return;
		case 17:
			this.recv_reconnect(buffer, len);
			return;
		case 18:
			this.recv_endofsnap(buffer, len);
			return;
		case 19:
		case 20:
		case 25:
		case 29:
		case 37:
		case 38:
		case 39:
		case 41:
		case 46:
		case 47:
		case 48:
		case 49:
		case 59:
		case 60:
		case 62:
		case 64:
		case 65:
		case 72:
		case 73:
		case 74:
		case 75:
		case 76:
		case 77:
		case 78:
		case 79:
		case 80:
		case 81:
		case 82:
		case 83:
		case 84:
		case 85:
		case 86:
		case 87:
		case 88:
		case 89:
		case 90:
		case 91:
		case 92:
		case 93:
		case 94:
		case 95:
		case 96:
		case 97:
		case 98:
		case 99:
		case 100:
		case 106:
			break;
		case 21:
			this.recv_buildblock(buffer, len);
			return;
		case 22:
			this.recv_my_data(buffer, len);
			return;
		case 23:
			this.recv_damage_helmet(buffer, len);
			return;
		case 24:
			this.recv_createent(buffer, len);
			return;
		case 26:
			this.recv_destroy_status(buffer, len);
			return;
		case 27:
			this.recv_explode(buffer, len);
			return;
		case 28:
			this.recv_private_info(buffer, len);
			return;
		case 30:
			this.recv_reconnect2(buffer, len);
			return;
		case 31:
			this.recv_ready_for_spawn(buffer, len);
			return;
		case 32:
			this.recv_spawnequip(buffer, len);
			return;
		case 33:
			this.recv_zm_countdown(buffer, len);
			return;
		case 34:
			this.recv_zm_infect(buffer, len);
			return;
		case 35:
			this.recv_zm_message(buffer, len);
			return;
		case 36:
			this.recv_sethealth(buffer, len);
			return;
		case 40:
			this.recv_message(buffer, len);
			return;
		case 42:
			this.recv_ct_radar(buffer, len);
			return;
		case 43:
			this.recv_damage_armor(buffer, len);
			return;
		case 44:
			this.recv_sound_fx(buffer, len);
			return;
		case 45:
			this.recv_reposition(buffer, len);
			return;
		case 50:
			this.recv_moveent(buffer, len);
			return;
		case 51:
			this.recv_destroyent(buffer, len);
			return;
		case 52:
			this.recv_gamemessage(buffer, len);
			return;
		case 53:
			this.recv_equip(buffer, len);
			return;
		case 54:
			this.recv_entposition(buffer, len);
			return;
		case 55:
			this.recv_moveboss(buffer, len);
			return;
		case 56:
			this.recv_liftup(buffer, len);
			return;
		case 57:
			this.recv_playerinfo2(buffer, len);
			return;
		case 58:
			this.recv_player_update(buffer, len);
			return;
		case 61:
			this.recv_enter_the_ent(buffer, len);
			return;
		case 63:
			this.recv_exit_the_ent(buffer, len);
			return;
		case 66:
			this.recv_vehicle_turret(buffer, len);
			return;
		case 67:
			this.recv_vehicle_health(buffer, len);
			return;
		case 68:
			this.recv_vehicle_explode(buffer, len);
			return;
		case 69:
			this.recv_vehicle_hit(buffer, len);
			return;
		case 70:
			this.recv_vehicle_targeting(buffer, len);
			return;
		case 71:
			this.recv_ent_health(buffer, len);
			return;
		case 101:
			this.recv_zplayerpos(buffer, len);
			return;
		case 102:
			this.recv_zentpos(buffer, len);
			return;
		case 103:
			this.recv_chunk(buffer, len);
			return;
		case 104:
			this.recv_chunk_finish(buffer, len);
			return;
		case 105:
			this.recv_mapdata(buffer, len);
			return;
		case 107:
			this.recv_flag_set(buffer, len);
			return;
		case 108:
			this.recv_flag_update(buffer, len);
			return;
		case 109:
			this.recv_accept_weapons(buffer, len);
			return;
		case 110:
			this.recv_selected_block(buffer, len);
			return;
		default:
			if (b == 200)
			{
				this.recv_auth_ready(buffer, len);
				return;
			}
			if (b == 255)
			{
				this.recv_app_disconnect();
				return;
			}
			break;
		}
		MonoBehaviour.print("incorrect packet " + buffer[1].ToString());
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x00055901 File Offset: 0x00053B01
	private void recv_auth_ready(byte[] buffer, int len)
	{
		this.ready_for_auth = true;
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x0005590C File Offset: 0x00053B0C
	private void recv_auth(byte[] buffer, int len)
	{
		PlayerProfile.myteam = -1;
		int num = 4;
		this.mode = (int)buffer[num];
		num++;
		CONST.SetGameMode((MODE)this.mode);
		ConnectionInfo.mode = this.mode;
		this.LocalPlayer.GetComponent<PlayerControl>().SetSky(this.mode, false);
		this.ziploader.gamemode = (MODE)this.mode;
		int num2 = this.DecodeInteger(buffer, num);
		num += 4;
		string @string = Encoding.UTF8.GetString(buffer, num, num2);
		if (num2 > 0)
		{
			GM.currExtState = GAME_STATES.GAMEAUTHCOMPLITE;
			this.LocalPlayer.GetComponent<PlayerControl>().StartMap(@string);
		}
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x000559A4 File Offset: 0x00053BA4
	private void recv_position(byte[] buffer, int len)
	{
		int i = 4;
		while (i < len)
		{
			int num = (int)buffer[i];
			i++;
			if (num == this.myindex)
			{
				i += 9;
			}
			else
			{
				ushort num2 = this.DecodeShort2(buffer, i);
				i += 2;
				ushort num3 = this.DecodeShort2(buffer, i);
				i += 2;
				float num4 = (float)this.DecodeShort2(buffer, i);
				i += 2;
				float pX = (float)num2 * 0.00390625f;
				float pY = (float)num3 * 0.00390625f;
				float pZ = num4 * 0.00390625f;
				float rX = (float)buffer[i] * 360f / 256f;
				i++;
				float rY = (float)buffer[i] * 360f / 256f;
				i++;
				int state = (int)buffer[i];
				i++;
				RemotePlayersUpdater.Instance.UpdatePosition(num, pX, pY, pZ, rX, rY, 0f, state);
			}
		}
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x00055A70 File Offset: 0x00053C70
	private void recv_playerinfo(byte[] buffer, int len)
	{
		int num = 4;
		byte b = buffer[num];
		num++;
		int num2 = (int)buffer[num];
		num++;
		int num3 = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Name = Encoding.UTF8.GetString(buffer, num, num3);
		num += num3;
		num3 = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].ClanName = Encoding.UTF8.GetString(buffer, num, num3);
		num += num3 + 1;
		RemotePlayersUpdater.Instance.Bots[num2].Dead = buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Team = buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Stats_Kills = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Stats_Deads = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].CountryID = buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Helmet = buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[54] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[146] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[147] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Skin = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Znak = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[6] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[136] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[198] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[211] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[222] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[223] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[224] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[225] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[226] = (int)buffer[num];
		num++;
		if (RemotePlayersUpdater.Instance.Bots[num2].Item[211] > 0)
		{
			RemotePlayersUpdater.Instance.Bots[num2].goTykva.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			RemotePlayersUpdater.Instance.Bots[num2].goTykva.GetComponent<Renderer>().enabled = false;
		}
		if (RemotePlayersUpdater.Instance.Bots[num2].Item[222] > 0)
		{
			RemotePlayersUpdater.Instance.Bots[num2].goKolpak.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			RemotePlayersUpdater.Instance.Bots[num2].goKolpak.GetComponent<Renderer>().enabled = false;
		}
		if (RemotePlayersUpdater.Instance.Bots[num2].Item[223] > 0)
		{
			RemotePlayersUpdater.Instance.Bots[num2].goRoga.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			RemotePlayersUpdater.Instance.Bots[num2].goRoga.GetComponent<Renderer>().enabled = false;
		}
		if (RemotePlayersUpdater.Instance.Bots[num2].Item[224] > 0)
		{
			RemotePlayersUpdater.Instance.Bots[num2].goMaskBear.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			RemotePlayersUpdater.Instance.Bots[num2].goMaskBear.GetComponent<Renderer>().enabled = false;
		}
		if (RemotePlayersUpdater.Instance.Bots[num2].Item[225] > 0)
		{
			RemotePlayersUpdater.Instance.Bots[num2].goMaskFox.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			RemotePlayersUpdater.Instance.Bots[num2].goMaskFox.GetComponent<Renderer>().enabled = false;
		}
		if (RemotePlayersUpdater.Instance.Bots[num2].Item[226] > 0)
		{
			RemotePlayersUpdater.Instance.Bots[num2].goMaskRabbit.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			RemotePlayersUpdater.Instance.Bots[num2].goMaskRabbit.GetComponent<Renderer>().enabled = false;
		}
		RemotePlayersUpdater.Instance.SetPlayerActive(num2, true);
		if (CONST.GetGameMode() == MODE.ZOMBIE && RemotePlayersUpdater.Instance.Bots[num2].Team == 1)
		{
			RemotePlayersUpdater.Instance.Bots[num2].zombie = true;
			RemotePlayersUpdater.Instance.Bots[num2].Team = 1;
			RemotePlayersUpdater.Instance.Bots[num2].Dead = 0;
			RemotePlayersUpdater.Instance.SetCurrentWeapon(num2, 35);
			RemotePlayersUpdater.Instance.Bots[num2].WeaponID = 35;
			RemotePlayersUpdater.Instance.BotsGmObj[num2].GetComponent<TeamColor>().SetTeam(0, 1, RemotePlayersUpdater.Instance.Bots[num2].goHelmet, RemotePlayersUpdater.Instance.Bots[num2].goCap, RemotePlayersUpdater.Instance.Bots[num2].Znak);
			RemotePlayersUpdater.Instance.BotsGmObj[num2].GetComponent<Animator>().SetBool("isZombie", true);
			RemotePlayersUpdater.Instance.BotsGmObj[num2].GetComponent<Sound>().PlaySound_ZM_Infected();
			RemotePlayersUpdater.Instance.BotsGmObj[num2].GetComponent<FX>().Infect();
			RemotePlayersUpdater.Instance.BotsGmObj[num2].layer = 15;
			return;
		}
		RemotePlayersUpdater.Instance.BotsGmObj[num2].GetComponent<TeamColor>().SetTeam((int)RemotePlayersUpdater.Instance.Bots[num2].Team, RemotePlayersUpdater.Instance.Bots[num2].Skin, RemotePlayersUpdater.Instance.Bots[num2].goHelmet, RemotePlayersUpdater.Instance.Bots[num2].goCap, RemotePlayersUpdater.Instance.Bots[num2].Znak);
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x000560C0 File Offset: 0x000542C0
	private void recv_myinfo(byte[] buffer, int len)
	{
		int num = 4;
		this.myindex = (int)buffer[num];
		num++;
		PlayerProfile.myindex = this.myindex;
		int num2 = (int)buffer[num];
		num++;
		int num3 = (int)buffer[num];
		num++;
		int num4 = (int)buffer[num];
		num++;
		this.goGui.GetComponent<Radar>().team_pos[0] = new Vector3((float)num2, (float)num3, (float)num4);
		num2 = (int)buffer[num];
		num++;
		num3 = (int)buffer[num];
		num++;
		num4 = (int)buffer[num];
		num++;
		this.goGui.GetComponent<Radar>().team_pos[1] = new Vector3((float)num2, (float)num3, (float)num4);
		num2 = (int)buffer[num];
		num++;
		num3 = (int)buffer[num];
		num++;
		num4 = (int)buffer[num];
		num++;
		this.goGui.GetComponent<Radar>().team_pos[2] = new Vector3((float)num2, (float)num3, (float)num4);
		num2 = (int)buffer[num];
		num++;
		num3 = (int)buffer[num];
		num++;
		num4 = (int)buffer[num];
		num++;
		this.goGui.GetComponent<Radar>().team_pos[3] = new Vector3((float)num2, (float)num3, (float)num4);
		num2 = (int)buffer[num];
		num++;
		num3 = (int)buffer[num];
		num++;
		num4 = (int)buffer[num];
		num++;
		int num5 = (int)buffer[num];
		num++;
		int num6 = (int)buffer[num];
		num++;
		int num7 = (int)buffer[num];
		num++;
		int num8 = (int)buffer[num];
		num++;
		int num9 = (int)buffer[num];
		num++;
		int num10 = (int)buffer[num];
		num++;
		this.map.mlx = new Vector2((float)num5, (float)num6);
		this.map.mly = new Vector2((float)num7, (float)num8);
		this.map.mlz = new Vector2((float)num9, (float)num10);
		this.goGui.GetComponent<Radar>().base_pos = new Vector3((float)num2, (float)num3, (float)num4);
		if (CONST.GetGameMode() == MODE.BATTLE)
		{
			HoloBase.Create(new Vector3((float)num2, (float)num3, (float)num4), 0);
		}
		if (CONST.GetGameMode() == MODE.PRORIV)
		{
			HoloBase.Create(new Vector3((float)num2, (float)(num3 + 1), (float)num4), 9);
		}
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x000562B0 File Offset: 0x000544B0
	private void recv_blockattack(byte[] buffer, int len)
	{
		int num = 4;
		int x = (int)buffer[num];
		num++;
		int y = (int)buffer[num];
		num++;
		int z = (int)buffer[num];
		num++;
		int health = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.UpdateBlock(x, y, z, health, true);
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x000562F0 File Offset: 0x000544F0
	private void recv_blockinfo(byte[] buffer, int len)
	{
		int i = 4;
		while (i < len)
		{
			if (i + 4 > len)
			{
				return;
			}
			int x = (int)buffer[i];
			i++;
			int y = (int)buffer[i];
			i++;
			int z = (int)buffer[i];
			i++;
			int num = (int)buffer[i];
			i++;
			if (num == 0)
			{
				this.map.SetBlock(default(BlockData), new Vector3i(x, y, z));
			}
			else
			{
				this.ziploader.SetBlock(x, y, z, num);
			}
		}
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x00056360 File Offset: 0x00054560
	private void recv_block_destroy(byte[] buffer, int len)
	{
		int i = 4;
		while (i < len)
		{
			if (i + 3 > len)
			{
				return;
			}
			int x = (int)buffer[i];
			i++;
			int y = (int)buffer[i];
			i++;
			int z = (int)buffer[i];
			i++;
			this.pos.Add(new Vector3i(x, y, z));
		}
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x000563A8 File Offset: 0x000545A8
	private void recv_scores(byte[] buffer, int len)
	{
		int num = 4;
		int score = this.DecodeInteger(buffer, num);
		num += 4;
		int score2 = this.DecodeInteger(buffer, num);
		num += 4;
		int score3 = this.DecodeInteger(buffer, num);
		num += 4;
		int score4 = this.DecodeInteger(buffer, num);
		num += 4;
		int timer = this.DecodeInteger(buffer, num);
		num += 4;
		this.csTeamScore.UpdateScore(score, score2, score3, score4, timer);
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0005640C File Offset: 0x0005460C
	private void recv_jointeamclass(byte[] buffer, int len)
	{
		int num = 4;
		byte b = buffer[num];
		num++;
		byte b2 = buffer[num];
		num++;
		RemotePlayersUpdater.Instance.JoinTeamClass((int)b, (int)b2);
		if ((int)b == this.myindex)
		{
			PlayerProfile.myteam = (int)b2;
			if (this.MG == null)
			{
				this.MG = (MainGUI)Object.FindObjectOfType(typeof(MainGUI));
			}
			this.MG.CloseAll();
			this.MG.OpenEMenu();
		}
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x00056484 File Offset: 0x00054684
	private void recv_spawn(byte[] buffer, int len)
	{
		int num = 4;
		byte b = buffer[num];
		num++;
		byte b2 = buffer[num];
		num++;
		byte b3 = buffer[num];
		num++;
		byte b4 = buffer[num];
		num++;
		byte b5 = buffer[num];
		num++;
		byte b6 = buffer[num];
		num++;
		byte b7 = buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[(int)b].Item[211] = 0;
		RemotePlayersUpdater.Instance.Bots[(int)b].Item[222] = 0;
		RemotePlayersUpdater.Instance.Bots[(int)b].Item[223] = 0;
		RemotePlayersUpdater.Instance.Bots[(int)b].Item[224] = 0;
		RemotePlayersUpdater.Instance.Bots[(int)b].Item[225] = 0;
		RemotePlayersUpdater.Instance.Bots[(int)b].Item[226] = 0;
		if (b5 != 211)
		{
			if (b5 == 222)
			{
				RemotePlayersUpdater.Instance.Bots[(int)b].Item[222] = 1;
			}
		}
		else
		{
			RemotePlayersUpdater.Instance.Bots[(int)b].Item[211] = 1;
		}
		switch (b6)
		{
		case 224:
			RemotePlayersUpdater.Instance.Bots[(int)b].Item[224] = 1;
			break;
		case 225:
			RemotePlayersUpdater.Instance.Bots[(int)b].Item[225] = 1;
			break;
		case 226:
			RemotePlayersUpdater.Instance.Bots[(int)b].Item[226] = 1;
			break;
		}
		if (b7 == 223)
		{
			RemotePlayersUpdater.Instance.Bots[(int)b].Item[223] = 1;
		}
		RemotePlayersUpdater.Instance.SetPosition((int)b, (float)b2, (float)b3, (float)b4);
		RemotePlayersUpdater.Instance.Bots[(int)b].Dead = 0;
		SkinnedMeshRenderer[] componentsInChildren = RemotePlayersUpdater.Instance.BotsGmObj[(int)b].GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		if (RemotePlayersUpdater.Instance.Bots[(int)b].Helmet > 0)
		{
			if (RemotePlayersUpdater.Instance.Bots[(int)b].Skin == 97 || RemotePlayersUpdater.Instance.Bots[(int)b].Skin == 99 || RemotePlayersUpdater.Instance.Bots[(int)b].Skin == 98)
			{
				RemotePlayersUpdater.Instance.Bots[(int)b].goCap.GetComponent<Renderer>().enabled = true;
			}
			else
			{
				RemotePlayersUpdater.Instance.Bots[(int)b].goHelmet.GetComponent<Renderer>().enabled = true;
			}
		}
		if (RemotePlayersUpdater.Instance.Bots[(int)b].Item[211] > 0)
		{
			RemotePlayersUpdater.Instance.Bots[(int)b].goTykva.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			RemotePlayersUpdater.Instance.Bots[(int)b].goTykva.GetComponent<Renderer>().enabled = false;
		}
		if (RemotePlayersUpdater.Instance.Bots[(int)b].Item[222] > 0)
		{
			RemotePlayersUpdater.Instance.Bots[(int)b].goKolpak.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			RemotePlayersUpdater.Instance.Bots[(int)b].goKolpak.GetComponent<Renderer>().enabled = false;
		}
		if (RemotePlayersUpdater.Instance.Bots[(int)b].Item[223] > 0)
		{
			RemotePlayersUpdater.Instance.Bots[(int)b].goRoga.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			RemotePlayersUpdater.Instance.Bots[(int)b].goRoga.GetComponent<Renderer>().enabled = false;
		}
		if (RemotePlayersUpdater.Instance.Bots[(int)b].Item[224] > 0)
		{
			RemotePlayersUpdater.Instance.Bots[(int)b].goMaskBear.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			RemotePlayersUpdater.Instance.Bots[(int)b].goMaskBear.GetComponent<Renderer>().enabled = false;
		}
		if (RemotePlayersUpdater.Instance.Bots[(int)b].Item[225] > 0)
		{
			RemotePlayersUpdater.Instance.Bots[(int)b].goMaskFox.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			RemotePlayersUpdater.Instance.Bots[(int)b].goMaskFox.GetComponent<Renderer>().enabled = false;
		}
		if (RemotePlayersUpdater.Instance.Bots[(int)b].Item[226] > 0)
		{
			RemotePlayersUpdater.Instance.Bots[(int)b].goMaskRabbit.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			RemotePlayersUpdater.Instance.Bots[(int)b].goMaskRabbit.GetComponent<Renderer>().enabled = false;
		}
		if (CONST.GetGameMode() == MODE.ZOMBIE)
		{
			RemotePlayersUpdater.Instance.Bots[(int)b].Team = 0;
			RemotePlayersUpdater.Instance.BotsGmObj[(int)b].GetComponent<Animator>().SetBool("isZombie", false);
			RemotePlayersUpdater.Instance.BotsGmObj[(int)b].GetComponent<TeamColor>().SetTeam((int)RemotePlayersUpdater.Instance.Bots[(int)b].Team, RemotePlayersUpdater.Instance.Bots[(int)b].Skin, RemotePlayersUpdater.Instance.Bots[(int)b].goHelmet, RemotePlayersUpdater.Instance.Bots[(int)b].goCap, RemotePlayersUpdater.Instance.Bots[(int)b].Znak);
			RemotePlayersUpdater.Instance.BotsGmObj[(int)b].layer = 10;
		}
		RemotePlayersUpdater.Instance.Bots[(int)b].inVehicle = false;
		RemotePlayersUpdater.Instance.Bots[(int)b].zombie = false;
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x000569E4 File Offset: 0x00054BE4
	private void recv_ready_for_spawn(byte[] buffer, int len)
	{
		int num = 4;
		int num2 = (int)buffer[num];
		num++;
		if (num2 == 1)
		{
			this.goMap.GetComponent<SpawnManager>().SetRandomFollow(this.myindex, -1);
			if (CONST.GetGameMode() == MODE.FFA)
			{
				if (this.MG == null)
				{
					this.MG = (MainGUI)Object.FindObjectOfType(typeof(MainGUI));
				}
				if (!MainGUI.e_menu)
				{
					this.MG.CloseAll();
					this.MG.OpenEMenu();
				}
			}
			else
			{
				this.goMap.GetComponent<SpawnManager>().SetSpectatorMsg(Lang.GetLabel(529));
			}
			this.goMap.GetComponent<SpawnManager>().waiting_for_respawn = true;
		}
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x00056A94 File Offset: 0x00054C94
	private void recv_spawnequip(byte[] buffer, int len)
	{
		int num = 4;
		byte health = buffer[num];
		num++;
		byte b = buffer[num];
		num++;
		int num2 = (int)buffer[num];
		num++;
		byte b2 = buffer[num];
		num++;
		byte b3 = buffer[num];
		num++;
		byte b4 = buffer[num];
		num++;
		int num3 = this.DecodeInteger(buffer, num);
		num += 4;
		int num4 = this.DecodeInteger(buffer, num);
		num += 4;
		int num5 = this.DecodeInteger(buffer, num);
		num += 4;
		int num6 = this.DecodeInteger(buffer, num);
		num += 4;
		int num7 = this.DecodeInteger(buffer, num);
		num += 4;
		int num8 = this.DecodeInteger(buffer, num);
		num += 4;
		int num9 = this.DecodeInteger(buffer, num);
		num += 4;
		int num10 = this.DecodeInteger(buffer, num);
		num += 4;
		int clipammo = this.DecodeInteger(buffer, num);
		num += 4;
		int backpack = this.DecodeInteger(buffer, num);
		num += 4;
		int clipammo2 = this.DecodeInteger(buffer, num);
		num += 4;
		int backpack2 = this.DecodeInteger(buffer, num);
		num += 4;
		int blockammo = this.DecodeInteger(buffer, num);
		num += 4;
		int timer = this.DecodeInteger(buffer, num);
		num += 4;
		int ammo = this.DecodeInteger(buffer, num);
		num += 4;
		int ammo2 = this.DecodeInteger(buffer, num);
		num += 4;
		int ammo3 = this.DecodeInteger(buffer, num);
		num += 4;
		int gren = this.DecodeInteger(buffer, num);
		num += 4;
		int gren2 = this.DecodeInteger(buffer, num);
		num += 4;
		byte medkit_w = buffer[num];
		num++;
		byte medkit_g = buffer[num];
		num++;
		byte medkit_o = buffer[num];
		num++;
		byte zbk18m = 0;
		byte zof = 0;
		byte snaryad = 0;
		byte repair_kit = 0;
		byte tank_light = 0;
		byte tank_medium = 0;
		byte tank_heavy = 0;
		byte mg = 0;
		byte flash = 0;
		byte smoke = 0;
		byte gp = buffer[num];
		num++;
		PlayerPrefs.SetInt("PWID", num4);
		PlayerPrefs.SetInt("SWID", num5);
		PlayerPrefs.SetInt("MWID", num3);
		PlayerPrefs.SetInt("A1WID", num6);
		PlayerPrefs.SetInt("A2WID", num7);
		PlayerPrefs.SetInt("A3WID", num8);
		PlayerPrefs.SetInt("G1WID", num9);
		PlayerPrefs.SetInt("G2WID", num10);
		E_Menu e_Menu = Object.FindObjectOfType<E_Menu>();
		if (e_Menu != null)
		{
			e_Menu.Init();
		}
		int num11 = this.myindex;
		this.LocalPlayer.GetComponent<TransportDetect>().enabled = true;
		if (CONST.GetGameMode() == MODE.ZOMBIE || CONST.GetGameMode() == MODE.SURVIVAL || CONST.GetGameMode() == MODE.CLEAR || CONST.GetGameMode() == MODE.BUILD)
		{
			RemotePlayersUpdater.Instance.Bots[num11].zombie = false;
			RemotePlayersUpdater.Instance.Bots[num11].Team = 0;
			this.goGui.GetComponent<Overlay>().SetActive(false);
			if (CONST.GetGameMode() == MODE.ZOMBIE)
			{
				this.goMap.GetComponent<Sky>().SetFog(0f, 12f, new Color(0.05f, 0.05f, 0.05f, 1f));
			}
			else if (CONST.GetGameMode() == MODE.SURVIVAL)
			{
				this.goMap.GetComponent<Sky>().SetFog(0f, 16f, new Color(0.05f, 0.05f, 0.05f, 1f));
			}
			else if (CONST.GetGameMode() == MODE.CLEAR)
			{
				this.goMap.GetComponent<Sky>().SetFog(0f, 10f, new Color(0.05f, 0.05f, 0.05f, 1f));
			}
		}
		base.gameObject.GetComponent<WeaponSystem>().StartEquip(num3, num4, num5, num6, num7, num8, num9, num10, clipammo, backpack, clipammo2, backpack2, blockammo, ammo, ammo2, ammo3, gren, gren2, (int)medkit_w, (int)medkit_g, (int)medkit_o, (int)zbk18m, (int)zof, (int)snaryad, (int)repair_kit, (int)tank_light, (int)tank_medium, (int)tank_heavy, mg, (int)gp, (int)flash, (int)smoke);
		((Ammo)Object.FindObjectOfType(typeof(Ammo))).draw = true;
		this.LocalPlayer.GetComponent<PlayerControl>().Spawn((int)b2, (int)b3, (int)b4);
		this.goMap.GetComponent<SpawnManager>().Spawn((float)b2, (float)b3, (float)b4);
		this.goGui.GetComponent<Health>().SetHealth((int)health);
		if (b > 0)
		{
			this.goGui.GetComponent<Health>().SetHelmet(true);
		}
		if (num2 > 0)
		{
			this.goGui.GetComponent<Health>().SetArmor(true);
		}
		if (RemotePlayersUpdater.Instance.Bots[num11].Skin == 182)
		{
			this.goGui.GetComponent<Health>().SetMask(true);
		}
		this.goGui.GetComponent<TeamScore>().SetTimer(timer);
		RemotePlayersUpdater.Instance.Bots[num11].Dead = 0;
		if (this.MG == null)
		{
			this.MG = (MainGUI)Object.FindObjectOfType(typeof(MainGUI));
		}
		this.MG.CloseAll();
		for (int i = 0; i < 32; i++)
		{
			if (RemotePlayersUpdater.Instance.Bots[i].Active)
			{
				RemotePlayersUpdater.Instance.SetPlayerActive(i, true);
			}
		}
		RemotePlayersUpdater.Instance.Bots[num11].Active = true;
		this.goMap.GetComponent<SpawnManager>().waiting_for_respawn = false;
		MainGUI.ForceCursor = false;
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x00056F74 File Offset: 0x00055174
	private void recv_zm_countdown(byte[] buffer, int len)
	{
		int num = 4;
		byte b = buffer[num];
		num++;
		this.goGui.GetComponent<Messages>().set_message(3, 0);
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x00056FA0 File Offset: 0x000551A0
	private void recv_zm_message(byte[] buffer, int len)
	{
		int num = 4;
		byte msgid = buffer[num];
		num++;
		this.goGui.GetComponent<Messages>().set_message((int)msgid, 0);
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x00056FCC File Offset: 0x000551CC
	private void recv_zm_infect(byte[] buffer, int len)
	{
		int num = 4;
		byte b = buffer[num];
		num++;
		byte attackerid = buffer[num];
		num++;
		if ((int)b == this.myindex)
		{
			RemotePlayersUpdater.Instance.SetZombie();
			this.goGui.GetComponent<Health>().SetHealth(5000);
			this.goGui.GetComponent<Overlay>().SetActive(true);
			this.goMap.GetComponent<Sky>().SetFog(10f, 40f, new Color(0.05f, 0.05f, 0.05f, 1f));
			this.LocalPlayer.GetComponent<Sound>().PlaySound_ZM_Infected();
			this.LocalPlayer.GetComponent<FX>().Infect();
		}
		RemotePlayersUpdater.Instance.Bots[(int)b].zombie = true;
		RemotePlayersUpdater.Instance.Bots[(int)b].Team = 1;
		RemotePlayersUpdater.Instance.Bots[(int)b].Dead = 0;
		RemotePlayersUpdater.Instance.SetCurrentWeapon((int)b, 35);
		RemotePlayersUpdater.Instance.Bots[(int)b].WeaponID = 35;
		RemotePlayersUpdater.Instance.BotsGmObj[(int)b].GetComponent<TeamColor>().SetTeam(0, 1, RemotePlayersUpdater.Instance.Bots[(int)b].goHelmet, RemotePlayersUpdater.Instance.Bots[(int)b].goCap, RemotePlayersUpdater.Instance.Bots[(int)b].Znak);
		RemotePlayersUpdater.Instance.BotsGmObj[(int)b].GetComponent<Animator>().SetBool("isZombie", true);
		RemotePlayersUpdater.Instance.BotsGmObj[(int)b].GetComponent<Sound>().PlaySound_ZM_Infected();
		RemotePlayersUpdater.Instance.BotsGmObj[(int)b].GetComponent<FX>().Infect();
		RemotePlayersUpdater.Instance.BotsGmObj[(int)b].layer = 15;
		this.goGui.GetComponent<DeathMsg>().AddDeathMsg((int)attackerid, (int)b, 35, 0);
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x0005718C File Offset: 0x0005538C
	private void recv_attack_milk(byte[] buffer, int len)
	{
		int num = 4;
		byte index = buffer[num];
		num++;
		int weaponid = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.SetAttack((int)index, weaponid);
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x000571BC File Offset: 0x000553BC
	private void recv_setblock(byte[] buffer, int len)
	{
		int num = 4;
		byte x = buffer[num];
		num++;
		byte y = buffer[num];
		num++;
		byte z = buffer[num];
		num++;
		byte team = buffer[num];
		num++;
		RemotePlayersUpdater.Instance.SetBlock((int)x, (int)y, (int)z, (int)team);
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x000571FC File Offset: 0x000553FC
	private void recv_damage(byte[] buffer, int len)
	{
		int num = 4;
		byte b = buffer[num];
		num++;
		byte b2 = buffer[num];
		num++;
		int num2 = this.DecodeInteger(buffer, num);
		num += 4;
		int num3 = this.DecodeInteger(buffer, num);
		num += 4;
		byte b3 = buffer[num];
		num++;
		if ((int)b2 == this.myindex)
		{
			this.goGui.GetComponent<Health>().SetHealth(num2);
			this.goGui.GetComponent<Health>().SetDamageIndicator((int)b);
			if (num3 == 184 || num3 == 315)
			{
				this.goGui.GetComponent<Crosshit>().FireHit();
			}
			if (num3 == 186)
			{
				this.goGui.GetComponent<Crosshit>().GazHit();
			}
			if (b3 == 77)
			{
				base.GetComponent<Sound>().PlaySound_ShieldHit(GameObject.Find("Player/MainCamera/TMPAudio").GetComponent<AudioSource>());
				base.GetComponent<WeaponSystem>().Shake(base.transform.position + base.transform.forward * 11f, false);
			}
		}
		if (num2 == 0)
		{
			if ((int)b2 == this.myindex)
			{
				base.GetComponent<Sound>().PlaySound_Stop(GameObject.Find("Player/MainCamera/TMPAudio").GetComponent<AudioSource>());
				RemotePlayersUpdater.Instance.CreateDeadEventSelf((int)b, (int)b2, num3);
				this.LocalPlayer.GetComponent<PlayerControl>().SetHit();
				this.LocalPlayer.GetComponent<TransportDetect>().enabled = false;
			}
			else
			{
				RemotePlayersUpdater.Instance.CreateDeadEvent((int)b, (int)b2, num3);
			}
			this.goGui.GetComponent<DeathMsg>().AddDeathMsg((int)b, (int)b2, num3, (int)b3);
		}
		if ((int)b2 == this.myindex)
		{
			if (b3 != 77)
			{
				this.LocalPlayer.GetComponent<PlayerControl>().SetHit();
			}
			return;
		}
		RemotePlayersUpdater.Instance.BotsGmObj[(int)b2].GetComponent<Sound>().PlaySound_Hit();
		if ((int)b == this.myindex)
		{
			this.LocalPlayer.GetComponent<PlayerControl>().SetTraceHit();
			this.goGui.GetComponent<Crosshit>().Hit();
			return;
		}
		this.cspm.CreateHit(base.transform, 1, RemotePlayersUpdater.Instance.BotsGmObj[(int)b2].transform.position.x, RemotePlayersUpdater.Instance.BotsGmObj[(int)b2].transform.position.y + 1f, RemotePlayersUpdater.Instance.BotsGmObj[(int)b2].transform.position.z);
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x00057440 File Offset: 0x00055640
	private void recv_chat(byte[] buffer, int len)
	{
		int num = 4;
		int index = (int)buffer[num];
		num++;
		int team = (int)buffer[num];
		num++;
		int teamchat = (int)buffer[num];
		num++;
		int num2 = this.DecodeInteger(buffer, num);
		num += 4;
		string msg = "";
		if (num2 > 0)
		{
			msg = Encoding.UTF8.GetString(buffer, num, num2);
		}
		this.goGui.GetComponent<Chat>().AddMessage(index, team, msg, teamchat);
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x000574A8 File Offset: 0x000556A8
	private void recv_stats(byte[] buffer, int len)
	{
		int num = 4;
		int num2 = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Stats_Kills = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Stats_Deads = this.DecodeInteger(buffer, num);
		num += 4;
		int num3 = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num3].Stats_Kills = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num3].Stats_Deads = this.DecodeInteger(buffer, num);
		num += 4;
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x0005753C File Offset: 0x0005573C
	private void recv_currentweapon(byte[] buffer, int len)
	{
		int num = 4;
		int id = (int)buffer[num];
		num++;
		int weaponid = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.SetCurrentWeapon(id, weaponid);
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x0005756C File Offset: 0x0005576C
	private void recv_disconnect(byte[] buffer, int len)
	{
		int num = 4;
		int index = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.SetPlayerActive(index, false);
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x00057590 File Offset: 0x00055790
	private void recv_reconnect(byte[] buffer, int len)
	{
		for (int i = 0; i < 32; i++)
		{
			RemotePlayersUpdater.Instance.Bots[i].Active = false;
		}
		GameController.STATE = GAME_STATES.GAME;
		GM.currMainState = GAME_STATES.CONNECTING;
		GM.currExtState = GAME_STATES.NULL;
		SceneManager.LoadScene(1);
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x0000248C File Offset: 0x0000068C
	private void recv_endofsnap(byte[] buffer, int len)
	{
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x000575D8 File Offset: 0x000557D8
	private void recv_buildblock(byte[] buffer, int len)
	{
		int num = 4;
		int x = (int)buffer[num];
		num++;
		int y = (int)buffer[num];
		num++;
		int z = (int)buffer[num];
		num++;
		int flag = (int)buffer[num];
		num++;
		this.ziploader.SetBlock2(x, y, z, flag);
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x00057618 File Offset: 0x00055818
	private void recv_my_data(byte[] buffer, int len)
	{
		int num = 4;
		int num2 = (int)buffer[num];
		num++;
		int num3 = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Name = Encoding.UTF8.GetString(buffer, num, num3);
		num += num3;
		num3 = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].ClanName = Encoding.UTF8.GetString(buffer, num, num3);
		num += num3 + 1;
		RemotePlayersUpdater.Instance.Bots[num2].CountryID = buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Team = byte.MaxValue;
		RemotePlayersUpdater.Instance.Bots[num2].Helmet = buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[54] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[146] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[147] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Skin = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Znak = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[6] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[2] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[3] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[9] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[12] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[13] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[14] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[15] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[16] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[17] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[18] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[19] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[40] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[47] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[48] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[49] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[50] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[51] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[52] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[53] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[60] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[61] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[68] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[69] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[70] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[71] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[72] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[73] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[74] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[77] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[78] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[79] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[80] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[81] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[82] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[89] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[90] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[91] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[34] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[93] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[94] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[95] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[96] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[101] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[102] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[103] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[104] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[105] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[106] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[107] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[108] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[109] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[110] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[111] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[112] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[62] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[137] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[138] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[10] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[100] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[55] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[7] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[139] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[140] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[141] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[142] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[143] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[144] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[145] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[169] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[168] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[170] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[161] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[162] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[160] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[159] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[157] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[158] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[171] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[172] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[173] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[174] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[175] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[176] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[177] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[183] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[184] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[185] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[186] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[188] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[189] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[190] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[191] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[192] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[193] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[201] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[202] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[203] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[204] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[136] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[135] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[205] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[206] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[198] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[207] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[208] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[209] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[210] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[211] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[218] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[219] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[220] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[221] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[222] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[223] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[224] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[225] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[226] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[301] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[302] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[303] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[304] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[305] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[308] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[309] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[313] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[314] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[315] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[329] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[330] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[331] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[332] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[333] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[39] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[154] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[155] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[92] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[156] = this.DecodeInteger(buffer, num);
		num += 4;
		RemotePlayersUpdater.Instance.Bots[num2].Item[334] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[337] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[340] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[342] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[344] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[348] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[349] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[354] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[359] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[370] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[335] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[336] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[338] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[339] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[341] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[343] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[345] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[346] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[347] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[350] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[351] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[352] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[353] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[355] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[356] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[357] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[358] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[360] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[361] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[362] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[363] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[371] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[372] = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.Bots[num2].Item[33] = 1;
		RemotePlayersUpdater.Instance.Bots[num2].Item[43] = 1;
		RemotePlayersUpdater.Instance.Bots[num2].Item[45] = 1;
		RemotePlayersUpdater.Instance.Bots[num2].Item[44] = 1;
		RemotePlayersUpdater.Instance.Bots[num2].Item[46] = 1;
		if (RemotePlayersUpdater.Instance.Bots[num2].Helmet > 0 || RemotePlayersUpdater.Instance.Bots[num2].Item[146] > 0)
		{
			this.goGui.GetComponent<Health>().SetHelmet(true);
		}
		if (RemotePlayersUpdater.Instance.Bots[num2].Item[54] > 0 || RemotePlayersUpdater.Instance.Bots[num2].Item[147] > 0)
		{
			this.goGui.GetComponent<Health>().SetArmor(true);
		}
		if (RemotePlayersUpdater.Instance.Bots[num2].Skin == 182)
		{
			this.goGui.GetComponent<Health>().SetMask(true);
		}
		this.goMap.GetComponent<SpawnManager>().SetRandomFollow(this.myindex, -1);
		if (this.mode == 3)
		{
			this.goMap.GetComponent<SpawnManager>().SetSpectatorMsg(Lang.GetLabel(156));
		}
		else if (this.mode == 2)
		{
			this.goMap.GetComponent<SpawnManager>().SetSpectatorMsg(Lang.GetLabel(157));
		}
		else if (this.mode != 7)
		{
			int num4 = this.mode;
		}
		GM.currExtState = GAME_STATES.GAMELOADMYPROFILECOMPLITE;
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x00058CB0 File Offset: 0x00056EB0
	private void recv_damage_helmet(byte[] buffer, int len)
	{
		int num = 4;
		int num2 = (int)buffer[num];
		num++;
		int num3 = (int)buffer[num];
		num++;
		int num4 = (int)buffer[num];
		num++;
		if (num4 == 0)
		{
			RemotePlayersUpdater.Instance.Bots[num3].goHelmet.GetComponent<Renderer>().enabled = false;
			RemotePlayersUpdater.Instance.Bots[num3].goCap.GetComponent<Renderer>().enabled = false;
			RemotePlayersUpdater.Instance.Bots[num3].goTykva.GetComponent<Renderer>().enabled = false;
			RemotePlayersUpdater.Instance.Bots[num3].goKolpak.GetComponent<Renderer>().enabled = false;
			RemotePlayersUpdater.Instance.Bots[num3].goRoga.GetComponent<Renderer>().enabled = false;
			RemotePlayersUpdater.Instance.Bots[num3].goMaskBear.GetComponent<Renderer>().enabled = false;
			RemotePlayersUpdater.Instance.Bots[num3].goMaskFox.GetComponent<Renderer>().enabled = false;
			RemotePlayersUpdater.Instance.Bots[num3].goMaskRabbit.GetComponent<Renderer>().enabled = false;
			this.csrm.CreateHelmetRagDoll(RemotePlayersUpdater.Instance.BotsGmObj[num3], (int)RemotePlayersUpdater.Instance.Bots[num3].Team, RemotePlayersUpdater.Instance.Bots[num3].Skin, 0);
		}
		else if (num4 == 211)
		{
			RemotePlayersUpdater.Instance.Bots[num3].goTykva.GetComponent<Renderer>().enabled = false;
			RemotePlayersUpdater.Instance.Bots[num3].Item[211] = 0;
			this.csrm.CreateHelmetRagDoll(RemotePlayersUpdater.Instance.BotsGmObj[num3], (int)RemotePlayersUpdater.Instance.Bots[num3].Team, RemotePlayersUpdater.Instance.Bots[num3].Skin, 211);
		}
		else if (num4 == 212 || num4 == 231 || num4 == 232 || num4 != 233)
		{
		}
		if (num3 != this.myindex)
		{
			RemotePlayersUpdater.Instance.BotsGmObj[num3].GetComponent<Sound>().PlaySound_HitHelmet();
			if (num2 == this.myindex)
			{
				this.LocalPlayer.GetComponent<Sound>().PlaySound_TraceHelmet();
				return;
			}
		}
		else
		{
			this.LocalPlayer.GetComponent<Sound>().PlaySound_HitHelmet();
			if (num4 == 0)
			{
				this.goGui.GetComponent<Health>().SetHelmet(false);
			}
		}
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x00058F00 File Offset: 0x00057100
	private void recv_createent(byte[] buffer, int len)
	{
		int num = 4;
		int num2 = (int)buffer[num];
		num++;
		int uid = this.DecodeInteger(buffer, num);
		num += 4;
		int num3 = (int)buffer[num];
		num++;
		ushort num4 = this.DecodeShort2(buffer, num);
		num += 2;
		ushort num5 = this.DecodeShort2(buffer, num);
		num += 2;
		float num6 = (float)this.DecodeShort2(buffer, num);
		num += 2;
		float px = (float)num4 * 0.00390625f;
		float py = (float)num5 * 0.00390625f;
		float pz = num6 * 0.00390625f;
		float rx = this.DecodeSingle(buffer, num);
		num += 4;
		float ry = this.DecodeSingle(buffer, num);
		num += 4;
		float rz = this.DecodeSingle(buffer, num);
		num += 4;
		float fx = this.DecodeSingle(buffer, num);
		num += 4;
		float fy = this.DecodeSingle(buffer, num);
		num += 4;
		float fz = this.DecodeSingle(buffer, num);
		num += 4;
		float tx = this.DecodeSingle(buffer, num);
		num += 4;
		float ty = this.DecodeSingle(buffer, num);
		num += 4;
		float tz = this.DecodeSingle(buffer, num);
		num += 4;
		if (num3 == CONST.ENTS.ENT_GRENADE)
		{
			this.csrm.CreateGrenade(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_SHMEL)
		{
			this.csrm.CreateRocket(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_ZOMBIE || num3 == CONST.ENTS.ENT_ZOMBIE2)
		{
			this.csrm.CreateZombie(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_GP)
		{
			this.csrm.CreateGP(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_SHTURM_MINEN)
		{
			this.csrm.CreateMINEN(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_TANK)
		{
			this.csrm.CreateTank(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_TANK_SNARYAD)
		{
			this.csrm.CreateSnaryad(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_RPG)
		{
			this.csrm.CreateRPG7(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_TANK_LIGHT)
		{
			this.csrm.CreateTankLight(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_TANK_MEDIUM)
		{
			this.csrm.CreateTankMedium(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_TANK_HEAVY)
		{
			this.csrm.CreateTankHeavy(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_ZBK18M)
		{
			this.csrm.CreateZBK18M(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_ZOF26)
		{
			this.csrm.CreateZOF26(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_MINEFLY)
		{
			this.csrm.CreateMinefly(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_JAVELIN)
		{
			this.csrm.CreateJavelinMissle(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_ARROW)
		{
			this.csrm.CreateCrossbowArrow(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_SMOKE_GRENADE)
		{
			this.csrm.CreateM18(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_HE_GRENADE)
		{
			this.csrm.CreateMK3(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_RKG3)
		{
			this.csrm.CreateRKG3(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_MINE)
		{
			this.csrm.CreateMine(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_C4)
		{
			this.csrm.CreateC4(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_JEEP)
		{
			this.csrm.CreateJeep(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz, (int)RemotePlayersUpdater.Instance.Bots[num2].Team);
			return;
		}
		if (num3 == CONST.ENTS.ENT_AT_MINE)
		{
			this.csrm.CreateATMine(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_M202)
		{
			this.csrm.CreateM202Rocket(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_GAZ_GRENADE)
		{
			this.csrm.CreateGG(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_MOLOTOV)
		{
			this.csrm.CreateMolotov(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_SMOKE)
		{
			this.csrm.CreateSMOKE(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_ANTI_MISSLE)
		{
			this.csrm.CreateFLASH(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
			return;
		}
		if (num3 == CONST.ENTS.ENT_STIELHANDGRANATE)
		{
			this.csrm.CreateSTIELHANDGRANATE(num2, uid, px, py, pz, rx, ry, rz, fx, fy, fz, tx, ty, tz);
		}
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x00059540 File Offset: 0x00057740
	private void recv_destroy_status(byte[] buffer, int len)
	{
		int num = 4;
		int num2 = (int)buffer[num];
		num++;
		if (num2 == 1)
		{
			this.pos.Clear();
			return;
		}
		if (num2 == 0)
		{
			RemotePlayersUpdater.Instance.PhysicsBlock(this.pos);
		}
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x0005957C File Offset: 0x0005777C
	private void recv_explode(byte[] buffer, int len)
	{
		int num = 4;
		ushort num2 = this.DecodeShort2(buffer, num);
		num += 2;
		ushort num3 = this.DecodeShort2(buffer, num);
		num += 2;
		float num4 = (float)this.DecodeShort2(buffer, num);
		num += 2;
		float num5 = (float)num2 * 0.00390625f;
		float num6 = (float)num3 * 0.00390625f;
		float num7 = num4 * 0.00390625f;
		RemotePlayersUpdater.Instance.CreateFX(num5, num6, num7);
		base.gameObject.GetComponent<WeaponSystem>().Shake(new Vector3(num5, num6, num7), false);
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x000595F8 File Offset: 0x000577F8
	private void recv_private_info(byte[] buffer, int len)
	{
		int num = 4;
		int privateServer = (int)buffer[num];
		num++;
		this.LocalPlayer.GetComponent<PlayerControl>().SetPrivateServer(privateServer);
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x00059620 File Offset: 0x00057820
	private void recv_reconnect2(byte[] buffer, int len)
	{
		ConnectionInfo.waitandreconnect = true;
		GameController.STATE = GAME_STATES.GAME;
		GM.currMainState = GAME_STATES.CONNECTING;
		GM.currExtState = GAME_STATES.NULL;
		SceneManager.LoadScene(1);
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x00059644 File Offset: 0x00057844
	private void recv_sethealth(byte[] buffer, int len)
	{
		int num = 4;
		int health = this.DecodeInteger(buffer, num);
		num += 4;
		this.goGui.GetComponent<Health>().SetHealth(health);
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x00059674 File Offset: 0x00057874
	private void recv_gamemessage(byte[] buffer, int len)
	{
		int num = 4;
		byte b = buffer[num];
		num++;
		int num2 = this.DecodeInteger(buffer, num);
		num += 4;
		if (this.mode == 9)
		{
			if (b == 1)
			{
				for (int i = 0; i < 4; i++)
				{
					this.map.flags[i].FP.accepted = true;
				}
			}
			if (b == 2)
			{
				Object.FindObjectOfType<EvacPlase>().accepted = true;
			}
		}
		this.goGui.GetComponent<Messages>().set_message((int)b, num2);
		if (b == 1)
		{
			this.zs_wave = num2;
		}
		if (this.mode == 10)
		{
			if (b == 0)
			{
				if (this.LocalPlayer.GetComponent<LiftMeUp>() == null)
				{
					this.LocalPlayer.AddComponent<LiftMeUp>();
				}
				this.LocalPlayer.GetComponent<LiftMeUp>().PlayWait();
			}
			if (b == 2 && num2 == 1)
			{
				if (this.LocalPlayer.GetComponent<LiftMeUp>() == null)
				{
					this.LocalPlayer.AddComponent<LiftMeUp>();
				}
				this.LocalPlayer.GetComponent<LiftMeUp>().BreakSound();
			}
		}
		if (b == 66)
		{
			base.StartCoroutine(this.camPause());
		}
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x0005977B File Offset: 0x0005797B
	private IEnumerator camPause()
	{
		yield return new WaitForSeconds(4f);
		this.goMap.GetComponent<SpawnManager>().SetRandomFollow(this.myindex, -1);
		yield break;
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x0005978C File Offset: 0x0005798C
	private void recv_equip(byte[] buffer, int len)
	{
		int num = 4;
		int ammo_block = this.DecodeInteger(buffer, num);
		num += 4;
		base.gameObject.GetComponent<WeaponSystem>().ammo_block = ammo_block;
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x000597B9 File Offset: 0x000579B9
	private void recv_app_disconnect()
	{
		this.active = false;
		GameController.STATE = GAME_STATES.MAINMENU;
		GM.currGUIState = GUIGS.SERVERLIST;
		SceneManager.LoadScene(0);
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x000597D8 File Offset: 0x000579D8
	private void recv_message(byte[] buffer, int len)
	{
		int num = 4;
		int num2 = (int)buffer[num];
		num++;
		int num3 = (int)buffer[num];
		num++;
		int num4 = (int)buffer[num];
		num++;
		int num5 = this.DecodeInteger(buffer, num);
		num += 4;
		if (!RemotePlayersUpdater.Instance.Bots[num2].Active)
		{
			return;
		}
		if (!RemotePlayersUpdater.Instance.Bots[num3].Active)
		{
			return;
		}
		string msg = "";
		if (num4 == 0)
		{
			msg = "^1" + RemotePlayersUpdater.Instance.Bots[num2].Name + Lang.GetLabel(159) + RemotePlayersUpdater.Instance.Bots[num3].Name;
		}
		else if (num4 == 1)
		{
			string label;
			if (num5 == 0)
			{
				label = Lang.GetLabel(160);
			}
			else
			{
				label = Lang.GetLabel(161);
			}
			msg = string.Concat(new string[]
			{
				"^1",
				RemotePlayersUpdater.Instance.Bots[num2].Name,
				" ",
				label,
				Lang.GetLabel(227),
				RemotePlayersUpdater.Instance.Bots[num3].Name
			});
		}
		else if (num4 == 2)
		{
			msg = "^1" + RemotePlayersUpdater.Instance.Bots[num2].Name + Lang.GetLabel(162);
		}
		else if (num4 == 3)
		{
			msg = "^1" + RemotePlayersUpdater.Instance.Bots[num2].Name + Lang.GetLabel(163) + num5.ToString();
		}
		else if (num4 == 4)
		{
			msg = "^1" + RemotePlayersUpdater.Instance.Bots[num2].Name + Lang.GetLabel(164);
		}
		this.goGui.GetComponent<Chat>().AddMessage(-1, 1, msg, 0);
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x000599A8 File Offset: 0x00057BA8
	private void recv_ct_radar(byte[] buffer, int len)
	{
		int num = 4;
		byte[,] array = new byte[16, 16];
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				array[i, j] = buffer[num];
				num++;
			}
		}
		this.goGui.GetComponent<Radar>().UpdateRadarColor(array);
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x000599F9 File Offset: 0x00057BF9
	private void recv_damage_armor(byte[] buffer, int len)
	{
		this.goGui.GetComponent<Health>().SetArmor(false);
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x00059A0C File Offset: 0x00057C0C
	private void recv_sound_fx(byte[] buffer, int len)
	{
		int num = 4;
		int num2 = (int)buffer[num];
		num++;
		if (this.myindex == num2)
		{
			this.LocalPlayer.GetComponent<Sound>().PlaySound_TNT();
			return;
		}
		RemotePlayersUpdater.Instance.BotsGmObj[num2].GetComponent<Sound>().PlaySound_TNT();
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x00059A54 File Offset: 0x00057C54
	private void recv_reposition(byte[] buffer, int len)
	{
		int num = 4;
		ushort num2 = this.DecodeShort2(buffer, num);
		num += 2;
		ushort num3 = this.DecodeShort2(buffer, num);
		num += 2;
		float num4 = (float)this.DecodeShort2(buffer, num);
		num += 2;
		float x = (float)num2 * 0.00390625f;
		float y = (float)num3 * 0.00390625f;
		float z = num4 * 0.00390625f;
		this.LocalPlayer.transform.position = new Vector3(x, y, z);
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x00059AC0 File Offset: 0x00057CC0
	private void recv_moveent(byte[] buffer, int len)
	{
		int num = 4;
		int id = (int)buffer[num];
		num++;
		int uid = this.DecodeInteger(buffer, num);
		num += 4;
		byte b = buffer[num];
		num++;
		ushort num2 = this.DecodeShort2(buffer, num);
		num += 2;
		ushort num3 = this.DecodeShort2(buffer, num);
		num += 2;
		float num4 = (float)this.DecodeShort2(buffer, num);
		num += 2;
		float num5 = (float)num2 * 0.00390625f;
		float num6 = (float)num3 * 0.00390625f;
		float num7 = num4 * 0.00390625f;
		CEnt ent = EntManager.GetEnt(uid);
		if (ent == null)
		{
			this.csrm.CreateZombie(id, uid, num5, num6, num7, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
			return;
		}
		Vector3 b2 = new Vector3(ent.go.transform.position.x, ent.go.transform.position.y, ent.go.transform.position.z);
		Vector3 vector = new Vector3(num5, num6 + 0.5f, num7);
		float y = Health.AngleSigned(new Vector3(0f, 0f, 1f), vector - b2, ent.go.transform.up);
		ent.position = vector;
		ent.rotation = new Vector3(0f, y, 0f);
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x00059C30 File Offset: 0x00057E30
	private void recv_destroyent(byte[] buffer, int len)
	{
		int num = 4;
		int uid = this.DecodeInteger(buffer, num);
		num += 4;
		CEnt ent = EntManager.GetEnt(uid);
		if (ent == null)
		{
			return;
		}
		int index = ent.index;
		if (ent.classID == 3 || ent.classID == 10 || ent.classID == 11)
		{
			this.csrm.CreatePlayerRagDoll(ent.go, this.LocalPlayer, 0, false, ent.team, ent.skin, 0, false, false, false, false, false, false, false, false);
		}
		if (ent.classID == CONST.ENTS.ENT_GHOST || ent.classID == CONST.ENTS.ENT_GHOST_BOSS)
		{
			ParticleManager.THIS.CreateGhostDeath(ent.go.transform.position + Vector3.up);
		}
		Object.Destroy(ent.go);
		EntManager.DeleteEnt(index);
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x00059CF8 File Offset: 0x00057EF8
	private void recv_entposition(byte[] buffer, int len)
	{
		int i = 4;
		while (i < len)
		{
			int id = (int)buffer[i];
			i++;
			int uid = this.DecodeInteger(buffer, i);
			i += 4;
			byte b = buffer[i];
			i++;
			ushort num = this.DecodeShort2(buffer, i);
			i += 2;
			ushort num2 = this.DecodeShort2(buffer, i);
			i += 2;
			float num3 = (float)this.DecodeShort2(buffer, i);
			i += 2;
			float num4 = (float)num * 0.00390625f;
			float num5 = (float)num2 * 0.00390625f;
			float num6 = num3 * 0.00390625f;
			CEnt ent = EntManager.GetEnt(uid);
			if (ent == null)
			{
				this.csrm.CreateZombie(id, uid, num4, num5, num6, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
			}
			else
			{
				Vector3 b2 = new Vector3(ent.go.transform.position.x, ent.go.transform.position.y, ent.go.transform.position.z);
				Vector3 vector = new Vector3(num4, num5 + 0.5f, num6);
				float y = Health.AngleSigned(new Vector3(0f, 0f, 1f), vector - b2, ent.go.transform.up);
				ent.position = vector;
				ent.rotation = new Vector3(0f, y, 0f);
			}
		}
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x00059E78 File Offset: 0x00058078
	private void recv_moveboss(byte[] buffer, int len)
	{
		int num = 4;
		int uid = this.DecodeInteger(buffer, num);
		num += 4;
		int move = (int)buffer[num];
		num++;
		CEnt ent = EntManager.GetEnt(uid);
		if (ent == null)
		{
			return;
		}
		ent.go.GetComponent<NpcLerp>().PlayAnimation(move);
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x00059EB8 File Offset: 0x000580B8
	private void recv_liftup(byte[] buffer, int len)
	{
		Vector3 position = this.LocalPlayer.transform.position;
		Vector3 position2 = this.LocalPlayer.transform.position;
		Vector3 position3 = this.LocalPlayer.transform.position;
		base.gameObject.GetComponent<WeaponSystem>().StartShake();
		if (this.LocalPlayer.GetComponent<LiftMeUp>() == null)
		{
			this.LocalPlayer.AddComponent<LiftMeUp>();
		}
		this.LocalPlayer.GetComponent<LiftMeUp>().PlaySound();
		Batch batch = (Batch)Object.FindObjectOfType(typeof(Batch));
		if (batch)
		{
			batch.Combine();
		}
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x00059F68 File Offset: 0x00058168
	private void recv_playerinfo2(byte[] buffer, int len)
	{
		this.BEGIN_READ(buffer, len, 4);
		int num = this.READ_BYTE();
		string uid = this.READ_STRING();
		RemotePlayersUpdater.Instance.Bots[num].uid = uid;
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00059F9E File Offset: 0x0005819E
	private void recv_player_update(byte[] buffer, int len)
	{
		this.BEGIN_READ(buffer, len, 4);
		this.READ_BYTE();
		this.READ_STRING();
		GameController.THIS.UpdatePlayerInfo();
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x00059FC4 File Offset: 0x000581C4
	private void recv_enter_the_ent(byte[] buffer, int len)
	{
		int num = 4;
		int uid = this.DecodeInteger(buffer, num);
		num += 4;
		int num2 = this.DecodeInteger(buffer, num);
		num += 4;
		this.DecodeInteger(buffer, num);
		num += 4;
		int num3 = this.DecodeInteger(buffer, num);
		num += 4;
		int health = this.DecodeInteger(buffer, num);
		num += 4;
		int armor = this.DecodeInteger(buffer, num);
		num += 4;
		int speed = this.DecodeInteger(buffer, num);
		num += 4;
		int reload = this.DecodeInteger(buffer, num);
		num += 4;
		int turretRotation = this.DecodeInteger(buffer, num);
		num += 4;
		int skin = this.DecodeInteger(buffer, num);
		num += 4;
		CEnt ent = EntManager.GetEnt(uid);
		GameObject gameObject = GameObject.Find("RayCastBox");
		if (ent != null)
		{
			if (ent.classID == CONST.ENTS.ENT_TANK)
			{
				skin = 0;
			}
			if (num2 == this.myindex)
			{
				((Crosshair)Object.FindObjectOfType(typeof(Crosshair))).SetActive(false);
				OrbitCam componentInChildren = base.gameObject.GetComponentInChildren<OrbitCam>();
				((Ammo)Object.FindObjectOfType(typeof(Ammo))).draw = false;
				this.LocalPlayer.transform.parent = null;
				if (ent.classID == CONST.ENTS.ENT_TANK || ent.classID == CONST.ENTS.ENT_TANK_LIGHT || ent.classID == CONST.ENTS.ENT_TANK_MEDIUM || ent.classID == CONST.ENTS.ENT_TANK_HEAVY)
				{
					if (this.tc == null)
					{
						this.tc = base.gameObject.GetComponent<TankController>();
					}
					gameObject.transform.position = new Vector3(ent.go.transform.position.x, ent.go.transform.position.y + this.tc.distFromGround, ent.go.transform.position.z);
					gameObject.transform.eulerAngles = new Vector3(0f, ent.go.transform.eulerAngles.y, 0f);
					componentInChildren.vehicleType = CONST.VEHICLES.TANKS;
					this.tc.currTank = null;
					this.LocalPlayer.transform.position = ent.go.transform.position;
					this.LocalPlayer.transform.eulerAngles = new Vector3(0f, ent.go.transform.eulerAngles.y, 0f);
					ent.go.transform.parent = this.LocalPlayer.transform;
					ent.go.transform.localPosition = new Vector3(0f, 0f, 0f);
					ent.go.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
					ent.go.GetComponent<Tank>().client = true;
					ent.go.GetComponent<Tank>().health = health;
					ent.go.GetComponent<Tank>().armor = armor;
					ent.go.GetComponent<Tank>().speed = speed;
					ent.go.GetComponent<Tank>().reload = reload;
					ent.go.GetComponent<Tank>().turretRotation = turretRotation;
				}
				else if (ent.classID == CONST.ENTS.ENT_JEEP)
				{
					if (this.cc == null)
					{
						this.cc = base.gameObject.GetComponent<CarController>();
					}
					if (ent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_DRIVER] == num2 && num3 != CONST.VEHICLES.POSITION_JEEP_DRIVER && ent.go.transform.parent != null)
					{
						ent.go.transform.parent = null;
					}
					if (num3 == CONST.VEHICLES.POSITION_JEEP_DRIVER)
					{
						gameObject.transform.position = new Vector3(ent.go.transform.position.x, ent.go.transform.position.y + this.cc.distFromGround, ent.go.transform.position.z);
						gameObject.transform.eulerAngles = new Vector3(0f, ent.go.transform.eulerAngles.y, 0f);
					}
					componentInChildren.vehicleType = CONST.VEHICLES.JEEP;
					this.cc.currCar = null;
					ent.go.GetComponent<Car>().gunner = false;
					if (num3 == CONST.VEHICLES.POSITION_JEEP_DRIVER)
					{
						if (ent.go.transform.parent != null)
						{
							ent.go.transform.parent = null;
						}
						this.LocalPlayer.transform.position = ent.go.transform.position;
						this.LocalPlayer.transform.eulerAngles = new Vector3(0f, ent.go.transform.eulerAngles.y, 0f);
						ent.go.transform.parent = this.LocalPlayer.transform;
						ent.go.transform.localPosition = new Vector3(0f, 0f, 0f);
						ent.go.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
					}
					else if (num3 == CONST.VEHICLES.POSITION_JEEP_PASS)
					{
						this.LocalPlayer.transform.position = ent.go.transform.position;
						this.LocalPlayer.transform.eulerAngles = new Vector3(0f, ent.go.transform.eulerAngles.y, 0f);
						this.LocalPlayer.transform.parent = ent.go.transform;
					}
					else
					{
						this.LocalPlayer.transform.position = ent.go.GetComponent<Car>().GunnerPos.transform.position;
						this.LocalPlayer.transform.eulerAngles = new Vector3(0f, ent.go.transform.eulerAngles.y, 0f);
						this.LocalPlayer.transform.parent = ent.go.GetComponent<Car>().GunnerPos.transform;
						ent.go.GetComponent<Car>().gunner = true;
					}
					ent.go.GetComponent<Car>().health = health;
					ent.go.GetComponent<Car>().armor = armor;
					ent.go.GetComponent<Car>().speed = speed;
					ent.go.GetComponent<Car>().reload = reload;
					ent.go.GetComponent<Car>().turretRotation = turretRotation;
					ent.go.GetComponent<Car>().UnactiveGunner();
					ent.go.GetComponent<Car>().CheckSlots(num2);
					GameObject gameObject2 = ent.go.GetComponent<Car>().JeepMesh.gameObject;
					if (num2 == ent.ownerID || (ent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_DRIVER] == CONST.VEHICLES.POSITION_NONE && ent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_GUNNER] == CONST.VEHICLES.POSITION_NONE && ent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_PASS] == CONST.VEHICLES.POSITION_NONE))
					{
						gameObject2.GetComponent<SkinnedMeshRenderer>().materials[1].mainTexture = SkinManager.GetTankSkin(skin, (int)RemotePlayersUpdater.Instance.Bots[this.myindex].Team);
					}
					gameObject2.GetComponent<MeshCollider>().enabled = false;
					ent.go.GetComponent<Car>().slots[num3] = num2;
					if (num3 == CONST.VEHICLES.POSITION_JEEP_GUNNER)
					{
						bool trooper = true;
						bool helmet = false;
						bool cap = false;
						bool budge = false;
						if (RemotePlayersUpdater.Instance.Bots[num2].Helmet > 0)
						{
							if (RemotePlayersUpdater.Instance.Bots[num2].Skin == 97 || RemotePlayersUpdater.Instance.Bots[num2].Skin == 98 || RemotePlayersUpdater.Instance.Bots[num2].Skin == 99)
							{
								helmet = false;
								cap = true;
							}
							else
							{
								helmet = true;
								cap = false;
							}
						}
						if (RemotePlayersUpdater.Instance.Bots[num2].Znak > 0)
						{
							budge = true;
						}
						ent.go.GetComponent<Car>().Gunner.GetComponent<Renderer>().material.mainTexture = SkinManager.GetSkin((int)RemotePlayersUpdater.Instance.Bots[num2].Team, RemotePlayersUpdater.Instance.Bots[num2].Skin);
						ent.go.GetComponent<Car>().GunnerCap.GetComponent<Renderer>().material.mainTexture = SkinManager.GetSkin((int)RemotePlayersUpdater.Instance.Bots[num2].Team, RemotePlayersUpdater.Instance.Bots[num2].Skin);
						ent.go.GetComponent<Car>().GunnerHelmet.GetComponent<Renderer>().material.mainTexture = SkinManager.GetSkin((int)RemotePlayersUpdater.Instance.Bots[num2].Team, RemotePlayersUpdater.Instance.Bots[num2].Skin);
						ent.go.GetComponent<Car>().GunnerBudge.GetComponent<Renderer>().material.mainTexture = SkinManager.GetBadge(RemotePlayersUpdater.Instance.Bots[num2].Znak);
						if (RemotePlayersUpdater.Instance.Bots[num2].Skin == 311)
						{
							Color value = Color.white;
							if (RemotePlayersUpdater.Instance.Bots[num2].Team == 0)
							{
								value = new Color(0f, 0.45f, 1f);
							}
							else if (RemotePlayersUpdater.Instance.Bots[num2].Team == 1)
							{
								value = Color.red;
							}
							else if (RemotePlayersUpdater.Instance.Bots[num2].Team == 2)
							{
								value = Color.green;
							}
							else if (RemotePlayersUpdater.Instance.Bots[num2].Team == 3)
							{
								value = Color.yellow;
							}
							ent.go.GetComponent<Car>().Gunner.GetComponent<Renderer>().material.SetColor("_EmissionColor", value);
							ent.go.GetComponent<Car>().GunnerHelmet.GetComponent<Renderer>().material.SetColor("_EmissionColor", value);
						}
						else
						{
							ent.go.GetComponent<Car>().Gunner.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
							ent.go.GetComponent<Car>().GunnerHelmet.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
						}
						ent.go.GetComponent<Car>().ActiveGunner(trooper, helmet, cap, budge);
					}
				}
				base.gameObject.GetComponent<vp_FPController>().enabled = false;
				base.gameObject.GetComponent<vp_FPInput>().enabled = false;
				base.gameObject.GetComponent<vp_FPWeaponHandler>().enabled = false;
				base.gameObject.GetComponent<TransportDetect>().enabled = false;
				base.gameObject.GetComponentInChildren<vp_FPCamera>().enabled = false;
				base.gameObject.GetComponent<WeaponSystem>().SetPrimaryWeapon(false);
				base.gameObject.GetComponent<WeaponSystem>().goBlockSetup.transform.position = new Vector3(-1000f, -1000f, -1000f);
				base.gameObject.GetComponent<WeaponSystem>().goBlockCrash.transform.position = new Vector3(-1000f, -1000f, -1000f);
				base.gameObject.GetComponent<WeaponSystem>().goBlockTNT.transform.position = new Vector3(-1000f, -1000f, -1000f);
				base.gameObject.GetComponent<WeaponSystem>().awid = -1;
				MainGUI.ForceCursor = false;
				vp_FPWeaponShooter.fpsguidraw = false;
				base.gameObject.GetComponent<vp_FPInput>().Player.Zoom.TryStop(true);
				if (ent.classID == CONST.ENTS.ENT_TANK || ent.classID == CONST.ENTS.ENT_TANK_LIGHT || ent.classID == CONST.ENTS.ENT_TANK_MEDIUM || ent.classID == CONST.ENTS.ENT_TANK_HEAVY)
				{
					string str = "Player/";
					this.tc.L1 = GameObject.Find("L1").transform;
					this.tc.L2 = GameObject.Find("L2").transform;
					this.tc.L3 = GameObject.Find("L3").transform;
					this.tc.L4 = GameObject.Find("L4").transform;
					this.tc.L5 = GameObject.Find("L5").transform;
					this.tc.R1 = GameObject.Find("R1").transform;
					this.tc.R2 = GameObject.Find("R2").transform;
					this.tc.R3 = GameObject.Find("R3").transform;
					this.tc.R4 = GameObject.Find("R4").transform;
					this.tc.R5 = GameObject.Find("R5").transform;
					this.tc.CF = GameObject.Find("CF").transform;
					this.tc.CB = GameObject.Find("CB").transform;
					this.tc.M = GameObject.Find("M").transform;
					this.tc.RayCastBox = gameObject.transform;
					this.tc.cam = GameObject.Find("MainCamera").GetComponent<Camera>();
					this.tc.Turret = GameObject.Find(str + ent.go.name + "/root/turret").transform;
					this.tc.Gun = GameObject.Find(str + ent.go.name + "/root/turret/barrel").transform;
					this.tc.FP = GameObject.Find(str + ent.go.name + "/root/turret/barrel/FirePoint").transform;
					GameObject gameObject2 = GameObject.Find(str + ent.go.name + "/blockade_tank_default");
					gameObject2.GetComponent<SkinnedMeshRenderer>().material.mainTexture = SkinManager.GetTankSkin(skin, (int)RemotePlayersUpdater.Instance.Bots[this.myindex].Team);
					gameObject2.GetComponent<MeshCollider>().enabled = false;
					if (this.tc.Gun.GetComponentInChildren<Cannon>() == null)
					{
						this.tc.Gun.gameObject.AddComponent<Cannon>();
					}
					GameObject.Find("WeaponCamera").GetComponent<Camera>().enabled = false;
					this.tc.enabled = true;
					this.tc.enableExit = Time.time + 0.5f;
					this.tc.distFromGround = 2.5f;
				}
				else if (ent.classID == CONST.ENTS.ENT_JEEP)
				{
					if (num3 == CONST.VEHICLES.POSITION_JEEP_DRIVER)
					{
						this.cc.L1 = GameObject.Find("L1").transform;
						this.cc.L3 = GameObject.Find("L3").transform;
						this.cc.L5 = GameObject.Find("L5").transform;
						this.cc.R1 = GameObject.Find("R1").transform;
						this.cc.R3 = GameObject.Find("R3").transform;
						this.cc.R5 = GameObject.Find("R5").transform;
						this.cc.CF = GameObject.Find("CF").transform;
						this.cc.CB = GameObject.Find("CB").transform;
						this.cc.M = GameObject.Find("M").transform;
						this.cc.RayCastBox = gameObject.transform;
					}
					if (this.cc.cam == null)
					{
						this.cc.cam = GameObject.Find("MainCamera").GetComponent<Camera>();
					}
					if (this.cc.Turret == null)
					{
						this.cc.Turret = ent.go.GetComponent<Car>().turret.transform;
					}
					GameObject.Find("WeaponCamera").GetComponent<Camera>().enabled = false;
					this.cc.myPosition = num3;
					this.cc.enabled = true;
					this.cc.enableExit = Time.time + 0.5f;
					this.cc.distFromGround = 2.5f;
				}
				componentInChildren.visota = 5f;
				componentInChildren.enabled = true;
				componentInChildren.target = ent.go.transform;
				base.GetComponent<Sound>().PlaySound_Stop(null);
				return;
			}
			RemotePlayersUpdater.Instance.SetCurrentWeapon(num2, 999);
			string str2 = RemotePlayersUpdater.Instance.BotsGmObj[num2].name + "/";
			RemotePlayersUpdater.Instance.BotsGmObj[num2].transform.parent = null;
			if (ent.classID == CONST.ENTS.ENT_TANK || ent.classID == CONST.ENTS.ENT_TANK_LIGHT || ent.classID == CONST.ENTS.ENT_TANK_MEDIUM || ent.classID == CONST.ENTS.ENT_TANK_HEAVY)
			{
				this.SetBotCollider(num2, false);
				GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[num2].name + "/trooper").GetComponent<SkinnedMeshRenderer>().enabled = false;
				ent.go.transform.parent = RemotePlayersUpdater.Instance.BotsGmObj[num2].transform;
				ent.go.GetComponent<Tank>().id = num2;
				if (ent.classID == CONST.ENTS.ENT_TANK_LIGHT || ent.classID == CONST.ENTS.ENT_TANK_MEDIUM || ent.classID == CONST.ENTS.ENT_TANK_HEAVY)
				{
					ent.go.GetComponent<Tank>().health = health;
					ent.go.GetComponent<Tank>().armor = armor;
					ent.go.GetComponent<Tank>().speed = speed;
					ent.go.GetComponent<Tank>().reload = reload;
					ent.go.GetComponent<Tank>().turretRotation = turretRotation;
					if (RemotePlayersUpdater.Instance.Bots[num2].Item[136] > 0)
					{
						ent.go.GetComponent<Tank>().MG.gameObject.SetActive(true);
					}
				}
				GameObject gameObject3 = GameObject.Find(str2 + ent.go.name + "/blockade_tank_default");
				Texture tankSkin = SkinManager.GetTankSkin(skin, (int)RemotePlayersUpdater.Instance.Bots[num2].Team);
				base.StartCoroutine(this.SetTankSkin(tankSkin, gameObject3.GetComponent<SkinnedMeshRenderer>().material));
				RemotePlayersUpdater.Instance.BotsGmObj[num2].transform.position = ent.go.transform.position;
				RemotePlayersUpdater.Instance.BotsGmObj[num2].transform.rotation = ent.go.transform.rotation;
				ent.go.transform.localPosition = new Vector3(0f, 0f, 0f);
				ent.go.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
			}
			else if (ent.classID == CONST.ENTS.ENT_JEEP)
			{
				if (num3 != CONST.VEHICLES.POSITION_JEEP_GUNNER)
				{
					GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[num2].name + "/trooper").GetComponent<SkinnedMeshRenderer>().enabled = false;
				}
				else
				{
					this.SetBotCollider(num2, true);
					GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[num2].name + "/trooper").GetComponent<SkinnedMeshRenderer>().enabled = true;
				}
				GameObject gameObject4 = ent.go.GetComponent<Car>().JeepMesh.gameObject;
				Texture tankSkin2;
				if (num2 == ent.ownerID)
				{
					tankSkin2 = SkinManager.GetTankSkin(skin, (int)RemotePlayersUpdater.Instance.Bots[num2].Team);
				}
				else
				{
					tankSkin2 = SkinManager.GetTankSkin(3, (int)RemotePlayersUpdater.Instance.Bots[num2].Team);
				}
				ent.go.GetComponent<Car>().health = health;
				ent.go.GetComponent<Car>().armor = armor;
				ent.go.GetComponent<Car>().speed = speed;
				ent.go.GetComponent<Car>().reload = reload;
				ent.go.GetComponent<Car>().turretRotation = turretRotation;
				if (ent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_DRIVER] == num2)
				{
					ent.go.transform.parent = null;
				}
				if (num3 == CONST.VEHICLES.POSITION_JEEP_DRIVER)
				{
					ent.go.transform.parent = RemotePlayersUpdater.Instance.BotsGmObj[num2].transform;
					ent.go.transform.localPosition = new Vector3(0f, 0f, 0f);
					ent.go.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
				}
				else
				{
					if (num3 == CONST.VEHICLES.POSITION_JEEP_PASS)
					{
						RemotePlayersUpdater.Instance.BotsGmObj[num2].transform.parent = ent.go.transform;
					}
					else
					{
						RemotePlayersUpdater.Instance.BotsGmObj[num2].transform.parent = ent.go.GetComponent<Car>().GunnerPos.transform;
					}
					RemotePlayersUpdater.Instance.BotsGmObj[num2].transform.localPosition = new Vector3(0f, 0f, 0f);
					RemotePlayersUpdater.Instance.BotsGmObj[num2].transform.localRotation = Quaternion.Euler(new Vector3(0f, -45f, 0f));
				}
				ent.go.GetComponent<Car>().CheckSlots(num2);
				if (num2 == ent.ownerID || (ent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_DRIVER] == CONST.VEHICLES.POSITION_NONE && ent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_GUNNER] == CONST.VEHICLES.POSITION_NONE && ent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_PASS] == CONST.VEHICLES.POSITION_NONE))
				{
					base.StartCoroutine(this.SetTankSkin(tankSkin2, gameObject4.GetComponent<SkinnedMeshRenderer>().materials[1]));
				}
				ent.go.GetComponent<Car>().slots[num3] = num2;
			}
			RemotePlayersUpdater.Instance.Bots[num2].inVehicle = true;
			RemotePlayersUpdater.Instance.Bots[num2].inVehiclePos = num3;
		}
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x0005B6EC File Offset: 0x000598EC
	private void SetBotCollider(int id, bool value)
	{
		Collider[] componentsInChildren = RemotePlayersUpdater.Instance.BotsGmObj[id].GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = value;
		}
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x0005B722 File Offset: 0x00059922
	private IEnumerator SetTankSkin(Texture tex, Material mat)
	{
		yield return null;
		mat.mainTexture = tex;
		yield break;
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x0005B738 File Offset: 0x00059938
	private void recv_exit_the_ent(byte[] buffer, int len)
	{
		int num = 4;
		int uid = this.DecodeInteger(buffer, num);
		num += 4;
		int num2 = this.DecodeInteger(buffer, num);
		num += 4;
		CEnt ent = EntManager.GetEnt(uid);
		GameObject gameObject = GameObject.Find("RayCastBox");
		if (ent != null)
		{
			if (num2 == this.myindex)
			{
				if (this.tc == null)
				{
					this.tc = base.gameObject.GetComponent<TankController>();
				}
				if (this.cc == null)
				{
					this.cc = base.gameObject.GetComponent<CarController>();
				}
				ent.go.transform.parent = null;
				GameObject gameObject2;
				if (ent.classID == CONST.ENTS.ENT_JEEP)
				{
					gameObject2 = ent.go.GetComponent<Car>().JeepMesh.gameObject;
					if (this.cc.myPosition == CONST.VEHICLES.POSITION_JEEP_GUNNER)
					{
						ent.go.GetComponent<Car>().gunner = false;
						ent.go.GetComponent<Car>().UnactiveGunner();
					}
					else
					{
						gameObject2.GetComponent<SkinnedMeshRenderer>().enabled = true;
					}
					ent.go.GetComponent<Car>().CheckSlots(num2);
					this.cc.cam.gameObject.GetComponent<AudioSource>().Stop();
					this.cc.currCar = null;
					this.cc.enabled = false;
					if (ent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_DRIVER] == CONST.VEHICLES.POSITION_NONE && ent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_GUNNER] == CONST.VEHICLES.POSITION_NONE && ent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_PASS] == CONST.VEHICLES.POSITION_NONE)
					{
						gameObject2.GetComponent<SkinnedMeshRenderer>().materials[1].mainTexture = SkinManager.GetTankSkin(3, 2);
					}
				}
				else
				{
					ent.go.GetComponent<Tank>().client = false;
					gameObject2 = GameObject.Find(ent.go.name + "/blockade_tank_default");
					gameObject2.GetComponent<SkinnedMeshRenderer>().enabled = true;
					this.tc.cam.gameObject.GetComponent<AudioSource>().Stop();
					this.tc.currTank = null;
					this.tc.enabled = false;
					gameObject2.GetComponent<SkinnedMeshRenderer>().material.mainTexture = SkinManager.GetTankSkin(1, 2);
				}
				((Ammo)Object.FindObjectOfType(typeof(Ammo))).draw = true;
				base.gameObject.GetComponentInChildren<OrbitCam>().enabled = false;
				base.gameObject.GetComponent<TransportExit>().enabled = false;
				base.gameObject.GetComponent<TransportExit>().vehicleType = CONST.VEHICLES.NONE;
				base.gameObject.GetComponent<vp_FPController>().enabled = true;
				base.gameObject.GetComponent<vp_FPInput>().enabled = true;
				base.gameObject.GetComponent<vp_FPWeaponHandler>().enabled = true;
				base.gameObject.GetComponent<TransportDetect>().enabled = true;
				base.gameObject.GetComponentInChildren<vp_FPCamera>().enabled = true;
				GameObject.Find("WeaponCamera").GetComponent<Camera>().enabled = true;
				gameObject2.GetComponent<MeshCollider>().enabled = true;
				gameObject.transform.position = new Vector3(0f, 0f, 0f);
				return;
			}
			this.SetBotCollider(num2, true);
			GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[num2].name + "/trooper").GetComponent<SkinnedMeshRenderer>().enabled = true;
			if (ent.classID == CONST.ENTS.ENT_TANK || ent.classID == CONST.ENTS.ENT_TANK_LIGHT || ent.classID == CONST.ENTS.ENT_TANK_MEDIUM || ent.classID == CONST.ENTS.ENT_TANK_HEAVY)
			{
				if (ent.classID == CONST.ENTS.ENT_TANK)
				{
					ent.go.GetComponent<Tank>().id = 200;
				}
				ent.go.transform.parent = null;
				ent.go.GetComponent<Tank>().s = null;
				GameObject.Find(ent.go.name + "/blockade_tank_default").GetComponent<SkinnedMeshRenderer>().material.mainTexture = SkinManager.GetTankSkin(1, 2);
			}
			else if (ent.classID == CONST.ENTS.ENT_JEEP)
			{
				ent.go.GetComponent<Car>().s = null;
				GameObject gameObject3 = ent.go.GetComponent<Car>().JeepMesh.gameObject;
				if (RemotePlayersUpdater.Instance.Bots[num2].inVehiclePos == CONST.VEHICLES.POSITION_JEEP_DRIVER)
				{
					ent.go.transform.parent = null;
				}
				else
				{
					RemotePlayersUpdater.Instance.BotsGmObj[num2].transform.parent = null;
				}
				ent.go.GetComponent<Car>().CheckSlots(num2);
				if (ent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_DRIVER] == CONST.VEHICLES.POSITION_NONE && ent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_GUNNER] == CONST.VEHICLES.POSITION_NONE && ent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_PASS] == CONST.VEHICLES.POSITION_NONE)
				{
					gameObject3.GetComponent<SkinnedMeshRenderer>().materials[1].mainTexture = SkinManager.GetTankSkin(3, 2);
				}
			}
			RemotePlayersUpdater.Instance.Bots[num2].inVehicle = false;
			RemotePlayersUpdater.Instance.Bots[num2].inVehiclePos = CONST.VEHICLES.POSITION_NONE;
		}
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x0005BC70 File Offset: 0x00059E70
	private void recv_vehicle_turret(byte[] buffer, int len)
	{
		int num = 4;
		float t_x = this.DecodeSingle(buffer, num);
		num += 4;
		float t_z = this.DecodeSingle(buffer, num);
		num += 4;
		float t_ry = this.DecodeSingle(buffer, num);
		num += 4;
		float g_rx = this.DecodeSingle(buffer, num);
		num += 4;
		int uid = this.DecodeInteger(buffer, num);
		num += 4;
		int num2 = this.DecodeInteger(buffer, num);
		num += 4;
		if (num2 != this.myindex)
		{
			CEnt ent = EntManager.GetEnt(uid);
			if (ent != null)
			{
				if (ent.classID == CONST.ENTS.ENT_TANK || ent.classID == CONST.ENTS.ENT_TANK_LIGHT || ent.classID == CONST.ENTS.ENT_TANK_MEDIUM || ent.classID == CONST.ENTS.ENT_TANK_HEAVY)
				{
					Tank component = ent.go.GetComponent<Tank>();
					component.t_x = t_x;
					component.t_z = t_z;
					component.t_ry = t_ry;
					component.g_rx = g_rx;
					return;
				}
				if (ent.classID == CONST.ENTS.ENT_JEEP)
				{
					Car component2 = ent.go.GetComponent<Car>();
					if (RemotePlayersUpdater.Instance.Bots[num2].inVehiclePos == CONST.VEHICLES.POSITION_JEEP_DRIVER)
					{
						component2.t_x = t_x;
						component2.t_z = t_z;
						return;
					}
					if (RemotePlayersUpdater.Instance.Bots[num2].inVehiclePos == CONST.VEHICLES.POSITION_JEEP_GUNNER)
					{
						component2.t_ry = t_ry;
					}
				}
			}
		}
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x0005BDB8 File Offset: 0x00059FB8
	private void recv_vehicle_health(byte[] buffer, int len)
	{
		int num = 4;
		int uid = this.DecodeInteger(buffer, num);
		num += 4;
		int health = this.DecodeInteger(buffer, num);
		num += 4;
		int armor = this.DecodeInteger(buffer, num);
		num += 4;
		CEnt ent = EntManager.GetEnt(uid);
		if (ent != null)
		{
			if (ent.classID == CONST.ENTS.ENT_TANK || ent.classID == CONST.ENTS.ENT_TANK_LIGHT || ent.classID == CONST.ENTS.ENT_TANK_MEDIUM || ent.classID == CONST.ENTS.ENT_TANK_HEAVY)
			{
				Tank component = ent.go.GetComponent<Tank>();
				if (component != null)
				{
					component.health = health;
					component.armor = armor;
					return;
				}
			}
			else if (ent.classID == CONST.ENTS.ENT_JEEP)
			{
				Car component2 = ent.go.GetComponent<Car>();
				if (component2 != null)
				{
					component2.health = health;
					component2.armor = armor;
				}
			}
		}
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x0005BE8C File Offset: 0x0005A08C
	private void recv_vehicle_explode(byte[] buffer, int len)
	{
		int num = 4;
		int uid = this.DecodeInteger(buffer, num);
		num += 4;
		byte epic = buffer[num];
		num++;
		CEnt ent = EntManager.GetEnt(uid);
		if (ent != null)
		{
			if (ent.classID == CONST.ENTS.ENT_TANK || ent.classID == CONST.ENTS.ENT_TANK_LIGHT || ent.classID == CONST.ENTS.ENT_TANK_MEDIUM || ent.classID == CONST.ENTS.ENT_TANK_HEAVY)
			{
				this.csrm.CreateEmptyTankRagDoll(ent.go, epic);
				return;
			}
			if (ent.classID == CONST.ENTS.ENT_JEEP)
			{
				Car component = ent.go.GetComponent<Car>();
				if (component.slots[CONST.VEHICLES.POSITION_JEEP_GUNNER] != CONST.VEHICLES.POSITION_NONE)
				{
					if (component.slots[CONST.VEHICLES.POSITION_JEEP_GUNNER] == this.myindex)
					{
						this.LocalPlayer.transform.parent = null;
					}
					else
					{
						RemotePlayersUpdater.Instance.BotsGmObj[component.slots[CONST.VEHICLES.POSITION_JEEP_GUNNER]].transform.parent = null;
					}
				}
				if (component.slots[CONST.VEHICLES.POSITION_JEEP_PASS] != CONST.VEHICLES.POSITION_NONE)
				{
					if (component.slots[CONST.VEHICLES.POSITION_JEEP_PASS] == this.myindex)
					{
						this.LocalPlayer.transform.parent = null;
					}
					else
					{
						RemotePlayersUpdater.Instance.BotsGmObj[component.slots[CONST.VEHICLES.POSITION_JEEP_GUNNER]].transform.parent = null;
					}
				}
				this.csrm.CreateEmptyJeepRagDoll(ent.go, epic);
			}
		}
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x0005BFE8 File Offset: 0x0005A1E8
	private void recv_vehicle_hit(byte[] buffer, int len)
	{
		int num = 4;
		int uid = this.DecodeInteger(buffer, num);
		num += 4;
		int num2 = this.DecodeInteger(buffer, num);
		num += 4;
		int armor = this.DecodeInteger(buffer, num);
		num += 4;
		int num3 = this.DecodeInteger(buffer, num);
		num += 4;
		int num4 = this.DecodeInteger(buffer, num);
		num += 4;
		if (num3 == this.myindex)
		{
			this.LocalPlayer.GetComponent<PlayerControl>().SetTraceHit();
			this.goGui.GetComponent<Crosshit>().Hit();
		}
		CEnt ent = EntManager.GetEnt(uid);
		if (ent != null)
		{
			if (num4 == this.myindex)
			{
				this.goGui.GetComponent<Health>().SetDamageIndicator(num3);
				base.gameObject.GetComponentInChildren<OrbitCam>().shake = 0.5f;
			}
			if (num2 <= 0)
			{
				num2 = 0;
			}
			if (ent.classID == CONST.ENTS.ENT_TANK || ent.classID == CONST.ENTS.ENT_TANK_LIGHT || ent.classID == CONST.ENTS.ENT_TANK_MEDIUM || ent.classID == CONST.ENTS.ENT_TANK_HEAVY)
			{
				ent.go.GetComponent<Tank>().health = num2;
				ent.go.GetComponent<Tank>().armor = armor;
				if (ent.go.GetComponent<Tank>().client)
				{
					if (this.tc == null)
					{
						this.tc = base.gameObject.GetComponent<TankController>();
					}
					base.GetComponent<Sound>().PlaySound_TankHit(this.tc.cam.GetComponent<AudioSource>());
					return;
				}
			}
			else if (ent.classID == CONST.ENTS.ENT_JEEP)
			{
				ent.go.GetComponent<Car>().health = num2;
				ent.go.GetComponent<Car>().armor = armor;
				if (this.cc == null)
				{
					this.cc = base.gameObject.GetComponent<CarController>();
				}
				if (this.cc.enabled)
				{
					base.GetComponent<Sound>().PlaySound_TankHit(this.cc.cam.GetComponent<AudioSource>());
				}
			}
		}
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x0005C1CC File Offset: 0x0005A3CC
	private void recv_vehicle_targeting(byte[] buffer, int len)
	{
		if (this.tc == null)
		{
			this.tc = base.GetComponent<TankController>();
		}
		if (this.tc.enabled)
		{
			this.tc.javelinAIM(1);
		}
		if (this.cc == null)
		{
			this.cc = base.GetComponent<CarController>();
		}
		if (this.cc.enabled)
		{
			this.cc.javelinAIM(1);
		}
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x0005C240 File Offset: 0x0005A440
	private void recv_ent_health(byte[] buffer, int len)
	{
		int num = 4;
		int uid = this.DecodeInteger(buffer, num);
		num += 4;
		int num2 = this.DecodeInteger(buffer, num);
		num += 4;
		int num3 = this.DecodeInteger(buffer, num);
		num += 4;
		this.DecodeInteger(buffer, num);
		num += 4;
		if (num3 == this.myindex)
		{
			this.goGui.GetComponent<Crosshit>().Hit();
		}
		CEnt ent = EntManager.GetEnt(uid);
		if (ent != null)
		{
			if (num2 <= 0)
			{
				num2 = 0;
			}
			if (ent.classID == 78 && ent.go.GetComponent<Pashalka>().health > num2)
			{
				ent.go.GetComponent<Pashalka>().SetHealth(num2);
			}
		}
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x0005C2D8 File Offset: 0x0005A4D8
	private void recv_zplayerpos(byte[] buffer, int len)
	{
		byte[] array = new byte[len - 4];
		int num = 0;
		for (int i = 4; i < len; i++)
		{
			array[num] = buffer[i];
			num++;
		}
		byte[] array2 = ZlibStream.UncompressBuffer(array);
		int j = 0;
		while (j < array2.Length)
		{
			int num2 = (int)array2[j];
			j++;
			if (num2 == this.myindex)
			{
				j += 9;
			}
			else
			{
				ushort num3 = this.DecodeShort2(array2, j);
				j += 2;
				ushort num4 = this.DecodeShort2(array2, j);
				j += 2;
				float num5 = (float)this.DecodeShort2(array2, j);
				j += 2;
				float pX = (float)num3 * 0.00390625f;
				float pY = (float)num4 * 0.00390625f;
				float pZ = num5 * 0.00390625f;
				float rX = (float)array2[j] * 360f / 256f;
				j++;
				float rY = (float)array2[j] * 360f / 256f;
				j++;
				int state = (int)array2[j];
				j++;
				RemotePlayersUpdater.Instance.UpdatePosition(num2, pX, pY, pZ, rX, rY, 0f, state);
			}
		}
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x0005C3D8 File Offset: 0x0005A5D8
	private void recv_zentpos(byte[] buffer, int len)
	{
		byte[] array = new byte[len - 4];
		int num = 0;
		for (int i = 4; i < len; i++)
		{
			array[num] = buffer[i];
			num++;
		}
		byte[] array2 = ZlibStream.UncompressBuffer(array);
		int j = 0;
		while (j < array2.Length)
		{
			int id = (int)array2[j];
			j++;
			int uid = this.DecodeInteger(array2, j);
			j += 4;
			int num2 = (int)array2[j];
			j++;
			ushort num3 = this.DecodeShort2(array2, j);
			j += 2;
			ushort num4 = this.DecodeShort2(array2, j);
			j += 2;
			float num5 = (float)this.DecodeShort2(array2, j);
			j += 2;
			float num6 = (float)num3 * 0.00390625f;
			float num7 = (float)num4 * 0.00390625f;
			float num8 = num5 * 0.00390625f;
			CEnt ent = EntManager.GetEnt(uid);
			if (ent == null)
			{
				if (num2 != CONST.ENTS.ENT_GHOST && num2 != CONST.ENTS.ENT_GHOST_BOSS)
				{
					this.csrm.CreateZombie(id, uid, num6, num7, num8, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
				}
			}
			else
			{
				Vector3 b = new Vector3(ent.go.transform.position.x, ent.go.transform.position.y, ent.go.transform.position.z);
				Vector3 vector = new Vector3(num6, num7 + 0.5f, num8);
				float y = Health.AngleSigned(new Vector3(0f, 0f, 1f), vector - b, ent.go.transform.up);
				ent.position = vector;
				ent.rotation = new Vector3(0f, y, 0f);
			}
		}
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x0005C5A8 File Offset: 0x0005A7A8
	private void recv_chunk(byte[] buffer, int len)
	{
		int num = 4;
		int num2 = (int)buffer[num];
		num++;
		int num3 = (int)buffer[num];
		num++;
		int num4 = (int)buffer[num];
		num++;
		byte[] array = new byte[len - 4 - 3];
		int num5 = 0;
		for (int i = 7; i < len; i++)
		{
			array[num5] = buffer[i];
			num5++;
		}
		byte[] array2 = ZlibStream.UncompressBuffer(array);
		int num6 = 0;
		for (int j = 0; j < 16; j++)
		{
			for (int k = 0; k < 16; k++)
			{
				for (int l = 0; l < 16; l++)
				{
					int num7 = (int)array2[num6];
					num6++;
					if (num7 != 0)
					{
						Vector3i vector3i = new Vector3i(j + num2 * 16, k + num3 * 16, l + num4 * 16);
						Block block = this.ziploader.GetBlock(num7);
						if (block == null)
						{
							block = this.ziploader.brick;
						}
						this.map.SetBlock(block, vector3i);
					}
				}
			}
		}
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x0005C69E File Offset: 0x0005A89E
	private void recv_chunk_finish(byte[] buffer, int len)
	{
		MonoBehaviour.print("NET.recv_chunk_finish");
		GM.currExtState = GAME_STATES.GAMELOADMAPCHANGESCOMPLITE;
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x0005C6B4 File Offset: 0x0005A8B4
	private void recv_mapdata(byte[] buffer, int len)
	{
		this.ziploader.mapversion = 1;
		int num = 4;
		if (this.mode == 0 || this.mode == 1)
		{
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					int x = (int)buffer[num];
					num++;
					int y = (int)buffer[num];
					num++;
					int z = (int)buffer[num];
					num++;
					this.ziploader.rblock.Add(new CRespawnBlock(i, x, y, z, this.mode));
				}
			}
			return;
		}
		if (this.mode == 5 || this.mode == 11 || this.mode == 8)
		{
			for (int k = 0; k < 32; k++)
			{
				int t = 0;
				if (k >= 16)
				{
					t = 1;
				}
				int x2 = (int)buffer[num];
				num++;
				int y2 = (int)buffer[num];
				num++;
				int z2 = (int)buffer[num];
				num++;
				this.ziploader.rblock.Add(new CRespawnBlock(t, x2, y2, z2, this.mode));
			}
			return;
		}
		if (this.mode == 9)
		{
			for (int l = 0; l < 4; l++)
			{
				for (int m = 0; m < 8; m++)
				{
					int x3 = (int)buffer[num];
					num++;
					int y3 = (int)buffer[num];
					num++;
					int z3 = (int)buffer[num];
					num++;
					this.ziploader.rblock.Add(new CRespawnBlock(l, x3, y3, z3, 0));
				}
			}
			for (int n = 0; n < 32; n++)
			{
				int t2 = 0;
				if (n >= 16)
				{
					t2 = 1;
				}
				int x4 = (int)buffer[num];
				num++;
				int y4 = (int)buffer[num];
				num++;
				int z4 = (int)buffer[num];
				num++;
				this.ziploader.rblock.Add(new CRespawnBlock(t2, x4, y4, z4, this.mode));
			}
		}
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x0005C870 File Offset: 0x0005AA70
	private void recv_flag_set(byte[] buffer, int len)
	{
		int num = 4;
		int num2 = this.DecodeInteger(buffer, num);
		num += 4;
		int x = this.DecodeInteger(buffer, num);
		num += 4;
		int y = this.DecodeInteger(buffer, num);
		num += 4;
		int z = this.DecodeInteger(buffer, num);
		num += 4;
		int t = this.DecodeInteger(buffer, num);
		num += 4;
		int t2 = this.DecodeInteger(buffer, num);
		num += 4;
		if (this.map == null)
		{
			this.map = Object.FindObjectOfType<Map>();
		}
		if (this.map == null)
		{
			return;
		}
		this.map.flags[num2].SetFlag(new Vector3i(x, y, z), t, t2);
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x0005C918 File Offset: 0x0005AB18
	private void recv_flag_update(byte[] buffer, int len)
	{
		int num = 4;
		int num2 = this.DecodeInteger(buffer, num);
		num += 4;
		int t = this.DecodeInteger(buffer, num);
		num += 4;
		int t2 = this.DecodeInteger(buffer, num);
		num += 4;
		if (this.map == null)
		{
			this.map = Object.FindObjectOfType<Map>();
		}
		if (this.map == null)
		{
			return;
		}
		if (!this.map.flags[num2].inited)
		{
			return;
		}
		this.map.flags[num2].UpdateFlag(t, t2);
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x0005C99F File Offset: 0x0005AB9F
	private void recv_accept_weapons(byte[] buffer, int len)
	{
		if (this.MG == null)
		{
			this.MG = (MainGUI)Object.FindObjectOfType(typeof(MainGUI));
		}
		this.MG.CloseAll();
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x0005C9D4 File Offset: 0x0005ABD4
	private void recv_selected_block(byte[] buffer, int len)
	{
		int num = 4;
		int id = (int)buffer[num];
		num++;
		int flag = (int)buffer[num];
		num++;
		RemotePlayersUpdater.Instance.SetCurrentWeaponBlock(id, flag);
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x0005C9FF File Offset: 0x0005ABFF
	public void send_dummy()
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(200, false);
		this.WRITE_STRING("DUMMY STRING");
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x0005CA30 File Offset: 0x0005AC30
	public void send_auth()
	{
		if (!this.active)
		{
			return;
		}
		if (this.client == null)
		{
			return;
		}
		this.BEGIN_WRITE(0, false);
		this.WRITE_BYTE((byte)PlayerProfile.country);
		this.WRITE_BYTE((byte)PlayerProfile.network);
		this.WRITE_LONG(ConnectionInfo.PASSWORD);
		this.WRITE_STRING_CLASSIC(PlayerProfile.id);
		this.WRITE_STRING_CLASSIC(PlayerProfile.authkey);
		this.WRITE_STRING_CLASSIC(PlayerProfile.session);
		this.WRITE_STRING_CLASSIC(PlayerProfile.gameSession);
		this.END_WRITE();
		this.SendPacket();
		MonoBehaviour.print("send_auth");
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x0005CABC File Offset: 0x0005ACBC
	public void send_position(byte state)
	{
		if (!this.active)
		{
			return;
		}
		if (this.LocalPlayer == null)
		{
			return;
		}
		float num = Camera.main.transform.rotation.eulerAngles.x;
		if (num > 0f && num <= 90f)
		{
			num *= -1f;
		}
		else if (num >= 270f && num <= 360f)
		{
			num = 360f - num;
		}
		num += 90f;
		this.BEGIN_WRITE(1, false);
		this.WRITE_FLOAT(this.LocalPlayer.transform.position.x);
		this.WRITE_FLOAT(this.LocalPlayer.transform.position.y);
		this.WRITE_FLOAT(this.LocalPlayer.transform.position.z);
		this.WRITE_FLOAT(num);
		this.WRITE_FLOAT(this.LocalPlayer.transform.rotation.eulerAngles.y);
		this.WRITE_BYTE(state);
		this.WRITE_BYTE(PlayerProfile.loh);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x0005CBD9 File Offset: 0x0005ADD9
	private bool send_bonus()
	{
		if (!this.active)
		{
			return false;
		}
		this.BEGIN_WRITE(2, false);
		this.WRITE_BYTE(PlayerProfile.loh);
		this.END_WRITE();
		this.SendPacket();
		return true;
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x0005CC05 File Offset: 0x0005AE05
	public void send_myinfo()
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(3, false);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x0005CC24 File Offset: 0x0005AE24
	public void send_attackblock(int x, int y, int z, int weaponid, float fvalue, float x1 = 0f, float y1 = 0f, float z1 = 0f, float x2 = 0f, float y2 = 0f, float z2 = 0f)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(4, false);
		this.WRITE_LONG(x);
		this.WRITE_LONG(y);
		this.WRITE_LONG(z);
		this.WRITE_LONG(weaponid);
		this.WRITE_FLOAT(fvalue);
		this.WRITE_FLOAT(x1);
		this.WRITE_FLOAT(y1);
		this.WRITE_FLOAT(z1);
		this.WRITE_FLOAT(x2);
		this.WRITE_FLOAT(y2);
		this.WRITE_FLOAT(z2);
		this.WRITE_BYTE(PlayerProfile.loh);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x0005CCAE File Offset: 0x0005AEAE
	public void send_blockinfo()
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(5, false);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x0005CCCD File Offset: 0x0005AECD
	public void send_jointeamclass(byte _team, byte _class)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(9, false);
		this.WRITE_BYTE(_team);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x0005CCF4 File Offset: 0x0005AEF4
	public void send_auto_jointeamclass()
	{
		if (!this.active)
		{
			return;
		}
		byte ivalue = 1;
		byte ivalue2 = 4;
		byte ivalue3 = 9;
		byte ivalue4 = 0;
		byte ivalue5 = 0;
		byte ivalue6 = 0;
		byte ivalue7 = 0;
		byte ivalue8 = 0;
		if (PlayerPrefs.HasKey("PWID"))
		{
			ivalue2 = (byte)PlayerPrefs.GetInt("PWID");
		}
		if (PlayerPrefs.HasKey("SWID"))
		{
			ivalue3 = (byte)PlayerPrefs.GetInt("SWID");
		}
		if (PlayerPrefs.HasKey("MWID"))
		{
			ivalue = (byte)PlayerPrefs.GetInt("MWID");
		}
		if (PlayerPrefs.HasKey("A1WID"))
		{
			ivalue4 = (byte)PlayerPrefs.GetInt("A1WID");
		}
		if (PlayerPrefs.HasKey("A2WID"))
		{
			ivalue5 = (byte)PlayerPrefs.GetInt("A2WID");
		}
		if (PlayerPrefs.HasKey("A3WID"))
		{
			ivalue6 = (byte)PlayerPrefs.GetInt("A3WID");
		}
		if (PlayerPrefs.HasKey("G1WID"))
		{
			ivalue7 = (byte)PlayerPrefs.GetInt("G1WID");
		}
		if (PlayerPrefs.HasKey("G2WID"))
		{
			ivalue8 = (byte)PlayerPrefs.GetInt("G2WID");
		}
		this.BEGIN_WRITE(102, false);
		this.WRITE_LONG((int)ivalue);
		this.WRITE_LONG((int)ivalue2);
		this.WRITE_LONG((int)ivalue3);
		this.WRITE_LONG((int)ivalue4);
		this.WRITE_LONG((int)ivalue5);
		this.WRITE_LONG((int)ivalue6);
		this.WRITE_LONG((int)ivalue7);
		this.WRITE_LONG((int)ivalue8);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x0005CE34 File Offset: 0x0005B034
	public void send_last_config(byte _team)
	{
		if (!this.active)
		{
			return;
		}
		byte ivalue = 4;
		byte ivalue2 = 9;
		byte ivalue3 = 1;
		if (PlayerPrefs.HasKey("PWID"))
		{
			ivalue = (byte)PlayerPrefs.GetInt("PWID");
		}
		if (PlayerPrefs.HasKey("SWID"))
		{
			ivalue2 = (byte)PlayerPrefs.GetInt("SWID");
		}
		if (PlayerPrefs.HasKey("MWID"))
		{
			ivalue3 = (byte)PlayerPrefs.GetInt("MWID");
		}
		this.BEGIN_WRITE(10, false);
		this.WRITE_BYTE(_team);
		this.WRITE_BYTE(0);
		this.WRITE_LONG((int)ivalue);
		this.WRITE_LONG((int)ivalue2);
		this.WRITE_LONG((int)ivalue3);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x0005CED4 File Offset: 0x0005B0D4
	public void send_new_config(int _mwid, int _pwid, int _swid, int _ammo1, int _ammo2, int _ammo3, int _gr1, int _gr2)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(101, false);
		this.WRITE_LONG(_mwid);
		this.WRITE_LONG(_pwid);
		this.WRITE_LONG(_swid);
		this.WRITE_LONG(_ammo1);
		this.WRITE_LONG(_ammo2);
		this.WRITE_LONG(_ammo3);
		this.WRITE_LONG(_gr1);
		this.WRITE_LONG(_gr2);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x0005CF3C File Offset: 0x0005B13C
	public void send_damage(int weaponid, int victimid, int hitbox, float fvalue, float ax, float ay, float az, float vx, float vy, float vz, float x1 = 0f, float y1 = 0f, float z1 = 0f, float x2 = 0f, float y2 = 0f, float z2 = 0f)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(7, false);
		this.WRITE_LONG(weaponid);
		this.WRITE_LONG(victimid);
		this.WRITE_BYTE((byte)hitbox);
		this.WRITE_FLOAT(fvalue);
		this.WRITE_FLOAT(ax);
		this.WRITE_FLOAT(ay);
		this.WRITE_FLOAT(az);
		this.WRITE_FLOAT(vx);
		this.WRITE_FLOAT(vy);
		this.WRITE_FLOAT(vz);
		this.WRITE_FLOAT(x1);
		this.WRITE_FLOAT(y1);
		this.WRITE_FLOAT(z1);
		this.WRITE_FLOAT(x2);
		this.WRITE_FLOAT(y2);
		this.WRITE_FLOAT(z2);
		this.WRITE_BYTE(PlayerProfile.loh);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x0005CFEF File Offset: 0x0005B1EF
	public void send_milkattack(int weaponid, float fvalue)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(11, false);
		this.WRITE_LONG(weaponid);
		this.WRITE_FLOAT(fvalue);
		this.WRITE_BYTE(PlayerProfile.loh);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x0005D028 File Offset: 0x0005B228
	public void send_setblock(int x, int y, int z, float fvalue)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(12, false);
		this.WRITE_BYTE((byte)x);
		this.WRITE_BYTE((byte)y);
		this.WRITE_BYTE((byte)z);
		this.WRITE_FLOAT(fvalue);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x0005D068 File Offset: 0x0005B268
	public void send_chat(byte teamsay, string msg)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(13, false);
		this.WRITE_BYTE(teamsay);
		this.WRITE_STRING(msg);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x0005D096 File Offset: 0x0005B296
	public void send_currentweapon(int weaponid)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(15, false);
		this.WRITE_LONG(weaponid);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x0005D0BD File Offset: 0x0005B2BD
	public void send_disconnect()
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(18, false);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x0005D0DD File Offset: 0x0005B2DD
	public void send_selectblock(byte flag)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(20, false);
		this.WRITE_BYTE(flag);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x0005D104 File Offset: 0x0005B304
	public void send_savemap()
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(22, false);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x0005D124 File Offset: 0x0005B324
	public void send_createent(Vector3 pos, Vector3 rot, Vector3 force, Vector3 torque, int enttypeid)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(24, false);
		this.WRITE_BYTE((byte)enttypeid);
		this.WRITE_FLOAT(pos.x);
		this.WRITE_FLOAT(pos.y);
		this.WRITE_FLOAT(pos.z);
		this.WRITE_FLOAT(rot.x);
		this.WRITE_FLOAT(rot.y);
		this.WRITE_FLOAT(rot.z);
		this.WRITE_FLOAT(force.x);
		this.WRITE_FLOAT(force.y);
		this.WRITE_FLOAT(force.z);
		this.WRITE_FLOAT(torque.x);
		this.WRITE_FLOAT(torque.y);
		this.WRITE_FLOAT(torque.z);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x0005D1EC File Offset: 0x0005B3EC
	public void send_detonateent(int uid, Vector3 pos)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(25, false);
		this.WRITE_LONG(uid);
		this.WRITE_FLOAT(pos.x);
		this.WRITE_FLOAT(pos.y);
		this.WRITE_FLOAT(pos.z);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x0005D242 File Offset: 0x0005B442
	public void send_private_settings(int gamemode, int mapid)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(29, false);
		this.WRITE_BYTE((byte)gamemode);
		this.WRITE_LONG(mapid);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x0005D271 File Offset: 0x0005B471
	public void send_spawn_me()
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(31, false);
		this.WRITE_BYTE(1);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x0005D298 File Offset: 0x0005B498
	public void send_prereload(int weaponid, float fvalue)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(33, false);
		this.WRITE_LONG(weaponid);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x0005D298 File Offset: 0x0005B498
	public void send_reload(int weaponid, float fvalue)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(33, false);
		this.WRITE_LONG(weaponid);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x0005D2BF File Offset: 0x0005B4BF
	public void send_private_command(int cmd, int pid, int param1)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(39, false);
		this.WRITE_BYTE((byte)cmd);
		this.WRITE_BYTE((byte)pid);
		this.WRITE_LONG(param1);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x0005D2F8 File Offset: 0x0005B4F8
	public void send_private_command(int cmd, int pid, int param1, byte x1, byte x2, byte y1, byte y2, byte z1, byte z2)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(39, false);
		this.WRITE_BYTE((byte)cmd);
		this.WRITE_BYTE((byte)pid);
		this.WRITE_LONG(param1);
		this.WRITE_BYTE(x1);
		this.WRITE_BYTE(x2);
		this.WRITE_BYTE(y1);
		this.WRITE_BYTE(y2);
		this.WRITE_BYTE(z1);
		this.WRITE_BYTE(z2);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x0005D36A File Offset: 0x0005B56A
	public void send_weapon_medkit(int medkit)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(41, false);
		this.WRITE_BYTE((byte)medkit);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x0005D392 File Offset: 0x0005B592
	public void send_weapon_tnt(int x, int y, int z, float fvalue)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(42, false);
		this.WRITE_BYTE((byte)x);
		this.WRITE_BYTE((byte)y);
		this.WRITE_BYTE((byte)z);
		this.WRITE_FLOAT(fvalue);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x0005D3D2 File Offset: 0x0005B5D2
	public void send_enter_the_ent(int ent_id, int pos_id = 200)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(60, false);
		this.WRITE_LONG(ent_id);
		this.WRITE_LONG(pos_id);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x0005D400 File Offset: 0x0005B600
	public void send_attack_ent(int uid, int wid)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(91, false);
		this.WRITE_LONG(uid);
		this.WRITE_LONG(wid);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x0005D42E File Offset: 0x0005B62E
	public void send_spawn_my_vehicle(int wid)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(92, false);
		this.WRITE_LONG(wid);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x0005D455 File Offset: 0x0005B655
	public void send_exit_the_ent(int ent_id)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(64, false);
		this.WRITE_LONG(ent_id);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x0005D47C File Offset: 0x0005B67C
	public void send_vehicle_turret(Vector3 T_r, float T_lr, Vector3 G_lr, int ent_id)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(65, false);
		this.WRITE_FLOAT(T_r.x);
		this.WRITE_FLOAT(T_r.z);
		this.WRITE_FLOAT(T_lr);
		this.WRITE_FLOAT(G_lr.x);
		this.WRITE_LONG(ent_id);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x0005D4DA File Offset: 0x0005B6DA
	public void send_vehicle_targeting(int id)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(67, false);
		this.WRITE_BYTE((byte)id);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x0005D502 File Offset: 0x0005B702
	public void send_use_module(int module_index)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(66, false);
		this.WRITE_LONG(module_index);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x0005D529 File Offset: 0x0005B729
	public void send_detonate_my_c4()
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(93, false);
		this.WRITE_BYTE(1);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x0005D550 File Offset: 0x0005B750
	public void send_new_ent_pos(int uid, Vector3 pos)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(94, false);
		this.WRITE_LONG(uid);
		this.WRITE_FLOAT(pos.x);
		this.WRITE_FLOAT(pos.y);
		this.WRITE_FLOAT(pos.z);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x0005D5A6 File Offset: 0x0005B7A6
	public void send_mission_status(int mID)
	{
		if (!this.active)
		{
			return;
		}
		this.BEGIN_WRITE(151, false);
		this.WRITE_LONG(mID);
		this.END_WRITE();
		this.SendPacket();
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x0005D5D0 File Offset: 0x0005B7D0
	private void BEGIN_WRITE(byte cmd, bool ignore_time = false)
	{
		this.writepos = 0;
		this.WRITE_BYTE(245);
		this.WRITE_BYTE(cmd);
		this.WRITE_BYTE(0);
		this.WRITE_BYTE(0);
		if (ignore_time)
		{
			return;
		}
		this.WRITE_LONG((int)(Time.time * 1000f));
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x0005D60F File Offset: 0x0005B80F
	private void WRITE_BYTE(byte bvalue)
	{
		this.sendbuffer[this.writepos] = bvalue;
		this.writepos++;
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x0005D630 File Offset: 0x0005B830
	private void WRITE_SHORT(short svalue)
	{
		byte[] array = this.EncodeShort(svalue);
		this.sendbuffer[this.writepos] = array[0];
		this.sendbuffer[this.writepos + 1] = array[1];
		this.writepos += 2;
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x0005D678 File Offset: 0x0005B878
	private void WRITE_FLOAT(float fvalue)
	{
		byte[] array = this.EncodeFloat(fvalue);
		this.sendbuffer[this.writepos] = array[0];
		this.sendbuffer[this.writepos + 1] = array[1];
		this.sendbuffer[this.writepos + 2] = array[2];
		this.sendbuffer[this.writepos + 3] = array[3];
		this.writepos += 4;
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x0005D6E4 File Offset: 0x0005B8E4
	private void WRITE_LONG(int ivalue)
	{
		byte[] array = this.EncodeInteger(ivalue);
		this.sendbuffer[this.writepos] = array[0];
		this.sendbuffer[this.writepos + 1] = array[1];
		this.sendbuffer[this.writepos + 2] = array[2];
		this.sendbuffer[this.writepos + 3] = array[3];
		this.writepos += 4;
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x0005D750 File Offset: 0x0005B950
	private void WRITE_STRING_CLASSIC(string svalue)
	{
		UTF8Encoding utf8Encoding = new UTF8Encoding();
		int byteCount = utf8Encoding.GetByteCount(svalue);
		byte[] array = new byte[byteCount];
		Buffer.BlockCopy(utf8Encoding.GetBytes(svalue), 0, array, 0, byteCount);
		for (int i = 0; i < byteCount; i++)
		{
			this.WRITE_BYTE(array[i]);
		}
		this.WRITE_BYTE(0);
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x0005D79C File Offset: 0x0005B99C
	private void WRITE_STRING(string svalue)
	{
		UTF8Encoding utf8Encoding = new UTF8Encoding();
		int byteCount = utf8Encoding.GetByteCount(svalue);
		this.WRITE_LONG(byteCount);
		byte[] array = new byte[byteCount];
		Buffer.BlockCopy(utf8Encoding.GetBytes(svalue), 0, array, 0, byteCount);
		for (int i = 0; i < byteCount; i++)
		{
			this.WRITE_BYTE(array[i]);
		}
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x0005D7E8 File Offset: 0x0005B9E8
	private int WRITE_LEN()
	{
		return this.writepos;
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x0005D7F0 File Offset: 0x0005B9F0
	private void END_WRITE()
	{
		short svalue = (short)this.writepos;
		this.writepos = 2;
		this.WRITE_SHORT(svalue);
		this.writepos = (int)svalue;
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x0005D81A File Offset: 0x0005BA1A
	public byte[] EncodeShort(short inShort)
	{
		return BitConverter.GetBytes(inShort);
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x0005D822 File Offset: 0x0005BA22
	public byte[] EncodeInteger(int inInt)
	{
		return BitConverter.GetBytes(inInt);
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x0005D82A File Offset: 0x0005BA2A
	public byte[] EncodeFloat(float inFloat)
	{
		return BitConverter.GetBytes(inFloat);
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x0005D834 File Offset: 0x0005BA34
	public byte[] EncodeStringUTF8(string inString)
	{
		UTF8Encoding utf8Encoding = new UTF8Encoding();
		int byteCount = utf8Encoding.GetByteCount(inString);
		byte[] array = new byte[byteCount];
		Buffer.BlockCopy(utf8Encoding.GetBytes(inString), 0, array, 0, byteCount);
		return array;
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x0005D868 File Offset: 0x0005BA68
	public byte[] EncodeStringASCII(string inString)
	{
		ASCIIEncoding asciiencoding = new ASCIIEncoding();
		int byteCount = asciiencoding.GetByteCount(inString);
		byte[] array = new byte[byteCount];
		Buffer.BlockCopy(asciiencoding.GetBytes(inString), 0, array, 0, byteCount);
		return array;
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x0005D89C File Offset: 0x0005BA9C
	public byte[] EncodeVector2(Vector2 inObject)
	{
		byte[] array = new byte[8];
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.x), 0, array, 0, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.y), 0, array, 4, 4);
		return array;
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x0005D8DC File Offset: 0x0005BADC
	public byte[] EncodeVector3(Vector3 inObject)
	{
		byte[] array = new byte[12];
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.x), 0, array, 0, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.y), 0, array, 4, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.z), 0, array, 8, 4);
		return array;
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x0005D930 File Offset: 0x0005BB30
	public byte[] EncodeVector4(Vector4 inObject)
	{
		byte[] array = new byte[16];
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.x), 0, array, 0, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.y), 0, array, 4, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.z), 0, array, 8, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.w), 0, array, 12, 4);
		return array;
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x0005D998 File Offset: 0x0005BB98
	private byte[] EncodeQuaternion(Quaternion inObject)
	{
		byte[] array = new byte[16];
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.x), 0, array, 0, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.y), 0, array, 4, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.z), 0, array, 8, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.w), 0, array, 12, 4);
		return array;
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x0005DA00 File Offset: 0x0005BC00
	public byte[] EncodeColor(Color inObject)
	{
		byte[] array = new byte[16];
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.r), 0, array, 0, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.g), 0, array, 4, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.b), 0, array, 8, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.a), 0, array, 12, 4);
		return array;
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x0005DA67 File Offset: 0x0005BC67
	public int DecodeShort(byte[] inBytes, int pos)
	{
		return (int)BitConverter.ToUInt16(inBytes, pos);
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x0005DA67 File Offset: 0x0005BC67
	public ushort DecodeShort2(byte[] inBytes, int pos)
	{
		return BitConverter.ToUInt16(inBytes, pos);
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x0005DA70 File Offset: 0x0005BC70
	public int DecodeInteger(byte[] inBytes, int pos)
	{
		return BitConverter.ToInt32(inBytes, pos);
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x0005DA79 File Offset: 0x0005BC79
	public float DecodeSingle(byte[] inBytes, int pos)
	{
		return BitConverter.ToSingle(inBytes, pos);
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x0005DA84 File Offset: 0x0005BC84
	public Vector2 DecodeVector2(byte[] inBytes)
	{
		return new Vector2
		{
			x = BitConverter.ToSingle(inBytes, 0),
			y = BitConverter.ToSingle(inBytes, 4)
		};
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x0005DAB8 File Offset: 0x0005BCB8
	public Vector3 DecodeVector3(byte[] inBytes)
	{
		return new Vector3
		{
			x = BitConverter.ToSingle(inBytes, 0),
			y = BitConverter.ToSingle(inBytes, 4),
			z = BitConverter.ToSingle(inBytes, 8)
		};
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x0005DAF8 File Offset: 0x0005BCF8
	public Vector4 DecodeVector4(byte[] inBytes)
	{
		return new Vector4
		{
			x = BitConverter.ToSingle(inBytes, 0),
			y = BitConverter.ToSingle(inBytes, 4),
			z = BitConverter.ToSingle(inBytes, 8),
			w = BitConverter.ToSingle(inBytes, 12)
		};
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x0005DB48 File Offset: 0x0005BD48
	private Quaternion DecodeQuaternion(byte[] inBytes)
	{
		return new Quaternion
		{
			x = BitConverter.ToSingle(inBytes, 0),
			y = BitConverter.ToSingle(inBytes, 4),
			z = BitConverter.ToSingle(inBytes, 8),
			w = BitConverter.ToSingle(inBytes, 12)
		};
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x0005DB98 File Offset: 0x0005BD98
	public Color DecodeColor(byte[] inBytes)
	{
		return new Color(0f, 0f, 0f, 0f)
		{
			r = BitConverter.ToSingle(inBytes, 0),
			g = BitConverter.ToSingle(inBytes, 4),
			b = BitConverter.ToSingle(inBytes, 8),
			a = BitConverter.ToSingle(inBytes, 12)
		};
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x0005DC00 File Offset: 0x0005BE00
	private void OnGUI()
	{
		if (!this.net_stats)
		{
			return;
		}
		float num = 60f;
		for (int i = 0; i < 256; i++)
		{
			if (this.net_stats_packet[i] != 0)
			{
				GUI.Label(new Rect(150f, num, 200f, 20f), string.Concat(new string[]
				{
					i.ToString(),
					": ",
					this.net_stats_packet[i].ToString(),
					" ",
					this.net_stats_size[i].ToString(),
					" bytes"
				}));
				num += 22f;
			}
		}
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x0005DCB3 File Offset: 0x0005BEB3
	public void BEGIN_READ(byte[] inBytes, int len, int startpos)
	{
		this.readbuffer = inBytes;
		this.readlen = len;
		this.readpos = startpos;
		this.readerror = false;
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x0005DCD1 File Offset: 0x0005BED1
	public int READ_BYTE()
	{
		if (this.readpos >= this.readlen)
		{
			this.readerror = true;
			return 0;
		}
		int result = (int)this.readbuffer[this.readpos];
		this.readpos++;
		return result;
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x0005DD05 File Offset: 0x0005BF05
	public int READ_LONG()
	{
		if (this.readpos + 4 >= this.readlen)
		{
			this.readerror = true;
			return 0;
		}
		int result = this.DecodeInteger(this.readbuffer, this.readpos);
		this.readpos += 4;
		return result;
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x0005DD40 File Offset: 0x0005BF40
	public string READ_STRING()
	{
		int num = 0;
		int index = this.readpos;
		while (this.readpos < this.readlen && this.readbuffer[this.readpos] != 0)
		{
			num++;
			this.readpos++;
		}
		this.readpos++;
		if (num == 0)
		{
			return "";
		}
		return Encoding.UTF8.GetString(this.readbuffer, index, num);
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x0005DDB0 File Offset: 0x0005BFB0
	public float READ_COORD()
	{
		if (this.readpos + 2 >= this.readlen)
		{
			this.readerror = true;
			return 0f;
		}
		float result = (float)this.DecodeShort2(this.readbuffer, this.readpos) * (1f / Client.MAX_COEF);
		this.readpos += 2;
		return result;
	}

	// Token: 0x04000933 RID: 2355
	public static Client THIS;

	// Token: 0x04000934 RID: 2356
	public int myindex;

	// Token: 0x04000935 RID: 2357
	public int zs_wave = 1;

	// Token: 0x04000936 RID: 2358
	public TcpClient client;

	// Token: 0x04000937 RID: 2359
	private string lastMessage;

	// Token: 0x04000938 RID: 2360
	private GameObject LocalPlayer;

	// Token: 0x04000939 RID: 2361
	private bool active;

	// Token: 0x0400093A RID: 2362
	private bool ready_for_auth;

	// Token: 0x0400093B RID: 2363
	private GameObject goMap;

	// Token: 0x0400093C RID: 2364
	private Map map;

	// Token: 0x0400093D RID: 2365
	private ZipLoader ziploader;

	// Token: 0x0400093E RID: 2366
	private RagDollManager csrm;

	// Token: 0x0400093F RID: 2367
	private ParticleManager cspm;

	// Token: 0x04000940 RID: 2368
	private vp_FPPlayerEventHandler Player;

	// Token: 0x04000941 RID: 2369
	public PlayerControl cspc;

	// Token: 0x04000942 RID: 2370
	private GameObject goGui;

	// Token: 0x04000943 RID: 2371
	private TeamScore csTeamScore;

	// Token: 0x04000944 RID: 2372
	private List<Client.RecvData> Tlist = new List<Client.RecvData>();

	// Token: 0x04000945 RID: 2373
	private byte[] sendbuffer = new byte[1025];

	// Token: 0x04000946 RID: 2374
	private List<Vector3i> pos = new List<Vector3i>();

	// Token: 0x04000947 RID: 2375
	private int[] net_stats_packet = new int[256];

	// Token: 0x04000948 RID: 2376
	private int[] net_stats_size = new int[256];

	// Token: 0x04000949 RID: 2377
	private bool net_stats;

	// Token: 0x0400094A RID: 2378
	private static float MAX_COEF = 256f;

	// Token: 0x0400094B RID: 2379
	private TankController tc;

	// Token: 0x0400094C RID: 2380
	private CarController cc;

	// Token: 0x0400094D RID: 2381
	private string load_msg1;

	// Token: 0x0400094E RID: 2382
	private string load_msg2;

	// Token: 0x0400094F RID: 2383
	private int oldtime = Environment.TickCount;

	// Token: 0x04000950 RID: 2384
	private Dictionary<int, string> ass_list = new Dictionary<int, string>();

	// Token: 0x04000951 RID: 2385
	private bool inited;

	// Token: 0x04000952 RID: 2386
	private bool potratil;

	// Token: 0x04000953 RID: 2387
	private float missions_timeout;

	// Token: 0x04000954 RID: 2388
	private byte[] readBuffer = new byte[1048576];

	// Token: 0x04000955 RID: 2389
	private int SplitRead;

	// Token: 0x04000956 RID: 2390
	private int BytesRead;

	// Token: 0x04000957 RID: 2391
	private int mode;

	// Token: 0x04000958 RID: 2392
	private MainGUI MG;

	// Token: 0x04000959 RID: 2393
	private int writepos;

	// Token: 0x0400095A RID: 2394
	private byte[] readbuffer;

	// Token: 0x0400095B RID: 2395
	private int readlen;

	// Token: 0x0400095C RID: 2396
	private int readpos;

	// Token: 0x0400095D RID: 2397
	private bool readerror;

	// Token: 0x02000872 RID: 2162
	public class RecvData
	{
		// Token: 0x06004BF8 RID: 19448 RVA: 0x001AA524 File Offset: 0x001A8724
		public RecvData(byte[] _buffer, int _len)
		{
			this.Buffer = new byte[_len];
			for (int i = 0; i < _len; i++)
			{
				this.Buffer[i] = _buffer[i];
			}
			this.Len = _len;
		}

		// Token: 0x06004BF9 RID: 19449 RVA: 0x001AA564 File Offset: 0x001A8764
		~RecvData()
		{
			this.Buffer = null;
		}

		// Token: 0x040032DE RID: 13022
		public byte[] Buffer;

		// Token: 0x040032DF RID: 13023
		public int Len;
	}
}
