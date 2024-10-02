using System;
using UnityEngine;

// Token: 0x0200012B RID: 299
public class MapRayIntersection
{
	// Token: 0x06000A6C RID: 2668 RVA: 0x00085400 File Offset: 0x00083600
	public static Vector3? Intersection(Map map, Ray ray, float distance)
	{
		Vector3 origin = ray.origin;
		Vector3 vector = ray.origin + ray.direction * distance;
		int num = Mathf.RoundToInt(origin.x);
		int num2 = Mathf.RoundToInt(origin.y);
		int num3 = Mathf.RoundToInt(origin.z);
		int num4 = Mathf.RoundToInt(vector.x);
		int num5 = Mathf.RoundToInt(vector.y);
		int num6 = Mathf.RoundToInt(vector.z);
		if (num > num4)
		{
			int num7 = num;
			num = num4;
			num4 = num7;
		}
		if (num2 > num5)
		{
			int num8 = num2;
			num2 = num5;
			num5 = num8;
		}
		if (num3 > num6)
		{
			int num9 = num3;
			num3 = num6;
			num6 = num9;
		}
		float num10 = distance;
		for (int i = num3; i <= num6; i++)
		{
			for (int j = num2; j <= num5; j++)
			{
				for (int k = num; k <= num4; k++)
				{
					BlockData block = map.GetBlock(k, j, i);
					if (!block.IsEmpty() && !block.IsFluid())
					{
						float b = MapRayIntersection.BlockRayIntersection(new Vector3((float)k, (float)j, (float)i), ray);
						num10 = Mathf.Min(num10, b);
					}
				}
			}
		}
		if (num10 != distance)
		{
			return new Vector3?(ray.origin + ray.direction * num10);
		}
		return null;
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x00085540 File Offset: 0x00083740
	private static float BlockRayIntersection(Vector3 blockPos, Ray ray)
	{
		float num = float.MinValue;
		float num2 = float.MaxValue;
		for (int i = 0; i < 3; i++)
		{
			float num3 = blockPos[i] - 0.5f;
			float num4 = blockPos[i] + 0.5f;
			float num5 = ray.origin[i];
			float num6 = ray.direction[i];
			if (Mathf.Abs(num6) <= 1E-45f && (num5 < num3 || num5 > num4))
			{
				return float.MaxValue;
			}
			float num7 = (num3 - num5) / num6;
			float num8 = (num4 - num5) / num6;
			if (num7 > num8)
			{
				float num9 = num7;
				num7 = num8;
				num8 = num9;
			}
			num = Mathf.Max(num7, num);
			num2 = Mathf.Min(num8, num2);
			if (num > num2)
			{
				return float.MaxValue;
			}
			if (num2 < 0f)
			{
				return float.MaxValue;
			}
		}
		if (num > 0f)
		{
			return num;
		}
		return num2;
	}
}
