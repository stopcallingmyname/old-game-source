using System;
using UnityEngine;

// Token: 0x0200008B RID: 139
public class Switch : MonoBehaviour
{
	// Token: 0x060003F1 RID: 1009 RVA: 0x0004D656 File Offset: 0x0004B856
	private void Awake()
	{
		this.NS = base.gameObject.GetComponent<New_Slots>();
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x0004D669 File Offset: 0x0004B869
	public void ShowPanel(int _slot)
	{
		this.showTime = Time.time + 1.25f;
		this.slot = 0;
		this.panel = _slot;
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x0004D68A File Offset: 0x0004B88A
	public void ShowPanel2(int _slot)
	{
		this.showTime = Time.time + 1.25f;
		this.slot = _slot;
		this.panel = 5;
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x0004D6AB File Offset: 0x0004B8AB
	private void OnGUI()
	{
		if (Time.time > this.showTime)
		{
			return;
		}
		this.NS.Draw_New_Slots(this.panel + this.slot);
	}

	// Token: 0x04000888 RID: 2184
	private float showTime;

	// Token: 0x04000889 RID: 2185
	private int slot;

	// Token: 0x0400088A RID: 2186
	private int panel;

	// Token: 0x0400088B RID: 2187
	private New_Slots NS;
}
