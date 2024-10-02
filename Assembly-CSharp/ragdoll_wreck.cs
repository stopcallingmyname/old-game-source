using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class ragdoll_wreck : MonoBehaviour
{
	// Token: 0x06000540 RID: 1344 RVA: 0x00062357 File Offset: 0x00060557
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(5f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x00016B46 File Offset: 0x00014D46
	private void KillSelf()
	{
		Object.Destroy(base.gameObject);
	}
}
