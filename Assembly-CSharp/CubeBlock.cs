using System;
using UnityEngine;

// Token: 0x020000F9 RID: 249
public class CubeBlock : Block
{
	// Token: 0x060008FC RID: 2300 RVA: 0x0007DA90 File Offset: 0x0007BC90
	public override void Init(BlockSet blockSet)
	{
		base.Init(blockSet);
		this.texCoords = new Vector2[][]
		{
			base.ToTexCoords(this.front),
			base.ToTexCoords(this.back),
			base.ToTexCoords(this.right),
			base.ToTexCoords(this.left),
			base.ToTexCoords(this.top),
			base.ToTexCoords(this.bottom)
		};
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x0007DB0A File Offset: 0x0007BD0A
	public override Rect GetPreviewFace()
	{
		return base.ToRect(this.front);
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0007DB18 File Offset: 0x0007BD18
	public override Rect GetTopFace()
	{
		return base.ToRect(this.top);
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0007DB26 File Offset: 0x0007BD26
	public Vector2[] GetFaceUV(CubeFace face, BlockDirection dir)
	{
		face = CubeBlock.TransformFace(face, dir);
		return this.texCoords[(int)face];
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0007DB3C File Offset: 0x0007BD3C
	public static CubeFace TransformFace(CubeFace face, BlockDirection dir)
	{
		if (face == CubeFace.Top || face == CubeFace.Bottom)
		{
			return face;
		}
		int num = 0;
		if (face == CubeFace.Right)
		{
			num = 90;
		}
		if (face == CubeFace.Back)
		{
			num = 180;
		}
		if (face == CubeFace.Left)
		{
			num = 270;
		}
		if (dir == BlockDirection.X_MINUS)
		{
			num += 90;
		}
		if (dir == BlockDirection.Z_MINUS)
		{
			num += 180;
		}
		if (dir == BlockDirection.X_PLUS)
		{
			num += 270;
		}
		num %= 360;
		if (num == 0)
		{
			return CubeFace.Front;
		}
		if (num == 90)
		{
			return CubeFace.Right;
		}
		if (num == 180)
		{
			return CubeFace.Back;
		}
		if (num == 270)
		{
			return CubeFace.Left;
		}
		return CubeFace.Front;
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x0007DBBA File Offset: 0x0007BDBA
	public override void Build(Vector3i localPos, Vector3i worldPos, Map map, MeshBuilder mesh, bool onlyLight)
	{
		CubeBuilder.Build(localPos, worldPos, map, mesh, onlyLight);
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x0007DBC8 File Offset: 0x0007BDC8
	public override MeshBuilder Build()
	{
		return CubeBuilder.Build(this);
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x0006AE98 File Offset: 0x00069098
	public override bool IsSolid()
	{
		return true;
	}

	// Token: 0x04000F47 RID: 3911
	[SerializeField]
	private int front;

	// Token: 0x04000F48 RID: 3912
	[SerializeField]
	private int back;

	// Token: 0x04000F49 RID: 3913
	[SerializeField]
	private int right;

	// Token: 0x04000F4A RID: 3914
	[SerializeField]
	private int left;

	// Token: 0x04000F4B RID: 3915
	[SerializeField]
	private int top;

	// Token: 0x04000F4C RID: 3916
	[SerializeField]
	private int bottom;

	// Token: 0x04000F4D RID: 3917
	private Vector2[][] texCoords;
}
