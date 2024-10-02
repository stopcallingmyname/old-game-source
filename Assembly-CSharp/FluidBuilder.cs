using System;
using UnityEngine;

// Token: 0x020000FA RID: 250
public class FluidBuilder
{
	// Token: 0x06000905 RID: 2309 RVA: 0x0007DBD0 File Offset: 0x0007BDD0
	public static void Build(Vector3i localPos, Vector3i worldPos, Map map, MeshBuilder mesh, bool onlyLight)
	{
		FluidBlock fluid = (FluidBlock)map.GetBlock(worldPos).block;
		for (int i = 0; i < 6; i++)
		{
			CubeFace face = CubeBuilder.faces[i];
			Vector3i b = CubeBuilder.directions[i];
			Vector3i nearPos = worldPos + b;
			if (FluidBuilder.IsFaceVisible(map, nearPos, face))
			{
				if (!onlyLight)
				{
					FluidBuilder.BuildFace(face, fluid, localPos, mesh);
				}
				FluidBuilder.BuildFaceLight(face, map, worldPos, mesh);
			}
		}
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x0007DC40 File Offset: 0x0007BE40
	private static bool IsFaceVisible(Map map, Vector3i nearPos, CubeFace face)
	{
		if (face == CubeFace.Top)
		{
			BlockData block = map.GetBlock(nearPos);
			return block.IsEmpty() || !block.IsFluid();
		}
		return map.GetBlock(nearPos).IsEmpty();
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x0007DC80 File Offset: 0x0007BE80
	private static void BuildFace(CubeFace face, FluidBlock fluid, Vector3 localPos, MeshBuilder mesh)
	{
		mesh.AddFaceIndices(fluid.GetAtlasID());
		mesh.AddVertices(CubeBuilder.vertices[(int)face], localPos);
		mesh.AddNormals(CubeBuilder.normals[(int)face]);
		mesh.AddTexCoords(fluid.GetFaceUV());
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x0007DCC4 File Offset: 0x0007BEC4
	private static void BuildFaceLight(CubeFace face, Map map, Vector3i pos, MeshBuilder mesh)
	{
		Vector3i b = CubeBuilder.directions[(int)face];
		Color blockLight = BuildUtils.GetBlockLight(map, pos + b);
		mesh.AddFaceColor(blockLight);
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x0007DCF4 File Offset: 0x0007BEF4
	public static MeshBuilder Build(FluidBlock fluid)
	{
		MeshBuilder meshBuilder = new MeshBuilder();
		for (int i = 0; i < CubeBuilder.vertices.Length; i++)
		{
			meshBuilder.AddFaceIndices(0);
			meshBuilder.AddVertices(CubeBuilder.vertices[i], Vector3.zero);
			meshBuilder.AddNormals(CubeBuilder.normals[i]);
			meshBuilder.AddTexCoords(fluid.GetFaceUV());
			meshBuilder.AddFaceColor(new Color(0f, 0f, 0f, 1f));
		}
		return meshBuilder;
	}
}
