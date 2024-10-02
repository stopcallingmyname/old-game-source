using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000082 RID: 130
public class LoadScreen : MonoBehaviour
{
	// Token: 0x060003AC RID: 940 RVA: 0x00045A8C File Offset: 0x00043C8C
	private void Awake()
	{
		LoadScreen.THIS = this;
		GUIManager.Init(false);
		this.LocalPlayer = GameObject.Find("Player");
		this.gui_style = new GUIStyle();
		this.gui_style.font = FontManager.font[0];
		this.gui_style.fontSize = 20;
		this.gui_style.normal.textColor = new Color(0f, 0f, 0f, 1f);
		this.gui_style.alignment = TextAnchor.MiddleCenter;
		this.black = new Texture2D(1, 1);
		this.black.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.5f));
		this.black.Apply();
		this.tmpmsg = new string[4];
		this.tmpmsg[0] = Lang.GetLabel(225);
		this.tmpmsg[1] = Lang.GetLabel(226);
		this.tmpmsg[2] = Lang.GetLabel(348);
		this.tmpmsg[3] = Lang.GetLabel(349);
		this.msg[0] = this.tmpmsg;
		this.tmpmsg = new string[0];
		this.msg[1] = this.tmpmsg;
		this.msg[8] = this.tmpmsg;
		this.tmpmsg = new string[4];
		this.tmpmsg[0] = Lang.GetLabel(225);
		this.tmpmsg[1] = Lang.GetLabel(226);
		this.tmpmsg[2] = Lang.GetLabel(350);
		this.tmpmsg[3] = Lang.GetLabel(351);
		this.msg[3] = this.tmpmsg;
		this.tmpmsg = new string[5];
		this.tmpmsg[0] = Lang.GetLabel(225);
		this.tmpmsg[1] = Lang.GetLabel(226);
		this.tmpmsg[2] = Lang.GetLabel(352);
		this.tmpmsg[3] = Lang.GetLabel(353);
		this.tmpmsg[4] = Lang.GetLabel(354);
		this.msg[4] = this.tmpmsg;
		this.tmpmsg = new string[4];
		this.tmpmsg[0] = Lang.GetLabel(225);
		this.tmpmsg[1] = Lang.GetLabel(226);
		this.tmpmsg[2] = Lang.GetLabel(355);
		this.tmpmsg[3] = Lang.GetLabel(356);
		this.msg[5] = this.tmpmsg;
		this.tmpmsg = new string[4];
		this.tmpmsg[0] = Lang.GetLabel(225);
		this.tmpmsg[1] = Lang.GetLabel(226);
		this.tmpmsg[2] = Lang.GetLabel(357);
		this.tmpmsg[3] = Lang.GetLabel(358);
		this.msg[6] = this.tmpmsg;
		this.tmpmsg = new string[4];
		this.tmpmsg[0] = Lang.GetLabel(225);
		this.tmpmsg[1] = Lang.GetLabel(226);
		this.tmpmsg[2] = Lang.GetLabel(359);
		this.tmpmsg[3] = Lang.GetLabel(360);
		this.msg[7] = this.tmpmsg;
		this.tmpmsg = new string[4];
		this.tmpmsg[0] = Lang.GetLabel(225);
		this.tmpmsg[1] = Lang.GetLabel(226);
		this.tmpmsg[2] = Lang.GetLabel(361);
		this.tmpmsg[3] = Lang.GetLabel(362);
		this.msg[11] = this.tmpmsg;
		this.tmpmsg = new string[2];
		this.tmpmsg[0] = Lang.GetLabel(225);
		this.tmpmsg[1] = Lang.GetLabel(226);
		this.msg[100] = this.tmpmsg;
		this.msg[9] = this.tmpmsg;
		this.initialized = false;
	}

	// Token: 0x060003AD RID: 941 RVA: 0x00045EBC File Offset: 0x000440BC
	private void OnGUI()
	{
		if (!this.initialized)
		{
			if (GM.currMainState != GAME_STATES.CONNECTING)
			{
				GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), GUI3.blackTex);
				return;
			}
			this.Init();
		}
		if (this.need_rename)
		{
			if (this.progress == 0)
			{
				this.loadtext = Lang.GetLabel(152) + Lang.GetLabel(490);
			}
			else if (this.progress == 1)
			{
				this.loadtext = Lang.GetLabel(152) + Lang.GetLabel(491);
			}
			else if (this.progress == 2)
			{
				this.loadtext = Lang.GetLabel(152) + Lang.GetLabel(492);
			}
			else if (this.progress == 3)
			{
				this.loadtext = Lang.GetLabel(152) + Lang.GetLabel(493);
			}
			else if (this.progress == 4)
			{
				this.loadtext = Lang.GetLabel(152) + Lang.GetLabel(494);
			}
			else if (this.progress == 5)
			{
				this.loadtext = Lang.GetLabel(152);
			}
		}
		GUI.depth = -2;
		Rect position = new Rect(0f, 0f, (float)(Screen.height * 2), (float)Screen.height);
		position.center = new Vector2((float)Screen.width, (float)Screen.height) / 2f;
		Rect texCoords = new Rect(this.x1, 0f, 1f, 1f);
		Rect texCoords2 = new Rect(this.x2, 0f, 1f, 1f);
		GUI.DrawTexture(position, this.background[0]);
		GUI.DrawTextureWithTexCoords(position, this.background[1], texCoords);
		GUI.DrawTexture(position, this.background[2]);
		GUI.DrawTextureWithTexCoords(position, this.background[3], texCoords2);
		this.x1 += 0.02f * Time.deltaTime;
		this.x2 -= 0.015f * Time.deltaTime;
		GUI.Label(new Rect((float)Screen.width / 2f - 100f, (float)Screen.height * 0.8f - 30f, 200f, 32f), this.loadtext, this.gui_style);
		int num = (int)((float)Screen.width / 2f - 86f);
		int num2 = (int)((float)Screen.height * 0.8f);
		GUI.DrawTexture(new Rect((float)num, (float)num2, 172f, 20f), this.black);
		num += 2;
		num2 += 2;
		for (int i = 0; i < this.progress; i++)
		{
			GUI.DrawTexture(new Rect((float)num, (float)num2, 32f, 16f), this.bar);
			num += 34;
		}
		if (this.last_mode != ConnectionInfo.mode)
		{
			this.last_mode = ConnectionInfo.mode;
			if (ConnectionInfo.mode != 2 && ConnectionInfo.mode != 10 && ConnectionInfo.mode != 8 && ConnectionInfo.mode != 13)
			{
				this.msg_type = Random.Range(0, this.msg[ConnectionInfo.mode].Length - 1);
			}
		}
		if (ConnectionInfo.mode == 12)
		{
			return;
		}
		num = 40;
		num2 = Screen.height - 120;
		if ((float)num2 < (float)Screen.height * 0.8f + 20f)
		{
			return;
		}
		if (ConnectionInfo.mode != 2 && ConnectionInfo.mode != 10 && ConnectionInfo.mode != 1 && ConnectionInfo.mode != 8 && ConnectionInfo.mode != 13)
		{
			GUI.DrawTexture(new Rect((float)num, (float)num2, (float)(Screen.width - 80), 80f), this.black);
			GUIManager.DrawText2(new Rect((float)(num + 4), (float)num2, (float)(Screen.width - 80), 80f), Lang.GetLabel(224), 18, TextAnchor.UpperLeft, Color.white);
			GUIManager.DrawText2(new Rect((float)num, (float)num2, (float)(Screen.width - 80), 80f), this.msg[ConnectionInfo.mode][this.msg_type], 18, TextAnchor.MiddleCenter, Color.yellow);
		}
	}

	// Token: 0x060003AE RID: 942 RVA: 0x00046308 File Offset: 0x00044508
	private void Init()
	{
		this.background[0] = ContentLoader.LoadTexture("layer_wall");
		this.background[1] = ContentLoader.LoadTexture("layer_back");
		if (Lang.current == 0)
		{
			this.background[2] = ContentLoader.LoadTexture("layer_load_rus");
		}
		else
		{
			this.background[2] = ContentLoader.LoadTexture("layer_load_eng");
		}
		this.background[3] = ContentLoader.LoadTexture("layer_front");
		this.initialized = true;
	}

	// Token: 0x0400080F RID: 2063
	public static LoadScreen THIS;

	// Token: 0x04000810 RID: 2064
	private bool Active;

	// Token: 0x04000811 RID: 2065
	public Texture2D[] background = new Texture2D[4];

	// Token: 0x04000812 RID: 2066
	public Texture2D bar;

	// Token: 0x04000813 RID: 2067
	public int progress;

	// Token: 0x04000814 RID: 2068
	public string loadtext = "";

	// Token: 0x04000815 RID: 2069
	private Texture2D black;

	// Token: 0x04000816 RID: 2070
	private GUIStyle gui_style;

	// Token: 0x04000817 RID: 2071
	private GameObject LocalPlayer;

	// Token: 0x04000818 RID: 2072
	private Rect r;

	// Token: 0x04000819 RID: 2073
	private float x1;

	// Token: 0x0400081A RID: 2074
	private float x2;

	// Token: 0x0400081B RID: 2075
	private float y1;

	// Token: 0x0400081C RID: 2076
	private float y2;

	// Token: 0x0400081D RID: 2077
	private Dictionary<int, string[]> msg = new Dictionary<int, string[]>();

	// Token: 0x0400081E RID: 2078
	private string[] tmpmsg;

	// Token: 0x0400081F RID: 2079
	private int msg_type;

	// Token: 0x04000820 RID: 2080
	private int last_mode = 50;

	// Token: 0x04000821 RID: 2081
	public bool need_rename = true;

	// Token: 0x04000822 RID: 2082
	private bool initialized;
}
