using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class MapCulling : MonoBehaviour
{
	// Token: 0x06000A0C RID: 2572 RVA: 0x0008145C File Offset: 0x0007F65C
	private void Awake()
	{
		this.map = base.GetComponent<Map>();
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x0008146A File Offset: 0x0007F66A
	private void Update()
	{
		if (!this.building)
		{
			base.StartCoroutine(this.Building());
		}
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x00081481 File Offset: 0x0007F681
	private IEnumerator Building()
	{
		this.building = true;
		Vector3 position = Camera.main.transform.position;
		Vector3i vector3i = Chunk.ToChunkPosition((int)position.x, (int)position.y, (int)position.z);
		Vector3i? closestEmptyColumn = this.columnMap.GetClosestEmptyColumn(vector3i.x, vector3i.z, 4);
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

	// Token: 0x06000A0F RID: 2575 RVA: 0x00081490 File Offset: 0x0007F690
	public IEnumerator BuildColumn(int cx, int cz)
	{
		int num;
		for (int cy = 0; cy < 8; cy = num + 1)
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

	// Token: 0x04000FBC RID: 4028
	private Map map;

	// Token: 0x04000FBD RID: 4029
	private ColumnMap columnMap = new ColumnMap();

	// Token: 0x04000FBE RID: 4030
	private bool building;
}
