using System;
using UnityEngine;

// Token: 0x02000115 RID: 277
public struct Vector3i
{
	// Token: 0x060009FD RID: 2557 RVA: 0x000810F1 File Offset: 0x0007F2F1
	public Vector3i(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x00081108 File Offset: 0x0007F308
	public Vector3i(int x, int y)
	{
		this.x = x;
		this.y = y;
		this.z = 0;
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x00081120 File Offset: 0x0007F320
	public static int DistanceSquared(Vector3i a, Vector3i b)
	{
		int num = b.x - a.x;
		int num2 = b.y - a.y;
		int num3 = b.z - a.z;
		return num * num + num2 * num2 + num3 * num3;
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x00081160 File Offset: 0x0007F360
	public int DistanceSquared(Vector3i v)
	{
		return Vector3i.DistanceSquared(this, v);
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x0008116E File Offset: 0x0007F36E
	public override int GetHashCode()
	{
		return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2;
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x00081198 File Offset: 0x0007F398
	public override bool Equals(object other)
	{
		if (!(other is Vector3i))
		{
			return false;
		}
		Vector3i vector3i = (Vector3i)other;
		return this.x == vector3i.x && this.y == vector3i.y && this.z == vector3i.z;
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x000811E4 File Offset: 0x0007F3E4
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"Vector3i(",
			this.x,
			" ",
			this.y,
			" ",
			this.z,
			")"
		});
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x00081246 File Offset: 0x0007F446
	public static Vector3i Min(Vector3i a, Vector3i b)
	{
		return new Vector3i(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Min(a.z, b.z));
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x00081280 File Offset: 0x0007F480
	public static Vector3i Max(Vector3i a, Vector3i b)
	{
		return new Vector3i(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x000812BA File Offset: 0x0007F4BA
	public static bool operator ==(Vector3i a, Vector3i b)
	{
		return a.x == b.x && a.y == b.y && a.z == b.z;
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x000812E8 File Offset: 0x0007F4E8
	public static bool operator !=(Vector3i a, Vector3i b)
	{
		return a.x != b.x || a.y != b.y || a.z != b.z;
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x00081319 File Offset: 0x0007F519
	public static Vector3i operator -(Vector3i a, Vector3i b)
	{
		return new Vector3i(a.x - b.x, a.y - b.y, a.z - b.z);
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x00081347 File Offset: 0x0007F547
	public static Vector3i operator +(Vector3i a, Vector3i b)
	{
		return new Vector3i(a.x + b.x, a.y + b.y, a.z + b.z);
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x00081375 File Offset: 0x0007F575
	public static implicit operator Vector3(Vector3i v)
	{
		return new Vector3((float)v.x, (float)v.y, (float)v.z);
	}

	// Token: 0x04000FB0 RID: 4016
	public int x;

	// Token: 0x04000FB1 RID: 4017
	public int y;

	// Token: 0x04000FB2 RID: 4018
	public int z;

	// Token: 0x04000FB3 RID: 4019
	public static readonly Vector3i zero = new Vector3i(0, 0, 0);

	// Token: 0x04000FB4 RID: 4020
	public static readonly Vector3i one = new Vector3i(1, 1, 1);

	// Token: 0x04000FB5 RID: 4021
	public static readonly Vector3i forward = new Vector3i(0, 0, 1);

	// Token: 0x04000FB6 RID: 4022
	public static readonly Vector3i back = new Vector3i(0, 0, -1);

	// Token: 0x04000FB7 RID: 4023
	public static readonly Vector3i up = new Vector3i(0, 1, 0);

	// Token: 0x04000FB8 RID: 4024
	public static readonly Vector3i down = new Vector3i(0, -1, 0);

	// Token: 0x04000FB9 RID: 4025
	public static readonly Vector3i left = new Vector3i(-1, 0, 0);

	// Token: 0x04000FBA RID: 4026
	public static readonly Vector3i right = new Vector3i(1, 0, 0);

	// Token: 0x04000FBB RID: 4027
	public static readonly Vector3i[] directions = new Vector3i[]
	{
		Vector3i.left,
		Vector3i.right,
		Vector3i.back,
		Vector3i.forward,
		Vector3i.down,
		Vector3i.up
	};
}
