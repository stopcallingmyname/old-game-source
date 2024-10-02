using System;
using UnityEngine;

// Token: 0x0200011D RID: 285
public class TreeGenerator
{
	// Token: 0x06000A33 RID: 2611 RVA: 0x000826EC File Offset: 0x000808EC
	public TreeGenerator(Map map)
	{
		this.map = map;
		BlockSet blockSet = map.GetBlockSet();
		this.wood = blockSet.GetBlock("Wood");
		this.leaves = blockSet.GetBlock("Leaves");
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x0008272F File Offset: 0x0008092F
	public TreeGenerator(Map map, Block wood, Block leaves)
	{
		this.map = map;
		this.wood = wood;
		this.leaves = leaves;
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x0008274C File Offset: 0x0008094C
	public void Generate(int x, int y, int z)
	{
		BlockData block = this.map.GetBlock(x, y - 1, z);
		if (block.IsEmpty() || !block.block.GetName().Equals("Dirt"))
		{
			return;
		}
		if (Random.Range(0f, 1f) > 0.2f)
		{
			return;
		}
		this.GenerateTree(x, y, z);
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x000827AC File Offset: 0x000809AC
	private void GenerateTree(int x, int y, int z)
	{
		this.GenerateLeaves(new Vector3i(x, y + 6, z), new Vector3i(x, y + 6, z));
		for (int i = 0; i < 8; i++)
		{
			this.map.SetBlock(new BlockData(this.wood), new Vector3i(x, y + i, z));
		}
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x00082800 File Offset: 0x00080A00
	private void GenerateLeaves(Vector3i center, Vector3i pos)
	{
		Vector3 vector = center - pos;
		vector.y *= 2f;
		if (vector.sqrMagnitude > 36f)
		{
			return;
		}
		if (!this.map.GetBlock(pos).IsEmpty())
		{
			return;
		}
		this.map.SetBlock(this.leaves, pos);
		foreach (Vector3i b in Vector3i.directions)
		{
			this.GenerateLeaves(center, pos + b);
		}
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x00082890 File Offset: 0x00080A90
	private void GenerateLeaves(Vector3i center)
	{
		int num = center.x - 6;
		int num2 = center.y - 6;
		int num3 = center.z - 6;
		int num4 = center.x + 6;
		int num5 = center.y + 6;
		int num6 = center.z + 6;
		for (int i = num; i <= num4; i++)
		{
			for (int j = num2; j <= num5; j++)
			{
				for (int k = num3; k <= num6; k++)
				{
					Vector3 vector = center - new Vector3i(i, j, k);
					vector.y *= 2f;
					if (vector.sqrMagnitude <= 36f && this.map.GetBlock(i, j, k).IsEmpty())
					{
						this.map.SetBlock(this.leaves, new Vector3i(i, j, k));
					}
				}
			}
		}
	}

	// Token: 0x04000FE5 RID: 4069
	private Map map;

	// Token: 0x04000FE6 RID: 4070
	private Block wood;

	// Token: 0x04000FE7 RID: 4071
	private Block leaves;
}
