using System;
using UnityEngine;

// Token: 0x020000DA RID: 218
[RequireComponent(typeof(AudioSource))]
public class vp_Shooter : vp_Component
{
	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0007773C File Offset: 0x0007593C
	public GameObject MuzzleFlash
	{
		get
		{
			return this.m_MuzzleFlash;
		}
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x00077744 File Offset: 0x00075944
	protected override void Awake()
	{
		base.Awake();
		this.m_OperatorTransform = base.Transform;
		this.m_CharacterController = this.m_OperatorTransform.root.GetComponentInChildren<CharacterController>();
		if (ItemsDB.CheckItem(this.m_FPSWeapon.WeaponID) && ItemsDB.Items[this.m_FPSWeapon.WeaponID].Upgrades != null && ItemsDB.Items[this.m_FPSWeapon.WeaponID].Upgrades[4][0] != null)
		{
			this.ProjectileROF = 60f / (float)ItemsDB.Items[this.m_FPSWeapon.WeaponID].Upgrades[4][0].Val;
		}
		this.m_NextAllowedFireTime = Time.time;
		this.m_SecondNextAllowedFireTime = Time.time;
		this.ProjectileSpawnDelay = Mathf.Min(this.ProjectileSpawnDelay, this.ProjectileFiringRate - 0.1f);
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x00077820 File Offset: 0x00075A20
	protected override void Start()
	{
		base.Start();
		if (this.MuzzleFlashPrefab != null)
		{
			this.m_MuzzleFlash = Object.Instantiate<GameObject>(this.MuzzleFlashPrefab, this.m_OperatorTransform.position, this.m_OperatorTransform.rotation);
			this.m_MuzzleFlash.name = base.transform.name + "MuzzleFlash";
			this.m_MuzzleFlash.transform.parent = this.m_OperatorTransform;
		}
		base.Audio.playOnAwake = false;
		base.Audio.dopplerLevel = 0f;
		base.RefreshDefaultState();
		this.Refresh();
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x000778C6 File Offset: 0x00075AC6
	public virtual void TryFire()
	{
		if (this.secondFire)
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
		this.Fire();
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x000778F4 File Offset: 0x00075AF4
	protected virtual void Fire()
	{
		if (this.ProjectileROF < 0f && ItemsDB.CheckItem(this.m_FPSWeapon.WeaponID) && ItemsDB.Items[this.m_FPSWeapon.WeaponID].Upgrades != null && ItemsDB.Items[this.m_FPSWeapon.WeaponID].Upgrades[4][0] != null)
		{
			this.ProjectileROF = 60f / (float)ItemsDB.Items[this.m_FPSWeapon.WeaponID].Upgrades[4][0].Val;
		}
		if (this.ProjectileROF > 0f)
		{
			this.ProjectileFiringRate = this.ProjectileROF;
		}
		if (!this.secondFire)
		{
			float nextAllowedFireTime = this.m_NextAllowedFireTime;
			this.m_NextAllowedFireTime = Time.time + this.ProjectileFiringRate;
		}
		else
		{
			this.m_SecondNextAllowedFireTime = Time.time + this.ProjectileFiringRate;
		}
		if (this.SoundFireDelay == 0f)
		{
			this.PlayFireSound();
		}
		else
		{
			vp_Timer.In(this.SoundFireDelay, new vp_Timer.Callback(this.PlayFireSound), null);
		}
		if (this.ProjectileSpawnDelay == 0f)
		{
			this.SpawnProjectiles();
		}
		else
		{
			vp_Timer.In(this.ProjectileSpawnDelay, new vp_Timer.Callback(this.SpawnProjectiles), null);
		}
		if (this.ShellEjectDelay == 0f)
		{
			this.EjectShell();
		}
		else
		{
			vp_Timer.In(this.ShellEjectDelay, new vp_Timer.Callback(this.EjectShell), null);
		}
		if (this.MuzzleFlashDelay == 0f)
		{
			this.ShowMuzzleFlash();
			return;
		}
		vp_Timer.In(this.MuzzleFlashDelay, new vp_Timer.Callback(this.ShowMuzzleFlash), null);
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x00077A88 File Offset: 0x00075C88
	protected virtual void PlayFireSound()
	{
		if (base.Audio == null)
		{
			return;
		}
		if (this.sound == null)
		{
			this.sound = (Sound)Object.FindObjectOfType(typeof(Sound));
		}
		if (this.SoundFire == null)
		{
			this.SoundFire = this.sound.ReturnSound_Weapon(base.GetComponent<vp_FPWeapon>().WeaponID);
		}
		if (this.currentFiringWeaponID != 315)
		{
			base.Audio.pitch = Random.Range(this.SoundFirePitch.x, this.SoundFirePitch.y) * Time.timeScale;
			base.Audio.clip = this.SoundFire;
			base.Audio.Play();
			return;
		}
		this.flameFiringTimer = Time.time + 0.15f;
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x00077B60 File Offset: 0x00075D60
	protected override void Update()
	{
		base.Update();
		if (this.currentFiringWeaponID != 315)
		{
			return;
		}
		if (this.flameFiringTimer < Time.time)
		{
			if (base.Audio.isPlaying && base.Audio.clip != SoundManager.weapon_flamethrower_end)
			{
				base.Audio.pitch = 1f;
				base.Audio.loop = false;
				base.Audio.clip = SoundManager.weapon_flamethrower_end;
				base.Audio.Play();
			}
			return;
		}
		if (!base.Audio.isPlaying && base.Audio.clip != SoundManager.weapon_flamethrower_start)
		{
			base.Audio.pitch = 1f;
			base.Audio.loop = false;
			base.Audio.clip = SoundManager.weapon_flamethrower_start;
			base.Audio.Play();
			vp_Timer.In(SoundManager.weapon_flamethrower_start.length - 0.1f, delegate()
			{
				if (this.flameFiringTimer > Time.time && base.Audio.clip != this.SoundFire)
				{
					base.Audio.pitch = 1f;
					base.Audio.loop = true;
					base.Audio.clip = this.SoundFire;
					base.Audio.Play();
				}
			}, null);
		}
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x00077C68 File Offset: 0x00075E68
	protected virtual void SpawnProjectiles()
	{
		for (int i = 0; i < this.ProjectileCount; i++)
		{
			if (this.ProjectilePrefab != null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.ProjectilePrefab, this.m_OperatorTransform.position, this.m_OperatorTransform.rotation);
				gameObject.transform.localScale = new Vector3(this.ProjectileScale, this.ProjectileScale, this.ProjectileScale);
				gameObject.transform.Rotate(0f, 0f, (float)Random.Range(0, 360));
				gameObject.transform.Rotate(0f, Random.Range(-this.ProjectileSpread, this.ProjectileSpread), 0f);
			}
		}
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x00077D27 File Offset: 0x00075F27
	protected virtual void ShowMuzzleFlash()
	{
		if (this.m_MuzzleFlash == null)
		{
			return;
		}
		this.m_MuzzleFlash.SendMessage("Shoot", SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x00077D4C File Offset: 0x00075F4C
	protected virtual void EjectShell()
	{
		if (this.ShellPrefab == null)
		{
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.ShellPrefab, this.m_OperatorTransform.position + this.m_OperatorTransform.TransformDirection(this.ShellEjectPosition), this.m_OperatorTransform.rotation);
		gameObject.transform.localScale = new Vector3(this.ShellScale, this.ShellScale, this.ShellScale);
		vp_Layer.Set(gameObject.gameObject, 29, false);
		if (gameObject.GetComponent<Rigidbody>())
		{
			Vector3 force = base.transform.TransformDirection(this.ShellEjectDirection) * this.ShellEjectVelocity;
			gameObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
		}
		if (this.m_CharacterController)
		{
			Vector3 velocity = this.m_CharacterController.velocity;
			gameObject.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
		}
		if (this.ShellEjectSpin > 0f)
		{
			if (Random.value > 0.5f)
			{
				gameObject.GetComponent<Rigidbody>().AddRelativeTorque(-Random.rotation.eulerAngles * this.ShellEjectSpin);
				return;
			}
			gameObject.GetComponent<Rigidbody>().AddRelativeTorque(Random.rotation.eulerAngles * this.ShellEjectSpin);
		}
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x00077E94 File Offset: 0x00076094
	public virtual void DisableFiring(float seconds = 10000000f)
	{
		this.m_NextAllowedFireTime = Time.time + seconds;
		this.m_SecondNextAllowedFireTime = Time.time + seconds;
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x00077EB0 File Offset: 0x000760B0
	public virtual void EnableFiring()
	{
		this.m_NextAllowedFireTime = Time.time;
		this.m_SecondNextAllowedFireTime = Time.time;
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x00077EC8 File Offset: 0x000760C8
	public override void Refresh()
	{
		if (this.m_MuzzleFlash != null)
		{
			this.m_MuzzleFlash.transform.localPosition = this.MuzzleFlashPosition;
			this.m_MuzzleFlash.transform.localScale = this.MuzzleFlashScale;
			this.m_MuzzleFlash.SendMessage("SetFadeSpeed", this.MuzzleFlashFadeSpeed, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x00077F2B File Offset: 0x0007612B
	public override void Activate()
	{
		base.Activate();
		if (this.m_MuzzleFlash != null)
		{
			vp_Utility.Activate(this.m_MuzzleFlash, true);
		}
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x00077F4D File Offset: 0x0007614D
	public override void Deactivate()
	{
		base.Deactivate();
		if (this.m_MuzzleFlash != null)
		{
			vp_Utility.Activate(this.m_MuzzleFlash, false);
		}
	}

	// Token: 0x04000E39 RID: 3641
	protected CharacterController m_CharacterController;

	// Token: 0x04000E3A RID: 3642
	protected Transform m_OperatorTransform;

	// Token: 0x04000E3B RID: 3643
	public vp_FPWeapon m_FPSWeapon;

	// Token: 0x04000E3C RID: 3644
	public GameObject ProjectilePrefab;

	// Token: 0x04000E3D RID: 3645
	public float ProjectileScale = 1f;

	// Token: 0x04000E3E RID: 3646
	public float ProjectileFiringRate = 0.3f;

	// Token: 0x04000E3F RID: 3647
	public float ProjectileROF = -1f;

	// Token: 0x04000E40 RID: 3648
	public float ProjectileSpawnDelay;

	// Token: 0x04000E41 RID: 3649
	public int ProjectileCount = 1;

	// Token: 0x04000E42 RID: 3650
	public float ProjectileSpread;

	// Token: 0x04000E43 RID: 3651
	protected float m_NextAllowedFireTime;

	// Token: 0x04000E44 RID: 3652
	protected float m_SecondNextAllowedFireTime;

	// Token: 0x04000E45 RID: 3653
	protected bool secondFire;

	// Token: 0x04000E46 RID: 3654
	protected int currentFiringWeaponID;

	// Token: 0x04000E47 RID: 3655
	protected float flameFiringTimer;

	// Token: 0x04000E48 RID: 3656
	public Vector3 MuzzleFlashPosition = Vector3.zero;

	// Token: 0x04000E49 RID: 3657
	public Vector3 MuzzleFlashScale = Vector3.one;

	// Token: 0x04000E4A RID: 3658
	public float MuzzleFlashFadeSpeed = 0.075f;

	// Token: 0x04000E4B RID: 3659
	public GameObject MuzzleFlashPrefab;

	// Token: 0x04000E4C RID: 3660
	public float MuzzleFlashDelay;

	// Token: 0x04000E4D RID: 3661
	protected GameObject m_MuzzleFlash;

	// Token: 0x04000E4E RID: 3662
	public GameObject ShellPrefab;

	// Token: 0x04000E4F RID: 3663
	public float ShellScale = 1f;

	// Token: 0x04000E50 RID: 3664
	public Vector3 ShellEjectDirection = new Vector3(1f, 1f, 1f);

	// Token: 0x04000E51 RID: 3665
	public Vector3 ShellEjectPosition = new Vector3(1f, 0f, 1f);

	// Token: 0x04000E52 RID: 3666
	public float ShellEjectVelocity = 0.2f;

	// Token: 0x04000E53 RID: 3667
	public float ShellEjectDelay;

	// Token: 0x04000E54 RID: 3668
	public float ShellEjectSpin;

	// Token: 0x04000E55 RID: 3669
	public AudioClip SoundFire;

	// Token: 0x04000E56 RID: 3670
	public float SoundFireDelay;

	// Token: 0x04000E57 RID: 3671
	public Vector2 SoundFirePitch = new Vector2(1f, 1f);

	// Token: 0x04000E58 RID: 3672
	public Sound sound;
}
