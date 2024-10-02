using System;
using UnityEngine;

// Token: 0x020000F4 RID: 244
public class CubeBuilder
{
	// Token: 0x060008DD RID: 2269 RVA: 0x0007D298 File Offset: 0x0007B498
	public static void Build(Vector3i localPos, Vector3i worldPos, Map map, MeshBuilder mesh, bool onlyLight)
	{
		BlockData block = map.GetBlock(worldPos);
		CubeBlock cube = (CubeBlock)block.block;
		BlockDirection direction = block.GetDirection();
		for (int i = 0; i < 6; i++)
		{
			CubeFace face = CubeBuilder.faces[i];
			Vector3i b = CubeBuilder.directions[i];
			Vector3i nearPos = worldPos + b;
			if (CubeBuilder.IsFaceVisible(map, nearPos))
			{
				if (!onlyLight)
				{
					CubeBuilder.BuildFace(face, cube, direction, localPos, mesh);
				}
				CubeBuilder.BuildFaceLight(face, map, worldPos, mesh);
			}
		}
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x0007D318 File Offset: 0x0007B518
	private static bool IsFaceVisible(Map map, Vector3i nearPos)
	{
		if (nearPos.x > 255 || nearPos.x < 0)
		{
			return false;
		}
		if (nearPos.y > 63 || nearPos.y < 0)
		{
			return false;
		}
		if (nearPos.z > 255 || nearPos.z < 0)
		{
			return false;
		}
		Block block = map.GetBlock(nearPos).block;
		return !(block is CubeBlock) || block.IsAlpha();
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x0007D388 File Offset: 0x0007B588
	private static void BuildFace(CubeFace face, CubeBlock cube, BlockDirection direction, Vector3 localPos, MeshBuilder mesh)
	{
		mesh.AddFaceIndices(cube.GetAtlasID());
		mesh.AddVertices(CubeBuilder.vertices[(int)face], localPos);
		mesh.AddNormals(CubeBuilder.normals[(int)face]);
		mesh.AddTexCoords(cube.GetFaceUV(face, direction));
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x0007D3D0 File Offset: 0x0007B5D0
	private static void BuildFaceLight(CubeFace face, Map map, Vector3i pos, MeshBuilder mesh)
	{
		foreach (Vector3 vertex in CubeBuilder.vertices[(int)face])
		{
			Color smoothVertexLight = BuildUtils.GetSmoothVertexLight(map, pos, vertex, face);
			mesh.AddColor(smoothVertexLight);
		}
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x0007D40C File Offset: 0x0007B60C
	public static MeshBuilder Build(CubeBlock cube)
	{
		MeshBuilder meshBuilder = new MeshBuilder();
		for (int i = 0; i < CubeBuilder.vertices.Length; i++)
		{
			meshBuilder.AddFaceIndices(0);
			meshBuilder.AddVertices(CubeBuilder.vertices[i], Vector3.zero);
			meshBuilder.AddNormals(CubeBuilder.normals[i]);
			Vector2[] faceUV = cube.GetFaceUV((CubeFace)i, BlockDirection.Z_PLUS);
			meshBuilder.AddTexCoords(faceUV);
			meshBuilder.AddFaceColor(new Color(0f, 0f, 0f, 1f));
		}
		return meshBuilder;
	}

	// Token: 0x04000F32 RID: 3890
	public static CubeFace[] faces = new CubeFace[]
	{
		CubeFace.Front,
		CubeFace.Back,
		CubeFace.Right,
		CubeFace.Left,
		CubeFace.Top,
		CubeFace.Bottom
	};

	// Token: 0x04000F33 RID: 3891
	public static Vector3i[] directions = new Vector3i[]
	{
		Vector3i.forward,
		Vector3i.back,
		Vector3i.right,
		Vector3i.left,
		Vector3i.up,
		Vector3i.down
	};

	// Token: 0x04000F34 RID: 3892
	public static Vector3[][] vertices = new Vector3[][]
	{
		new Vector3[]
		{
			new Vector3(-0.5f, -0.5f, 0.5f),
			new Vector3(-0.5f, 0.5f, 0.5f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, 0.5f)
		},
		new Vector3[]
		{
			new Vector3(0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, 0.5f, -0.5f),
			new Vector3(-0.5f, 0.5f, -0.5f),
			new Vector3(-0.5f, -0.5f, -0.5f)
		},
		new Vector3[]
		{
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(0.5f, 0.5f, -0.5f),
			new Vector3(0.5f, -0.5f, -0.5f)
		},
		new Vector3[]
		{
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(-0.5f, 0.5f, -0.5f),
			new Vector3(-0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f)
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

	// Token: 0x04000F35 RID: 3893
	public static Vector3[][] normals = new Vector3[][]
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
