using System;
using UnityEngine;

// Token: 0x0200010A RID: 266
public class SunLightMap
{
	// Token: 0x06000990 RID: 2448 RVA: 0x0007FF89 File Offset: 0x0007E189
	public void SetSunHeight(int height, int x, int z)
	{
		this.rays.Set((short)height, x, z);
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x0007FF9A File Offset: 0x0007E19A
	public int GetSunHeight(int x, int z)
	{
		return (int)this.rays.Get(x, z);
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x0007FFA9 File Offset: 0x0007E1A9
	public bool IsSunLight(int x, int y, int z)
	{
		return this.GetSunHeight(x, z) <= y;
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x0007FFBC File Offset: 0x0007E1BC
	private bool IsSunLight(Vector3i chunkPos, Vector3i localPos, int worldY)
	{
		Chunk2D<short> chunk = this.rays.GetChunk(chunkPos.x, chunkPos.z);
		return chunk != null && (int)chunk.Get(localPos.x, localPos.z) <= worldY;
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x0007FFFE File Offset: 0x0007E1FE
	public bool SetMaxLight(byte light, Vector3i pos)
	{
		return this.SetMaxLight(light, pos.x, pos.y, pos.z);
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x0008001C File Offset: 0x0007E21C
	public bool SetMaxLight(byte light, int x, int y, int z)
	{
		Vector3i vector3i = Chunk.ToChunkPosition(x, y, z);
		Vector3i vector3i2 = Chunk.ToLocalPosition(x, y, z);
		if (this.IsSunLight(vector3i, vector3i2, y))
		{
			return false;
		}
		Chunk3D<byte> chunkInstance = this.lights.GetChunkInstance(vector3i);
		if (chunkInstance.Get(vector3i2) < light)
		{
			chunkInstance.Set(light, vector3i2);
			return true;
		}
		return false;
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x0008006C File Offset: 0x0007E26C
	public void SetLight(byte light, Vector3i pos)
	{
		this.SetLight(light, pos.x, pos.y, pos.z);
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x00080087 File Offset: 0x0007E287
	public void SetLight(byte light, int x, int y, int z)
	{
		this.lights.Set(light, x, y, z);
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x00080099 File Offset: 0x0007E299
	public void SetLight(byte light, Vector3i chunkPos, Vector3i localPos)
	{
		this.lights.GetChunkInstance(chunkPos).Set(light, localPos);
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x000800AE File Offset: 0x0007E2AE
	public byte GetLight(Vector3i pos)
	{
		return this.GetLight(pos.x, pos.y, pos.z);
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x000800C8 File Offset: 0x0007E2C8
	public byte GetLight(int x, int y, int z)
	{
		Vector3i chunkPos = Chunk.ToChunkPosition(x, y, z);
		Vector3i localPos = Chunk.ToLocalPosition(x, y, z);
		return this.GetLight(chunkPos, localPos, y);
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x000800F0 File Offset: 0x0007E2F0
	public byte GetLight(Vector3i chunkPos, Vector3i localPos, int worldY)
	{
		if (this.IsSunLight(chunkPos, localPos, worldY))
		{
			return 15;
		}
		Chunk3D<byte> chunk = this.lights.GetChunk(chunkPos);
		if (chunk != null)
		{
			byte b = chunk.Get(localPos);
			return (byte)Mathf.Max(5, (int)b);
		}
		return 5;
	}

	// Token: 0x04000F89 RID: 3977
	private Map2D<short> rays = new Map2D<short>();

	// Token: 0x04000F8A RID: 3978
	private Map3D<byte> lights = new Map3D<byte>();
}
