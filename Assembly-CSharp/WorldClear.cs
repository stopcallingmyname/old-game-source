using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000120 RID: 288
[AddComponentMenu("VoxelEngine/WorldGenerator")]
public class WorldClear : MonoBehaviour
{
	// Token: 0x06000A41 RID: 2625 RVA: 0x00082C20 File Offset: 0x00080E20
	private void Awake()
	{
		this.map = base.GetComponent<Map>();
		this.terrainGenerator = new TerrainClear(this.map);
		for (int i = 0; i < 32; i++)
		{
			for (int j = 0; j < 32; j++)
			{
				this.terrainGenerator.GenerateChunk(i, 0, j);
			}
		}
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x00082C72 File Offset: 0x00080E72
	private void Update()
	{
		if (!this.building)
		{
			base.StartCoroutine(this.Building());
		}
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x00082C89 File Offset: 0x00080E89
	private IEnumerator Building()
	{
		this.building = true;
		Vector3 position = Camera.main.transform.position;
		Vector3i vector3i = Chunk.ToChunkPosition((int)position.x, (int)position.y, (int)position.z);
		Vector3i? closestEmptyColumn = this.columnMap.GetClosestEmptyColumn(vector3i.x, vector3i.z, 40);
		if (closestEmptyColumn != null)
		{
			string[] array = new string[6];
			array[0] = "Building( ) column: ";
			int num = 1;
			Vector3i value = closestEmptyColumn.Value;
			array[num] = value.x.ToString();
			array[2] = " ";
			int num2 = 3;
			value = closestEmptyColumn.Value;
			array[num2] = value.y.ToString();
			array[4] = " ";
			int num3 = 5;
			value = closestEmptyColumn.Value;
			array[num3] = value.z.ToString();
			MonoBehaviour.print(string.Concat(array));
		}
		if (closestEmptyColumn != null)
		{
			int x = closestEmptyColumn.Value.x;
			int z = closestEmptyColumn.Value.z;
			this.columnMap.SetBuilt(x, z);
			ChunkSunLightComputer.ComputeRays(this.map, x, z);
			ChunkSunLightComputer.Scatter(this.map, this.columnMap, x, z);
			yield return base.StartCoroutine(this.BuildColumn(x, z));
		}
		this.building = false;
		yield break;
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x00082C98 File Offset: 0x00080E98
	private IEnumerator GenerateColumn(int cx, int cz)
	{
		yield return null;
		yield break;
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x00082CA0 File Offset: 0x00080EA0
	public IEnumerator BuildColumn(int cx, int cz)
	{
		int num;
		for (int cy = 0; cy < 4; cy = num + 1)
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

	// Token: 0x06000A46 RID: 2630 RVA: 0x00082CC0 File Offset: 0x00080EC0
	private void BuildColumn_(int cx, int cz)
	{
		for (int i = 0; i < 4; i++)
		{
			Chunk chunk = this.map.GetChunk(new Vector3i(cx, i, cz));
			if (chunk != null)
			{
				chunk.GetChunkRendererInstance().SetDirty();
			}
		}
	}

	// Token: 0x04000FFB RID: 4091
	private Map map;

	// Token: 0x04000FFC RID: 4092
	private ColumnMap columnMap = new ColumnMap();

	// Token: 0x04000FFD RID: 4093
	private TerrainClear terrainGenerator;

	// Token: 0x04000FFE RID: 4094
	private bool building;
}
