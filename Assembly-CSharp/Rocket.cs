using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class Rocket : MonoBehaviour
{
	// Token: 0x06000325 RID: 805 RVA: 0x00039AB4 File Offset: 0x00037CB4
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.obj = GameObject.Find("/" + base.gameObject.name + "/rpg");
	}

	// Token: 0x06000326 RID: 806 RVA: 0x00039B00 File Offset: 0x00037D00
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(3.05f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x06000327 RID: 807 RVA: 0x00039B0F File Offset: 0x00037D0F
	private void KillSelf()
	{
		if (base.gameObject == null)
		{
			return;
		}
		Object.Destroy(base.gameObject);
		EntManager.DeleteEnt(this.entid);
	}

	// Token: 0x06000328 RID: 808 RVA: 0x00039B36 File Offset: 0x00037D36
	private void OnCollisionEnter(Collision collision)
	{
		this.Explode();
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00039B3E File Offset: 0x00037D3E
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

	// Token: 0x0600032A RID: 810 RVA: 0x00039B64 File Offset: 0x00037D64
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

	// Token: 0x040005CF RID: 1487
	public int id;

	// Token: 0x040005D0 RID: 1488
	public int uid;

	// Token: 0x040005D1 RID: 1489
	public int entid;

	// Token: 0x040005D2 RID: 1490
	private Client cscl;

	// Token: 0x040005D3 RID: 1491
	private EntManager entmanager;

	// Token: 0x040005D4 RID: 1492
	private GameObject obj;

	// Token: 0x040005D5 RID: 1493
	private bool exploded;
}
