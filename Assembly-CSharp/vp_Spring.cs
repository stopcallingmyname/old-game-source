using System;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class vp_Spring
{
	// Token: 0x17000026 RID: 38
	// (set) Token: 0x060006BD RID: 1725 RVA: 0x0006E003 File Offset: 0x0006C203
	public Transform Transform
	{
		set
		{
			this.m_Transform = value;
			this.RefreshUpdateMode();
		}
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x0006E014 File Offset: 0x0006C214
	public vp_Spring(Transform transform, vp_Spring.UpdateMode mode, bool autoUpdate = true)
	{
		this.Mode = mode;
		this.Transform = transform;
		this.m_AutoUpdate = autoUpdate;
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x0006E0FC File Offset: 0x0006C2FC
	public void FixedUpdate()
	{
		if (this.m_VelocityFadeInEndTime > Time.time)
		{
			this.m_VelocityFadeInCap = Mathf.Clamp01(1f - (this.m_VelocityFadeInEndTime - Time.time) / this.m_VelocityFadeInLength);
		}
		else
		{
			this.m_VelocityFadeInCap = 1f;
		}
		if (this.m_SoftForceFrame[0] != Vector3.zero)
		{
			this.AddForceInternal(this.m_SoftForceFrame[0]);
			for (int i = 0; i < 120; i++)
			{
				this.m_SoftForceFrame[i] = ((i < 119) ? this.m_SoftForceFrame[i + 1] : Vector3.zero);
				if (this.m_SoftForceFrame[i] == Vector3.zero)
				{
					break;
				}
			}
		}
		this.Calculate();
		this.m_UpdateFunc();
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x0006E1CD File Offset: 0x0006C3CD
	private void Position()
	{
		this.m_Transform.localPosition = this.State;
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x0006E1E0 File Offset: 0x0006C3E0
	private void Rotation()
	{
		this.m_Transform.localEulerAngles = this.State;
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x0006E1F3 File Offset: 0x0006C3F3
	private void Scale()
	{
		this.m_Transform.localScale = this.State;
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x0006E206 File Offset: 0x0006C406
	private void PositionAdditive()
	{
		this.m_Transform.localPosition += this.State;
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x0006E224 File Offset: 0x0006C424
	private void RotationAdditive()
	{
		this.m_Transform.localEulerAngles += this.State;
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x0006E242 File Offset: 0x0006C442
	private void ScaleAdditive()
	{
		this.m_Transform.localScale += this.State;
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x0000248C File Offset: 0x0000068C
	private void None()
	{
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x0006E260 File Offset: 0x0006C460
	protected void RefreshUpdateMode()
	{
		this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.None);
		switch (this.Mode)
		{
		case vp_Spring.UpdateMode.Position:
			this.State = this.m_Transform.localPosition;
			if (this.m_AutoUpdate)
			{
				this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.Position);
			}
			break;
		case vp_Spring.UpdateMode.PositionAdditive:
			this.State = this.m_Transform.localPosition;
			if (this.m_AutoUpdate)
			{
				this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.PositionAdditive);
			}
			break;
		case vp_Spring.UpdateMode.Rotation:
			this.State = this.m_Transform.localEulerAngles;
			if (this.m_AutoUpdate)
			{
				this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.Rotation);
			}
			break;
		case vp_Spring.UpdateMode.RotationAdditive:
			this.State = this.m_Transform.localEulerAngles;
			if (this.m_AutoUpdate)
			{
				this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.RotationAdditive);
			}
			break;
		case vp_Spring.UpdateMode.Scale:
			this.State = this.m_Transform.localScale;
			if (this.m_AutoUpdate)
			{
				this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.Scale);
			}
			break;
		case vp_Spring.UpdateMode.ScaleAdditive:
			this.State = this.m_Transform.localScale;
			if (this.m_AutoUpdate)
			{
				this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.ScaleAdditive);
			}
			break;
		}
		this.RestState = this.State;
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x0006E3D4 File Offset: 0x0006C5D4
	protected void Calculate()
	{
		if (this.State == this.RestState)
		{
			return;
		}
		this.m_Velocity += Vector3.Scale(this.RestState - this.State, this.Stiffness);
		this.m_Velocity = Vector3.Scale(this.m_Velocity, this.Damping);
		this.m_Velocity = Vector3.ClampMagnitude(this.m_Velocity, this.MaxVelocity);
		if (this.m_Velocity.sqrMagnitude > this.MinVelocity * this.MinVelocity)
		{
			this.Move();
			return;
		}
		this.Reset();
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x0006E477 File Offset: 0x0006C677
	private void AddForceInternal(Vector3 force)
	{
		force *= this.m_VelocityFadeInCap;
		this.m_Velocity += force;
		this.m_Velocity = Vector3.ClampMagnitude(this.m_Velocity, this.MaxVelocity);
		this.Move();
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x0006E4B6 File Offset: 0x0006C6B6
	public void AddForce(Vector3 force)
	{
		if (Time.timeScale < 1f)
		{
			this.AddSoftForce(force, 1f);
			return;
		}
		this.AddForceInternal(force);
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x0006E4D8 File Offset: 0x0006C6D8
	public void AddSoftForce(Vector3 force, float frames)
	{
		force /= Time.timeScale;
		frames = Mathf.Clamp(frames, 1f, 120f);
		this.AddForceInternal(force / frames);
		for (int i = 0; i < Mathf.RoundToInt(frames) - 1; i++)
		{
			this.m_SoftForceFrame[i] += force / frames;
		}
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x0006E548 File Offset: 0x0006C748
	protected void Move()
	{
		this.State += this.m_Velocity * Time.timeScale;
		this.State.x = Mathf.Clamp(this.State.x, this.MinState.x, this.MaxState.x);
		this.State.y = Mathf.Clamp(this.State.y, this.MinState.y, this.MaxState.y);
		this.State.z = Mathf.Clamp(this.State.z, this.MinState.z, this.MaxState.z);
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x0006E609 File Offset: 0x0006C809
	public void Reset()
	{
		this.m_Velocity = Vector3.zero;
		this.State = this.RestState;
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x0006E622 File Offset: 0x0006C822
	public void Stop(bool includeSoftForce = false)
	{
		this.m_Velocity = Vector3.zero;
		if (includeSoftForce)
		{
			this.StopSoftForce();
		}
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x0006E638 File Offset: 0x0006C838
	public void StopSoftForce()
	{
		for (int i = 0; i < 120; i++)
		{
			this.m_SoftForceFrame[i] = Vector3.zero;
		}
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x0006E663 File Offset: 0x0006C863
	public void ForceVelocityFadeIn(float seconds)
	{
		this.m_VelocityFadeInLength = seconds;
		this.m_VelocityFadeInEndTime = Time.time + seconds;
		this.m_VelocityFadeInCap = 0f;
	}

	// Token: 0x04000CA7 RID: 3239
	protected vp_Spring.UpdateMode Mode;

	// Token: 0x04000CA8 RID: 3240
	protected bool m_AutoUpdate = true;

	// Token: 0x04000CA9 RID: 3241
	protected vp_Spring.UpdateDelegate m_UpdateFunc;

	// Token: 0x04000CAA RID: 3242
	public Vector3 State = Vector3.zero;

	// Token: 0x04000CAB RID: 3243
	protected Vector3 m_Velocity = Vector3.zero;

	// Token: 0x04000CAC RID: 3244
	public Vector3 RestState = Vector3.zero;

	// Token: 0x04000CAD RID: 3245
	public Vector3 Stiffness = new Vector3(0.5f, 0.5f, 0.5f);

	// Token: 0x04000CAE RID: 3246
	public Vector3 Damping = new Vector3(0.75f, 0.75f, 0.75f);

	// Token: 0x04000CAF RID: 3247
	protected float m_VelocityFadeInCap = 1f;

	// Token: 0x04000CB0 RID: 3248
	protected float m_VelocityFadeInEndTime;

	// Token: 0x04000CB1 RID: 3249
	protected float m_VelocityFadeInLength;

	// Token: 0x04000CB2 RID: 3250
	protected Vector3[] m_SoftForceFrame = new Vector3[120];

	// Token: 0x04000CB3 RID: 3251
	public float MaxVelocity = 10000f;

	// Token: 0x04000CB4 RID: 3252
	public float MinVelocity = 1E-07f;

	// Token: 0x04000CB5 RID: 3253
	public Vector3 MaxState = new Vector3(10000f, 10000f, 10000f);

	// Token: 0x04000CB6 RID: 3254
	public Vector3 MinState = new Vector3(-10000f, -10000f, -10000f);

	// Token: 0x04000CB7 RID: 3255
	protected Transform m_Transform;

	// Token: 0x020008A8 RID: 2216
	public enum UpdateMode
	{
		// Token: 0x04003397 RID: 13207
		Position,
		// Token: 0x04003398 RID: 13208
		PositionAdditive,
		// Token: 0x04003399 RID: 13209
		Rotation,
		// Token: 0x0400339A RID: 13210
		RotationAdditive,
		// Token: 0x0400339B RID: 13211
		Scale,
		// Token: 0x0400339C RID: 13212
		ScaleAdditive
	}

	// Token: 0x020008A9 RID: 2217
	// (Invoke) Token: 0x06004CE8 RID: 19688
	protected delegate void UpdateDelegate();
}
