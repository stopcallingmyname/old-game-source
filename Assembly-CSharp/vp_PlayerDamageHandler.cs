using System;
using UnityEngine;

// Token: 0x020000E9 RID: 233
[RequireComponent(typeof(vp_FPPlayerEventHandler))]
public class vp_PlayerDamageHandler : vp_DamageHandler
{
	// Token: 0x06000869 RID: 2153 RVA: 0x0007AAE2 File Offset: 0x00078CE2
	protected override void Awake()
	{
		base.Awake();
		this.m_Player = base.transform.GetComponent<vp_FPPlayerEventHandler>();
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x0007AAFB File Offset: 0x00078CFB
	protected virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x0007AB17 File Offset: 0x00078D17
	protected virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x0007AB33 File Offset: 0x00078D33
	public override void Damage(float damage)
	{
		if (!base.enabled)
		{
			return;
		}
		if (!vp_Utility.IsActive(base.gameObject))
		{
			return;
		}
		base.Damage(damage);
		this.m_Player.HUDDamageFlash.Send(damage);
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x0007AB6C File Offset: 0x00078D6C
	public override void Die()
	{
		if (!base.enabled || !vp_Utility.IsActive(base.gameObject))
		{
			return;
		}
		if (this.DeathEffect != null)
		{
			Object.Instantiate<GameObject>(this.DeathEffect, base.transform.position, base.transform.rotation);
		}
		this.m_Player.SetWeapon.Argument = 0;
		this.m_Player.SetWeapon.Start(0f);
		this.m_Player.Dead.Start(0f);
		this.m_Player.AllowGameplayInput.Set(false);
		if (this.Respawns)
		{
			vp_Timer.In(Random.Range(this.MinRespawnTime, this.MaxRespawnTime), new vp_Timer.Callback(this.Respawn), null);
		}
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x0007AC44 File Offset: 0x00078E44
	protected override void Respawn()
	{
		if (this == null)
		{
			return;
		}
		if (Physics.CheckSphere(this.m_StartPosition + Vector3.up * this.m_RespawnOffset, this.RespawnCheckRadius, 1342177280))
		{
			this.m_RespawnOffset += 1f;
			this.Respawn();
			return;
		}
		this.m_RespawnOffset = 0f;
		this.Reactivate();
		this.Reset();
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x0007ACB8 File Offset: 0x00078EB8
	protected override void Reset()
	{
		this.m_CurrentHealth = this.MaxHealth;
		this.m_Player.Position.Set(this.m_StartPosition);
		this.m_Player.Rotation.Set(this.m_StartRotation.eulerAngles);
		this.m_Player.Stop.Send();
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x0007AD28 File Offset: 0x00078F28
	protected override void Reactivate()
	{
		this.m_Player.Dead.Stop(0f);
		this.m_Player.AllowGameplayInput.Set(true);
		this.m_Player.HUDDamageFlash.Send(0f);
		if (this.m_Audio != null)
		{
			this.m_Audio.pitch = Time.timeScale;
			this.m_Audio.PlayOneShot(this.RespawnSound, AudioListener.volume);
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000871 RID: 2161 RVA: 0x0007ADAE File Offset: 0x00078FAE
	// (set) Token: 0x06000872 RID: 2162 RVA: 0x0007ADB6 File Offset: 0x00078FB6
	protected virtual float OnValue_Health
	{
		get
		{
			return this.m_CurrentHealth;
		}
		set
		{
			this.m_CurrentHealth = value;
		}
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x0007ADBF File Offset: 0x00078FBF
	private void Update()
	{
		if (this.m_Player.Dead.Active && Time.timeScale < 1f)
		{
			vp_TimeUtility.FadeTimeScale(1f, 0.05f);
		}
	}

	// Token: 0x04000EDE RID: 3806
	private vp_FPPlayerEventHandler m_Player;

	// Token: 0x04000EDF RID: 3807
	protected float m_RespawnOffset;
}
