using System;
using UnityEngine;

// Token: 0x02000135 RID: 309
[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Force")]
public class DetonatorForce : DetonatorComponent
{
	// Token: 0x06000AB7 RID: 2743 RVA: 0x0000248C File Offset: 0x0000068C
	public override void Init()
	{
	}

	// Token: 0x06000AB8 RID: 2744 RVA: 0x0008795F File Offset: 0x00085B5F
	private void Update()
	{
		if (this._delayedExplosionStarted)
		{
			this._explodeDelay -= Time.deltaTime;
			if (this._explodeDelay <= 0f)
			{
				this.Explode();
			}
		}
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x00087990 File Offset: 0x00085B90
	public override void Explode()
	{
		if (!this.on)
		{
			return;
		}
		if (this.detailThreshold > this.detail)
		{
			return;
		}
		if (!this._delayedExplosionStarted)
		{
			this._explodeDelay = this.explodeDelayMin + Random.value * (this.explodeDelayMax - this.explodeDelayMin);
		}
		if (this._explodeDelay <= 0f)
		{
			this._explosionPosition = base.transform.position;
			this._colliders = Physics.OverlapSphere(this._explosionPosition, this.radius);
			foreach (Collider collider in this._colliders)
			{
				if (collider && collider.GetComponent<Rigidbody>())
				{
					collider.GetComponent<Rigidbody>().AddExplosionForce(this.power * this.size, this._explosionPosition, this.radius * this.size, 4f * base.MyDetonator().upwardsBias * this.size);
					collider.gameObject.SendMessage("OnDetonatorForceHit", null, SendMessageOptions.DontRequireReceiver);
					if (this.fireObject)
					{
						if (collider.transform.Find(this.fireObject.name + "(Clone)"))
						{
							return;
						}
						this._tempFireObject = Object.Instantiate<GameObject>(this.fireObject, base.transform.position, base.transform.rotation);
						this._tempFireObject.transform.parent = collider.transform;
						this._tempFireObject.transform.localPosition = new Vector3(0f, 0f, 0f);
					}
				}
			}
			this._delayedExplosionStarted = false;
			this._explodeDelay = 0f;
			return;
		}
		this._delayedExplosionStarted = true;
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x00087B57 File Offset: 0x00085D57
	public void Reset()
	{
		this.radius = this._baseRadius;
		this.power = this._basePower;
	}

	// Token: 0x04001122 RID: 4386
	private float _baseRadius = 50f;

	// Token: 0x04001123 RID: 4387
	private float _basePower = 4000f;

	// Token: 0x04001124 RID: 4388
	private float _scaledRange;

	// Token: 0x04001125 RID: 4389
	private float _scaledIntensity;

	// Token: 0x04001126 RID: 4390
	private bool _delayedExplosionStarted;

	// Token: 0x04001127 RID: 4391
	private float _explodeDelay;

	// Token: 0x04001128 RID: 4392
	public float radius;

	// Token: 0x04001129 RID: 4393
	public float power;

	// Token: 0x0400112A RID: 4394
	public GameObject fireObject;

	// Token: 0x0400112B RID: 4395
	public float fireObjectLife;

	// Token: 0x0400112C RID: 4396
	private Collider[] _colliders;

	// Token: 0x0400112D RID: 4397
	private GameObject _tempFireObject;

	// Token: 0x0400112E RID: 4398
	private Vector3 _explosionPosition;
}
