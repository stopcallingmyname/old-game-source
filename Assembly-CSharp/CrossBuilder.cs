using System;
using UnityEngine;

// Token: 0x020000F3 RID: 243
public class CrossBuilder
{
	// Token: 0x060008D7 RID: 2263 RVA: 0x0007CD7B File Offset: 0x0007AF7B
	public static void Build(Vector3i localPos, Vector3i worldPos, Map map, MeshBuilder mesh, bool onlyLight)
	{
		if (!onlyLight)
		{
			CrossBuilder.BuildCross(localPos, worldPos, map, mesh);
		}
		CrossBuilder.BuildCrossLight(map, worldPos, mesh);
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x0007CD98 File Offset: 0x0007AF98
	private static void BuildCross(Vector3 localPos, Vector3i worldPos, Map map, MeshBuilder mesh)
	{
		CrossBlock crossBlock = (CrossBlock)map.GetBlock(worldPos).block;
		mesh.AddIndices(crossBlock.GetAtlasID(), CrossBuilder.indices);
		mesh.AddVertices(CrossBuilder.vertices, localPos);
		mesh.AddNormals(CrossBuilder.normals);
		mesh.AddTexCoords(crossBlock.GetFaceUV());
		mesh.AddTexCoords(crossBlock.GetFaceUV());
		mesh.AddTexCoords(crossBlock.GetFaceUV());
		mesh.AddTexCoords(crossBlock.GetFaceUV());
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x0007CE10 File Offset: 0x0007B010
	private static void BuildCrossLight(Map map, Vector3i pos, MeshBuilder mesh)
	{
		Color blockLight = BuildUtils.GetBlockLight(map, pos);
		mesh.AddColors(blockLight, CrossBuilder.vertices.Length);
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x0007CE34 File Offset: 0x0007B034
	public static MeshBuilder Build(CrossBlock cross)
	{
		MeshBuilder meshBuilder = new MeshBuilder();
		meshBuilder.AddIndices(0, CrossBuilder.indices);
		meshBuilder.AddVertices(CrossBuilder.vertices, Vector3.zero);
		meshBuilder.AddNormals(CrossBuilder.normals);
		meshBuilder.AddTexCoords(cross.GetFaceUV());
		meshBuilder.AddTexCoords(cross.GetFaceUV());
		meshBuilder.AddTexCoords(cross.GetFaceUV());
		meshBuilder.AddTexCoords(cross.GetFaceUV());
		meshBuilder.AddColors(new Color(0f, 0f, 0f, 1f), CrossBuilder.vertices.Length);
		return meshBuilder;
	}

	// Token: 0x04000F2F RID: 3887
	private static Vector3[] vertices = new Vector3[]
	{
		new Vector3(-0.5f, -0.5f, -0.5f),
		new Vector3(-0.5f, 0.5f, -0.5f),
		new Vector3(0.5f, 0.5f, 0.5f),
		new Vector3(0.5f, -0.5f, 0.5f),
		new Vector3(-0.5f, -0.5f, -0.5f),
		new Vector3(-0.5f, 0.5f, -0.5f),
		new Vector3(0.5f, 0.5f, 0.5f),
		new Vector3(0.5f, -0.5f, 0.5f),
		new Vector3(-0.5f, -0.5f, 0.5f),
		new Vector3(-0.5f, 0.5f, 0.5f),
		new Vector3(0.5f, 0.5f, -0.5f),
		new Vector3(0.5f, -0.5f, -0.5f),
		new Vector3(-0.5f, -0.5f, 0.5f),
		new Vector3(-0.5f, 0.5f, 0.5f),
		new Vector3(0.5f, 0.5f, -0.5f),
		new Vector3(0.5f, -0.5f, -0.5f)
	};

	// Token: 0x04000F30 RID: 3888
	private static Vector3[] normals = new Vector3[]
	{
		new Vector3(-0.7f, 0f, 0.7f),
		new Vector3(-0.7f, 0f, 0.7f),
		new Vector3(-0.7f, 0f, 0.7f),
		new Vector3(-0.7f, 0f, 0.7f),
		-new Vector3(-0.7f, 0f, 0.7f),
		-new Vector3(-0.7f, 0f, 0.7f),
		-new Vector3(-0.7f, 0f, 0.7f),
		-new Vector3(-0.7f, 0f, 0.7f),
		new Vector3(0.7f, 0f, 0.7f),
		new Vector3(0.7f, 0f, 0.7f),
		new Vector3(0.7f, 0f, 0.7f),
		new Vector3(0.7f, 0f, 0.7f),
		-new Vector3(0.7f, 0f, 0.7f),
		-new Vector3(0.7f, 0f, 0.7f),
		-new Vector3(0.7f, 0f, 0.7f),
		-new Vector3(0.7f, 0f, 0.7f)
	};

	// Token: 0x04000F31 RID: 3889
	private static int[] indices = new int[]
	{
		2,
		1,
		0,
		3,
		2,
		0,
		4,
		6,
		7,
		4,
		5,
		6,
		10,
		9,
		8,
		11,
		10,
		8,
		12,
		14,
		15,
		12,
		13,
		14
	};
}
