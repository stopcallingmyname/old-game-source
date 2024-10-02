using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000067 RID: 103
public class Minefly : MonoBehaviour
{
	// Token: 0x06000302 RID: 770 RVA: 0x00038F6C File Offset: 0x0003716C
	private void Update()
	{
		if (this.chekTime < Time.time)
		{
			this.chekTime = Time.time + 0.2f;
			this.myTransform.LookAt(this.myTransform.position + base.gameObject.GetComponent<Rigidbody>().velocity);
			this.myTransform.eulerAngles += new Vector3(90f, 0f, 90f);
		}
	}

	// Token: 0x06000303 RID: 771 RVA: 0x00038FEC File Offset: 0x000371EC
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.obj = GameObject.Find("/" + base.gameObject.name);
		this.chekTime = Time.time;
		this.myTransform = base.transform;
		Sound sound = null;
		if (sound == null)
		{
			sound = (Sound)Object.FindObjectOfType(typeof(Sound));
		}
		base.GetComponent<AudioSource>().clip = sound.GetMineFly();
		base.GetComponent<AudioSource>().Play();
	}

	// Token: 0x06000304 RID: 772 RVA: 0x00039086 File Offset: 0x00037286
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
		yield return new WaitForSeconds(10f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x06000305 RID: 773 RVA: 0x00039095 File Offset: 0x00037295
	private void KillSelf()
	{
		if (base.gameObject == null)
		{
			return;
		}
		Object.Destroy(base.gameObject);
		EntManager.DeleteEnt(this.entid);
	}

	// Token: 0x06000306 RID: 774 RVA: 0x000390BC File Offset: 0x000372BC
	private void OnCollisionEnter(Collision collision)
	{
		this.Explode();
	}

	// Token: 0x06000307 RID: 775 RVA: 0x000390C4 File Offset: 0x000372C4
	private void OnTriggerEnter(Collider col)
	{
		if (col.GetComponent<Data>() != null)
		{
			this.Explode();
		}
	}

	// Token: 0x06000308 RID: 776 RVA: 0x000390DC File Offset: 0x000372DC
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
		base.GetComponent<AudioSource>().Stop();
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

	// Token: 0x0400059D RID: 1437
	public int id;

	// Token: 0x0400059E RID: 1438
	public int uid;

	// Token: 0x0400059F RID: 1439
	public int entid;

	// Token: 0x040005A0 RID: 1440
	private Client cscl;

	// Token: 0x040005A1 RID: 1441
	private EntManager entmanager;

	// Token: 0x040005A2 RID: 1442
	private GameObject obj;

	// Token: 0x040005A3 RID: 1443
	private Transform myTransform;

	// Token: 0x040005A4 RID: 1444
	private float chekTime;

	// Token: 0x040005A5 RID: 1445
	private bool exploded;
}
