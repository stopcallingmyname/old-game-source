using System;
using UnityEngine;

// Token: 0x02000138 RID: 312
[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Light")]
public class DetonatorLight : DetonatorComponent
{
	// Token: 0x06000AC9 RID: 2761 RVA: 0x000882A8 File Offset: 0x000864A8
	public override void Init()
	{
		this._light = new GameObject("Light");
		this._light.transform.parent = base.transform;
		this._light.transform.localPosition = this.localPosition;
		this._lightComponent = this._light.AddComponent<Light>();
		this._lightComponent.type = LightType.Point;
		this._lightComponent.enabled = false;
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x0008831C File Offset: 0x0008651C
	private void Update()
	{
		if (this._explodeTime + this._scaledDuration > Time.time && this._lightComponent.intensity > 0f)
		{
			this._reduceAmount = this.intensity * (Time.deltaTime / this._scaledDuration);
			this._lightComponent.intensity -= this._reduceAmount;
			return;
		}
		if (this._lightComponent)
		{
			this._lightComponent.enabled = false;
		}
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x0008839C File Offset: 0x0008659C
	public override void Explode()
	{
		if (this.detailThreshold > this.detail)
		{
			return;
		}
		this._lightComponent.color = this.color;
		this._lightComponent.range = this.size * 50f;
		this._scaledDuration = this.duration * this.timeScale;
		this._lightComponent.enabled = true;
		this._lightComponent.intensity = this.intensity;
		this._explodeTime = Time.time;
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x0008841B File Offset: 0x0008661B
	public void Reset()
	{
		this.color = this._baseColor;
		this.intensity = this._baseIntensity;
	}

	// Token: 0x04001144 RID: 4420
	private float _baseIntensity = 1f;

	// Token: 0x04001145 RID: 4421
	private Color _baseColor = Color.white;

	// Token: 0x04001146 RID: 4422
	private float _scaledDuration;

	// Token: 0x04001147 RID: 4423
	private float _explodeTime = -1000f;

	// Token: 0x04001148 RID: 4424
	private GameObject _light;

	// Token: 0x04001149 RID: 4425
	private Light _lightComponent;

	// Token: 0x0400114A RID: 4426
	public float intensity;

	// Token: 0x0400114B RID: 4427
	private float _reduceAmount;
}
