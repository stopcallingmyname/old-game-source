using System;

// Token: 0x0200010F RID: 271
public class List3D<T>
{
	// Token: 0x060009C2 RID: 2498 RVA: 0x0008089C File Offset: 0x0007EA9C
	public List3D()
	{
		this.list = new T[0, 0, 0];
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x000808B4 File Offset: 0x0007EAB4
	public List3D(Vector3i min, Vector3i max)
	{
		this.min = min;
		this.max = max;
		Vector3i size = this.GetSize();
		this.list = new T[size.z, size.y, size.x];
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x000808F9 File Offset: 0x0007EAF9
	public void Set(T obj, Vector3i pos)
	{
		this.Set(obj, pos.x, pos.y, pos.z);
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x00080914 File Offset: 0x0007EB14
	public void Set(T obj, int x, int y, int z)
	{
		this.list[z - this.min.z, y - this.min.y, x - this.min.x] = obj;
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x0008094A File Offset: 0x0007EB4A
	public T GetInstance(Vector3i pos)
	{
		return this.GetInstance(pos.x, pos.y, pos.z);
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x00080964 File Offset: 0x0007EB64
	public T GetInstance(int x, int y, int z)
	{
		T t = this.SafeGet(x, y, z);
		if (object.Equals(t, default(T)))
		{
			t = Activator.CreateInstance<T>();
			this.AddOrReplace(t, x, y, z);
		}
		return t;
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x000809A7 File Offset: 0x0007EBA7
	public T Get(Vector3i pos)
	{
		return this.Get(pos.x, pos.y, pos.z);
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x000809C1 File Offset: 0x0007EBC1
	public T Get(int x, int y, int z)
	{
		return this.list[z - this.min.z, y - this.min.y, x - this.min.x];
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x000809F5 File Offset: 0x0007EBF5
	public T SafeGet(Vector3i pos)
	{
		return this.SafeGet(pos.x, pos.y, pos.z);
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x00080A10 File Offset: 0x0007EC10
	public T SafeGet(int x, int y, int z)
	{
		if (!this.IsCorrectIndex(x, y, z))
		{
			return default(T);
		}
		return this.Get(x, y, z);
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x00080A3C File Offset: 0x0007EC3C
	public void AddOrReplace(T obj, Vector3i pos)
	{
		Vector3i vector3i = Vector3i.Min(this.min, pos);
		Vector3i vector3i2 = Vector3i.Max(this.max, pos + Vector3i.one);
		if (vector3i != this.min || vector3i2 != this.max)
		{
			this.Resize(vector3i, vector3i2);
		}
		this.Set(obj, pos);
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x00080A99 File Offset: 0x0007EC99
	public void AddOrReplace(T obj, int x, int y, int z)
	{
		this.AddOrReplace(obj, new Vector3i(x, y, z));
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x00080AAC File Offset: 0x0007ECAC
	private void Resize(Vector3i newMin, Vector3i newMax)
	{
		Vector3i vector3i = this.min;
		Vector3i vector3i2 = this.max;
		T[,,] array = this.list;
		this.min = newMin;
		this.max = newMax;
		Vector3i vector3i3 = newMax - newMin;
		this.list = new T[vector3i3.z, vector3i3.y, vector3i3.x];
		for (int i = vector3i.x; i < vector3i2.x; i++)
		{
			for (int j = vector3i.y; j < vector3i2.y; j++)
			{
				for (int k = vector3i.z; k < vector3i2.z; k++)
				{
					T obj = array[k - vector3i.z, j - vector3i.y, i - vector3i.x];
					this.Set(obj, i, j, k);
				}
			}
		}
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x00080B80 File Offset: 0x0007ED80
	public bool IsCorrectIndex(Vector3i pos)
	{
		return this.IsCorrectIndex(pos.x, pos.y, pos.z);
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x00080B9C File Offset: 0x0007ED9C
	public bool IsCorrectIndex(int x, int y, int z)
	{
		return x >= this.min.x && y >= this.min.y && z >= this.min.z && x < this.max.x && y < this.max.y && z < this.max.z;
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x00080C02 File Offset: 0x0007EE02
	public Vector3i GetMin()
	{
		return this.min;
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x00080C0A File Offset: 0x0007EE0A
	public Vector3i GetMax()
	{
		return this.max;
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x00080C12 File Offset: 0x0007EE12
	public Vector3i GetSize()
	{
		return this.max - this.min;
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x00080C25 File Offset: 0x0007EE25
	public int GetMinX()
	{
		return this.min.x;
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x00080C32 File Offset: 0x0007EE32
	public int GetMinY()
	{
		return this.min.y;
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x00080C3F File Offset: 0x0007EE3F
	public int GetMinZ()
	{
		return this.min.z;
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x00080C4C File Offset: 0x0007EE4C
	public int GetMaxX()
	{
		return this.max.x;
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x00080C59 File Offset: 0x0007EE59
	public int GetMaxY()
	{
		return this.max.y;
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x00080C66 File Offset: 0x0007EE66
	public int GetMaxZ()
	{
		return this.max.z;
	}

	// Token: 0x04000F9F RID: 3999
	private T[,,] list;

	// Token: 0x04000FA0 RID: 4000
	private Vector3i min;

	// Token: 0x04000FA1 RID: 4001
	private Vector3i max;
}
