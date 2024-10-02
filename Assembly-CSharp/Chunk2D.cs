using System;

// Token: 0x02000111 RID: 273
public class Chunk2D<I>
{
	// Token: 0x060009E0 RID: 2528 RVA: 0x00080D5C File Offset: 0x0007EF5C
	public void Set(I val, int x, int z)
	{
		this.chunk[z, x] = val;
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x00080D6C File Offset: 0x0007EF6C
	public I Get(int x, int z)
	{
		return this.chunk[z, x];
	}

	// Token: 0x04000FA4 RID: 4004
	private I[,] chunk = new I[8, 8];
}
