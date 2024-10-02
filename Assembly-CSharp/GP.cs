using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000061 RID: 97
public class GP : MonoBehaviour
{
	// Token: 0x060002DB RID: 731 RVA: 0x000383D4 File Offset: 0x000365D4
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.obj = GameObject.Find("/" + base.gameObject.name);
		this.starttime = Time.time;
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00038426 File Offset: 0x00036626
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

	// Token: 0x060002DD RID: 733 RVA: 0x00038438 File Offset: 0x00036638
	private void KillSelf()
	{
		if (base.gameObject == null)
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
		if (this.id == this.cscl.myindex)
		{
			this.cscl.send_detonateent(this.uid, this.obj.transform.position);
		}
		EntManager.DeleteEnt(this.entid);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060002DE RID: 734 RVA: 0x000384D5 File Offset: 0x000366D5
	private void OnCollisionEnter(Collision collision)
	{
		if (this.collignore)
		{
			return;
		}
		if (Time.time < this.starttime + 0.2f)
		{
			this.collignore = true;
			return;
		}
		this.KillSelf();
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00038501 File Offset: 0x00036701
	private void OnTriggerEnter(Collider col)
	{
		if (base.gameObject == null)
		{
			return;
		}
		if (col.GetComponent<Data>() != null)
		{
			this.KillSelf();
		}
	}

	// Token: 0x0400056C RID: 1388
	public int id;

	// Token: 0x0400056D RID: 1389
	public int uid;

	// Token: 0x0400056E RID: 1390
	public int entid;

	// Token: 0x0400056F RID: 1391
	private Client cscl;

	// Token: 0x04000570 RID: 1392
	private EntManager entmanager;

	// Token: 0x04000571 RID: 1393
	private GameObject obj;

	// Token: 0x04000572 RID: 1394
	private float starttime;

	// Token: 0x04000573 RID: 1395
	private bool collignore;
}
