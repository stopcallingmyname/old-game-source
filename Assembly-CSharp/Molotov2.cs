using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class Molotov2 : MonoBehaviour
{
	// Token: 0x0600001B RID: 27 RVA: 0x000024B4 File Offset: 0x000006B4
	private void OnCollisionEnter(Collision col)
	{
		if (base.gameObject == null)
		{
			return;
		}
		if (this.coll)
		{
			return;
		}
		this.coll = true;
		for (int i = 0; i < 10; i++)
		{
			Object.Instantiate<GameObject>(this.torch, base.gameObject.transform.position + Vector3.up * (float)i * 0.05f, this.torch.transform.rotation).GetComponent<Rigidbody>().AddForce(Vector3.up * 20f + Vector3.right * (float)Random.Range(-30, 30) + Vector3.forward * (float)Random.Range(-30, 30));
		}
	}

	// Token: 0x0400000C RID: 12
	public GameObject torch;

	// Token: 0x0400000D RID: 13
	private bool coll;
}
