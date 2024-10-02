using System;

// Token: 0x02000113 RID: 275
public class Chunk3D<I>
{
	// Token: 0x060009ED RID: 2541 RVA: 0x00080EA4 File Offset: 0x0007F0A4
	public void Set(I val, Vector3i pos)
	{
		this.Set(val, pos.x, pos.y, pos.z);
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x00080EBF File Offset: 0x0007F0BF
	public void Set(I val, int x, int y, int z)
	{
		this.chunk[z, y, x] = val;
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x00080ED1 File Offset: 0x0007F0D1
	public I Get(Vector3i pos)
	{
		return this.Get(pos.x, pos.y, pos.z);
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x00080EEB File Offset: 0x0007F0EB
	public I Get(int x, int y, int z)
	{
		return this.chunk[z, y, x];
	}

	// Token: 0x04000FA7 RID: 4007
	private I[,,] chunk = new I[8, 8, 8];
}
