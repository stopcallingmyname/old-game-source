using System;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class MapLoader : MonoBehaviour
{
	// Token: 0x06000A48 RID: 2632 RVA: 0x00082D10 File Offset: 0x00080F10
	private void Awake()
	{
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x00082D20 File Offset: 0x00080F20
	public void SaveMap()
	{
		XMap xmap = new XMap();
		xmap.SetName("devmap_name");
		this.map.GetChunks();
		for (int i = 0; i < 32; i++)
		{
			for (int j = 0; j < 32; j++)
			{
				for (int k = 0; k < 8; k++)
				{
					Chunk chunk = this.map.GetChunk(new Vector3i(i, k, j));
					if (chunk != null)
					{
						XChunk xchunk = new XChunk();
						xchunk.pos = new Vector3i(i, k, j);
						for (int l = 0; l < 8; l++)
						{
							for (int m = 0; m < 8; m++)
							{
								for (int n = 0; n < 8; n++)
								{
									BlockData block = chunk.GetBlock(l, n, m);
									if (!block.IsEmpty() && block.block.GetName() != null)
									{
										XBlock xblock = new XBlock();
										xblock.pos = new Vector3i(l, n, m);
										if (block.block.GetName() == "Stoneend")
										{
											xblock.flag = 1;
										}
										else if (block.block.GetName() == "Dirt")
										{
											xblock.flag = 2;
										}
										else if (block.block.GetName() == "Grass")
										{
											xblock.flag = 3;
										}
										else if (block.block.GetName() == "Snow")
										{
											xblock.flag = 4;
										}
										else if (block.block.GetName() == "Sand")
										{
											xblock.flag = 5;
										}
										else if (block.block.GetName() == "Stone")
										{
											xblock.flag = 6;
										}
										else if (block.block.GetName() == "Water")
										{
											xblock.flag = 7;
										}
										else if (block.block.GetName() == "Wood")
										{
											xblock.flag = 8;
										}
										else if (block.block.GetName() == "Wood2")
										{
											xblock.flag = 9;
										}
										else if (block.block.GetName() == "Leaf")
										{
											xblock.flag = 10;
										}
										else if (block.block.GetName() == "Brick")
										{
											xblock.flag = 11;
										}
										if (k != 0 || n >= 7)
										{
											xchunk.Blocks.Add(xblock);
										}
									}
								}
							}
						}
						xmap.Chunks.Add(xchunk);
					}
				}
			}
		}
		xmap.Save("c:/map.xml");
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x00082FF8 File Offset: 0x000811F8
	public void LoadMap(bool external, string mapname)
	{
		XMap xmap;
		if (external)
		{
			xmap = XMap.Load("c:/map.xml");
		}
		else
		{
			xmap = XMap.InternalLoad(mapname);
		}
		if (xmap == null)
		{
			MonoBehaviour.print("map not found");
			return;
		}
		MonoBehaviour.print("MapName: " + xmap.MapName);
		MonoBehaviour.print("Chunks: " + xmap.Chunks.Count);
		for (int i = 0; i < 32; i++)
		{
			for (int j = 0; j < 32; j++)
			{
				for (int k = 0; k < 8; k++)
				{
					for (int l = 0; l < 8; l++)
					{
						for (int m = 0; m < 7; m++)
						{
							Vector3i pos = Chunk.ToWorldPosition(new Vector3i(i, 0, j), new Vector3i(k, m, l));
							if ((i == 7 && j == 7) || (i == 8 && j == 7) || (i == 8 && j == 8) || (i == 7 && j == 8))
							{
								this.map.SetBlock(this.stoneend, pos);
							}
							else if (m < 2)
							{
								this.map.SetBlock(this.stoneend, pos);
							}
							else if (m < 6)
							{
								this.map.SetBlock(this.dirt, pos);
							}
							else
							{
								this.map.SetBlock(this.grass, pos);
							}
						}
					}
				}
			}
		}
		for (int n = 0; n < xmap.Chunks.Count; n++)
		{
			for (int num = 0; num < xmap.Chunks[n].Blocks.Count; num++)
			{
				if (xmap.Chunks[n].Blocks[num].flag != 0)
				{
					Vector3i vector3i = Chunk.ToWorldPosition(new Vector3i(xmap.Chunks[n].pos.x, xmap.Chunks[n].pos.y, xmap.Chunks[n].pos.z), new Vector3i(xmap.Chunks[n].Blocks[num].pos.x, xmap.Chunks[n].Blocks[num].pos.y, xmap.Chunks[n].Blocks[num].pos.z));
					if (vector3i.y == 0)
					{
						this.map.SetBlock(this.stoneend, vector3i);
					}
					else
					{
						switch (xmap.Chunks[n].Blocks[num].flag)
						{
						case 1:
							this.map.SetBlock(this.stoneend, vector3i);
							break;
						case 2:
							this.map.SetBlock(this.dirt, vector3i);
							break;
						case 3:
							this.map.SetBlock(this.grass, vector3i);
							break;
						case 4:
							this.map.SetBlock(this.snow, vector3i);
							break;
						case 5:
							this.map.SetBlock(this.sand, vector3i);
							break;
						case 6:
							this.map.SetBlock(this.stone, vector3i);
							break;
						case 7:
							this.map.SetBlock(this.water, vector3i);
							break;
						case 8:
							this.map.SetBlock(this.wood, vector3i);
							break;
						case 9:
							this.map.SetBlock(this.wood2, vector3i);
							break;
						case 10:
							this.map.SetBlock(this.leaf, vector3i);
							break;
						case 11:
							this.map.SetBlock(this.brick, vector3i);
							break;
						}
					}
				}
			}
			this.map.GetChunk(new Vector3i(xmap.Chunks[n].pos.x, xmap.Chunks[n].pos.y, xmap.Chunks[n].pos.z)).GetChunkRendererInstance().SetDirty();
			ChunkSunLightComputer.ComputeRays(this.map, xmap.Chunks[n].pos.x, xmap.Chunks[n].pos.z);
		}
	}

	// Token: 0x04000FFF RID: 4095
	private Map map;

	// Token: 0x04001000 RID: 4096
	private Block stoneend;

	// Token: 0x04001001 RID: 4097
	private Block dirt;

	// Token: 0x04001002 RID: 4098
	private Block grass;

	// Token: 0x04001003 RID: 4099
	private Block snow;

	// Token: 0x04001004 RID: 4100
	private Block sand;

	// Token: 0x04001005 RID: 4101
	private Block stone;

	// Token: 0x04001006 RID: 4102
	private Block water;

	// Token: 0x04001007 RID: 4103
	private Block wood;

	// Token: 0x04001008 RID: 4104
	private Block wood2;

	// Token: 0x04001009 RID: 4105
	private Block leaf;

	// Token: 0x0400100A RID: 4106
	private Block brick;
}
