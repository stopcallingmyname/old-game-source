using System;
using UnityEngine;

// Token: 0x0200008F RID: 143
public class MainGUI : MonoBehaviour
{
	// Token: 0x06000429 RID: 1065 RVA: 0x0005184C File Offset: 0x0004FA4C
	private void Start()
	{
		this.EM = base.GetComponent<E_Menu>();
		this.NS = base.GetComponent<New_Slots>();
		this.NST = base.GetComponent<New_Select_Team>();
		MainGUI.sel_team = false;
		Debug.Log("SET E_MENU FALSE");
		MainGUI.e_menu = false;
		Map map = (Map)Object.FindObjectOfType(typeof(Map));
		this.csr = (Radar)Object.FindObjectOfType(typeof(Radar));
		this.cszl = (ZipLoader)Object.FindObjectOfType(typeof(ZipLoader));
		this.blockSet = map.GetBlockSet();
		this.blocksel = null;
		this.SetTeamBlocks();
		this.tex_inv = (Resources.Load("GUI/inv") as Texture2D);
		this.gui_style = new GUIStyle();
		this.gui_style.font = FontManager.font[2];
		this.gui_style.fontSize = 14;
		this.gui_style.normal.textColor = new Color(1f, 1f, 1f, 1f);
		this.gui_style.alignment = TextAnchor.LowerCenter;
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x0005196C File Offset: 0x0004FB6C
	public void SetTeamBlocks()
	{
		this.teamblock[0] = this.blockSet.GetBlock("Brick_blue");
		this.teamblock[1] = this.blockSet.GetBlock("Brick_red");
		this.teamblock[2] = this.blockSet.GetBlock("Brick_green");
		this.teamblock[3] = this.blockSet.GetBlock("Brick_yellow");
		this.teamblock[4] = this.blockSet.GetBlock("ArmoredBrickBlue");
		this.teamblock[5] = this.blockSet.GetBlock("ArmoredBrickRed");
		this.teamblock[6] = this.blockSet.GetBlock("ArmoredBrickGreen");
		this.teamblock[7] = this.blockSet.GetBlock("ArmoredBrickYellow");
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x00051A3C File Offset: 0x0004FC3C
	private void Update()
	{
		MainGUI.UpdateCursorLock();
		if (ConnectionInfo.mode == 2)
		{
			if (this.blocksel == null)
			{
				if (this.blockSet != null)
				{
					this.blocksel = this.blockSet.GetBlock(1);
					this.selectedBlock = this.blockSet.GetBlock(1);
					this.SetBlockTexture(this.blocksel);
					if (this.cscl == null)
					{
						this.cscl = Object.FindObjectOfType<Client>();
					}
					int block = ZipLoader.GetBlock(this.blocksel.GetName());
					this.cscl.send_selectblock((byte)block);
					RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].blockFlag = block;
				}
			}
			else if ((this.blocksel.GetName() == "Brick_blue" || this.blocksel.GetName() == "Brick_red" || this.blocksel.GetName() == "Brick_green" || this.blocksel.GetName() == "Brick_yellow" || this.blocksel.GetName() == "!Water" || this.blocksel.GetName() == "TNT" || this.blocksel.GetName() == "ArmoredBrickBlue" || this.blocksel.GetName() == "ArmoredBrickRed" || this.blocksel.GetName() == "ArmoredBrickGreen" || this.blocksel.GetName() == "ArmoredBrickYellow") && this.blockSet != null)
			{
				this.blocksel = this.blockSet.GetBlock(1);
				this.selectedBlock = this.blockSet.GetBlock(1);
				this.SetBlockTexture(this.blocksel);
				if (this.cscl == null)
				{
					this.cscl = Object.FindObjectOfType<Client>();
				}
				int block2 = ZipLoader.GetBlock(this.blocksel.GetName());
				this.cscl.send_selectblock((byte)block2);
				RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].blockFlag = block2;
			}
		}
		if (CONST.GetGameMode() == MODE.ZOMBIE && RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].zombie)
		{
			this.g_canbuy = false;
			if (MainGUI.e_menu)
			{
				this.CloseEMenu(false);
			}
			if (MainGUI.sel_team)
			{
				this.CloseSelectTeam();
			}
			return;
		}
		if (RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Active && RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Dead == 1 && MainGUI.sel_team)
		{
			this.CloseSelectTeam();
			return;
		}
		if ((CONST.GetGameMode() == MODE.SURVIVAL || CONST.GetGameMode() == MODE.CLEAR || CONST.GetGameMode() == MODE.FFA) && MainGUI.sel_team)
		{
			this.CloseSelectTeam();
			return;
		}
		if (this.tc == null)
		{
			this.tc = (TankController)Object.FindObjectOfType(typeof(TankController));
		}
		if (this.cc == null)
		{
			this.cc = (CarController)Object.FindObjectOfType(typeof(CarController));
		}
		if (this.tc != null && this.tc.enabled)
		{
			this.CloseSelectTeam();
			this.CloseEMenu(true);
			this.g_canbuy = false;
			return;
		}
		if (this.cc != null && this.cc.enabled)
		{
			this.CloseSelectTeam();
			this.CloseEMenu(true);
			this.g_canbuy = false;
			return;
		}
		if (ConnectionInfo.mode != 12 && ConnectionInfo.mode != 8 && Input.GetKeyDown(KeyCode.E) && this.g_canbuy)
		{
			if (!RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Active || RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Dead == 1)
			{
				this.g_canbuy = false;
				return;
			}
			if (MainGUI.sel_team)
			{
				return;
			}
			Debug.Log("SET E_MENU " + (!MainGUI.e_menu).ToString());
			MainGUI.e_menu = !MainGUI.e_menu;
			if (MainGUI.e_menu)
			{
				this.OpenEMenu();
			}
			else
			{
				this.CloseEMenu(false);
			}
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			if (CONST.GetGameMode() == MODE.BUILD || CONST.GetGameMode() == MODE.ZOMBIE || CONST.GetGameMode() == MODE.SURVIVAL || CONST.GetGameMode() == MODE.CLEAR || !RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Active || RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Dead == 1)
			{
				return;
			}
			if (MainGUI.e_menu)
			{
				return;
			}
			MainGUI.sel_team = !MainGUI.sel_team;
			if (MainGUI.sel_team)
			{
				this.OpenSelectTeam();
			}
			else
			{
				this.CloseSelectTeam();
			}
		}
		if (Time.time > this.g_buycheck)
		{
			if (!RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Active || RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Dead == 1)
			{
				this.g_canbuy = false;
			}
			if (this.cspc == null)
			{
				this.cspc = (PlayerControl)Object.FindObjectOfType(typeof(PlayerControl));
			}
			this.g_canbuy = false;
			this.g_buycheck += 0.5f;
			if (CONST.GetGameMode() == MODE.BUILD || CONST.GetGameMode() == MODE.ZOMBIE || CONST.GetGameMode() == MODE.SURVIVAL || CONST.GetGameMode() == MODE.CLEAR)
			{
				this.g_canbuy = true;
				return;
			}
			if (this.goPlayer == null)
			{
				this.goPlayer = GameObject.Find("Player");
			}
			if (this.cspc && this.goPlayer && this.cspc.GetTeam() <= 3)
			{
				if ((CONST.GetGameMode() == MODE.BATTLE && this.cszl.rblock.Count == 32) || (CONST.GetGameMode() == MODE.FFA || CONST.GetGameMode() == MODE.CLASSIC || (this.cszl.mapversion > 0 && CONST.GetGameMode() == MODE.CONTRA)) || CONST.GetGameMode() == MODE.TANK || CONST.GetGameMode() == MODE.PRORIV)
				{
					for (int i = 0; i < this.cszl.rblock.Count; i++)
					{
						if ((this.cszl.rblock[i].mode == (int)CONST.GetGameMode() || CONST.GetGameMode() == MODE.PRORIV) && (this.cszl.rblock[i].team == this.cspc.GetTeam() || CONST.GetGameMode() == MODE.PRORIV) && Mathf.Abs(this.cszl.rblock[i].pos.x - this.goPlayer.transform.position.x) < 4f && Mathf.Abs(this.cszl.rblock[i].pos.z - this.goPlayer.transform.position.z) < 4f)
						{
							this.g_canbuy = true;
						}
					}
					return;
				}
				int num = 8;
				if (CONST.GetGameMode() == MODE.CONTRA)
				{
					num = 24;
				}
				if (Mathf.Abs(this.csr.team_pos[this.cspc.GetTeam()].x - this.goPlayer.transform.position.x) < (float)num && Mathf.Abs(this.csr.team_pos[this.cspc.GetTeam()].z - this.goPlayer.transform.position.z) < (float)num)
				{
					this.g_canbuy = true;
					return;
				}
				this.g_canbuy = false;
			}
		}
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x000521F0 File Offset: 0x000503F0
	public void CloseAll()
	{
		GM.currGUIState = GUIGS.NULL;
		MainGUI.e_menu = false;
		MainGUI.sel_team = false;
		if (this.csws == null)
		{
			this.csws = Object.FindObjectOfType<WeaponSystem>();
		}
		if (!RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].zombie && RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Active && RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Dead == 0 && CONST.GetGameMode() == MODE.ZOMBIE)
		{
			this.csws.SetPrimaryWeapon(RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].WeaponID == 0 && ConnectionInfo.mode == 2);
			this.NS.Active[4] = true;
		}
		if (PlayerProfile.myteam > -1)
		{
			MainGUI.ForceCursor = false;
			if (!RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].zombie && RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Active && RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Dead == 0)
			{
				this.csws.SetPrimaryWeapon(RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].WeaponID == 0 && ConnectionInfo.mode == 2);
			}
			this.NS.Active[4] = true;
		}
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x00052344 File Offset: 0x00050544
	public void CloseSelectTeam()
	{
		GM.currGUIState = GUIGS.NULL;
		MainGUI.sel_team = false;
		MainGUI.ForceCursor = false;
		if (!MainGUI.e_menu)
		{
			if (this.csws == null)
			{
				this.csws = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
			}
			if (PlayerProfile.myteam > -1)
			{
				if (!RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].zombie && RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Active && RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Dead == 0)
				{
					this.csws.SetPrimaryWeapon(RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].WeaponID == 0 && ConnectionInfo.mode == 2);
				}
				this.NS.Active[4] = true;
			}
		}
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x00052424 File Offset: 0x00050624
	public void CloseEMenu(bool skipSetWeapon = false)
	{
		GM.currGUIState = GUIGS.NULL;
		MainGUI.e_menu = false;
		MainGUI.ForceCursor = false;
		if (this.csws == null)
		{
			this.csws = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		if (!RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].zombie && RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Active && RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Dead == 0 && CONST.GetGameMode() == MODE.ZOMBIE)
		{
			this.csws.SetPrimaryWeapon(false);
			this.NS.Active[4] = true;
		}
		if (PlayerProfile.myteam > -1 && !skipSetWeapon)
		{
			if (!RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].zombie && RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Active && RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].Dead == 0)
			{
				this.csws.SetPrimaryWeapon(RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].WeaponID == 0 && ConnectionInfo.mode == 2);
			}
			this.NS.Active[4] = true;
		}
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x00052564 File Offset: 0x00050764
	public void OpenSelectTeam()
	{
		this.NS.Active[4] = false;
		MainGUI.e_menu = false;
		MainGUI.sel_team = true;
		if (this.csws == null)
		{
			this.csws = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		MainGUI.ForceCursor = true;
		this.csws.HideWeapons(true);
		GM.currGUIState = GUIGS.TEAMSELECT;
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x000525CC File Offset: 0x000507CC
	public void OpenEMenu()
	{
		this.NS.Active[4] = false;
		this.EM.Init();
		MainGUI.e_menu = true;
		MainGUI.sel_team = false;
		if (this.csws == null)
		{
			this.csws = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		MainGUI.ForceCursor = true;
		this.csws.HideWeapons(true);
		GM.currGUIState = GUIGS.WEAPONSELECT;
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x00052640 File Offset: 0x00050840
	private void OnGUI()
	{
		if (MainGUI.e_menu)
		{
			if (CONST.GetGameMode() == MODE.BUILD)
			{
				GUILayout.Window(1, new Rect(0f, 0f, (float)Screen.width * 0.5f, (float)Screen.height * 0.6f)
				{
					center = new Vector2((float)Screen.width, (float)Screen.height) / 2f
				}, new GUI.WindowFunction(this.DoInventoryWindow), "", this.gui_style, Array.Empty<GUILayoutOption>());
			}
			else
			{
				if (CONST.GetGameMode() == MODE.SNOWBALLS || CONST.GetGameMode() == MODE.M1945)
				{
					return;
				}
				this.EM.Draw_E_Menu();
				if (this.EM.Active_Tab_Index < 4)
				{
					this.NS.Draw_New_Slots(this.EM.Active_Tab_Index + this.EM.Active_Item_PodIndex + 1);
				}
				else
				{
					this.NS.Draw_New_Slots(-1);
				}
			}
		}
		if (MainGUI.sel_team)
		{
			this.NST.Draw_New_Select_Team();
		}
		if (this.g_canbuy)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width - 40), (float)(Screen.height - 220), 32f, 32f), this.tex_inv);
		}
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x00052778 File Offset: 0x00050978
	public void SetBlockTextureTeam(GameObject face, GameObject top, int team, bool self = false)
	{
		this.block_face = face;
		this.block_top = top;
		if (team > 7)
		{
			team = 0;
		}
		if (!self || CONST.GetGameMode() != MODE.BUILD)
		{
			this.SetBlockTexture(this.teamblock[team]);
			return;
		}
		if (this.blocksel != null)
		{
			this.SetBlockTexture(this.blocksel);
			return;
		}
		this.SetBlockTexture(this.teamblock[team]);
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x000527D8 File Offset: 0x000509D8
	public void SetBlockTextureForBuild(GameObject face, GameObject top, int flag)
	{
		ZipLoader zipLoader = Object.FindObjectOfType<ZipLoader>();
		Block block;
		if (zipLoader != null)
		{
			block = zipLoader.GetBlock(flag);
		}
		else
		{
			if (this.blockSet == null)
			{
				Map map = Object.FindObjectOfType<Map>();
				this.blockSet = map.GetBlockSet();
			}
			block = this.blockSet.GetBlock(flag);
		}
		if (block == null)
		{
			return;
		}
		if (block.GetTexture() == null)
		{
			return;
		}
		if (face == null || top == null)
		{
			return;
		}
		face.GetComponent<Renderer>().materials[0].mainTexture = block.GetTexture();
		Rect previewFace = block.GetPreviewFace();
		face.GetComponent<Renderer>().materials[0].mainTextureOffset = new Vector2(previewFace.x, previewFace.y + 0.0625f);
		face.GetComponent<Renderer>().materials[0].mainTextureScale = new Vector2(previewFace.width, previewFace.height);
		top.GetComponent<Renderer>().materials[0].mainTexture = block.GetTexture();
		Rect topFace = block.GetTopFace();
		top.GetComponent<Renderer>().materials[0].mainTextureOffset = new Vector2(topFace.x, topFace.y);
		top.GetComponent<Renderer>().materials[0].mainTextureScale = new Vector2(topFace.width, topFace.height);
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x0005292B File Offset: 0x00050B2B
	public Block GetBlockTextureTeam(int team)
	{
		if (team > 7)
		{
			return this.teamblock[0];
		}
		return this.teamblock[team];
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x00052944 File Offset: 0x00050B44
	public void SetBlockTexture(Block block)
	{
		if (block == null)
		{
			return;
		}
		if (block.GetTexture() == null)
		{
			return;
		}
		if (this.block_face == null || this.block_top == null)
		{
			return;
		}
		this.block_face.GetComponent<Renderer>().materials[0].mainTexture = block.GetTexture();
		Rect previewFace = block.GetPreviewFace();
		this.block_face.GetComponent<Renderer>().materials[0].mainTextureOffset = new Vector2(previewFace.x, previewFace.y + 0.0625f);
		this.block_face.GetComponent<Renderer>().materials[0].mainTextureScale = new Vector2(previewFace.width, previewFace.height);
		this.block_top.GetComponent<Renderer>().materials[0].mainTexture = block.GetTexture();
		Rect topFace = block.GetTopFace();
		this.block_top.GetComponent<Renderer>().materials[0].mainTextureOffset = new Vector2(topFace.x, topFace.y);
		this.block_top.GetComponent<Renderer>().materials[0].mainTextureScale = new Vector2(topFace.width, topFace.height);
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x00052A78 File Offset: 0x00050C78
	private void DoInventoryWindow(int windowID)
	{
		this.selectedBlock = this.DrawInventory(this.blockSet, ref this.scrollPosition, this.selectedBlock);
		if (this.selectedBlock != null && this.selectedBlock != this.blocksel)
		{
			this.blocksel = this.selectedBlock;
			this.SetBlockTexture(this.blocksel);
			int num = ZipLoader.GetBlock(this.blocksel.GetName());
			if (this.blocksel.GetName() == "Water")
			{
				num = 7;
			}
			if (this.cscl == null)
			{
				this.cscl = Object.FindObjectOfType<Client>();
			}
			if (this.csws == null)
			{
				this.csws = Object.FindObjectOfType<WeaponSystem>();
			}
			if (num > 0)
			{
				this.cscl.send_selectblock((byte)num);
				RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].blockFlag = num;
				RemotePlayersUpdater.Instance.SetCurrentWeapon(PlayerProfile.myindex, 0);
				RemotePlayersUpdater.Instance.Bots[PlayerProfile.myindex].WeaponID = 0;
			}
		}
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x00052B80 File Offset: 0x00050D80
	private Block DrawInventory(BlockSet blockSet, ref Vector2 scrollPosition, Block selected)
	{
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, Array.Empty<GUILayoutOption>());
		int i = 0;
		int num = 0;
		while (i < blockSet.GetBlockCount())
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			int j = 0;
			while (j < 8)
			{
				Block block = blockSet.GetBlock(i);
				PlayerControl.isPrivateAdmin();
				if (block != null && block.GetName() != null && (block.GetName() == "Brick_blue" || block.GetName() == "Brick_red" || block.GetName() == "Brick_green" || block.GetName() == "Brick_yellow" || block.GetName() == "!Water" || block.GetName() == "TNT" || block.GetName() == "ArmoredBrickBlue" || block.GetName() == "ArmoredBrickRed" || block.GetName() == "ArmoredBrickGreen" || block.GetName() == "ArmoredBrickYellow"))
				{
					j--;
				}
				else if (MainGUI.DrawBlock(block, block == selected && selected != null))
				{
					selected = block;
				}
				j++;
				i++;
			}
			GUILayout.EndHorizontal();
			num++;
		}
		GUILayout.EndScrollView();
		return selected;
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x00052CDC File Offset: 0x00050EDC
	private static bool DrawBlock(Block block, bool selected)
	{
		Rect aspectRect = GUILayoutUtility.GetAspectRect(1f);
		if (selected)
		{
			GUI.Box(aspectRect, GUIContent.none);
		}
		Vector3 v = aspectRect.center;
		aspectRect.width -= 8f;
		aspectRect.height -= 8f;
		aspectRect.center = v;
		return block != null && block.DrawPreview(aspectRect);
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x00052D50 File Offset: 0x00050F50
	public static void UpdateCursorLock()
	{
		if (MainGUI.ForceCursor)
		{
			Cursor.visible = true;
			if (Cursor.lockState.Equals(CursorLockMode.None))
			{
				return;
			}
			Cursor.lockState = CursorLockMode.None;
			return;
		}
		else
		{
			Cursor.visible = false;
			if (Cursor.lockState.Equals(CursorLockMode.Locked))
			{
				return;
			}
			Cursor.lockState = CursorLockMode.Locked;
			return;
		}
	}

	// Token: 0x040008DA RID: 2266
	private Vector2 scrollPosition = Vector3.zero;

	// Token: 0x040008DB RID: 2267
	private E_Menu EM;

	// Token: 0x040008DC RID: 2268
	private New_Slots NS;

	// Token: 0x040008DD RID: 2269
	private New_Select_Team NST;

	// Token: 0x040008DE RID: 2270
	public static bool sel_team;

	// Token: 0x040008DF RID: 2271
	public static bool e_menu;

	// Token: 0x040008E0 RID: 2272
	private WeaponSystem csws;

	// Token: 0x040008E1 RID: 2273
	private GameObject block_face;

	// Token: 0x040008E2 RID: 2274
	private GameObject block_top;

	// Token: 0x040008E3 RID: 2275
	private GameObject goMap;

	// Token: 0x040008E4 RID: 2276
	private GameObject goPlayer;

	// Token: 0x040008E5 RID: 2277
	private Client cscl;

	// Token: 0x040008E6 RID: 2278
	private ZipLoader cszl;

	// Token: 0x040008E7 RID: 2279
	private PlayerControl cspc;

	// Token: 0x040008E8 RID: 2280
	private Radar csr;

	// Token: 0x040008E9 RID: 2281
	private Block[] teamblock = new Block[8];

	// Token: 0x040008EA RID: 2282
	private BlockSet blockSet;

	// Token: 0x040008EB RID: 2283
	private Texture tex_inv;

	// Token: 0x040008EC RID: 2284
	private int last_block;

	// Token: 0x040008ED RID: 2285
	private Block selectedBlock;

	// Token: 0x040008EE RID: 2286
	private Block blocksel;

	// Token: 0x040008EF RID: 2287
	private GUIStyle gui_style;

	// Token: 0x040008F0 RID: 2288
	public static bool ForceCursor;

	// Token: 0x040008F1 RID: 2289
	private float g_buycheck;

	// Token: 0x040008F2 RID: 2290
	private bool g_canbuy = true;

	// Token: 0x040008F3 RID: 2291
	private TankController tc;

	// Token: 0x040008F4 RID: 2292
	private CarController cc;
}
