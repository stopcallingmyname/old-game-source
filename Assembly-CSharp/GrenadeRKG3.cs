using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class GrenadeRKG3 : MonoBehaviour
{
	// Token: 0x060002E1 RID: 737 RVA: 0x00038526 File Offset: 0x00036726
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x00038542 File Offset: 0x00036742
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(3f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x00038551 File Offset: 0x00036751
	private void KillSelf()
	{
		if (base.gameObject == null)
		{
			return;
		}
		Object.Destroy(base.gameObject);
		EntManager.DeleteEnt(this.entid);
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x00038578 File Offset: 0x00036778
	private void OnCollisionEnter(Collision collision)
	{
		this.Explode(collision.collider);
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x00038586 File Offset: 0x00036786
	private void OnTriggerEnter(Collider col)
	{
		if (base.gameObject == null)
		{
			return;
		}
		if (col.GetComponent<Data>() != null)
		{
			this.Explode(col);
		}
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x000385AC File Offset: 0x000367AC
	public void Explode(Collider collision)
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
		if (this.id == this.cscl.myindex)
		{
			if (collision.transform.name[0] == '(')
			{
				this.cscl.send_detonateent(this.uid, base.gameObject.transform.position);
				return;
			}
			this.cscl.send_detonateent(this.uid, collision.transform.position);
		}
	}

	// Token: 0x04000574 RID: 1396
	public int id;

	// Token: 0x04000575 RID: 1397
	public int uid;

	// Token: 0x04000576 RID: 1398
	public int entid;

	// Token: 0x04000577 RID: 1399
	private Client cscl;

	// Token: 0x04000578 RID: 1400
	private EntManager entmanager;

	// Token: 0x04000579 RID: 1401
	private bool exploded;
}
