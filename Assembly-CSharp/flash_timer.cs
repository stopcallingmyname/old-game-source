using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000077 RID: 119
public class flash_timer : MonoBehaviour
{
	// Token: 0x0600035A RID: 858 RVA: 0x0003A86D File Offset: 0x00038A6D
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(2f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00016B46 File Offset: 0x00014D46
	private void KillSelf()
	{
		Object.Destroy(base.gameObject);
	}
}
