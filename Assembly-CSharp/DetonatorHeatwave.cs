using System;
using UnityEngine;

// Token: 0x02000137 RID: 311
[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Heatwave (Pro Only)")]
public class DetonatorHeatwave : DetonatorComponent
{
	// Token: 0x06000AC4 RID: 2756 RVA: 0x0000248C File Offset: 0x0000068C
	public override void Init()
	{
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x00087FAC File Offset: 0x000861AC
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
		if (this._heatwave)
		{
			this._heatwave.transform.rotation = Quaternion.FromToRotation(Vector3.up, Camera.main.transform.position - this._heatwave.transform.position);
			this._heatwave.transform.localPosition = this.localPosition + Vector3.forward * this.zOffset;
			this._elapsedTime += Time.deltaTime;
			this._normalizedTime = this._elapsedTime / this.duration;
			this.s = Mathf.Lerp(this._startSize, this._maxSize, this._normalizedTime);
			this._heatwave.GetComponent<Renderer>().material.SetFloat("_BumpAmt", (1f - this._normalizedTime) * this.distortion);
			this._heatwave.gameObject.transform.localScale = new Vector3(this.s, this.s, this.s);
			if (this._elapsedTime > this.duration)
			{
				Object.Destroy(this._heatwave.gameObject);
			}
		}
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x00088118 File Offset: 0x00086318
	public override void Explode()
	{
		if (SystemInfo.supportsImageEffects)
		{
			if (this.detailThreshold > this.detail || !this.on)
			{
				return;
			}
			if (!this._delayedExplosionStarted)
			{
				this._explodeDelay = this.explodeDelayMin + Random.value * (this.explodeDelayMax - this.explodeDelayMin);
			}
			if (this._explodeDelay <= 0f)
			{
				this._startSize = 0f;
				this._maxSize = this.size * 10f;
				this._material = new Material(Shader.Find("HeatDistort"));
				this._heatwave = GameObject.CreatePrimitive(PrimitiveType.Plane);
				this._heatwave.name = "Heatwave";
				this._heatwave.transform.parent = base.transform;
				Object.Destroy(this._heatwave.GetComponent(typeof(MeshCollider)));
				if (!this.heatwaveMaterial)
				{
					this.heatwaveMaterial = base.MyDetonator().heatwaveMaterial;
				}
				this._material.CopyPropertiesFromMaterial(this.heatwaveMaterial);
				this._heatwave.GetComponent<Renderer>().material = this._material;
				this._heatwave.transform.parent = base.transform;
				this._delayedExplosionStarted = false;
				this._explodeDelay = 0f;
				return;
			}
			this._delayedExplosionStarted = true;
		}
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x00088271 File Offset: 0x00086471
	public void Reset()
	{
		this.duration = this._baseDuration;
	}

	// Token: 0x04001137 RID: 4407
	private GameObject _heatwave;

	// Token: 0x04001138 RID: 4408
	private float s;

	// Token: 0x04001139 RID: 4409
	private float _startSize;

	// Token: 0x0400113A RID: 4410
	private float _maxSize;

	// Token: 0x0400113B RID: 4411
	private float _baseDuration = 0.25f;

	// Token: 0x0400113C RID: 4412
	private bool _delayedExplosionStarted;

	// Token: 0x0400113D RID: 4413
	private float _explodeDelay;

	// Token: 0x0400113E RID: 4414
	public float zOffset = 0.5f;

	// Token: 0x0400113F RID: 4415
	public float distortion = 64f;

	// Token: 0x04001140 RID: 4416
	private float _elapsedTime;

	// Token: 0x04001141 RID: 4417
	private float _normalizedTime;

	// Token: 0x04001142 RID: 4418
	public Material heatwaveMaterial;

	// Token: 0x04001143 RID: 4419
	private Material _material;
}
