using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200002A RID: 42
public class FX : MonoBehaviour
{
	// Token: 0x06000138 RID: 312 RVA: 0x000170C8 File Offset: 0x000152C8
	private void Awake()
	{
		this.fx = (Resources.Load("Prefab/Infect") as GameObject);
	}

	// Token: 0x06000139 RID: 313 RVA: 0x000170DF File Offset: 0x000152DF
	public void Infect()
	{
		base.StartCoroutine(this.r_Infect());
	}

	// Token: 0x0600013A RID: 314 RVA: 0x000170EE File Offset: 0x000152EE
	private IEnumerator r_Infect()
	{
		GameObject newfx = Object.Instantiate<GameObject>(this.fx);
		newfx.transform.position = base.gameObject.transform.position;
		yield return new WaitForSeconds(1f);
		Object.Destroy(newfx);
		yield break;
	}

	// Token: 0x04000151 RID: 337
	private GameObject fx;
}
