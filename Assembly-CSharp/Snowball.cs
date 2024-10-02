using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000072 RID: 114
public class Snowball : MonoBehaviour
{
	// Token: 0x06000344 RID: 836 RVA: 0x0003A1FC File Offset: 0x000383FC
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
		this.cspm = (ParticleManager)Object.FindObjectOfType(typeof(ParticleManager));
		this.myColliders = base.GetComponents<Collider>();
		this.myTransform = base.transform;
		this.fx = base.GetComponentInChildren<ParticleSystem>();
		this.myRenderer = base.GetComponent<Renderer>();
		this.myRigidbody = base.GetComponent<Rigidbody>();
		this.startTime = Time.time;
	}

	// Token: 0x06000345 RID: 837 RVA: 0x0003A29E File Offset: 0x0003849E
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.1f);
		base.gameObject.layer = 0;
		yield return new WaitForSeconds(5f);
		base.StartCoroutine(this.KillSelf());
		yield break;
	}

	// Token: 0x06000346 RID: 838 RVA: 0x0003A2AD File Offset: 0x000384AD
	private void OnTriggerEnter(Collider collision)
	{
		if (Time.time - this.startTime < 0.1f)
		{
			return;
		}
		this.Explode(collision, this.myTransform.position, Vector3.zero);
	}

	// Token: 0x06000347 RID: 839 RVA: 0x0003A2DC File Offset: 0x000384DC
	private void OnCollisionEnter(Collision collision)
	{
		if (Time.time - this.startTime < 0.1f)
		{
			return;
		}
		this.Explode(collision.collider, collision.contacts[0].point, collision.contacts[0].normal);
	}

	// Token: 0x06000348 RID: 840 RVA: 0x0003A32C File Offset: 0x0003852C
	public void Explode(Collider col, Vector3 pos, Vector3 rot)
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
		if (this.cspm == null)
		{
			this.cspm = (ParticleManager)Object.FindObjectOfType(typeof(ParticleManager));
		}
		this.exploded = true;
		if (col.gameObject.GetComponent<Data>())
		{
			if (this.cspm != null)
			{
				this.cspm.CreateParticle(this.myTransform.position.x, this.myTransform.position.y, this.myTransform.position.z, 1f, 1f, 1f, 1f);
			}
			if (this.fx != null)
			{
				this.fx.Play();
			}
		}
		else
		{
			if (this.cspm != null)
			{
				this.cspm.CreateParticle(this.myTransform.position.x, this.myTransform.position.y, this.myTransform.position.z, 1f, 1f, 1f, 1f);
			}
			if (this.fx != null)
			{
				this.fx.Play();
			}
			if (col.transform.name[0] == '(' && this.decal != null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.decal);
				gameObject.transform.position = pos;
				gameObject.transform.rotation = Quaternion.LookRotation(rot);
				gameObject.transform.RotateAroundLocal(gameObject.transform.forward, (float)Random.Range(0, 360));
				Object.DestroyObject(gameObject, 5f);
			}
		}
		base.GetComponent<AudioSource>().PlayOneShot(this.soundHit);
		base.StartCoroutine(this.KillSelf());
	}

	// Token: 0x06000349 RID: 841 RVA: 0x0003A545 File Offset: 0x00038745
	private IEnumerator KillSelf()
	{
		if (this.myColliders.Length != 0)
		{
			Collider[] array = this.myColliders;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
		}
		this.myRenderer.enabled = false;
		this.myRigidbody.isKinematic = true;
		yield return new WaitForSeconds(2f);
		if (base.gameObject == null)
		{
			yield return null;
		}
		EntManager.DeleteEnt(this.entid);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040005EF RID: 1519
	public int id;

	// Token: 0x040005F0 RID: 1520
	public int uid;

	// Token: 0x040005F1 RID: 1521
	public int entid;

	// Token: 0x040005F2 RID: 1522
	private Client cscl;

	// Token: 0x040005F3 RID: 1523
	private Collider[] myColliders;

	// Token: 0x040005F4 RID: 1524
	private EntManager entmanager;

	// Token: 0x040005F5 RID: 1525
	private ParticleManager cspm;

	// Token: 0x040005F6 RID: 1526
	private Transform myTransform;

	// Token: 0x040005F7 RID: 1527
	private Renderer myRenderer;

	// Token: 0x040005F8 RID: 1528
	private Rigidbody myRigidbody;

	// Token: 0x040005F9 RID: 1529
	public AudioClip soundHit;

	// Token: 0x040005FA RID: 1530
	private ParticleSystem fx;

	// Token: 0x040005FB RID: 1531
	public GameObject decal;

	// Token: 0x040005FC RID: 1532
	private float startTime;

	// Token: 0x040005FD RID: 1533
	private bool exploded;
}
