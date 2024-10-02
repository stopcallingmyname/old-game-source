using System;
using UnityEngine;

// Token: 0x02000132 RID: 306
[RequireComponent(typeof(Detonator))]
public class DetonatorCloudRing : DetonatorComponent
{
	// Token: 0x06000AA0 RID: 2720 RVA: 0x00086B3B File Offset: 0x00084D3B
	public override void Init()
	{
		this.FillMaterials(false);
		this.BuildCloudRing();
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x00086B4A File Offset: 0x00084D4A
	public void FillMaterials(bool wipe)
	{
		if (!this.cloudRingMaterial || wipe)
		{
			this.cloudRingMaterial = base.MyDetonator().smokeBMaterial;
		}
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x00086B70 File Offset: 0x00084D70
	public void BuildCloudRing()
	{
		this._cloudRing = new GameObject("CloudRing");
		this._cloudRingEmitter = this._cloudRing.AddComponent<DetonatorBurstEmitter>();
		this._cloudRing.transform.parent = base.transform;
		this._cloudRing.transform.localPosition = this.localPosition;
		this._cloudRingEmitter.material = this.cloudRingMaterial;
		this._cloudRingEmitter.useExplicitColorAnimation = true;
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x00086BE8 File Offset: 0x00084DE8
	public void UpdateCloudRing()
	{
		this._cloudRing.transform.localPosition = Vector3.Scale(this.localPosition, new Vector3(this.size, this.size, this.size));
		this._cloudRingEmitter.color = this.color;
		this._cloudRingEmitter.duration = this.duration;
		this._cloudRingEmitter.durationVariation = this.duration / 4f;
		this._cloudRingEmitter.count = (float)((int)(this.detail * 50f));
		this._cloudRingEmitter.particleSize = 10f;
		this._cloudRingEmitter.sizeVariation = 2f;
		this._cloudRingEmitter.velocity = this.velocity;
		this._cloudRingEmitter.startRadius = 3f;
		this._cloudRingEmitter.size = this.size;
		this._cloudRingEmitter.force = this.force;
		this._cloudRingEmitter.explodeDelayMin = this.explodeDelayMin;
		this._cloudRingEmitter.explodeDelayMax = this.explodeDelayMax;
		Color color = Color.Lerp(this.color, new Color(0.2f, 0.2f, 0.2f, 0.6f), 0.5f);
		Color color2 = new Color(0.2f, 0.2f, 0.2f, 0.5f);
		Color color3 = new Color(0.2f, 0.2f, 0.2f, 0.3f);
		Color color4 = new Color(0.2f, 0.2f, 0.2f, 0f);
		this._cloudRingEmitter.colorAnimation[0] = color;
		this._cloudRingEmitter.colorAnimation[1] = color2;
		this._cloudRingEmitter.colorAnimation[2] = color2;
		this._cloudRingEmitter.colorAnimation[3] = color3;
		this._cloudRingEmitter.colorAnimation[4] = color4;
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x00086DD4 File Offset: 0x00084FD4
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

	// Token: 0x06000AA5 RID: 2725 RVA: 0x00086E41 File Offset: 0x00085041
	public override void Explode()
	{
		if (this.on)
		{
			this.UpdateCloudRing();
			this._cloudRingEmitter.Explode();
		}
	}

	// Token: 0x040010F4 RID: 4340
	private float _baseSize = 1f;

	// Token: 0x040010F5 RID: 4341
	private float _baseDuration = 5f;

	// Token: 0x040010F6 RID: 4342
	private Vector3 _baseVelocity = new Vector3(155f, 5f, 155f);

	// Token: 0x040010F7 RID: 4343
	private Color _baseColor = Color.white;

	// Token: 0x040010F8 RID: 4344
	private Vector3 _baseForce = new Vector3(0.162f, 2.56f, 0f);

	// Token: 0x040010F9 RID: 4345
	private GameObject _cloudRing;

	// Token: 0x040010FA RID: 4346
	private DetonatorBurstEmitter _cloudRingEmitter;

	// Token: 0x040010FB RID: 4347
	public Material cloudRingMaterial;
}
