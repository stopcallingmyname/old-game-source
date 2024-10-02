using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class dk : MonoBehaviour
{
	// Token: 0x06000135 RID: 309 RVA: 0x0001709F File Offset: 0x0001529F
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.8f);
		if (base.GetComponent<Collider>())
		{
			base.GetComponent<Collider>().enabled = false;
		}
		yield return new WaitForSeconds(2f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x06000136 RID: 310 RVA: 0x000170AE File Offset: 0x000152AE
	private void KillSelf()
	{
		if (base.gameObject)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
