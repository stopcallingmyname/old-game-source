using System;
using System.Collections;

// Token: 0x0200011C RID: 284
public class TerrainGenerator
{
	// Token: 0x06000A2A RID: 2602 RVA: 0x00082298 File Offset: 0x00080498
	public TerrainGenerator(Map map)
	{
		this.map = map;
		BlockSet blockSet = map.GetBlockSet();
		this.water = blockSet.GetBlock("Water");
		this.grass = blockSet.GetBlock("Grass");
		this.dirt = blockSet.GetBlock("Dirt");
		this.sand = blockSet.GetBlock("Sand");
		this.stone = blockSet.GetBlock("Grass");
		this.snow = blockSet.GetBlock("Snow");
		this.ice = blockSet.GetBlock("Snow");
		this.stoneend = blockSet.GetBlock("Stoneend");
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x0008239D File Offset: 0x0008059D
	public IEnumerator Generate(int cx, int cz)
	{
		if (cx < 0 || cz < 0)
		{
			yield return null;
		}
		if (cx > 32 || cz > 32)
		{
			yield return null;
		}
		this.terrainNoise.GenerateNoise(cx * 8, cz * 8);
		this.islandNoise.GenerateNoise(cx * 8, cz * 8);
		int cy = 0;
		for (;;)
		{
			Vector3i offset = Chunk.ToWorldPosition(new Vector3i(cx, cy, cz), Vector3i.zero);
			this.terrainNoise3D.GenerateNoise(offset);
			this.islandNoise3D.GenerateNoise(offset);
			this.caveNoise3D.GenerateNoise(offset);
			if (!this.GenerateChunk(new Vector3i(cx, cy, cz)))
			{
				break;
			}
			yield return null;
			int num = cy;
			cy = num + 1;
		}
		yield break;
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x000823BC File Offset: 0x000805BC
	private bool GenerateChunk(Vector3i chunkPos)
	{
		bool result = false;
		for (int i = -1; i < 9; i++)
		{
			for (int j = -1; j < 9; j++)
			{
				for (int k = 0; k < 8; k++)
				{
					Vector3i vector3i = Chunk.ToWorldPosition(chunkPos, new Vector3i(j, k, i));
					if (vector3i.y <= 2)
					{
						this.map.SetBlock(this.stoneend, vector3i);
						result = true;
					}
					if (vector3i.y <= 6)
					{
						this.map.SetBlock(this.dirt, vector3i);
						result = true;
					}
					int terrainHeight = this.GetTerrainHeight(vector3i.x, vector3i.z);
					if (vector3i.y <= terrainHeight)
					{
						this.GenerateBlockForBaseTerrain(vector3i);
						result = true;
					}
					else
					{
						int islandHeight = this.GetIslandHeight(vector3i.x, vector3i.z);
						if (vector3i.y <= islandHeight)
						{
							this.GenerateBlockForIsland(vector3i, islandHeight - vector3i.y, islandHeight);
							result = true;
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x000824B8 File Offset: 0x000806B8
	public void GeneratePlants(int cx, int cz)
	{
		for (int i = -1; i < 9; i++)
		{
			for (int j = -1; j < 9; j++)
			{
				Vector3i vector3i = new Vector3i(cx * 8 + j, 0, cz * 8 + i);
				vector3i.y = this.map.GetMaxY(vector3i.x, vector3i.z);
				while (vector3i.y >= 5)
				{
					if (this.map.GetBlock(vector3i).block == this.dirt && this.map.GetSunLightmap().GetLight(vector3i + Vector3i.up) > 5)
					{
						this.map.SetBlock(this.grass, vector3i);
					}
					vector3i.y--;
				}
			}
		}
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x0008257B File Offset: 0x0008077B
	private int GetTerrainHeight(int x, int z)
	{
		return (int)(this.terrainNoise.GetNoise(x, z) * 10f);
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x00082591 File Offset: 0x00080791
	private int GetIslandHeight(int x, int z)
	{
		return (int)(this.islandNoise.GetNoise(x, z) * 50f);
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x000825A8 File Offset: 0x000807A8
	private void GenerateBlockForBaseTerrain(Vector3i worldPos)
	{
		float noise = this.terrainNoise3D.GetNoise(worldPos.x, worldPos.y, worldPos.z);
		Block block = null;
		if (TerrainGenerator.IsInRange(noise, 0f, 0.2f))
		{
			block = this.sand;
		}
		if (TerrainGenerator.IsInRange(noise, 0.2f, 0.6f))
		{
			block = this.dirt;
		}
		if (TerrainGenerator.IsInRange(noise, 0.6f, 1f))
		{
			block = this.stone;
		}
		if (block != null)
		{
			this.map.SetBlock(block, worldPos);
		}
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x00082630 File Offset: 0x00080830
	private void GenerateBlockForIsland(Vector3i worldPos, int deep, int height)
	{
		if (this.caveNoise3D.GetNoise(worldPos.x, worldPos.y, worldPos.z) > 0.7f)
		{
			return;
		}
		float noise = this.islandNoise3D.GetNoise(worldPos.x, worldPos.y, worldPos.z);
		Block block = null;
		if (TerrainGenerator.IsInRange(noise, 0f, 0.2f))
		{
			block = this.sand;
		}
		if (TerrainGenerator.IsInRange(noise, 0.2f, 0.6f))
		{
			block = this.dirt;
		}
		if (TerrainGenerator.IsInRange(noise, 0.6f, 1f))
		{
			block = this.stone;
		}
		if (block != null)
		{
			this.map.SetBlock(block, worldPos);
		}
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x000826DB File Offset: 0x000808DB
	private static bool IsInRange(float val, float min, float max)
	{
		return val >= min && val <= max;
	}

	// Token: 0x04000FD6 RID: 4054
	private const int WATER_LEVEL = 5;

	// Token: 0x04000FD7 RID: 4055
	private NoiseArray2D terrainNoise = new NoiseArray2D(0.02f).SetOctaves(1);

	// Token: 0x04000FD8 RID: 4056
	private NoiseArray3D terrainNoise3D = new NoiseArray3D(0.033333335f);

	// Token: 0x04000FD9 RID: 4057
	private NoiseArray2D islandNoise = new NoiseArray2D(0.006666667f).SetOctaves(3);

	// Token: 0x04000FDA RID: 4058
	private NoiseArray3D islandNoise3D = new NoiseArray3D(0.033333335f);

	// Token: 0x04000FDB RID: 4059
	private NoiseArray3D caveNoise3D = new NoiseArray3D(1f);

	// Token: 0x04000FDC RID: 4060
	private Map map;

	// Token: 0x04000FDD RID: 4061
	private Block water;

	// Token: 0x04000FDE RID: 4062
	private Block grass;

	// Token: 0x04000FDF RID: 4063
	private Block dirt;

	// Token: 0x04000FE0 RID: 4064
	private Block sand;

	// Token: 0x04000FE1 RID: 4065
	private Block stone;

	// Token: 0x04000FE2 RID: 4066
	private Block snow;

	// Token: 0x04000FE3 RID: 4067
	private Block ice;

	// Token: 0x04000FE4 RID: 4068
	private Block stoneend;
}
