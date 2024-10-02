using System;
using UnityEngine;

// Token: 0x020000C9 RID: 201
public class vp_SmoothRandom
{
	// Token: 0x060006A8 RID: 1704 RVA: 0x0006D46C File Offset: 0x0006B66C
	public static Vector3 GetVector3(float speed)
	{
		float x = Time.time * 0.01f * speed;
		return new Vector3(vp_SmoothRandom.Get().HybridMultifractal(x, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(x, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(x, 0.2f, 0.58f));
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x0006D4CC File Offset: 0x0006B6CC
	public static Vector3 GetVector3Centered(float speed)
	{
		float x = Time.time * 0.01f * speed;
		float x2 = (Time.time - 1f) * 0.01f * speed;
		Vector3 a = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(x, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(x, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(x, 0.2f, 0.58f));
		Vector3 b = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(x2, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(x2, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(x2, 0.2f, 0.58f));
		return a - b;
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x0006D58C File Offset: 0x0006B78C
	public static float Get(float speed)
	{
		float num = Time.time * 0.01f * speed;
		return vp_SmoothRandom.Get().HybridMultifractal(num * 0.01f, 15.7f, 0.65f);
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x0006D5C2 File Offset: 0x0006B7C2
	private static vp_FractalNoise Get()
	{
		if (vp_SmoothRandom.s_Noise == null)
		{
			vp_SmoothRandom.s_Noise = new vp_FractalNoise(1.27f, 2.04f, 8.36f);
		}
		return vp_SmoothRandom.s_Noise;
	}

	// Token: 0x04000C9A RID: 3226
	private static vp_FractalNoise s_Noise;
}
