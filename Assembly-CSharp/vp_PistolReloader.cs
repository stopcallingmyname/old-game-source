using System;
using UnityEngine;

// Token: 0x020000E8 RID: 232
public class vp_PistolReloader : vp_FPWeaponReloader
{
	// Token: 0x06000863 RID: 2147 RVA: 0x0007A8D4 File Offset: 0x00078AD4
	protected override void OnStart_Reload()
	{
		if (this.m_Weapon.gameObject != base.gameObject)
		{
			return;
		}
		if (this.m_Timer.Active)
		{
			return;
		}
		base.OnStart_Reload();
		vp_Timer.In(0.4f, delegate()
		{
			if (!vp_Utility.IsActive(this.m_Weapon.gameObject))
			{
				return;
			}
			if (!this.m_Weapon.StateEnabled("Reload"))
			{
				return;
			}
			this.m_Weapon.AddForce2(new Vector3(0f, 0.05f, 0f), new Vector3(0f, 0f, 0f));
			vp_Timer.In(0.15f, delegate()
			{
				if (!vp_Utility.IsActive(this.m_Weapon.gameObject))
				{
					return;
				}
				if (!this.m_Weapon.StateEnabled("Reload"))
				{
					return;
				}
				this.m_Weapon.SetState("Reload", false, false, false);
				this.m_Weapon.SetState("Reload2", true, false, false);
				this.m_Weapon.RotationOffset.z = 0f;
				this.m_Weapon.Refresh();
				vp_Timer.In(0.35f, delegate()
				{
					if (!vp_Utility.IsActive(this.m_Weapon.gameObject))
					{
						return;
					}
					if (!this.m_Weapon.StateEnabled("Reload2"))
					{
						return;
					}
					this.m_Weapon.AddForce2(new Vector3(0f, 0f, -0.05f), new Vector3(5f, 0f, 0f));
					vp_Timer.In(0.1f, delegate()
					{
						this.m_Weapon.SetState("Reload2", false, false, false);
					}, null);
				}, null);
			}, null);
		}, this.m_Timer);
	}

	// Token: 0x04000EDD RID: 3805
	private vp_Timer.Handle m_Timer = new vp_Timer.Handle();
}
