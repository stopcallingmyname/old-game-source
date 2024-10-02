using System;

// Token: 0x0200010E RID: 270
public class List2D<T>
{
	// Token: 0x060009B4 RID: 2484 RVA: 0x000805F1 File Offset: 0x0007E7F1
	public List2D()
	{
		this.list = new T[0, 0];
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x00080606 File Offset: 0x0007E806
	public void Set(T obj, Vector2i pos)
	{
		this.Set(obj, pos.x, pos.y);
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x0008061B File Offset: 0x0007E81B
	public void Set(T obj, int x, int y)
	{
		this.list[y - this.min.y, x - this.min.x] = obj;
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x00080643 File Offset: 0x0007E843
	public T GetInstance(Vector2i pos)
	{
		return this.GetInstance(pos.x, pos.y);
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x00080658 File Offset: 0x0007E858
	public T GetInstance(int x, int y)
	{
		T t = this.SafeGet(x, y);
		if (object.Equals(t, default(T)))
		{
			t = Activator.CreateInstance<T>();
			this.AddOrReplace(t, x, y);
		}
		return t;
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x00080699 File Offset: 0x0007E899
	public T Get(Vector2i pos)
	{
		return this.Get(pos.x, pos.y);
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x000806AD File Offset: 0x0007E8AD
	public T Get(int x, int y)
	{
		return this.list[y - this.min.y, x - this.min.x];
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x000806D4 File Offset: 0x0007E8D4
	public T SafeGet(Vector2i pos)
	{
		return this.SafeGet(pos.x, pos.y);
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x000806E8 File Offset: 0x0007E8E8
	public T SafeGet(int x, int y)
	{
		if (!this.IsCorrectIndex(x, y))
		{
			return default(T);
		}
		return this.list[y - this.min.y, x - this.min.x];
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x00080730 File Offset: 0x0007E930
	public void AddOrReplace(T obj, Vector2i pos)
	{
		Vector2i vector2i = Vector2i.Min(this.min, pos);
		Vector2i vector2i2 = Vector2i.Max(this.max, pos + Vector2i.one);
		if (vector2i != this.min || vector2i2 != this.max)
		{
			this.Resize(vector2i, vector2i2);
		}
		this.Set(obj, pos);
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x0008078D File Offset: 0x0007E98D
	public void AddOrReplace(T obj, int x, int y)
	{
		this.AddOrReplace(obj, new Vector2i(x, y));
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x000807A0 File Offset: 0x0007E9A0
	private void Resize(Vector2i newMin, Vector2i newMax)
	{
		Vector2i vector2i = this.min;
		Vector2i vector2i2 = this.max;
		T[,] array = this.list;
		this.min = newMin;
		this.max = newMax;
		Vector2i vector2i3 = newMax - newMin;
		this.list = new T[vector2i3.y, vector2i3.x];
		for (int i = vector2i.x; i < vector2i2.x; i++)
		{
			for (int j = vector2i.y; j < vector2i2.y; j++)
			{
				T obj = array[j - vector2i.y, i - vector2i.x];
				this.Set(obj, i, j);
			}
		}
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x00080849 File Offset: 0x0007EA49
	public bool IsCorrectIndex(Vector2i pos)
	{
		return this.IsCorrectIndex(pos.x, pos.y);
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x0008085D File Offset: 0x0007EA5D
	private bool IsCorrectIndex(int x, int y)
	{
		return x >= this.min.x && y >= this.min.y && x < this.max.x && y < this.max.y;
	}

	// Token: 0x04000F9C RID: 3996
	private T[,] list;

	// Token: 0x04000F9D RID: 3997
	private Vector2i min;

	// Token: 0x04000F9E RID: 3998
	private Vector2i max;
}
