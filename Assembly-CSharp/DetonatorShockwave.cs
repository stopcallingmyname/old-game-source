using System;
using UnityEngine;

// Token: 0x02000139 RID: 313
[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Shockwave")]
public class DetonatorShockwave : DetonatorComponent
{
	// Token: 0x06000ACE RID: 2766 RVA: 0x0008845E File Offset: 0x0008665E
	public override void Init()
	{
		this.FillMaterials(false);
		this.BuildShockwave();
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x0008846D File Offset: 0x0008666D
	public void FillMaterials(bool wipe)
	{
		if (!this.shockwaveMaterial || wipe)
		{
			this.shockwaveMaterial = base.MyDetonator().shockwaveMaterial;
		}
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x00088494 File Offset: 0x00086694
	public void BuildShockwave()
	{
		this._shockwave = new GameObject("Shockwave");
		this._shockwaveEmitter = this._shockwave.AddComponent<DetonatorBurstEmitter>();
		this._shockwave.transform.parent = base.transform;
		this._shockwave.transform.localRotation = Quaternion.identity;
		this._shockwave.transform.localPosition = this.localPosition;
		this._shockwaveEmitter.material = this.shockwaveMaterial;
		this._shockwaveEmitter.exponentialGrowth = false;
		this._shockwaveEmitter.useWorldSpace = base.MyDetonator().useWorldSpace;
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x00088538 File Offset: 0x00086738
	public void UpdateShockwave()
	{
		this._shockwave.transform.localPosition = Vector3.Scale(this.localPosition, new Vector3(this.size, this.size, this.size));
		this._shockwaveEmitter.color = this.color;
		this._shockwaveEmitter.duration = this.duration;
		this._shockwaveEmitter.durationVariation = this.duration * 0.1f;
		this._shockwaveEmitter.count = 1f;
		this._shockwaveEmitter.detail = 1f;
		this._shockwaveEmitter.particleSize = 25f;
		this._shockwaveEmitter.sizeVariation = 0f;
		this._shockwaveEmitter.velocity = new Vector3(0f, 0f, 0f);
		this._shockwaveEmitter.startRadius = 0f;
		this._shockwaveEmitter.sizeGrow = 202f;
		this._shockwaveEmitter.size = this.size;
		this._shockwaveEmitter.explodeDelayMin = this.explodeDelayMin;
		this._shockwaveEmitter.explodeDelayMax = this.explodeDelayMax;
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x00088664 File Offset: 0x00086864
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

	// Token: 0x06000AD3 RID: 2771 RVA: 0x000886C5 File Offset: 0x000868C5
	public override void Explode()
	{
		if (this.on)
		{
			this.UpdateShockwave();
			this._shockwaveEmitter.Explode();
		}
	}

	// Token: 0x0400114C RID: 4428
	private float _baseSize = 1f;

	// Token: 0x0400114D RID: 4429
	private float _baseDuration = 0.25f;

	// Token: 0x0400114E RID: 4430
	private Vector3 _baseVelocity = new Vector3(0f, 0f, 0f);

	// Token: 0x0400114F RID: 4431
	private Color _baseColor = Color.white;

	// Token: 0x04001150 RID: 4432
	private GameObject _shockwave;

	// Token: 0x04001151 RID: 4433
	private DetonatorBurstEmitter _shockwaveEmitter;

	// Token: 0x04001152 RID: 4434
	public Material shockwaveMaterial;
}
