using System;
using UnityEngine;

// Token: 0x02000081 RID: 129
public class InventoryGUI : MonoBehaviour
{
	// Token: 0x060003A0 RID: 928 RVA: 0x0004461C File Offset: 0x0004281C
	private void Awake()
	{
		this.csr = (Radar)Object.FindObjectOfType(typeof(Radar));
		Map map = (Map)Object.FindObjectOfType(typeof(Map));
		this.cszl = (ZipLoader)Object.FindObjectOfType(typeof(ZipLoader));
		this.blockSet = map.GetBlockSet();
		this.blocksel = null;
		this.goMap = GameObject.Find("Map");
		this.gui_style = new GUIStyle();
		this.gui_style.font = FontManager.font[2];
		this.gui_style.fontSize = 14;
		this.gui_style.normal.textColor = new Color(1f, 1f, 1f, 1f);
		this.gui_style.alignment = TextAnchor.LowerCenter;
		this.blackTexture = new Texture2D(1, 1);
		this.blackTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.5f));
		this.blackTexture.Apply();
		this.yellowTexture = new Texture2D(1, 1);
		this.yellowTexture.SetPixel(0, 0, new Color(1f, 0.75f, 0.25f, 0.5f));
		this.yellowTexture.Apply();
		this.tex_shovel = (Resources.Load("GUI/shop/shovel") as Texture2D);
		this.tex_cube = (Resources.Load("GUI/shop/cube") as Texture2D);
		this.tex_medkit_w = (Resources.Load("GUI/shop/medkit_w") as Texture2D);
		this.tex_mp5 = (Resources.Load("GUI/shop/mp5") as Texture2D);
		this.tex_m3 = (Resources.Load("GUI/shop/m3") as Texture2D);
		this.tex_m14 = (Resources.Load("GUI/shop/m14") as Texture2D);
		this.tex_ak47 = (Resources.Load("GUI/shop/ak47") as Texture2D);
		this.tex_svd = (Resources.Load("GUI/shop/svd") as Texture2D);
		this.tex_glock = (Resources.Load("GUI/shop/glock") as Texture2D);
		this.tex_deagle = (Resources.Load("GUI/shop/deagle") as Texture2D);
		this.tex_asval = (Resources.Load("GUI/shop/asval") as Texture2D);
		this.tex_g36c = (Resources.Load("GUI/shop/g36c") as Texture2D);
		this.tex_kriss = (Resources.Load("GUI/shop/kriss") as Texture2D);
		this.tex_m4a1 = (Resources.Load("GUI/shop/m4a1") as Texture2D);
		this.tex_m249 = (Resources.Load("GUI/shop/m249") as Texture2D);
		this.tex_sps12 = (Resources.Load("GUI/shop/sps12") as Texture2D);
		this.tex_vintorez = (Resources.Load("GUI/shop/vintorez") as Texture2D);
		this.tex_vsk94 = (Resources.Load("GUI/shop/vsk94") as Texture2D);
		this.tex_usp = (Resources.Load("GUI/shop/usp") as Texture2D);
		this.tex_barrett = (Resources.Load("GUI/shop/barrett") as Texture2D);
		this.tex_tmp = (Resources.Load("GUI/shop/tmp") as Texture2D);
		this.tex_minigun = (Resources.Load("GUI/shop/minigun") as Texture2D);
		this.tex_knife = (Resources.Load("GUI/shop/knife") as Texture2D);
		this.tex_axe = (Resources.Load("GUI/shop/axe") as Texture2D);
		this.tex_bat = (Resources.Load("GUI/shop/bat") as Texture2D);
		this.tex_crowbar = (Resources.Load("GUI/shop/crowbar") as Texture2D);
		this.tex_caramel = (Resources.Load("GUI/shop/caramel") as Texture2D);
		this.tex_auga3 = (Resources.Load("GUI/shop/aug") as Texture2D);
		this.tex_sg552 = (Resources.Load("GUI/shop/sg552") as Texture2D);
		this.tex_m14ebr = (Resources.Load("GUI/shop/m14ebr") as Texture2D);
		this.tex_l96a1 = (Resources.Load("GUI/shop/l96a1") as Texture2D);
		this.tex_nova = (Resources.Load("GUI/shop/nova") as Texture2D);
		this.tex_kord = (Resources.Load("GUI/shop/kord") as Texture2D);
		this.tex_anaconda = (Resources.Load("GUI/shop/anaconda") as Texture2D);
		this.tex_scar = (Resources.Load("GUI/shop/scar") as Texture2D);
		this.tex_p90 = (Resources.Load("GUI/shop/p90") as Texture2D);
		this.tex_rpk = (Resources.Load("GUI/shop/rpk") as Texture2D);
		this.tex_hk416 = (Resources.Load("GUI/shop/hk416") as Texture2D);
		this.tex_ak102 = (Resources.Load("GUI/shop/ak102") as Texture2D);
		this.tex_sr25 = (Resources.Load("GUI/shop/sr25") as Texture2D);
		this.tex_mglmk1 = (Resources.Load("GUI/shop/mglmk1") as Texture2D);
		this.tex_mosin = (Resources.Load("GUI/shop/mosin") as Texture2D);
		this.tex_ppsh = (Resources.Load("GUI/shop/ppsh") as Texture2D);
		this.tex_mp40 = (Resources.Load("GUI/shop/mp40") as Texture2D);
		this.tex_l96a1mod = (Resources.Load("GUI/shop/l96a1mod") as Texture2D);
		this.tex_kacpdw = (Resources.Load("GUI/shop/kacpdw") as Texture2D);
		this.tex_famas = (Resources.Load("GUI/shop/famas") as Texture2D);
		this.tex_beretta = (Resources.Load("GUI/shop/beretta") as Texture2D);
		this.tex_machete = (Resources.Load("GUI/shop/machete") as Texture2D);
		this.tex_repair_tool = (Resources.Load("GUI/shop/repair tool") as Texture2D);
		this.tex_aa12 = (Resources.Load("GUI/shop/aa12") as Texture2D);
		this.tex_fn57 = (Resources.Load("GUI/shop/fn57") as Texture2D);
		this.tex_fs2000 = (Resources.Load("GUI/shop/fs2000") as Texture2D);
		this.tex_l85 = (Resources.Load("GUI/shop/l85") as Texture2D);
		this.tex_mac10 = (Resources.Load("GUI/shop/mac10") as Texture2D);
		this.tex_pkp = (Resources.Load("GUI/shop/pkp") as Texture2D);
		this.tex_pm = (Resources.Load("GUI/shop/pm") as Texture2D);
		this.tex_tar21 = (Resources.Load("GUI/shop/tar21") as Texture2D);
		this.tex_ump45 = (Resources.Load("GUI/shop/ump45") as Texture2D);
		this.tex_ntw20 = (Resources.Load("GUI/shop/ntw20") as Texture2D);
		this.tex_vintorez_desert = (Resources.Load("GUI/shop/vintorez_desert") as Texture2D);
		this.tex_tank_default = (Resources.Load("GUI/shop/tank_default") as Texture2D);
		this.tex_tank_light = (Resources.Load("GUI/shop/tank_light") as Texture2D);
		this.tex_tank_heavy = (Resources.Load("GUI/shop/tank_heavy") as Texture2D);
		this.tex_zaa12 = (Resources.Load("GUI/shop/zaa12") as Texture2D);
		this.tex_zasval = (Resources.Load("GUI/shop/zasval") as Texture2D);
		this.tex_zfn57 = (Resources.Load("GUI/shop/zfn57") as Texture2D);
		this.tex_zkord = (Resources.Load("GUI/shop/zkord") as Texture2D);
		this.tex_zm249 = (Resources.Load("GUI/shop/zm249") as Texture2D);
		this.tex_zminigun = (Resources.Load("GUI/shop/zminigun") as Texture2D);
		this.tex_zsps12 = (Resources.Load("GUI/shop/zsps12") as Texture2D);
		this.teamblock[0] = this.blockSet.GetBlock("Brick_blue");
		this.teamblock[1] = this.blockSet.GetBlock("Brick_red");
		this.teamblock[2] = this.blockSet.GetBlock("Brick_green");
		this.teamblock[3] = this.blockSet.GetBlock("Brick_yellow");
		this.tex_inv = (Resources.Load("GUI/inv") as Texture2D);
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x00044DB8 File Offset: 0x00042FB8
	private void Update()
	{
		if (this.tc == null)
		{
			this.tc = (TankController)Object.FindObjectOfType(typeof(TankController));
		}
		if (this.tc != null && this.tc.enabled)
		{
			this.g_canbuy = false;
		}
		if (Input.GetKeyDown(KeyCode.E) && this.g_canbuy)
		{
			this.show = !this.show;
			MainGUI.ForceCursor = this.show;
		}
		if (Time.time > this.g_buycheck)
		{
			if (this.cspc == null)
			{
				this.cspc = (PlayerControl)Object.FindObjectOfType(typeof(PlayerControl));
			}
			this.g_canbuy = false;
			this.g_buycheck += 0.5f;
		}
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x00044E88 File Offset: 0x00043088
	private void OnGUI()
	{
		if (this.show && !this.g_canbuy)
		{
			this.show = !this.show;
			MainGUI.ForceCursor = this.show;
			return;
		}
		if (this.g_canbuy)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width - 40), (float)(Screen.height - 220), 32f, 32f), this.tex_inv);
		}
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x00044EF8 File Offset: 0x000430F8
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
				this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
			}
			if (num > 0)
			{
				this.cscl.send_selectblock((byte)num);
			}
		}
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x00044FB8 File Offset: 0x000431B8
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
				if (block != null && block.GetName() != null && (block.GetName() == "Brick_blue" || block.GetName() == "Brick_red" || block.GetName() == "Brick_green" || block.GetName() == "Brick_yellow" || block.GetName() == "!Water" || block.GetName() == "TNT"))
				{
					j--;
				}
				else if (InventoryGUI.DrawBlock(block, block == selected && selected != null))
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

	// Token: 0x060003A5 RID: 933 RVA: 0x000450C0 File Offset: 0x000432C0
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

	// Token: 0x060003A6 RID: 934 RVA: 0x00045134 File Offset: 0x00043334
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

	// Token: 0x060003A7 RID: 935 RVA: 0x00045268 File Offset: 0x00043468
	private void InventoryBattle()
	{
		if (this.cscl == null)
		{
			this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		int myindex = this.cscl.myindex;
		int num = 0;
		GUIManager.DrawText(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height / 2f - 200f), Lang.GetLabel(128), 20, TextAnchor.LowerCenter, 8);
		this.x_pos = 24f;
		this.y_pos = (float)Screen.height / 2f - 200f + 4f;
		GUI.DrawTexture(new Rect(0f, (float)Screen.height / 2f - 200f, (float)Screen.width, 72f), this.blackTexture);
		this.DrawItem(this.tex_m14, "M14", 2, 3, num);
		num++;
		this.x_pos = 24f;
		this.y_pos += 84f;
		GUI.DrawTexture(new Rect(0f, this.y_pos - 4f, (float)Screen.width, 72f), this.blackTexture);
		this.DrawItem(this.tex_mp5, "MP5", 0, 4, num);
		num++;
		this.x_pos = 24f;
		this.y_pos += 84f;
		GUI.DrawTexture(new Rect(0f, this.y_pos - 4f, (float)Screen.width, 72f), this.blackTexture);
		this.DrawItem(this.tex_m3, "M3", 1, 2, num);
		num++;
		if (RemotePlayersUpdater.Instance.Bots[myindex].Item[8] == 1 || RemotePlayersUpdater.Instance.Bots[myindex].Item[20] == 1 || RemotePlayersUpdater.Instance.Bots[myindex].Item[34] == 1 || RemotePlayersUpdater.Instance.Bots[myindex].Item[48] == 1 || RemotePlayersUpdater.Instance.Bots[myindex].Item[52] == 1 || RemotePlayersUpdater.Instance.Bots[myindex].Item[57] == 1)
		{
			this.x_pos = 24f;
			this.y_pos += 84f;
			GUI.DrawTexture(new Rect(0f, this.y_pos - 4f, (float)Screen.width, 72f), this.blackTexture);
			this.DrawItem(this.tex_glock, "GLOCK", 0, 9, num);
			num++;
		}
		if (RemotePlayersUpdater.Instance.Bots[myindex].Item[23] == 1 || RemotePlayersUpdater.Instance.Bots[myindex].Item[24] == 1 || RemotePlayersUpdater.Instance.Bots[myindex].Item[25] == 1 || RemotePlayersUpdater.Instance.Bots[myindex].Item[26] == 1 || RemotePlayersUpdater.Instance.Bots[myindex].Item[27] == 1 || RemotePlayersUpdater.Instance.Bots[myindex].Item[49] == 1 || RemotePlayersUpdater.Instance.Bots[myindex].Item[50] == 1)
		{
			this.x_pos = 24f;
			this.y_pos += 84f;
			GUI.DrawTexture(new Rect(0f, this.y_pos - 4f, (float)Screen.width, 72f), this.blackTexture);
			this.DrawItem(this.tex_shovel, Lang.GetLabel(166), 0, 1, num);
			num++;
		}
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x00045610 File Offset: 0x00043810
	private void InventoryCarnage()
	{
		if (this.cscl == null)
		{
			this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		int myindex = this.cscl.myindex;
		int num = 0;
		GUIManager.DrawText(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height / 2f - 120f), Lang.GetLabel(128), 20, TextAnchor.LowerCenter, 8);
		float num2 = (float)Screen.height / 2f - 120f;
		GUI.DrawTexture(new Rect(0f, num2 + 4f, (float)Screen.width, 72f), this.blackTexture);
		this.x_pos = 24f;
		this.y_pos = num2 + 4f;
		this.DrawItem(this.tex_shovel, Lang.GetLabel(166), 0, 1, num);
		num++;
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x000456FC File Offset: 0x000438FC
	private void InventoryClassic()
	{
		if (this.cscl == null)
		{
			this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		int myindex = this.cscl.myindex;
		GUIManager.DrawText(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height / 2f - 120f), Lang.GetLabel(128), 20, TextAnchor.LowerCenter, 8);
		float num = (float)Screen.height / 2f - 120f;
		GUI.DrawTexture(new Rect(0f, num + 4f, (float)Screen.width, 72f), this.blackTexture);
		this.x_pos = 24f;
		this.y_pos = num + 4f;
	}

	// Token: 0x060003AA RID: 938 RVA: 0x000457CC File Offset: 0x000439CC
	private void DrawItem(Texture2D tex, string name, int cid, int wid, int id)
	{
		if (this.x_pos + 70f > (float)Screen.width)
		{
			this.x_pos = 24f;
			this.y_pos += 76f;
			GUI.DrawTexture(new Rect(0f, this.y_pos - 4f, (float)Screen.width, 72f), this.blackTexture);
		}
		float x = Input.mousePosition.x;
		float y = (float)Screen.height - Input.mousePosition.y;
		float num = this.x_pos;
		float num2 = this.y_pos;
		Rect position = new Rect(num, num2, 64f, 64f);
		Rect position2 = new Rect(num + 1f, num2 + 1f, 64f, 64f);
		if (position.Contains(new Vector2(x, y)))
		{
			if (!this.hover[id])
			{
				this.hover[id] = true;
			}
		}
		else if (this.hover[id])
		{
			this.hover[id] = false;
		}
		this.x_pos += 70f;
		if (this.hover[id])
		{
			position = new Rect(num, num2 - 2f, 64f, 64f);
			position2 = new Rect(num + 1f, num2 + 1f - 2f, 65f, 65f);
		}
		if (this.hover[id])
		{
			GUI.DrawTexture(position, this.yellowTexture);
		}
		GUI.DrawTexture(position, tex);
		this.gui_style.normal.textColor = new Color(0f, 0f, 0f, 1f);
		GUI.Label(position2, name, this.gui_style);
		this.gui_style.normal.textColor = new Color(1f, 1f, 1f, 1f);
		GUI.Label(position, name, this.gui_style);
		if (GUI.Button(position, "", this.gui_style))
		{
			this.show = !this.show;
			MainGUI.ForceCursor = this.show;
			if (this.cscl == null)
			{
				this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
			}
			int myindex = this.cscl.myindex;
			byte team = RemotePlayersUpdater.Instance.Bots[myindex].Team;
			if (wid < 100)
			{
				this.cscl.send_jointeamclass(team, (byte)cid);
				return;
			}
			this.cscl.send_spawn_my_vehicle(wid);
		}
	}

	// Token: 0x040007B0 RID: 1968
	private BlockSet blockSet;

	// Token: 0x040007B1 RID: 1969
	private Block[] teamblock = new Block[4];

	// Token: 0x040007B2 RID: 1970
	private bool show;

	// Token: 0x040007B3 RID: 1971
	private Vector2 scrollPosition = Vector3.zero;

	// Token: 0x040007B4 RID: 1972
	private GameObject goMap;

	// Token: 0x040007B5 RID: 1973
	private GameObject goPlayer;

	// Token: 0x040007B6 RID: 1974
	private GameObject block_face;

	// Token: 0x040007B7 RID: 1975
	private GameObject block_top;

	// Token: 0x040007B8 RID: 1976
	private Client cscl;

	// Token: 0x040007B9 RID: 1977
	private ZipLoader cszl;

	// Token: 0x040007BA RID: 1978
	private PlayerControl cspc;

	// Token: 0x040007BB RID: 1979
	private Radar csr;

	// Token: 0x040007BC RID: 1980
	private GUIStyle gui_style;

	// Token: 0x040007BD RID: 1981
	public Texture2D tex_cube;

	// Token: 0x040007BE RID: 1982
	public Texture2D tex_medkit_w;

	// Token: 0x040007BF RID: 1983
	public Texture2D tex_mp5;

	// Token: 0x040007C0 RID: 1984
	public Texture2D tex_m3;

	// Token: 0x040007C1 RID: 1985
	public Texture2D tex_m14;

	// Token: 0x040007C2 RID: 1986
	public Texture2D tex_ak47;

	// Token: 0x040007C3 RID: 1987
	public Texture2D tex_svd;

	// Token: 0x040007C4 RID: 1988
	public Texture2D tex_glock;

	// Token: 0x040007C5 RID: 1989
	public Texture2D tex_deagle;

	// Token: 0x040007C6 RID: 1990
	public Texture2D tex_asval;

	// Token: 0x040007C7 RID: 1991
	public Texture2D tex_g36c;

	// Token: 0x040007C8 RID: 1992
	public Texture2D tex_kriss;

	// Token: 0x040007C9 RID: 1993
	public Texture2D tex_m4a1;

	// Token: 0x040007CA RID: 1994
	public Texture2D tex_m249;

	// Token: 0x040007CB RID: 1995
	public Texture2D tex_sps12;

	// Token: 0x040007CC RID: 1996
	public Texture2D tex_vintorez;

	// Token: 0x040007CD RID: 1997
	public Texture2D tex_vsk94;

	// Token: 0x040007CE RID: 1998
	public Texture2D tex_usp;

	// Token: 0x040007CF RID: 1999
	public Texture2D tex_barrett;

	// Token: 0x040007D0 RID: 2000
	public Texture2D tex_tmp;

	// Token: 0x040007D1 RID: 2001
	public Texture2D tex_shovel;

	// Token: 0x040007D2 RID: 2002
	public Texture2D tex_knife;

	// Token: 0x040007D3 RID: 2003
	public Texture2D tex_axe;

	// Token: 0x040007D4 RID: 2004
	public Texture2D tex_bat;

	// Token: 0x040007D5 RID: 2005
	public Texture2D tex_crowbar;

	// Token: 0x040007D6 RID: 2006
	public Texture2D tex_caramel;

	// Token: 0x040007D7 RID: 2007
	public Texture2D tex_auga3;

	// Token: 0x040007D8 RID: 2008
	public Texture2D tex_sg552;

	// Token: 0x040007D9 RID: 2009
	public Texture2D tex_m14ebr;

	// Token: 0x040007DA RID: 2010
	public Texture2D tex_l96a1;

	// Token: 0x040007DB RID: 2011
	public Texture2D tex_kord;

	// Token: 0x040007DC RID: 2012
	public Texture2D tex_nova;

	// Token: 0x040007DD RID: 2013
	public Texture2D tex_p90;

	// Token: 0x040007DE RID: 2014
	public Texture2D tex_scar;

	// Token: 0x040007DF RID: 2015
	public Texture2D tex_anaconda;

	// Token: 0x040007E0 RID: 2016
	public Texture2D tex_rpk;

	// Token: 0x040007E1 RID: 2017
	public Texture2D tex_hk416;

	// Token: 0x040007E2 RID: 2018
	public Texture2D tex_ak102;

	// Token: 0x040007E3 RID: 2019
	public Texture2D tex_sr25;

	// Token: 0x040007E4 RID: 2020
	public Texture2D tex_mglmk1;

	// Token: 0x040007E5 RID: 2021
	public Texture2D tex_mosin;

	// Token: 0x040007E6 RID: 2022
	public Texture2D tex_ppsh;

	// Token: 0x040007E7 RID: 2023
	public Texture2D tex_mp40;

	// Token: 0x040007E8 RID: 2024
	public Texture2D tex_l96a1mod;

	// Token: 0x040007E9 RID: 2025
	public Texture2D tex_kacpdw;

	// Token: 0x040007EA RID: 2026
	public Texture2D tex_famas;

	// Token: 0x040007EB RID: 2027
	public Texture2D tex_beretta;

	// Token: 0x040007EC RID: 2028
	public Texture2D tex_machete;

	// Token: 0x040007ED RID: 2029
	public Texture2D tex_repair_tool;

	// Token: 0x040007EE RID: 2030
	public Texture2D tex_aa12;

	// Token: 0x040007EF RID: 2031
	public Texture2D tex_fn57;

	// Token: 0x040007F0 RID: 2032
	public Texture2D tex_fs2000;

	// Token: 0x040007F1 RID: 2033
	public Texture2D tex_l85;

	// Token: 0x040007F2 RID: 2034
	public Texture2D tex_mac10;

	// Token: 0x040007F3 RID: 2035
	public Texture2D tex_pkp;

	// Token: 0x040007F4 RID: 2036
	public Texture2D tex_pm;

	// Token: 0x040007F5 RID: 2037
	public Texture2D tex_tar21;

	// Token: 0x040007F6 RID: 2038
	public Texture2D tex_ump45;

	// Token: 0x040007F7 RID: 2039
	public Texture2D tex_ntw20;

	// Token: 0x040007F8 RID: 2040
	public Texture2D tex_vintorez_desert;

	// Token: 0x040007F9 RID: 2041
	public Texture2D tex_tank_default;

	// Token: 0x040007FA RID: 2042
	public Texture2D tex_tank_light;

	// Token: 0x040007FB RID: 2043
	public Texture2D tex_tank_heavy;

	// Token: 0x040007FC RID: 2044
	public Texture2D tex_minigun;

	// Token: 0x040007FD RID: 2045
	public Texture2D tex_zaa12;

	// Token: 0x040007FE RID: 2046
	public Texture2D tex_zasval;

	// Token: 0x040007FF RID: 2047
	public Texture2D tex_zfn57;

	// Token: 0x04000800 RID: 2048
	public Texture2D tex_zkord;

	// Token: 0x04000801 RID: 2049
	public Texture2D tex_zm249;

	// Token: 0x04000802 RID: 2050
	public Texture2D tex_zminigun;

	// Token: 0x04000803 RID: 2051
	public Texture2D tex_zsps12;

	// Token: 0x04000804 RID: 2052
	private Texture2D blackTexture;

	// Token: 0x04000805 RID: 2053
	private Texture2D yellowTexture;

	// Token: 0x04000806 RID: 2054
	private Texture2D tex_inv;

	// Token: 0x04000807 RID: 2055
	private float x_pos;

	// Token: 0x04000808 RID: 2056
	private float y_pos;

	// Token: 0x04000809 RID: 2057
	private bool[] hover = new bool[128];

	// Token: 0x0400080A RID: 2058
	private Block selectedBlock;

	// Token: 0x0400080B RID: 2059
	private Block blocksel;

	// Token: 0x0400080C RID: 2060
	private TankController tc;

	// Token: 0x0400080D RID: 2061
	private float g_buycheck;

	// Token: 0x0400080E RID: 2062
	private bool g_canbuy;
}
