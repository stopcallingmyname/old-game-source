using System;
using UnityEngine;

// Token: 0x020000FE RID: 254
internal class ChunkBuilder
{
	// Token: 0x0600092A RID: 2346 RVA: 0x0007E63E File Offset: 0x0007C83E
	public static Mesh BuildChunk(Mesh mesh, Chunk chunk)
	{
		ChunkBuilder.Build(chunk, false);
		return ChunkBuilder.meshData.ToMesh(mesh);
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x0007E654 File Offset: 0x0007C854
	public static void BuildChunkLighting(Mesh mesh, Chunk chunk)
	{
		ChunkBuilder.Build(chunk, true);
		if (ChunkBuilder.meshData.GetColors() == null)
		{
			return;
		}
		if (mesh == null)
		{
			return;
		}
		if (mesh.colors.Length == ChunkBuilder.meshData.GetColors().ToArray().Length)
		{
			mesh.colors = ChunkBuilder.meshData.GetColors().ToArray();
		}
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x0007E6B0 File Offset: 0x0007C8B0
	private static void Build(Chunk chunk, bool onlyLight)
	{
		Map map = chunk.GetMap();
		ChunkBuilder.meshData.Clear();
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				for (int k = 0; k < 8; k++)
				{
					Block block = chunk.GetBlock(k, j, i).block;
					if (block != null)
					{
						Vector3i vector3i = new Vector3i(k, j, i);
						Vector3i worldPos = Chunk.ToWorldPosition(chunk.GetPosition(), vector3i);
						block.Build(vector3i, worldPos, map, ChunkBuilder.meshData, onlyLight);
					}
				}
			}
		}
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x0007E734 File Offset: 0x0007C934
	public static bool BuildChunkPro(MeshFilter filter, MeshCollider collider, Chunk chunk)
	{
		ChunkBuilder.lastupdate = Time.time;
		if (ChunkBuilder.Build(chunk, false, false))
		{
			filter.sharedMesh = ChunkBuilder.meshData.ToMesh(null);
			if (filter.sharedMesh != null)
			{
				ChunkBuilder.Build(chunk, false, true);
				collider.sharedMesh = ChunkBuilder.meshData.ToMesh(null);
			}
			else
			{
				collider.sharedMesh = null;
			}
			return false;
		}
		filter.sharedMesh = ChunkBuilder.meshData.ToMesh(null);
		return true;
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x0007E7AB File Offset: 0x0007C9AB
	public static void BuildChunkCollider(MeshFilter filter, MeshCollider collider)
	{
		collider.sharedMesh = null;
		if (filter.sharedMesh == null)
		{
			return;
		}
		collider.sharedMesh = filter.sharedMesh;
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x0007E7D0 File Offset: 0x0007C9D0
	private static bool Build(Chunk chunk, bool onlyLight, bool solidignore)
	{
		bool result = false;
		Map map = chunk.GetMap();
		ChunkBuilder.meshData.Clear();
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				for (int k = 0; k < 8; k++)
				{
					Block block = chunk.GetBlock(k, j, i).block;
					if (block != null)
					{
						if (block.GetName()[0] == '!')
						{
							result = true;
							if (solidignore)
							{
								goto IL_77;
							}
						}
						Vector3i vector3i = new Vector3i(k, j, i);
						Vector3i worldPos = Chunk.ToWorldPosition(chunk.GetPosition(), vector3i);
						block.Build(vector3i, worldPos, map, ChunkBuilder.meshData, onlyLight);
					}
					IL_77:;
				}
			}
		}
		return result;
	}

	// Token: 0x04000F51 RID: 3921
	private static MeshBuilder meshData = new MeshBuilder();

	// Token: 0x04000F52 RID: 3922
	public static float lastupdate = 0f;
}
