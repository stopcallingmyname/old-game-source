using System;

// Token: 0x02000112 RID: 274
public class Map3D<I>
{
	// Token: 0x060009E3 RID: 2531 RVA: 0x00080D90 File Offset: 0x0007EF90
	public Map3D()
	{
		this.defaultValue = default(I);
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x00080DAF File Offset: 0x0007EFAF
	public Map3D(I defaultValue)
	{
		this.defaultValue = defaultValue;
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x00080DC9 File Offset: 0x0007EFC9
	public void Set(I val, Vector3i pos)
	{
		this.Set(val, pos.x, pos.y, pos.z);
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x00080DE4 File Offset: 0x0007EFE4
	public void Set(I val, int x, int y, int z)
	{
		Vector3i pos = Chunk.ToChunkPosition(x, y, z);
		Vector3i pos2 = Chunk.ToLocalPosition(x, y, z);
		this.GetChunkInstance(pos).Set(val, pos2);
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x00080E13 File Offset: 0x0007F013
	public I Get(Vector3i pos)
	{
		return this.Get(pos.x, pos.y, pos.z);
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x00080E30 File Offset: 0x0007F030
	public I Get(int x, int y, int z)
	{
		Vector3i pos = Chunk.ToChunkPosition(x, y, z);
		Vector3i pos2 = Chunk.ToLocalPosition(x, y, z);
		Chunk3D<I> chunk = this.GetChunk(pos);
		if (chunk != null)
		{
			return chunk.Get(pos2);
		}
		return this.defaultValue;
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x00080E68 File Offset: 0x0007F068
	public Chunk3D<I> GetChunkInstance(Vector3i pos)
	{
		return this.chunks.GetInstance(pos);
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x00080E76 File Offset: 0x0007F076
	public Chunk3D<I> GetChunkInstance(int x, int y, int z)
	{
		return this.chunks.GetInstance(x, y, z);
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x00080E86 File Offset: 0x0007F086
	public Chunk3D<I> GetChunk(Vector3i pos)
	{
		return this.chunks.SafeGet(pos);
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x00080E94 File Offset: 0x0007F094
	public Chunk3D<I> GetChunk(int x, int y, int z)
	{
		return this.chunks.SafeGet(x, y, z);
	}

	// Token: 0x04000FA5 RID: 4005
	private List3D<Chunk3D<I>> chunks = new List3D<Chunk3D<I>>();

	// Token: 0x04000FA6 RID: 4006
	private I defaultValue;
}
