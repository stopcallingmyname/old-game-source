using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class Snaryad : MonoBehaviour
{
	// Token: 0x0600033D RID: 829 RVA: 0x00039FB0 File Offset: 0x000381B0
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.obj = GameObject.Find("/" + base.gameObject.name);
		this.starttime = Time.time;
	}

	// Token: 0x0600033E RID: 830 RVA: 0x0003A002 File Offset: 0x00038202
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

	// Token: 0x0600033F RID: 831 RVA: 0x0003A011 File Offset: 0x00038211
	private void KillSelf()
	{
		if (base.gameObject == null)
		{
			return;
		}
		Object.Destroy(base.gameObject);
		EntManager.DeleteEnt(this.entid);
	}

	// Token: 0x06000340 RID: 832 RVA: 0x0003A038 File Offset: 0x00038238
	private void OnCollisionEnter(Collision collision)
	{
		if (Time.time > this.starttime + 0.01f)
		{
			this.collignore = false;
		}
		if (this.collignore)
		{
			return;
		}
		this.Explode(collision.collider);
	}

	// Token: 0x06000341 RID: 833 RVA: 0x0003A06C File Offset: 0x0003826C
	private void OnTriggerEnter(Collider col)
	{
		if (Time.time > this.starttime + 0.01f)
		{
			this.collignore = false;
		}
		if (this.collignore)
		{
			return;
		}
		if (base.gameObject == null)
		{
			return;
		}
		if (col.GetComponent<Data>() != null)
		{
			this.Explode(col);
		}
	}

	// Token: 0x06000342 RID: 834 RVA: 0x0003A0C0 File Offset: 0x000382C0
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
			if (this.classid == 19)
			{
				if (collision.transform.name[0] == '(')
				{
					this.cscl.send_detonateent(this.uid, base.gameObject.transform.position);
					return;
				}
				this.cscl.send_detonateent(this.uid, collision.transform.position);
				return;
			}
			else
			{
				this.cscl.send_detonateent(this.uid, base.gameObject.transform.position);
			}
		}
	}

	// Token: 0x040005E5 RID: 1509
	public int id;

	// Token: 0x040005E6 RID: 1510
	public int uid;

	// Token: 0x040005E7 RID: 1511
	public int entid;

	// Token: 0x040005E8 RID: 1512
	public int classid;

	// Token: 0x040005E9 RID: 1513
	private Client cscl;

	// Token: 0x040005EA RID: 1514
	private EntManager entmanager;

	// Token: 0x040005EB RID: 1515
	private GameObject obj;

	// Token: 0x040005EC RID: 1516
	private float starttime;

	// Token: 0x040005ED RID: 1517
	private bool collignore = true;

	// Token: 0x040005EE RID: 1518
	private bool exploded;
}
