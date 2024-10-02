using System;
using System.Collections.Generic;

// Token: 0x02000105 RID: 261
public class LightComputer
{
	// Token: 0x0600096F RID: 2415 RVA: 0x0007F4F0 File Offset: 0x0007D6F0
	public static void RecomputeLightAtPosition(Map map, Vector3i pos)
	{
		int light = (int)map.GetLightmap().GetLight(pos);
		int light2 = (int)map.GetBlock(pos).GetLight();
		if (light > light2)
		{
			LightComputer.RemoveLight(map, pos);
		}
		if (light2 > 5)
		{
			LightComputer.Scatter(map, pos);
		}
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x0007F52E File Offset: 0x0007D72E
	private static void Scatter(Map map, Vector3i pos)
	{
		LightComputer.list.Clear();
		LightComputer.list.Add(pos);
		LightComputer.Scatter(map, LightComputer.list);
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x0007F550 File Offset: 0x0007D750
	private static void Scatter(Map map, List<Vector3i> list)
	{
		LightMap lightmap = map.GetLightmap();
		foreach (Vector3i pos in list)
		{
			byte light = map.GetBlock(pos).GetLight();
			if (light > 5)
			{
				lightmap.SetMaxLight(light, pos);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			Vector3i vector3i = list[i];
			if (vector3i.y >= 0)
			{
				BlockData block = map.GetBlock(vector3i);
				int num = (int)lightmap.GetLight(vector3i) - LightComputerUtils.GetLightStep(block);
				if (num > 5)
				{
					foreach (Vector3i b in Vector3i.directions)
					{
						Vector3i vector3i2 = vector3i + b;
						block = map.GetBlock(vector3i2);
						if (block.IsAlpha() && lightmap.SetMaxLight((byte)num, vector3i2))
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

	// Token: 0x06000972 RID: 2418 RVA: 0x0007F678 File Offset: 0x0007D878
	private static void RemoveLight(Map map, Vector3i pos)
	{
		LightComputer.list.Clear();
		LightComputer.list.Add(pos);
		LightComputer.RemoveLight(map, LightComputer.list);
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x0007F69C File Offset: 0x0007D89C
	private static void RemoveLight(Map map, List<Vector3i> list)
	{
		LightMap lightmap = map.GetLightmap();
		foreach (Vector3i pos in list)
		{
			lightmap.SetLight(15, pos);
		}
		List<Vector3i> list2 = new List<Vector3i>();
		for (int i = 0; i < list.Count; i++)
		{
			Vector3i vector3i = list[i];
			if (vector3i.y >= 0)
			{
				int num = (int)(lightmap.GetLight(vector3i) - 1);
				lightmap.SetLight(5, vector3i);
				if (num > 5)
				{
					foreach (Vector3i b in Vector3i.directions)
					{
						Vector3i vector3i2 = vector3i + b;
						BlockData block = map.GetBlock(vector3i2);
						if (block.IsAlpha())
						{
							if ((int)lightmap.GetLight(vector3i2) <= num)
							{
								list.Add(vector3i2);
							}
							else
							{
								list2.Add(vector3i2);
							}
						}
						if (block.GetLight() > 5)
						{
							list2.Add(vector3i2);
						}
						if (!block.IsEmpty())
						{
							LightComputerUtils.SetLightDirty(map, vector3i2);
						}
					}
				}
			}
		}
		LightComputer.Scatter(map, list2);
	}

	// Token: 0x04000F7C RID: 3964
	public const byte MIN_LIGHT = 5;

	// Token: 0x04000F7D RID: 3965
	public const byte MAX_LIGHT = 15;

	// Token: 0x04000F7E RID: 3966
	public const byte STEP_LIGHT = 1;

	// Token: 0x04000F7F RID: 3967
	private static List<Vector3i> list = new List<Vector3i>();
}
