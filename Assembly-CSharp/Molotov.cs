using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200006A RID: 106
public class Molotov : MonoBehaviour
{
	// Token: 0x06000314 RID: 788 RVA: 0x000395E8 File Offset: 0x000377E8
	private void Update()
	{
		if (this.chekTime < Time.time)
		{
			this.chekTime = Time.time + 0.2f;
			this.myTransform.LookAt(this.myTransform.position + base.gameObject.GetComponent<Rigidbody>().velocity);
			this.myTransform.eulerAngles += new Vector3(90f, 0f, 90f);
		}
	}

	// Token: 0x06000315 RID: 789 RVA: 0x00039668 File Offset: 0x00037868
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.chekTime = Time.time;
		this.myTransform = base.transform;
		if (this.sound == null)
		{
			this.sound = (Sound)Object.FindObjectOfType(typeof(Sound));
		}
		base.GetComponent<AudioSource>().clip = this.sound.GetMolotovFly();
		base.GetComponent<AudioSource>().Play();
	}

	// Token: 0x06000316 RID: 790 RVA: 0x000396EF File Offset: 0x000378EF
	private IEnumerator Start()
	{
		if (this.obj == null)
		{
			yield break;
		}
		this.obj.GetComponent<Renderer>().enabled = false;
		this.obj.GetComponent<Renderer>().castShadows = false;
		this.obj.GetComponent<Renderer>().receiveShadows = true;
		yield return new WaitForSeconds(0.1f);
		this.spec_active = true;
		if (this.obj.GetComponent<Renderer>().receiveShadows)
		{
			this.obj.GetComponent<Renderer>().enabled = true;
			this.obj.GetComponent<Renderer>().castShadows = true;
		}
		yield break;
	}

	// Token: 0x06000317 RID: 791 RVA: 0x000396FE File Offset: 0x000378FE
	private void KillSelf()
	{
		if (base.gameObject == null)
		{
			return;
		}
		Object.Destroy(base.gameObject);
		EntManager.DeleteEnt(this.entid);
	}

	// Token: 0x06000318 RID: 792 RVA: 0x00039728 File Offset: 0x00037928
	private void OnCollisionEnter(Collision collision)
	{
		if (!this.spec_active)
		{
			return;
		}
		if (base.gameObject == null)
		{
			return;
		}
		if (this.coll)
		{
			return;
		}
		this.coll = true;
		base.StartCoroutine(this.Explode());
		for (int i = 0; i < 10; i++)
		{
			Object.Instantiate<GameObject>(this.torch, base.gameObject.transform.position + Vector3.up * (float)i * 0.05f, this.torch.transform.rotation).GetComponent<Rigidbody>().AddForce(Vector3.up * 20f + Vector3.right * (float)Random.Range(-30, 30) + Vector3.forward * (float)Random.Range(-30, 30));
		}
	}

	// Token: 0x06000319 RID: 793 RVA: 0x00039810 File Offset: 0x00037A10
	private void OnTriggerEnter(Collider col)
	{
		if (!this.spec_active)
		{
			return;
		}
		if (base.gameObject == null)
		{
			return;
		}
		if (this.coll)
		{
			return;
		}
		if (col.GetComponent<Data>() != null)
		{
			this.coll = true;
			base.StartCoroutine(this.Explode());
			for (int i = 0; i < 10; i++)
			{
				Object.Instantiate<GameObject>(this.torch, base.gameObject.transform.position + Vector3.up * (float)i * 0.05f, this.torch.transform.rotation).GetComponent<Rigidbody>().AddForce(Vector3.up * 20f + Vector3.right * (float)Random.Range(-30, 30) + Vector3.forward * (float)Random.Range(-30, 30));
			}
		}
	}

	// Token: 0x0600031A RID: 794 RVA: 0x00039907 File Offset: 0x00037B07
	private IEnumerator Explode()
	{
		if (this.exploded)
		{
			yield break;
		}
		if (this.obj == null)
		{
			yield break;
		}
		this.flare.GetComponent<AudioSource>().PlayOneShot(this.sound.GetMolotovExplosion(), 1f);
		if (this.cscl == null)
		{
			this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		if (this.cscl == null)
		{
			yield break;
		}
		if (this.id == this.cscl.myindex)
		{
			this.cscl.send_new_ent_pos(this.uid, this.myTransform.position);
		}
		this.exploded = true;
		base.gameObject.GetComponent<Rigidbody>().Sleep();
		Object.Destroy(this.obj);
		base.GetComponent<AudioSource>().Stop();
		base.GetComponent<AudioSource>().clip = this.sound.GetMolotovBurn();
		base.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(11f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x040005B8 RID: 1464
	public int id;

	// Token: 0x040005B9 RID: 1465
	public int uid;

	// Token: 0x040005BA RID: 1466
	public int entid;

	// Token: 0x040005BB RID: 1467
	private Client cscl;

	// Token: 0x040005BC RID: 1468
	private EntManager entmanager;

	// Token: 0x040005BD RID: 1469
	public GameObject obj;

	// Token: 0x040005BE RID: 1470
	public GameObject flare;

	// Token: 0x040005BF RID: 1471
	private Transform myTransform;

	// Token: 0x040005C0 RID: 1472
	public GameObject torch;

	// Token: 0x040005C1 RID: 1473
	private bool coll;

	// Token: 0x040005C2 RID: 1474
	private float chekTime;

	// Token: 0x040005C3 RID: 1475
	private Sound sound;

	// Token: 0x040005C4 RID: 1476
	private bool spec_active;

	// Token: 0x040005C5 RID: 1477
	private bool exploded;
}
