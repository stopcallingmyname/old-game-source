using System;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class FluidBlock : Block
{
	// Token: 0x060008E4 RID: 2276 RVA: 0x0007D928 File Offset: 0x0007BB28
	public override void Init(BlockSet blockSet)
	{
		base.Init(blockSet);
		this.texCoords = base.ToTexCoords(this.face);
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x0007D943 File Offset: 0x0007BB43
	public override Rect GetPreviewFace()
	{
		return base.ToRect(this.face);
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x0007D943 File Offset: 0x0007BB43
	public override Rect GetTopFace()
	{
		return base.ToRect(this.face);
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x0007D951 File Offset: 0x0007BB51
	public Vector2[] GetFaceUV()
	{
		return this.texCoords;
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x0007D959 File Offset: 0x0007BB59
	public override void Build(Vector3i localPos, Vector3i worldPos, Map map, MeshBuilder mesh, bool onlyLight)
	{
		FluidBuilder.Build(localPos, worldPos, map, mesh, onlyLight);
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x0007D967 File Offset: 0x0007BB67
	public override MeshBuilder Build()
	{
		return FluidBuilder.Build(this);
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x0007D96F File Offset: 0x0007BB6F
	public override bool IsSolid()
	{
		return false;
	}

	// Token: 0x04000F36 RID: 3894
	[SerializeField]
	private int face;

	// Token: 0x04000F37 RID: 3895
	private Vector2[] texCoords;
}
