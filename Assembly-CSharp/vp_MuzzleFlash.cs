using System;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public class vp_MuzzleFlash : MonoBehaviour
{
	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000850 RID: 2128 RVA: 0x0007A108 File Offset: 0x00078308
	// (set) Token: 0x06000851 RID: 2129 RVA: 0x0007A110 File Offset: 0x00078310
	public float FadeSpeed
	{
		get
		{
			return this.m_FadeSpeed;
		}
		set
		{
			this.m_FadeSpeed = value;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000852 RID: 2130 RVA: 0x0007A119 File Offset: 0x00078319
	// (set) Token: 0x06000853 RID: 2131 RVA: 0x0007A121 File Offset: 0x00078321
	public bool ForceShow
	{
		get
		{
			return this.m_ForceShow;
		}
		set
		{
			this.m_ForceShow = value;
		}
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x0007A12C File Offset: 0x0007832C
	private void Awake()
	{
		this.m_Transform = base.transform;
		if (this.flamethrower)
		{
			this.myPS = base.GetComponent<ParticleSystem>();
		}
		else
		{
			this.m_Color = base.GetComponent<Renderer>().material.GetColor("_TintColor");
			this.m_Color.a = 0f;
		}
		this.m_ForceShow = false;
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x0007A18D File Offset: 0x0007838D
	private void Start()
	{
		if (this.m_Transform.root.gameObject.layer == 30)
		{
			base.gameObject.layer = 31;
		}
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x0007A1B8 File Offset: 0x000783B8
	private void Update()
	{
		if (!this.flamethrower)
		{
			if (this.m_ForceShow)
			{
				this.Show();
			}
			else if (this.m_Color.a > 0f)
			{
				this.m_Color.a = this.m_Color.a - this.m_FadeSpeed * (Time.deltaTime * 60f);
			}
			base.GetComponent<Renderer>().material.SetColor("_TintColor", this.m_Color);
			return;
		}
		if (this.myPS == null)
		{
			return;
		}
		if (!this.myPS.isPlaying)
		{
			return;
		}
		if (this.flameTimer > Time.time)
		{
			return;
		}
		this.myPS.Stop(true);
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x0007A263 File Offset: 0x00078463
	public void Show()
	{
		this.m_Color.a = 0.5f;
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x0007A278 File Offset: 0x00078478
	public void Shoot()
	{
		if (!this.flamethrower)
		{
			this.m_Transform.Rotate(0f, 0f, (float)Random.Range(0, 360));
			this.m_Color.a = 0.5f;
			return;
		}
		if (this.myPS == null)
		{
			return;
		}
		if (!this.myPS.isPlaying)
		{
			this.myPS.Play(true);
		}
		this.flameTimer = Time.time + 0.1f;
	}

	// Token: 0x04000EBA RID: 3770
	protected float m_FadeSpeed = 0.075f;

	// Token: 0x04000EBB RID: 3771
	protected bool m_ForceShow;

	// Token: 0x04000EBC RID: 3772
	protected Color m_Color = new Color(1f, 1f, 1f, 0f);

	// Token: 0x04000EBD RID: 3773
	protected Transform m_Transform;

	// Token: 0x04000EBE RID: 3774
	public bool flamethrower;

	// Token: 0x04000EBF RID: 3775
	private ParticleSystem myPS;

	// Token: 0x04000EC0 RID: 3776
	private float flameTimer;
}
