using System;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class TransportDetect : MonoBehaviour
{
	// Token: 0x0600029D RID: 669 RVA: 0x00036BD4 File Offset: 0x00034DD4
	private void Start()
	{
		this.cscr = (Crosshair)Object.FindObjectOfType(typeof(Crosshair));
		this.cscr.SetActive(true);
		this.lockT = ContentLoader.LoadTexture("javelin_lock");
		this.MyTransform = base.transform;
		this.gui_style = new GUIStyle();
		this.gui_style.font = this.f;
		this.gui_style.fontSize = 12;
		this.gui_style.normal.textColor = new Color(1f, 1f, 1f, 1f);
		this.gui_style.alignment = TextAnchor.MiddleLeft;
	}

	// Token: 0x0600029E RID: 670 RVA: 0x00036C84 File Offset: 0x00034E84
	private void Update()
	{
		if (GM.currGUIState == GUIGS.WEAPONSELECT || GM.currGUIState == GUIGS.TEAMSELECT)
		{
			return;
		}
		if (this.cl == null)
		{
			this.cl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		if (this.WS == null)
		{
			this.WS = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		if (this.activeControl && this.cscr.active && !MainGUI.e_menu && !MainGUI.sel_team)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f)), out raycastHit, 2f))
			{
				if (raycastHit.collider.gameObject.name == "blockade_tank_default")
				{
					if (raycastHit.collider.transform.parent.gameObject.GetComponent<Tank>().id == 200 || raycastHit.collider.transform.parent.gameObject.GetComponent<Tank>().id == this.cl.myindex)
					{
						this.ico = true;
						this.my = true;
						if (Input.GetKeyDown(KeyCode.F))
						{
							this.cl.send_enter_the_ent(raycastHit.collider.transform.parent.gameObject.GetComponent<Tank>().uid, 200);
						}
					}
					else if (raycastHit.collider.transform.parent.gameObject.GetComponent<Tank>().id != 200)
					{
						this.ico = true;
						this.my = false;
					}
					else
					{
						this.ico = false;
						this.my = false;
					}
					this.icoIndex = 0;
				}
				else if (raycastHit.collider.gameObject.name == "blockade_jeep")
				{
					Car component = raycastHit.collider.transform.parent.gameObject.GetComponent<Car>();
					if ((component.slots[CONST.VEHICLES.POSITION_JEEP_DRIVER] == CONST.VEHICLES.POSITION_NONE || component.slots[CONST.VEHICLES.POSITION_JEEP_GUNNER] == CONST.VEHICLES.POSITION_NONE || component.slots[CONST.VEHICLES.POSITION_JEEP_PASS] == CONST.VEHICLES.POSITION_NONE) && component.team == PlayerProfile.myteam)
					{
						this.ico = true;
						this.my = true;
						if (Input.GetKeyDown(KeyCode.F))
						{
							this.cl.send_enter_the_ent(component.uid, 200);
						}
					}
					else
					{
						this.ico = true;
						this.my = false;
					}
					this.icoIndex = 1;
				}
				else
				{
					this.ico = false;
					this.my = false;
					this.icoIndex = 0;
				}
			}
			else
			{
				this.ico = false;
				this.my = false;
				this.icoIndex = 0;
			}
		}
		if (this.AS == null)
		{
			this.AS = GameObject.Find("Player/MainCamera/TMPAudio").GetComponent<AudioSource>();
		}
		if (this.WS.targetLock)
		{
			if (this.startTargetingTime == 0f || Time.time - this.startTargetingTime < 3.187f)
			{
				if ((this.targetingTime == 0f || this.targetingTime < Time.time) && !this.targeting)
				{
					if (this.targetingTime == 0f)
					{
						this.startTargetingTime = Time.time;
					}
					this.targetingTime = Time.time + 0.375f;
					this.targeting = true;
					if (this.lastJavelinState != this.WS.targetLock)
					{
						this.lastJavelinState = this.WS.targetLock;
						base.GetComponent<Sound>().PlaySound_JavelinTargeting(this.AS);
					}
				}
				if (this.targetingTime < Time.time && this.targeting)
				{
					this.targetingTime = Time.time + 0.375f;
					this.targeting = false;
					return;
				}
			}
			else
			{
				if (Time.time - this.startTargetingTime >= 3.187f && Time.time - this.startTargetingTime <= 5.25f)
				{
					this.targeting = true;
					this.WS.targetLocked = true;
					return;
				}
				if (Time.time - this.startTargetingTime > 5.25f)
				{
					this.targeting = false;
					this.WS.targetLocked = false;
					base.GetComponent<Sound>().PlaySound_Stop(this.AS);
					return;
				}
			}
		}
		else
		{
			this.startTargetingTime = 0f;
			this.targetingTime = 0f;
			this.targeting = false;
			this.WS.targetLocked = false;
			if (this.lastJavelinState != this.WS.targetLock)
			{
				this.lastJavelinState = this.WS.targetLock;
				base.GetComponent<Sound>().PlaySound_Stop(this.AS);
			}
		}
	}

	// Token: 0x0600029F RID: 671 RVA: 0x00037148 File Offset: 0x00035348
	private void OnGUI()
	{
		if (this.ico)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width / 2 - 30), (float)(Screen.height / 2 - 80), 60f, 60f), this.icons[this.icoIndex]);
			if (this.my)
			{
				GUI.Label(new Rect((float)(Screen.width / 2 - 35), (float)(Screen.height / 2 - 55), 10f, 20f), "F", this.gui_style);
				if (this.icoIndex == 0)
				{
					GUI.Label(new Rect((float)(Screen.width / 2 - 45), (float)(Screen.height / 2 - 40), 100f, 20f), Lang.GetLabel(222), this.gui_style);
				}
				else
				{
					GUI.Label(new Rect((float)(Screen.width / 2 - 45), (float)(Screen.height / 2 - 40), 100f, 20f), Lang.GetLabel(478), this.gui_style);
				}
			}
			else
			{
				GUI.Label(new Rect((float)(Screen.width / 2 - 45), (float)(Screen.height / 2 - 40), 100f, 20f), Lang.GetLabel(223), this.gui_style);
			}
		}
		if (this.targeting && this.WS.target != null)
		{
			this.DrawTargetLock(this.WS.target.position);
		}
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x000372C4 File Offset: 0x000354C4
	private void DrawTargetLock(Vector3 p)
	{
		p.y += 1f;
		Vector3 vector = Camera.main.WorldToScreenPoint(p);
		vector.y = (float)Screen.height - vector.y;
		if (Vector3.Angle(p - this.MyTransform.position, this.MyTransform.forward) < 90f && vector.z > 0f)
		{
			GUI.DrawTexture(new Rect(vector.x - 32f, vector.y - 32f, 64f, 64f), this.lockT);
		}
	}

	// Token: 0x04000505 RID: 1285
	public Texture[] icons;

	// Token: 0x04000506 RID: 1286
	private int icoIndex;

	// Token: 0x04000507 RID: 1287
	public Texture lockT;

	// Token: 0x04000508 RID: 1288
	private bool ico;

	// Token: 0x04000509 RID: 1289
	private bool my;

	// Token: 0x0400050A RID: 1290
	private Client cl;

	// Token: 0x0400050B RID: 1291
	private PauseGUI pGUI;

	// Token: 0x0400050C RID: 1292
	public bool activeControl = true;

	// Token: 0x0400050D RID: 1293
	private Transform MyTransform;

	// Token: 0x0400050E RID: 1294
	public AudioSource AS;

	// Token: 0x0400050F RID: 1295
	private float targetingTime;

	// Token: 0x04000510 RID: 1296
	private float startTargetingTime;

	// Token: 0x04000511 RID: 1297
	protected vp_FPInput m_Input;

	// Token: 0x04000512 RID: 1298
	private WeaponSystem WS;

	// Token: 0x04000513 RID: 1299
	private Crosshair cscr;

	// Token: 0x04000514 RID: 1300
	private bool targeting;

	// Token: 0x04000515 RID: 1301
	private bool lastJavelinState;

	// Token: 0x04000516 RID: 1302
	private GUIStyle gui_style;

	// Token: 0x04000517 RID: 1303
	public Font f;
}
