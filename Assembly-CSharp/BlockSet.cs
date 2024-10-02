using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000FB RID: 251
[AddComponentMenu("VoxelEngine/BlockSet")]
[ExecuteInEditMode]
public class BlockSet : ScriptableObject
{
	// Token: 0x0600090B RID: 2315 RVA: 0x0007DD6B File Offset: 0x0007BF6B
	private void OnEnable()
	{
		BlockSetImport.Import(this, this.data);
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0007DD79 File Offset: 0x0007BF79
	public void SetAtlases(Atlas[] atlases)
	{
		this.atlases = atlases;
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0007DD82 File Offset: 0x0007BF82
	public Atlas[] GetAtlases()
	{
		return this.atlases;
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x0007DD8A File Offset: 0x0007BF8A
	public Atlas GetAtlas(int i)
	{
		if (i < 0 || i >= this.atlases.Length)
		{
			return null;
		}
		return this.atlases[i];
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x0007DDA8 File Offset: 0x0007BFA8
	public Material[] GetMaterials(int count)
	{
		Material[] array = new Material[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = this.atlases[i].GetMaterial();
		}
		return array;
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x0007DDD9 File Offset: 0x0007BFD9
	public void SetBlocks(Block[] blocks)
	{
		this.blocks = blocks;
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x0007DDE2 File Offset: 0x0007BFE2
	public Block[] GetBlocks()
	{
		return this.blocks;
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x0007DDEA File Offset: 0x0007BFEA
	public int GetBlockCount()
	{
		return this.blocks.Length;
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x0007DDF4 File Offset: 0x0007BFF4
	public Block GetBlock(int index)
	{
		if (index < 0 || index >= this.blocks.Length)
		{
			return null;
		}
		return this.blocks[index];
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x0007DE10 File Offset: 0x0007C010
	public Block GetBlock(string name)
	{
		foreach (Block block in this.blocks)
		{
			if (block.GetName() == name)
			{
				return block;
			}
		}
		return null;
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x0007DE48 File Offset: 0x0007C048
	public T GetBlock<T>(string name) where T : Block
	{
		foreach (Block block in this.blocks)
		{
			if (block.GetName() == name && block is T)
			{
				return (T)((object)block);
			}
		}
		return default(T);
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x0007DE94 File Offset: 0x0007C094
	public Block[] GetBlocks(string name)
	{
		List<Block> list = new List<Block>();
		foreach (Block block in this.blocks)
		{
			if (block.GetName() == name)
			{
				list.Add(block);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x0007DEDB File Offset: 0x0007C0DB
	public void SetData(string data)
	{
		this.data = data;
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x0007DEE4 File Offset: 0x0007C0E4
	public string GetData()
	{
		return this.data;
	}

	// Token: 0x04000F4E RID: 3918
	[SerializeField]
	private string data = "";

	// Token: 0x04000F4F RID: 3919
	private Atlas[] atlases = new Atlas[0];

	// Token: 0x04000F50 RID: 3920
	private Block[] blocks = new Block[0];
}
