using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class Car : MonoBehaviour
{
	// Token: 0x06000254 RID: 596 RVA: 0x0002BBCC File Offset: 0x00029DCC
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.obj = GameObject.Find("/" + base.gameObject.name);
		if (!this.tex)
		{
			this.tex = (Resources.Load("GUI/jeep_health") as Texture);
		}
		if (!this.armor_tex)
		{
			this.armor_tex = (Resources.Load("GUI/tank_armor") as Texture);
		}
		this.gui_style = new GUIStyle();
		this.gui_style.font = FontManager.font[0];
		this.gui_style.fontSize = 52;
		this.lastPos = Vector3.zero;
		this.MyTransform = base.transform;
		this.slots[0] = CONST.VEHICLES.POSITION_NONE;
		this.slots[1] = CONST.VEHICLES.POSITION_NONE;
		this.slots[2] = CONST.VEHICLES.POSITION_NONE;
		this.UnactiveGunner();
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0002BCC5 File Offset: 0x00029EC5
	public void UnactiveGunner()
	{
		this.Gunner.SetActive(false);
		this.GunnerCap.SetActive(false);
		this.GunnerHelmet.SetActive(false);
		this.GunnerBudge.SetActive(false);
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0002BCF7 File Offset: 0x00029EF7
	public void ActiveGunner(bool _trooper, bool _helmet, bool _cap, bool _budge)
	{
		this.Gunner.SetActive(_trooper);
		this.GunnerCap.SetActive(_cap);
		this.GunnerHelmet.SetActive(_helmet);
		this.GunnerBudge.SetActive(_budge);
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0002BD2C File Offset: 0x00029F2C
	private void Update()
	{
		if (this.cc == null)
		{
			this.cc = (CarController)Object.FindObjectOfType(typeof(CarController));
		}
		if (this.MGFlash == null)
		{
			this.MGFlash = GameObject.Find(base.gameObject.name + "/root/turret/flash").transform;
		}
		if (Time.time > this.FlashTime && this.MGFlash != null)
		{
			this.MGFlash.gameObject.SetActive(false);
		}
		if (this.turret == null)
		{
			this.turret = GameObject.Find(base.gameObject.name + "/root/turret").transform;
		}
		if (this.particles == null)
		{
			this.particles = base.gameObject.GetComponentInChildren<ParticleSystem>();
		}
		if (this.TurretAs == null)
		{
			this.TurretAs = this.turret.GetComponent<AudioSource>();
			if (this.TurretAs == null)
			{
				this.TurretAs = this.turret.gameObject.AddComponent<AudioSource>();
			}
			this.TurretAs.maxDistance = 30f;
			this.TurretAs.spatialBlend = 1f;
		}
		if (this.cc.myPosition == CONST.VEHICLES.POSITION_JEEP_GUNNER)
		{
			float x = Mathf.LerpAngle(this.MyTransform.eulerAngles.x, this.t_x, Time.deltaTime * 10f);
			float z = Mathf.LerpAngle(this.MyTransform.eulerAngles.z, this.t_z, Time.deltaTime * 10f);
			this.MyTransform.eulerAngles = new Vector3(x, this.MyTransform.eulerAngles.y, z);
			if (this.cc.Turret == null)
			{
				this.cc.Turret = this.turret;
			}
			this.t_ry = this.cc.Turret.localRotation.eulerAngles.y;
		}
		else if (this.cc.myPosition == CONST.VEHICLES.POSITION_JEEP_PASS)
		{
			float x2 = Mathf.LerpAngle(this.MyTransform.eulerAngles.x, this.t_x, Time.deltaTime * 10f);
			float z2 = Mathf.LerpAngle(this.MyTransform.eulerAngles.z, this.t_z, Time.deltaTime * 10f);
			float y = Mathf.LerpAngle(this.turret.localEulerAngles.y, this.t_ry, Time.deltaTime);
			this.MyTransform.eulerAngles = new Vector3(x2, this.MyTransform.eulerAngles.y, z2);
			this.turret.localEulerAngles = new Vector3(this.turret.localEulerAngles.x, y, this.turret.localEulerAngles.z);
		}
		else
		{
			this.t_x = this.MyTransform.eulerAngles.x;
			this.t_z = this.MyTransform.eulerAngles.z;
			float y2 = Mathf.LerpAngle(this.turret.localEulerAngles.y, this.t_ry, Time.deltaTime);
			this.turret.localEulerAngles = new Vector3(this.turret.localEulerAngles.x, y2, this.turret.localEulerAngles.z);
		}
		if (this.turret.GetComponent<AudioSource>() == null)
		{
			this.TurretAs = this.turret.gameObject.AddComponent<AudioSource>();
			this.TurretAs.maxDistance = 30f;
			this.TurretAs.spatialBlend = 1f;
		}
		if (this.TurretAs == null)
		{
			this.TurretAs = this.turret.gameObject.GetComponent<AudioSource>();
			this.TurretAs.maxDistance = 30f;
			this.TurretAs.spatialBlend = 1f;
		}
		if (this.health > 40)
		{
			if (this.particles.isPlaying)
			{
				this.particles.Stop();
			}
		}
		else if (this.health < 40 && this.health > 30)
		{
			if (!this.particles.isPlaying)
			{
				this.particles.Play();
			}
			this.particles.startColor = Color.white;
			this.particles.startSize = 0.2f;
		}
		else if (this.health < 30 && this.health > 20)
		{
			if (!this.particles.isPlaying)
			{
				this.particles.Play();
			}
			this.particles.startColor = Color.gray;
			this.particles.startSize = 1f;
		}
		else if (this.health < 20)
		{
			if (!this.particles.isPlaying)
			{
				this.particles.Play();
			}
			this.particles.startSize = 3f;
			this.particles.startColor = Color.black;
		}
		else if (this.health == 0)
		{
			this.KillSelf();
		}
		if (Time.time > this.lastTime)
		{
			if (this.MyTransform.parent != null)
			{
				Vector3 vector = this.lastPos - this.MyTransform.parent.position;
				Vector3 vector2 = this.lastRot - this.MyTransform.parent.rotation.eulerAngles;
				this.lastPos = this.MyTransform.parent.position;
				this.lastRot = this.MyTransform.parent.rotation.eulerAngles;
				if (this.MyTransform.parent.GetComponent<Client>() == null)
				{
					if (this.s == null)
					{
						this.s = this.MyTransform.parent.GetComponent<Sound>();
						this.AS = this.MyTransform.parent.GetComponent<AudioSource>();
					}
					if (this.s != null)
					{
						if (vector.magnitude > 0.05f || vector2.magnitude > 0.05f)
						{
							if (this.state == 0)
							{
								this.s.PlaySound_TankEnter(this.AS);
								this.state = 1;
							}
							else if ((this.state == 1 && this.lastState == 1) || (this.state == 1 && this.lastState == 2))
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
							}
							this.lastRot = this.MyTransform.rotation.eulerAngles;
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
				}
			}
			this.lastTime = Time.time + this.replayTime;
		}
		if (Time.time > this.lastTime2)
		{
			if (this.MyTransform.parent != null && this.MyTransform.parent.GetComponent<Client>() == null)
			{
				if (this.s == null)
				{
					this.s = this.MyTransform.parent.GetComponent<Sound>();
					this.AS = this.MyTransform.parent.GetComponent<AudioSource>();
				}
				Vector3 vector3 = this.turretState - this.turret.localRotation.eulerAngles;
				this.turretState = this.turret.localRotation.eulerAngles;
				if (vector3.magnitude > 0.5f)
				{
					this.s.PlaySound_TurretMove(this.TurretAs);
				}
				else if (vector3.magnitude < 0.1f)
				{
					this.s.PlaySound_TurretStart(this.TurretAs);
				}
			}
			this.lastTime2 = Time.time + this.replayTime2;
		}
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0002C5C8 File Offset: 0x0002A7C8
	private void OnGUI()
	{
		if (this.cc != null)
		{
			this.activeCC = this.cc.enabled;
		}
		if (this.activeCC)
		{
			this.gui_style.normal.textColor = GUIManager.c[9];
			GUI.Label(new Rect(GUIManager.XRES(512f) + 2f, GUIManager.YRES(768f) - 39f + 2f, 0f, 0f), this.health.ToString(), this.gui_style);
			GUI.Label(new Rect(GUIManager.XRES(512f) + 2f - 45f - 25f - 60f - 10f, GUIManager.YRES(768f) - 39f + 2f, 0f, 0f), this.armor.ToString(), this.gui_style);
			if (this.health < 20)
			{
				this.gui_style.normal.textColor = GUIManager.c[1];
			}
			else if (this.health >= 20 && this.health <= 35)
			{
				this.gui_style.normal.textColor = GUIManager.c[3];
			}
			else
			{
				this.gui_style.normal.textColor = GUIManager.c[8];
			}
			GUI.Label(new Rect(GUIManager.XRES(512f), GUIManager.YRES(768f) - 39f, 0f, 0f), this.health.ToString(), this.gui_style);
			GUI.Label(new Rect(GUIManager.XRES(512f) - 45f - 25f - 60f - 10f, GUIManager.YRES(768f) - 39f, 0f, 0f), this.armor.ToString(), this.gui_style);
			GUI.DrawTexture(new Rect(GUIManager.XRES(512f) - 40f, GUIManager.YRES(768f) - 41f, 32f, 32f), this.tex);
			GUI.DrawTexture(new Rect(GUIManager.XRES(512f) - 40f - 10f - 60f - 60f - 10f, GUIManager.YRES(768f) - 41f, 32f, 32f), this.armor_tex);
		}
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0002C860 File Offset: 0x0002AA60
	public void CheckSlots(int plid)
	{
		if (this.slots[CONST.VEHICLES.POSITION_JEEP_DRIVER] == plid)
		{
			this.slots[CONST.VEHICLES.POSITION_JEEP_DRIVER] = CONST.VEHICLES.POSITION_NONE;
		}
		if (this.slots[CONST.VEHICLES.POSITION_JEEP_GUNNER] == plid)
		{
			this.slots[CONST.VEHICLES.POSITION_JEEP_GUNNER] = CONST.VEHICLES.POSITION_NONE;
		}
		if (this.slots[CONST.VEHICLES.POSITION_JEEP_PASS] == plid)
		{
			this.slots[CONST.VEHICLES.POSITION_JEEP_PASS] = CONST.VEHICLES.POSITION_NONE;
		}
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0002C8CD File Offset: 0x0002AACD
	public void KillSelf()
	{
		if (base.gameObject == null)
		{
			return;
		}
		EntManager.DeleteEnt(this.entid);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040002FC RID: 764
	public int id;

	// Token: 0x040002FD RID: 765
	public int uid;

	// Token: 0x040002FE RID: 766
	public int entid;

	// Token: 0x040002FF RID: 767
	public int classID;

	// Token: 0x04000300 RID: 768
	public int tank_type;

	// Token: 0x04000301 RID: 769
	public int health = 100;

	// Token: 0x04000302 RID: 770
	public int armor;

	// Token: 0x04000303 RID: 771
	public int speed = 100;

	// Token: 0x04000304 RID: 772
	public int reload = 100;

	// Token: 0x04000305 RID: 773
	public int turretRotation = 100;

	// Token: 0x04000306 RID: 774
	public int skin_id;

	// Token: 0x04000307 RID: 775
	public float dlina = 4f;

	// Token: 0x04000308 RID: 776
	public float shirina = 3f;

	// Token: 0x04000309 RID: 777
	public Transform MGFlash;

	// Token: 0x0400030A RID: 778
	private Transform MyTransform;

	// Token: 0x0400030B RID: 779
	public float FlashTime;

	// Token: 0x0400030C RID: 780
	private Client cscl;

	// Token: 0x0400030D RID: 781
	public bool gunner;

	// Token: 0x0400030E RID: 782
	private EntManager entmanager;

	// Token: 0x0400030F RID: 783
	private GameObject obj;

	// Token: 0x04000310 RID: 784
	private GUIStyle gui_style;

	// Token: 0x04000311 RID: 785
	public Texture tex;

	// Token: 0x04000312 RID: 786
	public Texture armor_tex;

	// Token: 0x04000313 RID: 787
	private CarController cc;

	// Token: 0x04000314 RID: 788
	private bool activeCC;

	// Token: 0x04000315 RID: 789
	private Vector3 lastPos;

	// Token: 0x04000316 RID: 790
	private Vector3 lastRot;

	// Token: 0x04000317 RID: 791
	private int state;

	// Token: 0x04000318 RID: 792
	private int lastState;

	// Token: 0x04000319 RID: 793
	public Sound s;

	// Token: 0x0400031A RID: 794
	private AudioSource AS;

	// Token: 0x0400031B RID: 795
	private float lastTime;

	// Token: 0x0400031C RID: 796
	private float replayTime = 0.04f;

	// Token: 0x0400031D RID: 797
	private float lastTime2;

	// Token: 0x0400031E RID: 798
	private float replayTime2 = 0.1f;

	// Token: 0x0400031F RID: 799
	private int currTurretState = 1;

	// Token: 0x04000320 RID: 800
	private Vector3 turretState = Vector3.zero;

	// Token: 0x04000321 RID: 801
	private AudioSource TurretAs;

	// Token: 0x04000322 RID: 802
	public Transform turret;

	// Token: 0x04000323 RID: 803
	public Transform JeepMesh;

	// Token: 0x04000324 RID: 804
	public Transform firePoint;

	// Token: 0x04000325 RID: 805
	public float t_x;

	// Token: 0x04000326 RID: 806
	public float t_z;

	// Token: 0x04000327 RID: 807
	public float t_ry;

	// Token: 0x04000328 RID: 808
	public float g_rx;

	// Token: 0x04000329 RID: 809
	public int team;

	// Token: 0x0400032A RID: 810
	private bool initialized;

	// Token: 0x0400032B RID: 811
	private ParticleSystem particles;

	// Token: 0x0400032C RID: 812
	public int[] slots = new int[3];

	// Token: 0x0400032D RID: 813
	public GameObject GunnerPos;

	// Token: 0x0400032E RID: 814
	public GameObject Gunner;

	// Token: 0x0400032F RID: 815
	public GameObject GunnerCap;

	// Token: 0x04000330 RID: 816
	public GameObject GunnerHelmet;

	// Token: 0x04000331 RID: 817
	public GameObject GunnerBudge;
}
