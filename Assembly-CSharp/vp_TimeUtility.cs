using System;
using UnityEngine;

// Token: 0x020000CF RID: 207
public static class vp_TimeUtility
{
	// Token: 0x17000028 RID: 40
	// (get) Token: 0x060006EC RID: 1772 RVA: 0x0006ECC7 File Offset: 0x0006CEC7
	// (set) Token: 0x060006ED RID: 1773 RVA: 0x0006ECCE File Offset: 0x0006CECE
	public static float TimeScale
	{
		get
		{
			return Time.timeScale;
		}
		set
		{
			value = vp_TimeUtility.ClampTimeScale(value);
			Time.timeScale = value;
			Time.fixedDeltaTime = vp_TimeUtility.InitialFixedTimeStep * Time.timeScale;
		}
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x0006ECF0 File Offset: 0x0006CEF0
	public static void FadeTimeScale(float targetTimeScale, float fadeSpeed)
	{
		if (vp_TimeUtility.TimeScale == targetTimeScale)
		{
			return;
		}
		targetTimeScale = vp_TimeUtility.ClampTimeScale(targetTimeScale);
		vp_TimeUtility.TimeScale = Mathf.Lerp(vp_TimeUtility.TimeScale, targetTimeScale, Time.deltaTime * 60f * fadeSpeed);
		if (Mathf.Abs(vp_TimeUtility.TimeScale - targetTimeScale) < 0.01f)
		{
			vp_TimeUtility.TimeScale = targetTimeScale;
		}
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x0006ED44 File Offset: 0x0006CF44
	private static float ClampTimeScale(float t)
	{
		if (t < vp_TimeUtility.m_MinTimeScale || t > vp_TimeUtility.m_MaxTimeScale)
		{
			t = Mathf.Clamp(t, vp_TimeUtility.m_MinTimeScale, vp_TimeUtility.m_MaxTimeScale);
			Debug.LogWarning(string.Concat(new object[]
			{
				"Warning: (vp_TimeUtility) TimeScale was clamped to within the supported range (",
				vp_TimeUtility.m_MinTimeScale,
				" - ",
				vp_TimeUtility.m_MaxTimeScale,
				")."
			}));
		}
		return t;
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0006EDB6 File Offset: 0x0006CFB6
	// (set) Token: 0x060006F1 RID: 1777 RVA: 0x0006EDC0 File Offset: 0x0006CFC0
	public static bool Paused
	{
		get
		{
			return vp_TimeUtility.m_Paused;
		}
		set
		{
			if (value)
			{
				if (vp_TimeUtility.m_Paused)
				{
					return;
				}
				vp_TimeUtility.m_Paused = true;
				vp_TimeUtility.m_TimeScaleOnPause = Time.timeScale;
				Time.timeScale = 0f;
				return;
			}
			else
			{
				if (!vp_TimeUtility.m_Paused)
				{
					return;
				}
				vp_TimeUtility.m_Paused = false;
				Time.timeScale = vp_TimeUtility.m_TimeScaleOnPause;
				vp_TimeUtility.m_TimeScaleOnPause = 1f;
				return;
			}
		}
	}

	// Token: 0x04000CCE RID: 3278
	private static float m_MinTimeScale = 0.1f;

	// Token: 0x04000CCF RID: 3279
	private static float m_MaxTimeScale = 1f;

	// Token: 0x04000CD0 RID: 3280
	private static bool m_Paused = false;

	// Token: 0x04000CD1 RID: 3281
	private static float m_TimeScaleOnPause = 1f;

	// Token: 0x04000CD2 RID: 3282
	public static float InitialFixedTimeStep = Time.fixedDeltaTime;
}
