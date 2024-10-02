using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class CarController : MonoBehaviour
{
	// Token: 0x0600025C RID: 604 RVA: 0x0002C974 File Offset: 0x0002AB74
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
		this.moduleRepairKitTex = (Resources.Load("GUI/repair_kit") as Texture);
		this.MGTex = (Resources.Load("GUI/tankMG" + Lang.current.ToString()) as Texture);
		this.ammo_machinegun = (Resources.Load("GUI/ammo_mp5") as Texture);
		this.PlayerPositions = new Rect[3];
		this.BaseText = (Resources.Load("GUI/humvee_indicator") as Texture2D);
		this.PlayerIndicator = (Resources.Load("GUI/player_indicator") as Texture2D);
		this.indicatorAIM = (Resources.Load("GUI/target") as Texture);
		this.turretState = this.Turret.localRotation.eulerAngles;
		this.tik = Time.time;
		this.gui_style = new GUIStyle();
		this.gui_style.font = FontManager.font[0];
		this.gui_style.fontSize = 24;
		this.gui_style.normal.textColor = new Color(1f, 1f, 1f, 1f);
		this.gui_style.alignment = TextAnchor.MiddleLeft;
	}

	// Token: 0x0600025D RID: 605 RVA: 0x0002CBEF File Offset: 0x0002ADEF
	private void OnEnable()
	{
		this.myTransform = base.transform;
		this.MissleAIMTime = 0f;
		this.MissleAIMTimer = 0f;
		this.underAIM = false;
		this.underMissleAIM = false;
		this.missle = null;
	}

	// Token: 0x0600025E RID: 606 RVA: 0x0002CC28 File Offset: 0x0002AE28
	private void OnGUI()
	{
		if (this.currCar == null)
		{
			this.currCar = base.gameObject.GetComponentInChildren<Car>();
		}
		if (this.currCar == null)
		{
			this.currCar = base.gameObject.GetComponentInParent<Car>();
		}
		if (this.currCar == null)
		{
			return;
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
		if (this.myPosition == CONST.VEHICLES.POSITION_JEEP_GUNNER)
		{
			this.r_ammo_gun = new Rect((float)(Screen.width - 40 - 100 - 15), (float)(Screen.height - 40), 32f, 32f);
			GUI.DrawTexture(this.r_ammo_gun, this.ammo_machinegun);
			this.gui_style.fontSize = 36;
			this.gui_style.normal.textColor = GUIManager.c[9];
			GUI.Label(new Rect((float)(Screen.width - 40 - 60 + 2 - 110 - 15), (float)(Screen.height - 20 + 2), 0f, 0f), this.WS.GetJeepMGAmmo().ToString(), this.gui_style);
			this.gui_style.normal.textColor = GUIManager.c[8];
			GUI.Label(new Rect((float)(Screen.width - 40 - 60 - 110 - 15), (float)(Screen.height - 20), 0f, 0f), this.WS.GetJeepMGAmmo().ToString(), this.gui_style);
		}
		if (this.myPosition == CONST.VEHICLES.POSITION_JEEP_DRIVER && this.WS.GetModuleRepairKit() > 0)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width - 40), (float)(Screen.height - 196), 32f, 32f), this.moduleRepairKitTex);
		}
		this.DrawCarState();
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0002D020 File Offset: 0x0002B220
	private void DrawCarState()
	{
		Rect position = new Rect((float)(Screen.width - 120), (float)(Screen.height - 156), 120f, 156f);
		this.PlayerPositions[CONST.VEHICLES.POSITION_JEEP_DRIVER] = new Rect((float)(Screen.width - 92), (float)(Screen.height - 80), 16f, 16f);
		this.PlayerPositions[CONST.VEHICLES.POSITION_JEEP_PASS] = new Rect((float)(Screen.width - 44), (float)(Screen.height - 80), 16f, 16f);
		this.PlayerPositions[CONST.VEHICLES.POSITION_JEEP_GUNNER] = new Rect((float)(Screen.width - 68), (float)(Screen.height - 41), 16f, 16f);
		GUI.DrawTexture(position, this.BaseText);
		Color color = GUI.color;
		GUI.color = Color.green;
		GUI.DrawTexture(this.PlayerPositions[this.myPosition], this.PlayerIndicator);
		GUI.color = color;
		for (int i = 0; i < 3; i++)
		{
			if (i != this.myPosition && this.currCar.slots[i] != CONST.VEHICLES.POSITION_NONE)
			{
				GUI.DrawTexture(this.PlayerPositions[i], this.PlayerIndicator);
			}
		}
	}

	// Token: 0x06000260 RID: 608 RVA: 0x0002D160 File Offset: 0x0002B360
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

	// Token: 0x06000261 RID: 609 RVA: 0x0002D249 File Offset: 0x0002B449
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

	// Token: 0x06000262 RID: 610 RVA: 0x0002D258 File Offset: 0x0002B458
	private void Update()
	{
		if (this.TurretAs == null)
		{
			this.TurretAs = this.TMPAudio.GetComponent<AudioSource>();
		}
		if (this.cl == null)
		{
			this.cl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		if (this.m_PlayerControl == null)
		{
			this.m_PlayerControl = (PlayerControl)Object.FindObjectOfType(typeof(PlayerControl));
		}
		if (this.am == null)
		{
			this.am = (Ammo)Object.FindObjectOfType(typeof(Ammo));
			this.am.SetWeapon(203, 0, 0);
		}
		if (this.myPosition == CONST.VEHICLES.POSITION_JEEP_DRIVER)
		{
			this.UpdateForDriver();
			return;
		}
		if (this.myPosition == CONST.VEHICLES.POSITION_JEEP_GUNNER)
		{
			this.UpdateForGunner();
			return;
		}
		if (this.myPosition == CONST.VEHICLES.POSITION_JEEP_PASS)
		{
			this.UpdateForPass();
		}
	}

	// Token: 0x06000263 RID: 611 RVA: 0x0002D34C File Offset: 0x0002B54C
	private void UpdateForDriver()
	{
		if (Time.time > this.enableExit && !base.gameObject.GetComponent<TransportExit>().enabled)
		{
			base.gameObject.GetComponent<TransportExit>().enabled = true;
			base.gameObject.GetComponent<TransportExit>().vehicleType = CONST.VEHICLES.JEEP;
			this.distFromGround = 1.25f;
		}
		if (this.currCar == null)
		{
			this.currCar = base.gameObject.GetComponentInChildren<Car>();
		}
		if (this.currCar == null)
		{
			this.currCar = base.gameObject.GetComponentInParent<Car>();
		}
		if (this.currCar == null)
		{
			return;
		}
		this.dlina = this.currCar.dlina;
		this.shirina = this.currCar.shirina;
		if (this.activeControl)
		{
			if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				this.WS.UseVehicleModul(this.AS, CONST.VEHICLES.VEHICLE_MODUL_REPAIR_KIT);
			}
			this.CheckKeys();
			if (Vector3.Distance(this.RayCastBox.position, GameObject.Find("Player").transform.position) > 10f)
			{
				this.RayCastBox.position = GameObject.Find("Player").transform.position;
			}
			this.speedF = Time.deltaTime * Input.GetAxisRaw("Vertical") * 5f / (Mathf.Abs(180f * this.angX / 3.1415927f) / 8f + 1f);
			this.speedR = Time.deltaTime * Input.GetAxisRaw("Horizontal") * 500f * this.speedF;
			this.speedF *= (float)this.currCar.speed / 100f;
			this.speedR *= (float)this.currCar.speed / 100f;
			this.tryPreRepos();
			this.tryRayCast();
			this.tryRepos();
			this.angX = Mathf.Atan((this.getYF() - this.getYB()) / Vector3.Distance(this.CF.position, this.CB.position));
			this.angZ = Mathf.Atan((this.getYL() - this.getYR()) / Vector3.Distance(this.L3.position, this.R3.position));
			if (this.canMove)
			{
				if (this.lastState != 0)
				{
					this.RayCastBox.rotation = Quaternion.Euler(this.RayCastBox.rotation.eulerAngles.x, this.RayCastBox.rotation.eulerAngles.y + this.speedR, this.RayCastBox.rotation.eulerAngles.z);
					this.RayCastBox.position = this.M.position;
				}
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
						this.s.PlaySound_JeepStart(this.AS);
						this.lastState = 2;
					}
					else if (this.state == 2 && this.lastState == 2)
					{
						this.s.PlaySound_JeepMove(this.AS);
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
						this.s.PlaySound_JeepStop(this.AS);
						this.lastState = 1;
					}
					else if (this.state == 1 && this.lastState == 1)
					{
						this.s.PlaySound_JeepStand(this.AS);
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
					this.s.PlaySound_JeepStop(this.AS);
					this.state = 1;
					this.lastState = 1;
				}
				if (this.state == 1 && this.lastState == 1)
				{
					this.s.PlaySound_JeepStand(this.AS);
				}
				this.lastState = 1;
			}
			if (this.tik < Time.time)
			{
				if (this.cl == null)
				{
					this.cl = (Client)Object.FindObjectOfType(typeof(Client));
				}
				if (this.Turret == null)
				{
					this.Turret = this.currCar.turret;
				}
				this.cl.send_vehicle_turret(this.myTransform.rotation.eulerAngles, this.Turret.localRotation.eulerAngles.y, new Vector3(0f, 0f, 0f), this.currCar.uid);
				this.tik = Time.time + 0.5f;
			}
		}
	}

	// Token: 0x06000264 RID: 612 RVA: 0x0002DB44 File Offset: 0x0002BD44
	private void UpdateForGunner()
	{
		if (!this.klik && this.nextKlik < Time.time)
		{
			this.klik = true;
		}
		if (Time.time > this.enableExit && !base.gameObject.GetComponent<TransportExit>().enabled)
		{
			base.gameObject.GetComponent<TransportExit>().enabled = true;
			base.gameObject.GetComponent<TransportExit>().vehicleType = CONST.VEHICLES.JEEP;
		}
		if (this.currCar == null)
		{
			this.currCar = base.gameObject.GetComponentInChildren<Car>();
		}
		if (this.currCar == null)
		{
			this.currCar = base.gameObject.GetComponentInParent<Car>();
		}
		if (this.currCar == null)
		{
			return;
		}
		if (this.activeControl)
		{
			if (Input.GetKey(KeyCode.Mouse0))
			{
				if (this.WS.GetJeepMGAmmo() > 0)
				{
					if (Time.time > this.MGlastShotTime + this.MGPause)
					{
						this.weapon_raycast(136, 90f, 45);
						this.WS.JeepMGFire();
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
			this.CheckKeys();
			this.ray = this.cam.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f));
			Vector3 point;
			if (Physics.Raycast(this.ray, out this.hit, 1000f))
			{
				point = this.hit.point;
			}
			else
			{
				point = this.ray.GetPoint(1000f);
			}
			Quaternion to = Quaternion.LookRotation(point - this.myTransform.position);
			to.eulerAngles = Quaternion.Euler(0f, to.eulerAngles.y - this.currCar.transform.rotation.eulerAngles.y, 0f).eulerAngles;
			this.Turret.transform.localRotation = Quaternion.RotateTowards(this.Turret.transform.localRotation, to, 40f / ((float)this.currCar.turretRotation / 100f) * Time.deltaTime);
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
			RaycastHit raycastHit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f)), out raycastHit))
			{
				this.target_point = raycastHit.point;
			}
			Vector3 forward = this.currCar.firePoint.position - this.target_point;
			Vector3 eulerAngles = this.currCar.firePoint.eulerAngles;
			Quaternion quaternion = Quaternion.LookRotation(forward, Vector3.right);
			this.currCar.firePoint.rotation = Quaternion.Euler(-quaternion.eulerAngles.x, eulerAngles.y, quaternion.eulerAngles.z);
			if (this.tik < Time.time)
			{
				if (this.cl == null)
				{
					this.cl = (Client)Object.FindObjectOfType(typeof(Client));
				}
				this.cl.send_vehicle_turret(this.myTransform.rotation.eulerAngles, this.Turret.localRotation.eulerAngles.y, new Vector3(0f, 0f, 0f), this.currCar.uid);
				this.tik = Time.time + 0.5f;
			}
		}
	}

	// Token: 0x06000265 RID: 613 RVA: 0x0002E040 File Offset: 0x0002C240
	private void UpdateForPass()
	{
		if (Time.time > this.enableExit && !base.gameObject.GetComponent<TransportExit>().enabled)
		{
			base.gameObject.GetComponent<TransportExit>().enabled = true;
			base.gameObject.GetComponent<TransportExit>().vehicleType = CONST.VEHICLES.JEEP;
		}
		if (this.currCar == null)
		{
			this.currCar = base.gameObject.GetComponentInChildren<Car>();
		}
		if (this.currCar == null)
		{
			this.currCar = base.gameObject.GetComponentInParent<Car>();
		}
		if (this.currCar == null)
		{
			return;
		}
		if (this.activeControl)
		{
			this.CheckKeys();
		}
	}

	// Token: 0x06000266 RID: 614 RVA: 0x0002E0F0 File Offset: 0x0002C2F0
	private void CheckKeys()
	{
		if (Input.GetKeyDown(KeyCode.F1) && (this.currCar.id == this.cl.myindex || this.currCar.slots[CONST.VEHICLES.POSITION_JEEP_DRIVER] == CONST.VEHICLES.POSITION_NONE))
		{
			this.cl.send_enter_the_ent(this.currCar.uid, CONST.VEHICLES.POSITION_JEEP_DRIVER);
		}
		if (Input.GetKeyDown(KeyCode.F2) && (this.currCar.id == this.cl.myindex || this.currCar.slots[CONST.VEHICLES.POSITION_JEEP_GUNNER] == CONST.VEHICLES.POSITION_NONE))
		{
			this.cl.send_enter_the_ent(this.currCar.uid, CONST.VEHICLES.POSITION_JEEP_GUNNER);
		}
		if (Input.GetKeyDown(KeyCode.F3) && (this.currCar.id == this.cl.myindex || this.currCar.slots[CONST.VEHICLES.POSITION_JEEP_PASS] == CONST.VEHICLES.POSITION_NONE))
		{
			this.cl.send_enter_the_ent(this.currCar.uid, CONST.VEHICLES.POSITION_JEEP_PASS);
		}
	}

	// Token: 0x06000267 RID: 615 RVA: 0x0002E204 File Offset: 0x0002C404
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
		this.cl.send_damage(CONST.VEHICLES.VEHICLE_JEEP, component.index, component.hitzone, Time.time, base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z, hit.transform.position.x, hit.transform.position.y, hit.transform.position.z, 0f, 0f, 0f, 0f, 0f, 0f);
		return false;
	}

	// Token: 0x06000268 RID: 616 RVA: 0x0002E3C8 File Offset: 0x0002C5C8
	private void tryRayCast()
	{
		int num = 1025;
		int num2 = 1049601;
		this.L1_Down = Physics.Raycast(this.L1.position, Vector3.down, out this.L1_hit_Down, 10f, num);
		this.L3_Down = Physics.Raycast(this.L3.position, Vector3.down, out this.L3_hit_Down, 10f, num);
		this.L5_Down = Physics.Raycast(this.L5.position, Vector3.down, out this.L5_hit_Down, 10f, num);
		this.R1_Down = Physics.Raycast(this.R1.position, Vector3.down, out this.R1_hit_Down, 10f, num);
		this.R3_Down = Physics.Raycast(this.R3.position, Vector3.down, out this.R3_hit_Down, 10f, num);
		this.R5_Down = Physics.Raycast(this.R5.position, Vector3.down, out this.R5_hit_Down, 10f, num);
		this.M_Down = Physics.Raycast(this.M.position, Vector3.down, out this.M_hit_Down, 10f, num);
		this.CF_Down = Physics.Raycast(this.CF.position, Vector3.down, out this.CF_hit_Down, 10f, num);
		this.CB_Down = Physics.Raycast(this.CB.position, Vector3.down, out this.CB_hit_Down, 10f, num);
		this.L1_Right = Physics.Raycast(new Vector3(this.L1_hit_Down.point.x, this.L1_hit_Down.point.y + 0.5f, this.L1_hit_Down.point.z), this.L1.right, out this.L1_hit_Right, this.shirina, num2);
		this.R1_Back = Physics.Raycast(new Vector3(this.R1_hit_Down.point.x, this.R1_hit_Down.point.y + 0.5f, this.R1_hit_Down.point.z), -this.R1.forward, out this.R1_hit_Back, this.dlina, num2);
		this.R5_Left = Physics.Raycast(new Vector3(this.R5_hit_Down.point.x, this.R5_hit_Down.point.y + 0.5f, this.R5_hit_Down.point.z), -this.R5.right, out this.R5_hit_Left, this.shirina, num2);
		this.L5_Forward = Physics.Raycast(new Vector3(this.L5_hit_Down.point.x, this.L5_hit_Down.point.y + 0.5f, this.L5_hit_Down.point.z), this.L5.forward, out this.L5_hit_Forward, this.dlina, num2);
	}

	// Token: 0x06000269 RID: 617 RVA: 0x0002E6C4 File Offset: 0x0002C8C4
	private void tryRepos()
	{
		if (!this.M_Down || !this.L1_Down || !this.L3_Down || !this.L5_Down || !this.R1_Down || !this.R3_Down || !this.R5_Down || !this.CF_Down || !this.CB_Down)
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
		if ((this.L1_Down && this.L3_Down && this.CF_Down && this.speedF > 0f && (Mathf.Abs(this.L1_hit_Down.point.y - this.L3_hit_Down.point.y) > 2f || Mathf.Abs(this.L1_hit_Down.point.y - this.CF_hit_Down.point.y) > 2f)) || (this.R1_Down && this.R3_Down && this.CF_Down && this.speedF > 0f && (Mathf.Abs(this.R1_hit_Down.point.y - this.R3_hit_Down.point.y) > 2f || Mathf.Abs(this.R1_hit_Down.point.y - this.CF_hit_Down.point.y) > 2f)) || (this.L5_Down && this.L3_Down && this.CB_Down && this.speedF < 0f && (Mathf.Abs(this.L5_hit_Down.point.y - this.L3_hit_Down.point.y) > 2f || Mathf.Abs(this.L5_hit_Down.point.y - this.CB_hit_Down.point.y) > 2f)) || (this.R5_Down && this.R3_Down && this.CB_Down && this.speedF < 0f && (Mathf.Abs(this.R5_hit_Down.point.y - this.R3_hit_Down.point.y) > 2f || Mathf.Abs(this.R5_hit_Down.point.y - this.CB_hit_Down.point.y) > 2f)))
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
		if (this.L3_Down)
		{
			this.L3.position = new Vector3(this.L3_hit_Down.point.x, this.L3_hit_Down.point.y + this.distFromGround, this.L3_hit_Down.point.z);
		}
		else
		{
			this.L3.localPosition = new Vector3(this.M.localPosition.x - this.shirina / 2f, 0f, this.M.localPosition.z);
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
		if (this.R3_Down)
		{
			this.R3.position = new Vector3(this.R3_hit_Down.point.x, this.R3_hit_Down.point.y + this.distFromGround, this.R3_hit_Down.point.z);
		}
		else
		{
			this.R3.localPosition = new Vector3(this.M.localPosition.x + this.shirina / 2f, 0f, this.M.localPosition.z);
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
		this.L3.position = new Vector3(this.L3.position.x, this.getMaxY(), this.L3.position.z);
		this.L5.position = new Vector3(this.L5.position.x, this.getMaxY(), this.L5.position.z);
		this.R1.position = new Vector3(this.R1.position.x, this.getMaxY(), this.R1.position.z);
		this.R3.position = new Vector3(this.R3.position.x, this.getMaxY(), this.R3.position.z);
		this.R5.position = new Vector3(this.R5.position.x, this.getMaxY(), this.R5.position.z);
		this.CF.position = new Vector3(this.CF.position.x, this.getMaxY(), this.CF.position.z);
		this.CB.position = new Vector3(this.CB.position.x, this.getMaxY(), this.CB.position.z);
		float num = Mathf.Sqrt(Mathf.Pow(this.currCar.dlina, 2f) - Mathf.Pow(Mathf.Abs(this.getYF() - this.getYB()), 2f));
		float num2 = Mathf.Sqrt(Mathf.Pow(this.currCar.shirina, 2f) - Mathf.Pow(Mathf.Abs(this.getYL() - this.getYR()), 2f));
		if (num > 4.1f && Mathf.Abs(this.dlina - num) > 0.4f)
		{
			this.dlina = Mathf.Sqrt(Mathf.Pow(this.currCar.dlina, 2f) - Mathf.Pow(Mathf.Abs(this.getYF() - this.getYB()), 2f));
		}
		if (num2 > 3.1f && Mathf.Abs(this.shirina - num2) > 0.4f)
		{
			this.shirina = Mathf.Sqrt(Mathf.Pow(this.currCar.shirina, 2f) - Mathf.Pow(Mathf.Abs(this.getYL() - this.getYR()), 2f));
		}
	}

	// Token: 0x0600026A RID: 618 RVA: 0x0002F484 File Offset: 0x0002D684
	private void tryPreRepos()
	{
		this.M.position = new Vector3(this.RayCastBox.forward.x * this.speedF + this.RayCastBox.position.x, this.RayCastBox.position.y, this.RayCastBox.forward.z * this.speedF + this.RayCastBox.position.z);
		this.L1.localPosition = new Vector3(this.M.localPosition.x - this.shirina / 2f, 0f, this.M.localPosition.z + this.dlina / 2f);
		this.L3.localPosition = new Vector3(this.M.localPosition.x - this.shirina / 2f, 0f, this.M.localPosition.z);
		this.L5.localPosition = new Vector3(this.M.localPosition.x - this.shirina / 2f, 0f, this.M.localPosition.z - this.dlina / 2f);
		this.R1.localPosition = new Vector3(this.M.localPosition.x + this.shirina / 2f, 0f, this.M.localPosition.z + this.dlina / 2f);
		this.R3.localPosition = new Vector3(this.M.localPosition.x + this.shirina / 2f, 0f, this.M.localPosition.z);
		this.R5.localPosition = new Vector3(this.M.localPosition.x + this.shirina / 2f, 0f, this.M.localPosition.z - this.dlina / 2f);
		this.CF.localPosition = new Vector3(this.M.localPosition.x, 0f, this.M.localPosition.z + this.dlina / 2f);
		this.CB.localPosition = new Vector3(this.M.localPosition.x, 0f, this.M.localPosition.z - this.dlina / 2f);
	}

	// Token: 0x0600026B RID: 619 RVA: 0x0002F748 File Offset: 0x0002D948
	private float getMaxY()
	{
		return Mathf.Max(Mathf.Max(Mathf.Max(Mathf.Max(this.L5.position.y, this.R5.position.y), Mathf.Max(this.M.position.y, this.M.position.y)), Mathf.Max(this.CF.position.y, this.CB.position.y)), Mathf.Max(Mathf.Max(Mathf.Max(this.L1.position.y, this.R1.position.y), Mathf.Max(this.L1.position.y, this.R1.position.y)), Mathf.Max(Mathf.Max(this.L1.position.y, this.R1.position.y), Mathf.Max(this.L1.position.y, this.R1.position.y))));
	}

	// Token: 0x0600026C RID: 620 RVA: 0x0002F876 File Offset: 0x0002DA76
	private float getYF()
	{
		return (this.L1_hit_Down.point.y + this.R1_hit_Down.point.y + this.CF_hit_Down.point.y) / 3f;
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0002F8B0 File Offset: 0x0002DAB0
	private float getYB()
	{
		return (this.L5_hit_Down.point.y + this.R5_hit_Down.point.y + this.CB_hit_Down.point.y) / 3f;
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0002F8EA File Offset: 0x0002DAEA
	private float getYL()
	{
		return (this.L1_hit_Down.point.y + this.L5_hit_Down.point.y) / 2f;
	}

	// Token: 0x0600026F RID: 623 RVA: 0x0002F913 File Offset: 0x0002DB13
	private float getYR()
	{
		return (this.R1_hit_Down.point.y + this.R5_hit_Down.point.y) / 2f;
	}

	// Token: 0x06000270 RID: 624 RVA: 0x0002F93C File Offset: 0x0002DB3C
	private float getMaxV()
	{
		float num = Mathf.Max((Mathf.Max(this.L1_hit_Down.point.y, this.R5_hit_Down.point.y) - Mathf.Min(this.L1_hit_Down.point.y, this.R5_hit_Down.point.y)) / 2f + Mathf.Min(this.L1_hit_Down.point.y, this.R5_hit_Down.point.y), (Mathf.Max(this.R1_hit_Down.point.y, this.L5_hit_Down.point.y) - Mathf.Min(this.R1_hit_Down.point.y, this.L5_hit_Down.point.y)) / 2f + Mathf.Min(this.R1_hit_Down.point.y, this.L5_hit_Down.point.y));
		float num2 = Mathf.Max((Mathf.Max(this.L3_hit_Down.point.y, this.R3_hit_Down.point.y) - Mathf.Min(this.L3_hit_Down.point.y, this.R3_hit_Down.point.y)) / 2f + Mathf.Min(this.L3_hit_Down.point.y, this.R3_hit_Down.point.y), (Mathf.Max(this.R3_hit_Down.point.y, this.L3_hit_Down.point.y) - Mathf.Min(this.R3_hit_Down.point.y, this.L3_hit_Down.point.y)) / 2f + Mathf.Min(this.R3_hit_Down.point.y, this.L3_hit_Down.point.y));
		return (num + num2) / 2f;
	}

	// Token: 0x06000271 RID: 625 RVA: 0x0002FB38 File Offset: 0x0002DD38
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

	// Token: 0x06000272 RID: 626 RVA: 0x0002FBB4 File Offset: 0x0002DDB4
	private bool weapon_attack_block(int wid, Vector3i pointvalue)
	{
		if (this.cl == null)
		{
			this.cl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		bool result = true;
		if (wid != 136)
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

	// Token: 0x06000273 RID: 627 RVA: 0x000300F0 File Offset: 0x0002E2F0
	private void weapon_raycast(int wid, float dist, int blockdist)
	{
		if (this.cl == null)
		{
			this.cl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		base.GetComponent<Sound>().PlaySound_WeaponMGTank(this.AS);
		if (!this.oc.zoom || this.myPosition == CONST.VEHICLES.POSITION_JEEP_GUNNER)
		{
			this.currCar.MGFlash.gameObject.SetActive(true);
			this.currCar.FlashTime = Time.time + 0.05f;
		}
		RaycastHit raycastHit;
		if (!Physics.Raycast(this.currCar.firePoint.position, this.currCar.firePoint.forward, out raycastHit))
		{
			this.cl.send_milkattack(wid, Time.time);
			return;
		}
		RaycastHit raycastHit2;
		if (Physics.Linecast(raycastHit.point, this.currCar.firePoint.position, out raycastHit2, this.layerMask))
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
		this.cl.send_damage(wid, component.index, component.hitzone, Time.time, base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z, raycastHit.transform.position.x, raycastHit.transform.position.y, raycastHit.transform.position.z, 0f, 0f, 0f, 0f, 0f, 0f);
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0003039C File Offset: 0x0002E59C
	private Vector3i? GetCursor(bool inside, int radius)
	{
		Ray ray = new Ray(this.currCar.firePoint.position, this.currCar.firePoint.forward);
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

	// Token: 0x04000332 RID: 818
	public Transform RayCastBox;

	// Token: 0x04000333 RID: 819
	public Transform M;

	// Token: 0x04000334 RID: 820
	public Transform L1;

	// Token: 0x04000335 RID: 821
	public Transform L3;

	// Token: 0x04000336 RID: 822
	public Transform L5;

	// Token: 0x04000337 RID: 823
	public Transform R1;

	// Token: 0x04000338 RID: 824
	public Transform R3;

	// Token: 0x04000339 RID: 825
	public Transform R5;

	// Token: 0x0400033A RID: 826
	public Transform CF;

	// Token: 0x0400033B RID: 827
	public Transform CB;

	// Token: 0x0400033C RID: 828
	private Transform myTransform;

	// Token: 0x0400033D RID: 829
	public float distFromGround = 1.45f;

	// Token: 0x0400033E RID: 830
	private int state;

	// Token: 0x0400033F RID: 831
	private int lastState;

	// Token: 0x04000340 RID: 832
	private float angXKoef;

	// Token: 0x04000341 RID: 833
	private float angZKoef;

	// Token: 0x04000342 RID: 834
	private float angX;

	// Token: 0x04000343 RID: 835
	private float angZ;

	// Token: 0x04000344 RID: 836
	private Vector3 lastPos;

	// Token: 0x04000345 RID: 837
	private Vector3 nextPos;

	// Token: 0x04000346 RID: 838
	private Vector3 lastRot;

	// Token: 0x04000347 RID: 839
	private Vector3 nextRot;

	// Token: 0x04000348 RID: 840
	private RaycastHit M_hit_Down;

	// Token: 0x04000349 RID: 841
	private RaycastHit L1_hit_Up;

	// Token: 0x0400034A RID: 842
	private RaycastHit L1_hit_Down;

	// Token: 0x0400034B RID: 843
	private RaycastHit L1_hit_Left;

	// Token: 0x0400034C RID: 844
	private RaycastHit L1_hit_Right;

	// Token: 0x0400034D RID: 845
	private RaycastHit L1_hit_Forward;

	// Token: 0x0400034E RID: 846
	private RaycastHit L1_hit_Back;

	// Token: 0x0400034F RID: 847
	private RaycastHit L3_hit_Up;

	// Token: 0x04000350 RID: 848
	private RaycastHit L3_hit_Down;

	// Token: 0x04000351 RID: 849
	private RaycastHit L3_hit_Left;

	// Token: 0x04000352 RID: 850
	private RaycastHit L3_hit_Right;

	// Token: 0x04000353 RID: 851
	private RaycastHit L3_hit_Forward;

	// Token: 0x04000354 RID: 852
	private RaycastHit L3_hit_Back;

	// Token: 0x04000355 RID: 853
	private RaycastHit L5_hit_Up;

	// Token: 0x04000356 RID: 854
	private RaycastHit L5_hit_Down;

	// Token: 0x04000357 RID: 855
	private RaycastHit L5_hit_Left;

	// Token: 0x04000358 RID: 856
	private RaycastHit L5_hit_Right;

	// Token: 0x04000359 RID: 857
	private RaycastHit L5_hit_Forward;

	// Token: 0x0400035A RID: 858
	private RaycastHit L5_hit_Back;

	// Token: 0x0400035B RID: 859
	private RaycastHit R1_hit_Up;

	// Token: 0x0400035C RID: 860
	private RaycastHit R1_hit_Down;

	// Token: 0x0400035D RID: 861
	private RaycastHit R1_hit_Left;

	// Token: 0x0400035E RID: 862
	private RaycastHit R1_hit_Forward;

	// Token: 0x0400035F RID: 863
	private RaycastHit R1_hit_Back;

	// Token: 0x04000360 RID: 864
	private RaycastHit R3_hit_Up;

	// Token: 0x04000361 RID: 865
	private RaycastHit R3_hit_Down;

	// Token: 0x04000362 RID: 866
	private RaycastHit R3_hit_Left;

	// Token: 0x04000363 RID: 867
	private RaycastHit R3_hit_Right;

	// Token: 0x04000364 RID: 868
	private RaycastHit R3_hit_Forward;

	// Token: 0x04000365 RID: 869
	private RaycastHit R3_hit_Back;

	// Token: 0x04000366 RID: 870
	private RaycastHit R5_hit_Up;

	// Token: 0x04000367 RID: 871
	private RaycastHit R5_hit_Down;

	// Token: 0x04000368 RID: 872
	private RaycastHit R5_hit_Left;

	// Token: 0x04000369 RID: 873
	private RaycastHit R5_hit_Right;

	// Token: 0x0400036A RID: 874
	private RaycastHit R5_hit_Forward;

	// Token: 0x0400036B RID: 875
	private RaycastHit R5_hit_Back;

	// Token: 0x0400036C RID: 876
	private RaycastHit CF_hit_Down;

	// Token: 0x0400036D RID: 877
	private RaycastHit CB_hit_Down;

	// Token: 0x0400036E RID: 878
	private bool M_Down;

	// Token: 0x0400036F RID: 879
	private bool L1_Up;

	// Token: 0x04000370 RID: 880
	private bool L1_Down;

	// Token: 0x04000371 RID: 881
	private bool L1_Left;

	// Token: 0x04000372 RID: 882
	private bool L1_Right;

	// Token: 0x04000373 RID: 883
	private bool L1_Forward;

	// Token: 0x04000374 RID: 884
	private bool L1_Back;

	// Token: 0x04000375 RID: 885
	private bool L3_Up;

	// Token: 0x04000376 RID: 886
	private bool L3_Down;

	// Token: 0x04000377 RID: 887
	private bool L3_Left;

	// Token: 0x04000378 RID: 888
	private bool L3_Right;

	// Token: 0x04000379 RID: 889
	private bool L3_Forward;

	// Token: 0x0400037A RID: 890
	private bool L3_Back;

	// Token: 0x0400037B RID: 891
	private bool L5_Up;

	// Token: 0x0400037C RID: 892
	private bool L5_Down;

	// Token: 0x0400037D RID: 893
	private bool L5_Left;

	// Token: 0x0400037E RID: 894
	private bool L5_Right;

	// Token: 0x0400037F RID: 895
	private bool L5_Forward;

	// Token: 0x04000380 RID: 896
	private bool L5_Back;

	// Token: 0x04000381 RID: 897
	private bool R1_Up;

	// Token: 0x04000382 RID: 898
	private bool R1_Down;

	// Token: 0x04000383 RID: 899
	private bool R1_Left;

	// Token: 0x04000384 RID: 900
	private bool R1_Forward;

	// Token: 0x04000385 RID: 901
	private bool R1_Back;

	// Token: 0x04000386 RID: 902
	private bool R3_Up;

	// Token: 0x04000387 RID: 903
	private bool R3_Down;

	// Token: 0x04000388 RID: 904
	private bool R3_Left;

	// Token: 0x04000389 RID: 905
	private bool R3_Right;

	// Token: 0x0400038A RID: 906
	private bool R3_Forward;

	// Token: 0x0400038B RID: 907
	private bool R3_Back;

	// Token: 0x0400038C RID: 908
	private bool R5_Up;

	// Token: 0x0400038D RID: 909
	private bool R5_Down;

	// Token: 0x0400038E RID: 910
	private bool R5_Left;

	// Token: 0x0400038F RID: 911
	private bool R5_Right;

	// Token: 0x04000390 RID: 912
	private bool R5_Forward;

	// Token: 0x04000391 RID: 913
	private bool R5_Back;

	// Token: 0x04000392 RID: 914
	private bool CF_Down;

	// Token: 0x04000393 RID: 915
	private bool CB_Down;

	// Token: 0x04000394 RID: 916
	private bool canMove = true;

	// Token: 0x04000395 RID: 917
	private float dlina = 4f;

	// Token: 0x04000396 RID: 918
	private float shirina = 3f;

	// Token: 0x04000397 RID: 919
	public float rotationSpeed = 50f;

	// Token: 0x04000398 RID: 920
	public float gunSpeed = 30f;

	// Token: 0x04000399 RID: 921
	public Transform Turret;

	// Token: 0x0400039A RID: 922
	public Camera cam;

	// Token: 0x0400039B RID: 923
	private Ray ray;

	// Token: 0x0400039C RID: 924
	private RaycastHit hit;

	// Token: 0x0400039D RID: 925
	private float speedF;

	// Token: 0x0400039E RID: 926
	private float speedR;

	// Token: 0x0400039F RID: 927
	private Client cl;

	// Token: 0x040003A0 RID: 928
	private float lastTurretAng;

	// Token: 0x040003A1 RID: 929
	public bool activeControl = true;

	// Token: 0x040003A2 RID: 930
	private Map csmap;

	// Token: 0x040003A3 RID: 931
	private ParticleManager cspm;

	// Token: 0x040003A4 RID: 932
	private Sound s;

	// Token: 0x040003A5 RID: 933
	private AudioSource AS;

	// Token: 0x040003A6 RID: 934
	protected vp_FPCamera m_FPCamera;

	// Token: 0x040003A7 RID: 935
	private float lastShotTime;

	// Token: 0x040003A8 RID: 936
	private Texture moduleRepairKitTex;

	// Token: 0x040003A9 RID: 937
	private Texture indicatorAIM;

	// Token: 0x040003AA RID: 938
	private Texture MGTex;

	// Token: 0x040003AB RID: 939
	private Texture ammo_machinegun;

	// Token: 0x040003AC RID: 940
	private bool zoom;

	// Token: 0x040003AD RID: 941
	private Rect r_ammo_gun;

	// Token: 0x040003AE RID: 942
	public vp_FPPlayerEventHandler Player;

	// Token: 0x040003AF RID: 943
	private Ammo am;

	// Token: 0x040003B0 RID: 944
	private OrbitCam oc;

	// Token: 0x040003B1 RID: 945
	private Texture2D BaseText;

	// Token: 0x040003B2 RID: 946
	private Texture2D PlayerIndicator;

	// Token: 0x040003B3 RID: 947
	private Rect[] PlayerPositions;

	// Token: 0x040003B4 RID: 948
	protected PlayerControl m_PlayerControl;

	// Token: 0x040003B5 RID: 949
	private int currTurretState = 1;

	// Token: 0x040003B6 RID: 950
	private Vector3 turretState = Vector3.zero;

	// Token: 0x040003B7 RID: 951
	private AudioSource TurretAs;

	// Token: 0x040003B8 RID: 952
	public GameObject TMPAudio;

	// Token: 0x040003B9 RID: 953
	public float enableExit;

	// Token: 0x040003BA RID: 954
	private List<Vector3i> dirtyBlocks = new List<Vector3i>();

	// Token: 0x040003BB RID: 955
	private List<Vector3i> deletedDirtyBlocks = new List<Vector3i>();

	// Token: 0x040003BC RID: 956
	private WeaponSystem WS;

	// Token: 0x040003BD RID: 957
	private float tik;

	// Token: 0x040003BE RID: 958
	private int lastTeam = -1;

	// Token: 0x040003BF RID: 959
	public Car currCar;

	// Token: 0x040003C0 RID: 960
	public GUIStyle gui_style;

	// Token: 0x040003C1 RID: 961
	private float MGPause = 0.15f;

	// Token: 0x040003C2 RID: 962
	private float MGlastShotTime;

	// Token: 0x040003C3 RID: 963
	private float MGReloadStart;

	// Token: 0x040003C4 RID: 964
	public Transform missle;

	// Token: 0x040003C5 RID: 965
	private bool klik = true;

	// Token: 0x040003C6 RID: 966
	private float nextKlik;

	// Token: 0x040003C7 RID: 967
	private bool isReloading;

	// Token: 0x040003C8 RID: 968
	private int layerMask = 1025;

	// Token: 0x040003C9 RID: 969
	public int myPosition = CONST.VEHICLES.POSITION_NONE;

	// Token: 0x040003CA RID: 970
	private AudioSource AIMAudio;

	// Token: 0x040003CB RID: 971
	private float MissleAIMTime;

	// Token: 0x040003CC RID: 972
	private float MissleAIMTimer;

	// Token: 0x040003CD RID: 973
	private bool underAIM;

	// Token: 0x040003CE RID: 974
	private bool underMissleAIM;

	// Token: 0x040003CF RID: 975
	private Vector3 target_point;
}
