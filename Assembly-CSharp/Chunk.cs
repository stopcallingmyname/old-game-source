using System;

// Token: 0x02000102 RID: 258
public class Chunk
{
	// Token: 0x06000949 RID: 2377 RVA: 0x0007ECD7 File Offset: 0x0007CED7
	public Chunk(Map map, Vector3i position)
	{
		this.map = map;
		this.position = position;
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x0007ECFB File Offset: 0x0007CEFB
	public ChunkRenderer GetChunkRendererInstance()
	{
		if (this.chunkRenderer == null)
		{
			this.chunkRenderer = ChunkRenderer.CreateChunkRenderer(this.position, this.map, this);
		}
		return this.chunkRenderer;
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x0007ED29 File Offset: 0x0007CF29
	public ChunkRenderer GetChunkRenderer()
	{
		return this.chunkRenderer;
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x0007ED31 File Offset: 0x0007CF31
	public void SetBlock(BlockData block, Vector3i pos)
	{
		this.SetBlock(block, pos.x, pos.y, pos.z);
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x0007ED4C File Offset: 0x0007CF4C
	public void SetBlock(BlockData block, int x, int y, int z)
	{
		this.blocks[z, y, x] = block;
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x0007ED5E File Offset: 0x0007CF5E
	public BlockData GetBlock(Vector3i pos)
	{
		return this.GetBlock(pos.x, pos.y, pos.z);
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x0007ED78 File Offset: 0x0007CF78
	public BlockData GetBlock(int x, int y, int z)
	{
		return this.blocks[z, y, x];
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x0007ED88 File Offset: 0x0007CF88
	public Map GetMap()
	{
		return this.map;
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0007ED90 File Offset: 0x0007CF90
	public Vector3i GetPosition()
	{
		return this.position;
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x0007ED98 File Offset: 0x0007CF98
	public static bool FixCoords(ref Vector3i chunk, ref Vector3i local)
	{
		bool result = false;
		if (local.x < 0)
		{
			chunk.x--;
			local.x += 8;
			result = true;
		}
		if (local.y < 0)
		{
			chunk.y--;
			local.y += 8;
			result = true;
		}
		if (local.z < 0)
		{
			chunk.z--;
			local.z += 8;
			result = true;
		}
		if (local.x >= 8)
		{
			chunk.x++;
			local.x -= 8;
			result = true;
		}
		if (local.y >= 8)
		{
			chunk.y++;
			local.y -= 8;
			result = true;
		}
		if (local.z >= 8)
		{
			chunk.z++;
			local.z -= 8;
			result = true;
		}
		return result;
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x0007EE6E File Offset: 0x0007D06E
	public static bool IsCorrectLocalPosition(Vector3i local)
	{
		return Chunk.IsCorrectLocalPosition(local.x, local.y, local.z);
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x0007EE87 File Offset: 0x0007D087
	public static bool IsCorrectLocalPosition(int x, int y, int z)
	{
		return (x & 7) == x && (y & 7) == y && (z & 7) == z;
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x0007EE9D File Offset: 0x0007D09D
	public static Vector3i ToChunkPosition(Vector3i point)
	{
		return Chunk.ToChunkPosition(point.x, point.y, point.z);
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x0007EEB8 File Offset: 0x0007D0B8
	public static Vector3i ToChunkPosition(int pointX, int pointY, int pointZ)
	{
		int x = pointX >> 3;
		int y = pointY >> 3;
		int z = pointZ >> 3;
		return new Vector3i(x, y, z);
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x0007EED7 File Offset: 0x0007D0D7
	public static Vector3i ToLocalPosition(Vector3i point)
	{
		return Chunk.ToLocalPosition(point.x, point.y, point.z);
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x0007EEF0 File Offset: 0x0007D0F0
	public static Vector3i ToLocalPosition(int pointX, int pointY, int pointZ)
	{
		int x = pointX & 7;
		int y = pointY & 7;
		int z = pointZ & 7;
		return new Vector3i(x, y, z);
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x0007EF10 File Offset: 0x0007D110
	public static Vector3i ToWorldPosition(Vector3i chunkPosition, Vector3i localPosition)
	{
		int x = (chunkPosition.x << 3) + localPosition.x;
		int y = (chunkPosition.y << 3) + localPosition.y;
		int z = (chunkPosition.z << 3) + localPosition.z;
		return new Vector3i(x, y, z);
	}

	// Token: 0x04000F60 RID: 3936
	public const int SIZE_X_BITS = 3;

	// Token: 0x04000F61 RID: 3937
	public const int SIZE_Y_BITS = 3;

	// Token: 0x04000F62 RID: 3938
	public const int SIZE_Z_BITS = 3;

	// Token: 0x04000F63 RID: 3939
	public const int SIZE_X = 8;

	// Token: 0x04000F64 RID: 3940
	public const int SIZE_Y = 8;

	// Token: 0x04000F65 RID: 3941
	public const int SIZE_Z = 8;

	// Token: 0x04000F66 RID: 3942
	private BlockData[,,] blocks = new BlockData[8, 8, 8];

	// Token: 0x04000F67 RID: 3943
	private Map map;

	// Token: 0x04000F68 RID: 3944
	private Vector3i position;

	// Token: 0x04000F69 RID: 3945
	private ChunkRenderer chunkRenderer;
}
