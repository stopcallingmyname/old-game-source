using System;
using UnityEngine;

// Token: 0x02000131 RID: 305
public class DetonatorBurstEmitter : DetonatorComponent
{
	// Token: 0x06000A98 RID: 2712 RVA: 0x00086582 File Offset: 0x00084782
	public override void Init()
	{
		MonoBehaviour.print("UNUSED");
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x0008658E File Offset: 0x0008478E
	public void Awake()
	{
		if (this.explodeOnAwake)
		{
			this.Explode();
		}
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x000865A0 File Offset: 0x000847A0
	private void Update()
	{
		if (this.exponentialGrowth)
		{
			float num = Time.time - this._emitTime;
			float num2 = this.SizeFunction(num - DetonatorBurstEmitter.epsilon);
			float num3 = (this.SizeFunction(num) / num2 - 1f) / DetonatorBurstEmitter.epsilon;
		}
		if (this._delayedExplosionStarted)
		{
			this._explodeDelay -= Time.deltaTime;
			if (this._explodeDelay <= 0f)
			{
				this.Explode();
			}
		}
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x00086614 File Offset: 0x00084814
	private float SizeFunction(float elapsedTime)
	{
		float num = 1f - 1f / (1f + elapsedTime * this.speed);
		return this.initFraction + (1f - this.initFraction) * num;
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x00086651 File Offset: 0x00084851
	public void Reset()
	{
		this.size = this._baseSize;
		this.color = this._baseColor;
		this.damping = this._baseDamping;
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00086678 File Offset: 0x00084878
	public override void Explode()
	{
		if (this.on)
		{
			this._scaledDuration = this.timeScale * this.duration;
			this._scaledDurationVariation = this.timeScale * this.durationVariation;
			this._scaledStartRadius = this.size * this.startRadius;
			if (!this._delayedExplosionStarted)
			{
				this._explodeDelay = this.explodeDelayMin + Random.value * (this.explodeDelayMax - this.explodeDelayMin);
			}
			if (this._explodeDelay <= 0f)
			{
				Color[] array = new Color[5];
				if (this.useExplicitColorAnimation)
				{
					array[0] = this.colorAnimation[0];
					array[1] = this.colorAnimation[1];
					array[2] = this.colorAnimation[2];
					array[3] = this.colorAnimation[3];
					array[4] = this.colorAnimation[4];
				}
				else
				{
					array[0] = new Color(this.color.r, this.color.g, this.color.b, this.color.a * 0.7f);
					array[1] = new Color(this.color.r, this.color.g, this.color.b, this.color.a * 1f);
					array[2] = new Color(this.color.r, this.color.g, this.color.b, this.color.a * 0.5f);
					array[3] = new Color(this.color.r, this.color.g, this.color.b, this.color.a * 0.3f);
					array[4] = new Color(this.color.r, this.color.g, this.color.b, this.color.a * 0f);
				}
				this._tmpCount = this.count * this.detail;
				if (this._tmpCount < 1f)
				{
					this._tmpCount = 1f;
				}
				int num = 1;
				while ((float)num <= this._tmpCount)
				{
					this._tmpPos = Vector3.Scale(Random.insideUnitSphere, new Vector3(this._scaledStartRadius, this._scaledStartRadius, this._scaledStartRadius));
					this._tmpPos = this._thisPos + this._tmpPos;
					this._tmpDir = Vector3.Scale(Random.insideUnitSphere, new Vector3(this.velocity.x, this.velocity.y, this.velocity.z));
					this._tmpDir.y = this._tmpDir.y + 2f * (Mathf.Abs(this._tmpDir.y) * this.upwardsBias);
					if (this.randomRotation)
					{
						this._randomizedRotation = Random.Range(-1f, 1f);
						this._tmpAngularVelocity = Random.Range(-1f, 1f) * this.angularVelocity;
					}
					else
					{
						this._randomizedRotation = 0f;
						this._tmpAngularVelocity = this.angularVelocity;
					}
					this._tmpDir = Vector3.Scale(this._tmpDir, new Vector3(this.size, this.size, this.size));
					this._tmpParticleSize = this.size * (this.particleSize + Random.value * this.sizeVariation);
					this._tmpDuration = this._scaledDuration + Random.value * this._scaledDurationVariation;
					num++;
				}
				this._emitTime = Time.time;
				this._delayedExplosionStarted = false;
				this._explodeDelay = 0f;
				return;
			}
			this._delayedExplosionStarted = true;
		}
	}

	// Token: 0x040010CD RID: 4301
	private float _baseDamping = 0.1300004f;

	// Token: 0x040010CE RID: 4302
	private float _baseSize = 1f;

	// Token: 0x040010CF RID: 4303
	private Color _baseColor = Color.white;

	// Token: 0x040010D0 RID: 4304
	public float damping = 1f;

	// Token: 0x040010D1 RID: 4305
	public float startRadius = 1f;

	// Token: 0x040010D2 RID: 4306
	public float maxScreenSize = 2f;

	// Token: 0x040010D3 RID: 4307
	public bool explodeOnAwake;

	// Token: 0x040010D4 RID: 4308
	public bool oneShot = true;

	// Token: 0x040010D5 RID: 4309
	public float sizeVariation;

	// Token: 0x040010D6 RID: 4310
	public float particleSize = 1f;

	// Token: 0x040010D7 RID: 4311
	public float count = 1f;

	// Token: 0x040010D8 RID: 4312
	public float sizeGrow = 20f;

	// Token: 0x040010D9 RID: 4313
	public bool exponentialGrowth = true;

	// Token: 0x040010DA RID: 4314
	public float durationVariation;

	// Token: 0x040010DB RID: 4315
	public bool useWorldSpace = true;

	// Token: 0x040010DC RID: 4316
	public float upwardsBias;

	// Token: 0x040010DD RID: 4317
	public float angularVelocity = 20f;

	// Token: 0x040010DE RID: 4318
	public bool randomRotation = true;

	// Token: 0x040010DF RID: 4319
	public bool useExplicitColorAnimation;

	// Token: 0x040010E0 RID: 4320
	public Color[] colorAnimation = new Color[5];

	// Token: 0x040010E1 RID: 4321
	private bool _delayedExplosionStarted;

	// Token: 0x040010E2 RID: 4322
	private float _explodeDelay;

	// Token: 0x040010E3 RID: 4323
	public Material material;

	// Token: 0x040010E4 RID: 4324
	private float _emitTime;

	// Token: 0x040010E5 RID: 4325
	private float speed = 3f;

	// Token: 0x040010E6 RID: 4326
	private float initFraction = 0.1f;

	// Token: 0x040010E7 RID: 4327
	private static float epsilon = 0.01f;

	// Token: 0x040010E8 RID: 4328
	private float _tmpParticleSize;

	// Token: 0x040010E9 RID: 4329
	private Vector3 _tmpPos;

	// Token: 0x040010EA RID: 4330
	private Vector3 _tmpDir;

	// Token: 0x040010EB RID: 4331
	private Vector3 _thisPos;

	// Token: 0x040010EC RID: 4332
	private float _tmpDuration;

	// Token: 0x040010ED RID: 4333
	private float _tmpCount;

	// Token: 0x040010EE RID: 4334
	private float _scaledDuration;

	// Token: 0x040010EF RID: 4335
	private float _scaledDurationVariation;

	// Token: 0x040010F0 RID: 4336
	private float _scaledStartRadius;

	// Token: 0x040010F1 RID: 4337
	private float _scaledColor;

	// Token: 0x040010F2 RID: 4338
	private float _randomizedRotation;

	// Token: 0x040010F3 RID: 4339
	private float _tmpAngularVelocity;
}
