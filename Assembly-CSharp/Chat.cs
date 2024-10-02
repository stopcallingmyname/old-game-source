using System;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class Chat : MonoBehaviour
{
	// Token: 0x0600036B RID: 875 RVA: 0x0003BD30 File Offset: 0x00039F30
	private void Awake()
	{
		this.Map = GameObject.Find("Map");
		this.gui_style = new GUIStyle();
		this.gui_style.font = FontManager.font[1];
		this.gui_style.fontSize = 14;
		this.gui_style.normal.textColor = new Color(255f, 255f, 255f, 255f);
		this.lastupdate = Time.time;
		this.tex_chat = ContentLoader.LoadTexture("chat");
	}

	// Token: 0x0600036C RID: 876 RVA: 0x0003BDBC File Offset: 0x00039FBC
	private void Update()
	{
		if (Time.time > this.lastupdate + 10f)
		{
			this.AddMessage(-1, 0, "", 0);
		}
		if (!this.entermode && Input.GetKeyUp(KeyCode.T))
		{
			this.teamsay = 1;
			this.entermode = true;
			this.chat_prefix = Lang.GetLabel(126);
			MainGUI.ForceCursor = true;
		}
		if (!this.entermode && Input.GetKeyUp(KeyCode.Return))
		{
			this.teamsay = 0;
			this.entermode = true;
			this.chat_prefix = Lang.GetLabel(127);
			MainGUI.ForceCursor = true;
		}
	}

	// Token: 0x0600036D RID: 877 RVA: 0x0003BE50 File Offset: 0x0003A050
	private void OnGUI()
	{
		if (!this.show)
		{
			return;
		}
		for (int i = 0; i < 6; i++)
		{
			GUIManager.DrawColorText(20f, (float)(Screen.height - 160 + i * 20), this.message[i], TextAnchor.MiddleLeft);
		}
		if (this.entermode)
		{
			GUI.DrawTexture(new Rect((float)Screen.width / 2f - 100f - 80f, (float)(Screen.height - 100 + 1), 275f, 23f), this.tex_chat);
			GUIManager.DrawColorText((float)Screen.width / 2f - 100f - 78f, (float)(Screen.height - 100 + 2), this.chat_prefix, TextAnchor.MiddleLeft);
			GUIManager.BeginScrollView(new Rect((float)Screen.width / 2f - 100f, (float)(Screen.height - 100 + 4), 200f, 25f), new Vector2(0f, 0f), new Rect(0f, 0f, 0f, 0f));
			GUI.SetNextControlName("input");
			TextAnchor alignment = GUIManager.gs_style2.alignment;
			GUIManager.gs_style2.alignment = TextAnchor.UpperLeft;
			this.stringToEdit = GUI.TextField(new Rect(0f, 0f, 165f, 25f), this.stringToEdit, 64, GUIManager.gs_style2);
			GUIManager.gs_style2.alignment = alignment;
			int length = this.stringToEdit.Length;
			this.stringToEdit = this.stringToEdit.Replace("\n", "").Replace("\r", "");
			if (this.stringToEdit.Length == 63)
			{
				this.stringToEdit = this.stringToEdit.Substring(0, this.stringToEdit.Length - 1);
			}
			if (Event.current.type == EventType.KeyDown && Event.current.character == '\n')
			{
				if (this.stringToEdit.Length != 0)
				{
					if (this.cscl == null)
					{
						this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
					}
					this.stringToEdit = this.stringToEdit.Replace("^0", "^8");
					this.stringToEdit = this.stringToEdit.Replace("^1", "^8");
					this.stringToEdit = this.stringToEdit.Replace("^2", "^8");
					this.stringToEdit = this.stringToEdit.Replace("^3", "^8");
					this.stringToEdit = this.stringToEdit.Replace("^4", "^8");
					this.stringToEdit = this.stringToEdit.Replace("^9", "^8");
					this.cscl.send_chat(this.teamsay, this.stringToEdit);
					this.entertime = Time.time + 0.3f;
				}
				this.stringToEdit = "";
				MainGUI.ForceCursor = false;
				this.entermode = false;
			}
			GUI.FocusControl("input");
			GUIManager.EndScrollView();
		}
	}

	// Token: 0x0600036E RID: 878 RVA: 0x0003C174 File Offset: 0x0003A374
	public void AddMessage(int index, int team, string msg, int teamchat)
	{
		for (int i = 0; i < 5; i++)
		{
			this.message[i] = this.message[i + 1];
		}
		if (index >= 0 && teamchat < 2)
		{
			string str = "";
			if (teamchat > 0)
			{
				str = "^8(" + Lang.GetLabel(126) + ")";
			}
			switch (team)
			{
			case 0:
				str += "^0";
				break;
			case 1:
				str += "^1";
				break;
			case 2:
				str += "^2";
				break;
			case 3:
				str += "^3";
				break;
			}
			this.message[5] = str + RemotePlayersUpdater.Instance.Bots[index].Name + "^8 : " + msg;
		}
		else if (teamchat == 2)
		{
			string text = "";
			switch (team)
			{
			case 0:
				text += "^0";
				break;
			case 1:
				text += "^1";
				break;
			case 2:
				text += "^2";
				break;
			case 3:
				text += "^3";
				break;
			}
			this.message[5] = Lang.GetLabel(665) + text + RemotePlayersUpdater.Instance.Bots[index].Name + Lang.GetLabel(666);
		}
		else
		{
			this.message[5] = msg;
		}
		this.lastupdate = Time.time;
	}

	// Token: 0x04000659 RID: 1625
	private Client cscl;

	// Token: 0x0400065A RID: 1626
	private GameObject LocalPlayer;

	// Token: 0x0400065B RID: 1627
	private GameObject Map;

	// Token: 0x0400065C RID: 1628
	private GUIStyle gui_style;

	// Token: 0x0400065D RID: 1629
	private byte teamsay;

	// Token: 0x0400065E RID: 1630
	private bool entermode;

	// Token: 0x0400065F RID: 1631
	private bool messageready;

	// Token: 0x04000660 RID: 1632
	private string stringToEdit = "";

	// Token: 0x04000661 RID: 1633
	private bool userHasHitReturn;

	// Token: 0x04000662 RID: 1634
	private string chat_prefix = "";

	// Token: 0x04000663 RID: 1635
	private string[] message = new string[6];

	// Token: 0x04000664 RID: 1636
	private float lastupdate;

	// Token: 0x04000665 RID: 1637
	private Texture2D tex_chat;

	// Token: 0x04000666 RID: 1638
	private float entertime;

	// Token: 0x04000667 RID: 1639
	private float starttime;

	// Token: 0x04000668 RID: 1640
	public bool show = true;

	// Token: 0x04000669 RID: 1641
	private int tmpId;
}
