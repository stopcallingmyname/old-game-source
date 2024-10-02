using System;
using UnityEngine;

// Token: 0x020000E2 RID: 226
public class vp_Earthquake : MonoBehaviour
{
	// Token: 0x17000057 RID: 87
	// (get) Token: 0x06000829 RID: 2089 RVA: 0x00078948 File Offset: 0x00076B48
	private vp_FPPlayerEventHandler Player
	{
		get
		{
			if (this.m_Player == null && this.EventHandler != null)
			{
				this.m_Player = (vp_FPPlayerEventHandler)this.EventHandler;
			}
			return this.m_Player;
		}
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x0007897D File Offset: 0x00076B7D
	protected virtual void Awake()
	{
		this.EventHandler = (vp_EventHandler)Object.FindObjectOfType(typeof(vp_EventHandler));
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x00078999 File Offset: 0x00076B99
	protected virtual void OnEnable()
	{
		if (this.EventHandler != null)
		{
			this.EventHandler.Register(this);
		}
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x000789B5 File Offset: 0x00076BB5
	protected virtual void OnDisable()
	{
		if (this.EventHandler != null)
		{
			this.EventHandler.Unregister(this);
		}
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x000789D1 File Offset: 0x00076BD1
	protected void FixedUpdate()
	{
		if (Time.timeScale != 0f)
		{
			this.UpdateEarthQuake();
		}
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x000789E8 File Offset: 0x00076BE8
	protected void UpdateEarthQuake()
	{
		if (!this.Player.Earthquake.Active)
		{
			this.m_EarthQuakeForce = Vector3.zero;
			return;
		}
		this.m_EarthQuakeForce = Vector3.Scale(vp_SmoothRandom.GetVector3Centered(1f), this.m_Magnitude.x * (Vector3.right + Vector3.forward) * Mathf.Min(this.m_Endtime - Time.time, 1f) * Time.timeScale);
		this.m_EarthQuakeForce.y = 0f;
		if (Random.value < 0.3f * Time.timeScale)
		{
			this.m_EarthQuakeForce.y = Random.Range(0f, this.m_Magnitude.y * 0.35f) * Mathf.Min(this.m_Endtime - Time.time, 1f);
		}
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x00078ACC File Offset: 0x00076CCC
	protected virtual void OnStart_Earthquake()
	{
		Vector3 vector = (Vector3)this.Player.Earthquake.Argument;
		this.m_Magnitude.x = vector.x;
		this.m_Magnitude.y = vector.y;
		this.m_Endtime = Time.time + vector.z;
		this.Player.Earthquake.AutoDuration = vector.z;
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x00078B39 File Offset: 0x00076D39
	protected virtual void OnMessage_BombShake(float impact)
	{
		this.Player.Earthquake.TryStart<Vector3>(new Vector3(impact * 0.5f, impact * 0.5f, 1f));
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x06000831 RID: 2097 RVA: 0x00078B64 File Offset: 0x00076D64
	// (set) Token: 0x06000832 RID: 2098 RVA: 0x00078B6C File Offset: 0x00076D6C
	protected virtual Vector3 OnValue_EarthQuakeForce
	{
		get
		{
			return this.m_EarthQuakeForce;
		}
		set
		{
			this.m_EarthQuakeForce = value;
		}
	}

	// Token: 0x04000E72 RID: 3698
	protected Vector3 m_EarthQuakeForce;

	// Token: 0x04000E73 RID: 3699
	protected float m_Endtime;

	// Token: 0x04000E74 RID: 3700
	protected Vector2 m_Magnitude = Vector2.zero;

	// Token: 0x04000E75 RID: 3701
	protected vp_EventHandler EventHandler;

	// Token: 0x04000E76 RID: 3702
	private vp_FPPlayerEventHandler m_Player;
}
