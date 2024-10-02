using System;
using UnityEngine;

// Token: 0x02000047 RID: 71
public class SpawnManager : MonoBehaviour
{
	// Token: 0x06000227 RID: 551 RVA: 0x0002ABCC File Offset: 0x00028DCC
	private void Awake()
	{
		this.goGUI = GameObject.Find("GUI");
		this.csb = (Batch)Object.FindObjectOfType(typeof(Batch));
		this.csr = this.goGUI.GetComponent<Radar>();
		this.cspc = (PlayerControl)Object.FindObjectOfType(typeof(PlayerControl));
		this.gui_style = new GUIStyle();
		this.gui_style.font = FontManager.font[0];
		this.gui_style.fontSize = 20;
		this.gui_style.normal.textColor = new Color(255f, 255f, 255f, 255f);
		this.gui_style.alignment = TextAnchor.MiddleCenter;
	}

	// Token: 0x06000228 RID: 552 RVA: 0x0002AC90 File Offset: 0x00028E90
	public void PreSpawn()
	{
		this.goGUI.GetComponent<Radar>().ForceUpdateRadar();
		if (CONST.GetGameMode() != MODE.FFA)
		{
			this.goGUI.GetComponent<MainGUI>().OpenSelectTeam();
		}
		this.goGUI.GetComponent<LoadScreen>().enabled = false;
		GM.currMainState = GAME_STATES.GAME;
	}

	// Token: 0x06000229 RID: 553 RVA: 0x0002ACE0 File Offset: 0x00028EE0
	public void Spawn(float x, float y, float z)
	{
		if (this.LocalPlayer == null)
		{
			this.LocalPlayer = GameObject.Find("Player");
		}
		if (this.LocalPlayer.GetComponent<TankController>() != null || this.LocalPlayer.GetComponent<CarController>() != null)
		{
			this.LocalPlayer.GetComponent<TankController>().currTank = null;
			this.LocalPlayer.GetComponent<TankController>().enabled = false;
			this.LocalPlayer.GetComponent<CarController>().currCar = null;
			this.LocalPlayer.GetComponent<CarController>().enabled = false;
			this.LocalPlayer.GetComponentInChildren<OrbitCam>().enabled = false;
			this.LocalPlayer.GetComponent<TransportExit>().enabled = false;
			this.LocalPlayer.GetComponent<TransportExit>().vehicleType = CONST.VEHICLES.NONE;
		}
		this.LocalPlayer.GetComponent<vp_FPController>().enabled = true;
		this.LocalPlayer.GetComponent<vp_FPInput>().enabled = true;
		this.LocalPlayer.GetComponent<vp_FPWeaponHandler>().enabled = true;
		this.LocalPlayer.GetComponent<TransportDetect>().enabled = true;
		this.LocalPlayer.GetComponentInChildren<vp_FPCamera>().enabled = true;
		GameObject.Find("WeaponCamera").GetComponent<Camera>().enabled = true;
		((Crosshair)Object.FindObjectOfType(typeof(Crosshair))).SetActive(true);
		this.goCamera = null;
		this.DeathPos = Vector3.zero;
		this.msgshow = false;
		this.last_follow_player_index = -1;
		if (this.csws == null)
		{
			this.csws = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		this.csws.SetPrimaryWeapon(false);
		this.csb.Combine();
		if (this.m_Controller == null)
		{
			this.m_Controller = (vp_FPController)Object.FindObjectOfType(typeof(vp_FPController));
		}
		if (this.m_Controller)
		{
			this.m_Controller.ClearPos(x, y, z);
		}
	}

	// Token: 0x0600022A RID: 554 RVA: 0x0002AED4 File Offset: 0x000290D4
	public void SpawnCamera(GameObject head)
	{
		if (this.csws == null)
		{
			this.csws = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		this.csws.HideWeapons(true);
		this.goCamera = head;
		if (this.LocalPlayer == null)
		{
			this.LocalPlayer = GameObject.Find("Player");
		}
		if (this.LocalPlayer != null)
		{
			this.LocalPlayer.GetComponent<Sound>().PlaySound_Death();
		}
	}

	// Token: 0x0600022B RID: 555 RVA: 0x0002AF58 File Offset: 0x00029158
	private void LateUpdate()
	{
		if (this.last_follow_player_index > -1 && (!RemotePlayersUpdater.Instance.Bots[this.last_follow_player_index].Active || RemotePlayersUpdater.Instance.Bots[this.last_follow_player_index].Dead > 0))
		{
			this.SetRandomFollow(this._myindex, this.last_follow_player_index);
		}
		if (this.goCamera)
		{
			if (this.csws == null)
			{
				this.csws = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
			}
			if (this.csws == null)
			{
				return;
			}
			Camera.main.transform.position = this.goCamera.transform.position;
			Camera.main.transform.rotation = this.goCamera.transform.rotation;
			this.csws.HideWeapons(true);
			this.DeathPos = this.goCamera.transform.position;
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.SetRandomFollow(this._myindex, this.last_follow_player_index);
			}
		}
		if (this.DeathPos != Vector3.zero)
		{
			if (this.csws == null)
			{
				this.csws = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
			}
			if (this.csws == null)
			{
				return;
			}
			Camera.main.transform.position = this.DeathPos;
			this.csws.HideWeapons(true);
		}
	}

	// Token: 0x0600022C RID: 556 RVA: 0x0002B0E0 File Offset: 0x000292E0
	private void Update()
	{
		if (this.waiting_for_respawn && Input.GetMouseButtonDown(0) && CONST.GetGameMode() != MODE.FFA)
		{
			if (this.cl == null)
			{
				this.cl = Object.FindObjectOfType<Client>();
			}
			if (this.cl == null)
			{
				return;
			}
			this.cl.send_spawn_me();
			this.waiting_for_respawn = false;
		}
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0002B140 File Offset: 0x00029340
	private void OnGUI()
	{
		if (RemotePlayersUpdater.Instance != null && this.last_follow_player_index >= 0)
		{
			string text = RemotePlayersUpdater.Instance.Bots[this.last_follow_player_index].Name;
			if (RemotePlayersUpdater.Instance.Bots[this.last_follow_player_index].ClanName.Length > 0)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					Lang.GetLabel(10),
					": ",
					RemotePlayersUpdater.Instance.Bots[this.last_follow_player_index].ClanName
				});
			}
			this.gui_style.normal.textColor = GUIManager.c[9];
			GUI.Label(new Rect(2f, 22f, (float)(Screen.width - 2), 50f), text, this.gui_style);
			this.gui_style.normal.textColor = GUIManager.c[8];
			GUI.Label(new Rect(0f, 20f, (float)Screen.width, 50f), text, this.gui_style);
		}
		if (!this.msgshow)
		{
			return;
		}
		GUI.depth = -1;
		GUIManager.DrawText(this.rSpectatormsg, this.spectatormsg, 20, TextAnchor.MiddleCenter, 8);
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0002B28C File Offset: 0x0002948C
	public void SetRandomFollow(int myindex, int _last_index)
	{
		this._myindex = myindex;
		if (RemotePlayersUpdater.Instance == null)
		{
			return;
		}
		for (int i = 0; i < 32; i++)
		{
			if (RemotePlayersUpdater.Instance.Bots[i].Active && RemotePlayersUpdater.Instance.Bots[i].Dead <= 0 && (RemotePlayersUpdater.Instance.Bots[i].Team == RemotePlayersUpdater.Instance.Bots[myindex].Team || RemotePlayersUpdater.Instance.Bots[myindex].Team == 255) && i > _last_index && i != myindex)
			{
				this.SetFollow(i);
				return;
			}
		}
		this.goCamera = GameObject.Find("CamPos");
		this.last_follow_player_index = -1;
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0002B345 File Offset: 0x00029545
	public void SetFollow(int index)
	{
		this.goCamera = RemotePlayersUpdater.Instance.Bots[index].SpecView;
		this.last_follow_player_index = index;
	}

	// Token: 0x06000230 RID: 560 RVA: 0x0002B365 File Offset: 0x00029565
	public void SetSpectatorMsg(string msg)
	{
		this.spectatormsg = msg;
		this.rSpectatormsg = new Rect(0f, (float)Screen.height / 6f, (float)Screen.width, 32f);
		this.msgshow = true;
	}

	// Token: 0x040002DF RID: 735
	private GameObject goGUI;

	// Token: 0x040002E0 RID: 736
	private WeaponSystem csws;

	// Token: 0x040002E1 RID: 737
	private Batch csb;

	// Token: 0x040002E2 RID: 738
	private Radar csr;

	// Token: 0x040002E3 RID: 739
	private Client cl;

	// Token: 0x040002E4 RID: 740
	private PlayerControl cspc;

	// Token: 0x040002E5 RID: 741
	private int last_follow_player_index = -1;

	// Token: 0x040002E6 RID: 742
	private int _myindex = -1;

	// Token: 0x040002E7 RID: 743
	private Vector3 DeathPos = Vector3.zero;

	// Token: 0x040002E8 RID: 744
	private GameObject goCamera;

	// Token: 0x040002E9 RID: 745
	private GameObject LocalPlayer;

	// Token: 0x040002EA RID: 746
	private vp_FPController m_Controller;

	// Token: 0x040002EB RID: 747
	private bool msgshow;

	// Token: 0x040002EC RID: 748
	private string spectatormsg = "";

	// Token: 0x040002ED RID: 749
	private Rect rSpectatormsg;

	// Token: 0x040002EE RID: 750
	private GUIStyle gui_style;

	// Token: 0x040002EF RID: 751
	public bool waiting_for_respawn;
}
