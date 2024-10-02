using System;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class PerlinNoise2D
{
	// Token: 0x06000A14 RID: 2580 RVA: 0x000819A4 File Offset: 0x0007FBA4
	public PerlinNoise2D(float scale)
	{
		this.scale = scale;
		this.offset = new Vector2(Random.Range(-100f, 100f), Random.Range(-100f, 100f));
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x00081A04 File Offset: 0x0007FC04
	public PerlinNoise2D SetPersistence(float persistence)
	{
		this.persistence = persistence;
		return this;
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x00081A0E File Offset: 0x0007FC0E
	public PerlinNoise2D SetOctaves(int octaves)
	{
		this.octaves = octaves;
		return this;
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x00081A18 File Offset: 0x0007FC18
	public float Noise(float x, float y)
	{
		x = x * this.scale + this.offset.x;
		y = y * this.scale + this.offset.y;
		float num = 0f;
		float num2 = 1f;
		float num3 = 1f;
		for (int i = 0; i < this.octaves; i++)
		{
			if (i >= 1)
			{
				num2 *= 2f;
				num3 *= this.persistence;
			}
			num += Mathf.PerlinNoise(x * num2, y * num2) * num3;
		}
		return num;
	}

	// Token: 0x04000FC6 RID: 4038
	private float scale;

	// Token: 0x04000FC7 RID: 4039
	private Vector2 offset = Vector2.zero;

	// Token: 0x04000FC8 RID: 4040
	private float persistence = 0.5f;

	// Token: 0x04000FC9 RID: 4041
	private int octaves = 5;
}
