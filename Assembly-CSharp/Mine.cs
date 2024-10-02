using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class Mine : MonoBehaviour
{
	// Token: 0x060002FB RID: 763 RVA: 0x0000248C File Offset: 0x0000068C
	private void Update()
	{
	}

	// Token: 0x060002FC RID: 764 RVA: 0x00038E20 File Offset: 0x00037020
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.obj = GameObject.Find("/" + base.gameObject.name);
		this.myTransform = base.transform;
	}

	// Token: 0x060002FD RID: 765 RVA: 0x00038E73 File Offset: 0x00037073
	private IEnumerator Start()
	{
		if (this.obj == null)
		{
			yield break;
		}
		this.obj.GetComponent<Renderer>().enabled = false;
		this.obj.GetComponent<Renderer>().castShadows = false;
		this.obj.GetComponent<Renderer>().receiveShadows = true;
		yield return new WaitForSeconds(0.05f);
		if (this.obj.GetComponent<Renderer>().receiveShadows)
		{
			this.obj.GetComponent<Renderer>().enabled = true;
			this.obj.GetComponent<Renderer>().castShadows = true;
		}
		yield return new WaitForSeconds(180f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x060002FE RID: 766 RVA: 0x00038E82 File Offset: 0x00037082
	public void KillSelf()
	{
		if (base.gameObject == null)
		{
			return;
		}
		Object.Destroy(base.gameObject);
		EntManager.DeleteEnt(this.entid);
	}

	// Token: 0x060002FF RID: 767 RVA: 0x0000248C File Offset: 0x0000068C
	private void OnCollisionEnter(Collision collision)
	{
	}

	// Token: 0x06000300 RID: 768 RVA: 0x00038EAC File Offset: 0x000370AC
	public void Explode()
	{
		if (this.exploded)
		{
			return;
		}
		if (this.cscl == null)
		{
			this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		if (this.cscl == null)
		{
			return;
		}
		this.exploded = true;
		base.GetComponent<Renderer>().castShadows = false;
		base.GetComponent<Renderer>().receiveShadows = false;
		base.GetComponent<Renderer>().enabled = false;
		base.gameObject.transform.parent = null;
		if (this.id == this.cscl.myindex)
		{
			this.cscl.send_detonateent(this.uid, base.gameObject.transform.position);
		}
	}

	// Token: 0x04000595 RID: 1429
	public int id;

	// Token: 0x04000596 RID: 1430
	public int uid;

	// Token: 0x04000597 RID: 1431
	public int entid;

	// Token: 0x04000598 RID: 1432
	private Client cscl;

	// Token: 0x04000599 RID: 1433
	private EntManager entmanager;

	// Token: 0x0400059A RID: 1434
	private GameObject obj;

	// Token: 0x0400059B RID: 1435
	private Transform myTransform;

	// Token: 0x0400059C RID: 1436
	private bool exploded;
}
