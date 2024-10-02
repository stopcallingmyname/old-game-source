using System;
using UnityEngine;

// Token: 0x0200013A RID: 314
[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Smoke")]
public class DetonatorSmoke : DetonatorComponent
{
	// Token: 0x06000AD5 RID: 2773 RVA: 0x0008872E File Offset: 0x0008692E
	public override void Init()
	{
		this.FillMaterials(false);
		this.BuildSmokeA();
		this.BuildSmokeB();
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x00088744 File Offset: 0x00086944
	public void FillMaterials(bool wipe)
	{
		if (!this.smokeAMaterial || wipe)
		{
			this.smokeAMaterial = base.MyDetonator().smokeAMaterial;
		}
		if (!this.smokeBMaterial || wipe)
		{
			this.smokeBMaterial = base.MyDetonator().smokeBMaterial;
		}
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x00088798 File Offset: 0x00086998
	public void BuildSmokeA()
	{
		this._smokeA = new GameObject("SmokeA");
		this._smokeAEmitter = this._smokeA.AddComponent<DetonatorBurstEmitter>();
		this._smokeA.transform.parent = base.transform;
		this._smokeA.transform.localPosition = this.localPosition;
		this._smokeA.transform.localRotation = Quaternion.identity;
		this._smokeAEmitter.material = this.smokeAMaterial;
		this._smokeAEmitter.exponentialGrowth = false;
		this._smokeAEmitter.sizeGrow = 0.095f;
		this._smokeAEmitter.useWorldSpace = base.MyDetonator().useWorldSpace;
		this._smokeAEmitter.upwardsBias = base.MyDetonator().upwardsBias;
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x00088860 File Offset: 0x00086A60
	public void UpdateSmokeA()
	{
		this._smokeA.transform.localPosition = Vector3.Scale(this.localPosition, new Vector3(this.size, this.size, this.size));
		this._smokeA.transform.LookAt(Camera.main.transform);
		this._smokeA.transform.localPosition = -(Vector3.forward * -1.5f);
		this._smokeAEmitter.color = this.color;
		this._smokeAEmitter.duration = this.duration * 0.5f;
		this._smokeAEmitter.durationVariation = 0f;
		this._smokeAEmitter.timeScale = this.timeScale;
		this._smokeAEmitter.count = 4f;
		this._smokeAEmitter.particleSize = 25f;
		this._smokeAEmitter.sizeVariation = 3f;
		this._smokeAEmitter.velocity = this.velocity;
		this._smokeAEmitter.startRadius = 10f;
		this._smokeAEmitter.size = this.size;
		this._smokeAEmitter.useExplicitColorAnimation = true;
		this._smokeAEmitter.explodeDelayMin = this.explodeDelayMin;
		this._smokeAEmitter.explodeDelayMax = this.explodeDelayMax;
		Color color = new Color(0.2f, 0.2f, 0.2f, 0.4f);
		Color color2 = new Color(0.2f, 0.2f, 0.2f, 0.7f);
		Color color3 = new Color(0.2f, 0.2f, 0.2f, 0.4f);
		Color color4 = new Color(0.2f, 0.2f, 0.2f, 0f);
		this._smokeAEmitter.colorAnimation[0] = color;
		this._smokeAEmitter.colorAnimation[1] = color2;
		this._smokeAEmitter.colorAnimation[2] = color2;
		this._smokeAEmitter.colorAnimation[3] = color3;
		this._smokeAEmitter.colorAnimation[4] = color4;
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x00088A7C File Offset: 0x00086C7C
	public void BuildSmokeB()
	{
		this._smokeB = new GameObject("SmokeB");
		this._smokeBEmitter = this._smokeB.AddComponent<DetonatorBurstEmitter>();
		this._smokeB.transform.parent = base.transform;
		this._smokeB.transform.localPosition = this.localPosition;
		this._smokeB.transform.localRotation = Quaternion.identity;
		this._smokeBEmitter.material = this.smokeBMaterial;
		this._smokeBEmitter.exponentialGrowth = false;
		this._smokeBEmitter.sizeGrow = 0.095f;
		this._smokeBEmitter.useWorldSpace = base.MyDetonator().useWorldSpace;
		this._smokeBEmitter.upwardsBias = base.MyDetonator().upwardsBias;
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x00088B44 File Offset: 0x00086D44
	public void UpdateSmokeB()
	{
		this._smokeB.transform.localPosition = Vector3.Scale(this.localPosition, new Vector3(this.size, this.size, this.size));
		this._smokeB.transform.LookAt(Camera.main.transform);
		this._smokeB.transform.localPosition = -(Vector3.forward * -1f);
		this._smokeBEmitter.color = this.color;
		this._smokeBEmitter.duration = this.duration * 0.5f;
		this._smokeBEmitter.durationVariation = 0f;
		this._smokeBEmitter.count = 2f;
		this._smokeBEmitter.particleSize = 25f;
		this._smokeBEmitter.sizeVariation = 3f;
		this._smokeBEmitter.velocity = this.velocity;
		this._smokeBEmitter.startRadius = 10f;
		this._smokeBEmitter.size = this.size;
		this._smokeBEmitter.useExplicitColorAnimation = true;
		this._smokeBEmitter.explodeDelayMin = this.explodeDelayMin;
		this._smokeBEmitter.explodeDelayMax = this.explodeDelayMax;
		Color color = new Color(0.2f, 0.2f, 0.2f, 0.4f);
		Color color2 = new Color(0.2f, 0.2f, 0.2f, 0.7f);
		Color color3 = new Color(0.2f, 0.2f, 0.2f, 0.4f);
		Color color4 = new Color(0.2f, 0.2f, 0.2f, 0f);
		this._smokeBEmitter.colorAnimation[0] = color;
		this._smokeBEmitter.colorAnimation[1] = color2;
		this._smokeBEmitter.colorAnimation[2] = color2;
		this._smokeBEmitter.colorAnimation[3] = color3;
		this._smokeBEmitter.colorAnimation[4] = color4;
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x00088D50 File Offset: 0x00086F50
	public void Reset()
	{
		this.FillMaterials(true);
		this.on = true;
		this.size = 1f;
		this.duration = 8f;
		this.explodeDelayMin = 0f;
		this.explodeDelayMax = 0f;
		this.color = this._baseColor;
		this.velocity = new Vector3(3f, 3f, 3f);
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x00088DC0 File Offset: 0x00086FC0
	public override void Explode()
	{
		if (this.detailThreshold > this.detail)
		{
			return;
		}
		if (this.on)
		{
			this.UpdateSmokeA();
			this.UpdateSmokeB();
			if (this.drawSmokeA)
			{
				this._smokeAEmitter.Explode();
			}
			if (this.drawSmokeB)
			{
				this._smokeBEmitter.Explode();
			}
		}
	}

	// Token: 0x04001153 RID: 4435
	private const float _baseSize = 1f;

	// Token: 0x04001154 RID: 4436
	private const float _baseDuration = 8f;

	// Token: 0x04001155 RID: 4437
	private Color _baseColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

	// Token: 0x04001156 RID: 4438
	private const float _baseDamping = 0.1300004f;

	// Token: 0x04001157 RID: 4439
	private float _scaledDuration;

	// Token: 0x04001158 RID: 4440
	private GameObject _smokeA;

	// Token: 0x04001159 RID: 4441
	private DetonatorBurstEmitter _smokeAEmitter;

	// Token: 0x0400115A RID: 4442
	public Material smokeAMaterial;

	// Token: 0x0400115B RID: 4443
	private GameObject _smokeB;

	// Token: 0x0400115C RID: 4444
	private DetonatorBurstEmitter _smokeBEmitter;

	// Token: 0x0400115D RID: 4445
	public Material smokeBMaterial;

	// Token: 0x0400115E RID: 4446
	public bool drawSmokeA = true;

	// Token: 0x0400115F RID: 4447
	public bool drawSmokeB = true;
}
