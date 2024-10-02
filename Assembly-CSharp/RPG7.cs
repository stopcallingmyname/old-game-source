using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200006E RID: 110
public class RPG7 : MonoBehaviour
{
	// Token: 0x0600032C RID: 812 RVA: 0x00039C52 File Offset: 0x00037E52
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.obj = base.gameObject;
	}

	// Token: 0x0600032D RID: 813 RVA: 0x00039C7A File Offset: 0x00037E7A
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
		yield return new WaitForSeconds(2f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x0600032E RID: 814 RVA: 0x00039C89 File Offset: 0x00037E89
	private void KillSelf()
	{
		if (base.gameObject == null)
		{
			return;
		}
		Object.Destroy(base.gameObject);
		EntManager.DeleteEnt(this.entid);
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00039CB0 File Offset: 0x00037EB0
	private void OnCollisionEnter(Collision collision)
	{
		this.Explode();
	}

	// Token: 0x06000330 RID: 816 RVA: 0x00039CB8 File Offset: 0x00037EB8
	private void OnTriggerEnter(Collider col)
	{
		if (base.gameObject == null)
		{
			return;
		}
		if (col.GetComponent<Data>() != null)
		{
			this.Explode();
		}
	}

	// Token: 0x06000331 RID: 817 RVA: 0x00039CE0 File Offset: 0x00037EE0
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

	// Token: 0x040005D6 RID: 1494
	public int id;

	// Token: 0x040005D7 RID: 1495
	public int uid;

	// Token: 0x040005D8 RID: 1496
	public int entid;

	// Token: 0x040005D9 RID: 1497
	private Client cscl;

	// Token: 0x040005DA RID: 1498
	private EntManager entmanager;

	// Token: 0x040005DB RID: 1499
	private GameObject obj;

	// Token: 0x040005DC RID: 1500
	private bool exploded;
}
