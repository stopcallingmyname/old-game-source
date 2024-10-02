using System;
using UnityEngine;

// Token: 0x02000130 RID: 304
[AddComponentMenu("Detonator/Detonator")]
public class Detonator : MonoBehaviour
{
	// Token: 0x06000A87 RID: 2695 RVA: 0x00085A78 File Offset: 0x00083C78
	private void Awake()
	{
		this.FillDefaultMaterials();
		this.components = base.GetComponents(typeof(DetonatorComponent));
		foreach (DetonatorComponent detonatorComponent in this.components)
		{
			if (detonatorComponent is DetonatorFireball)
			{
				this._fireball = (detonatorComponent as DetonatorFireball);
			}
			if (detonatorComponent is DetonatorSparks)
			{
				this._sparks = (detonatorComponent as DetonatorSparks);
			}
			if (detonatorComponent is DetonatorShockwave)
			{
				this._shockwave = (detonatorComponent as DetonatorShockwave);
			}
			if (detonatorComponent is DetonatorSmoke)
			{
				this._smoke = (detonatorComponent as DetonatorSmoke);
			}
			if (detonatorComponent is DetonatorGlow)
			{
				this._glow = (detonatorComponent as DetonatorGlow);
			}
			if (detonatorComponent is DetonatorLight)
			{
				this._light = (detonatorComponent as DetonatorLight);
			}
			if (detonatorComponent is DetonatorForce)
			{
				this._force = (detonatorComponent as DetonatorForce);
			}
			if (detonatorComponent is DetonatorHeatwave)
			{
				this._heatwave = (detonatorComponent as DetonatorHeatwave);
			}
		}
		if (!this._fireball && this.autoCreateFireball)
		{
			this._fireball = base.gameObject.AddComponent<DetonatorFireball>();
			this._fireball.Reset();
		}
		if (!this._smoke && this.autoCreateSmoke)
		{
			this._smoke = base.gameObject.AddComponent<DetonatorSmoke>();
			this._smoke.Reset();
		}
		if (!this._sparks && this.autoCreateSparks)
		{
			this._sparks = base.gameObject.AddComponent<DetonatorSparks>();
			this._sparks.Reset();
		}
		if (!this._shockwave && this.autoCreateShockwave)
		{
			this._shockwave = base.gameObject.AddComponent<DetonatorShockwave>();
			this._shockwave.Reset();
		}
		if (!this._glow && this.autoCreateGlow)
		{
			this._glow = base.gameObject.AddComponent<DetonatorGlow>();
			this._glow.Reset();
		}
		if (!this._light && this.autoCreateLight)
		{
			this._light = base.gameObject.AddComponent<DetonatorLight>();
			this._light.Reset();
		}
		if (!this._force && this.autoCreateForce)
		{
			this._force = base.gameObject.AddComponent<DetonatorForce>();
			this._force.Reset();
		}
		if (!this._heatwave && this.autoCreateHeatwave && SystemInfo.supportsImageEffects)
		{
			this._heatwave = base.gameObject.AddComponent<DetonatorHeatwave>();
			this._heatwave.Reset();
		}
		this.components = base.GetComponents(typeof(DetonatorComponent));
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x00085D0C File Offset: 0x00083F0C
	private void FillDefaultMaterials()
	{
		if (!this.fireballAMaterial)
		{
			this.fireballAMaterial = Detonator.DefaultFireballAMaterial();
		}
		if (!this.fireballBMaterial)
		{
			this.fireballBMaterial = Detonator.DefaultFireballBMaterial();
		}
		if (!this.smokeAMaterial)
		{
			this.smokeAMaterial = Detonator.DefaultSmokeAMaterial();
		}
		if (!this.smokeBMaterial)
		{
			this.smokeBMaterial = Detonator.DefaultSmokeBMaterial();
		}
		if (!this.shockwaveMaterial)
		{
			this.shockwaveMaterial = Detonator.DefaultShockwaveMaterial();
		}
		if (!this.sparksMaterial)
		{
			this.sparksMaterial = Detonator.DefaultSparksMaterial();
		}
		if (!this.glowMaterial)
		{
			this.glowMaterial = Detonator.DefaultGlowMaterial();
		}
		if (!this.heatwaveMaterial)
		{
			this.heatwaveMaterial = Detonator.DefaultHeatwaveMaterial();
		}
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x00085DD9 File Offset: 0x00083FD9
	private void Start()
	{
		if (this.explodeOnStart)
		{
			this.UpdateComponents();
			this.Explode();
		}
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x00085DEF File Offset: 0x00083FEF
	private void Update()
	{
		if (this.destroyTime > 0f && this._lastExplosionTime + this.destroyTime <= Time.time)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x00085E20 File Offset: 0x00084020
	private void UpdateComponents()
	{
		if (this._firstComponentUpdate)
		{
			foreach (DetonatorComponent detonatorComponent in this.components)
			{
				detonatorComponent.Init();
				detonatorComponent.SetStartValues();
			}
			this._firstComponentUpdate = false;
		}
		if (!this._firstComponentUpdate)
		{
			float num = this.size / Detonator._baseSize;
			Vector3 vector = new Vector3(this.direction.x * num, this.direction.y * num, this.direction.z * num);
			float timeScale = this.duration / Detonator._baseDuration;
			foreach (DetonatorComponent detonatorComponent2 in this.components)
			{
				if (detonatorComponent2.detonatorControlled)
				{
					detonatorComponent2.size = detonatorComponent2.startSize * num;
					detonatorComponent2.timeScale = timeScale;
					detonatorComponent2.detail = detonatorComponent2.startDetail * this.detail;
					detonatorComponent2.force = new Vector3(detonatorComponent2.startForce.x * num + vector.x, detonatorComponent2.startForce.y * num + vector.y, detonatorComponent2.startForce.z * num + vector.z);
					detonatorComponent2.velocity = new Vector3(detonatorComponent2.startVelocity.x * num + vector.x, detonatorComponent2.startVelocity.y * num + vector.y, detonatorComponent2.startVelocity.z * num + vector.z);
					detonatorComponent2.color = Color.Lerp(detonatorComponent2.startColor, this.color, this.color.a);
				}
			}
		}
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x00085FD4 File Offset: 0x000841D4
	public void Explode()
	{
		this._lastExplosionTime = Time.time;
		foreach (DetonatorComponent detonatorComponent in this.components)
		{
			this.UpdateComponents();
			detonatorComponent.Explode();
		}
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x00086014 File Offset: 0x00084214
	public void Reset()
	{
		this.size = 10f;
		this.color = Detonator._baseColor;
		this.duration = Detonator._baseDuration;
		this.FillDefaultMaterials();
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x00086040 File Offset: 0x00084240
	public static Material DefaultFireballAMaterial()
	{
		if (Detonator.defaultFireballAMaterial != null)
		{
			return Detonator.defaultFireballAMaterial;
		}
		Detonator.defaultFireballAMaterial = new Material(Shader.Find("Particles/Additive"));
		Detonator.defaultFireballAMaterial.name = "FireballA-Default";
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Fireball") as Texture2D;
		Detonator.defaultFireballAMaterial.SetColor("_TintColor", Color.white);
		Detonator.defaultFireballAMaterial.mainTexture = mainTexture;
		Detonator.defaultFireballAMaterial.mainTextureScale = new Vector2(0.5f, 1f);
		return Detonator.defaultFireballAMaterial;
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x000860D0 File Offset: 0x000842D0
	public static Material DefaultFireballBMaterial()
	{
		if (Detonator.defaultFireballBMaterial != null)
		{
			return Detonator.defaultFireballBMaterial;
		}
		Detonator.defaultFireballBMaterial = new Material(Shader.Find("Particles/Additive"));
		Detonator.defaultFireballBMaterial.name = "FireballB-Default";
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Fireball") as Texture2D;
		Detonator.defaultFireballBMaterial.SetColor("_TintColor", Color.white);
		Detonator.defaultFireballBMaterial.mainTexture = mainTexture;
		Detonator.defaultFireballBMaterial.mainTextureScale = new Vector2(0.5f, 1f);
		Detonator.defaultFireballBMaterial.mainTextureOffset = new Vector2(0.5f, 0f);
		return Detonator.defaultFireballBMaterial;
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x0008617C File Offset: 0x0008437C
	public static Material DefaultSmokeAMaterial()
	{
		if (Detonator.defaultSmokeAMaterial != null)
		{
			return Detonator.defaultSmokeAMaterial;
		}
		Detonator.defaultSmokeAMaterial = new Material(Shader.Find("Particles/Alpha Blended"));
		Detonator.defaultSmokeAMaterial.name = "SmokeA-Default";
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Smoke") as Texture2D;
		Detonator.defaultSmokeAMaterial.SetColor("_TintColor", Color.white);
		Detonator.defaultSmokeAMaterial.mainTexture = mainTexture;
		Detonator.defaultSmokeAMaterial.mainTextureScale = new Vector2(0.5f, 1f);
		return Detonator.defaultSmokeAMaterial;
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x0008620C File Offset: 0x0008440C
	public static Material DefaultSmokeBMaterial()
	{
		if (Detonator.defaultSmokeBMaterial != null)
		{
			return Detonator.defaultSmokeBMaterial;
		}
		Detonator.defaultSmokeBMaterial = new Material(Shader.Find("Particles/Alpha Blended"));
		Detonator.defaultSmokeBMaterial.name = "SmokeB-Default";
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Smoke") as Texture2D;
		Detonator.defaultSmokeBMaterial.SetColor("_TintColor", Color.white);
		Detonator.defaultSmokeBMaterial.mainTexture = mainTexture;
		Detonator.defaultSmokeBMaterial.mainTextureScale = new Vector2(0.5f, 1f);
		Detonator.defaultSmokeBMaterial.mainTextureOffset = new Vector2(0.5f, 0f);
		return Detonator.defaultSmokeBMaterial;
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x000862B8 File Offset: 0x000844B8
	public static Material DefaultSparksMaterial()
	{
		if (Detonator.defaultSparksMaterial != null)
		{
			return Detonator.defaultSparksMaterial;
		}
		Detonator.defaultSparksMaterial = new Material(Shader.Find("Particles/Additive"));
		Detonator.defaultSparksMaterial.name = "Sparks-Default";
		Texture2D mainTexture = Resources.Load("Detonator/Textures/GlowDot") as Texture2D;
		Detonator.defaultSparksMaterial.SetColor("_TintColor", Color.white);
		Detonator.defaultSparksMaterial.mainTexture = mainTexture;
		return Detonator.defaultSparksMaterial;
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x00086330 File Offset: 0x00084530
	public static Material DefaultShockwaveMaterial()
	{
		if (Detonator.defaultShockwaveMaterial != null)
		{
			return Detonator.defaultShockwaveMaterial;
		}
		Detonator.defaultShockwaveMaterial = new Material(Shader.Find("Particles/Additive"));
		Detonator.defaultShockwaveMaterial.name = "Shockwave-Default";
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Shockwave") as Texture2D;
		Detonator.defaultShockwaveMaterial.SetColor("_TintColor", new Color(0.1f, 0.1f, 0.1f, 1f));
		Detonator.defaultShockwaveMaterial.mainTexture = mainTexture;
		return Detonator.defaultShockwaveMaterial;
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x000863BC File Offset: 0x000845BC
	public static Material DefaultGlowMaterial()
	{
		if (Detonator.defaultGlowMaterial != null)
		{
			return Detonator.defaultGlowMaterial;
		}
		Detonator.defaultGlowMaterial = new Material(Shader.Find("Particles/Additive"));
		Detonator.defaultGlowMaterial.name = "Glow-Default";
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Glow") as Texture2D;
		Detonator.defaultGlowMaterial.SetColor("_TintColor", Color.white);
		Detonator.defaultGlowMaterial.mainTexture = mainTexture;
		return Detonator.defaultGlowMaterial;
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x00086434 File Offset: 0x00084634
	public static Material DefaultHeatwaveMaterial()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			return null;
		}
		if (Detonator.defaultHeatwaveMaterial != null)
		{
			return Detonator.defaultHeatwaveMaterial;
		}
		Detonator.defaultHeatwaveMaterial = new Material(Shader.Find("HeatDistort"));
		Detonator.defaultHeatwaveMaterial.name = "Heatwave-Default";
		Texture2D value = Resources.Load("Detonator/Textures/Heatwave") as Texture2D;
		Detonator.defaultHeatwaveMaterial.SetTexture("_BumpMap", value);
		return Detonator.defaultHeatwaveMaterial;
	}

	// Token: 0x0400109D RID: 4253
	private static float _baseSize = 30f;

	// Token: 0x0400109E RID: 4254
	private static Color _baseColor = new Color(1f, 0.423f, 0f, 0.5f);

	// Token: 0x0400109F RID: 4255
	private static float _baseDuration = 3f;

	// Token: 0x040010A0 RID: 4256
	public float size = 10f;

	// Token: 0x040010A1 RID: 4257
	public Color color = Detonator._baseColor;

	// Token: 0x040010A2 RID: 4258
	public bool explodeOnStart = true;

	// Token: 0x040010A3 RID: 4259
	public float duration = Detonator._baseDuration;

	// Token: 0x040010A4 RID: 4260
	public float detail = 1f;

	// Token: 0x040010A5 RID: 4261
	public float upwardsBias;

	// Token: 0x040010A6 RID: 4262
	public float destroyTime = 7f;

	// Token: 0x040010A7 RID: 4263
	public bool useWorldSpace = true;

	// Token: 0x040010A8 RID: 4264
	public Vector3 direction = Vector3.zero;

	// Token: 0x040010A9 RID: 4265
	public Material fireballAMaterial;

	// Token: 0x040010AA RID: 4266
	public Material fireballBMaterial;

	// Token: 0x040010AB RID: 4267
	public Material smokeAMaterial;

	// Token: 0x040010AC RID: 4268
	public Material smokeBMaterial;

	// Token: 0x040010AD RID: 4269
	public Material shockwaveMaterial;

	// Token: 0x040010AE RID: 4270
	public Material sparksMaterial;

	// Token: 0x040010AF RID: 4271
	public Material glowMaterial;

	// Token: 0x040010B0 RID: 4272
	public Material heatwaveMaterial;

	// Token: 0x040010B1 RID: 4273
	private Component[] components;

	// Token: 0x040010B2 RID: 4274
	private DetonatorFireball _fireball;

	// Token: 0x040010B3 RID: 4275
	private DetonatorSparks _sparks;

	// Token: 0x040010B4 RID: 4276
	private DetonatorShockwave _shockwave;

	// Token: 0x040010B5 RID: 4277
	private DetonatorSmoke _smoke;

	// Token: 0x040010B6 RID: 4278
	private DetonatorGlow _glow;

	// Token: 0x040010B7 RID: 4279
	private DetonatorLight _light;

	// Token: 0x040010B8 RID: 4280
	private DetonatorForce _force;

	// Token: 0x040010B9 RID: 4281
	private DetonatorHeatwave _heatwave;

	// Token: 0x040010BA RID: 4282
	public bool autoCreateFireball = true;

	// Token: 0x040010BB RID: 4283
	public bool autoCreateSparks = true;

	// Token: 0x040010BC RID: 4284
	public bool autoCreateShockwave = true;

	// Token: 0x040010BD RID: 4285
	public bool autoCreateSmoke = true;

	// Token: 0x040010BE RID: 4286
	public bool autoCreateGlow = true;

	// Token: 0x040010BF RID: 4287
	public bool autoCreateLight = true;

	// Token: 0x040010C0 RID: 4288
	public bool autoCreateForce = true;

	// Token: 0x040010C1 RID: 4289
	public bool autoCreateHeatwave;

	// Token: 0x040010C2 RID: 4290
	private float _lastExplosionTime = 1000f;

	// Token: 0x040010C3 RID: 4291
	private bool _firstComponentUpdate = true;

	// Token: 0x040010C4 RID: 4292
	private Component[] _subDetonators;

	// Token: 0x040010C5 RID: 4293
	public static Material defaultFireballAMaterial;

	// Token: 0x040010C6 RID: 4294
	public static Material defaultFireballBMaterial;

	// Token: 0x040010C7 RID: 4295
	public static Material defaultSmokeAMaterial;

	// Token: 0x040010C8 RID: 4296
	public static Material defaultSmokeBMaterial;

	// Token: 0x040010C9 RID: 4297
	public static Material defaultShockwaveMaterial;

	// Token: 0x040010CA RID: 4298
	public static Material defaultSparksMaterial;

	// Token: 0x040010CB RID: 4299
	public static Material defaultGlowMaterial;

	// Token: 0x040010CC RID: 4300
	public static Material defaultHeatwaveMaterial;
}
