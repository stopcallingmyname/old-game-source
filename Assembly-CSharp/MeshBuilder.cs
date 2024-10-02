using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000FF RID: 255
public class MeshBuilder
{
	// Token: 0x06000932 RID: 2354 RVA: 0x0007E888 File Offset: 0x0007CA88
	public void AddVertices(Vector3[] vertices, Vector3 offset)
	{
		foreach (Vector3 a in vertices)
		{
			this.vertices.Add(a + offset);
		}
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x0007E8BF File Offset: 0x0007CABF
	public void AddNormals(Vector3[] normals)
	{
		this.normals.AddRange(normals);
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x0007E8D0 File Offset: 0x0007CAD0
	public void AddColor(Color color)
	{
		float num = color.a;
		if (num < 0.1f)
		{
			num = 0.5f;
		}
		this.colors.Add(new Color(num, num, num, 1f));
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x0007E90C File Offset: 0x0007CB0C
	public void AddFaceColor(Color color)
	{
		this.colors.Add(new Color(color.a, color.a, color.a, 1f));
		this.colors.Add(new Color(color.a, color.a, color.a, 1f));
		this.colors.Add(new Color(color.a, color.a, color.a, 1f));
		this.colors.Add(new Color(color.a, color.a, color.a, 1f));
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x0007E9B8 File Offset: 0x0007CBB8
	public void AddColors(Color color, int count)
	{
		for (int i = 0; i < count; i++)
		{
			float a = color.a;
			this.colors.Add(new Color(a, a, a, 1f));
		}
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x0007E9F0 File Offset: 0x0007CBF0
	public void AddTexCoords(Vector2[] uv)
	{
		this.uv.AddRange(uv);
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x0007EA00 File Offset: 0x0007CC00
	public void AddFaceIndices(int materialIndex)
	{
		int count = this.vertices.Count;
		List<int> list = this.GetIndices(materialIndex);
		list.Add(count + 2);
		list.Add(count + 1);
		list.Add(count);
		list.Add(count + 3);
		list.Add(count + 2);
		list.Add(count);
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x0007EA54 File Offset: 0x0007CC54
	public void AddIndices(int materialIndex, int[] indices)
	{
		int count = this.vertices.Count;
		List<int> list = this.GetIndices(materialIndex);
		foreach (int num in indices)
		{
			list.Add(num + count);
		}
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x0007EA94 File Offset: 0x0007CC94
	public List<int> GetIndices(int index)
	{
		if (index >= this.indices.Length)
		{
			int num = this.indices.Length;
			Array.Resize<List<int>>(ref this.indices, index + 1);
			for (int i = num; i < this.indices.Length; i++)
			{
				this.indices[i] = new List<int>();
			}
		}
		return this.indices[index];
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x0007EAE9 File Offset: 0x0007CCE9
	public List<Color> GetColors()
	{
		return this.colors;
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x0007EAF4 File Offset: 0x0007CCF4
	public void Clear()
	{
		this.vertices.Clear();
		this.uv.Clear();
		this.normals.Clear();
		this.colors.Clear();
		List<int>[] array = this.indices;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Clear();
		}
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x0007EB4C File Offset: 0x0007CD4C
	public Mesh ToMesh(Mesh mesh)
	{
		if (this.vertices.Count == 0)
		{
			Object.Destroy(mesh);
			return null;
		}
		if (mesh == null)
		{
			mesh = new Mesh();
		}
		mesh.Clear();
		mesh.vertices = this.vertices.ToArray();
		mesh.colors = this.colors.ToArray();
		mesh.normals = this.normals.ToArray();
		mesh.uv = this.uv.ToArray();
		mesh.subMeshCount = this.indices.Length;
		for (int i = 0; i < this.indices.Length; i++)
		{
			mesh.SetTriangles(this.indices[i].ToArray(), i);
		}
		return mesh;
	}

	// Token: 0x04000F53 RID: 3923
	private List<Vector3> vertices = new List<Vector3>();

	// Token: 0x04000F54 RID: 3924
	private List<Vector2> uv = new List<Vector2>();

	// Token: 0x04000F55 RID: 3925
	private List<Vector3> normals = new List<Vector3>();

	// Token: 0x04000F56 RID: 3926
	private List<Color> colors = new List<Color>();

	// Token: 0x04000F57 RID: 3927
	private List<int>[] indices = new List<int>[0];
}
