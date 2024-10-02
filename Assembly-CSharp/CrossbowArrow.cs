using System;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class CrossbowArrow : MonoBehaviour
{
	// Token: 0x060002BF RID: 703 RVA: 0x00037CF0 File Offset: 0x00035EF0
	private void Update()
	{
		if (this.m_WeaponSystem == null)
		{
			this.m_WeaponSystem = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		if (this.m_WeaponSystem.GetPrimaryAmmo() > 0)
		{
			this.Show();
			return;
		}
		this.Hide();
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x00037CD1 File Offset: 0x00035ED1
	public void Hide()
	{
		base.GetComponent<Renderer>().enabled = false;
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x00037CDF File Offset: 0x00035EDF
	public void Show()
	{
		base.GetComponent<Renderer>().enabled = true;
	}

	// Token: 0x04000547 RID: 1351
	private WeaponSystem m_WeaponSystem;
}
