using System;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public abstract class Block
{
	// Token: 0x060008BD RID: 2237 RVA: 0x0007C60D File Offset: 0x0007A80D
	public virtual void Init(BlockSet blockSet)
	{
		this._atlas = blockSet.GetAtlas(this.atlas);
		if (this._atlas != null)
		{
			this.alpha = this._atlas.IsAlpha();
		}
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x0007C63C File Offset: 0x0007A83C
	public Rect ToRect(int pos)
	{
		if (this._atlas != null)
		{
			return this._atlas.ToRect(pos);
		}
		return default(Rect);
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x0007C668 File Offset: 0x0007A868
	protected Vector2[] ToTexCoords(int pos)
	{
		Rect rect = this.ToRect(pos);
		return new Vector2[]
		{
			new Vector2(rect.xMax, rect.yMin),
			new Vector2(rect.xMax, rect.yMax),
			new Vector2(rect.xMin, rect.yMax),
			new Vector2(rect.xMin, rect.yMin)
		};
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x0007C6EC File Offset: 0x0007A8EC
	public bool DrawPreview(Rect position)
	{
		Texture texture = this.GetTexture();
		if (texture != null)
		{
			GUI.DrawTextureWithTexCoords(position, texture, this.GetPreviewFace());
		}
		return Event.current.type == EventType.MouseDown && position.Contains(Event.current.mousePosition);
	}

	// Token: 0x060008C1 RID: 2241
	public abstract Rect GetPreviewFace();

	// Token: 0x060008C2 RID: 2242
	public abstract Rect GetTopFace();

	// Token: 0x060008C3 RID: 2243
	public abstract void Build(Vector3i localPos, Vector3i worldPos, Map map, MeshBuilder mesh, bool onlyLight);

	// Token: 0x060008C4 RID: 2244
	public abstract MeshBuilder Build();

	// Token: 0x060008C5 RID: 2245 RVA: 0x0007C735 File Offset: 0x0007A935
	public void SetName(string name)
	{
		this.name = name;
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x0007C73E File Offset: 0x0007A93E
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x0007C746 File Offset: 0x0007A946
	public void SetAtlasID(int atlas)
	{
		this.atlas = atlas;
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x0007C74F File Offset: 0x0007A94F
	public int GetAtlasID()
	{
		return this.atlas;
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x0007C757 File Offset: 0x0007A957
	public Atlas GetAtlas()
	{
		return this._atlas;
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x0007C75F File Offset: 0x0007A95F
	public Texture GetTexture()
	{
		if (this._atlas != null)
		{
			return this._atlas.GetTexture();
		}
		return null;
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x0007C776 File Offset: 0x0007A976
	public void SetLight(int light)
	{
		this.light = Mathf.Clamp(light, 0, 15);
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x0007C787 File Offset: 0x0007A987
	public byte GetLight()
	{
		return (byte)this.light;
	}

	// Token: 0x060008CD RID: 2253
	public abstract bool IsSolid();

	// Token: 0x060008CE RID: 2254 RVA: 0x0007C790 File Offset: 0x0007A990
	public bool IsAlpha()
	{
		return this.alpha;
	}

	// Token: 0x04000F28 RID: 3880
	[SerializeField]
	private string name;

	// Token: 0x04000F29 RID: 3881
	[SerializeField]
	private int atlas;

	// Token: 0x04000F2A RID: 3882
	[SerializeField]
	private int light;

	// Token: 0x04000F2B RID: 3883
	private Atlas _atlas;

	// Token: 0x04000F2C RID: 3884
	private bool alpha;
}
