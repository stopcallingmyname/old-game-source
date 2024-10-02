using System;

// Token: 0x02000104 RID: 260
public class ColumnMap
{
	// Token: 0x0600096A RID: 2410 RVA: 0x0007F412 File Offset: 0x0007D612
	public void SetBuilt(int x, int z)
	{
		this.GetColumnChunk(x, z).built = true;
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x0007F422 File Offset: 0x0007D622
	public bool IsBuilt(int x, int z)
	{
		return this.GetColumnChunk(x, z).built;
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0007F434 File Offset: 0x0007D634
	public Vector3i? GetClosestEmptyColumn(int cx, int cz, int rad)
	{
		Vector3i vector3i = new Vector3i(cx, 0, cz);
		Vector3i? result = null;
		for (int i = cz - rad; i <= cz + rad; i++)
		{
			for (int j = cx - rad; j <= cx + rad; j++)
			{
				Vector3i vector3i2 = new Vector3i(j, 0, i);
				int num = vector3i.DistanceSquared(vector3i2);
				if (num <= rad * rad && !this.IsBuilt(j, i))
				{
					if (result == null)
					{
						result = new Vector3i?(vector3i2);
					}
					else
					{
						int num2 = vector3i.DistanceSquared(result.Value);
						if (num < num2)
						{
							result = new Vector3i?(vector3i2);
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x0007F4CD File Offset: 0x0007D6CD
	private ColumnMap.ColumnChunk GetColumnChunk(int x, int z)
	{
		return this.columns.GetInstance(x, z);
	}

	// Token: 0x04000F7B RID: 3963
	private List2D<ColumnMap.ColumnChunk> columns = new List2D<ColumnMap.ColumnChunk>();

	// Token: 0x020008BE RID: 2238
	private class ColumnChunk
	{
		// Token: 0x040033D7 RID: 13271
		public bool built;
	}
}
