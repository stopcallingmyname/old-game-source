using System;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using Ionic.Zlib;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000125 RID: 293
public class ZipLoader : MonoBehaviour
{
	// Token: 0x06000A53 RID: 2643 RVA: 0x000835C0 File Offset: 0x000817C0
	private void Awake()
	{
		ZipLoader.THIS = this;
		this.map = base.GetComponent<Map>();
		BlockSet blockSet = this.map.GetBlockSet();
		this.stoneend = blockSet.GetBlock("Stoneend");
		this.dirt = blockSet.GetBlock("Dirt");
		this.grass = blockSet.GetBlock("Grass");
		this.snow = blockSet.GetBlock("Snow");
		this.sand = blockSet.GetBlock("Sand");
		this.stone = blockSet.GetBlock("Stone");
		this.water = blockSet.GetBlock("!Water");
		this.wood = blockSet.GetBlock("Wood");
		this.wood2 = blockSet.GetBlock("Wood2");
		this.leaf = blockSet.GetBlock("Leaf");
		this.brick = blockSet.GetBlock("Brick");
		this.brick_blue = blockSet.GetBlock("Brick_blue");
		this.brick_red = blockSet.GetBlock("Brick_red");
		this.brick_green = blockSet.GetBlock("Brick_green");
		this.brick_yellow = blockSet.GetBlock("Brick_yellow");
		this.window = blockSet.GetBlock("Window");
		this.box = blockSet.GetBlock("Box");
		this.brick2 = blockSet.GetBlock("Brick2");
		this.stone2 = blockSet.GetBlock("Stone2");
		this.stone3 = blockSet.GetBlock("Stone3");
		this.stone4 = blockSet.GetBlock("Stone4");
		this.tile = blockSet.GetBlock("Tile");
		this.stone5 = blockSet.GetBlock("Stone5");
		this.sand2 = blockSet.GetBlock("Sand2");
		this.stone6 = blockSet.GetBlock("Stone6");
		this.metall1 = blockSet.GetBlock("Metall1");
		this.metall2 = blockSet.GetBlock("Metall2");
		this.stone7 = blockSet.GetBlock("Stone7");
		this.stone8 = blockSet.GetBlock("Stone8");
		this.r_b_blue = blockSet.GetBlock("R_b_blue");
		this.r_b_red = blockSet.GetBlock("R_b_red");
		this.r_b_green = blockSet.GetBlock("R_b_green");
		this.r_b_yellow = blockSet.GetBlock("R_b_yellow");
		this.r_z = blockSet.GetBlock("R_z");
		this.r_c_blue = blockSet.GetBlock("R_c_blue");
		this.r_c_red = blockSet.GetBlock("R_c_red");
		this.r_center = blockSet.GetBlock("R_center");
		this.color1 = blockSet.GetBlock("Color1");
		this.color2 = blockSet.GetBlock("Color2");
		this.color3 = blockSet.GetBlock("Color3");
		this.color4 = blockSet.GetBlock("Color4");
		this.color5 = blockSet.GetBlock("Color5");
		this.color6 = blockSet.GetBlock("Color6");
		this.color7 = blockSet.GetBlock("Color7");
		this.color8 = blockSet.GetBlock("Color8");
		this.color9 = blockSet.GetBlock("Color9");
		this.color10 = blockSet.GetBlock("Color10");
		this.color11 = blockSet.GetBlock("Color11");
		this.color12 = blockSet.GetBlock("Color12");
		this.waterdev = blockSet.GetBlock("Water");
		this.tnt = blockSet.GetBlock("TNT");
		this.danger = blockSet.GetBlock("Danger");
		this.barrel1 = blockSet.GetBlock("Barrel1");
		this.barrel2 = blockSet.GetBlock("Barrel2");
		this.barrel3 = blockSet.GetBlock("Barrel3");
		this.barrel4 = blockSet.GetBlock("Barrel4");
		this.barrel5 = blockSet.GetBlock("Barrel5");
		this.block1 = blockSet.GetBlock("Block1");
		this.box2 = blockSet.GetBlock("Box2");
		this.block2 = blockSet.GetBlock("Block2");
		this.block3 = blockSet.GetBlock("Block3");
		this.block4 = blockSet.GetBlock("Block4");
		this.block5 = blockSet.GetBlock("Block5");
		this.block6 = blockSet.GetBlock("Block6");
		this.block7 = blockSet.GetBlock("Block7");
		this.block8 = blockSet.GetBlock("Block8");
		this.block9 = blockSet.GetBlock("Block9");
		this.block10 = blockSet.GetBlock("Block10");
		this.block11 = blockSet.GetBlock("Block11");
		this.block12 = blockSet.GetBlock("Block12");
		this.block13 = blockSet.GetBlock("Block13");
		this.block14 = blockSet.GetBlock("Block14");
		this.block15 = blockSet.GetBlock("Block15");
		this.block16 = blockSet.GetBlock("Block16");
		this.armored_brick_blue = blockSet.GetBlock("ArmoredBrickBlue");
		this.armored_brick_red = blockSet.GetBlock("ArmoredBrickRed");
		this.armored_brick_green = blockSet.GetBlock("ArmoredBrickGreen");
		this.armored_brick_yellow = blockSet.GetBlock("ArmoredBrickYellow");
		GameObject gameObject = GameObject.Find("GUI");
		this.loadscreen = gameObject.GetComponent<LoadScreen>();
		this.rblock.Clear();
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x00083B3B File Offset: 0x00081D3B
	public void WebLoadMap(string _mapname)
	{
		this.mapname = _mapname;
		this.mapload = false;
		this.gamemode = CONST.GetGameMode();
		base.StartCoroutine(this.WaitForDownload());
	}

	// Token: 0x06000A55 RID: 2645 RVA: 0x00083B63 File Offset: 0x00081D63
	public void WebLoadMapFinish()
	{
		base.StartCoroutine(this.visualize());
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x00083B72 File Offset: 0x00081D72
	private IEnumerator WaitForDownload()
	{
		Debug.LogError("WaitForDownload started");
		int num = 0;
		int.TryParse(this.mapname, out num);
		if (num >= 1000)
		{
			if (CONST.CFG.VERSION == Version.RELEASE)
			{
				if (PlayerProfile.network == NETWORK.VK)
				{
					this.f = string.Concat(new object[]
					{
						"http://maps.novalink.kz/maps/",
						this.mapname,
						".map?",
						DateTime.Now.Minute * 60,
						DateTime.Now.Second
					});
					this.f2 = string.Concat(new object[]
					{
						"http://31.131.253.108/maps/",
						this.mapname,
						".map?",
						DateTime.Now.Minute * 60,
						DateTime.Now.Second
					});
				}
				else if (PlayerProfile.network == NETWORK.OK)
				{
					this.f = string.Concat(new object[]
					{
						"http://5.178.80.226/maps/",
						this.mapname,
						".map?",
						DateTime.Now.Minute * 60,
						DateTime.Now.Second
					});
					this.f2 = string.Concat(new object[]
					{
						"http://5.178.80.226/maps/",
						this.mapname,
						".map?",
						DateTime.Now.Minute * 60,
						DateTime.Now.Second
					});
				}
				else if (PlayerProfile.network == NETWORK.MM)
				{
					this.f = string.Concat(new object[]
					{
						"http://95.213.130.196:800/mail/maps/",
						this.mapname,
						".map?",
						DateTime.Now.Minute * 60,
						DateTime.Now.Second
					});
					this.f2 = string.Concat(new object[]
					{
						"http://95.213.130.196:800/mail/maps/",
						this.mapname,
						".map?",
						DateTime.Now.Minute * 60,
						DateTime.Now.Second
					});
				}
				else if (PlayerProfile.network == NETWORK.FB)
				{
					this.f = string.Concat(new object[]
					{
						"http://95.213.130.196:800/fb/maps/",
						this.mapname,
						".map?",
						DateTime.Now.Minute * 60,
						DateTime.Now.Second
					});
					this.f2 = string.Concat(new object[]
					{
						"http://95.213.130.196:800/fb/maps/",
						this.mapname,
						".map?",
						DateTime.Now.Minute * 60,
						DateTime.Now.Second
					});
				}
				else if (PlayerProfile.network == NETWORK.KG)
				{
					this.f = string.Concat(new object[]
					{
						"http://95.213.130.195/kg/maps/",
						this.mapname,
						".map?",
						DateTime.Now.Minute * 60,
						DateTime.Now.Second
					});
					this.f2 = string.Concat(new object[]
					{
						"http://95.213.130.195/kg/maps/",
						this.mapname,
						".map?",
						DateTime.Now.Minute * 60,
						DateTime.Now.Second
					});
				}
			}
			else
			{
				this.f = string.Concat(new object[]
				{
					"http://95.213.130.196/sf/maps/",
					this.mapname,
					".map?",
					DateTime.Now.Minute * 60,
					DateTime.Now.Second
				});
				this.f2 = string.Concat(new object[]
				{
					"http:/95.213.130.196/sf/maps/",
					this.mapname,
					".map?",
					DateTime.Now.Minute * 60,
					DateTime.Now.Second
				});
			}
		}
		else
		{
			this.f = "http://novalink.kz/sf/maps/" + this.mapname + ".map";
			this.f2 = "http://178.89.110.222/sf/maps/" + this.mapname + ".map";
		}
		WWW www = new WWW(this.f);
		yield return www;
		if (www.error == null)
		{
			MonoBehaviour.print("[1]downloaded size: " + www.size.ToString());
		}
		else
		{
			MonoBehaviour.print(string.Concat(new string[]
			{
				"not downloaded: ",
				this.f,
				" (",
				www.error,
				")"
			}));
			www = new WWW(this.f2);
			yield return www;
			if (www.error != null)
			{
				MonoBehaviour.print("not downloaded " + this.f2 + " " + www.error);
				SceneManager.LoadScene(0);
				yield break;
			}
			MonoBehaviour.print("[2]downloaded size: " + www.size.ToString());
		}
		byte[] array = GZipStream.UncompressBuffer(www.bytes);
		MonoBehaviour.print("unpacksize: " + array.Length.ToString());
		int num2 = 0;
		for (int i = 0; i < 256; i++)
		{
			for (int j = 0; j < 256; j++)
			{
				for (int k = 0; k < 64; k++)
				{
					int num3 = (int)array[num2];
					num2++;
					if (num3 != 0)
					{
						if (this.gamemode != MODE.BUILD && num3 >= 30 && num3 <= 37)
						{
							int num4 = -1;
							if (num3 == 30)
							{
								num4 = 0;
							}
							else if (num3 == 31)
							{
								num4 = 1;
							}
							else if (num3 == 32)
							{
								num4 = 2;
							}
							else if (num3 == 33)
							{
								num4 = 3;
							}
							if (num4 >= 0)
							{
								this.rblock.Add(new CRespawnBlock(num4, i, k, j, 0));
							}
						}
						else if (k == 0)
						{
							this.map.SetBlock(this.stoneend, new Vector3i(i, k, j));
						}
						else
						{
							Vector3i pos = new Vector3i(i, k, j);
							Block block = this.GetBlock(num3);
							if (block == null)
							{
								block = this.brick;
							}
							this.map.SetBlock(block, pos);
						}
					}
				}
			}
		}
		this.mapload = true;
		GM.currExtState = GAME_STATES.GAMELOADMAPCOMPLITE;
		MonoBehaviour.print("rblock = " + this.rblock.Count.ToString());
		yield break;
	}

	// Token: 0x06000A57 RID: 2647 RVA: 0x00083B81 File Offset: 0x00081D81
	public IEnumerator visualize()
	{
		int i = 0;
		yield return null;
		int num;
		for (int cx = 0; cx < 32; cx = num + 1)
		{
			for (int cz = 0; cz < 32; cz = num + 1)
			{
				for (int cy = 0; cy < 8; cy = num + 1)
				{
					Chunk chunk = this.map.GetChunk(new Vector3i(cx, cy, cz));
					if (chunk != null)
					{
						chunk.GetChunkRendererInstance().FastBuild();
						ChunkSunLightComputer.ComputeRays(this.map, cx, cz);
						chunk.GetChunkRenderer().SetLightDirty();
						num = i;
						i = num + 1;
						if (i > 10)
						{
							i = 0;
							yield return null;
						}
					}
					num = cy;
				}
				num = cz;
			}
			num = cx;
		}
		GM.currExtState = GAME_STATES.GAMEVISUALIZINGMAPCOMPLITE;
		yield break;
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x00083B90 File Offset: 0x00081D90
	public void SetBlock(int x, int y, int z, int flag)
	{
		Block block = this.GetBlock(flag);
		if (block != null)
		{
			this.map.SetBlock(block, new Vector3i(x, y, z));
		}
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x00083BC0 File Offset: 0x00081DC0
	public void SetBlock2(int x, int y, int z, int flag)
	{
		Block block = this.GetBlock(flag);
		if (block != null)
		{
			this.map.SetBlockAndRecompute(new BlockData(block), new Vector3i(x, y, z));
		}
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x00083BF4 File Offset: 0x00081DF4
	public Block GetBlock(int flag)
	{
		if (this.gamemode == MODE.BUILD && flag == 7)
		{
			return this.waterdev;
		}
		Block result;
		switch (flag)
		{
		case 1:
			result = this.stoneend;
			break;
		case 2:
			result = this.dirt;
			break;
		case 3:
			result = this.grass;
			break;
		case 4:
			result = this.snow;
			break;
		case 5:
			result = this.sand;
			break;
		case 6:
			result = this.stone;
			break;
		case 7:
			result = this.water;
			break;
		case 8:
			result = this.wood;
			break;
		case 9:
			result = this.wood2;
			break;
		case 10:
			result = this.leaf;
			break;
		case 11:
			result = this.brick;
			break;
		case 12:
			result = this.brick_blue;
			break;
		case 13:
			result = this.brick_red;
			break;
		case 14:
			result = this.brick_green;
			break;
		case 15:
			result = this.brick_yellow;
			break;
		case 16:
			result = this.window;
			break;
		case 17:
			result = this.box;
			break;
		case 18:
			result = this.brick2;
			break;
		case 19:
			result = this.stone2;
			break;
		case 20:
			result = this.stone3;
			break;
		case 21:
			result = this.stone4;
			break;
		case 22:
			result = this.tile;
			break;
		case 23:
			result = this.stone5;
			break;
		case 24:
			result = this.sand2;
			break;
		case 25:
			result = this.stone6;
			break;
		case 26:
			result = this.metall1;
			break;
		case 27:
			result = this.metall2;
			break;
		case 28:
			result = this.stone7;
			break;
		case 29:
			result = this.stone8;
			break;
		case 30:
			result = this.r_b_blue;
			break;
		case 31:
			result = this.r_b_red;
			break;
		case 32:
			result = this.r_b_green;
			break;
		case 33:
			result = this.r_b_yellow;
			break;
		case 34:
			result = this.r_z;
			break;
		case 35:
			result = this.r_c_blue;
			break;
		case 36:
			result = this.r_c_red;
			break;
		case 37:
			result = this.r_center;
			break;
		case 38:
			result = this.color1;
			break;
		case 39:
			result = this.color2;
			break;
		case 40:
			result = this.color3;
			break;
		case 41:
			result = this.color4;
			break;
		case 42:
			result = this.color5;
			break;
		case 43:
			result = this.color6;
			break;
		case 44:
			result = this.color7;
			break;
		case 45:
			result = this.color8;
			break;
		case 46:
			result = this.color9;
			break;
		case 47:
			result = this.color10;
			break;
		case 48:
			result = this.color11;
			break;
		case 49:
			result = this.color12;
			break;
		case 50:
			result = this.tnt;
			break;
		case 51:
			result = this.danger;
			break;
		case 52:
			result = this.barrel1;
			break;
		case 53:
			result = this.barrel2;
			break;
		case 54:
			result = this.barrel3;
			break;
		case 55:
			result = this.barrel4;
			break;
		case 56:
			result = this.barrel5;
			break;
		case 57:
			result = this.block1;
			break;
		case 58:
			result = this.box2;
			break;
		case 59:
			result = this.block2;
			break;
		case 60:
			result = this.block3;
			break;
		case 61:
			result = this.block4;
			break;
		case 62:
			result = this.block5;
			break;
		case 63:
			result = this.block6;
			break;
		case 64:
			result = this.block7;
			break;
		case 65:
			result = this.block8;
			break;
		case 66:
			result = this.block9;
			break;
		case 67:
			result = this.block10;
			break;
		case 68:
			result = this.block11;
			break;
		case 69:
			result = this.block12;
			break;
		case 70:
			result = this.block13;
			break;
		case 71:
			result = this.block14;
			break;
		case 72:
			result = this.block15;
			break;
		case 73:
			result = this.block16;
			break;
		case 74:
			result = this.armored_brick_blue;
			break;
		case 75:
			result = this.armored_brick_red;
			break;
		case 76:
			result = this.armored_brick_green;
			break;
		case 77:
			result = this.armored_brick_yellow;
			break;
		default:
			result = null;
			break;
		}
		return result;
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x000840CC File Offset: 0x000822CC
	public static int GetBlock(string name)
	{
		int result = -1;
		if (name == "Stoneend")
		{
			result = 1;
		}
		else if (name == "Dirt")
		{
			result = 2;
		}
		else if (name == "Grass")
		{
			result = 3;
		}
		else if (name == "Snow")
		{
			result = 4;
		}
		else if (name == "Sand")
		{
			result = 5;
		}
		else if (name == "Stone")
		{
			result = 6;
		}
		else if (name == "!Water")
		{
			result = 7;
		}
		else if (name == "Wood")
		{
			result = 8;
		}
		else if (name == "Wood2")
		{
			result = 9;
		}
		else if (name == "Leaf")
		{
			result = 10;
		}
		else if (name == "Brick")
		{
			result = 11;
		}
		else if (name == "Window")
		{
			result = 16;
		}
		else if (name == "Box")
		{
			result = 17;
		}
		else if (name == "Brick2")
		{
			result = 18;
		}
		else if (name == "Stone2")
		{
			result = 19;
		}
		else if (name == "Stone3")
		{
			result = 20;
		}
		else if (name == "Stone4")
		{
			result = 21;
		}
		else if (name == "Tile")
		{
			result = 22;
		}
		else if (name == "Stone5")
		{
			result = 23;
		}
		else if (name == "Sand2")
		{
			result = 24;
		}
		else if (name == "Stone6")
		{
			result = 25;
		}
		else if (name == "Metall1")
		{
			result = 26;
		}
		else if (name == "Metall2")
		{
			result = 27;
		}
		else if (name == "Stone7")
		{
			result = 28;
		}
		else if (name == "Stone8")
		{
			result = 29;
		}
		else if (name == "R_b_blue")
		{
			result = 30;
		}
		else if (name == "R_b_red")
		{
			result = 31;
		}
		else if (name == "R_b_green")
		{
			result = 32;
		}
		else if (name == "R_b_yellow")
		{
			result = 33;
		}
		else if (name == "R_z")
		{
			result = 34;
		}
		else if (name == "R_c_blue")
		{
			result = 35;
		}
		else if (name == "R_c_red")
		{
			result = 36;
		}
		else if (name == "R_center")
		{
			result = 37;
		}
		else if (name == "Color1")
		{
			result = 38;
		}
		else if (name == "Color2")
		{
			result = 39;
		}
		else if (name == "Color3")
		{
			result = 40;
		}
		else if (name == "Color4")
		{
			result = 41;
		}
		else if (name == "Color5")
		{
			result = 42;
		}
		else if (name == "Color6")
		{
			result = 43;
		}
		else if (name == "Color7")
		{
			result = 44;
		}
		else if (name == "Color8")
		{
			result = 45;
		}
		else if (name == "Color9")
		{
			result = 46;
		}
		else if (name == "Color10")
		{
			result = 47;
		}
		else if (name == "Color11")
		{
			result = 48;
		}
		else if (name == "Color12")
		{
			result = 49;
		}
		else if (name == "TNT")
		{
			result = 50;
		}
		else if (name == "Danger")
		{
			result = 51;
		}
		else if (name == "Barrel1")
		{
			result = 52;
		}
		else if (name == "Barrel2")
		{
			result = 53;
		}
		else if (name == "Barrel3")
		{
			result = 54;
		}
		else if (name == "Barrel4")
		{
			result = 55;
		}
		else if (name == "Barrel5")
		{
			result = 56;
		}
		else if (name == "Block1")
		{
			result = 57;
		}
		else if (name == "Box2")
		{
			result = 58;
		}
		else if (name == "Block2")
		{
			result = 59;
		}
		else if (name == "Block3")
		{
			result = 60;
		}
		else if (name == "Block4")
		{
			result = 61;
		}
		else if (name == "Block5")
		{
			result = 62;
		}
		else if (name == "Block6")
		{
			result = 63;
		}
		else if (name == "Block7")
		{
			result = 64;
		}
		else if (name == "Block8")
		{
			result = 65;
		}
		else if (name == "Block9")
		{
			result = 66;
		}
		else if (name == "Block10")
		{
			result = 67;
		}
		else if (name == "Block11")
		{
			result = 68;
		}
		else if (name == "Block12")
		{
			result = 69;
		}
		else if (name == "Block13")
		{
			result = 70;
		}
		else if (name == "Block14")
		{
			result = 71;
		}
		else if (name == "Block15")
		{
			result = 72;
		}
		else if (name == "Block16")
		{
			result = 73;
		}
		else if (name == "ArmoredBrickBlue")
		{
			result = 74;
		}
		else if (name == "ArmoredBrickRed")
		{
			result = 75;
		}
		else if (name == "ArmoredBrickGreen")
		{
			result = 76;
		}
		else if (name == "ArmoredBrickYellow")
		{
			result = 77;
		}
		return result;
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x000846B8 File Offset: 0x000828B8
	public static int GetBlockType(Block _b)
	{
		if (_b.GetName() == "Stoneend")
		{
			return 4;
		}
		if (_b.GetName() == "Dirt")
		{
			return 1;
		}
		if (_b.GetName() == "Grass")
		{
			return 1;
		}
		if (_b.GetName() == "Snow")
		{
			return 2;
		}
		if (_b.GetName() == "Sand")
		{
			return 1;
		}
		if (_b.GetName() == "Stone")
		{
			return 3;
		}
		if (_b.GetName() == "!Water")
		{
			return 5;
		}
		if (_b.GetName() == "Wood")
		{
			return 6;
		}
		if (_b.GetName() == "Wood2")
		{
			return 6;
		}
		if (_b.GetName() == "Leaf")
		{
			return 1;
		}
		if (_b.GetName() == "Brick")
		{
			return 3;
		}
		if (_b.GetName() == "Brick_red")
		{
			return 3;
		}
		if (_b.GetName() == "Brick_blue")
		{
			return 3;
		}
		if (_b.GetName() == "Brick_green")
		{
			return 3;
		}
		if (_b.GetName() == "Brick_yellow")
		{
			return 3;
		}
		if (_b.GetName() == "Window")
		{
			return 6;
		}
		if (_b.GetName() == "Box")
		{
			return 6;
		}
		if (_b.GetName() == "Brick2")
		{
			return 3;
		}
		if (_b.GetName() == "Stone2")
		{
			return 3;
		}
		if (_b.GetName() == "Stone3")
		{
			return 3;
		}
		if (_b.GetName() == "Stone4")
		{
			return 3;
		}
		if (_b.GetName() == "Tile")
		{
			return 1;
		}
		if (_b.GetName() == "Stone5")
		{
			return 3;
		}
		if (_b.GetName() == "Sand2")
		{
			return 1;
		}
		if (_b.GetName() == "Stone6")
		{
			return 1;
		}
		if (_b.GetName() == "Metall1")
		{
			return 4;
		}
		if (_b.GetName() == "Metall2")
		{
			return 4;
		}
		if (_b.GetName() == "Stone7")
		{
			return 3;
		}
		if (_b.GetName() == "Stone8")
		{
			return 3;
		}
		if (_b.GetName() == "R_b_blue")
		{
			return 1;
		}
		if (_b.GetName() == "R_b_red")
		{
			return 1;
		}
		if (_b.GetName() == "R_b_green")
		{
			return 1;
		}
		if (_b.GetName() == "R_b_yellow")
		{
			return 1;
		}
		if (_b.GetName() == "R_z")
		{
			return 1;
		}
		if (_b.GetName() == "R_c_blue")
		{
			return 1;
		}
		if (_b.GetName() == "R_c_red")
		{
			return 1;
		}
		if (_b.GetName() == "R_center")
		{
			return 1;
		}
		if (_b.GetName() == "Color1")
		{
			return 3;
		}
		if (_b.GetName() == "Color2")
		{
			return 3;
		}
		if (_b.GetName() == "Color3")
		{
			return 3;
		}
		if (_b.GetName() == "Color4")
		{
			return 3;
		}
		if (_b.GetName() == "Color5")
		{
			return 3;
		}
		if (_b.GetName() == "Color6")
		{
			return 3;
		}
		if (_b.GetName() == "Color7")
		{
			return 3;
		}
		if (_b.GetName() == "Color8")
		{
			return 3;
		}
		if (_b.GetName() == "Color9")
		{
			return 3;
		}
		if (_b.GetName() == "Color10")
		{
			return 3;
		}
		if (_b.GetName() == "Color11")
		{
			return 3;
		}
		if (_b.GetName() == "Color12")
		{
			return 3;
		}
		if (_b.GetName() == "TNT")
		{
			return 1;
		}
		if (_b.GetName() == "Danger")
		{
			return 3;
		}
		if (_b.GetName() == "Barrel1")
		{
			return 4;
		}
		if (_b.GetName() == "Barrel2")
		{
			return 4;
		}
		if (_b.GetName() == "Barrel3")
		{
			return 4;
		}
		if (_b.GetName() == "Barrel4")
		{
			return 4;
		}
		if (_b.GetName() == "Barrel5")
		{
			return 4;
		}
		if (_b.GetName() == "Block1")
		{
			return 3;
		}
		if (_b.GetName() == "Box2")
		{
			return 6;
		}
		if (_b.GetName() == "Block2")
		{
			return 3;
		}
		if (_b.GetName() == "Block3")
		{
			return 3;
		}
		if (_b.GetName() == "Block4")
		{
			return 4;
		}
		if (_b.GetName() == "Block5")
		{
			return 4;
		}
		if (_b.GetName() == "Block6")
		{
			return 3;
		}
		if (_b.GetName() == "Block7")
		{
			return 3;
		}
		if (_b.GetName() == "Block8")
		{
			return 3;
		}
		if (_b.GetName() == "Block9")
		{
			return 1;
		}
		if (_b.GetName() == "Block10")
		{
			return 3;
		}
		if (_b.GetName() == "Block11")
		{
			return 3;
		}
		if (_b.GetName() == "Block12")
		{
			return 3;
		}
		if (_b.GetName() == "Block13")
		{
			return 3;
		}
		if (_b.GetName() == "Block14")
		{
			return 3;
		}
		if (_b.GetName() == "Block15")
		{
			return 3;
		}
		if (_b.GetName() == "Block16")
		{
			return 3;
		}
		if (_b.GetName() == "ArmoredBrickBlue")
		{
			return 4;
		}
		if (_b.GetName() == "ArmoredBrickRed")
		{
			return 4;
		}
		if (_b.GetName() == "ArmoredBrickGreen")
		{
			return 4;
		}
		if (_b.GetName() == "ArmoredBrickYellow")
		{
			return 4;
		}
		return 1;
	}

	// Token: 0x04001011 RID: 4113
	public static ZipLoader THIS;

	// Token: 0x04001012 RID: 4114
	private Map map;

	// Token: 0x04001013 RID: 4115
	private Block stoneend;

	// Token: 0x04001014 RID: 4116
	private Block dirt;

	// Token: 0x04001015 RID: 4117
	private Block grass;

	// Token: 0x04001016 RID: 4118
	private Block snow;

	// Token: 0x04001017 RID: 4119
	private Block sand;

	// Token: 0x04001018 RID: 4120
	private Block stone;

	// Token: 0x04001019 RID: 4121
	private Block water;

	// Token: 0x0400101A RID: 4122
	private Block wood;

	// Token: 0x0400101B RID: 4123
	private Block wood2;

	// Token: 0x0400101C RID: 4124
	private Block leaf;

	// Token: 0x0400101D RID: 4125
	public Block brick;

	// Token: 0x0400101E RID: 4126
	private Block brick_blue;

	// Token: 0x0400101F RID: 4127
	private Block brick_red;

	// Token: 0x04001020 RID: 4128
	private Block brick_green;

	// Token: 0x04001021 RID: 4129
	private Block brick_yellow;

	// Token: 0x04001022 RID: 4130
	private Block window;

	// Token: 0x04001023 RID: 4131
	private Block box;

	// Token: 0x04001024 RID: 4132
	private Block brick2;

	// Token: 0x04001025 RID: 4133
	private Block stone2;

	// Token: 0x04001026 RID: 4134
	private Block stone3;

	// Token: 0x04001027 RID: 4135
	private Block stone4;

	// Token: 0x04001028 RID: 4136
	private Block tile;

	// Token: 0x04001029 RID: 4137
	private Block stone5;

	// Token: 0x0400102A RID: 4138
	private Block sand2;

	// Token: 0x0400102B RID: 4139
	private Block stone6;

	// Token: 0x0400102C RID: 4140
	private Block metall1;

	// Token: 0x0400102D RID: 4141
	private Block metall2;

	// Token: 0x0400102E RID: 4142
	private Block stone7;

	// Token: 0x0400102F RID: 4143
	private Block stone8;

	// Token: 0x04001030 RID: 4144
	private Block r_b_blue;

	// Token: 0x04001031 RID: 4145
	private Block r_b_red;

	// Token: 0x04001032 RID: 4146
	private Block r_b_green;

	// Token: 0x04001033 RID: 4147
	private Block r_b_yellow;

	// Token: 0x04001034 RID: 4148
	private Block r_z;

	// Token: 0x04001035 RID: 4149
	private Block r_c_blue;

	// Token: 0x04001036 RID: 4150
	private Block r_c_red;

	// Token: 0x04001037 RID: 4151
	private Block r_center;

	// Token: 0x04001038 RID: 4152
	private Block color1;

	// Token: 0x04001039 RID: 4153
	private Block color2;

	// Token: 0x0400103A RID: 4154
	private Block color3;

	// Token: 0x0400103B RID: 4155
	private Block color4;

	// Token: 0x0400103C RID: 4156
	private Block color5;

	// Token: 0x0400103D RID: 4157
	private Block color6;

	// Token: 0x0400103E RID: 4158
	private Block color7;

	// Token: 0x0400103F RID: 4159
	private Block color8;

	// Token: 0x04001040 RID: 4160
	private Block color9;

	// Token: 0x04001041 RID: 4161
	private Block color10;

	// Token: 0x04001042 RID: 4162
	private Block color11;

	// Token: 0x04001043 RID: 4163
	private Block color12;

	// Token: 0x04001044 RID: 4164
	private Block waterdev;

	// Token: 0x04001045 RID: 4165
	private Block tnt;

	// Token: 0x04001046 RID: 4166
	private Block danger;

	// Token: 0x04001047 RID: 4167
	private Block barrel1;

	// Token: 0x04001048 RID: 4168
	private Block barrel2;

	// Token: 0x04001049 RID: 4169
	private Block barrel3;

	// Token: 0x0400104A RID: 4170
	private Block barrel4;

	// Token: 0x0400104B RID: 4171
	private Block barrel5;

	// Token: 0x0400104C RID: 4172
	private Block block1;

	// Token: 0x0400104D RID: 4173
	private Block box2;

	// Token: 0x0400104E RID: 4174
	private Block block2;

	// Token: 0x0400104F RID: 4175
	private Block block3;

	// Token: 0x04001050 RID: 4176
	private Block block4;

	// Token: 0x04001051 RID: 4177
	private Block block5;

	// Token: 0x04001052 RID: 4178
	private Block block6;

	// Token: 0x04001053 RID: 4179
	private Block block7;

	// Token: 0x04001054 RID: 4180
	private Block block8;

	// Token: 0x04001055 RID: 4181
	private Block block9;

	// Token: 0x04001056 RID: 4182
	private Block block10;

	// Token: 0x04001057 RID: 4183
	private Block block11;

	// Token: 0x04001058 RID: 4184
	private Block block12;

	// Token: 0x04001059 RID: 4185
	private Block block13;

	// Token: 0x0400105A RID: 4186
	private Block block14;

	// Token: 0x0400105B RID: 4187
	private Block block15;

	// Token: 0x0400105C RID: 4188
	private Block block16;

	// Token: 0x0400105D RID: 4189
	private Block armored_brick_blue;

	// Token: 0x0400105E RID: 4190
	private Block armored_brick_red;

	// Token: 0x0400105F RID: 4191
	private Block armored_brick_green;

	// Token: 0x04001060 RID: 4192
	private Block armored_brick_yellow;

	// Token: 0x04001061 RID: 4193
	private string mapname;

	// Token: 0x04001062 RID: 4194
	public bool mapload;

	// Token: 0x04001063 RID: 4195
	public MODE gamemode;

	// Token: 0x04001064 RID: 4196
	public int mapversion;

	// Token: 0x04001065 RID: 4197
	private Client cscl;

	// Token: 0x04001066 RID: 4198
	private PlayerControl cspc;

	// Token: 0x04001067 RID: 4199
	private LoadScreen loadscreen;

	// Token: 0x04001068 RID: 4200
	public List<CRespawnBlock> rblock = new List<CRespawnBlock>();

	// Token: 0x04001069 RID: 4201
	private string f;

	// Token: 0x0400106A RID: 4202
	private string f2;
}
