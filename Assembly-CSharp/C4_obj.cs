using System;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class C4_obj : MonoBehaviour
{
	// Token: 0x060002BB RID: 699 RVA: 0x00037C80 File Offset: 0x00035E80
	private void Update()
	{
		if (this.m_WeaponSystem == null)
		{
			this.m_WeaponSystem = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		if (this.m_WeaponSystem.GetAmmo(1) > 0)
		{
			this.Show();
			return;
		}
		this.Hide();
	}

	// Token: 0x060002BC RID: 700 RVA: 0x00037CD1 File Offset: 0x00035ED1
	public void Hide()
	{
		base.GetComponent<Renderer>().enabled = false;
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00037CDF File Offset: 0x00035EDF
	public void Show()
	{
		base.GetComponent<Renderer>().enabled = true;
	}

	// Token: 0x04000546 RID: 1350
	private WeaponSystem m_WeaponSystem;
}
