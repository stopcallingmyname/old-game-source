using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class Minen : MonoBehaviour
{
	// Token: 0x0600030A RID: 778 RVA: 0x000391D8 File Offset: 0x000373D8
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.obj = base.gameObject;
		this.starttime = Time.time;
		this.myTransform = base.transform;
		Sound sound = null;
		if (sound == null)
		{
			sound = (Sound)Object.FindObjectOfType(typeof(Sound));
		}
		base.GetComponent<AudioSource>().clip = sound.GetMineFly();
		base.GetComponent<AudioSource>().Play();
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00039260 File Offset: 0x00037460
	private void Update()
	{
		float num = Vector3.Distance(this.targetPosition, this.myTransform.position);
		if (num < 1f)
		{
			this.Explode();
			return;
		}
		this.myTransform.up = Vector3.Lerp(this.myTransform.up, this.targetPosition - this.myTransform.position, Time.deltaTime * (this.BallisticKoef / num));
		this.myTransform.Translate(-this.myTransform.up * this.speed * Time.deltaTime);
	}

	// Token: 0x0600030C RID: 780 RVA: 0x00039302 File Offset: 0x00037502
	private IEnumerator Start()
	{
		if (this.obj == null)
		{
			yield break;
		}
		yield return new WaitForSeconds(15f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00039311 File Offset: 0x00037511
	private void KillSelf()
	{
		if (this.obj == null)
		{
			return;
		}
		EntManager.DeleteEnt(this.entid);
		Object.Destroy(this.obj);
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00039338 File Offset: 0x00037538
	private void OnCollisionEnter(Collision collision)
	{
		if (Time.time > this.starttime + 0.5f)
		{
			this.collignore = false;
		}
		if (this.collignore)
		{
			return;
		}
		this.Explode();
	}

	// Token: 0x0600030F RID: 783 RVA: 0x00039364 File Offset: 0x00037564
	private void OnTriggerEnter(Collider col)
	{
		if (base.gameObject == null)
		{
			return;
		}
		if (Time.time > this.starttime + 0.5f)
		{
			this.collignore = false;
		}
		if (this.collignore)
		{
			return;
		}
		if (col.GetComponent<Data>() != null)
		{
			this.Explode();
		}
	}

	// Token: 0x06000310 RID: 784 RVA: 0x000393B8 File Offset: 0x000375B8
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
		base.GetComponentInChildren<Renderer>().castShadows = false;
		base.GetComponentInChildren<Renderer>().receiveShadows = false;
		base.GetComponentInChildren<Renderer>().enabled = false;
		base.gameObject.transform.parent = null;
		base.gameObject.GetComponentInChildren<ParticleSystem>().loop = false;
		base.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
		if (this.id == this.cscl.myindex)
		{
			this.cscl.send_detonateent(this.uid, base.gameObject.transform.position);
		}
	}

	// Token: 0x040005A6 RID: 1446
	public int id;

	// Token: 0x040005A7 RID: 1447
	public int uid;

	// Token: 0x040005A8 RID: 1448
	public int entid;

	// Token: 0x040005A9 RID: 1449
	private Client cscl;

	// Token: 0x040005AA RID: 1450
	private EntManager entmanager;

	// Token: 0x040005AB RID: 1451
	private GameObject obj;

	// Token: 0x040005AC RID: 1452
	private float starttime;

	// Token: 0x040005AD RID: 1453
	private bool collignore = true;

	// Token: 0x040005AE RID: 1454
	private float BallisticKoef = 10f;

	// Token: 0x040005AF RID: 1455
	public Vector3 targetPosition;

	// Token: 0x040005B0 RID: 1456
	private Transform myTransform;

	// Token: 0x040005B1 RID: 1457
	private float speed = 30f;

	// Token: 0x040005B2 RID: 1458
	private bool exploded;
}
