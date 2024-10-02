using System;
using UnityEngine;

// Token: 0x020000D9 RID: 217
[RequireComponent(typeof(vp_FPWeapon))]
public class vp_FPWeaponShooter : vp_Shooter
{
	// Token: 0x17000053 RID: 83
	// (get) Token: 0x060007EB RID: 2027 RVA: 0x00076276 File Offset: 0x00074476
	private vp_FPPlayerEventHandler Player
	{
		get
		{
			if (this.m_Player == null && this.EventHandler != null)
			{
				this.m_Player = (vp_FPPlayerEventHandler)this.EventHandler;
			}
			return this.m_Player;
		}
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x000762AC File Offset: 0x000744AC
	protected override void Awake()
	{
		this.m_FPSWeapon = base.transform.GetComponent<vp_FPWeapon>();
		base.Awake();
		this.m_FPSCamera = base.transform.root.GetComponentInChildren<vp_FPCamera>();
		this.m_FPSWeaponReloader = base.gameObject.GetComponent<vp_FPWeaponReloader>();
		this.m_OperatorTransform = this.m_FPSCamera.transform;
		this.m_NextAllowedFireTime = Time.time;
		this.m_SecondNextAllowedFireTime = Time.time;
		this.ProjectileSpawnDelay = Mathf.Min(this.ProjectileSpawnDelay, this.ProjectileFiringRate - 0.1f);
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x0007633C File Offset: 0x0007453C
	protected override void Start()
	{
		base.Start();
		this.m_FPSWeapon = base.transform.GetComponent<vp_FPWeapon>();
		ITEM weaponID = (ITEM)this.m_FPSWeapon.WeaponID;
		this.ZoomTexture = ContentLoader.LoadTexture(weaponID.ToString() + "_ZOOM");
		weaponID = (ITEM)this.m_FPSWeapon.WeaponID;
		this.ZoomTexture2 = ContentLoader.LoadTexture(weaponID.ToString() + "_ZOOM_BLACK");
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x000763BC File Offset: 0x000745BC
	protected override void Update()
	{
		if (this.tc == null)
		{
			this.tc = (TankController)Object.FindObjectOfType(typeof(TankController));
		}
		if (this.tc != null)
		{
			this.activeTC = this.tc.enabled;
		}
		if (this.cc == null)
		{
			this.cc = (CarController)Object.FindObjectOfType(typeof(CarController));
		}
		if (this.cc != null)
		{
			this.activeCC = this.cc.enabled;
		}
		if (this.sound == null)
		{
			this.sound = (Sound)Object.FindObjectOfType(typeof(Sound));
		}
		if (!this.activeTC && !this.activeCC)
		{
			this.m_FPSCamera.ShakeSpeed = Mathf.Lerp(this.m_FPSCamera.ShakeSpeed, 0f, Time.deltaTime * 25f);
			base.Update();
			if (this.Player.Attack.Active)
			{
				this.TryFire();
			}
			if (this.m_FPSCamera.GetComponent<Camera>().fieldOfView < 46f)
			{
				this.m_FPSWeapon.WeaponModel.SetActive(false);
				vp_FPWeaponShooter.fpsguidraw = true;
			}
			else if (this.m_FPSCamera.GetComponent<Camera>().fieldOfView > 64f)
			{
				this.m_FPSWeapon.WeaponModel.SetActive(true);
				vp_FPWeaponShooter.fpsguidraw = false;
			}
		}
		if (this.m_FPSWeapon.WeaponID == 137 || this.m_FPSWeapon.WeaponID == 144)
		{
			if (Input.GetMouseButton(1))
			{
				if (this.m_FPSCamera.FPController.Velocity.magnitude > 2.3f)
				{
					this.m_FPSWeapon.m_Input.stopmove = Time.time + 0.1f;
				}
				if (this.MG == null)
				{
					this.MG = (Minigun)Object.FindObjectOfType(typeof(Minigun));
				}
				if (this.MG.speed < 800)
				{
					this.MG.speed += 5 + this.MG.speed / 100;
					if (this.MG.speed > 800)
					{
						this.MG.speed = 800;
					}
					this.MG.speedUp = true;
				}
			}
			if (Input.GetMouseButtonUp(1))
			{
				if (this.MG == null)
				{
					this.MG = (Minigun)Object.FindObjectOfType(typeof(Minigun));
				}
				this.MG.speedUp = false;
			}
		}
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x00076670 File Offset: 0x00074870
	private void OnGUI()
	{
		if (vp_FPWeaponShooter.fpsguidraw)
		{
			float num = (float)Screen.height * 1.33333f;
			float num2 = ((float)Screen.width - num) / 2f;
			float num3 = this.m_LastFireTime + this.ProjectileTapFiringRate - Time.time;
			if (num3 < 0f)
			{
				num3 = 0f;
			}
			else
			{
				num3 *= 300f;
			}
			Rect position = new Rect(num2 + -num3 / 2f, -num3 / 2f, num + num3, (float)Screen.height + num3);
			if (this.ZoomTexture2)
			{
				if (this.m_WeaponSystem == null)
				{
					this.m_WeaponSystem = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
				}
				if (this.m_WeaponSystem.GetBlackSkin())
				{
					GUI.DrawTexture(position, this.ZoomTexture2);
				}
				else if (this.ZoomTexture)
				{
					GUI.DrawTexture(position, this.ZoomTexture);
				}
			}
			else if (this.ZoomTexture)
			{
				GUI.DrawTexture(position, this.ZoomTexture);
			}
			if (this.m_FPSWeapon.WeaponID == 3 || this.m_FPSWeapon.WeaponID == 10 || this.m_FPSWeapon.WeaponID == 19 || this.m_FPSWeapon.WeaponID == 18 || this.m_FPSWeapon.WeaponID == 47 || this.m_FPSWeapon.WeaponID == 69 || this.m_FPSWeapon.WeaponID == 71 || this.m_FPSWeapon.WeaponID == 81 || this.m_FPSWeapon.WeaponID == 34 || this.m_FPSWeapon.WeaponID == 100 || this.m_FPSWeapon.WeaponID == 104 || this.m_FPSWeapon.WeaponID == 111 || this.m_FPSWeapon.WeaponID == 112 || this.m_FPSWeapon.WeaponID == 138 || this.m_FPSWeapon.WeaponID == 142 || this.m_FPSWeapon.WeaponID == 161 || this.m_FPSWeapon.WeaponID == 176 || this.m_FPSWeapon.WeaponID == 185 || this.m_FPSWeapon.WeaponID == 218 || this.m_FPSWeapon.WeaponID == 302 || this.m_FPSWeapon.WeaponID == 334 || this.m_FPSWeapon.WeaponID == 335 || this.m_FPSWeapon.WeaponID == 336 || this.m_FPSWeapon.WeaponID == 337 || this.m_FPSWeapon.WeaponID == 338 || this.m_FPSWeapon.WeaponID == 350 || this.m_FPSWeapon.WeaponID == 332)
			{
				GUI.DrawTexture(new Rect(0f, 0f, num2, (float)Screen.height), GUIManager.tex_black);
				GUI.DrawTexture(new Rect(num + num2, 0f, num2, (float)Screen.height), GUIManager.tex_black);
			}
		}
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x000769AC File Offset: 0x00074BAC
	public override void TryFire()
	{
		if (this.m_WeaponSystem == null)
		{
			this.m_WeaponSystem = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		this.currentFiringWeaponID = this.m_FPSWeapon.WeaponID;
		if (this.m_FPSWeapon.WeaponID == 221 && this.secondFire)
		{
			if (Time.time < this.m_SecondNextAllowedFireTime)
			{
				return;
			}
		}
		else if (Time.time < this.m_NextAllowedFireTime)
		{
			return;
		}
		if (this.Player.SetWeapon.Active)
		{
			return;
		}
		if (!this.m_FPSWeapon.Wielded)
		{
			return;
		}
		if (this.m_FPSWeapon.WeaponID == 62)
		{
			this.Player.Crouch.TryStart(true);
		}
		if (this.m_FPSWeapon.WeaponID == 137 || this.m_FPSWeapon.WeaponID == 144)
		{
			if (this.m_FPSCamera.FPController.Velocity.magnitude > 2.3f)
			{
				this.m_FPSWeapon.m_Input.stopmove = Time.time + 0.1f;
			}
			if (this.MG == null)
			{
				this.MG = (Minigun)Object.FindObjectOfType(typeof(Minigun));
			}
			if (this.MG.speed < 800)
			{
				this.MG.speed += 5 + this.MG.speed / 100;
				if (this.MG.speed > 800)
				{
					this.MG.speed = 800;
				}
				this.MG.speedUp = true;
			}
			if (this.m_FPSCamera.FPController.Velocity.magnitude > 3.5f)
			{
				return;
			}
			if (this.MG.speed < 700)
			{
				return;
			}
		}
		if (this.m_FPSWeapon.WeaponID == 71 || this.m_FPSWeapon.WeaponID == 111 || this.m_FPSWeapon.WeaponID == 62 || this.m_FPSWeapon.WeaponID == 142)
		{
			this.m_FPSWeapon.m_Input.stopmove = Time.time + 0.15f;
			if (this.m_FPSCamera.FPController.Velocity.magnitude >= 0.1f)
			{
				return;
			}
		}
		if (this.m_FPSWeapon.WeaponID == 221)
		{
			if (this.secondFire)
			{
				if (this.m_WeaponSystem.secondFireCount >= this.m_WeaponSystem.ammo_fullclip2 / 2)
				{
					this.DryFire();
					this.secondFire = false;
					return;
				}
			}
			else if (this.m_WeaponSystem.firstFireCount >= this.m_WeaponSystem.ammo_fullclip2 / 2)
			{
				this.DryFire();
				this.secondFire = false;
				return;
			}
		}
		if (!this.m_WeaponSystem.OnWeaponAttack(this))
		{
			this.DryFire();
			this.secondFire = false;
			return;
		}
		this.Fire();
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x00076C90 File Offset: 0x00074E90
	protected override void Fire()
	{
		this.m_LastFireTime = Time.time;
		if (this.AnimationFire != null)
		{
			this.m_FPSWeapon.WeaponModel.GetComponent<Animation>().Stop(this.AnimationFire.name);
			this.m_FPSWeapon.WeaponModel.GetComponent<Animation>().Play(this.AnimationFire.name);
		}
		if (this.m_FPSWeapon.WeaponID == 221)
		{
			this.m_Player.Zoom.TryStop(true);
			this.m_Player.Zoom.NextAllowedStartTime = Time.time;
			if (this.secondFire)
			{
				this.m_WeaponSystem.secondFireCount++;
				this.m_FPSWeapon.WeaponModel.GetComponent<CustomAnimation>().RightColtFire();
			}
			else
			{
				this.m_WeaponSystem.firstFireCount++;
				this.m_FPSWeapon.WeaponModel.GetComponent<CustomAnimation>().LeftColtFire();
			}
		}
		if (this.MotionRecoilDelay == 0f)
		{
			this.ApplyRecoil();
		}
		else
		{
			vp_Timer.In(this.MotionRecoilDelay, new vp_Timer.Callback(this.ApplyRecoil), null);
		}
		base.Fire();
		if (this.m_FPSWeapon.WeaponID == 16 || this.m_FPSWeapon.WeaponID == 143)
		{
			this.m_FPSCamera.ShakeSpeed = 0.25f;
		}
		else if (this.m_FPSWeapon.WeaponID == 3)
		{
			if (this.Player.Zoom.Active)
			{
				this.m_FPSCamera.ShakeSpeed = 0.35f;
			}
			else
			{
				this.m_FPSCamera.ShakeSpeed = 0.1f;
			}
		}
		else if (this.m_FPSWeapon.WeaponID == 47)
		{
			if (this.Player.Zoom.Active)
			{
				this.m_FPSCamera.ShakeSpeed = 0.45f;
			}
			else
			{
				this.m_FPSCamera.ShakeSpeed = 0.1f;
			}
		}
		else if (this.m_FPSWeapon.WeaponID == 71 || this.m_FPSWeapon.WeaponID == 142 || this.m_FPSWeapon.WeaponID == 137 || this.m_FPSWeapon.WeaponID == 144)
		{
			if (this.Player.Zoom.Active)
			{
				this.m_FPSCamera.ShakeSpeed = 0.2f;
			}
			else
			{
				this.m_FPSCamera.ShakeSpeed = 0.3f;
			}
		}
		else if (this.m_FPSWeapon.WeaponID == 69 || this.m_FPSWeapon.WeaponID == 34 || this.m_FPSWeapon.WeaponID == 334)
		{
			this.m_FPSCamera.ShakeSpeed = 0.25f;
		}
		else if (this.m_FPSWeapon.WeaponID == 89)
		{
			this.m_FPSCamera.ShakeSpeed = 0.1f;
		}
		else if (this.m_FPSWeapon.WeaponID == 315)
		{
			this.m_FPSCamera.ShakeSpeed = 0f;
		}
		else
		{
			this.m_FPSCamera.ShakeSpeed = 0.05f;
		}
		this.secondFire = false;
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x00076FAC File Offset: 0x000751AC
	protected virtual void ApplyRecoil()
	{
		this.m_FPSWeapon.ResetSprings(this.MotionPositionReset, this.MotionRotationReset, this.MotionPositionPause, this.MotionRotationPause);
		if (this.MotionRotationRecoil.z == 0f)
		{
			this.m_FPSWeapon.AddForce2(this.MotionPositionRecoil, this.MotionRotationRecoil);
			return;
		}
		this.m_FPSWeapon.AddForce2(this.MotionPositionRecoil, Vector3.Scale(this.MotionRotationRecoil, Vector3.one + Vector3.back) + ((Random.value < 0.5f) ? Vector3.forward : Vector3.back) * Random.Range(this.MotionRotationRecoil.z * this.MotionRotationRecoilDeadZone, this.MotionRotationRecoil.z));
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x00077078 File Offset: 0x00075278
	public virtual void DryFire()
	{
		this.m_LastFireTime = Time.time;
		this.m_FPSWeapon.AddForce2(this.MotionPositionRecoil * this.MotionDryFireRecoil, this.MotionRotationRecoil * this.MotionDryFireRecoil);
		if (base.Audio != null)
		{
			if (this.SoundDryFire == null)
			{
				this.SoundDryFire = this.sound.GetDryFire();
			}
			base.Audio.pitch = Time.timeScale;
			base.Audio.PlayOneShot(this.SoundDryFire, AudioListener.volume);
			this.DisableFiring(10000000f);
		}
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x0007711C File Offset: 0x0007531C
	protected virtual void OnStop_Attack()
	{
		if (this.m_FPSWeapon.WeaponID == 137 || this.m_FPSWeapon.WeaponID == 144)
		{
			if (this.MG == null)
			{
				this.MG = (Minigun)Object.FindObjectOfType(typeof(Minigun));
			}
			this.MG.speedUp = false;
		}
		if (this.ProjectileFiringRate == 0f)
		{
			this.EnableFiring();
			return;
		}
		this.DisableFiring(this.ProjectileTapFiringRate - (Time.time - this.m_LastFireTime));
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x000771B0 File Offset: 0x000753B0
	protected virtual void OnStart_Zoom()
	{
		if (this.m_WeaponSystem == null)
		{
			this.m_WeaponSystem = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		if (this.m_FPSWeapon.WeaponID == 221)
		{
			this.m_Player.Zoom.TryStop(true);
			this.m_Player.Zoom.NextAllowedStartTime = Time.time;
			this.secondFire = true;
			this.TryFire();
			return;
		}
		if (this.m_FPSWeapon.WeaponID == 62 || this.m_FPSWeapon.WeaponID == 137 || this.m_FPSWeapon.WeaponID == 144)
		{
			return;
		}
		this.Player.Zoom.NextAllowedStartTime = Time.time + 0.1f;
		this.Player.Zoom.NextAllowedStopTime = Time.time + 0.1f;
		if (this.m_FPSWeapon.WeaponID != 79 && this.m_FPSWeapon.WeaponID != 80 && this.m_FPSWeapon.WeaponID != 208)
		{
			if (this.m_FPSWeapon.WeaponID == 3 || this.m_FPSWeapon.WeaponID == 336)
			{
				this.m_FPSCamera.RenderingFieldOfView = 10f;
			}
			else if (this.m_FPSWeapon.WeaponID == 10)
			{
				this.m_FPSCamera.RenderingFieldOfView = 25f;
			}
			else if (this.m_FPSWeapon.WeaponID == 18 || this.m_FPSWeapon.WeaponID == 19 || this.m_FPSWeapon.WeaponID == 335)
			{
				this.m_FPSCamera.RenderingFieldOfView = 40f;
			}
			else if (this.m_FPSWeapon.WeaponID == 47)
			{
				this.m_FPSCamera.RenderingFieldOfView = 10f;
			}
			else if (this.m_FPSWeapon.WeaponID == 69 || this.m_FPSWeapon.WeaponID == 34 || this.m_FPSWeapon.WeaponID == 334)
			{
				this.m_FPSCamera.RenderingFieldOfView = 20f;
			}
			else if (this.m_FPSWeapon.WeaponID == 81 || this.m_FPSWeapon.WeaponID == 332)
			{
				this.m_FPSCamera.RenderingFieldOfView = 38f;
			}
			else if (this.m_FPSWeapon.WeaponID == 100 || this.m_FPSWeapon.WeaponID == 350)
			{
				this.m_FPSCamera.RenderingFieldOfView = 25f;
			}
			else if (this.m_FPSWeapon.WeaponID == 112)
			{
				this.m_FPSCamera.RenderingFieldOfView = 30f;
			}
			else if (this.m_FPSWeapon.WeaponID == 176 || this.m_FPSWeapon.WeaponID == 302 || this.m_FPSWeapon.WeaponID == 338)
			{
				this.m_FPSCamera.RenderingFieldOfView = 10f;
			}
			else if (this.m_FPSWeapon.WeaponID == 185)
			{
				this.m_FPSCamera.RenderingFieldOfView = 25f;
			}
			else if (this.m_FPSWeapon.WeaponID == 218)
			{
				this.m_FPSCamera.RenderingFieldOfView = 15f;
			}
			else if (this.m_FPSWeapon.WeaponID == 161)
			{
				this.m_FPSCamera.RenderingFieldOfView = 25f;
			}
			else
			{
				this.m_FPSCamera.RenderingFieldOfView = 45f;
			}
			this.m_FPSCamera.SnapZoom();
			this.m_FPSCamera.m_FinalZoomTime = 0f;
			this.m_WeaponSystem.OnWeaponZoomStart(this.m_FPSWeapon);
			return;
		}
		if (this.m_WeaponSystem.GetAmmoGP() == 0)
		{
			this.Player.Zoom.NextAllowedStopTime = Time.time - 0.1f;
			this.Player.Zoom.TryStop(true);
			return;
		}
		this.m_WeaponSystem.OnWeaponZoomStart(this.m_FPSWeapon);
		this.m_FPSWeapon.AddForce2(this.MotionPositionRecoil * this.MotionDryFireRecoil * -100f, this.MotionRotationRecoil * this.MotionDryFireRecoil * -100f);
		this.m_FPSWeaponReloader.ReloadDuration2 = 1f;
		this.m_FPSWeaponReloader.ReloadStart2 = Time.time;
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x00077600 File Offset: 0x00075800
	protected virtual void OnStop_Zoom()
	{
		this.Player.Zoom.NextAllowedStartTime = Time.time + 0.1f;
		this.Player.Zoom.NextAllowedStopTime = Time.time + 0.1f;
		this.m_FPSCamera.RenderingFieldOfView = 65f;
		this.m_FPSCamera.SnapZoom();
		this.m_FPSCamera.m_FinalZoomTime = 0f;
		if (this.m_WeaponSystem == null)
		{
			this.m_WeaponSystem = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		this.m_WeaponSystem.OnWeaponZoomEnd(this.m_FPSWeapon);
	}

	// Token: 0x04000E20 RID: 3616
	protected WeaponSystem m_WeaponSystem;

	// Token: 0x04000E21 RID: 3617
	protected vp_FPCamera m_FPSCamera;

	// Token: 0x04000E22 RID: 3618
	protected vp_FPWeaponReloader m_FPSWeaponReloader;

	// Token: 0x04000E23 RID: 3619
	protected Minigun MG;

	// Token: 0x04000E24 RID: 3620
	public float ProjectileTapFiringRate = 0.1f;

	// Token: 0x04000E25 RID: 3621
	protected float m_LastFireTime;

	// Token: 0x04000E26 RID: 3622
	public Vector3 MotionPositionRecoil = new Vector3(0f, 0f, -0.035f);

	// Token: 0x04000E27 RID: 3623
	public Vector3 MotionRotationRecoil = new Vector3(-10f, 0f, 0f);

	// Token: 0x04000E28 RID: 3624
	public float MotionRotationRecoilDeadZone = 0.5f;

	// Token: 0x04000E29 RID: 3625
	public float MotionPositionReset = 0.5f;

	// Token: 0x04000E2A RID: 3626
	public float MotionRotationReset = 0.5f;

	// Token: 0x04000E2B RID: 3627
	public float MotionPositionPause = 1f;

	// Token: 0x04000E2C RID: 3628
	public float MotionRotationPause = 1f;

	// Token: 0x04000E2D RID: 3629
	public float MotionDryFireRecoil = -0.1f;

	// Token: 0x04000E2E RID: 3630
	public float MotionRecoilDelay;

	// Token: 0x04000E2F RID: 3631
	public AnimationClip AnimationFire;

	// Token: 0x04000E30 RID: 3632
	public AudioClip SoundDryFire;

	// Token: 0x04000E31 RID: 3633
	public Texture2D ZoomTexture;

	// Token: 0x04000E32 RID: 3634
	public Texture2D ZoomTexture2;

	// Token: 0x04000E33 RID: 3635
	public static bool fpsguidraw;

	// Token: 0x04000E34 RID: 3636
	private TankController tc;

	// Token: 0x04000E35 RID: 3637
	private CarController cc;

	// Token: 0x04000E36 RID: 3638
	private bool activeTC;

	// Token: 0x04000E37 RID: 3639
	private bool activeCC;

	// Token: 0x04000E38 RID: 3640
	private vp_FPPlayerEventHandler m_Player;
}
