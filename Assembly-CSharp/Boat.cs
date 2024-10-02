using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
public class Boat : MonoBehaviour
{
	// Token: 0x060002B2 RID: 690 RVA: 0x00037A81 File Offset: 0x00035C81
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.obj = GameObject.Find("/" + base.gameObject.name);
	}

	// Token: 0x04000538 RID: 1336
	public int id;

	// Token: 0x04000539 RID: 1337
	public int uid;

	// Token: 0x0400053A RID: 1338
	public int entid;

	// Token: 0x0400053B RID: 1339
	private Client cscl;

	// Token: 0x0400053C RID: 1340
	private EntManager entmanager;

	// Token: 0x0400053D RID: 1341
	private GameObject obj;
}
