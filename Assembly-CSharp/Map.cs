using System;
using UnityEngine;

// Token: 0x0200010B RID: 267
[AddComponentMenu("VoxelEngine/Map")]
public class Map : MonoBehaviour
{
	// Token: 0x0600099D RID: 2461 RVA: 0x0008014C File Offset: 0x0007E34C
	private void Awake()
	{
		int tileset = Config.Tileset;
		if (tileset == 0)
		{
			this.blockSet = ContentLoader.LoadBlockSet("Simple/Simple");
		}
		else if (tileset == 1)
		{
			this.blockSet = ContentLoader.LoadBlockSet("Real/Real");
		}
		else if (tileset == 2)
		{
			this.blockSet = ContentLoader.LoadBlockSet("Apocalypse/Apocalypse");
		}
		else if (tileset == 3)
		{
			this.blockSet = ContentLoader.LoadBlockSet("Space/Space");
		}
		else if (tileset == 4)
		{
			this.blockSet = ContentLoader.LoadBlockSet("Steampunk/Steampunk");
		}
		int dlight = Config.Dlight;
		Material[] materials = this.blockSet.GetMaterials(1);
		if (dlight == 1)
		{
			materials[0].shader = Shader.Find("Custom/StandardVertex");
			GameObject gameObject = GameObject.Find("Dlight");
			if (gameObject)
			{
				gameObject.GetComponent<Light>().enabled = true;
				return;
			}
		}
		else
		{
			materials[0].shader = Shader.Find("Custom/StandardVertex");
		}
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x00080224 File Offset: 0x0007E424
	public void SetBlockAndRecompute(BlockData block, Vector3i pos)
	{
		this.SetBlock(block, pos);
		Vector3i vector3i = Chunk.ToChunkPosition(pos);
		Vector3i vector3i2 = Chunk.ToLocalPosition(pos);
		this.SetDirty(vector3i);
		if (vector3i2.x == 0)
		{
			this.SetDirty(vector3i - Vector3i.right);
		}
		if (vector3i2.y == 0)
		{
			this.SetDirty(vector3i - Vector3i.up);
		}
		if (vector3i2.z == 0)
		{
			this.SetDirty(vector3i - Vector3i.forward);
		}
		if (vector3i2.x == 7)
		{
			this.SetDirty(vector3i + Vector3i.right);
		}
		if (vector3i2.y == 7)
		{
			this.SetDirty(vector3i + Vector3i.up);
		}
		if (vector3i2.z == 7)
		{
			this.SetDirty(vector3i + Vector3i.forward);
		}
		SunLightComputer.RecomputeLightAtPosition(this, pos);
		LightComputer.RecomputeLightAtPosition(this, pos);
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x000802F3 File Offset: 0x0007E4F3
	public void BlockRecompute(Vector3i pos)
	{
		SunLightComputer.RecomputeLightAtPosition(this, pos);
		LightComputer.RecomputeLightAtPosition(this, pos);
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x00080304 File Offset: 0x0007E504
	private void SetDirty(Vector3i chunkPos)
	{
		Chunk chunk = this.GetChunk(chunkPos);
		if (chunk != null)
		{
			chunk.GetChunkRendererInstance().SetDirty();
		}
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x00080327 File Offset: 0x0007E527
	public void SetBlock(Block block, Vector3i pos)
	{
		this.SetBlock(new BlockData(block), pos);
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x00080336 File Offset: 0x0007E536
	public void SetBlock(Block block, int x, int y, int z)
	{
		this.SetBlock(new BlockData(block), x, y, z);
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x00080348 File Offset: 0x0007E548
	public void SetBlock(BlockData block, Vector3i pos)
	{
		this.SetBlock(block, pos.x, pos.y, pos.z);
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x00080364 File Offset: 0x0007E564
	public void SetBlock(BlockData block, int x, int y, int z)
	{
		Chunk chunkInstance = this.GetChunkInstance(Chunk.ToChunkPosition(x, y, z));
		if (chunkInstance != null)
		{
			chunkInstance.SetBlock(block, Chunk.ToLocalPosition(x, y, z));
		}
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x00080394 File Offset: 0x0007E594
	public BlockData GetBlock(Vector3i pos)
	{
		return this.GetBlock(pos.x, pos.y, pos.z);
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x000803B0 File Offset: 0x0007E5B0
	public BlockData GetBlock(int x, int y, int z)
	{
		Chunk chunk = this.GetChunk(Chunk.ToChunkPosition(x, y, z));
		if (chunk == null)
		{
			return default(BlockData);
		}
		return chunk.GetBlock(Chunk.ToLocalPosition(x, y, z));
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x000803E8 File Offset: 0x0007E5E8
	public int GetMaxY(int x, int z)
	{
		Vector3i vector3i = Chunk.ToChunkPosition(x, 0, z);
		vector3i.y = this.chunks.GetMax().y;
		Vector3i vector3i2 = Chunk.ToLocalPosition(x, 0, z);
		while (vector3i.y >= 0)
		{
			vector3i2.y = 7;
			while (vector3i2.y >= 0)
			{
				Chunk chunk = this.chunks.SafeGet(vector3i);
				if (chunk == null)
				{
					break;
				}
				if (!chunk.GetBlock(vector3i2).IsEmpty())
				{
					return Chunk.ToWorldPosition(vector3i, vector3i2).y;
				}
				vector3i2.y--;
			}
			vector3i.y--;
		}
		return 0;
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x00080484 File Offset: 0x0007E684
	public Chunk GetChunkInstance(Vector3i chunkPos)
	{
		if (chunkPos.y < 0)
		{
			return null;
		}
		Chunk chunk = this.GetChunk(chunkPos);
		if (chunk == null)
		{
			chunk = new Chunk(this, chunkPos);
			this.chunks.AddOrReplace(chunk, chunkPos);
		}
		return chunk;
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x000804BD File Offset: 0x0007E6BD
	public Chunk GetChunk(Vector3i chunkPos)
	{
		return this.chunks.SafeGet(chunkPos);
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x000804CB File Offset: 0x0007E6CB
	public List3D<Chunk> GetChunks()
	{
		return this.chunks;
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x000804D3 File Offset: 0x0007E6D3
	public SunLightMap GetSunLightmap()
	{
		return this.sunLightmap;
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x000804DB File Offset: 0x0007E6DB
	public LightMap GetLightmap()
	{
		return this.lightmap;
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x000804E3 File Offset: 0x0007E6E3
	public void SetBlockSet(BlockSet blockSet)
	{
		this.blockSet = blockSet;
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x000804EC File Offset: 0x0007E6EC
	public BlockSet GetBlockSet()
	{
		return this.blockSet;
	}

	// Token: 0x04000F8B RID: 3979
	public const int SIZE_X = 32;

	// Token: 0x04000F8C RID: 3980
	public const int SIZE_Y = 8;

	// Token: 0x04000F8D RID: 3981
	public const int SIZE_Z = 32;

	// Token: 0x04000F8E RID: 3982
	public const int MIN_X = 0;

	// Token: 0x04000F8F RID: 3983
	public const int MIN_Y = 0;

	// Token: 0x04000F90 RID: 3984
	public const int MIN_Z = 0;

	// Token: 0x04000F91 RID: 3985
	public const int MAX_X = 255;

	// Token: 0x04000F92 RID: 3986
	public const int MAX_Y = 63;

	// Token: 0x04000F93 RID: 3987
	public const int MAX_Z = 255;

	// Token: 0x04000F94 RID: 3988
	public Vector2 mlx = new Vector2(0f, 255f);

	// Token: 0x04000F95 RID: 3989
	public Vector2 mly = new Vector2(0f, 63f);

	// Token: 0x04000F96 RID: 3990
	public Vector2 mlz = new Vector2(0f, 255f);

	// Token: 0x04000F97 RID: 3991
	public Flag[] flags = new Flag[4];

	// Token: 0x04000F98 RID: 3992
	[SerializeField]
	private BlockSet blockSet;

	// Token: 0x04000F99 RID: 3993
	private List3D<Chunk> chunks = new List3D<Chunk>();

	// Token: 0x04000F9A RID: 3994
	private SunLightMap sunLightmap = new SunLightMap();

	// Token: 0x04000F9B RID: 3995
	private LightMap lightmap = new LightMap();
}
