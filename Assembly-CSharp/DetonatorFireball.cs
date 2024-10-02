using System;
using UnityEngine;

// Token: 0x02000134 RID: 308
[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Fireball")]
public class DetonatorFireball : DetonatorComponent
{
	// Token: 0x06000AAC RID: 2732 RVA: 0x00086FFE File Offset: 0x000851FE
	public override void Init()
	{
		this.FillMaterials(false);
		this.BuildFireballA();
		this.BuildFireballB();
		this.BuildFireShadow();
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0008701C File Offset: 0x0008521C
	public void FillMaterials(bool wipe)
	{
		if (!this.fireballAMaterial || wipe)
		{
			this.fireballAMaterial = base.MyDetonator().fireballAMaterial;
		}
		if (!this.fireballBMaterial || wipe)
		{
			this.fireballBMaterial = base.MyDetonator().fireballBMaterial;
		}
		if (!this.fireShadowMaterial || wipe)
		{
			if ((double)Random.value > 0.5)
			{
				this.fireShadowMaterial = base.MyDetonator().smokeAMaterial;
				return;
			}
			this.fireShadowMaterial = base.MyDetonator().smokeBMaterial;
		}
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x000870B8 File Offset: 0x000852B8
	public void BuildFireballA()
	{
		this._fireballA = new GameObject("FireballA");
		this._fireballAEmitter = this._fireballA.AddComponent<DetonatorBurstEmitter>();
		this._fireballA.transform.parent = base.transform;
		this._fireballA.transform.localRotation = Quaternion.identity;
		this._fireballAEmitter.material = this.fireballAMaterial;
		this._fireballAEmitter.useWorldSpace = base.MyDetonator().useWorldSpace;
		this._fireballAEmitter.upwardsBias = base.MyDetonator().upwardsBias;
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x00087150 File Offset: 0x00085350
	public void UpdateFireballA()
	{
		this._fireballA.transform.localPosition = Vector3.Scale(this.localPosition, new Vector3(this.size, this.size, this.size));
		this._fireballAEmitter.color = this.color;
		this._fireballAEmitter.duration = this.duration * 0.5f;
		this._fireballAEmitter.durationVariation = this.duration * 0.5f;
		this._fireballAEmitter.count = 2f;
		this._fireballAEmitter.timeScale = this.timeScale;
		this._fireballAEmitter.detail = this.detail;
		this._fireballAEmitter.particleSize = 14f;
		this._fireballAEmitter.sizeVariation = 3f;
		this._fireballAEmitter.velocity = this.velocity;
		this._fireballAEmitter.startRadius = 4f;
		this._fireballAEmitter.size = this.size;
		this._fireballAEmitter.useExplicitColorAnimation = true;
		Color b = new Color(1f, 1f, 1f, 0.5f);
		Color b2 = new Color(0.6f, 0.15f, 0.15f, 0.3f);
		Color color = new Color(0.1f, 0.2f, 0.45f, 0f);
		this._fireballAEmitter.colorAnimation[0] = Color.Lerp(this.color, b, 0.8f);
		this._fireballAEmitter.colorAnimation[1] = Color.Lerp(this.color, b, 0.5f);
		this._fireballAEmitter.colorAnimation[2] = this.color;
		this._fireballAEmitter.colorAnimation[3] = Color.Lerp(this.color, b2, 0.7f);
		this._fireballAEmitter.colorAnimation[4] = color;
		this._fireballAEmitter.explodeDelayMin = this.explodeDelayMin;
		this._fireballAEmitter.explodeDelayMax = this.explodeDelayMax;
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x00087360 File Offset: 0x00085560
	public void BuildFireballB()
	{
		this._fireballB = new GameObject("FireballB");
		this._fireballBEmitter = this._fireballB.AddComponent<DetonatorBurstEmitter>();
		this._fireballB.transform.parent = base.transform;
		this._fireballB.transform.localRotation = Quaternion.identity;
		this._fireballBEmitter.material = this.fireballBMaterial;
		this._fireballBEmitter.useWorldSpace = base.MyDetonator().useWorldSpace;
		this._fireballBEmitter.upwardsBias = base.MyDetonator().upwardsBias;
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x000873F8 File Offset: 0x000855F8
	public void UpdateFireballB()
	{
		this._fireballB.transform.localPosition = Vector3.Scale(this.localPosition, new Vector3(this.size, this.size, this.size));
		this._fireballBEmitter.color = this.color;
		this._fireballBEmitter.duration = this.duration * 0.5f;
		this._fireballBEmitter.durationVariation = this.duration * 0.5f;
		this._fireballBEmitter.count = 2f;
		this._fireballBEmitter.timeScale = this.timeScale;
		this._fireballBEmitter.detail = this.detail;
		this._fireballBEmitter.particleSize = 10f;
		this._fireballBEmitter.sizeVariation = 6f;
		this._fireballBEmitter.velocity = this.velocity;
		this._fireballBEmitter.startRadius = 4f;
		this._fireballBEmitter.size = this.size;
		this._fireballBEmitter.useExplicitColorAnimation = true;
		Color b = new Color(1f, 1f, 1f, 0.5f);
		Color b2 = new Color(0.6f, 0.15f, 0.15f, 0.3f);
		Color color = new Color(0.1f, 0.2f, 0.45f, 0f);
		this._fireballBEmitter.colorAnimation[0] = Color.Lerp(this.color, b, 0.8f);
		this._fireballBEmitter.colorAnimation[1] = Color.Lerp(this.color, b, 0.5f);
		this._fireballBEmitter.colorAnimation[2] = this.color;
		this._fireballBEmitter.colorAnimation[3] = Color.Lerp(this.color, b2, 0.7f);
		this._fireballBEmitter.colorAnimation[4] = color;
		this._fireballBEmitter.explodeDelayMin = this.explodeDelayMin;
		this._fireballBEmitter.explodeDelayMax = this.explodeDelayMax;
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x00087608 File Offset: 0x00085808
	public void BuildFireShadow()
	{
		this._fireShadow = new GameObject("FireShadow");
		this._fireShadowEmitter = this._fireShadow.AddComponent<DetonatorBurstEmitter>();
		this._fireShadow.transform.parent = base.transform;
		this._fireShadow.transform.localRotation = Quaternion.identity;
		this._fireShadowEmitter.material = this.fireShadowMaterial;
		this._fireShadowEmitter.useWorldSpace = base.MyDetonator().useWorldSpace;
		this._fireShadowEmitter.upwardsBias = base.MyDetonator().upwardsBias;
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x000876A0 File Offset: 0x000858A0
	public void UpdateFireShadow()
	{
		this._fireShadow.transform.localPosition = Vector3.Scale(this.localPosition, new Vector3(this.size, this.size, this.size));
		this._fireShadow.transform.LookAt(Camera.main.transform);
		this._fireShadow.transform.localPosition = -(Vector3.forward * 1f);
		this._fireShadowEmitter.color = new Color(0.1f, 0.1f, 0.1f, 0.6f);
		this._fireShadowEmitter.duration = this.duration * 0.5f;
		this._fireShadowEmitter.durationVariation = this.duration * 0.5f;
		this._fireShadowEmitter.timeScale = this.timeScale;
		this._fireShadowEmitter.detail = 1f;
		this._fireShadowEmitter.particleSize = 13f;
		this._fireShadowEmitter.velocity = this.velocity;
		this._fireShadowEmitter.sizeVariation = 1f;
		this._fireShadowEmitter.count = 4f;
		this._fireShadowEmitter.startRadius = 6f;
		this._fireShadowEmitter.size = this.size;
		this._fireShadowEmitter.explodeDelayMin = this.explodeDelayMin;
		this._fireShadowEmitter.explodeDelayMax = this.explodeDelayMax;
	}

	// Token: 0x06000AB4 RID: 2740 RVA: 0x00087814 File Offset: 0x00085A14
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
	}

	// Token: 0x06000AB5 RID: 2741 RVA: 0x00087878 File Offset: 0x00085A78
	public override void Explode()
	{
		if (this.detailThreshold > this.detail)
		{
			return;
		}
		if (this.on)
		{
			this.UpdateFireballA();
			this.UpdateFireballB();
			this.UpdateFireShadow();
			if (this.drawFireballA)
			{
				this._fireballAEmitter.Explode();
			}
			if (this.drawFireballB)
			{
				this._fireballBEmitter.Explode();
			}
			if (this.drawFireShadow)
			{
				this._fireShadowEmitter.Explode();
			}
		}
	}

	// Token: 0x04001110 RID: 4368
	private float _baseSize = 1f;

	// Token: 0x04001111 RID: 4369
	private float _baseDuration = 3f;

	// Token: 0x04001112 RID: 4370
	private Color _baseColor = new Color(1f, 0.423f, 0f, 0.5f);

	// Token: 0x04001113 RID: 4371
	private Vector3 _baseVelocity = new Vector3(60f, 60f, 60f);

	// Token: 0x04001114 RID: 4372
	private float _scaledDuration;

	// Token: 0x04001115 RID: 4373
	private GameObject _fireballA;

	// Token: 0x04001116 RID: 4374
	private DetonatorBurstEmitter _fireballAEmitter;

	// Token: 0x04001117 RID: 4375
	public Material fireballAMaterial;

	// Token: 0x04001118 RID: 4376
	private GameObject _fireballB;

	// Token: 0x04001119 RID: 4377
	private DetonatorBurstEmitter _fireballBEmitter;

	// Token: 0x0400111A RID: 4378
	public Material fireballBMaterial;

	// Token: 0x0400111B RID: 4379
	private GameObject _fireShadow;

	// Token: 0x0400111C RID: 4380
	private DetonatorBurstEmitter _fireShadowEmitter;

	// Token: 0x0400111D RID: 4381
	public Material fireShadowMaterial;

	// Token: 0x0400111E RID: 4382
	public bool drawFireballA = true;

	// Token: 0x0400111F RID: 4383
	public bool drawFireballB = true;

	// Token: 0x04001120 RID: 4384
	public bool drawFireShadow = true;

	// Token: 0x04001121 RID: 4385
	private Color _detailAdjustedColor;
}
