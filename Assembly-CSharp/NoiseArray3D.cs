using System;
using UnityEngine;

// Token: 0x0200011B RID: 283
internal class NoiseArray3D
{
	// Token: 0x06000A25 RID: 2597 RVA: 0x00081F6C File Offset: 0x0008016C
	public NoiseArray3D(float scale)
	{
		this.noise = new PerlinNoise3D(scale);
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x00081F91 File Offset: 0x00080191
	public void GenerateNoise(int offsetX, int offsetY, int offsetZ)
	{
		this.GenerateNoise(new Vector3i(offsetX, offsetY, offsetZ));
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x00081FA4 File Offset: 0x000801A4
	public void GenerateNoise(Vector3i offset)
	{
		this.offset = offset;
		int num = 10;
		int num2 = 10;
		int num3 = 10;
		for (int i = 0; i < num; i += 4)
		{
			for (int j = 0; j < num2; j += 4)
			{
				for (int k = 0; k < num3; k += 4)
				{
					Vector3i vector3i = new Vector3i(i, j, k) + offset;
					Vector3i vector3i2 = vector3i + new Vector3i(4, 4, 4);
					float a = this.noise.Noise((float)vector3i.x, (float)vector3i.y, (float)vector3i.z);
					float b = this.noise.Noise((float)vector3i2.x, (float)vector3i.y, (float)vector3i.z);
					float a2 = this.noise.Noise((float)vector3i.x, (float)vector3i2.y, (float)vector3i.z);
					float b2 = this.noise.Noise((float)vector3i2.x, (float)vector3i2.y, (float)vector3i.z);
					float a3 = this.noise.Noise((float)vector3i.x, (float)vector3i.y, (float)vector3i2.z);
					float b3 = this.noise.Noise((float)vector3i2.x, (float)vector3i.y, (float)vector3i2.z);
					float a4 = this.noise.Noise((float)vector3i.x, (float)vector3i2.y, (float)vector3i2.z);
					float b4 = this.noise.Noise((float)vector3i2.x, (float)vector3i2.y, (float)vector3i2.z);
					int num4 = 0;
					while (num4 < 4 && i + num4 < num)
					{
						int num5 = 0;
						while (num5 < 4 && j + num5 < num2)
						{
							int num6 = 0;
							while (num6 < 4 && k + num6 < num3)
							{
								float t = (float)num4 / 4f;
								float t2 = (float)num5 / 4f;
								float t3 = (float)num6 / 4f;
								float a5 = Mathf.Lerp(a, b, t);
								float b5 = Mathf.Lerp(a2, b2, t);
								float a6 = Mathf.Lerp(a5, b5, t2);
								float a7 = Mathf.Lerp(a3, b3, t);
								float b6 = Mathf.Lerp(a4, b4, t);
								float b7 = Mathf.Lerp(a7, b6, t2);
								float num7 = Mathf.Lerp(a6, b7, t3);
								this.map[i + num4, j + num5, k + num6] = num7;
								num6++;
							}
							num5++;
						}
						num4++;
					}
				}
			}
		}
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x0008222E File Offset: 0x0008042E
	public float GetNoise(Vector3i pos)
	{
		return this.GetNoise(pos.x, pos.y, pos.z);
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x00082248 File Offset: 0x00080448
	public float GetNoise(int x, int y, int z)
	{
		x -= this.offset.x;
		y -= this.offset.y;
		z -= this.offset.z;
		return this.map[x + 1, y + 1, z + 1];
	}

	// Token: 0x04000FD2 RID: 4050
	private const int step = 4;

	// Token: 0x04000FD3 RID: 4051
	private PerlinNoise3D noise;

	// Token: 0x04000FD4 RID: 4052
	private float[,,] map = new float[10, 10, 10];

	// Token: 0x04000FD5 RID: 4053
	private Vector3i offset;
}
