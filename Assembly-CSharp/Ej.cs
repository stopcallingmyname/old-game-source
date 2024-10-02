using System;
using UnityEngine;

// Token: 0x0200005B RID: 91
public class Ej : MonoBehaviour
{
	// Token: 0x060002C3 RID: 707 RVA: 0x00037D40 File Offset: 0x00035F40
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.obj = GameObject.Find("/" + base.gameObject.name);
	}

	// Token: 0x04000548 RID: 1352
	public int id;

	// Token: 0x04000549 RID: 1353
	public int uid;

	// Token: 0x0400054A RID: 1354
	public int entid;

	// Token: 0x0400054B RID: 1355
	private Client cscl;

	// Token: 0x0400054C RID: 1356
	private EntManager entmanager;

	// Token: 0x0400054D RID: 1357
	private GameObject obj;
}
