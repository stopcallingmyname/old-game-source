using System;

// Token: 0x02000110 RID: 272
public class Map2D<I>
{
	// Token: 0x060009DA RID: 2522 RVA: 0x00080C73 File Offset: 0x0007EE73
	public Map2D()
	{
		this.defaultValue = default(I);
	}

	// Token: 0x060009DB RID: 2523 RVA: 0x00080C92 File Offset: 0x0007EE92
	public Map2D(I defaultValue)
	{
		this.defaultValue = defaultValue;
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x00080CAC File Offset: 0x0007EEAC
	public void Set(I val, int x, int z)
	{
		Vector3i vector3i = Chunk.ToChunkPosition(x, 0, z);
		Vector3i vector3i2 = Chunk.ToLocalPosition(x, 0, z);
		this.GetChunkInstance(vector3i.x, vector3i.z).Set(val, vector3i2.x, vector3i2.z);
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x00080CF0 File Offset: 0x0007EEF0
	public I Get(int x, int z)
	{
		Vector3i vector3i = Chunk.ToChunkPosition(x, 0, z);
		Vector3i vector3i2 = Chunk.ToLocalPosition(x, 0, z);
		Chunk2D<I> chunk = this.GetChunk(vector3i.x, vector3i.z);
		if (chunk != null)
		{
			return chunk.Get(vector3i2.x, vector3i2.z);
		}
		return this.defaultValue;
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x00080D3E File Offset: 0x0007EF3E
	public Chunk2D<I> GetChunkInstance(int x, int z)
	{
		return this.chunks.GetInstance(x, z);
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x00080D4D File Offset: 0x0007EF4D
	public Chunk2D<I> GetChunk(int x, int z)
	{
		return this.chunks.SafeGet(x, z);
	}

	// Token: 0x04000FA2 RID: 4002
	private List2D<Chunk2D<I>> chunks = new List2D<Chunk2D<I>>();

	// Token: 0x04000FA3 RID: 4003
	private I defaultValue;
}
