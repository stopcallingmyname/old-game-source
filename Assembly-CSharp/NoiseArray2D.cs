using System;
using UnityEngine;

// Token: 0x02000119 RID: 281
public class NoiseArray2D
{
	// Token: 0x06000A18 RID: 2584 RVA: 0x00081A9A File Offset: 0x0007FC9A
	public NoiseArray2D(float scale)
	{
		this.noise = new PerlinNoise2D(scale);
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x00081ABD File Offset: 0x0007FCBD
	public NoiseArray2D SetPersistence(float persistence)
	{
		this.noise.SetPersistence(persistence);
		return this;
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x00081ACD File Offset: 0x0007FCCD
	public NoiseArray2D SetOctaves(int octaves)
	{
		this.noise.SetOctaves(octaves);
		return this;
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x00081ADD File Offset: 0x0007FCDD
	public void GenerateNoise(int offsetX, int offsetY)
	{
		this.GenerateNoise(new Vector2i(offsetX, offsetY));
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x00081AEC File Offset: 0x0007FCEC
	public void GenerateNoise(Vector2i offset)
	{
		this.offset = offset;
		int length = this.map.GetLength(0);
		int length2 = this.map.GetLength(1);
		for (int i = 0; i < length; i += 4)
		{
			for (int j = 0; j < length2; j += 4)
			{
				Vector2i vector2i = new Vector2i(i, j) + offset;
				Vector2i vector2i2 = vector2i + new Vector2i(4, 4);
				float a = this.noise.Noise((float)vector2i.x, (float)vector2i.y);
				float b = this.noise.Noise((float)vector2i2.x, (float)vector2i.y);
				float a2 = this.noise.Noise((float)vector2i.x, (float)vector2i2.y);
				float b2 = this.noise.Noise((float)vector2i2.x, (float)vector2i2.y);
				int num = 0;
				while (num < 4 && i + num < length)
				{
					int num2 = 0;
					while (num2 < 4 && j + num2 < length2)
					{
						float t = (float)num / 4f;
						float t2 = (float)num2 / 4f;
						float a3 = Mathf.Lerp(a, b, t);
						float b3 = Mathf.Lerp(a2, b2, t);
						this.map[i + num, j + num2] = Mathf.Lerp(a3, b3, t2);
						num2++;
					}
					num++;
				}
			}
		}
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x00081C4D File Offset: 0x0007FE4D
	public float GetNoise(int x, int y)
	{
		x -= this.offset.x;
		y -= this.offset.y;
		return this.map[x + 1, y + 1];
	}

	// Token: 0x04000FCA RID: 4042
	private const int step = 4;

	// Token: 0x04000FCB RID: 4043
	private PerlinNoise2D noise;

	// Token: 0x04000FCC RID: 4044
	private float[,] map = new float[10, 10];

	// Token: 0x04000FCD RID: 4045
	private Vector2i offset;
}
