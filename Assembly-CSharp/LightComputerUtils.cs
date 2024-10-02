using System;

// Token: 0x02000106 RID: 262
internal class LightComputerUtils
{
	// Token: 0x06000976 RID: 2422 RVA: 0x0007F7E0 File Offset: 0x0007D9E0
	public static void SetLightDirty(Map map, Vector3i pos)
	{
		Vector3i vector3i = Chunk.ToChunkPosition(pos);
		Chunk.ToLocalPosition(pos);
		LightComputerUtils.SetChunkLightDirty(map, vector3i);
		LightComputerUtils.SetChunkLightDirty(map, vector3i - Vector3i.right);
		LightComputerUtils.SetChunkLightDirty(map, vector3i - Vector3i.up);
		LightComputerUtils.SetChunkLightDirty(map, vector3i - Vector3i.forward);
		LightComputerUtils.SetChunkLightDirty(map, vector3i + Vector3i.right);
		LightComputerUtils.SetChunkLightDirty(map, vector3i + Vector3i.up);
		LightComputerUtils.SetChunkLightDirty(map, vector3i + Vector3i.forward);
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x0007F868 File Offset: 0x0007DA68
	private static void SetChunkLightDirty(Map map, Vector3i chunkPos)
	{
		Chunk chunk = map.GetChunk(chunkPos);
		if (chunk == null)
		{
			return;
		}
		ChunkRenderer chunkRenderer = chunk.GetChunkRenderer();
		if (chunkRenderer == null)
		{
			return;
		}
		chunkRenderer.SetLightDirty();
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x0007F898 File Offset: 0x0007DA98
	public static int GetLightStep(BlockData block)
	{
		if (block.IsEmpty())
		{
			return 1;
		}
		return 2;
	}
}
