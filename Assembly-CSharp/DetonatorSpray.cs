using System;
using UnityEngine;

// Token: 0x0200013D RID: 317
[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Object Spray")]
public class DetonatorSpray : DetonatorComponent
{
	// Token: 0x06000AEA RID: 2794 RVA: 0x0000248C File Offset: 0x0000068C
	public override void Init()
	{
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x0008932D File Offset: 0x0008752D
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

	// Token: 0x06000AEC RID: 2796 RVA: 0x0008935C File Offset: 0x0008755C
	public override void Explode()
	{
		if (!this._delayedExplosionStarted)
		{
			this._explodeDelay = this.explodeDelayMin + Random.value * (this.explodeDelayMax - this.explodeDelayMin);
		}
		if (this._explodeDelay <= 0f)
		{
			int num = (int)(this.detail * (float)this.count);
			for (int i = 0; i < num; i++)
			{
				Vector3 b = Random.onUnitSphere * (this.startingRadius * this.size);
				Vector3 vector = new Vector3(this.velocity.x * this.size, this.velocity.y * this.size, this.velocity.z * this.size);
				GameObject gameObject = Object.Instantiate<GameObject>(this.sprayObject, base.transform.position + b, base.transform.rotation);
				gameObject.transform.parent = base.transform;
				this._tmpScale = this.minScale + Random.value * (this.maxScale - this.minScale);
				this._tmpScale *= this.size;
				gameObject.transform.localScale = new Vector3(this._tmpScale, this._tmpScale, this._tmpScale);
				if (base.MyDetonator().upwardsBias > 0f)
				{
					vector = new Vector3(vector.x / Mathf.Log(base.MyDetonator().upwardsBias), vector.y * Mathf.Log(base.MyDetonator().upwardsBias), vector.z / Mathf.Log(base.MyDetonator().upwardsBias));
				}
				gameObject.GetComponent<Rigidbody>().velocity = Vector3.Scale(b.normalized, vector);
				gameObject.GetComponent<Rigidbody>().velocity = Vector3.Scale(b.normalized, vector);
				Object.Destroy(gameObject, this.duration * this.timeScale);
				this._delayedExplosionStarted = false;
				this._explodeDelay = 0f;
			}
			return;
		}
		this._delayedExplosionStarted = true;
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x0008955F File Offset: 0x0008775F
	public void Reset()
	{
		this.velocity = new Vector3(15f, 15f, 15f);
	}

	// Token: 0x04001173 RID: 4467
	public GameObject sprayObject;

	// Token: 0x04001174 RID: 4468
	public int count = 10;

	// Token: 0x04001175 RID: 4469
	public float startingRadius;

	// Token: 0x04001176 RID: 4470
	public float minScale = 1f;

	// Token: 0x04001177 RID: 4471
	public float maxScale = 1f;

	// Token: 0x04001178 RID: 4472
	private bool _delayedExplosionStarted;

	// Token: 0x04001179 RID: 4473
	private float _explodeDelay;

	// Token: 0x0400117A RID: 4474
	private Vector3 _explosionPosition;

	// Token: 0x0400117B RID: 4475
	private float _tmpScale;
}
