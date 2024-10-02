using System;
using UnityEngine;

// Token: 0x0200013C RID: 316
[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Sparks")]
public class DetonatorSparks : DetonatorComponent
{
	// Token: 0x06000AE3 RID: 2787 RVA: 0x00088FE3 File Offset: 0x000871E3
	public override void Init()
	{
		this.FillMaterials(false);
		this.BuildSparks();
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x00088FF2 File Offset: 0x000871F2
	public void FillMaterials(bool wipe)
	{
		if (!this.sparksMaterial || wipe)
		{
			this.sparksMaterial = base.MyDetonator().sparksMaterial;
		}
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x00089018 File Offset: 0x00087218
	public void BuildSparks()
	{
		this._sparks = new GameObject("Sparks");
		this._sparksEmitter = this._sparks.AddComponent<DetonatorBurstEmitter>();
		this._sparks.transform.parent = base.transform;
		this._sparks.transform.localPosition = this.localPosition;
		this._sparks.transform.localRotation = Quaternion.identity;
		this._sparksEmitter.material = this.sparksMaterial;
		this._sparksEmitter.force = Physics.gravity / 3f;
		this._sparksEmitter.useExplicitColorAnimation = false;
		this._sparksEmitter.useWorldSpace = base.MyDetonator().useWorldSpace;
		this._sparksEmitter.upwardsBias = base.MyDetonator().upwardsBias;
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x000890EC File Offset: 0x000872EC
	public void UpdateSparks()
	{
		this._scaledDuration = this.duration * this.timeScale;
		this._sparksEmitter.color = this.color;
		this._sparksEmitter.duration = this._scaledDuration / 4f;
		this._sparksEmitter.durationVariation = this._scaledDuration;
		this._sparksEmitter.count = (float)((int)(this.detail * 50f));
		this._sparksEmitter.particleSize = 0.5f;
		this._sparksEmitter.sizeVariation = 0.25f;
		if (this._sparksEmitter.upwardsBias > 0f)
		{
			this._sparksEmitter.velocity = new Vector3(this.velocity.x / Mathf.Log(this._sparksEmitter.upwardsBias), this.velocity.y * Mathf.Log(this._sparksEmitter.upwardsBias), this.velocity.z / Mathf.Log(this._sparksEmitter.upwardsBias));
		}
		else
		{
			this._sparksEmitter.velocity = this.velocity;
		}
		this._sparksEmitter.startRadius = 0f;
		this._sparksEmitter.size = this.size;
		this._sparksEmitter.explodeDelayMin = this.explodeDelayMin;
		this._sparksEmitter.explodeDelayMax = this.explodeDelayMax;
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x0008924C File Offset: 0x0008744C
	public void Reset()
	{
		this.FillMaterials(true);
		this.on = true;
		this.size = this._baseSize;
		this.duration = this._baseDuration;
		this.explodeDelayMin = 0f;
		this.explodeDelayMax = 0f;
		this.color = this._baseColor;
		this.velocity = this._baseVelocity;
		this.force = this._baseForce;
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x000892B9 File Offset: 0x000874B9
	public override void Explode()
	{
		if (this.on)
		{
			this.UpdateSparks();
			this._sparksEmitter.Explode();
		}
	}

	// Token: 0x0400116A RID: 4458
	private float _baseSize = 1f;

	// Token: 0x0400116B RID: 4459
	private float _baseDuration = 4f;

	// Token: 0x0400116C RID: 4460
	private Vector3 _baseVelocity = new Vector3(400f, 400f, 400f);

	// Token: 0x0400116D RID: 4461
	private Color _baseColor = Color.white;

	// Token: 0x0400116E RID: 4462
	private Vector3 _baseForce = Physics.gravity;

	// Token: 0x0400116F RID: 4463
	private float _scaledDuration;

	// Token: 0x04001170 RID: 4464
	private GameObject _sparks;

	// Token: 0x04001171 RID: 4465
	private DetonatorBurstEmitter _sparksEmitter;

	// Token: 0x04001172 RID: 4466
	public Material sparksMaterial;
}
