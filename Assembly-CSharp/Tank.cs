using System;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class Tank : MonoBehaviour
{
	// Token: 0x06000280 RID: 640 RVA: 0x000314DC File Offset: 0x0002F6DC
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.obj = GameObject.Find("/" + base.gameObject.name);
		if (!this.tex)
		{
			this.tex = (Resources.Load("GUI/tank_health") as Texture);
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
	}

	// Token: 0x06000281 RID: 641 RVA: 0x000315A8 File Offset: 0x0002F7A8
	private void Update()
	{
		if (this.MG == null)
		{
			this.MG = GameObject.Find(base.gameObject.name + "/root/turret/barrel/module1/TANK MG").transform;
		}
		if (this.MGFlash == null)
		{
			this.MGFlash = GameObject.Find(base.gameObject.name + "/root/turret/barrel/module1/TANK MG/flash").transform;
		}
		if (Time.time > this.FlashTime && this.MGFlash != null)
		{
			this.MGFlash.gameObject.SetActive(false);
		}
		if (this.turret == null)
		{
			this.turret = GameObject.Find(base.gameObject.name + "/root/turret").transform;
		}
		if (this.turretBone == null)
		{
			this.turretBone = GameObject.Find(base.gameObject.name + "/root/turret").transform;
		}
		if (this.gun == null)
		{
			this.gun = GameObject.Find(base.gameObject.name + "/root/turret/barrel").transform;
		}
		if (this.FP == null)
		{
			this.FP = GameObject.Find(base.gameObject.name + "/root/turret/barrel/FirePoint").transform;
		}
		if (this.particles == null)
		{
			this.particles = base.gameObject.GetComponentInChildren<ParticleSystem>();
		}
		if (!this.client)
		{
			float x = Mathf.LerpAngle(this.MyTransform.eulerAngles.x, this.t_x, Time.deltaTime * 10f);
			float z = Mathf.LerpAngle(this.MyTransform.eulerAngles.z, this.t_z, Time.deltaTime * 10f);
			float y = Mathf.LerpAngle(this.turretBone.localEulerAngles.y, this.t_ry, Time.deltaTime);
			float x2 = Mathf.LerpAngle(this.gun.localEulerAngles.x, this.g_rx, Time.deltaTime);
			this.MyTransform.eulerAngles = new Vector3(x, this.MyTransform.eulerAngles.y, z);
			this.turretBone.localEulerAngles = new Vector3(this.turretBone.localEulerAngles.x, y, this.turretBone.localEulerAngles.z);
			this.gun.localEulerAngles = new Vector3(x2, this.gun.localEulerAngles.y, this.gun.localEulerAngles.z);
		}
		else
		{
			if (this.tc == null)
			{
				this.tc = (TankController)Object.FindObjectOfType(typeof(TankController));
			}
			if (this.tc.Turret == null)
			{
				this.tc.Turret = this.turret;
			}
			if (this.tc.Gun == null)
			{
				this.tc.Gun = this.gun;
			}
			if (this.tc.FP == null)
			{
				this.tc.FP = this.FP;
			}
			this.t_x = this.MyTransform.eulerAngles.x;
			this.t_z = this.MyTransform.eulerAngles.z;
			this.t_ry = this.tc.Turret.localRotation.eulerAngles.y;
			this.g_rx = this.tc.Gun.localRotation.eulerAngles.x;
		}
		if (this.turret.GetComponent<AudioSource>() == null)
		{
			this.TurretAs = this.turret.gameObject.AddComponent<AudioSource>();
		}
		if (this.TurretAs == null)
		{
			this.TurretAs = this.turret.gameObject.GetComponent<AudioSource>();
		}
		this.TurretAs.maxDistance = 30f;
		this.TurretAs.spatialBlend = 1f;
		if (this.health > 69)
		{
			if (this.particles.isPlaying)
			{
				this.particles.Stop();
			}
		}
		else if (this.health < 69 && this.health > 60)
		{
			if (!this.particles.isPlaying)
			{
				this.particles.Play();
			}
			this.particles.startColor = Color.white;
			this.particles.startSize = 0.2f;
		}
		else if (this.health < 60 && this.health > 30)
		{
			if (!this.particles.isPlaying)
			{
				this.particles.Play();
			}
			this.particles.startColor = Color.gray;
			this.particles.startSize = 1f;
		}
		else if (this.health < 30)
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
								this.s.PlaySound_TankStart(this.AS);
								this.lastState = 2;
							}
							else if (this.state == 2 && this.lastState == 2)
							{
								this.s.PlaySound_TankMove(this.AS);
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
				Vector3 vector3 = this.turretState - this.turretBone.localRotation.eulerAngles;
				this.turretState = this.turretBone.localRotation.eulerAngles;
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

	// Token: 0x06000282 RID: 642 RVA: 0x00031E68 File Offset: 0x00030068
	private void OnGUI()
	{
		if (this.tc != null)
		{
			this.activeTC = this.tc.enabled;
		}
		if (this.activeTC && base.gameObject.transform.parent != null && base.gameObject.transform.parent.gameObject.name == "Player")
		{
			this.gui_style.normal.textColor = GUIManager.c[9];
			GUI.Label(new Rect(GUIManager.XRES(512f) + 2f, GUIManager.YRES(768f) - 39f + 2f, 0f, 0f), this.health.ToString(), this.gui_style);
			GUI.Label(new Rect(GUIManager.XRES(512f) + 2f - 45f - 25f - 60f - 10f, GUIManager.YRES(768f) - 39f + 2f, 0f, 0f), this.armor.ToString(), this.gui_style);
			if (this.health < 30)
			{
				this.gui_style.normal.textColor = GUIManager.c[1];
			}
			else if (this.health >= 30 && this.health <= 60)
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

	// Token: 0x06000283 RID: 643 RVA: 0x00032142 File Offset: 0x00030342
	public void KillSelf()
	{
		if (base.gameObject == null)
		{
			return;
		}
		EntManager.DeleteEnt(this.entid);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040003FA RID: 1018
	public int id;

	// Token: 0x040003FB RID: 1019
	public int uid;

	// Token: 0x040003FC RID: 1020
	public int entid;

	// Token: 0x040003FD RID: 1021
	public int classID;

	// Token: 0x040003FE RID: 1022
	public int tank_type;

	// Token: 0x040003FF RID: 1023
	public int health = 100;

	// Token: 0x04000400 RID: 1024
	public int armor;

	// Token: 0x04000401 RID: 1025
	public int speed = 100;

	// Token: 0x04000402 RID: 1026
	public int reload = 100;

	// Token: 0x04000403 RID: 1027
	public int turretRotation = 100;

	// Token: 0x04000404 RID: 1028
	public int skin_id;

	// Token: 0x04000405 RID: 1029
	public float dlina = 4.4f;

	// Token: 0x04000406 RID: 1030
	public float shirina = 3f;

	// Token: 0x04000407 RID: 1031
	public Transform MG;

	// Token: 0x04000408 RID: 1032
	public Transform MGFlash;

	// Token: 0x04000409 RID: 1033
	private Transform MyTransform;

	// Token: 0x0400040A RID: 1034
	public float FlashTime;

	// Token: 0x0400040B RID: 1035
	private Client cscl;

	// Token: 0x0400040C RID: 1036
	public bool client;

	// Token: 0x0400040D RID: 1037
	private EntManager entmanager;

	// Token: 0x0400040E RID: 1038
	private GameObject obj;

	// Token: 0x0400040F RID: 1039
	private GUIStyle gui_style;

	// Token: 0x04000410 RID: 1040
	public Texture tex;

	// Token: 0x04000411 RID: 1041
	public Texture armor_tex;

	// Token: 0x04000412 RID: 1042
	private TankController tc;

	// Token: 0x04000413 RID: 1043
	private bool activeTC;

	// Token: 0x04000414 RID: 1044
	private Vector3 lastPos;

	// Token: 0x04000415 RID: 1045
	private Vector3 lastRot;

	// Token: 0x04000416 RID: 1046
	private int state;

	// Token: 0x04000417 RID: 1047
	private int lastState;

	// Token: 0x04000418 RID: 1048
	public Sound s;

	// Token: 0x04000419 RID: 1049
	private AudioSource AS;

	// Token: 0x0400041A RID: 1050
	private float lastTime;

	// Token: 0x0400041B RID: 1051
	private float replayTime = 0.04f;

	// Token: 0x0400041C RID: 1052
	private float lastTime2;

	// Token: 0x0400041D RID: 1053
	private float replayTime2 = 0.1f;

	// Token: 0x0400041E RID: 1054
	private int currTurretState = 1;

	// Token: 0x0400041F RID: 1055
	private Vector3 turretState = Vector3.zero;

	// Token: 0x04000420 RID: 1056
	private AudioSource TurretAs;

	// Token: 0x04000421 RID: 1057
	private Transform turret;

	// Token: 0x04000422 RID: 1058
	private Transform turretBone;

	// Token: 0x04000423 RID: 1059
	private Transform gun;

	// Token: 0x04000424 RID: 1060
	private Transform FP;

	// Token: 0x04000425 RID: 1061
	public float t_x;

	// Token: 0x04000426 RID: 1062
	public float t_z;

	// Token: 0x04000427 RID: 1063
	public float t_ry;

	// Token: 0x04000428 RID: 1064
	public float g_rx;

	// Token: 0x04000429 RID: 1065
	private bool initialized;

	// Token: 0x0400042A RID: 1066
	private ParticleSystem particles;
}
