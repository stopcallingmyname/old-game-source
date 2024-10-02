using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EA RID: 234
public class vp_PrototypeMeleeAttack : vp_Component
{
	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06000875 RID: 2165 RVA: 0x0007ADF6 File Offset: 0x00078FF6
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

	// Token: 0x06000876 RID: 2166 RVA: 0x0007AE2C File Offset: 0x0007902C
	protected override void Start()
	{
		base.Start();
		this.m_Controller = (vp_FPController)base.Root.GetComponent(typeof(vp_FPController));
		this.m_Camera = (vp_FPCamera)base.Root.GetComponentInChildren(typeof(vp_FPCamera));
		this.m_Weapon = (vp_FPWeapon)base.Transform.GetComponent(typeof(vp_FPWeapon));
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x0007AE9F File Offset: 0x0007909F
	protected override void Update()
	{
		base.Update();
		this.UpdateAttack();
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x0007AEB0 File Offset: 0x000790B0
	private void UpdateAttack()
	{
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
		if (Time.time < this.m_NextAllowedSwingTime)
		{
			return;
		}
		this.m_NextAllowedSwingTime = Time.time + this.SwingRate;
		this.PickAttack();
		vp_Timer.In(this.SwingDelay, delegate()
		{
			if (this.SoundSwing.Count > 0)
			{
				base.Audio.pitch = Random.Range(this.SoundSwingPitch.x, this.SoundSwingPitch.y) * Time.timeScale;
				base.Audio.PlayOneShot((AudioClip)this.SoundSwing[Random.Range(0, this.SoundSwing.Count)], AudioListener.volume);
			}
			this.m_Weapon.SetState(this.WeaponStateCharge, false, false, false);
			this.m_Weapon.SetState(this.WeaponStateSwing, true, false, false);
			this.m_Weapon.Refresh();
			this.m_Weapon.AddSoftForce(this.SwingPositionSoftForce, this.SwingRotationSoftForce, this.SwingSoftForceFrames);
			vp_Timer.In(this.ImpactTime, delegate()
			{
				RaycastHit hit;
				Physics.SphereCast(new Ray(new Vector3(this.m_Controller.Transform.position.x, this.m_Camera.Transform.position.y, this.m_Controller.Transform.position.z), this.m_Camera.Transform.forward), this.DamageRadius, out hit, this.DamageRange, -1811939349);
				if (hit.collider != null)
				{
					this.SpawnImpactFX(hit);
					this.ApplyDamage(hit);
					this.ApplyRecoil();
					return;
				}
				vp_Timer.In(this.SwingDuration - this.ImpactTime, delegate()
				{
					this.m_Weapon.StopSprings();
					this.Reset();
				}, this.SwingDurationTimer);
			}, this.ImpactTimer);
		}, this.SwingDelayTimer);
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x0007AF44 File Offset: 0x00079144
	private void PickAttack()
	{
		int num;
		do
		{
			num = Random.Range(0, this.States.Count - 1);
		}
		while (this.States.Count > 1 && num == this.m_AttackCurrent && Random.value < 0.5f);
		this.m_AttackCurrent = num;
		base.SetState(this.States[this.m_AttackCurrent].Name, true, false, false);
		this.m_Weapon.SetState(this.WeaponStateCharge, true, false, false);
		this.m_Weapon.Refresh();
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x0007AFD0 File Offset: 0x000791D0
	private void SpawnImpactFX(RaycastHit hit)
	{
		Quaternion rotation = Quaternion.LookRotation(hit.normal);
		if (this.m_DustPrefab != null)
		{
			Object.Instantiate<GameObject>(this.m_DustPrefab, hit.point, rotation);
		}
		if (this.m_SparkPrefab != null && Random.value < this.SparkFactor)
		{
			Object.Instantiate<GameObject>(this.m_SparkPrefab, hit.point, rotation);
		}
		if (this.m_DebrisPrefab != null)
		{
			Object.Instantiate<GameObject>(this.m_DebrisPrefab, hit.point, rotation);
		}
		if (this.SoundImpact.Count > 0)
		{
			base.Audio.pitch = Random.Range(this.SoundImpactPitch.x, this.SoundImpactPitch.y) * Time.timeScale;
			base.Audio.PlayOneShot((AudioClip)this.SoundImpact[Random.Range(0, this.SoundImpact.Count)], AudioListener.volume);
		}
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x0007B0C8 File Offset: 0x000792C8
	private void ApplyDamage(RaycastHit hit)
	{
		hit.collider.SendMessage(this.DamageMethodName, this.Damage, SendMessageOptions.DontRequireReceiver);
		Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
		if (attachedRigidbody != null && !attachedRigidbody.isKinematic)
		{
			attachedRigidbody.AddForceAtPosition(this.m_Camera.Transform.forward * this.DamageForce / Time.timeScale, hit.point);
		}
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x0007B144 File Offset: 0x00079344
	private void ApplyRecoil()
	{
		this.m_Weapon.StopSprings();
		this.m_Weapon.AddForce(this.ImpactPositionSpringRecoil, this.ImpactRotationSpringRecoil);
		this.m_Weapon.AddForce2(this.ImpactPositionSpring2Recoil, this.ImpactRotationSpring2Recoil);
		this.Reset();
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x0007B190 File Offset: 0x00079390
	private void Reset()
	{
		vp_Timer.In(0.05f, delegate()
		{
			if (this.m_Weapon != null)
			{
				this.m_Weapon.SetState(this.WeaponStateCharge, false, false, false);
				this.m_Weapon.SetState(this.WeaponStateSwing, false, false, false);
				this.m_Weapon.Refresh();
				base.ResetState();
			}
		}, this.ResetTimer);
	}

	// Token: 0x04000EE0 RID: 3808
	private vp_FPWeapon m_Weapon;

	// Token: 0x04000EE1 RID: 3809
	private vp_FPController m_Controller;

	// Token: 0x04000EE2 RID: 3810
	private vp_FPCamera m_Camera;

	// Token: 0x04000EE3 RID: 3811
	public string WeaponStateCharge = "Charge";

	// Token: 0x04000EE4 RID: 3812
	public string WeaponStateSwing = "Swing";

	// Token: 0x04000EE5 RID: 3813
	public float SwingDelay = 0.5f;

	// Token: 0x04000EE6 RID: 3814
	public float SwingDuration = 0.5f;

	// Token: 0x04000EE7 RID: 3815
	public float SwingRate = 1f;

	// Token: 0x04000EE8 RID: 3816
	protected float m_NextAllowedSwingTime;

	// Token: 0x04000EE9 RID: 3817
	public int SwingSoftForceFrames = 50;

	// Token: 0x04000EEA RID: 3818
	public Vector3 SwingPositionSoftForce = new Vector3(-0.5f, -0.1f, 0.3f);

	// Token: 0x04000EEB RID: 3819
	public Vector3 SwingRotationSoftForce = new Vector3(50f, -25f, 0f);

	// Token: 0x04000EEC RID: 3820
	public float ImpactTime = 0.11f;

	// Token: 0x04000EED RID: 3821
	public Vector3 ImpactPositionSpringRecoil = new Vector3(0.01f, 0.03f, -0.05f);

	// Token: 0x04000EEE RID: 3822
	public Vector3 ImpactPositionSpring2Recoil = Vector3.zero;

	// Token: 0x04000EEF RID: 3823
	public Vector3 ImpactRotationSpringRecoil = Vector3.zero;

	// Token: 0x04000EF0 RID: 3824
	public Vector3 ImpactRotationSpring2Recoil = new Vector3(0f, 0f, 10f);

	// Token: 0x04000EF1 RID: 3825
	public string DamageMethodName = "Damage";

	// Token: 0x04000EF2 RID: 3826
	public float Damage = 5f;

	// Token: 0x04000EF3 RID: 3827
	public float DamageRadius = 0.3f;

	// Token: 0x04000EF4 RID: 3828
	public float DamageRange = 2f;

	// Token: 0x04000EF5 RID: 3829
	public float DamageForce = 1000f;

	// Token: 0x04000EF6 RID: 3830
	protected int m_AttackCurrent;

	// Token: 0x04000EF7 RID: 3831
	public float SparkFactor = 0.1f;

	// Token: 0x04000EF8 RID: 3832
	public GameObject m_DustPrefab;

	// Token: 0x04000EF9 RID: 3833
	public GameObject m_SparkPrefab;

	// Token: 0x04000EFA RID: 3834
	public GameObject m_DebrisPrefab;

	// Token: 0x04000EFB RID: 3835
	public List<Object> SoundSwing = new List<Object>();

	// Token: 0x04000EFC RID: 3836
	public List<Object> SoundImpact = new List<Object>();

	// Token: 0x04000EFD RID: 3837
	public Vector2 SoundSwingPitch = new Vector2(0.5f, 1.5f);

	// Token: 0x04000EFE RID: 3838
	public Vector2 SoundImpactPitch = new Vector2(1f, 1.5f);

	// Token: 0x04000EFF RID: 3839
	private vp_Timer.Handle SwingDelayTimer = new vp_Timer.Handle();

	// Token: 0x04000F00 RID: 3840
	private vp_Timer.Handle ImpactTimer = new vp_Timer.Handle();

	// Token: 0x04000F01 RID: 3841
	private vp_Timer.Handle SwingDurationTimer = new vp_Timer.Handle();

	// Token: 0x04000F02 RID: 3842
	private vp_Timer.Handle ResetTimer = new vp_Timer.Handle();

	// Token: 0x04000F03 RID: 3843
	private vp_FPPlayerEventHandler m_Player;
}
