using System;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class Fence : MonoBehaviour
{
	// Token: 0x060002CE RID: 718 RVA: 0x00037F7F File Offset: 0x0003617F
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.obj = GameObject.Find("/" + base.gameObject.name);
	}

	// Token: 0x04000557 RID: 1367
	public int id;

	// Token: 0x04000558 RID: 1368
	public int uid;

	// Token: 0x04000559 RID: 1369
	public int entid;

	// Token: 0x0400055A RID: 1370
	private Client cscl;

	// Token: 0x0400055B RID: 1371
	private EntManager entmanager;

	// Token: 0x0400055C RID: 1372
	private GameObject obj;
}
