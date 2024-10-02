using System;

// Token: 0x02000107 RID: 263
public class LightMap
{
	// Token: 0x0600097A RID: 2426 RVA: 0x0007F8A6 File Offset: 0x0007DAA6
	public bool SetMaxLight(byte light, Vector3i pos)
	{
		return this.SetMaxLight(light, pos.x, pos.y, pos.z);
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x0007F8C4 File Offset: 0x0007DAC4
	public bool SetMaxLight(byte light, int x, int y, int z)
	{
		Vector3i pos = Chunk.ToChunkPosition(x, y, z);
		Vector3i pos2 = Chunk.ToLocalPosition(x, y, z);
		Chunk3D<byte> chunkInstance = this.lights.GetChunkInstance(pos);
		if (chunkInstance.Get(pos2) < light)
		{
			chunkInstance.Set(light, pos2);
			return true;
		}
		return false;
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x0007F907 File Offset: 0x0007DB07
	public void SetLight(byte light, Vector3i pos)
	{
		this.SetLight(light, pos.x, pos.y, pos.z);
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x0007F922 File Offset: 0x0007DB22
	public void SetLight(byte light, int x, int y, int z)
	{
		if (light < 5)
		{
			light = 5;
		}
		this.lights.Set(light, x, y, z);
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x0007F93B File Offset: 0x0007DB3B
	public byte GetLight(Vector3i pos)
	{
		return this.GetLight(pos.x, pos.y, pos.z);
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x0007F958 File Offset: 0x0007DB58
	public byte GetLight(int x, int y, int z)
	{
		byte b = this.lights.Get(x, y, z);
		if (b < 5)
		{
			return 5;
		}
		return b;
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x0007F97C File Offset: 0x0007DB7C
	public byte GetLight(Vector3i chunkPos, Vector3i localPos)
	{
		byte b = this.lights.GetChunkInstance(chunkPos).Get(localPos);
		if (b < 5)
		{
			return 5;
		}
		return b;
	}

	// Token: 0x04000F80 RID: 3968
	private Map3D<byte> lights = new Map3D<byte>();
}
