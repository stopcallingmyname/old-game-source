using System;

// Token: 0x0200011F RID: 287
public class TerrainClear
{
	// Token: 0x06000A3F RID: 2623 RVA: 0x00082AA0 File Offset: 0x00080CA0
	public TerrainClear(Map map)
	{
		this.map = map;
		BlockSet blockSet = map.GetBlockSet();
		this.water = blockSet.GetBlock("Water");
		this.grass = blockSet.GetBlock("Grass");
		this.dirt = blockSet.GetBlock("Dirt");
		this.sand = blockSet.GetBlock("Sand");
		this.stoneend = blockSet.GetBlock("Stoneend");
		this.snow = blockSet.GetBlock("Snow");
		this.ice = blockSet.GetBlock("Ice");
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x00082B94 File Offset: 0x00080D94
	public void GenerateChunk(int cx, int cy, int cz)
	{
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				for (int k = 0; k < 7; k++)
				{
					Vector3i pos = Chunk.ToWorldPosition(new Vector3i(cx, cy, cz), new Vector3i(j, k, i));
					if (k < 2)
					{
						this.map.SetBlock(this.stoneend, pos);
					}
					else if (k < 6)
					{
						this.map.SetBlock(this.dirt, pos);
					}
					else
					{
						this.map.SetBlock(this.grass, pos);
					}
				}
			}
		}
	}

	// Token: 0x04000FED RID: 4077
	private const int WATER_LEVEL = -999;

	// Token: 0x04000FEE RID: 4078
	private NoiseArray2D terrainNoise = new NoiseArray2D(0.02f).SetOctaves(1);

	// Token: 0x04000FEF RID: 4079
	private NoiseArray3D terrainNoise3D = new NoiseArray3D(0.033333335f);

	// Token: 0x04000FF0 RID: 4080
	private NoiseArray2D islandNoise = new NoiseArray2D(0.008333334f).SetOctaves(3);

	// Token: 0x04000FF1 RID: 4081
	private NoiseArray3D islandNoise3D = new NoiseArray3D(0.025f);

	// Token: 0x04000FF2 RID: 4082
	private NoiseArray3D caveNoise3D = new NoiseArray3D(0.02f);

	// Token: 0x04000FF3 RID: 4083
	private Map map;

	// Token: 0x04000FF4 RID: 4084
	private Block water;

	// Token: 0x04000FF5 RID: 4085
	private Block grass;

	// Token: 0x04000FF6 RID: 4086
	private Block dirt;

	// Token: 0x04000FF7 RID: 4087
	private Block sand;

	// Token: 0x04000FF8 RID: 4088
	private Block stoneend;

	// Token: 0x04000FF9 RID: 4089
	private Block snow;

	// Token: 0x04000FFA RID: 4090
	private Block ice;
}
