using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000088 RID: 136
public class Radar : MonoBehaviour
{
	// Token: 0x060003CD RID: 973 RVA: 0x0004A544 File Offset: 0x00048744
	private void Awake()
	{
		this.RadarTexture = new Texture2D(256, 256);
		Color color = new Color(0f, 0.5f, 0f, 0.5f);
		for (int i = 0; i < 256; i++)
		{
			for (int j = 0; j < 256; j++)
			{
				this.RadarTexture.SetPixel(i, j, color);
			}
		}
		this.RadarTexture.Apply();
		this.RadarTexture.filterMode = FilterMode.Point;
		this.ZombiePosition = (Resources.Load("GUI/radar_zpos") as Texture2D);
		this.PlayerPosition = (Resources.Load("GUI/radar_pos") as Texture2D);
		this.TeamPosition = (Resources.Load("GUI/radar_team") as Texture2D);
		this.BasePosition = (Resources.Load("GUI/radar_base") as Texture2D);
		this.FlagPosition = new Texture2D[2];
		this.FlagPosition[0] = (Resources.Load("GUI/flag_blue") as Texture2D);
		this.FlagPosition[1] = (Resources.Load("GUI/flag_red") as Texture2D);
		this.map = (Map)Object.FindObjectOfType(typeof(Map));
		this.gui_style = new GUIStyle();
		this.gui_style.fontSize = 10;
		this.gui_style.alignment = TextAnchor.MiddleCenter;
		this.gui_style.normal.textColor = new Color(255f, 255f, 255f, 150f);
		this.tmpTex = new Texture2D(32, 32);
		this.blockset = this.map.GetBlockSet();
		this.blockTex = (this.blockset.GetAtlas(0).GetTexture() as Texture2D);
		for (int k = 0; k < 4; k++)
		{
			this._col[k] = Color.white;
			this.oldFlagScores[k] = 150;
			this.mig[k] = false;
		}
	}

	// Token: 0x060003CE RID: 974 RVA: 0x0004A72C File Offset: 0x0004892C
	private void OnGUI()
	{
		if (this.cscl == null)
		{
			this.cscl = Object.FindObjectOfType<Client>();
		}
		if (this.LocalPlayer == null)
		{
			this.LocalPlayer = GameObject.Find("Player");
		}
		if (this.cscl == null)
		{
			return;
		}
		GUI.DrawTexture(new Rect(GUIManager.XRES(4f), GUIManager.YRES(4f), GUIManager.YRES(128f), GUIManager.YRES(128f)), this.RadarTexture);
		if (this.RadarTexture2)
		{
			GUI.DrawTexture(new Rect(GUIManager.XRES(4f), GUIManager.YRES(4f), GUIManager.YRES(128f), GUIManager.YRES(128f)), this.RadarTexture2);
		}
		this.DrawBase(this.base_pos.x, this.base_pos.z);
		if (CONST.GetGameMode() == MODE.BATTLE)
		{
			GUI.color = Color.blue;
			this.DrawTeam(this.team_pos[0].x, this.team_pos[0].z);
			GUI.color = Color.red;
			this.DrawTeam(this.team_pos[1].x, this.team_pos[1].z);
			GUI.color = Color.green;
			this.DrawTeam(this.team_pos[2].x, this.team_pos[2].z);
			GUI.color = Color.yellow;
			this.DrawTeam(this.team_pos[3].x, this.team_pos[3].z);
		}
		if (CONST.GetGameMode() != MODE.FFA)
		{
			for (int i = 0; i < 32; i++)
			{
				if (i != this.cscl.myindex && RemotePlayersUpdater.Instance.Bots[i].Active && RemotePlayersUpdater.Instance.Bots[i].Team != 255 && RemotePlayersUpdater.Instance.Bots[i].Dead != 1)
				{
					this.UpdPlayer(i);
				}
			}
		}
		if (CONST.GetGameMode() == MODE.PRORIV)
		{
			for (int j = 0; j < 4; j++)
			{
				if (!(this.map == null) && this.map.flags[j].inited)
				{
					int num = 1;
					if (this.map.flags[j].timer[0] > this.map.flags[j].timer[1])
					{
						num = 0;
					}
					if (this.oldFlagScores[j] != this.map.flags[j].timer[num])
					{
						this.mig[j] = true;
					}
					if (this.mig[j])
					{
						if (this.oldFlagScores[j] != this.map.flags[j].timer[num])
						{
							this._col[j] = new Color(this._col[j].r, this._col[j].g, this._col[j].b, this._col[j].a - 0.02f);
						}
						else
						{
							this._col[j] = new Color(this._col[j].r, this._col[j].g, this._col[j].b, this._col[j].a + 0.02f);
						}
						if (this._col[j].a <= 0.5f)
						{
							this.oldFlagScores[j] = this.map.flags[j].timer[num];
						}
						if (this._col[j].a >= 1f)
						{
							this._col[j].a = 1f;
							this.mig[j] = false;
						}
					}
					GUI.color = this._col[j];
					this.DrawFlag(num, (float)(this.map.flags[j].pos.x + 6), (float)(this.map.flags[j].pos.z + 16));
					GUI.color = Color.white;
				}
			}
		}
		Vector3 position = this.LocalPlayer.transform.position;
		Vector3 eulerAngles = this.LocalPlayer.transform.eulerAngles;
		this.DP(position.x, position.z, eulerAngles.y, true, false);
		if (CONST.GetGameMode() == MODE.BUILD)
		{
			GUI.Label(new Rect(GUIManager.XRES(4f), GUIManager.YRES(132f), 200f, 20f), string.Concat(new string[]
			{
				"X: ",
				position.x.ToString("0"),
				" Y: ",
				position.y.ToString("0"),
				" Z: ",
				position.z.ToString("0")
			}));
		}
	}

	// Token: 0x060003CF RID: 975 RVA: 0x0004AC68 File Offset: 0x00048E68
	private void DP(float px, float pz, float ry, bool self, bool zombie)
	{
		if (self)
		{
			GUI.color = Color.yellow;
		}
		else if (zombie)
		{
			GUI.color = Color.red;
		}
		else
		{
			GUI.color = Color.white;
		}
		Rect position = new Rect(0f, 0f, GUIManager.YRES(16f), GUIManager.YRES(16f));
		position.center = new Vector2(GUIManager.XRES(4f) + GUIManager.YRES(px / 2f), GUIManager.YRES(4f) + GUIManager.YRES(128f - pz / 2f));
		float num = ry - 90f;
		if (num > 360f)
		{
			num -= 360f;
		}
		if (num < 0f)
		{
			num += 360f;
		}
		Matrix4x4 matrix = GUI.matrix;
		GUIUtility.RotateAroundPivot(num, position.center);
		if (zombie)
		{
			GUI.DrawTexture(position, this.ZombiePosition);
		}
		else
		{
			GUI.DrawTexture(position, this.PlayerPosition);
		}
		GUI.matrix = matrix;
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x0004AD64 File Offset: 0x00048F64
	public void UpdateRadarColor(byte[,] data)
	{
		if (this.RadarTexture2 == null)
		{
			this.RadarTexture2 = new Texture2D(256, 256);
		}
		this.team_dot[0] = 0;
		this.team_dot[1] = 0;
		this.team_dot[2] = 0;
		this.team_dot[3] = 0;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				Color color = new Color(1f, 1f, 1f, 0.75f);
				if (data[i, j] == 255)
				{
					color = new Color(0f, 0f, 0f, 0f);
				}
				else if (data[i, j] == 0)
				{
					color = new Color(0f, 0f, 1f, 0.75f);
					this.team_dot[0]++;
				}
				else if (data[i, j] == 1)
				{
					color = new Color(1f, 0f, 0f, 0.75f);
					this.team_dot[1]++;
				}
				else if (data[i, j] == 2)
				{
					color = new Color(0f, 1f, 0f, 0.75f);
					this.team_dot[2]++;
				}
				else if (data[i, j] == 3)
				{
					color = new Color(1f, 1f, 0f, 0.75f);
					this.team_dot[3]++;
				}
				for (int k = i * 64; k < (i + 1) * 64; k++)
				{
					for (int l = j * 64; l < (j + 1) * 64; l++)
					{
						if (k == 0 || k == 63 || k == 127 || k == 191 || k == 255)
						{
							this.RadarTexture2.SetPixel(k, l, new Color(0f, 0f, 0f, 0f));
						}
						else if (l == 0 || l == 63 || l == 127 || l == 191 || l == 255)
						{
							this.RadarTexture2.SetPixel(k, l, new Color(0f, 0f, 0f, 0f));
						}
						else
						{
							this.RadarTexture2.SetPixel(k, l, color);
						}
					}
				}
			}
		}
		this.RadarTexture2.Apply();
		this.team_dot_all = this.team_dot[0] + this.team_dot[1] + this.team_dot[2] + this.team_dot[3];
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x0004B016 File Offset: 0x00049216
	public void ForceUpdateRadar()
	{
		base.StartCoroutine(this.UpdateRadar());
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x0004B025 File Offset: 0x00049225
	private IEnumerator UpdateRadar()
	{
		for (int i = 0; i < 256; i++)
		{
			for (int j = 0; j < 256; j++)
			{
				for (int k = 63; k > 0; k--)
				{
					BlockData block = this.map.GetBlock(i, k, j);
					if (!block.IsEmpty() && block.block.GetName() != null)
					{
						Color color = this.GetColor(block.block);
						this.RadarTexture.SetPixel(i, j, color);
						break;
					}
				}
			}
		}
		yield return null;
		this.RadarTexture.Apply();
		yield break;
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x0004B034 File Offset: 0x00049234
	public Color GetColor(Block block)
	{
		for (int i = 0; i < this.colorlist.Count; i++)
		{
			if (this.colorlist[i].name == block.GetName())
			{
				return this.colorlist[i].color;
			}
		}
		Rect topFace = block.GetTopFace();
		if (Config.Tileset > 2)
		{
			this.tmpTex.SetPixels(this.blockTex.GetPixels((int)(topFace.x * 1024f), (int)(topFace.y * 1024f), 64, 64));
		}
		else
		{
			this.tmpTex.SetPixels(this.blockTex.GetPixels((int)(topFace.x * 512f), (int)(topFace.y * 512f), 32, 32));
		}
		this.tmpTex.filterMode = FilterMode.Point;
		this.tmpTex.Apply();
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		foreach (Color color in this.tmpTex.GetPixels())
		{
			num += color.r;
			num2 += color.g;
			num3 += color.b;
		}
		this.tmpColor = new Color(num / 1024f, num2 / 1024f, num3 / 1024f, 1f);
		this.colorlist.Add(new Radar.CBlockColor(block.GetName(), this.tmpColor));
		return this.tmpColor;
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x0004B1C4 File Offset: 0x000493C4
	private void DrawBase(float px, float pz)
	{
		GUI.DrawTexture(new Rect(0f, 0f, GUIManager.YRES(20f), GUIManager.YRES(20f))
		{
			center = new Vector2(GUIManager.XRES(4f) + GUIManager.YRES(px / 2f), GUIManager.YRES(4f) + GUIManager.YRES(128f - pz / 2f))
		}, this.BasePosition);
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x0004B244 File Offset: 0x00049444
	private void DrawTeam(float px, float pz)
	{
		GUI.DrawTexture(new Rect(0f, 0f, GUIManager.YRES(8f), GUIManager.YRES(8f))
		{
			center = new Vector2(GUIManager.XRES(4f) + GUIManager.YRES(px / 2f), GUIManager.YRES(4f) + GUIManager.YRES(128f - pz / 2f))
		}, this.TeamPosition);
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x0004B2C4 File Offset: 0x000494C4
	private void DrawFlag(int _team, float px, float pz)
	{
		GUI.DrawTexture(new Rect(0f, 0f, GUIManager.YRES(12f), GUIManager.YRES(12f))
		{
			center = new Vector2(GUIManager.XRES(6f) + GUIManager.YRES(px / 2f), GUIManager.YRES(6f) + GUIManager.YRES(128f - pz / 2f))
		}, this.FlagPosition[_team]);
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x000445E0 File Offset: 0x000427E0
	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
	{
		return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * 57.29578f;
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x0004B344 File Offset: 0x00049544
	public void GenerateSideTexture()
	{
		if (this.RadarSideTexture == null)
		{
			this.RadarSideTexture = new Texture2D(256, 64);
			this.RadarSideTexture.filterMode = FilterMode.Point;
		}
		Color color = new Color(0f, 0.5f, 0f, 0.5f);
		for (int i = 0; i < 256; i++)
		{
			for (int j = 0; j < 64; j++)
			{
				this.RadarSideTexture.SetPixel(i, j, color);
			}
		}
		for (int k = 0; k < 256; k++)
		{
			for (int l = 0; l < 64; l++)
			{
				for (int m = 0; m < 256; m++)
				{
					BlockData block = this.map.GetBlock(k, l, m);
					if (!block.IsEmpty() && block.block.GetName() != null)
					{
						color = new Color(0f, 0f, 0f, 0f);
						if (block.block.GetName() == "Stoneend")
						{
							color = new Color(0f, 0f, 0f, 1f);
						}
						else if (block.block.GetName() == "Dirt")
						{
							color = new Color(0.5f, 0.25f, 0f, 1f);
						}
						else if (block.block.GetName() == "Grass")
						{
							color = new Color(0f, 0.5f, 0f, 1f);
						}
						else if (block.block.GetName() == "Snow")
						{
							color = new Color(1f, 1f, 1f, 1f);
						}
						else if (block.block.GetName() == "Sand")
						{
							color = new Color(1f, 0.75f, 0.5f, 1f);
						}
						else if (block.block.GetName() == "Stone")
						{
							color = new Color(0.5f, 0.5f, 0.5f, 1f);
						}
						else if (block.block.GetName() == "!Water")
						{
							color = new Color(0f, 0.5f, 1f, 1f);
						}
						else if (block.block.GetName() == "Wood")
						{
							color = new Color(0.75f, 0.4f, 0.1f, 1f);
						}
						else if (block.block.GetName() == "Wood2")
						{
							color = new Color(0.5f, 0.3f, 0.1f, 1f);
						}
						else if (block.block.GetName() == "Leaf")
						{
							color = new Color(0f, 0.8f, 0f, 1f);
						}
						else if (block.block.GetName() == "Brick")
						{
							color = new Color(1f, 0.2f, 0.2f, 1f);
						}
						else if (block.block.GetName() == "Brick_blue")
						{
							color = new Color(0f, 0f, 0.5f, 1f);
						}
						else if (block.block.GetName() == "Brick_red")
						{
							color = new Color(0.5f, 0f, 0f, 1f);
						}
						else if (block.block.GetName() == "Brick_green")
						{
							color = new Color(0f, 0.5f, 0f, 1f);
						}
						else if (block.block.GetName() == "Brick_yellow")
						{
							color = new Color(0.5f, 0.5f, 0f, 1f);
						}
						else if (block.block.GetName() == "Window")
						{
							color = new Color(0.13f, 0.66f, 0.86f, 1f);
						}
						else if (block.block.GetName() == "Box")
						{
							color = new Color(0.6f, 0.46f, 0.6f, 1f);
						}
						else if (block.block.GetName() == "Brick2")
						{
							color = new Color(0.46f, 0.46f, 0.46f, 1f);
						}
						else if (block.block.GetName() == "Stone2")
						{
							color = new Color(0.46f, 0.53f, 0.46f, 1f);
						}
						else if (block.block.GetName() == "Stone3")
						{
							color = new Color(0.53f, 0.53f, 0.4f, 1f);
						}
						else if (block.block.GetName() == "Stone4")
						{
							color = new Color(0.46f, 0.2f, 0.06f, 1f);
						}
						else if (block.block.GetName() == "Tile")
						{
							color = new Color(0.6f, 0.33f, 0.13f, 1f);
						}
						else if (block.block.GetName() == "Stone5")
						{
							color = new Color(0.8f, 0.53f, 0.2f, 1f);
						}
						else if (block.block.GetName() == "Sand2")
						{
							color = new Color(1f, 0.73f, 0.46f, 1f);
						}
						else if (block.block.GetName() == "Stone6")
						{
							color = new Color(0.53f, 0.46f, 0.46f, 1f);
						}
						else if (block.block.GetName() == "Metall1")
						{
							color = new Color(0.8f, 0.4f, 0.33f, 1f);
						}
						else if (block.block.GetName() == "Metall2")
						{
							color = new Color(0.53f, 0.66f, 0.6f, 1f);
						}
						else if (block.block.GetName() == "Stone7")
						{
							color = new Color(0.8f, 0.8f, 0.8f, 1f);
						}
						else if (block.block.GetName() == "Stone8")
						{
							color = new Color(0.8f, 0.8f, 0.8f, 1f);
						}
						else if (block.block.GetName() == "R_b_blue")
						{
							color = new Color(0f, 0f, 0f, 1f);
						}
						else if (block.block.GetName() == "R_b_red")
						{
							color = new Color(0f, 0f, 0f, 1f);
						}
						else if (block.block.GetName() == "R_b_green")
						{
							color = new Color(0f, 0f, 0f, 1f);
						}
						else if (block.block.GetName() == "R_b_yellow")
						{
							color = new Color(0f, 0f, 0f, 1f);
						}
						else if (block.block.GetName() == "R_z")
						{
							color = new Color(0f, 0f, 0f, 1f);
						}
						else if (block.block.GetName() == "R_c_blue")
						{
							color = new Color(0f, 0f, 0f, 1f);
						}
						else if (block.block.GetName() == "R_c_red")
						{
							color = new Color(0f, 0f, 0f, 1f);
						}
						else if (block.block.GetName() == "R_center")
						{
							color = new Color(0f, 0f, 0f, 1f);
						}
						this.RadarSideTexture.SetPixel(k, l, color);
						break;
					}
				}
			}
		}
		this.RadarSideTexture.Apply();
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x0004BC80 File Offset: 0x00049E80
	private void UpdPlayer(int i)
	{
		if (!RemotePlayersUpdater.Instance.Bots[this.cscl.myindex].zombie && RemotePlayersUpdater.Instance.Bots[i].Team != RemotePlayersUpdater.Instance.Bots[this.cscl.myindex].Team)
		{
			return;
		}
		Vector3 position = RemotePlayersUpdater.Instance.BotsGmObj[i].transform.position;
		Vector3 eulerAngles = RemotePlayersUpdater.Instance.BotsGmObj[i].transform.eulerAngles;
		if (RemotePlayersUpdater.Instance.Bots[this.cscl.myindex].zombie && !RemotePlayersUpdater.Instance.Bots[i].zombie)
		{
			this.DP(position.x, position.z, eulerAngles.y, false, true);
			return;
		}
		this.DP(position.x, position.z, eulerAngles.y, false, false);
		this.DTPNTmp(position, RemotePlayersUpdater.Instance.Bots[i].Name);
	}

	// Token: 0x060003DA RID: 986 RVA: 0x0004BD84 File Offset: 0x00049F84
	private void DTPNTmp(Vector3 p, string name)
	{
		p.y += 2.5f;
		Vector3 vector = Camera.main.WorldToScreenPoint(p);
		vector.y = (float)Screen.height - vector.y;
		if (Vector3.Angle(p - this.LocalPlayer.transform.position, this.LocalPlayer.transform.forward) < 90f && vector.z > 0f)
		{
			GUI.Label(new Rect(vector.x - 100f, vector.y, 200f, 20f), name, this.gui_style);
		}
	}

	// Token: 0x04000849 RID: 2121
	public bool canupdate = true;

	// Token: 0x0400084A RID: 2122
	public Texture2D RadarTexture;

	// Token: 0x0400084B RID: 2123
	private Texture2D RadarTexture2;

	// Token: 0x0400084C RID: 2124
	private Texture2D ZombiePosition;

	// Token: 0x0400084D RID: 2125
	private Texture2D PlayerPosition;

	// Token: 0x0400084E RID: 2126
	private Texture2D TeamPosition;

	// Token: 0x0400084F RID: 2127
	private Texture2D BasePosition;

	// Token: 0x04000850 RID: 2128
	public Texture2D[] FlagPosition;

	// Token: 0x04000851 RID: 2129
	public Texture2D RadarSideTexture;

	// Token: 0x04000852 RID: 2130
	private GUIStyle gui_style;

	// Token: 0x04000853 RID: 2131
	private Map map;

	// Token: 0x04000854 RID: 2132
	private Client cscl;

	// Token: 0x04000855 RID: 2133
	private PlayerControl cspc;

	// Token: 0x04000856 RID: 2134
	private GameObject LocalPlayer;

	// Token: 0x04000857 RID: 2135
	public Vector3[] team_pos = new Vector3[4];

	// Token: 0x04000858 RID: 2136
	public Vector3 base_pos;

	// Token: 0x04000859 RID: 2137
	public int[] team_dot = new int[4];

	// Token: 0x0400085A RID: 2138
	public int team_dot_all;

	// Token: 0x0400085B RID: 2139
	private List<Radar.CBlockColor> colorlist = new List<Radar.CBlockColor>();

	// Token: 0x0400085C RID: 2140
	private Texture2D tmpTex;

	// Token: 0x0400085D RID: 2141
	private BlockSet blockset;

	// Token: 0x0400085E RID: 2142
	private Texture2D blockTex;

	// Token: 0x0400085F RID: 2143
	private Color tmpColor;

	// Token: 0x04000860 RID: 2144
	private Color[] _col = new Color[4];

	// Token: 0x04000861 RID: 2145
	private int[] oldFlagScores = new int[4];

	// Token: 0x04000862 RID: 2146
	private bool[] mig = new bool[4];

	// Token: 0x0200086D RID: 2157
	public class CBlockColor
	{
		// Token: 0x06004BE8 RID: 19432 RVA: 0x001AA288 File Offset: 0x001A8488
		public CBlockColor(string name, Color color)
		{
			this.name = name;
			this.color = color;
		}

		// Token: 0x06004BE9 RID: 19433 RVA: 0x001AA2A0 File Offset: 0x001A84A0
		~CBlockColor()
		{
		}

		// Token: 0x040032D1 RID: 13009
		public string name;

		// Token: 0x040032D2 RID: 13010
		public Color color;
	}
}
