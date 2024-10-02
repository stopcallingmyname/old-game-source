using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class C4 : MonoBehaviour
{
	// Token: 0x060002B4 RID: 692 RVA: 0x0000248C File Offset: 0x0000068C
	private void Update()
	{
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x00037AC0 File Offset: 0x00035CC0
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.obj = GameObject.Find("/" + base.gameObject.name);
		this.myTransform = base.transform;
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x00037B13 File Offset: 0x00035D13
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

	// Token: 0x060002B7 RID: 695 RVA: 0x00037B22 File Offset: 0x00035D22
	public void KillSelf()
	{
		if (base.gameObject == null)
		{
			return;
		}
		Object.Destroy(base.gameObject);
		EntManager.DeleteEnt(this.entid);
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x00037B4C File Offset: 0x00035D4C
	private void OnCollisionEnter(Collision collision)
	{
		if (this.cscl == null)
		{
			this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		if (this.cscl == null)
		{
			return;
		}
		if (this.id == this.cscl.myindex)
		{
			this.cscl.send_new_ent_pos(this.uid, this.myTransform.position);
		}
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00037BC0 File Offset: 0x00035DC0
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

	// Token: 0x0400053E RID: 1342
	public int id;

	// Token: 0x0400053F RID: 1343
	public int uid;

	// Token: 0x04000540 RID: 1344
	public int entid;

	// Token: 0x04000541 RID: 1345
	private Client cscl;

	// Token: 0x04000542 RID: 1346
	private EntManager entmanager;

	// Token: 0x04000543 RID: 1347
	private GameObject obj;

	// Token: 0x04000544 RID: 1348
	private Transform myTransform;

	// Token: 0x04000545 RID: 1349
	private bool exploded;
}
