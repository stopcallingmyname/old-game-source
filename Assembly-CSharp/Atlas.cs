using System;
using UnityEngine;

// Token: 0x020000F0 RID: 240
[Serializable]
public class Atlas
{
	// Token: 0x060008B1 RID: 2225 RVA: 0x0007C52C File Offset: 0x0007A72C
	public void SetMaterial(Material material)
	{
		this.material = material;
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x0007C535 File Offset: 0x0007A735
	public Material GetMaterial()
	{
		return this.material;
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0007C53D File Offset: 0x0007A73D
	public void SetWidth(int width)
	{
		this.width = width;
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x0007C546 File Offset: 0x0007A746
	public int GetWidth()
	{
		return this.width;
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x0007C54E File Offset: 0x0007A74E
	public void SetHeight(int height)
	{
		this.height = height;
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x0007C557 File Offset: 0x0007A757
	public int GetHeight()
	{
		return this.height;
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x0007C55F File Offset: 0x0007A75F
	public void SetAlpha(bool alpha)
	{
		this.alpha = alpha;
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x0007C568 File Offset: 0x0007A768
	public bool IsAlpha()
	{
		return this.alpha;
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x0007C570 File Offset: 0x0007A770
	public Texture GetTexture()
	{
		if (this.material)
		{
			return this.material.mainTexture;
		}
		return null;
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x0007C58C File Offset: 0x0007A78C
	public Rect ToRect(int pos)
	{
		float num = (float)(pos % this.width);
		int num2 = pos / this.width;
		float num3 = 1f / (float)this.width;
		float num4 = 1f / (float)this.height;
		return new Rect(num * num3, (float)num2 * num4, num3, num4);
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x0007C5D4 File Offset: 0x0007A7D4
	public override string ToString()
	{
		if (this.material != null)
		{
			return this.material.name;
		}
		return "Null";
	}

	// Token: 0x04000F24 RID: 3876
	[SerializeField]
	private Material material;

	// Token: 0x04000F25 RID: 3877
	[SerializeField]
	private int width = 16;

	// Token: 0x04000F26 RID: 3878
	[SerializeField]
	private int height = 16;

	// Token: 0x04000F27 RID: 3879
	[SerializeField]
	private bool alpha;
}
