using System;
using UnityEngine;

// Token: 0x0200006F RID: 111
public class RPGRocket : MonoBehaviour
{
	// Token: 0x06000333 RID: 819 RVA: 0x00039DD0 File Offset: 0x00037FD0
	private void Update()
	{
		if (this.m_WeaponSystem == null)
		{
			this.m_WeaponSystem = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		if (this.m_WeaponSystem.GetAmmoRPGClip() > 0)
		{
			this.Show();
			return;
		}
		this.Hide();
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00037CD1 File Offset: 0x00035ED1
	public void Hide()
	{
		base.GetComponent<Renderer>().enabled = false;
	}

	// Token: 0x06000335 RID: 821 RVA: 0x00037CDF File Offset: 0x00035EDF
	public void Show()
	{
		base.GetComponent<Renderer>().enabled = true;
	}

	// Token: 0x040005DD RID: 1501
	private WeaponSystem m_WeaponSystem;
}
