using System;
using UnityEngine;

// Token: 0x02000103 RID: 259
public class ChunkRenderer : MonoBehaviour
{
	// Token: 0x0600095A RID: 2394 RVA: 0x0007EF54 File Offset: 0x0007D154
	public static ChunkRenderer CreateChunkRenderer(Vector3i pos, Map map, Chunk chunk)
	{
		GameObject gameObject = new GameObject(string.Concat(new object[]
		{
			"(",
			pos.x,
			" ",
			pos.y,
			" ",
			pos.z,
			")"
		}), new Type[]
		{
			typeof(MeshFilter),
			typeof(MeshRenderer),
			typeof(ChunkRenderer)
		});
		gameObject.transform.parent = map.transform;
		gameObject.transform.localPosition = new Vector3((float)(pos.x * 8), (float)(pos.y * 8), (float)(pos.z * 8));
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		ChunkRenderer component = gameObject.GetComponent<ChunkRenderer>();
		component.blockSet = map.GetBlockSet();
		component.chunk = chunk;
		component.x = pos.x;
		component.y = pos.y;
		component.z = pos.z;
		component.map = map;
		gameObject.isStatic = true;
		return component;
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x0007F090 File Offset: 0x0007D290
	private void Awake()
	{
		this.filter = base.GetComponent<MeshFilter>();
		this.coll = base.gameObject.AddComponent<MeshCollider>();
		base.GetComponent<Collider>().enabled = true;
		this.LocalPlayer = GameObject.Find("Player");
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x0007F0CC File Offset: 0x0007D2CC
	private void FixedUpdate()
	{
		if (this.colliderDirty)
		{
			this.BuildCollider();
			this.colliderDirty = false;
		}
		if (this.dirty)
		{
			this.colliderDirty = this.BuildMesh();
			this.dirty = (this.lightDirty = false);
		}
		if (this.lightDirty)
		{
			this.BuildLighting();
			this.lightDirty = false;
		}
		if (this.rebuild)
		{
			this.colliderDirty = this.BuildMesh();
			this.rebuild = false;
			this.dirty = (this.lightDirty = false);
		}
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0007F154 File Offset: 0x0007D354
	private void NearLightUpdate(int cx, int cy, int cz)
	{
		Chunk chunk = this.map.GetChunk(new Vector3i(this.x, this.y, this.z));
		if (chunk == null)
		{
			return;
		}
		ChunkRenderer chunkRenderer = chunk.GetChunkRenderer();
		if (chunkRenderer == null)
		{
			return;
		}
		chunkRenderer.SetRebuild();
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x0007F1A3 File Offset: 0x0007D3A3
	public void FastBuild()
	{
		if (this.BuildMesh())
		{
			this.BuildCollider();
		}
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x0007F1B4 File Offset: 0x0007D3B4
	private bool BuildMesh()
	{
		bool result = ChunkBuilder.BuildChunkPro(this.filter, this.coll, this.chunk);
		if (this.filter.sharedMesh == null)
		{
			Object.Destroy(base.gameObject);
			return false;
		}
		base.GetComponent<Renderer>().sharedMaterials = this.blockSet.GetMaterials(this.filter.sharedMesh.subMeshCount);
		return result;
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x0007F220 File Offset: 0x0007D420
	private void BuildCollider()
	{
		ChunkBuilder.BuildChunkCollider(this.filter, this.coll);
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x0007F234 File Offset: 0x0007D434
	public void custom_build()
	{
		if (this.dirty)
		{
			this.in_build = true;
			this.cycle = 0;
		}
		if (!this.in_build)
		{
			return;
		}
		if (this.cycle == 0)
		{
			this.Build_start(this.chunk);
		}
		this.Build_continue(this.chunk, false);
		this.cycle++;
		if (this.cycle >= 8)
		{
			this.Build_end(this.filter.sharedMesh);
			this.dirty = (this.lightDirty = false);
			this.cycle = 0;
			this.in_build = false;
		}
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x0007F2C7 File Offset: 0x0007D4C7
	private void Build_start(Chunk chunk)
	{
		this.map = chunk.GetMap();
		this.meshData.Clear();
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x0007F2E0 File Offset: 0x0007D4E0
	private void Build_continue(Chunk chunk, bool onlyLight)
	{
		int num = this.cycle;
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				Block block = chunk.GetBlock(j, i, num).block;
				if (block != null)
				{
					Vector3i vector3i = new Vector3i(j, i, num);
					Vector3i worldPos = Chunk.ToWorldPosition(chunk.GetPosition(), vector3i);
					block.Build(vector3i, worldPos, this.map, this.meshData, onlyLight);
				}
			}
		}
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x0007F350 File Offset: 0x0007D550
	private void Build_end(Mesh mesh)
	{
		this.filter.sharedMesh = this.meshData.ToMesh(mesh);
		if (this.filter.sharedMesh == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		base.GetComponent<Renderer>().sharedMaterials = this.blockSet.GetMaterials(this.filter.sharedMesh.subMeshCount);
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x0007F3B9 File Offset: 0x0007D5B9
	private void BuildLighting()
	{
		if (this.filter.sharedMesh != null)
		{
			ChunkBuilder.BuildChunkLighting(this.coll.sharedMesh, this.chunk);
		}
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x0007F3E4 File Offset: 0x0007D5E4
	public void SetDirty()
	{
		this.dirty = true;
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x0007F3ED File Offset: 0x0007D5ED
	public void SetLightDirty()
	{
		this.lightDirty = true;
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x0007F3F6 File Offset: 0x0007D5F6
	public void SetRebuild()
	{
		this.rebuild = true;
	}

	// Token: 0x04000F6A RID: 3946
	private int x;

	// Token: 0x04000F6B RID: 3947
	private int y;

	// Token: 0x04000F6C RID: 3948
	private int z;

	// Token: 0x04000F6D RID: 3949
	private GameObject LocalPlayer;

	// Token: 0x04000F6E RID: 3950
	private BlockSet blockSet;

	// Token: 0x04000F6F RID: 3951
	private Chunk chunk;

	// Token: 0x04000F70 RID: 3952
	private Map map;

	// Token: 0x04000F71 RID: 3953
	private bool dirty;

	// Token: 0x04000F72 RID: 3954
	private bool lightDirty;

	// Token: 0x04000F73 RID: 3955
	private bool colliderDirty;

	// Token: 0x04000F74 RID: 3956
	private bool rebuild;

	// Token: 0x04000F75 RID: 3957
	private MeshFilter filter;

	// Token: 0x04000F76 RID: 3958
	private MeshCollider coll;

	// Token: 0x04000F77 RID: 3959
	private bool dirtylock;

	// Token: 0x04000F78 RID: 3960
	private int cycle;

	// Token: 0x04000F79 RID: 3961
	private MeshBuilder meshData = new MeshBuilder();

	// Token: 0x04000F7A RID: 3962
	private bool in_build;
}
