using System;
using System.Collections.Generic;

// Token: 0x02000108 RID: 264
public class SunLightComputer
{
	// Token: 0x06000982 RID: 2434 RVA: 0x0007F9B8 File Offset: 0x0007DBB8
	public static void ComputeRayAtPosition(Map map, int x, int z)
	{
		int maxY = map.GetMaxY(x, z);
		map.GetSunLightmap().SetSunHeight(maxY + 1, x, z);
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x0007F9E0 File Offset: 0x0007DBE0
	private static void Scatter(Map map, List<Vector3i> list)
	{
		SunLightMap sunLightmap = map.GetSunLightmap();
		for (int i = 0; i < list.Count; i++)
		{
			Vector3i vector3i = list[i];
			if (vector3i.y >= 0)
			{
				BlockData block = map.GetBlock(vector3i);
				int num = (int)sunLightmap.GetLight(vector3i) - LightComputerUtils.GetLightStep(block);
				if (num > 5)
				{
					foreach (Vector3i b in Vector3i.directions)
					{
						Vector3i vector3i2 = vector3i + b;
						block = map.GetBlock(vector3i2);
						if (block.IsAlpha() && sunLightmap.SetMaxLight((byte)num, vector3i2))
						{
							list.Add(vector3i2);
						}
						if (!block.IsEmpty())
						{
							LightComputerUtils.SetLightDirty(map, vector3i2);
						}
					}
				}
			}
		}
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x0007FAA4 File Offset: 0x0007DCA4
	public static void RecomputeLightAtPosition(Map map, Vector3i pos)
	{
		SunLightMap sunLightmap = map.GetSunLightmap();
		int sunHeight = sunLightmap.GetSunHeight(pos.x, pos.z);
		SunLightComputer.ComputeRayAtPosition(map, pos.x, pos.z);
		int sunHeight2 = sunLightmap.GetSunHeight(pos.x, pos.z);
		if (sunHeight2 < sunHeight)
		{
			SunLightComputer.list.Clear();
			for (int i = sunHeight2; i <= sunHeight; i++)
			{
				pos.y = i;
				sunLightmap.SetLight(5, pos);
				SunLightComputer.list.Add(pos);
			}
			SunLightComputer.Scatter(map, SunLightComputer.list);
		}
		if (sunHeight2 > sunHeight)
		{
			SunLightComputer.list.Clear();
			for (int j = sunHeight; j <= sunHeight2; j++)
			{
				pos.y = j;
				SunLightComputer.list.Add(pos);
			}
			SunLightComputer.RemoveLight(map, SunLightComputer.list);
		}
		if (sunHeight2 == sunHeight)
		{
			if (map.GetBlock(pos).IsAlpha())
			{
				SunLightComputer.UpdateLight(map, pos);
				return;
			}
			SunLightComputer.RemoveLight(map, pos);
		}
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x0007FB94 File Offset: 0x0007DD94
	private static void UpdateLight(Map map, Vector3i pos)
	{
		SunLightComputer.list.Clear();
		foreach (Vector3i b in Vector3i.directions)
		{
			SunLightComputer.list.Add(pos + b);
		}
		SunLightComputer.Scatter(map, SunLightComputer.list);
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x0007FBE3 File Offset: 0x0007DDE3
	private static void RemoveLight(Map map, Vector3i pos)
	{
		SunLightComputer.list.Clear();
		SunLightComputer.list.Add(pos);
		SunLightComputer.RemoveLight(map, SunLightComputer.list);
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x0007FC08 File Offset: 0x0007DE08
	private static void RemoveLight(Map map, List<Vector3i> list)
	{
		SunLightMap sunLightmap = map.GetSunLightmap();
		foreach (Vector3i pos in list)
		{
			sunLightmap.SetLight(15, pos);
		}
		List<Vector3i> list2 = new List<Vector3i>();
		for (int i = 0; i < list.Count; i++)
		{
			Vector3i vector3i = list[i];
			if (vector3i.y >= 0)
			{
				if (sunLightmap.IsSunLight(vector3i.x, vector3i.y, vector3i.z))
				{
					list2.Add(vector3i);
				}
				else
				{
					byte b = sunLightmap.GetLight(vector3i) - 1;
					sunLightmap.SetLight(5, vector3i);
					if (b > 5)
					{
						foreach (Vector3i b2 in Vector3i.directions)
						{
							Vector3i vector3i2 = vector3i + b2;
							BlockData block = map.GetBlock(vector3i2);
							if (block.IsAlpha())
							{
								if (sunLightmap.GetLight(vector3i2) <= b)
								{
									list.Add(vector3i2);
								}
								else
								{
									list2.Add(vector3i2);
								}
							}
							if (!block.IsEmpty())
							{
								LightComputerUtils.SetLightDirty(map, vector3i2);
							}
						}
					}
				}
			}
		}
		SunLightComputer.Scatter(map, list2);
	}

	// Token: 0x04000F81 RID: 3969
	public const byte MIN_LIGHT = 5;

	// Token: 0x04000F82 RID: 3970
	public const byte MAX_LIGHT = 15;

	// Token: 0x04000F83 RID: 3971
	public const byte STEP_LIGHT = 1;

	// Token: 0x04000F84 RID: 3972
	private static List<Vector3i> list = new List<Vector3i>();
}
