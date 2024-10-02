using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class blood_timer : MonoBehaviour
{
	// Token: 0x06000125 RID: 293 RVA: 0x00016B37 File Offset: 0x00014D37
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.2f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00016B46 File Offset: 0x00014D46
	private void KillSelf()
	{
		Object.Destroy(base.gameObject);
	}
}
