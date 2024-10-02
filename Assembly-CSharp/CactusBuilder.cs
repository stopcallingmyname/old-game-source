using System;
using UnityEngine;

// Token: 0x020000F2 RID: 242
public class CactusBuilder
{
	// Token: 0x060008D0 RID: 2256 RVA: 0x0007C798 File Offset: 0x0007A998
	public static void Build(Vector3i localPos, Vector3i worldPos, Map map, MeshBuilder mesh, bool onlyLight)
	{
		CactusBlock cactus = (CactusBlock)map.GetBlock(worldPos).block;
		for (int i = 0; i < 6; i++)
		{
			CubeFace face = CubeBuilder.faces[i];
			Vector3i b = CubeBuilder.directions[i];
			Vector3i nearPos = worldPos + b;
			if (CactusBuilder.IsFaceVisible(map, face, nearPos))
			{
				if (!onlyLight)
				{
					CactusBuilder.BuildFace(face, cactus, localPos, mesh);
				}
				CactusBuilder.BuildFaceLight(face, map, worldPos, mesh);
			}
		}
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x0007C808 File Offset: 0x0007AA08
	private static bool IsFaceVisible(Map map, CubeFace face, Vector3i nearPos)
	{
		if (face == CubeFace.Bottom || face == CubeFace.Top)
		{
			Block block = map.GetBlock(nearPos).block;
			if (block is CubeBlock && !block.IsAlpha())
			{
				return false;
			}
			if (block is CactusBlock)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x0007C848 File Offset: 0x0007AA48
	private static void BuildFace(CubeFace face, CactusBlock cactus, Vector3 localPos, MeshBuilder mesh)
	{
		mesh.AddFaceIndices(cactus.GetAtlasID());
		mesh.AddVertices(CactusBuilder.vertices[(int)face], localPos);
		mesh.AddNormals(CactusBuilder.normals[(int)face]);
		mesh.AddTexCoords(cactus.GetFaceUV(face));
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x0007C88C File Offset: 0x0007AA8C
	private static void BuildFaceLight(CubeFace face, Map map, Vector3i pos, MeshBuilder mesh)
	{
		foreach (Vector3 vertex in CactusBuilder.vertices[(int)face])
		{
			Color smoothVertexLight = BuildUtils.GetSmoothVertexLight(map, pos, vertex, face);
			mesh.AddColor(smoothVertexLight);
		}
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x0007C8C8 File Offset: 0x0007AAC8
	public static MeshBuilder Build(CactusBlock cactus)
	{
		MeshBuilder meshBuilder = new MeshBuilder();
		for (int i = 0; i < CactusBuilder.vertices.Length; i++)
		{
			meshBuilder.AddFaceIndices(0);
			meshBuilder.AddVertices(CactusBuilder.vertices[i], Vector3.zero);
			meshBuilder.AddNormals(CactusBuilder.normals[i]);
			Vector2[] faceUV = cactus.GetFaceUV((CubeFace)i);
			meshBuilder.AddTexCoords(faceUV);
			meshBuilder.AddFaceColor(new Color(0f, 0f, 0f, 1f));
		}
		return meshBuilder;
	}

	// Token: 0x04000F2D RID: 3885
	private static Vector3[][] vertices = new Vector3[][]
	{
		new Vector3[]
		{
			new Vector3(-0.5f, -0.5f, 0.4375f),
			new Vector3(-0.5f, 0.5f, 0.4375f),
			new Vector3(0.5f, 0.5f, 0.4375f),
			new Vector3(0.5f, -0.5f, 0.4375f)
		},
		new Vector3[]
		{
			new Vector3(0.5f, -0.5f, -0.4375f),
			new Vector3(0.5f, 0.5f, -0.4375f),
			new Vector3(-0.5f, 0.5f, -0.4375f),
			new Vector3(-0.5f, -0.5f, -0.4375f)
		},
		new Vector3[]
		{
			new Vector3(0.4375f, -0.5f, 0.5f),
			new Vector3(0.4375f, 0.5f, 0.5f),
			new Vector3(0.4375f, 0.5f, -0.5f),
			new Vector3(0.4375f, -0.5f, -0.5f)
		},
		new Vector3[]
		{
			new Vector3(-0.4375f, -0.5f, -0.5f),
			new Vector3(-0.4375f, 0.5f, -0.5f),
			new Vector3(-0.4375f, 0.5f, 0.5f),
			new Vector3(-0.4375f, -0.5f, 0.5f)
		},
		new Vector3[]
		{
			new Vector3(0.5f, 0.5f, -0.5f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, 0.5f, -0.5f)
		},
		new Vector3[]
		{
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, -0.5f)
		}
	};

	// Token: 0x04000F2E RID: 3886
	private static Vector3[][] normals = new Vector3[][]
	{
		new Vector3[]
		{
			Vector3.forward,
			Vector3.forward,
			Vector3.forward,
			Vector3.forward
		},
		new Vector3[]
		{
			Vector3.back,
			Vector3.back,
			Vector3.back,
			Vector3.back
		},
		new Vector3[]
		{
			Vector3.right,
			Vector3.right,
			Vector3.right,
			Vector3.right
		},
		new Vector3[]
		{
			Vector3.left,
			Vector3.left,
			Vector3.left,
			Vector3.left
		},
		new Vector3[]
		{
			Vector3.up,
			Vector3.up,
			Vector3.up,
			Vector3.up
		},
		new Vector3[]
		{
			Vector3.down,
			Vector3.down,
			Vector3.down,
			Vector3.down
		}
	};
}
