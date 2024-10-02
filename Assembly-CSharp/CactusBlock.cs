using System;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public class CactusBlock : Block
{
	// Token: 0x060008EC RID: 2284 RVA: 0x0007D97A File Offset: 0x0007BB7A
	public override void Init(BlockSet blockSet)
	{
		base.Init(blockSet);
		this._side = base.ToTexCoords(this.side);
		this._top = base.ToTexCoords(this.top);
		this._bottom = base.ToTexCoords(this.bottom);
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x0007D9B9 File Offset: 0x0007BBB9
	public override Rect GetPreviewFace()
	{
		return base.ToRect(this.side);
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x0007D9C7 File Offset: 0x0007BBC7
	public override Rect GetTopFace()
	{
		return base.ToRect(this.top);
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x0007D9D8 File Offset: 0x0007BBD8
	public Vector2[] GetFaceUV(CubeFace face)
	{
		switch (face)
		{
		case CubeFace.Front:
			return this._side;
		case CubeFace.Back:
			return this._side;
		case CubeFace.Right:
			return this._side;
		case CubeFace.Left:
			return this._side;
		case CubeFace.Top:
			return this._top;
		case CubeFace.Bottom:
			return this._bottom;
		default:
			return null;
		}
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x0007DA30 File Offset: 0x0007BC30
	public override void Build(Vector3i localPos, Vector3i worldPos, Map map, MeshBuilder mesh, bool onlyLight)
	{
		CactusBuilder.Build(localPos, worldPos, map, mesh, onlyLight);
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x0007DA3E File Offset: 0x0007BC3E
	public override MeshBuilder Build()
	{
		return CactusBuilder.Build(this);
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x0006AE98 File Offset: 0x00069098
	public override bool IsSolid()
	{
		return true;
	}

	// Token: 0x04000F38 RID: 3896
	[SerializeField]
	private int side;

	// Token: 0x04000F39 RID: 3897
	[SerializeField]
	private int top;

	// Token: 0x04000F3A RID: 3898
	[SerializeField]
	private int bottom;

	// Token: 0x04000F3B RID: 3899
	private Vector2[] _side;

	// Token: 0x04000F3C RID: 3900
	private Vector2[] _top;

	// Token: 0x04000F3D RID: 3901
	private Vector2[] _bottom;
}
