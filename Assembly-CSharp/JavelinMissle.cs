using System;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class JavelinMissle : MonoBehaviour
{
	// Token: 0x060002E8 RID: 744 RVA: 0x000386AB File Offset: 0x000368AB
	public void Awake()
	{
		this._thisTransform = base.transform;
		this.explosionPrefab = base.gameObject;
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x000386E0 File Offset: 0x000368E0
	public void Update()
	{
		this.explosionTime -= Time.deltaTime;
		if (this.explosionTime <= 0f)
		{
			this.Explode();
			return;
		}
		Vector3 b = this._thisTransform.forward * this.speed * Time.deltaTime;
		float num = 0f;
		Vector3 to;
		if (this.target != null && this.timeTarget == null)
		{
			if (Vector3.Distance(this.target.position, this._thisTransform.position) > 15f)
			{
				num = Vector3.Distance(this.target.position, this._thisTransform.position) / 2f;
			}
			to = this.target.position - this._thisTransform.position + new Vector3(0f, num + 1.75f, 0f);
		}
		else if (this.timeTarget != null)
		{
			if (Vector3.Distance(this.timeTarget.position, this._thisTransform.position) > 15f)
			{
				num = Vector3.Distance(this.timeTarget.position, this._thisTransform.position) / 2f;
			}
			to = this.timeTarget.position - this._thisTransform.position + new Vector3(0f, num + 1.75f, 0f);
		}
		else
		{
			if (Vector3.Distance(new Vector3(128f, 64f, 128f), this._thisTransform.position) > 15f)
			{
				num = Vector3.Distance(new Vector3(128f, 64f, 128f), this._thisTransform.position) / 2f;
			}
			to = new Vector3(128f, 64f, 128f) - this._thisTransform.position + new Vector3(0f, num + 1.75f, 0f);
		}
		float num2 = this.turnSpeed * Time.deltaTime;
		float num3 = Vector3.Angle(this._thisTransform.forward, to);
		if (num3 <= num2)
		{
			this._thisTransform.forward = to.normalized;
		}
		else
		{
			this._thisTransform.forward = Vector3.Slerp(this._thisTransform.forward, to.normalized, num2 / num3);
		}
		if (to.magnitude < b.magnitude)
		{
			this.Explode();
			return;
		}
		this._thisTransform.position += b;
	}

	// Token: 0x060002EA RID: 746 RVA: 0x00038990 File Offset: 0x00036B90
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
		base.gameObject.GetComponentInChildren<ParticleSystem>().loop = false;
		base.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
		if (this.timeTarget == null && this.id == this.cscl.myindex)
		{
			this.cscl.send_detonateent(this.uid, this._thisTransform.position);
		}
		if (this.targetPlayerID == this.cscl.myindex)
		{
			if (this.targetEntClassID != CONST.ENTS.ENT_JEEP)
			{
				TankController tankController = (TankController)Object.FindObjectOfType(typeof(TankController));
				if (tankController.enabled)
				{
					tankController.javelinAIM(3);
					tankController.missle = null;
				}
			}
			else
			{
				CarController carController = (CarController)Object.FindObjectOfType(typeof(CarController));
				if (carController.enabled)
				{
					carController.javelinAIM(3);
					carController.missle = null;
				}
			}
		}
		this.KillSelf();
	}

	// Token: 0x060002EB RID: 747 RVA: 0x00038AF5 File Offset: 0x00036CF5
	private void KillSelf()
	{
		if (base.gameObject == null)
		{
			return;
		}
		Object.Destroy(base.gameObject);
		EntManager.DeleteEnt(this.entid);
	}

	// Token: 0x060002EC RID: 748 RVA: 0x00038B1C File Offset: 0x00036D1C
	private void OnCollisionEnter(Collision collision)
	{
		this.Explode();
	}

	// Token: 0x060002ED RID: 749 RVA: 0x00038B24 File Offset: 0x00036D24
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

	// Token: 0x0400057A RID: 1402
	public Transform target;

	// Token: 0x0400057B RID: 1403
	public Transform timeTarget;

	// Token: 0x0400057C RID: 1404
	public GameObject explosionPrefab;

	// Token: 0x0400057D RID: 1405
	public float speed = 15f;

	// Token: 0x0400057E RID: 1406
	public float turnSpeed = 100f;

	// Token: 0x0400057F RID: 1407
	public float explosionTime = 10f;

	// Token: 0x04000580 RID: 1408
	public int id;

	// Token: 0x04000581 RID: 1409
	public int uid;

	// Token: 0x04000582 RID: 1410
	public int entid;

	// Token: 0x04000583 RID: 1411
	public int targetPlayerID;

	// Token: 0x04000584 RID: 1412
	public int targetEntClassID;

	// Token: 0x04000585 RID: 1413
	private Client cscl;

	// Token: 0x04000586 RID: 1414
	private EntManager entmanager;

	// Token: 0x04000587 RID: 1415
	private Transform _thisTransform;

	// Token: 0x04000588 RID: 1416
	private bool exploded;
}
