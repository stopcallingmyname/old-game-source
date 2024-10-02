using System;
using UnityEngine;

// Token: 0x02000114 RID: 276
public struct Vector2i
{
	// Token: 0x060009F2 RID: 2546 RVA: 0x00080F11 File Offset: 0x0007F111
	public Vector2i(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x00080F21 File Offset: 0x0007F121
	public override int GetHashCode()
	{
		return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x00080F3C File Offset: 0x0007F13C
	public override bool Equals(object other)
	{
		if (!(other is Vector2i))
		{
			return false;
		}
		Vector2i vector2i = (Vector2i)other;
		return this.x == vector2i.x && this.y == vector2i.y;
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x00080F78 File Offset: 0x0007F178
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"Vector2i(",
			this.x,
			" ",
			this.y,
			")"
		});
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x00080FC4 File Offset: 0x0007F1C4
	public static Vector2i Min(Vector2i a, Vector2i b)
	{
		return new Vector2i(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y));
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x00080FED File Offset: 0x0007F1ED
	public static Vector2i Max(Vector2i a, Vector2i b)
	{
		return new Vector2i(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x00081016 File Offset: 0x0007F216
	public static bool operator ==(Vector2i a, Vector2i b)
	{
		return a.x == b.x && a.y == b.y;
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x00081036 File Offset: 0x0007F236
	public static bool operator !=(Vector2i a, Vector2i b)
	{
		return a.x != b.x || a.y != b.y;
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x00081059 File Offset: 0x0007F259
	public static Vector2i operator -(Vector2i a, Vector2i b)
	{
		return new Vector2i(a.x - b.x, a.y - b.y);
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x0008107A File Offset: 0x0007F27A
	public static Vector2i operator +(Vector2i a, Vector2i b)
	{
		return new Vector2i(a.x + b.x, a.y + b.y);
	}

	// Token: 0x04000FA8 RID: 4008
	public int x;

	// Token: 0x04000FA9 RID: 4009
	public int y;

	// Token: 0x04000FAA RID: 4010
	public static readonly Vector2i zero = new Vector2i(0, 0);

	// Token: 0x04000FAB RID: 4011
	public static readonly Vector2i one = new Vector2i(1, 1);

	// Token: 0x04000FAC RID: 4012
	public static readonly Vector2i up = new Vector2i(0, 1);

	// Token: 0x04000FAD RID: 4013
	public static readonly Vector2i down = new Vector2i(0, -1);

	// Token: 0x04000FAE RID: 4014
	public static readonly Vector2i left = new Vector2i(-1, 0);

	// Token: 0x04000FAF RID: 4015
	public static readonly Vector2i right = new Vector2i(1, 0);
}
