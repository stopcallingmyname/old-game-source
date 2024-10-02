using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class TankController : MonoBehaviour
{
	// Token: 0x06000285 RID: 645 RVA: 0x000321E0 File Offset: 0x000303E0
	private void Start()
	{
		this.s = base.GetComponent<Sound>();
		this.csmap = (Map)Object.FindObjectOfType(typeof(Map));
		this.cspm = (ParticleManager)Object.FindObjectOfType(typeof(ParticleManager));
		this.m_FPCamera = base.transform.root.GetComponentInChildren<vp_FPCamera>();
		this.Player = (vp_FPPlayerEventHandler)Object.FindObjectOfType(typeof(vp_FPPlayerEventHandler));
		this.oc = (OrbitCam)Object.FindObjectOfType(typeof(OrbitCam));
		this.WS = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		this.AS = this.cam.gameObject.AddComponent<AudioSource>();
		this.AS.pitch = 1f;
		this.myTransform = base.transform;
		this.state = 0;
		this.lastState = 0;
		this.lastPos = this.myTransform.position;
		this.lastRot = this.myTransform.eulerAngles;
		this.angX = 0f;
		this.angZ = 0f;
		this.angXKoef = 0f;
		this.MissleAIMTime = 0f;
		this.MissleAIMTimer = 0f;
		this.underAIM = false;
		this.underMissleAIM = false;
		this.missle = null;
		this.snaryadTexFull[0] = (Resources.Load("GUI/snaryad_full" + Lang.current.ToString()) as Texture);
		this.snaryadTexEmpty[0] = (Resources.Load("GUI/snaryad_empty" + Lang.current.ToString()) as Texture);
		this.snaryadTexFull[1] = (Resources.Load("GUI/kumul_full" + Lang.current.ToString()) as Texture);
		this.snaryadTexEmpty[1] = (Resources.Load("GUI/kumul_empty" + Lang.current.ToString()) as Texture);
		this.snaryadTexFull[2] = (Resources.Load("GUI/fugas_full" + Lang.current.ToString()) as Texture);
		this.snaryadTexEmpty[2] = (Resources.Load("GUI/fugas_empty" + Lang.current.ToString()) as Texture);
		this.repairKitTex = (Resources.Load("GUI/repair_kit") as Texture);
		this.moduleFlash = (Resources.Load("GUI/flash32") as Texture);
		this.moduleSmoke = (Resources.Load("GUI/smoke32") as Texture);
		this.tankMGTex = (Resources.Load("GUI/tankMG" + Lang.current.ToString()) as Texture);
		this.ammo_machinegun = (Resources.Load("GUI/ammo_mp5") as Texture);
		this.currSnaraydId[0] = 14;
		this.currSnaraydId[1] = 19;
		this.currSnaraydId[2] = 20;
		this.BasePosition = (Resources.Load("GUI/tank_indicator_body") as Texture2D);
		this.TurretPosition = (Resources.Load("GUI/tank_indicator_head") as Texture2D);
		this.indicatorAIM = (Resources.Load("GUI/target") as Texture);
		this.turretState = this.Turret.localRotation.eulerAngles;
		this.tik = Time.time;
		this.gui_style = new GUIStyle();
		this.gui_style.font = FontManager.font[0];
		this.gui_style.fontSize = 24;
		this.gui_style.normal.textColor = new Color(1f, 1f, 1f, 1f);
		this.gui_style.alignment = TextAnchor.MiddleLeft;
	}

	// Token: 0x06000286 RID: 646 RVA: 0x0003257B File Offset: 0x0003077B
	private void OnEnable()
	{
		this.MissleAIMTime = 0f;
		this.MissleAIMTimer = 0f;
		this.underAIM = false;
		this.underMissleAIM = false;
		this.missle = null;
	}

	// Token: 0x06000287 RID: 647 RVA: 0x000325A8 File Offset: 0x000307A8
	private void OnGUI()
	{
		if (this.currTank == null)
		{
			this.currTank = base.gameObject.GetComponentInChildren<Tank>();
		}
		string text = "";
		if (this.currSnaraydIndex == 0)
		{
			text = this.WS.GetSnaryad().ToString();
		}
		else if (this.currSnaraydIndex == 1)
		{
			text = this.WS.GetZBK18M().ToString();
		}
		else if (this.currSnaraydIndex == 2)
		{
			text = this.WS.GetZOF26().ToString();
		}
		this.gui_style.fontSize = 70;
		this.gui_style.normal.textColor = GUIManager.c[9];
		GUI.Label(new Rect((float)(Screen.width - 58 - 128 - 70 + 2 - 35 - 15), (float)(Screen.height - 10 - 40 + 2), 48f, 48f), text, this.gui_style);
		this.gui_style.normal.textColor = GUIManager.c[8];
		GUI.Label(new Rect((float)(Screen.width - 58 - 128 - 70 - 35 - 15), (float)(Screen.height - 10 - 40), 48f, 48f), text, this.gui_style);
		if (this.WS.GetModuleRepairKit() > 0)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width - 40), (float)(Screen.height - 196), 32f, 32f), this.repairKitTex);
		}
		if (this.WS.GetModuleFlash() > 0)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width - 40 - 42 - 42 - 42), (float)(Screen.height - 196), 32f, 32f), this.moduleFlash);
		}
		if (this.WS.GetModuleSmoke() > 0)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width - 40 - 42 - 42), (float)(Screen.height - 196), 32f, 32f), this.moduleSmoke);
		}
		if (Time.time > this.lastShotTime + this.ReloadDuration * (float)this.currTank.reload / 100f)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width - 58 - 128 - 15), (float)(Screen.height - 10 - 48), 48f, 48f), this.snaryadTexFull[this.currSnaraydIndex]);
		}
		else
		{
			GUI.DrawTexture(new Rect((float)(Screen.width - 58 - 128 - 15), (float)(Screen.height - 10 - 48), 48f, 48f), this.snaryadTexEmpty[this.currSnaraydIndex]);
		}
		if (this.underAIM)
		{
			GUI.DrawTexture(new Rect(GUIManager.XRES(512f) + 100f, GUIManager.YRES(768f) - 40f, 32f, 32f), this.indicatorAIM);
			if (this.AIMAudio == null)
			{
				this.AIMAudio = this.cam.gameObject.AddComponent<AudioSource>();
				this.AIMAudio.maxDistance = 35f;
				this.AIMAudio.spatialBlend = 1f;
			}
			base.GetComponent<Sound>().PlaySound_JavelinAIM(this.AIMAudio);
		}
		if (this.underMissleAIM && this.missle)
		{
			float num = 1f - 0.014285714f * Vector3.Distance(this.missle.position, this.myTransform.position);
			if (this.AIMAudio == null)
			{
				this.AIMAudio = this.cam.gameObject.AddComponent<AudioSource>();
				this.AIMAudio.maxDistance = 35f;
				this.AIMAudio.spatialBlend = 1f;
			}
			base.GetComponent<Sound>().PlaySound_Pich(num, this.AIMAudio);
			base.GetComponent<Sound>().PlaySound_JavelinMissleAIM(this.AIMAudio);
			if (this.MissleAIMTime > 0f && this.MissleAIMTime > Time.time)
			{
				Color color = GUI.color;
				float num2;
				if (this.MissleAIMTime > 0f)
				{
					num2 = 1f / this.MissleAIMTimer * (this.MissleAIMTime - Time.time);
				}
				else
				{
					num2 = 1f;
				}
				if (num2 < 0.1f)
				{
					num2 = 1f;
				}
				GUI.color = new Color(color.r, color.g, color.b, num2);
				GUI.DrawTexture(new Rect(GUIManager.XRES(512f) + 100f, GUIManager.YRES(768f) - 40f, 32f, 32f), this.indicatorAIM);
				GUI.color = color;
			}
			else
			{
				this.MissleAIMTime = Time.time + 1f - num;
				this.MissleAIMTimer = num;
			}
		}
		if (this.WS.MGitem == 1 && this.currTank.classID > 13)
		{
			if (this.currTank.MG != null)
			{
				this.currTank.MG.gameObject.SetActive(true);
			}
			GUI.DrawTexture(new Rect((float)(Screen.width - 40 - 42), (float)(Screen.height - 196), 32f, 32f), this.tankMGTex);
			this.r_ammo_gun = new Rect((float)(Screen.width - 40 - 128 - 15), (float)(Screen.height - 40 - 42 - 10), 32f, 32f);
			GUI.DrawTexture(this.r_ammo_gun, this.ammo_machinegun);
			this.gui_style.fontSize = 40;
			this.gui_style.normal.textColor = GUIManager.c[9];
			GUI.Label(new Rect((float)(Screen.width - 40 - 60 + 2 - 128 - 15), (float)(Screen.height - 34 + 2 - 42 - 10), 0f, 0f), this.WS.GetMGAmmoClip().ToString(), this.gui_style);
			this.gui_style.normal.textColor = GUIManager.c[8];
			GUI.Label(new Rect((float)(Screen.width - 40 - 60 - 128 - 15), (float)(Screen.height - 34 - 42 - 10), 0f, 0f), this.WS.GetMGAmmoClip().ToString(), this.gui_style);
			this.gui_style.fontSize = 20;
			this.gui_style.normal.textColor = GUIManager.c[9];
			GUI.Label(new Rect((float)(Screen.width - 40 + 2 - 128 - 15), (float)(Screen.height - 50 + 2 - 42 - 10), 0f, 0f), this.WS.GetMGAmmoBackpack().ToString(), this.gui_style);
			this.gui_style.normal.textColor = GUIManager.c[8];
			GUI.Label(new Rect((float)(Screen.width - 40 - 128 - 15), (float)(Screen.height - 50 - 42 - 10), 0f, 0f), this.WS.GetMGAmmoBackpack().ToString(), this.gui_style);
		}
		else if (this.currTank.MG != null)
		{
			this.currTank.MG.gameObject.SetActive(false);
		}
		Vector3 vector = base.transform.eulerAngles - this.cam.transform.eulerAngles;
		Vector3 eulerAngles = new Vector3(0f, 0f, 0f);
		if (this.Turret != null)
		{
			eulerAngles = this.Turret.eulerAngles;
		}
		this.DrawTankState(vector.y, eulerAngles.y);
		float num3 = this.ReloadStart + this.ReloadDuration * (float)this.currTank.reload / 100f - Time.time;
		if (num3 < 0f)
		{
			return;
		}
		float num4 = 1f - num3 / (this.ReloadDuration * (float)this.currTank.reload / 100f);
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - 66f, (float)Screen.height * 0.75f - 2f, 132f, 12f), GUIManager.tex_black);
		if (this.ReloadTex)
		{
			GUI.DrawTexture(new Rect((float)Screen.width / 2f - 64f, (float)Screen.height * 0.75f, 128f * num4, 8f), this.ReloadTex);
		}
	}

	// Token: 0x06000288 RID: 648 RVA: 0x00032E94 File Offset: 0x00031094
	private void DrawTankState(float rBy, float rTy)
	{
		Rect position = new Rect((float)(Screen.width - 156), (float)(Screen.height - 156), 156f, 156f);
		position.center = new Vector2((float)(Screen.width - 156 + 78), (float)(Screen.height - 156 + 78));
		float num = rBy;
		float num2 = rTy + num - base.transform.eulerAngles.y;
		if (num > 360f)
		{
			num -= 360f;
		}
		if (num < 0f)
		{
			num += 360f;
		}
		if (num2 > 360f)
		{
			num2 -= 360f;
		}
		if (num2 < 0f)
		{
			num2 += 360f;
		}
		GUIUtility.RotateAroundPivot(num, position.center);
		GUI.DrawTexture(position, this.BasePosition);
		GUIUtility.RotateAroundPivot(-num, position.center);
		GUIUtility.RotateAroundPivot(num2, position.center);
		GUI.DrawTexture(position, this.TurretPosition);
		GUIUtility.RotateAroundPivot(-num2, position.center);
	}

	// Token: 0x06000289 RID: 649 RVA: 0x00032F9C File Offset: 0x0003119C
	public void javelinAIM(int aimIndex)
	{
		if (aimIndex == 1)
		{
			base.StartCoroutine(this.UnderAIM());
		}
		if (aimIndex == 2)
		{
			if (this.AIMAudio == null)
			{
				this.AIMAudio = this.cam.gameObject.AddComponent<AudioSource>();
				this.AIMAudio.maxDistance = 35f;
				this.AIMAudio.spatialBlend = 1f;
			}
			base.GetComponent<Sound>().PlaySound_Stop(this.AIMAudio);
			this.underMissleAIM = true;
			this.MissleAIMTimer = 0f;
		}
		if (aimIndex == 3)
		{
			if (this.AIMAudio == null)
			{
				this.AIMAudio = this.cam.gameObject.AddComponent<AudioSource>();
				this.AIMAudio.maxDistance = 35f;
				this.AIMAudio.spatialBlend = 1f;
			}
			base.GetComponent<Sound>().PlaySound_Stop(this.AIMAudio);
			this.underMissleAIM = false;
		}
	}

	// Token: 0x0600028A RID: 650 RVA: 0x00033085 File Offset: 0x00031285
	private IEnumerator UnderAIM()
	{
		this.underAIM = true;
		yield return new WaitForSeconds(5f);
		this.underAIM = false;
		if (this.AIMAudio == null)
		{
			this.AIMAudio = this.cam.gameObject.AddComponent<AudioSource>();
			this.AIMAudio.maxDistance = 35f;
			this.AIMAudio.spatialBlend = 1f;
		}
		base.GetComponent<Sound>().PlaySound_Stop(this.AIMAudio);
		yield break;
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00033094 File Offset: 0x00031294
	private void Update()
	{
		if (Time.time > this.enableExit && !base.gameObject.GetComponent<TransportExit>().enabled)
		{
			base.gameObject.GetComponent<TransportExit>().enabled = true;
			base.gameObject.GetComponent<TransportExit>().vehicleType = CONST.VEHICLES.TANKS;
			this.distFromGround = 1.25f;
		}
		if (this.currTank == null)
		{
			this.currTank = base.gameObject.GetComponentInChildren<Tank>();
		}
		if (this.oc.zoom)
		{
			this.currTank.MG.gameObject.SetActive(false);
		}
		this.dlina = this.currTank.dlina;
		this.shirina = this.currTank.shirina;
		if (this.TurretAs == null)
		{
			this.TurretAs = this.TMPAudio.GetComponent<AudioSource>();
		}
		this.TurretAs.maxDistance = 30f;
		this.TurretAs.spatialBlend = 1f;
		if (this.m_PlayerControl == null)
		{
			this.m_PlayerControl = (PlayerControl)Object.FindObjectOfType(typeof(PlayerControl));
		}
		if (this.am == null)
		{
			this.am = (Ammo)Object.FindObjectOfType(typeof(Ammo));
			this.am.SetWeapon(201, 0, 0);
		}
		if (this.activeControl)
		{
			if (Input.GetMouseButtonDown(0) && Time.time > this.lastShotTime + this.ReloadDuration * (float)this.currTank.reload / 100f)
			{
				this.Fire();
				this.lastShotTime = Time.time;
				this.ReloadStart = Time.time;
			}
			if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				this.WS.UseVehicleModul(this.AS, CONST.VEHICLES.VEHICLE_MODUL_REPAIR_KIT);
			}
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				this.WS.UseVehicleModul(this.AS, CONST.VEHICLES.VEHICLE_MODUL_ANTI_MISSLE);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				this.WS.UseVehicleModul(this.AS, CONST.VEHICLES.VEHICLE_MODUL_SMOKE);
				base.GetComponent<Sound>().PlaySound_Stop(this.AIMAudio);
				this.underAIM = false;
			}
			if (Input.GetKeyDown(KeyCode.Q))
			{
				this.NextSnaryad();
			}
			if (this.currTank.classID > CONST.ENTS.ENT_TANK)
			{
				if (Time.time > this.MGReloadStart + this.ReloadDuration * (float)this.currTank.reload / 100f && this.isReloading)
				{
					this.WS.TankMGReload();
					this.isReloading = false;
				}
				else if (Input.GetKey(KeyCode.Space))
				{
					if (this.WS.GetMGAmmo() > 0)
					{
						if (this.WS.GetMGAmmoClip() <= 0 && !this.isReloading)
						{
							this.lastShotTime = Time.time;
							this.ReloadStart = Time.time;
							this.MGlastShotTime = Time.time;
							this.MGReloadStart = Time.time;
							base.GetComponent<Sound>().PlaySound_TankMGNoAmmo(this.AS);
							base.GetComponent<Sound>().PlaySound_TankMGReload(this.AS);
							this.isReloading = true;
						}
						else if (Time.time > this.MGlastShotTime + this.MGPause && !this.isReloading)
						{
							this.weapon_raycast(220, 90f, 45);
							this.WS.TankMGFire();
							this.MGlastShotTime = Time.time;
						}
					}
					else if (this.klik)
					{
						base.GetComponent<Sound>().PlaySound_TankMGNoAmmo(this.AS);
						this.klik = false;
						this.nextKlik = Time.time + 3f;
					}
				}
			}
			if (!this.klik && this.nextKlik < Time.time)
			{
				this.klik = true;
			}
			if (Vector3.Distance(this.RayCastBox.position, GameObject.Find("Player").transform.position) > 10f)
			{
				this.RayCastBox.position = GameObject.Find("Player").transform.position;
			}
			this.speedF = Time.deltaTime * Input.GetAxisRaw("Vertical") * 5f / (Mathf.Abs(180f * this.angX / 3.1415927f) / 8f + 1f);
			this.speedR = Time.deltaTime * Input.GetAxisRaw("Horizontal") * 30f;
			this.speedF *= (float)this.currTank.speed / 100f;
			this.speedR *= (float)this.currTank.speed / 100f;
			this.tryPreRepos();
			this.tryRayCast();
			this.tryRepos();
			this.angX = Mathf.Atan((this.getYF() - this.getYB()) / Vector3.Distance(this.CF.position, this.CB.position));
			this.angZ = Mathf.Atan((this.getYL() - this.getYR()) / Vector3.Distance(this.L3.position, this.R3.position));
			if (this.canMove && this.lastState != 0)
			{
				this.RayCastBox.rotation = Quaternion.Euler(this.RayCastBox.rotation.eulerAngles.x, this.RayCastBox.rotation.eulerAngles.y + this.speedR, this.RayCastBox.rotation.eulerAngles.z);
				this.RayCastBox.position = this.M.position;
			}
			if (this.canMove)
			{
				this.nextPos = new Vector3(this.RayCastBox.position.x, this.getMaxV(), this.RayCastBox.position.z);
				this.nextRot = Quaternion.Lerp(this.myTransform.rotation, Quaternion.Euler(this.myTransform.rotation.eulerAngles.x, this.RayCastBox.rotation.eulerAngles.y, this.myTransform.rotation.eulerAngles.z), Time.time * 0.5f).eulerAngles;
				if (this.lastPos != this.nextPos || this.lastRot != this.nextRot)
				{
					if (this.state == 0)
					{
						this.s.PlaySound_TankEnter(this.AS);
					}
					else if (this.state == 1 && this.lastState == 1)
					{
						this.state = 2;
					}
					else if (this.state == 2 && this.lastState == 1)
					{
						this.s.PlaySound_TankStart(this.AS);
						this.lastState = 2;
					}
					else if (this.state == 2 && this.lastState == 2)
					{
						this.s.PlaySound_TankMove(this.AS);
						this.s.PlaySound_Pich(this.angX * Input.GetAxisRaw("Vertical") / 5f, this.AS);
					}
					this.myTransform.position = Vector3.Lerp(this.lastPos, this.nextPos, 50f);
					this.myTransform.rotation = Quaternion.Lerp(this.myTransform.rotation, Quaternion.Euler(this.myTransform.rotation.eulerAngles.x, this.RayCastBox.rotation.eulerAngles.y, this.myTransform.rotation.eulerAngles.z), Time.time * 0.5f);
					this.myTransform.rotation = Quaternion.Lerp(this.myTransform.rotation, Quaternion.Euler(this.RayCastBox.rotation.eulerAngles.x - 180f * this.angX / 3.1415927f, this.myTransform.rotation.eulerAngles.y, this.myTransform.rotation.eulerAngles.z), Time.time * (0.01f / (1f + Mathf.Abs(180f * this.angX / 3.1415927f))));
					this.myTransform.rotation = Quaternion.Lerp(this.myTransform.rotation, Quaternion.Euler(this.myTransform.rotation.eulerAngles.x, this.myTransform.rotation.eulerAngles.y, this.RayCastBox.rotation.eulerAngles.z - 180f * this.angZ / 3.1415927f), Time.time * (0.01f / (1f + Mathf.Abs(180f * this.angZ / 3.1415927f))));
					this.lastPos = this.nextPos;
					this.lastRot = this.nextRot;
				}
				else
				{
					if (this.state == 2 && this.lastState == 2)
					{
						this.state = 1;
					}
					else if (this.state == 1 && this.lastState == 2)
					{
						this.s.PlaySound_TankStop(this.AS);
						this.lastState = 1;
					}
					else if (this.state == 1 && this.lastState == 1)
					{
						this.s.PlaySound_TankStand(this.AS);
					}
					if (!this.AS.isPlaying)
					{
						this.lastState = 1;
						this.state = 1;
					}
				}
			}
			else
			{
				if (this.state == 2)
				{
					this.s.PlaySound_TankStop(this.AS);
					this.state = 1;
					this.lastState = 1;
				}
				if (this.state == 1 && this.lastState == 1)
				{
					this.s.PlaySound_TankStand(this.AS);
				}
				this.lastState = 1;
			}
		}
		this.ray = this.cam.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f));
		int num = 1049601;
		Vector3 point;
		if (Physics.Raycast(this.ray, out this.hit, 1000f, num))
		{
			point = this.hit.point;
		}
		else
		{
			point = this.ray.GetPoint(1000f);
		}
		Vector3 vector = point - this.myTransform.position;
		float num2 = Mathf.Abs(90f - Vector3.Angle(vector, this.myTransform.right));
		Quaternion to = Quaternion.LookRotation(vector);
		to.eulerAngles = Quaternion.Euler(0f, to.eulerAngles.y - base.transform.rotation.eulerAngles.y, 0f).eulerAngles;
		this.Turret.transform.localRotation = Quaternion.RotateTowards(this.Turret.transform.localRotation, to, 40f / ((float)this.currTank.turretRotation / 100f) * Time.deltaTime);
		if (this.turretState != this.Turret.localRotation.eulerAngles && this.currTurretState == 1 && Vector3.Distance(this.turretState, this.Turret.localRotation.eulerAngles) > 2f)
		{
			this.s.PlaySound_TurretStart(this.TurretAs);
			this.currTurretState = 2;
			this.turretState = this.Turret.localRotation.eulerAngles;
		}
		else if (this.turretState != this.Turret.localRotation.eulerAngles && (this.currTurretState == 2 || this.currTurretState == 3))
		{
			this.s.PlaySound_TurretMove(this.TurretAs);
			this.currTurretState = 3;
			this.turretState = this.Turret.localRotation.eulerAngles;
		}
		else if (this.turretState == this.Turret.localRotation.eulerAngles && this.currTurretState == 3)
		{
			this.s.PlaySound_TurretStop(this.TurretAs);
			this.currTurretState = 1;
			this.turretState = this.Turret.localRotation.eulerAngles;
		}
		vector = point - this.Gun.position;
		num2 = Mathf.Abs(90f - Vector3.Angle(vector, this.Gun.up));
		this.Gun.localRotation = Quaternion.Lerp(this.Gun.localRotation, Quaternion.Euler(this.Gun.localRotation.eulerAngles.x + Mathf.Clamp(this.gunSpeed * Time.deltaTime * Mathf.Sign(Vector3.Dot(Vector3.Cross(this.Gun.forward, vector), this.Gun.right)), -num2, num2), this.Gun.localRotation.eulerAngles.y, this.Gun.localRotation.eulerAngles.z), Time.time * 0.05f);
		if (this.Gun.localRotation.eulerAngles.x > 300f && this.Gun.localRotation.eulerAngles.x < 320f)
		{
			this.Gun.localRotation = Quaternion.Euler(320f, this.Gun.localRotation.eulerAngles.y, this.Gun.localRotation.eulerAngles.z);
		}
		if (this.Gun.localRotation.eulerAngles.x < 100f && this.Gun.localRotation.eulerAngles.x > 15f)
		{
			this.Gun.localRotation = Quaternion.Euler(15f, this.Gun.localRotation.eulerAngles.y, this.Gun.localRotation.eulerAngles.z);
		}
		if (this.tik < Time.time)
		{
			if (this.cl == null)
			{
				this.cl = (Client)Object.FindObjectOfType(typeof(Client));
			}
			this.cl.send_vehicle_turret(this.myTransform.rotation.eulerAngles, this.Turret.localRotation.eulerAngles.y, this.Gun.localRotation.eulerAngles, base.gameObject.GetComponentInChildren<Tank>().uid);
			int team = this.cl.cspc.GetTeam();
			if (this.lastTeam != team)
			{
				if (team == 0)
				{
					this.ReloadTex = GUIManager.tex_blue;
				}
				else if (team == 1)
				{
					this.ReloadTex = GUIManager.tex_red;
				}
				this.lastTeam = team;
			}
			this.tik = Time.time + 0.5f;
		}
	}

	// Token: 0x0600028C RID: 652 RVA: 0x00033FF0 File Offset: 0x000321F0
	private bool hitChecker(RaycastHit hit, Ray ray, float rad)
	{
		if (this.cl == null)
		{
			this.cl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		if (hit.transform.name[0] == '(')
		{
			Vector3i? point = this.GetPoint(ray, rad);
			if (point == null)
			{
				return false;
			}
			if (this.weapon_attack_block(201, point.Value))
			{
				return true;
			}
		}
		Data component = hit.transform.gameObject.GetComponent<Data>();
		if (component == null)
		{
			return false;
		}
		this.cspm.CreateHit(this.myTransform, component.hitzone, hit.point.x, hit.point.y, hit.point.z);
		this.cspm.CreateParticle(hit.point.x, hit.point.y, hit.point.z, 1f, 0f, 0f, 1f);
		this.cl.send_damage(CONST.VEHICLES.VEHICLE_TANK_MEDIUM, component.index, component.hitzone, Time.time, base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z, hit.transform.position.x, hit.transform.position.y, hit.transform.position.z, base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z, hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
		return false;
	}

	// Token: 0x0600028D RID: 653 RVA: 0x00034208 File Offset: 0x00032408
	private void tryRayCast()
	{
		int num = 1025;
		int num2 = 1049601;
		this.L1_Down = Physics.Raycast(this.L1.position, Vector3.down, out this.L1_hit_Down, 10f, num);
		this.L2_Down = Physics.Raycast(this.L2.position, Vector3.down, out this.L2_hit_Down, 10f, num);
		this.L3_Down = Physics.Raycast(this.L3.position, Vector3.down, out this.L3_hit_Down, 10f, num);
		this.L4_Down = Physics.Raycast(this.L4.position, Vector3.down, out this.L4_hit_Down, 10f, num);
		this.L5_Down = Physics.Raycast(this.L5.position, Vector3.down, out this.L5_hit_Down, 10f, num);
		this.R1_Down = Physics.Raycast(this.R1.position, Vector3.down, out this.R1_hit_Down, 10f, num);
		this.R2_Down = Physics.Raycast(this.R2.position, Vector3.down, out this.R2_hit_Down, 10f, num);
		this.R3_Down = Physics.Raycast(this.R3.position, Vector3.down, out this.R3_hit_Down, 10f, num);
		this.R4_Down = Physics.Raycast(this.R4.position, Vector3.down, out this.R4_hit_Down, 10f, num);
		this.R5_Down = Physics.Raycast(this.R5.position, Vector3.down, out this.R5_hit_Down, 10f, num);
		this.M_Down = Physics.Raycast(this.M.position, Vector3.down, out this.M_hit_Down, 10f, num);
		this.CF_Down = Physics.Raycast(this.CF.position, Vector3.down, out this.CF_hit_Down, 10f, num);
		this.CB_Down = Physics.Raycast(this.CB.position, Vector3.down, out this.CB_hit_Down, 10f, num);
		this.L1_Right = Physics.Raycast(new Vector3(this.L1_hit_Down.point.x, this.L1_hit_Down.point.y + 0.5f, this.L1_hit_Down.point.z), this.L1.right, out this.L1_hit_Right, this.shirina, num2);
		this.R1_Back = Physics.Raycast(new Vector3(this.R1_hit_Down.point.x, this.R1_hit_Down.point.y + 0.5f, this.R1_hit_Down.point.z), -this.R1.forward, out this.R1_hit_Back, this.dlina, num2);
		this.R5_Left = Physics.Raycast(new Vector3(this.R5_hit_Down.point.x, this.R5_hit_Down.point.y + 0.5f, this.R5_hit_Down.point.z), -this.R5.right, out this.R5_hit_Left, this.shirina, num2);
		this.L5_Forward = Physics.Raycast(new Vector3(this.L5_hit_Down.point.x, this.L5_hit_Down.point.y + 0.5f, this.L5_hit_Down.point.z), this.L5.forward, out this.L5_hit_Forward, this.dlina, num2);
	}

	// Token: 0x0600028E RID: 654 RVA: 0x000345A0 File Offset: 0x000327A0
	private void tryRepos()
	{
		if (!this.M_Down || !this.L1_Down || !this.L2_Down || !this.L3_Down || !this.L4_Down || !this.L5_Down || !this.R1_Down || !this.R2_Down || !this.R3_Down || !this.R4_Down || !this.R5_Down || !this.CF_Down || !this.CB_Down)
		{
			this.canMove = false;
		}
		else
		{
			this.canMove = true;
		}
		if (this.L1_Right && this.speedF > 0f && this.hitChecker(this.L1_hit_Right, new Ray(new Vector3(this.L1_hit_Down.point.x, this.L1_hit_Down.point.y + 0.5f, this.L1_hit_Down.point.z), this.L1.right), this.shirina))
		{
			this.canMove = false;
		}
		if (this.R1_Back && this.speedF < 0f && this.hitChecker(this.R1_hit_Back, new Ray(new Vector3(this.R1_hit_Down.point.x, this.R1_hit_Down.point.y + 0.5f, this.R1_hit_Down.point.z), -this.R1.forward), this.dlina))
		{
			this.canMove = false;
		}
		if (this.R5_Left && this.speedF < 0f && this.hitChecker(this.R5_hit_Left, new Ray(new Vector3(this.R5_hit_Down.point.x, this.R5_hit_Down.point.y + 0.5f, this.R5_hit_Down.point.z), -this.R5.right), this.shirina))
		{
			this.canMove = false;
		}
		if (this.L5_Forward && this.speedF > 0f && this.hitChecker(this.L5_hit_Forward, new Ray(new Vector3(this.L5_hit_Down.point.x, this.L5_hit_Down.point.y + 0.5f, this.L5_hit_Down.point.z), this.L5.forward), this.dlina))
		{
			this.canMove = false;
		}
		if ((this.L1_Down && this.L2_Down && this.CF_Down && this.speedF > 0f && (Mathf.Abs(this.L1_hit_Down.point.y - this.L2_hit_Down.point.y) > 2f || Mathf.Abs(this.L1_hit_Down.point.y - this.CF_hit_Down.point.y) > 2f)) || (this.R1_Down && this.R2_Down && this.CF_Down && this.speedF > 0f && (Mathf.Abs(this.R1_hit_Down.point.y - this.R2_hit_Down.point.y) > 2f || Mathf.Abs(this.R1_hit_Down.point.y - this.CF_hit_Down.point.y) > 2f)) || (this.L5_Down && this.L4_Down && this.CB_Down && this.speedF < 0f && (Mathf.Abs(this.L5_hit_Down.point.y - this.L4_hit_Down.point.y) > 2f || Mathf.Abs(this.L5_hit_Down.point.y - this.CB_hit_Down.point.y) > 2f)) || (this.R5_Down && this.R4_Down && this.CB_Down && this.speedF < 0f && (Mathf.Abs(this.R5_hit_Down.point.y - this.R4_hit_Down.point.y) > 2f || Mathf.Abs(this.R5_hit_Down.point.y - this.CB_hit_Down.point.y) > 2f)))
		{
			this.canMove = false;
		}
		if (this.L1_hit_Right.collider != null && this.L1_hit_Right.collider.tag == "Tank" && this.speedF > 0f)
		{
			this.canMove = false;
		}
		if (this.R1_hit_Back.collider != null && this.R1_hit_Back.collider.tag == "Tank" && this.speedF < 0f)
		{
			this.canMove = false;
		}
		if (this.R5_hit_Left.collider != null && this.R5_hit_Left.collider.tag == "Tank" && this.speedF < 0f)
		{
			this.canMove = false;
		}
		if (this.L5_hit_Forward.collider != null && this.L5_hit_Forward.collider.tag == "Tank" && this.speedF > 0f)
		{
			this.canMove = false;
		}
		if (this.M_Down)
		{
			this.M.position = new Vector3(this.M_hit_Down.point.x, this.M_hit_Down.point.y + this.distFromGround, this.M_hit_Down.point.z);
		}
		else
		{
			this.M.localPosition = new Vector3(0f, 0f, 0f);
		}
		if (this.CF_Down)
		{
			this.CF.position = new Vector3(this.CF_hit_Down.point.x, this.CF_hit_Down.point.y + this.distFromGround, this.CF_hit_Down.point.z);
		}
		else
		{
			this.CF.localPosition = new Vector3(this.M.localPosition.x, 0f, this.M.localPosition.z + this.dlina / 2f);
		}
		if (this.CB_Down)
		{
			this.CB.position = new Vector3(this.CB_hit_Down.point.x, this.CB_hit_Down.point.y + this.distFromGround, this.CB_hit_Down.point.z);
		}
		else
		{
			this.CB.localPosition = new Vector3(this.M.localPosition.x, 0f, this.M.localPosition.z - this.dlina / 2f);
		}
		if (this.L1_Down)
		{
			this.L1.position = new Vector3(this.L1_hit_Down.point.x, this.L1_hit_Down.point.y + this.distFromGround, this.L1_hit_Down.point.z);
		}
		else
		{
			this.L1.localPosition = new Vector3(this.M.localPosition.x - this.shirina / 2f, 0f, this.M.localPosition.z + this.dlina / 2f);
		}
		if (this.L2_Down)
		{
			this.L2.position = new Vector3(this.L2_hit_Down.point.x, this.L2_hit_Down.point.y + this.distFromGround, this.L2_hit_Down.point.z);
		}
		else
		{
			this.L2.localPosition = new Vector3(this.M.localPosition.x - this.shirina / 2f, 0f, this.M.localPosition.z + this.dlina / 4f);
		}
		if (this.L3_Down)
		{
			this.L3.position = new Vector3(this.L3_hit_Down.point.x, this.L3_hit_Down.point.y + this.distFromGround, this.L3_hit_Down.point.z);
		}
		else
		{
			this.L3.localPosition = new Vector3(this.M.localPosition.x - this.shirina / 2f, 0f, this.M.localPosition.z);
		}
		if (this.L4_Down)
		{
			this.L4.position = new Vector3(this.L4_hit_Down.point.x, this.L4_hit_Down.point.y + this.distFromGround, this.L4_hit_Down.point.z);
		}
		else
		{
			this.L4.localPosition = new Vector3(this.M.localPosition.x - this.shirina / 2f, 0f, this.M.localPosition.z - this.dlina / 4f);
		}
		if (this.L5_Down)
		{
			this.L5.position = new Vector3(this.L5_hit_Down.point.x, this.L5_hit_Down.point.y + this.distFromGround, this.L5_hit_Down.point.z);
		}
		else
		{
			this.L5.localPosition = new Vector3(this.M.localPosition.x - this.shirina / 2f, 0f, this.M.localPosition.z - this.dlina / 2f);
		}
		if (this.R1_Down)
		{
			this.R1.position = new Vector3(this.R1_hit_Down.point.x, this.R1_hit_Down.point.y + this.distFromGround, this.R1_hit_Down.point.z);
		}
		else
		{
			this.R1.localPosition = new Vector3(this.M.localPosition.x + this.shirina / 2f, 0f, this.M.localPosition.z + this.dlina / 2f);
		}
		if (this.R2_Down)
		{
			this.R2.position = new Vector3(this.R2_hit_Down.point.x, this.R2_hit_Down.point.y + this.distFromGround, this.R2_hit_Down.point.z);
		}
		else
		{
			this.R2.localPosition = new Vector3(this.M.localPosition.x + this.shirina / 2f, 0f, this.M.localPosition.z + this.dlina / 4f);
		}
		if (this.R3_Down)
		{
			this.R3.position = new Vector3(this.R3_hit_Down.point.x, this.R3_hit_Down.point.y + this.distFromGround, this.R3_hit_Down.point.z);
		}
		else
		{
			this.R3.localPosition = new Vector3(this.M.localPosition.x + this.shirina / 2f, 0f, this.M.localPosition.z);
		}
		if (this.R4_Down)
		{
			this.R4.position = new Vector3(this.R4_hit_Down.point.x, this.R4_hit_Down.point.y + this.distFromGround, this.R4_hit_Down.point.z);
		}
		else
		{
			this.R4.localPosition = new Vector3(this.M.localPosition.x + this.shirina / 2f, 0f, this.M.localPosition.z - this.dlina / 4f);
		}
		if (this.R5_Down)
		{
			this.R5.position = new Vector3(this.R5_hit_Down.point.x, this.R5_hit_Down.point.y + this.distFromGround, this.R5_hit_Down.point.z);
		}
		else
		{
			this.R5.localPosition = new Vector3(this.M.localPosition.x + this.shirina / 2f, 0f, this.M.localPosition.z - this.dlina / 2f);
		}
		this.M.position = new Vector3(this.M.position.x, this.getMaxY(), this.M.position.z);
		this.L1.position = new Vector3(this.L1.position.x, this.getMaxY(), this.L1.position.z);
		this.L2.position = new Vector3(this.L2.position.x, this.getMaxY(), this.L2.position.z);
		this.L3.position = new Vector3(this.L3.position.x, this.getMaxY(), this.L3.position.z);
		this.L4.position = new Vector3(this.L4.position.x, this.getMaxY(), this.L4.position.z);
		this.L5.position = new Vector3(this.L5.position.x, this.getMaxY(), this.L5.position.z);
		this.R1.position = new Vector3(this.R1.position.x, this.getMaxY(), this.R1.position.z);
		this.R2.position = new Vector3(this.R2.position.x, this.getMaxY(), this.R2.position.z);
		this.R3.position = new Vector3(this.R3.position.x, this.getMaxY(), this.R3.position.z);
		this.R4.position = new Vector3(this.R4.position.x, this.getMaxY(), this.R4.position.z);
		this.R5.position = new Vector3(this.R5.position.x, this.getMaxY(), this.R5.position.z);
		this.CF.position = new Vector3(this.CF.position.x, this.getMaxY(), this.CF.position.z);
		this.CB.position = new Vector3(this.CB.position.x, this.getMaxY(), this.CB.position.z);
		float num = Mathf.Sqrt(Mathf.Pow(this.currTank.dlina, 2f) - Mathf.Pow(Mathf.Abs(this.getYF() - this.getYB()), 2f));
		float num2 = Mathf.Sqrt(Mathf.Pow(this.currTank.shirina, 2f) - Mathf.Pow(Mathf.Abs(this.getYL() - this.getYR()), 2f));
		if (num > 3.6f && Mathf.Abs(this.dlina - num) > 0.4f)
		{
			this.dlina = Mathf.Sqrt(Mathf.Pow(this.currTank.dlina, 2f) - Mathf.Pow(Mathf.Abs(this.getYF() - this.getYB()), 2f));
		}
		if (num2 > 3.4f && Mathf.Abs(this.shirina - num2) > 0.4f)
		{
			this.shirina = Mathf.Sqrt(Mathf.Pow(this.currTank.shirina, 2f) - Mathf.Pow(Mathf.Abs(this.getYL() - this.getYR()), 2f));
		}
	}

	// Token: 0x0600028F RID: 655 RVA: 0x000356D8 File Offset: 0x000338D8
	private void tryPreRepos()
	{
		this.M.position = new Vector3(this.RayCastBox.forward.x * this.speedF + this.RayCastBox.position.x, this.RayCastBox.position.y, this.RayCastBox.forward.z * this.speedF + this.RayCastBox.position.z);
		this.L1.localPosition = new Vector3(this.M.localPosition.x - this.shirina / 2f, 0f, this.M.localPosition.z + this.dlina / 2f);
		this.L2.localPosition = new Vector3(this.M.localPosition.x - this.shirina / 2f, 0f, this.M.localPosition.z + this.dlina / 4f);
		this.L3.localPosition = new Vector3(this.M.localPosition.x - this.shirina / 2f, 0f, this.M.localPosition.z);
		this.L4.localPosition = new Vector3(this.M.localPosition.x - this.shirina / 2f, 0f, this.M.localPosition.z - this.dlina / 4f);
		this.L5.localPosition = new Vector3(this.M.localPosition.x - this.shirina / 2f, 0f, this.M.localPosition.z - this.dlina / 2f);
		this.R1.localPosition = new Vector3(this.M.localPosition.x + this.shirina / 2f, 0f, this.M.localPosition.z + this.dlina / 2f);
		this.R2.localPosition = new Vector3(this.M.localPosition.x + this.shirina / 2f, 0f, this.M.localPosition.z + this.dlina / 4f);
		this.R3.localPosition = new Vector3(this.M.localPosition.x + this.shirina / 2f, 0f, this.M.localPosition.z);
		this.R4.localPosition = new Vector3(this.M.localPosition.x + this.shirina / 2f, 0f, this.M.localPosition.z - this.dlina / 4f);
		this.R5.localPosition = new Vector3(this.M.localPosition.x + this.shirina / 2f, 0f, this.M.localPosition.z - this.dlina / 2f);
		this.CF.localPosition = new Vector3(this.M.localPosition.x, 0f, this.M.localPosition.z + this.dlina / 2f);
		this.CB.localPosition = new Vector3(this.M.localPosition.x, 0f, this.M.localPosition.z - this.dlina / 2f);
	}

	// Token: 0x06000290 RID: 656 RVA: 0x00035AD8 File Offset: 0x00033CD8
	private float getMaxY()
	{
		return Mathf.Max(Mathf.Max(Mathf.Max(Mathf.Max(this.L5.position.y, this.R5.position.y), Mathf.Max(this.M.position.y, this.M.position.y)), Mathf.Max(this.CF.position.y, this.CB.position.y)), Mathf.Max(Mathf.Max(Mathf.Max(this.L1.position.y, this.R1.position.y), Mathf.Max(this.L2.position.y, this.R2.position.y)), Mathf.Max(Mathf.Max(this.L3.position.y, this.R3.position.y), Mathf.Max(this.L4.position.y, this.R4.position.y))));
	}

	// Token: 0x06000291 RID: 657 RVA: 0x00035C06 File Offset: 0x00033E06
	private float getYF()
	{
		return (this.L1_hit_Down.point.y + this.R1_hit_Down.point.y + this.CF_hit_Down.point.y) / 3f;
	}

	// Token: 0x06000292 RID: 658 RVA: 0x00035C40 File Offset: 0x00033E40
	private float getYB()
	{
		return (this.L5_hit_Down.point.y + this.R5_hit_Down.point.y + this.CB_hit_Down.point.y) / 3f;
	}

	// Token: 0x06000293 RID: 659 RVA: 0x00035C7C File Offset: 0x00033E7C
	private float getYL()
	{
		return (this.L1_hit_Down.point.y + this.L2_hit_Down.point.y + this.L3_hit_Down.point.y + this.L4_hit_Down.point.y + this.L5_hit_Down.point.y) / 5f;
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00035CE4 File Offset: 0x00033EE4
	private float getYR()
	{
		return (this.R1_hit_Down.point.y + this.R2_hit_Down.point.y + this.R3_hit_Down.point.y + this.R4_hit_Down.point.y + this.R5_hit_Down.point.y) / 5f;
	}

	// Token: 0x06000295 RID: 661 RVA: 0x00035D4C File Offset: 0x00033F4C
	private float getMaxV()
	{
		float num = Mathf.Max((Mathf.Max(this.L1_hit_Down.point.y, this.R5_hit_Down.point.y) - Mathf.Min(this.L1_hit_Down.point.y, this.R5_hit_Down.point.y)) / 2f + Mathf.Min(this.L1_hit_Down.point.y, this.R5_hit_Down.point.y), (Mathf.Max(this.R1_hit_Down.point.y, this.L5_hit_Down.point.y) - Mathf.Min(this.R1_hit_Down.point.y, this.L5_hit_Down.point.y)) / 2f + Mathf.Min(this.R1_hit_Down.point.y, this.L5_hit_Down.point.y));
		float num2 = Mathf.Max((Mathf.Max(this.L2_hit_Down.point.y, this.R4_hit_Down.point.y) - Mathf.Min(this.L2_hit_Down.point.y, this.R4_hit_Down.point.y)) / 2f + Mathf.Min(this.L2_hit_Down.point.y, this.R4_hit_Down.point.y), (Mathf.Max(this.R2_hit_Down.point.y, this.L4_hit_Down.point.y) - Mathf.Min(this.R2_hit_Down.point.y, this.L4_hit_Down.point.y)) / 2f + Mathf.Min(this.R2_hit_Down.point.y, this.L4_hit_Down.point.y));
		float num3 = Mathf.Max((Mathf.Max(this.L3_hit_Down.point.y, this.R3_hit_Down.point.y) - Mathf.Min(this.L3_hit_Down.point.y, this.R3_hit_Down.point.y)) / 2f + Mathf.Min(this.L3_hit_Down.point.y, this.R3_hit_Down.point.y), (Mathf.Max(this.R3_hit_Down.point.y, this.L3_hit_Down.point.y) - Mathf.Min(this.R3_hit_Down.point.y, this.L3_hit_Down.point.y)) / 2f + Mathf.Min(this.R3_hit_Down.point.y, this.L3_hit_Down.point.y));
		return (num + num2 + num3) / 3f;
	}

	// Token: 0x06000296 RID: 662 RVA: 0x00036040 File Offset: 0x00034240
	private void Fire()
	{
		if (this.WS.TankFire(this.currSnaraydId[this.currSnaraydIndex], this.FP))
		{
			base.GetComponent<Sound>().PlaySound_WeaponTank(this.cam.gameObject.GetComponent<AudioSource>());
			this.oc.shakeAmount = 0.15f;
			this.oc.shake = 0.3f;
		}
	}

	// Token: 0x06000297 RID: 663 RVA: 0x000360A8 File Offset: 0x000342A8
	private Vector3i? GetPoint(Ray ray, float radius)
	{
		Vector3? vector = MapRayIntersection.Intersection(this.csmap, ray, radius);
		if (vector != null)
		{
			Vector3 vector2 = vector.Value + ray.direction * 0.01f;
			int x = Mathf.RoundToInt(vector2.x);
			int y = Mathf.RoundToInt(vector2.y);
			int z = Mathf.RoundToInt(vector2.z);
			return new Vector3i?(new Vector3i(x, y, z));
		}
		return null;
	}

	// Token: 0x06000298 RID: 664 RVA: 0x00036124 File Offset: 0x00034324
	private bool weapon_attack_block(int wid, Vector3i pointvalue)
	{
		if (this.cl == null)
		{
			this.cl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		bool result = true;
		if (wid != CONST.VEHICLES.VEHICLE_MODUL_TANK_MG)
		{
			for (int i = 2; i >= 0; i--)
			{
				Vector3i vector3i = new Vector3i(pointvalue.x, pointvalue.y + i, pointvalue.z);
				BlockData block = this.csmap.GetBlock(vector3i);
				if (block.block != null && (!(block.block.GetName() == "Grass") || !(block.block.GetName() == "Dirt") || !(block.block.GetName() == "Snow") || !(block.block.GetName() == "Stoneend")))
				{
					if (block.block.GetName() == "Leaf")
					{
						this.oc.shakeAmount = 0f;
					}
					else
					{
						this.oc.shakeAmount = 0.17f;
						result = false;
					}
					if (block.block != null && i == 1)
					{
						if (block.block.GetName() == "Leaf")
						{
							this.s.PlaySound_Block("Leaf");
							this.cspm.CreateParticle((float)pointvalue.x, (float)(pointvalue.y + 1), (float)pointvalue.z, 0.75f, 0.3f, 0.2f, 255f);
						}
						else
						{
							if (block.block.GetName() != "Grass" && block.block.GetName() != "Dirt" && block.block.GetName() != "Snow" && block.block.GetName() != "Stoneend")
							{
								this.s.PlaySound_Block("Wood");
							}
							this.cspm.CreateParticle((float)pointvalue.x, (float)(pointvalue.y + 1), (float)pointvalue.z, 0.75f, 0.3f, 0.2f, 255f);
						}
					}
					foreach (Vector3i vector3i2 in this.dirtyBlocks)
					{
						if (this.csmap.GetBlock(vector3i2).block == null)
						{
							this.deletedDirtyBlocks.Add(vector3i2);
						}
					}
					foreach (Vector3i item in this.deletedDirtyBlocks)
					{
						this.dirtyBlocks.Remove(item);
					}
					this.deletedDirtyBlocks.Clear();
					if (!this.dirtyBlocks.Contains(vector3i))
					{
						this.cl.send_attackblock(vector3i.x, vector3i.y, vector3i.z, wid, Time.time, 0f, 0f, 0f, 0f, 0f, 0f);
						this.dirtyBlocks.Add(vector3i);
					}
				}
			}
		}
		else
		{
			Vector3i vector3i3 = new Vector3i(pointvalue.x, pointvalue.y, pointvalue.z);
			BlockData block2 = this.csmap.GetBlock(vector3i3);
			if (block2.block == null)
			{
				return true;
			}
			if (block2.block.GetName() == "Grass" && block2.block.GetName() == "Dirt" && block2.block.GetName() == "Snow" && block2.block.GetName() == "Stoneend")
			{
				return true;
			}
			if (block2.block != null)
			{
				if (block2.block.GetName() == "Leaf")
				{
					this.s.PlaySound_Block("Leaf");
					this.cspm.CreateParticle((float)pointvalue.x, (float)pointvalue.y, (float)pointvalue.z, 0.75f, 0.3f, 0.2f, 255f);
				}
				else
				{
					if (block2.block.GetName() != "Grass" && block2.block.GetName() != "Dirt" && block2.block.GetName() != "Snow" && block2.block.GetName() != "Stoneend")
					{
						this.s.PlaySound_Block("Wood");
					}
					this.cspm.CreateParticle((float)pointvalue.x, (float)pointvalue.y, (float)pointvalue.z, 0.75f, 0.3f, 0.2f, 255f);
				}
			}
			this.cl.send_attackblock(vector3i3.x, vector3i3.y, vector3i3.z, wid, Time.time, 0f, 0f, 0f, 0f, 0f, 0f);
		}
		return result;
	}

	// Token: 0x06000299 RID: 665 RVA: 0x00036660 File Offset: 0x00034860
	private void NextSnaryad()
	{
		if (this.currSnaraydIndex == 0)
		{
			if (this.WS.GetZBK18M() > 0)
			{
				this.currSnaraydIndex = 1;
				return;
			}
			if (this.WS.GetZOF26() > 0)
			{
				this.currSnaraydIndex = 2;
				return;
			}
			this.currSnaraydIndex = 0;
			return;
		}
		else if (this.currSnaraydIndex == 1)
		{
			if (this.WS.GetZOF26() > 0)
			{
				this.currSnaraydIndex = 2;
				return;
			}
			if (this.WS.GetSnaryad() > 0)
			{
				this.currSnaraydIndex = 0;
				return;
			}
			if (this.WS.GetZBK18M() > 0)
			{
				this.currSnaraydIndex = 1;
				return;
			}
			this.currSnaraydIndex = 0;
			return;
		}
		else
		{
			if (this.currSnaraydIndex != 2)
			{
				return;
			}
			if (this.WS.GetSnaryad() > 0)
			{
				this.currSnaraydIndex = 0;
				return;
			}
			if (this.WS.GetZBK18M() > 0)
			{
				this.currSnaraydIndex = 1;
				return;
			}
			if (this.WS.GetZOF26() > 0)
			{
				this.currSnaraydIndex = 2;
				return;
			}
			this.currSnaraydIndex = 0;
			return;
		}
	}

	// Token: 0x0600029A RID: 666 RVA: 0x00036750 File Offset: 0x00034950
	private void weapon_raycast(int wid, float dist, int blockdist)
	{
		if (this.cl == null)
		{
			this.cl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		base.GetComponent<Sound>().PlaySound_WeaponMGTank(this.AS);
		if (!this.oc.zoom)
		{
			this.currTank.MGFlash.gameObject.SetActive(true);
			this.currTank.GetComponentInChildren<Tank>().FlashTime = Time.time + 0.05f;
		}
		RaycastHit raycastHit;
		if (!Physics.Raycast(this.Gun.position, this.Gun.TransformDirection(Vector3.forward), out raycastHit))
		{
			this.cl.send_milkattack(wid, Time.time);
			return;
		}
		RaycastHit raycastHit2;
		if (Physics.Linecast(raycastHit.point, this.Gun.position, out raycastHit2, this.layerMask))
		{
			this.cl.send_milkattack(wid, Time.time);
			return;
		}
		if (raycastHit.transform.name[0] == '(')
		{
			Vector3i? cursor = this.GetCursor(true, blockdist);
			if (cursor != null)
			{
				this.weapon_attack_block(wid, cursor.Value);
			}
			return;
		}
		Data component = raycastHit.transform.gameObject.GetComponent<Data>();
		if (component == null)
		{
			this.cl.send_milkattack(wid, Time.time);
			return;
		}
		if (this.GetCursor(true, (int)raycastHit.distance) != null)
		{
			return;
		}
		this.cspm.CreateHit(this.myTransform, component.hitzone, raycastHit.point.x, raycastHit.point.y, raycastHit.point.z);
		this.cspm.CreateParticle(raycastHit.point.x, raycastHit.point.y, raycastHit.point.z, 1f, 0f, 0f, 1f);
		this.cl.send_damage(wid, component.index, component.hitzone, Time.time, base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z, raycastHit.transform.position.x, raycastHit.transform.position.y, raycastHit.transform.position.z, base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z, raycastHit.transform.position.x, raycastHit.transform.position.y, raycastHit.transform.position.z);
	}

	// Token: 0x0600029B RID: 667 RVA: 0x00036A3C File Offset: 0x00034C3C
	private Vector3i? GetCursor(bool inside, int radius)
	{
		Ray ray = new Ray(this.Gun.position, this.Gun.TransformDirection(Vector3.forward));
		Vector3? vector = MapRayIntersection.Intersection(this.csmap, ray, (float)radius);
		if (vector != null)
		{
			Vector3 vector2 = vector.Value;
			if (inside)
			{
				vector2 += ray.direction * 0.01f;
			}
			if (!inside)
			{
				vector2 -= ray.direction * 0.01f;
			}
			int x = Mathf.RoundToInt(vector2.x);
			int y = Mathf.RoundToInt(vector2.y);
			int z = Mathf.RoundToInt(vector2.z);
			return new Vector3i?(new Vector3i(x, y, z));
		}
		return null;
	}

	// Token: 0x0400042B RID: 1067
	public Transform RayCastBox;

	// Token: 0x0400042C RID: 1068
	public Transform M;

	// Token: 0x0400042D RID: 1069
	public Transform L1;

	// Token: 0x0400042E RID: 1070
	public Transform L2;

	// Token: 0x0400042F RID: 1071
	public Transform L3;

	// Token: 0x04000430 RID: 1072
	public Transform L4;

	// Token: 0x04000431 RID: 1073
	public Transform L5;

	// Token: 0x04000432 RID: 1074
	public Transform R1;

	// Token: 0x04000433 RID: 1075
	public Transform R2;

	// Token: 0x04000434 RID: 1076
	public Transform R3;

	// Token: 0x04000435 RID: 1077
	public Transform R4;

	// Token: 0x04000436 RID: 1078
	public Transform R5;

	// Token: 0x04000437 RID: 1079
	public Transform CF;

	// Token: 0x04000438 RID: 1080
	public Transform CB;

	// Token: 0x04000439 RID: 1081
	private Transform myTransform;

	// Token: 0x0400043A RID: 1082
	public float distFromGround = 1.45f;

	// Token: 0x0400043B RID: 1083
	private int state;

	// Token: 0x0400043C RID: 1084
	private int lastState;

	// Token: 0x0400043D RID: 1085
	private float angXKoef;

	// Token: 0x0400043E RID: 1086
	private float angZKoef;

	// Token: 0x0400043F RID: 1087
	private float angX;

	// Token: 0x04000440 RID: 1088
	private float angZ;

	// Token: 0x04000441 RID: 1089
	private Vector3 lastPos;

	// Token: 0x04000442 RID: 1090
	private Vector3 nextPos;

	// Token: 0x04000443 RID: 1091
	private Vector3 lastRot;

	// Token: 0x04000444 RID: 1092
	private Vector3 nextRot;

	// Token: 0x04000445 RID: 1093
	private RaycastHit M_hit_Down;

	// Token: 0x04000446 RID: 1094
	private RaycastHit L1_hit_Up;

	// Token: 0x04000447 RID: 1095
	private RaycastHit L1_hit_Down;

	// Token: 0x04000448 RID: 1096
	private RaycastHit L1_hit_Left;

	// Token: 0x04000449 RID: 1097
	private RaycastHit L1_hit_Right;

	// Token: 0x0400044A RID: 1098
	private RaycastHit L1_hit_Forward;

	// Token: 0x0400044B RID: 1099
	private RaycastHit L1_hit_Back;

	// Token: 0x0400044C RID: 1100
	private RaycastHit L2_hit_Up;

	// Token: 0x0400044D RID: 1101
	private RaycastHit L2_hit_Down;

	// Token: 0x0400044E RID: 1102
	private RaycastHit L2_hit_Left;

	// Token: 0x0400044F RID: 1103
	private RaycastHit L2_hit_Right;

	// Token: 0x04000450 RID: 1104
	private RaycastHit L2_hit_Forward;

	// Token: 0x04000451 RID: 1105
	private RaycastHit L2_hit_Back;

	// Token: 0x04000452 RID: 1106
	private RaycastHit L3_hit_Up;

	// Token: 0x04000453 RID: 1107
	private RaycastHit L3_hit_Down;

	// Token: 0x04000454 RID: 1108
	private RaycastHit L3_hit_Left;

	// Token: 0x04000455 RID: 1109
	private RaycastHit L3_hit_Right;

	// Token: 0x04000456 RID: 1110
	private RaycastHit L3_hit_Forward;

	// Token: 0x04000457 RID: 1111
	private RaycastHit L3_hit_Back;

	// Token: 0x04000458 RID: 1112
	private RaycastHit L4_hit_Up;

	// Token: 0x04000459 RID: 1113
	private RaycastHit L4_hit_Down;

	// Token: 0x0400045A RID: 1114
	private RaycastHit L4_hit_Left;

	// Token: 0x0400045B RID: 1115
	private RaycastHit L4_hit_Right;

	// Token: 0x0400045C RID: 1116
	private RaycastHit L4_hit_Forward;

	// Token: 0x0400045D RID: 1117
	private RaycastHit L4_hit_Back;

	// Token: 0x0400045E RID: 1118
	private RaycastHit L5_hit_Up;

	// Token: 0x0400045F RID: 1119
	private RaycastHit L5_hit_Down;

	// Token: 0x04000460 RID: 1120
	private RaycastHit L5_hit_Left;

	// Token: 0x04000461 RID: 1121
	private RaycastHit L5_hit_Right;

	// Token: 0x04000462 RID: 1122
	private RaycastHit L5_hit_Forward;

	// Token: 0x04000463 RID: 1123
	private RaycastHit L5_hit_Back;

	// Token: 0x04000464 RID: 1124
	private RaycastHit R1_hit_Up;

	// Token: 0x04000465 RID: 1125
	private RaycastHit R1_hit_Down;

	// Token: 0x04000466 RID: 1126
	private RaycastHit R1_hit_Left;

	// Token: 0x04000467 RID: 1127
	private RaycastHit R1_hit_Forward;

	// Token: 0x04000468 RID: 1128
	private RaycastHit R1_hit_Back;

	// Token: 0x04000469 RID: 1129
	private RaycastHit R2_hit_Up;

	// Token: 0x0400046A RID: 1130
	private RaycastHit R2_hit_Down;

	// Token: 0x0400046B RID: 1131
	private RaycastHit R2_hit_Left;

	// Token: 0x0400046C RID: 1132
	private RaycastHit R2_hit_Right;

	// Token: 0x0400046D RID: 1133
	private RaycastHit R2_hit_Forward;

	// Token: 0x0400046E RID: 1134
	private RaycastHit R2_hit_Back;

	// Token: 0x0400046F RID: 1135
	private RaycastHit R3_hit_Up;

	// Token: 0x04000470 RID: 1136
	private RaycastHit R3_hit_Down;

	// Token: 0x04000471 RID: 1137
	private RaycastHit R3_hit_Left;

	// Token: 0x04000472 RID: 1138
	private RaycastHit R3_hit_Right;

	// Token: 0x04000473 RID: 1139
	private RaycastHit R3_hit_Forward;

	// Token: 0x04000474 RID: 1140
	private RaycastHit R3_hit_Back;

	// Token: 0x04000475 RID: 1141
	private RaycastHit R4_hit_Up;

	// Token: 0x04000476 RID: 1142
	private RaycastHit R4_hit_Down;

	// Token: 0x04000477 RID: 1143
	private RaycastHit R4_hit_Left;

	// Token: 0x04000478 RID: 1144
	private RaycastHit R4_hit_Right;

	// Token: 0x04000479 RID: 1145
	private RaycastHit R4_hit_Forward;

	// Token: 0x0400047A RID: 1146
	private RaycastHit R4_hit_Back;

	// Token: 0x0400047B RID: 1147
	private RaycastHit R5_hit_Up;

	// Token: 0x0400047C RID: 1148
	private RaycastHit R5_hit_Down;

	// Token: 0x0400047D RID: 1149
	private RaycastHit R5_hit_Left;

	// Token: 0x0400047E RID: 1150
	private RaycastHit R5_hit_Right;

	// Token: 0x0400047F RID: 1151
	private RaycastHit R5_hit_Forward;

	// Token: 0x04000480 RID: 1152
	private RaycastHit R5_hit_Back;

	// Token: 0x04000481 RID: 1153
	private RaycastHit CF_hit_Down;

	// Token: 0x04000482 RID: 1154
	private RaycastHit CB_hit_Down;

	// Token: 0x04000483 RID: 1155
	private bool M_Down;

	// Token: 0x04000484 RID: 1156
	private bool L1_Up;

	// Token: 0x04000485 RID: 1157
	private bool L1_Down;

	// Token: 0x04000486 RID: 1158
	private bool L1_Left;

	// Token: 0x04000487 RID: 1159
	private bool L1_Right;

	// Token: 0x04000488 RID: 1160
	private bool L1_Forward;

	// Token: 0x04000489 RID: 1161
	private bool L1_Back;

	// Token: 0x0400048A RID: 1162
	private bool L2_Up;

	// Token: 0x0400048B RID: 1163
	private bool L2_Down;

	// Token: 0x0400048C RID: 1164
	private bool L2_Left;

	// Token: 0x0400048D RID: 1165
	private bool L2_Right;

	// Token: 0x0400048E RID: 1166
	private bool L2_Forward;

	// Token: 0x0400048F RID: 1167
	private bool L2_Back;

	// Token: 0x04000490 RID: 1168
	private bool L3_Up;

	// Token: 0x04000491 RID: 1169
	private bool L3_Down;

	// Token: 0x04000492 RID: 1170
	private bool L3_Left;

	// Token: 0x04000493 RID: 1171
	private bool L3_Right;

	// Token: 0x04000494 RID: 1172
	private bool L3_Forward;

	// Token: 0x04000495 RID: 1173
	private bool L3_Back;

	// Token: 0x04000496 RID: 1174
	private bool L4_Up;

	// Token: 0x04000497 RID: 1175
	private bool L4_Down;

	// Token: 0x04000498 RID: 1176
	private bool L4_Left;

	// Token: 0x04000499 RID: 1177
	private bool L4_Right;

	// Token: 0x0400049A RID: 1178
	private bool L4_Forward;

	// Token: 0x0400049B RID: 1179
	private bool L4_Back;

	// Token: 0x0400049C RID: 1180
	private bool L5_Up;

	// Token: 0x0400049D RID: 1181
	private bool L5_Down;

	// Token: 0x0400049E RID: 1182
	private bool L5_Left;

	// Token: 0x0400049F RID: 1183
	private bool L5_Right;

	// Token: 0x040004A0 RID: 1184
	private bool L5_Forward;

	// Token: 0x040004A1 RID: 1185
	private bool L5_Back;

	// Token: 0x040004A2 RID: 1186
	private bool R1_Up;

	// Token: 0x040004A3 RID: 1187
	private bool R1_Down;

	// Token: 0x040004A4 RID: 1188
	private bool R1_Left;

	// Token: 0x040004A5 RID: 1189
	private bool R1_Forward;

	// Token: 0x040004A6 RID: 1190
	private bool R1_Back;

	// Token: 0x040004A7 RID: 1191
	private bool R2_Up;

	// Token: 0x040004A8 RID: 1192
	private bool R2_Down;

	// Token: 0x040004A9 RID: 1193
	private bool R2_Left;

	// Token: 0x040004AA RID: 1194
	private bool R2_Right;

	// Token: 0x040004AB RID: 1195
	private bool R2_Forward;

	// Token: 0x040004AC RID: 1196
	private bool R2_Back;

	// Token: 0x040004AD RID: 1197
	private bool R3_Up;

	// Token: 0x040004AE RID: 1198
	private bool R3_Down;

	// Token: 0x040004AF RID: 1199
	private bool R3_Left;

	// Token: 0x040004B0 RID: 1200
	private bool R3_Right;

	// Token: 0x040004B1 RID: 1201
	private bool R3_Forward;

	// Token: 0x040004B2 RID: 1202
	private bool R3_Back;

	// Token: 0x040004B3 RID: 1203
	private bool R4_Up;

	// Token: 0x040004B4 RID: 1204
	private bool R4_Down;

	// Token: 0x040004B5 RID: 1205
	private bool R4_Left;

	// Token: 0x040004B6 RID: 1206
	private bool R4_Right;

	// Token: 0x040004B7 RID: 1207
	private bool R4_Forward;

	// Token: 0x040004B8 RID: 1208
	private bool R4_Back;

	// Token: 0x040004B9 RID: 1209
	private bool R5_Up;

	// Token: 0x040004BA RID: 1210
	private bool R5_Down;

	// Token: 0x040004BB RID: 1211
	private bool R5_Left;

	// Token: 0x040004BC RID: 1212
	private bool R5_Right;

	// Token: 0x040004BD RID: 1213
	private bool R5_Forward;

	// Token: 0x040004BE RID: 1214
	private bool R5_Back;

	// Token: 0x040004BF RID: 1215
	private bool CF_Down;

	// Token: 0x040004C0 RID: 1216
	private bool CB_Down;

	// Token: 0x040004C1 RID: 1217
	private bool canMove = true;

	// Token: 0x040004C2 RID: 1218
	private float dlina = 4.4f;

	// Token: 0x040004C3 RID: 1219
	private float shirina = 4.5f;

	// Token: 0x040004C4 RID: 1220
	public float rotationSpeed = 50f;

	// Token: 0x040004C5 RID: 1221
	public float gunSpeed = 30f;

	// Token: 0x040004C6 RID: 1222
	public Transform Turret;

	// Token: 0x040004C7 RID: 1223
	public Transform Gun;

	// Token: 0x040004C8 RID: 1224
	public Camera cam;

	// Token: 0x040004C9 RID: 1225
	private Ray ray;

	// Token: 0x040004CA RID: 1226
	private RaycastHit hit;

	// Token: 0x040004CB RID: 1227
	private float speedF;

	// Token: 0x040004CC RID: 1228
	private float speedR;

	// Token: 0x040004CD RID: 1229
	public Transform FP;

	// Token: 0x040004CE RID: 1230
	private Client cl;

	// Token: 0x040004CF RID: 1231
	private float lastTurretAng;

	// Token: 0x040004D0 RID: 1232
	public bool activeControl = true;

	// Token: 0x040004D1 RID: 1233
	private Map csmap;

	// Token: 0x040004D2 RID: 1234
	private ParticleManager cspm;

	// Token: 0x040004D3 RID: 1235
	private Sound s;

	// Token: 0x040004D4 RID: 1236
	private AudioSource AS;

	// Token: 0x040004D5 RID: 1237
	protected vp_FPCamera m_FPCamera;

	// Token: 0x040004D6 RID: 1238
	private float lastShotTime;

	// Token: 0x040004D7 RID: 1239
	private Texture[] snaryadTexFull = new Texture[3];

	// Token: 0x040004D8 RID: 1240
	private Texture[] snaryadTexEmpty = new Texture[3];

	// Token: 0x040004D9 RID: 1241
	private Texture repairKitTex;

	// Token: 0x040004DA RID: 1242
	private Texture moduleFlash;

	// Token: 0x040004DB RID: 1243
	private Texture moduleSmoke;

	// Token: 0x040004DC RID: 1244
	private Texture tankMGTex;

	// Token: 0x040004DD RID: 1245
	private Texture ammo_machinegun;

	// Token: 0x040004DE RID: 1246
	private Texture indicatorAIM;

	// Token: 0x040004DF RID: 1247
	private bool zoom;

	// Token: 0x040004E0 RID: 1248
	private Rect r_ammo_gun;

	// Token: 0x040004E1 RID: 1249
	public vp_FPPlayerEventHandler Player;

	// Token: 0x040004E2 RID: 1250
	private Ammo am;

	// Token: 0x040004E3 RID: 1251
	private OrbitCam oc;

	// Token: 0x040004E4 RID: 1252
	private Texture2D BasePosition;

	// Token: 0x040004E5 RID: 1253
	private Texture2D TurretPosition;

	// Token: 0x040004E6 RID: 1254
	public float ReloadDuration = 3.5f;

	// Token: 0x040004E7 RID: 1255
	private float ReloadStart;

	// Token: 0x040004E8 RID: 1256
	private Texture2D ReloadTex;

	// Token: 0x040004E9 RID: 1257
	protected PlayerControl m_PlayerControl;

	// Token: 0x040004EA RID: 1258
	private int currTurretState = 1;

	// Token: 0x040004EB RID: 1259
	private Vector3 turretState = Vector3.zero;

	// Token: 0x040004EC RID: 1260
	private AudioSource TurretAs;

	// Token: 0x040004ED RID: 1261
	public GameObject TMPAudio;

	// Token: 0x040004EE RID: 1262
	public float enableExit;

	// Token: 0x040004EF RID: 1263
	private List<Vector3i> dirtyBlocks = new List<Vector3i>();

	// Token: 0x040004F0 RID: 1264
	private List<Vector3i> deletedDirtyBlocks = new List<Vector3i>();

	// Token: 0x040004F1 RID: 1265
	private WeaponSystem WS;

	// Token: 0x040004F2 RID: 1266
	private int[] currSnaraydId = new int[3];

	// Token: 0x040004F3 RID: 1267
	private int currSnaraydIndex;

	// Token: 0x040004F4 RID: 1268
	private float tik;

	// Token: 0x040004F5 RID: 1269
	private int lastTeam = -1;

	// Token: 0x040004F6 RID: 1270
	public Tank currTank;

	// Token: 0x040004F7 RID: 1271
	public GUIStyle gui_style;

	// Token: 0x040004F8 RID: 1272
	private float MGPause = 0.15f;

	// Token: 0x040004F9 RID: 1273
	private float MGlastShotTime;

	// Token: 0x040004FA RID: 1274
	private float MGReloadStart;

	// Token: 0x040004FB RID: 1275
	public Transform missle;

	// Token: 0x040004FC RID: 1276
	private bool klik = true;

	// Token: 0x040004FD RID: 1277
	private float nextKlik;

	// Token: 0x040004FE RID: 1278
	private bool isReloading;

	// Token: 0x040004FF RID: 1279
	private int layerMask = 1025;

	// Token: 0x04000500 RID: 1280
	private AudioSource AIMAudio;

	// Token: 0x04000501 RID: 1281
	private float MissleAIMTime;

	// Token: 0x04000502 RID: 1282
	private float MissleAIMTimer;

	// Token: 0x04000503 RID: 1283
	private bool underAIM;

	// Token: 0x04000504 RID: 1284
	private bool underMissleAIM;
}
