using System;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public class vp_DamageHandler : MonoBehaviour
{
	// Token: 0x06000817 RID: 2071 RVA: 0x0007835E File Offset: 0x0007655E
	protected virtual void Awake()
	{
		this.m_Audio = base.GetComponent<AudioSource>();
		this.m_CurrentHealth = this.MaxHealth;
		this.m_StartPosition = base.transform.position;
		this.m_StartRotation = base.transform.rotation;
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x0007839C File Offset: 0x0007659C
	public virtual void Damage(float damage)
	{
		if (!base.enabled)
		{
			return;
		}
		if (!vp_Utility.IsActive(base.gameObject))
		{
			return;
		}
		if (this.m_CurrentHealth <= 0f)
		{
			return;
		}
		this.m_CurrentHealth = Mathf.Min(this.m_CurrentHealth - damage, this.MaxHealth);
		if (this.m_CurrentHealth <= 0f)
		{
			if (this.m_Audio != null)
			{
				if (this.sound == null)
				{
					this.sound = (Sound)Object.FindObjectOfType(typeof(Sound));
				}
				if (this.DeathSound == null)
				{
					this.DeathSound = this.sound.GetDeath();
				}
				this.m_Audio.pitch = Time.timeScale;
				this.m_Audio.PlayOneShot(this.DeathSound, AudioListener.volume);
			}
			vp_Timer.In(Random.Range(this.MinDeathDelay, this.MaxDeathDelay), new vp_Timer.Callback(this.Die), null);
			return;
		}
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x00078498 File Offset: 0x00076698
	public virtual void Die()
	{
		if (!base.enabled || !vp_Utility.IsActive(base.gameObject))
		{
			return;
		}
		this.RemoveBulletHoles();
		vp_Utility.Activate(base.gameObject, false);
		if (this.DeathEffect != null)
		{
			Object.Instantiate<GameObject>(this.DeathEffect, base.transform.position, base.transform.rotation);
		}
		if (this.Respawns)
		{
			vp_Timer.In(Random.Range(this.MinRespawnTime, this.MaxRespawnTime), new vp_Timer.Callback(this.Respawn), null);
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x00078538 File Offset: 0x00076738
	protected virtual void Respawn()
	{
		if (this == null)
		{
			return;
		}
		if (Physics.CheckSphere(this.m_StartPosition, this.RespawnCheckRadius, 1342177280))
		{
			vp_Timer.In(Random.Range(this.MinRespawnTime, this.MaxRespawnTime), new vp_Timer.Callback(this.Respawn), null);
			return;
		}
		this.Reset();
		this.Reactivate();
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x00078598 File Offset: 0x00076798
	protected virtual void Reset()
	{
		this.m_CurrentHealth = this.MaxHealth;
		base.transform.position = this.m_StartPosition;
		base.transform.rotation = this.m_StartRotation;
		if (base.GetComponent<Rigidbody>() != null)
		{
			base.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			base.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x00078604 File Offset: 0x00076804
	protected virtual void Reactivate()
	{
		vp_Utility.Activate(base.gameObject, true);
		if (this.m_Audio != null)
		{
			this.m_Audio.pitch = Time.timeScale;
			this.m_Audio.PlayOneShot(this.RespawnSound, AudioListener.volume);
		}
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x00078654 File Offset: 0x00076854
	protected virtual void RemoveBulletHoles()
	{
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			Component[] components = transform.GetComponents<vp_HitscanBullet>();
			if (components.Length != 0)
			{
				Object.Destroy(transform.gameObject);
			}
		}
	}

	// Token: 0x04000E5D RID: 3677
	public float MaxHealth = 1f;

	// Token: 0x04000E5E RID: 3678
	public GameObject DeathEffect;

	// Token: 0x04000E5F RID: 3679
	public float MinDeathDelay;

	// Token: 0x04000E60 RID: 3680
	public float MaxDeathDelay;

	// Token: 0x04000E61 RID: 3681
	public float m_CurrentHealth;

	// Token: 0x04000E62 RID: 3682
	public bool Respawns = true;

	// Token: 0x04000E63 RID: 3683
	public float MinRespawnTime = 3f;

	// Token: 0x04000E64 RID: 3684
	public float MaxRespawnTime = 3f;

	// Token: 0x04000E65 RID: 3685
	public float RespawnCheckRadius = 1f;

	// Token: 0x04000E66 RID: 3686
	protected AudioSource m_Audio;

	// Token: 0x04000E67 RID: 3687
	public AudioClip DeathSound;

	// Token: 0x04000E68 RID: 3688
	public AudioClip RespawnSound;

	// Token: 0x04000E69 RID: 3689
	protected Vector3 m_StartPosition;

	// Token: 0x04000E6A RID: 3690
	protected Quaternion m_StartRotation;

	// Token: 0x04000E6B RID: 3691
	public Sound sound;
}
