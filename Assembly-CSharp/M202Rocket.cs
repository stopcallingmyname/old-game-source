using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class M202Rocket : MonoBehaviour
{
	// Token: 0x060002EF RID: 751 RVA: 0x00038B72 File Offset: 0x00036D72
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		if (this.obj == null)
		{
			this.obj = base.gameObject;
		}
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x00038BA8 File Offset: 0x00036DA8
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
		yield return new WaitForSeconds(3f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x00038BB7 File Offset: 0x00036DB7
	private void KillSelf()
	{
		if (base.gameObject == null)
		{
			return;
		}
		Object.Destroy(base.gameObject);
		EntManager.DeleteEnt(this.entid);
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x00038BDE File Offset: 0x00036DDE
	private void OnCollisionEnter(Collision collision)
	{
		this.Explode();
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x00038BE6 File Offset: 0x00036DE6
	private void OnTriggerEnter(Collider col)
	{
		if (col.GetComponent<Data>() != null)
		{
			this.Explode();
		}
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x00038BFC File Offset: 0x00036DFC
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
		base.gameObject.GetComponent<Rigidbody>().Sleep();
		base.gameObject.GetComponentInChildren<ParticleSystem>().loop = false;
		base.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
		if (this.id == this.cscl.myindex)
		{
			this.cscl.send_detonateent(this.uid, base.gameObject.transform.position);
		}
	}

	// Token: 0x04000589 RID: 1417
	public int id;

	// Token: 0x0400058A RID: 1418
	public int uid;

	// Token: 0x0400058B RID: 1419
	public int entid;

	// Token: 0x0400058C RID: 1420
	private Client cscl;

	// Token: 0x0400058D RID: 1421
	private EntManager entmanager;

	// Token: 0x0400058E RID: 1422
	private GameObject obj;

	// Token: 0x0400058F RID: 1423
	private bool exploded;
}
