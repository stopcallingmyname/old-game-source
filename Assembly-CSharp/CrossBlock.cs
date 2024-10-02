using System;
using UnityEngine;

// Token: 0x020000F7 RID: 247
public class CrossBlock : Block
{
	// Token: 0x060008F4 RID: 2292 RVA: 0x0007DA46 File Offset: 0x0007BC46
	public override void Init(BlockSet blockSet)
	{
		base.Init(blockSet);
		this._face = base.ToTexCoords(this.face);
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x0007DA61 File Offset: 0x0007BC61
	public override Rect GetPreviewFace()
	{
		return base.ToRect(this.face);
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x0007DA61 File Offset: 0x0007BC61
	public override Rect GetTopFace()
	{
		return base.ToRect(this.face);
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x0007DA6F File Offset: 0x0007BC6F
	public Vector2[] GetFaceUV()
	{
		return this._face;
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x0007DA77 File Offset: 0x0007BC77
	public override void Build(Vector3i localPos, Vector3i worldPos, Map map, MeshBuilder mesh, bool onlyLight)
	{
		CrossBuilder.Build(localPos, worldPos, map, mesh, onlyLight);
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0007DA85 File Offset: 0x0007BC85
	public override MeshBuilder Build()
	{
		return CrossBuilder.Build(this);
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0007D96F File Offset: 0x0007BB6F
	public override bool IsSolid()
	{
		return false;
	}

	// Token: 0x04000F3E RID: 3902
	[SerializeField]
	private int face;

	// Token: 0x04000F3F RID: 3903
	private Vector2[] _face;
}
