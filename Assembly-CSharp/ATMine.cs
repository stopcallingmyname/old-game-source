using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000056 RID: 86
public class ATMine : MonoBehaviour
{
	// Token: 0x060002AB RID: 683 RVA: 0x0000248C File Offset: 0x0000068C
	private void Update()
	{
	}

	// Token: 0x060002AC RID: 684 RVA: 0x00037938 File Offset: 0x00035B38
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.obj = GameObject.Find("/" + base.gameObject.name);
		this.myTransform = base.transform;
	}

	// Token: 0x060002AD RID: 685 RVA: 0x0003798B File Offset: 0x00035B8B
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

	// Token: 0x060002AE RID: 686 RVA: 0x0003799A File Offset: 0x00035B9A
	public void KillSelf()
	{
		if (base.gameObject == null)
		{
			return;
		}
		Object.Destroy(base.gameObject);
		EntManager.DeleteEnt(this.entid);
	}

	// Token: 0x060002AF RID: 687 RVA: 0x0000248C File Offset: 0x0000068C
	private void OnCollisionEnter(Collision collision)
	{
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x000379C4 File Offset: 0x00035BC4
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

	// Token: 0x04000530 RID: 1328
	public int id;

	// Token: 0x04000531 RID: 1329
	public int uid;

	// Token: 0x04000532 RID: 1330
	public int entid;

	// Token: 0x04000533 RID: 1331
	private Client cscl;

	// Token: 0x04000534 RID: 1332
	private EntManager entmanager;

	// Token: 0x04000535 RID: 1333
	private GameObject obj;

	// Token: 0x04000536 RID: 1334
	private Transform myTransform;

	// Token: 0x04000537 RID: 1335
	private bool exploded;
}
