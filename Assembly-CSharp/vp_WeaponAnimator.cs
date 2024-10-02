using System;
using UnityEngine;

// Token: 0x0200009F RID: 159
public class vp_WeaponAnimator : vp_Component
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000543 RID: 1347 RVA: 0x00062366 File Offset: 0x00060566
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

	// Token: 0x06000544 RID: 1348 RVA: 0x0006239B File Offset: 0x0006059B
	protected override void Start()
	{
		base.Start();
		this.m_Weapon = (vp_FPWeapon)base.Transform.GetComponent(typeof(vp_FPWeapon));
		this.m_Input = base.transform.root.GetComponentInChildren<vp_FPInput>();
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x000623D9 File Offset: 0x000605D9
	protected override void Update()
	{
		base.Update();
		this.UpdateAttack();
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x000623E8 File Offset: 0x000605E8
	public override void Activate()
	{
		base.Activate();
		this.m_ForceAttack = false;
		this.m_ForceExit = false;
		if (!ItemsDB.CheckItem(this.m_Weapon.WeaponID))
		{
			return;
		}
		if (ItemsDB.Items[this.m_Weapon.WeaponID].Category == 20)
		{
			this.m_ForceAttack = true;
		}
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x00062440 File Offset: 0x00060640
	private void UpdateAttack()
	{
		if (this.m_ForceExit && Time.time > this.m_NextAllowedTime)
		{
			this.m_ForceExit = false;
			this.m_Input.RestoreSetWeapon();
		}
		if (this.m_ForceAttack)
		{
			this.Player.Attack.TryStart(true);
		}
		if (!this.Player.Attack.Active)
		{
			return;
		}
		if (this.Player.SetWeapon.Active)
		{
			return;
		}
		if (this.m_Weapon == null)
		{
			return;
		}
		if (!this.m_Weapon.Wielded)
		{
			return;
		}
		if (Time.time < this.m_NextAllowedTime)
		{
			return;
		}
		if (this.m_ForceAttack)
		{
			this.m_ForceAttack = false;
			this.m_ForceExit = true;
		}
		if (this.m_WeaponSystem == null)
		{
			this.m_WeaponSystem = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		if (!this.m_WeaponSystem.OnWeaponMeleeAttack(this.m_Weapon))
		{
			return;
		}
		this.m_NextAllowedTime = Time.time + this.nextattack;
		this.m_Weapon.WeaponModel.GetComponent<Animation>().Stop();
		this.m_Weapon.WeaponModel.GetComponent<Animation>().Play(this.animation_name);
		if (this.soundattack)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.soundattack, AudioListener.volume);
		}
	}

	// Token: 0x040009B5 RID: 2485
	protected WeaponSystem m_WeaponSystem;

	// Token: 0x040009B6 RID: 2486
	protected vp_FPInput m_Input;

	// Token: 0x040009B7 RID: 2487
	public string animation_name = "";

	// Token: 0x040009B8 RID: 2488
	public float nextattack = 0.2f;

	// Token: 0x040009B9 RID: 2489
	public AudioClip soundattack;

	// Token: 0x040009BA RID: 2490
	private vp_FPWeapon m_Weapon;

	// Token: 0x040009BB RID: 2491
	private float m_NextAllowedTime;

	// Token: 0x040009BC RID: 2492
	private bool m_ForceAttack;

	// Token: 0x040009BD RID: 2493
	private bool m_ForceExit;

	// Token: 0x040009BE RID: 2494
	private vp_FPPlayerEventHandler m_Player;
}
