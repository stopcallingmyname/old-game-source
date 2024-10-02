using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000075 RID: 117
public class VehicleFlash : MonoBehaviour
{
	// Token: 0x06000352 RID: 850 RVA: 0x0003A7B3 File Offset: 0x000389B3
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(2.5f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x06000353 RID: 851 RVA: 0x0003A7C2 File Offset: 0x000389C2
	private void KillSelf()
	{
		EntManager.DeleteEnt(this.entid);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0400060E RID: 1550
	public int id;

	// Token: 0x0400060F RID: 1551
	public int uid;

	// Token: 0x04000610 RID: 1552
	public int entid;
}
