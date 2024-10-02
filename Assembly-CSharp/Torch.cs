using System;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class Torch : MonoBehaviour
{
	// Token: 0x0600001D RID: 29 RVA: 0x00002584 File Offset: 0x00000784
	private void Start()
	{
		this.deadtime = Time.time + (float)Random.Range(10, 15);
		this.myRigidBody = base.gameObject.GetComponent<Rigidbody>();
		this.myCollider = base.gameObject.GetComponent<Collider>();
		this.myTransform = base.transform;
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000025D8 File Offset: 0x000007D8
	private void Update()
	{
		if (base.gameObject == null)
		{
			return;
		}
		if (this.deadtime == 0f)
		{
			return;
		}
		if (Time.time < this.deadtime)
		{
			return;
		}
		Object.Destroy(base.gameObject);
		this.deadtime = 0f;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002628 File Offset: 0x00000828
	private void OnCollisionEnter(Collision col)
	{
		if (base.gameObject == null)
		{
			return;
		}
		if (this.coll)
		{
			return;
		}
		if (col.transform.name.Contains("Torch"))
		{
			return;
		}
		this.myRigidBody.isKinematic = true;
		Object.Destroy(this.myRigidBody);
		Object.Destroy(this.myCollider);
		this.myTransform.parent = col.collider.transform;
		this.coll = true;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x000026A4 File Offset: 0x000008A4
	private void OnTriggerEnter(Collider col)
	{
		if (base.gameObject == null)
		{
			return;
		}
		if (this.coll)
		{
			return;
		}
		if (col.transform.name.Contains("Torch"))
		{
			return;
		}
		this.myRigidBody.isKinematic = true;
		Object.Destroy(this.myRigidBody);
		Object.Destroy(this.myCollider);
		this.myTransform.parent = col.transform;
		this.coll = true;
	}

	// Token: 0x0400000E RID: 14
	private Rigidbody myRigidBody;

	// Token: 0x0400000F RID: 15
	private Collider myCollider;

	// Token: 0x04000010 RID: 16
	private Transform myTransform;

	// Token: 0x04000011 RID: 17
	private float deadtime;

	// Token: 0x04000012 RID: 18
	private bool coll;
}
