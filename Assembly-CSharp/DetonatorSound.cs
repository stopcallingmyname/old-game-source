using System;
using UnityEngine;

// Token: 0x0200013B RID: 315
[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Sound")]
public class DetonatorSound : DetonatorComponent
{
	// Token: 0x06000ADE RID: 2782 RVA: 0x00088E4B File Offset: 0x0008704B
	public override void Init()
	{
		this._soundComponent = base.gameObject.AddComponent<AudioSource>();
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x00088E60 File Offset: 0x00087060
	private void Update()
	{
		if (this._soundComponent == null)
		{
			return;
		}
		this._soundComponent.pitch = Time.timeScale;
		if (this._delayedExplosionStarted)
		{
			this._explodeDelay -= Time.deltaTime;
			if (this._explodeDelay <= 0f)
			{
				this.Explode();
			}
		}
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x00088EBC File Offset: 0x000870BC
	public override void Explode()
	{
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
			if (Vector3.Distance(Camera.main.transform.position, base.transform.position) < this.distanceThreshold)
			{
				this._idx = (int)(Random.value * (float)this.nearSounds.Length);
				this._soundComponent.PlayOneShot(this.nearSounds[this._idx]);
			}
			else
			{
				this._idx = (int)(Random.value * (float)this.farSounds.Length);
				this._soundComponent.PlayOneShot(this.farSounds[this._idx]);
			}
			this._delayedExplosionStarted = false;
			this._explodeDelay = 0f;
			return;
		}
		this._delayedExplosionStarted = true;
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x0000248C File Offset: 0x0000068C
	public void Reset()
	{
	}

	// Token: 0x04001160 RID: 4448
	public AudioClip[] nearSounds;

	// Token: 0x04001161 RID: 4449
	public AudioClip[] farSounds;

	// Token: 0x04001162 RID: 4450
	public float distanceThreshold = 50f;

	// Token: 0x04001163 RID: 4451
	public float minVolume = 0.4f;

	// Token: 0x04001164 RID: 4452
	public float maxVolume = 1f;

	// Token: 0x04001165 RID: 4453
	public float rolloffFactor = 0.5f;

	// Token: 0x04001166 RID: 4454
	private AudioSource _soundComponent;

	// Token: 0x04001167 RID: 4455
	private bool _delayedExplosionStarted;

	// Token: 0x04001168 RID: 4456
	private float _explodeDelay;

	// Token: 0x04001169 RID: 4457
	private int _idx;
}
