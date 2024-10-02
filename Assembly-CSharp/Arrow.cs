using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class Arrow : MonoBehaviour
{
	// Token: 0x060002A5 RID: 677 RVA: 0x000374B4 File Offset: 0x000356B4
	private void Start()
	{
		this.newPos = base.transform.position;
		this.oldPos = this.newPos;
		this.velocity = this.speed * base.transform.forward;
		this.myTransform = base.transform;
		this.cspm = (ParticleManager)Object.FindObjectOfType(typeof(ParticleManager));
		base.StartCoroutine(this.Start2());
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00037530 File Offset: 0x00035730
	private void Update()
	{
		if (this.hasHit)
		{
			return;
		}
		this.newPos += (this.velocity + this.direction) * Time.deltaTime;
		Vector3 vector = this.newPos - this.oldPos;
		float magnitude = vector.magnitude;
		vector /= magnitude;
		if (magnitude > 0f)
		{
			Debug.DrawLine(this.oldPos, vector, Color.red);
			if (Physics.Raycast(this.oldPos, vector, out this.hit, magnitude, this.layerMask))
			{
				this.newPos = this.hit.point;
				if (this.hit.collider)
				{
					this.myTransform.parent = this.hit.transform;
					this.hasHit = true;
					if (this.hit.rigidbody)
					{
						this.hit.rigidbody.AddForceAtPosition(this.forceToApply * vector, this.hit.point);
					}
					this.Explode(this.hit.collider);
				}
			}
		}
		this.oldPos = base.transform.position;
		base.transform.position = this.newPos;
		this.velocity.y = this.velocity.y - this.arrowGravity * Time.deltaTime;
		base.transform.rotation = Quaternion.LookRotation(vector);
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x000376AC File Offset: 0x000358AC
	public void Explode(Collider col)
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
		Data component = col.gameObject.GetComponent<Data>();
		if (component)
		{
			this.cspm.CreateHit(this.myTransform, component.hitzone, this.myTransform.position.x, this.myTransform.position.y, this.myTransform.position.z);
			this.cspm.CreateParticle(this.myTransform.position.x, this.myTransform.position.y, this.myTransform.position.z, 1f, 0f, 0f, 1f);
			if (this.id == this.cscl.myindex)
			{
				this.cscl.send_damage(161, component.index, component.hitzone, Time.time, this.cscl.transform.position.x, this.cscl.transform.position.y, this.cscl.transform.position.z, col.transform.position.x, col.transform.position.y, col.transform.position.z, this.cscl.transform.position.x, this.cscl.transform.position.y, this.cscl.transform.position.z, col.transform.position.x, col.transform.position.y, col.transform.position.z);
			}
		}
		base.GetComponent<AudioSource>().PlayOneShot(this.soundHit, 0.7f);
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x000378D8 File Offset: 0x00035AD8
	private IEnumerator Start2()
	{
		yield return new WaitForSeconds(10.5f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x000378E7 File Offset: 0x00035AE7
	public void KillSelf()
	{
		if (base.gameObject == null)
		{
			return;
		}
		Object.Destroy(base.gameObject);
		EntManager.DeleteEnt(this.entid);
	}

	// Token: 0x0400051D RID: 1309
	public int id;

	// Token: 0x0400051E RID: 1310
	public int uid;

	// Token: 0x0400051F RID: 1311
	public int entid;

	// Token: 0x04000520 RID: 1312
	public LayerMask layerMask;

	// Token: 0x04000521 RID: 1313
	public AudioClip soundHit;

	// Token: 0x04000522 RID: 1314
	private Vector3 velocity = Vector3.zero;

	// Token: 0x04000523 RID: 1315
	private Vector3 newPos = Vector3.zero;

	// Token: 0x04000524 RID: 1316
	private Vector3 oldPos = Vector3.zero;

	// Token: 0x04000525 RID: 1317
	private bool hasHit;

	// Token: 0x04000526 RID: 1318
	private Vector3 direction;

	// Token: 0x04000527 RID: 1319
	private RaycastHit hit;

	// Token: 0x04000528 RID: 1320
	public float speed;

	// Token: 0x04000529 RID: 1321
	private Transform myTransform;

	// Token: 0x0400052A RID: 1322
	public float forceToApply;

	// Token: 0x0400052B RID: 1323
	public float arrowGravity;

	// Token: 0x0400052C RID: 1324
	private GameObject follow;

	// Token: 0x0400052D RID: 1325
	private bool exploded;

	// Token: 0x0400052E RID: 1326
	private Client cscl;

	// Token: 0x0400052F RID: 1327
	private ParticleManager cspm;
}
