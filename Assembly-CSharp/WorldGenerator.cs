using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200011E RID: 286
[AddComponentMenu("VoxelEngine/WorldGenerator")]
public class WorldGenerator : MonoBehaviour
{
	// Token: 0x06000A39 RID: 2617 RVA: 0x00082984 File Offset: 0x00080B84
	private void Awake()
	{
		this.map = base.GetComponent<Map>();
		this.terrainGenerator = new TerrainGenerator(this.map);
		Block[] blocks = this.map.GetBlockSet().GetBlocks("Wood");
		Block[] blocks2 = this.map.GetBlockSet().GetBlocks("Leaf");
		this.treeGenerator = new TreeGenerator[Math.Max(blocks.Length, blocks2.Length)];
		for (int i = 0; i < this.treeGenerator.Length; i++)
		{
			Block wood = blocks[i % blocks.Length];
			Block leaves = blocks2[i % blocks2.Length];
			this.treeGenerator[i] = new TreeGenerator(this.map, wood, leaves);
		}
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x00082A2A File Offset: 0x00080C2A
	private void Update()
	{
		if (!this.building)
		{
			base.StartCoroutine(this.Building());
		}
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x00082A41 File Offset: 0x00080C41
	private IEnumerator Building()
	{
		this.building = true;
		Vector3 position = Camera.main.transform.position;
		Vector3i vector3i = Chunk.ToChunkPosition((int)position.x, (int)position.y, (int)position.z);
		Vector3i? closestEmptyColumn = this.columnMap.GetClosestEmptyColumn(vector3i.x, vector3i.z, 7);
		if (closestEmptyColumn != null)
		{
			int cx = closestEmptyColumn.Value.x;
			int cz = closestEmptyColumn.Value.z;
			this.columnMap.SetBuilt(cx, cz);
			yield return base.StartCoroutine(this.GenerateColumn(cx, cz));
			yield return null;
			ChunkSunLightComputer.ComputeRays(this.map, cx, cz);
			ChunkSunLightComputer.Scatter(this.map, this.columnMap, cx, cz);
			this.terrainGenerator.GeneratePlants(cx, cz);
			yield return base.StartCoroutine(this.BuildColumn(cx, cz));
		}
		this.building = false;
		yield break;
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x00082A50 File Offset: 0x00080C50
	private IEnumerator GenerateColumn(int cx, int cz)
	{
		yield return base.StartCoroutine(this.terrainGenerator.Generate(cx, cz));
		yield return null;
		if (this.treeGenerator.Length != 0)
		{
			int x = cx * 8 + 4;
			int z = cz * 8 + 4;
			int y = this.map.GetMaxY(x, z) + 1;
			int num = Random.Range(0, this.treeGenerator.Length);
			this.treeGenerator[num].Generate(x, y, z);
		}
		yield break;
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x00082A6D File Offset: 0x00080C6D
	public IEnumerator BuildColumn(int cx, int cz)
	{
		List3D<Chunk> chunks = this.map.GetChunks();
		int num;
		for (int cy = chunks.GetMinY(); cy < chunks.GetMaxY(); cy = num + 1)
		{
			Chunk chunk = this.map.GetChunk(new Vector3i(cx, cy, cz));
			if (chunk != null)
			{
				chunk.GetChunkRendererInstance().SetDirty();
			}
			if (chunk != null)
			{
				yield return null;
			}
			num = cy;
		}
		yield break;
	}

	// Token: 0x04000FE8 RID: 4072
	private Map map;

	// Token: 0x04000FE9 RID: 4073
	private ColumnMap columnMap = new ColumnMap();

	// Token: 0x04000FEA RID: 4074
	private TerrainGenerator terrainGenerator;

	// Token: 0x04000FEB RID: 4075
	private TreeGenerator[] treeGenerator;

	// Token: 0x04000FEC RID: 4076
	private bool building;
}
