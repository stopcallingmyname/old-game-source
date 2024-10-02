using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000B8 RID: 184
public class SH : MonoBehaviour
{
	// Token: 0x060005E5 RID: 1509 RVA: 0x00068B1E File Offset: 0x00066D1E
	private void Start()
	{
		this.olddt = DateTime.Now;
		this.oldTick = (long)Environment.TickCount;
		base.InvokeRepeating("invSpeedHackX", 5f, 5f);
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x00068B4C File Offset: 0x00066D4C
	private void invSpeedHackX()
	{
		TimeSpan timeSpan = DateTime.Now - this.olddt;
		this.olddt = DateTime.Now;
		long num = (long)Environment.TickCount - this.oldTick;
		this.oldTick = (long)Environment.TickCount;
		if (timeSpan.TotalMilliseconds * 1.2999999523162842 < (double)num)
		{
			this.errorCount++;
		}
		if (this.errorCount > 5)
		{
			SceneManager.LoadScene(0);
		}
	}

	// Token: 0x04000C4A RID: 3146
	private DateTime olddt;

	// Token: 0x04000C4B RID: 3147
	private long oldTick;

	// Token: 0x04000C4C RID: 3148
	private int errorCount;
}
