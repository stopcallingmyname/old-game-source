using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D7 RID: 215
public class vp_FPWeaponHandler : MonoBehaviour
{
	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060007C6 RID: 1990 RVA: 0x0007560F File Offset: 0x0007380F
	public vp_FPWeapon CurrentWeapon
	{
		get
		{
			return this.m_CurrentWeapon;
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x060007C7 RID: 1991 RVA: 0x00075617 File Offset: 0x00073817
	public int CurrentWeaponID
	{
		get
		{
			return this.m_CurrentWeaponID;
		}
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x00075620 File Offset: 0x00073820
	protected virtual void Awake()
	{
		if (base.GetComponent<vp_FPWeapon>())
		{
			Debug.LogError("Error: (" + this + ") Hierarchy error. This component should sit above any vp_FPWeapons in the gameobject hierarchy.");
			return;
		}
		this.m_Player = base.transform.GetComponent<vp_FPPlayerEventHandler>();
		foreach (vp_FPWeapon item in base.GetComponentsInChildren<vp_FPWeapon>(true))
		{
			this.m_Weapons.Insert(this.m_Weapons.Count, item);
		}
		if (this.m_Weapons.Count == 0)
		{
			Debug.LogError("Error: (" + this + ") Hierarchy error. This component must be added to a gameobject with vp_FPWeapon components in child gameobjects.");
			return;
		}
		IComparer @object = new vp_FPWeaponHandler.WeaponComparer();
		this.m_Weapons.Sort(new Comparison<vp_FPWeapon>(@object.Compare));
		this.StartWeapon = Mathf.Clamp(this.StartWeapon, 0, this.m_Weapons.Count);
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x000756F0 File Offset: 0x000738F0
	protected virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
		vp_Timer.In(0f, delegate()
		{
			if (base.enabled)
			{
				this.SetWeapon(this.m_CurrentWeaponID);
			}
		}, null);
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x00075723 File Offset: 0x00073923
	protected virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x0007573F File Offset: 0x0007393F
	protected virtual void Update()
	{
		if (this.m_CurrentWeaponID == -1)
		{
			this.m_CurrentWeaponID = this.StartWeapon;
			this.SetWeapon(this.m_CurrentWeaponID);
		}
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x00075764 File Offset: 0x00073964
	public virtual void SetWeapon(int i)
	{
		if (this.m_Weapons.Count < 1)
		{
			Debug.LogError("Error: (" + this + ") Tried to set weapon with an empty weapon list.");
			return;
		}
		if (i < 0 || i > this.m_Weapons.Count)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Error: (",
				this,
				") Weapon list does not have a weapon with index: ",
				i
			}));
			return;
		}
		if (this.m_CurrentWeapon != null)
		{
			this.m_CurrentWeapon.ResetState();
		}
		foreach (vp_FPWeapon vp_FPWeapon in this.m_Weapons)
		{
			vp_FPWeapon.ActivateGameObject(false);
		}
		this.m_CurrentWeaponID = i;
		this.m_CurrentWeapon = null;
		if (this.m_CurrentWeaponID > 0)
		{
			this.m_CurrentWeapon = this.m_Weapons[this.m_CurrentWeaponID - 1];
			if (this.m_CurrentWeapon != null)
			{
				this.m_CurrentWeapon.ActivateGameObject(true);
			}
		}
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x0007587C File Offset: 0x00073A7C
	public virtual void CancelTimers()
	{
		vp_Timer.CancelAll("EjectShell");
		this.m_DisableAttackStateTimer.Cancel();
		this.m_SetWeaponTimer.Cancel();
		this.m_SetWeaponRefreshTimer.Cancel();
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x000758A9 File Offset: 0x00073AA9
	public virtual void SetWeaponLayer(int layer)
	{
		if (this.m_CurrentWeaponID < 1 || this.m_CurrentWeaponID > this.m_Weapons.Count)
		{
			return;
		}
		vp_Layer.Set(this.m_Weapons[this.m_CurrentWeaponID - 1].gameObject, layer, true);
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x000758E7 File Offset: 0x00073AE7
	protected virtual void OnStart_Reload()
	{
		this.m_Player.Attack.Stop(this.m_Player.CurrentWeaponReloadDuration.Get() + this.ReloadAttackSleepDuration);
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x00075918 File Offset: 0x00073B18
	protected virtual void OnStart_SetWeapon()
	{
		this.CancelTimers();
		this.m_Player.Reload.Stop(this.SetWeaponDuration + this.SetWeaponReloadSleepDuration);
		this.m_Player.Zoom.Stop(this.SetWeaponDuration + this.SetWeaponZoomSleepDuration);
		this.m_Player.Attack.Stop(this.SetWeaponDuration + this.SetWeaponAttackSleepDuration);
		if (this.m_CurrentWeapon != null)
		{
			this.m_CurrentWeapon.Wield(false);
		}
		this.m_Player.SetWeapon.AutoDuration = this.SetWeaponDuration;
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x000759B4 File Offset: 0x00073BB4
	protected virtual void OnStop_SetWeapon()
	{
		int weapon = 0;
		if (this.m_Player.SetWeapon.Argument != null)
		{
			weapon = (int)this.m_Player.SetWeapon.Argument;
		}
		this.SetWeapon(weapon);
		if (this.m_CurrentWeapon != null)
		{
			this.m_CurrentWeapon.Wield(true);
		}
		vp_Timer.In(this.SetWeaponRefreshStatesDelay, delegate()
		{
			if (this != null && this.m_Player != null)
			{
				this.m_Player.RefreshActivityStates();
			}
		}, this.m_SetWeaponRefreshTimer);
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00075A2C File Offset: 0x00073C2C
	protected virtual bool CanStart_SetWeapon()
	{
		int num = (int)this.m_Player.SetWeapon.Argument;
		return num != this.m_CurrentWeaponID && num >= 0 && num <= this.m_Weapons.Count;
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x00075A70 File Offset: 0x00073C70
	protected virtual bool CanStart_Attack()
	{
		return !(this.m_CurrentWeapon == null) && !this.m_Player.Attack.Active && !this.m_Player.SetWeapon.Active && !this.m_Player.Reload.Active;
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x00075ACA File Offset: 0x00073CCA
	protected virtual void OnStop_Attack()
	{
		vp_Timer.In(this.AttackStateDisableDelay, delegate()
		{
			if (!this.m_Player.Attack.Active && this.m_CurrentWeapon != null)
			{
				this.m_CurrentWeapon.SetState("Attack", false, false, false);
			}
		}, this.m_DisableAttackStateTimer);
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00075AEC File Offset: 0x00073CEC
	protected virtual bool OnAttempt_SetPrevWeapon()
	{
		int num = this.m_CurrentWeaponID - 1;
		if (num < 1)
		{
			num = this.m_Weapons.Count;
		}
		int num2 = 0;
		while (!this.m_Player.SetWeapon.TryStart<int>(num))
		{
			num--;
			if (num < 1)
			{
				num = this.m_Weapons.Count;
			}
			num2++;
			if (num2 > this.m_Weapons.Count)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x00075B54 File Offset: 0x00073D54
	protected virtual bool OnAttempt_SetNextWeapon()
	{
		int num = this.m_CurrentWeaponID + 1;
		int num2 = 0;
		while (!this.m_Player.SetWeapon.TryStart<int>(num))
		{
			if (num > this.m_Weapons.Count + 1)
			{
				num = 0;
			}
			num++;
			num2++;
			if (num2 > this.m_Weapons.Count)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x00075BAC File Offset: 0x00073DAC
	protected virtual bool OnAttempt_SetWeaponByName(string name)
	{
		for (int i = 0; i < this.m_Weapons.Count; i++)
		{
			if (this.m_Weapons[i].name == name)
			{
				return this.m_Player.SetWeapon.TryStart<int>(i + 1);
			}
		}
		return false;
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x060007D8 RID: 2008 RVA: 0x00075BFD File Offset: 0x00073DFD
	protected virtual bool OnValue_CurrentWeaponWielded
	{
		get
		{
			return !(this.m_CurrentWeapon == null) && this.m_CurrentWeapon.Wielded;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x060007D9 RID: 2009 RVA: 0x00075C1A File Offset: 0x00073E1A
	protected virtual string OnValue_CurrentWeaponName
	{
		get
		{
			if (this.m_CurrentWeapon == null || this.m_Weapons == null)
			{
				return "";
			}
			return this.m_CurrentWeapon.name;
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x060007DA RID: 2010 RVA: 0x00075617 File Offset: 0x00073817
	protected virtual int OnValue_CurrentWeaponID
	{
		get
		{
			return this.m_CurrentWeaponID;
		}
	}

	// Token: 0x04000E02 RID: 3586
	public int StartWeapon;

	// Token: 0x04000E03 RID: 3587
	public float AttackStateDisableDelay = 0.5f;

	// Token: 0x04000E04 RID: 3588
	public float SetWeaponRefreshStatesDelay = 0.5f;

	// Token: 0x04000E05 RID: 3589
	public float SetWeaponDuration = 0.1f;

	// Token: 0x04000E06 RID: 3590
	public float SetWeaponReloadSleepDuration = 0.3f;

	// Token: 0x04000E07 RID: 3591
	public float SetWeaponZoomSleepDuration = 0.3f;

	// Token: 0x04000E08 RID: 3592
	public float SetWeaponAttackSleepDuration = 0.3f;

	// Token: 0x04000E09 RID: 3593
	public float ReloadAttackSleepDuration = 0.3f;

	// Token: 0x04000E0A RID: 3594
	protected vp_FPPlayerEventHandler m_Player;

	// Token: 0x04000E0B RID: 3595
	protected List<vp_FPWeapon> m_Weapons = new List<vp_FPWeapon>();

	// Token: 0x04000E0C RID: 3596
	protected int m_CurrentWeaponID = -1;

	// Token: 0x04000E0D RID: 3597
	protected vp_FPWeapon m_CurrentWeapon;

	// Token: 0x04000E0E RID: 3598
	protected vp_Timer.Handle m_SetWeaponTimer = new vp_Timer.Handle();

	// Token: 0x04000E0F RID: 3599
	protected vp_Timer.Handle m_SetWeaponRefreshTimer = new vp_Timer.Handle();

	// Token: 0x04000E10 RID: 3600
	protected vp_Timer.Handle m_DisableAttackStateTimer = new vp_Timer.Handle();

	// Token: 0x04000E11 RID: 3601
	protected vp_Timer.Handle m_DisableReloadStateTimer = new vp_Timer.Handle();

	// Token: 0x020008B4 RID: 2228
	protected class WeaponComparer : IComparer
	{
		// Token: 0x06004D25 RID: 19749 RVA: 0x001AD9AE File Offset: 0x001ABBAE
		int IComparer.Compare(object x, object y)
		{
			return new CaseInsensitiveComparer().Compare(((vp_FPWeapon)x).gameObject.name, ((vp_FPWeapon)y).gameObject.name);
		}
	}
}
