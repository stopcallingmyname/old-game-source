using System;
using UnityEngine;

// Token: 0x02000136 RID: 310
[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Glow")]
public class DetonatorGlow : DetonatorComponent
{
	// Token: 0x06000ABC RID: 2748 RVA: 0x00087B8F File Offset: 0x00085D8F
	public override void Init()
	{
		this.FillMaterials(false);
		this.BuildGlow();
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x00087B9E File Offset: 0x00085D9E
	public void FillMaterials(bool wipe)
	{
		if (!this.glowMaterial || wipe)
		{
			this.glowMaterial = base.MyDetonator().glowMaterial;
		}
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x00087BC4 File Offset: 0x00085DC4
	public void BuildGlow()
	{
		this._glow = new GameObject("Glow");
		this._glowEmitter = this._glow.AddComponent<DetonatorBurstEmitter>();
		this._glow.transform.parent = base.transform;
		this._glow.transform.localPosition = this.localPosition;
		this._glowEmitter.material = this.glowMaterial;
		this._glowEmitter.exponentialGrowth = false;
		this._glowEmitter.useExplicitColorAnimation = true;
		this._glowEmitter.useWorldSpace = base.MyDetonator().useWorldSpace;
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x00087C60 File Offset: 0x00085E60
	public void UpdateGlow()
	{
		this._glow.transform.localPosition = Vector3.Scale(this.localPosition, new Vector3(this.size, this.size, this.size));
		this._glowEmitter.color = this.color;
		this._glowEmitter.duration = this.duration;
		this._glowEmitter.timeScale = this.timeScale;
		this._glowEmitter.count = 1f;
		this._glowEmitter.particleSize = 65f;
		this._glowEmitter.randomRotation = false;
		this._glowEmitter.sizeVariation = 0f;
		this._glowEmitter.velocity = new Vector3(0f, 0f, 0f);
		this._glowEmitter.startRadius = 0f;
		this._glowEmitter.sizeGrow = 0f;
		this._glowEmitter.size = this.size;
		this._glowEmitter.explodeDelayMin = this.explodeDelayMin;
		this._glowEmitter.explodeDelayMax = this.explodeDelayMax;
		Color color = Color.Lerp(this.color, new Color(0.5f, 0.1f, 0.1f, 1f), 0.5f);
		color.a = 0.9f;
		Color color2 = Color.Lerp(this.color, new Color(0.6f, 0.3f, 0.3f, 1f), 0.5f);
		color2.a = 0.8f;
		Color color3 = Color.Lerp(this.color, new Color(0.7f, 0.3f, 0.3f, 1f), 0.5f);
		color3.a = 0.5f;
		Color color4 = Color.Lerp(this.color, new Color(0.4f, 0.3f, 0.4f, 1f), 0.5f);
		color4.a = 0.2f;
		Color color5 = new Color(0.1f, 0.1f, 0.4f, 0f);
		this._glowEmitter.colorAnimation[0] = color;
		this._glowEmitter.colorAnimation[1] = color2;
		this._glowEmitter.colorAnimation[2] = color3;
		this._glowEmitter.colorAnimation[3] = color4;
		this._glowEmitter.colorAnimation[4] = color5;
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x0000248C File Offset: 0x0000068C
	private void Update()
	{
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x00087ED0 File Offset: 0x000860D0
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

	// Token: 0x06000AC2 RID: 2754 RVA: 0x00087F31 File Offset: 0x00086131
	public override void Explode()
	{
		if (this.detailThreshold > this.detail)
		{
			return;
		}
		if (this.on)
		{
			this.UpdateGlow();
			this._glowEmitter.Explode();
		}
	}

	// Token: 0x0400112F RID: 4399
	private float _baseSize = 1f;

	// Token: 0x04001130 RID: 4400
	private float _baseDuration = 1f;

	// Token: 0x04001131 RID: 4401
	private Vector3 _baseVelocity = new Vector3(0f, 0f, 0f);

	// Token: 0x04001132 RID: 4402
	private Color _baseColor = Color.black;

	// Token: 0x04001133 RID: 4403
	private float _scaledDuration;

	// Token: 0x04001134 RID: 4404
	private GameObject _glow;

	// Token: 0x04001135 RID: 4405
	private DetonatorBurstEmitter _glowEmitter;

	// Token: 0x04001136 RID: 4406
	public Material glowMaterial;
}
