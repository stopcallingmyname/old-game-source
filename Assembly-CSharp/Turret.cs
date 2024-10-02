using System;
using UnityEngine;

// Token: 0x02000074 RID: 116
public class Turret : MonoBehaviour
{
	// Token: 0x06000350 RID: 848 RVA: 0x0003A777 File Offset: 0x00038977
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.obj = GameObject.Find("/" + base.gameObject.name);
	}

	// Token: 0x04000608 RID: 1544
	public int id;

	// Token: 0x04000609 RID: 1545
	public int uid;

	// Token: 0x0400060A RID: 1546
	public int entid;

	// Token: 0x0400060B RID: 1547
	private Client cscl;

	// Token: 0x0400060C RID: 1548
	private EntManager entmanager;

	// Token: 0x0400060D RID: 1549
	private GameObject obj;
}
