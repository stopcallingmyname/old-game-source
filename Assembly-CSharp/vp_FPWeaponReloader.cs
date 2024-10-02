using System;
using UnityEngine;

// Token: 0x020000D8 RID: 216
[RequireComponent(typeof(vp_FPWeapon))]
public class vp_FPWeaponReloader : MonoBehaviour
{
	// Token: 0x060007DF RID: 2015 RVA: 0x00075D51 File Offset: 0x00073F51
	protected virtual void Awake()
	{
		this.m_Audio = base.GetComponent<AudioSource>();
		this.m_Player = (vp_FPPlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_FPPlayerEventHandler));
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00075D84 File Offset: 0x00073F84
	protected virtual void Start()
	{
		this.m_Weapon = base.transform.GetComponent<vp_FPWeapon>();
		this.ReloadTex2 = GUIManager.tex_yellow;
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x00075DA2 File Offset: 0x00073FA2
	protected virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x00075DBE File Offset: 0x00073FBE
	protected virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x00075DDC File Offset: 0x00073FDC
	protected virtual bool CanStart_Reload()
	{
		if (!this.m_Player.CurrentWeaponWielded.Get())
		{
			return false;
		}
		if (this.m_WeaponSystem == null)
		{
			this.m_WeaponSystem = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		if (this.m_PlayerControl == null)
		{
			this.m_PlayerControl = (PlayerControl)Object.FindObjectOfType(typeof(PlayerControl));
		}
		if (!this.m_WeaponSystem.OnWeaponReloadStart(this.m_Weapon))
		{
			return false;
		}
		this.ReloadStart = Time.time;
		int team = this.m_PlayerControl.GetTeam();
		if (team == 0)
		{
			this.ReloadTex = GUIManager.tex_blue;
		}
		else if (team == 1)
		{
			this.ReloadTex = GUIManager.tex_red;
		}
		else if (team == 2)
		{
			this.ReloadTex = GUIManager.tex_green;
		}
		else if (team == 3)
		{
			this.ReloadTex = GUIManager.tex_yellow;
		}
		this.m_Player.Zoom.TryStop(true);
		return true;
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x00075ED4 File Offset: 0x000740D4
	protected virtual void OnStart_Reload()
	{
		if (this.sound == null)
		{
			this.sound = (Sound)Object.FindObjectOfType(typeof(Sound));
		}
		this.m_Player.Reload.AutoDuration = this.OnValue_CurrentWeaponReloadDuration;
		if (this.m_Player.Reload.AutoDuration == 0f && this.AnimationReload != null)
		{
			this.m_Player.Reload.AutoDuration = this.AnimationReload.length;
		}
		this.m_WeaponSystem.m_NextAllowedFireTimeOwerride = Time.time + 1.5f;
		if (this.AnimationReload != null)
		{
			this.m_Weapon.WeaponModel.GetComponent<Animation>().CrossFade(this.AnimationReload.name);
		}
		if (this.m_Audio != null)
		{
			if (this.SoundReload == null)
			{
				this.SoundReload = this.sound.GetReload();
			}
			this.m_Audio.pitch = Time.timeScale;
			this.m_Audio.PlayOneShot(this.SoundReload, AudioListener.volume);
		}
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00075FF8 File Offset: 0x000741F8
	protected virtual void OnStop_Reload()
	{
		this.m_Player.CurrentWeaponName.Get();
		if (this.m_WeaponSystem == null)
		{
			this.m_WeaponSystem = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		this.m_WeaponSystem.OnWeaponReloadEnd(this.m_Weapon);
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x060007E6 RID: 2022 RVA: 0x00076054 File Offset: 0x00074254
	protected virtual float OnValue_CurrentWeaponReloadDuration
	{
		get
		{
			return this.ReloadDuration;
		}
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x0007605C File Offset: 0x0007425C
	private void OnGUI()
	{
		if (this.m_Weapon.WeaponID == 122)
		{
			return;
		}
		this.GUIDrawReload();
		this.GUIDrawReload2();
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x0007607C File Offset: 0x0007427C
	private void GUIDrawReload()
	{
		float num = this.ReloadStart + this.ReloadDuration - Time.time;
		if (num < 0f)
		{
			return;
		}
		float num2 = 1f - num / this.ReloadDuration;
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - 66f, (float)Screen.height * 0.75f - 2f, 132f, 12f), GUIManager.tex_black);
		if (this.ReloadTex)
		{
			GUI.DrawTexture(new Rect((float)Screen.width / 2f - 64f, (float)Screen.height * 0.75f, 128f * num2, 8f), this.ReloadTex);
		}
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x0007613C File Offset: 0x0007433C
	private void GUIDrawReload2()
	{
		if (this.ReloadStart2 == 0f)
		{
			return;
		}
		float num = this.ReloadStart2 + this.ReloadDuration2 - Time.time;
		if (num < 0f && this.ReloadStart2 > 0f)
		{
			this.m_Player.Zoom.NextAllowedStopTime = Time.time - 0.1f;
			this.m_Player.Zoom.TryStop(true);
			this.ReloadStart2 = 0f;
			return;
		}
		float num2 = 1f - num / this.ReloadDuration2;
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - 66f, (float)Screen.height * 0.75f - 2f + 14f, 132f, 12f), GUIManager.tex_black);
		if (this.ReloadTex2)
		{
			GUI.DrawTexture(new Rect((float)Screen.width / 2f - 64f, (float)Screen.height * 0.75f + 14f, 128f * num2, 8f), this.ReloadTex2);
		}
	}

	// Token: 0x04000E12 RID: 3602
	protected WeaponSystem m_WeaponSystem;

	// Token: 0x04000E13 RID: 3603
	protected PlayerControl m_PlayerControl;

	// Token: 0x04000E14 RID: 3604
	protected vp_FPWeapon m_Weapon;

	// Token: 0x04000E15 RID: 3605
	protected vp_FPPlayerEventHandler m_Player;

	// Token: 0x04000E16 RID: 3606
	protected AudioSource m_Audio;

	// Token: 0x04000E17 RID: 3607
	public AudioClip SoundReload;

	// Token: 0x04000E18 RID: 3608
	public AnimationClip AnimationReload;

	// Token: 0x04000E19 RID: 3609
	public float ReloadDuration = 2f;

	// Token: 0x04000E1A RID: 3610
	private float ReloadStart;

	// Token: 0x04000E1B RID: 3611
	private Texture2D ReloadTex;

	// Token: 0x04000E1C RID: 3612
	public float ReloadDuration2 = 1f;

	// Token: 0x04000E1D RID: 3613
	public float ReloadStart2;

	// Token: 0x04000E1E RID: 3614
	private Texture2D ReloadTex2;

	// Token: 0x04000E1F RID: 3615
	private Sound sound;
}
