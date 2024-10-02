using System;
using UnityEngine;

// Token: 0x02000078 RID: 120
public class AdminGUI : MonoBehaviour
{
	// Token: 0x0600035D RID: 861 RVA: 0x0003A87C File Offset: 0x00038A7C
	private void Awake()
	{
		this.r_window = new Rect(0f, 0f, 600f, 400f);
		this.r_window.center = new Vector2((float)Screen.width, (float)Screen.height) / 2f;
		this.cssg = base.gameObject.GetComponent<SaveGUI>();
	}

	// Token: 0x0600035E RID: 862 RVA: 0x0003A8DF File Offset: 0x00038ADF
	private void OnGUI()
	{
		if (!this.active)
		{
			return;
		}
		GUI.Window(0, this.r_window, new GUI.WindowFunction(this.DrawWindow), "", GUIManager.gs_empty);
	}

	// Token: 0x0600035F RID: 863 RVA: 0x0003A910 File Offset: 0x00038B10
	public void SetActive(bool val)
	{
		this.active = val;
		if (this.active)
		{
			this.gamemode = (int)CONST.GetGameMode();
			this.mapid = PlayerControl.GetMapID();
			int.TryParse(this.mapid, out this.imapid);
			this.cssg.Init();
		}
	}

	// Token: 0x06000360 RID: 864 RVA: 0x0003A960 File Offset: 0x00038B60
	private void DrawWindow(int wid)
	{
		Vector2 mpos = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
		mpos.x -= this.r_window.x;
		mpos.y -= this.r_window.y;
		GUI.color = new Color(1f, 1f, 1f, 0.8f);
		GUI.DrawTexture(new Rect(0f, 0f, 600f, 32f), GUIManager.tex_black);
		GUI.color = new Color(1f, 1f, 1f, 1f);
		GUI.Label(new Rect(0f, 0f, 600f, 32f), Lang.GetLabel(116), GUIManager.gs_style1);
		GUI.color = new Color(1f, 1f, 1f, 0.6f);
		GUI.DrawTexture(new Rect(0f, 34f, 600f, 366f), GUIManager.tex_black);
		GUI.color = new Color(1f, 1f, 1f, 1f);
		GUIManager.gs_style1.alignment = TextAnchor.UpperLeft;
		GUI.Label(new Rect(64f, 42f, 256f, 32f), Lang.GetLabel(117), GUIManager.gs_style1);
		GUIManager.gs_style1.alignment = TextAnchor.MiddleCenter;
		this.buttoncolors[0] = new Color(1f, 1f, 1f, 1f);
		this.buttoncolors[1] = new Color(1f, 1f, 1f, 1f);
		this.buttoncolors[2] = new Color(1f, 1f, 1f, 1f);
		this.buttoncolors[3] = new Color(1f, 1f, 1f, 1f);
		this.buttoncolors[4] = new Color(1f, 1f, 1f, 1f);
		this.buttoncolors[5] = new Color(1f, 1f, 1f, 1f);
		this.buttoncolors[this.gamemode] = new Color(0f, 1f, 0f, 1f);
		if (this.gamemode == 1)
		{
			this.buttoncolors[2] = new Color(0f, 1f, 0f, 1f);
		}
		if (GUIManager.DrawButton(new Rect(256f, 40f, 96f, 20f), mpos, this.buttoncolors[0], Lang.GetLabel(48)))
		{
			this.gamemode = 0;
		}
		if (GUIManager.DrawButton(new Rect(356f, 40f, 96f, 20f), mpos, this.buttoncolors[2], Lang.GetLabel(49)))
		{
			this.active = false;
			base.gameObject.GetComponent<SaveGUI>().SetActive(true);
		}
		if (this.cssg.csmode > 0 && GUIManager.DrawButton(new Rect(456f, 40f, 96f, 20f), mpos, this.buttoncolors[5], Lang.GetLabel(52)))
		{
			this.gamemode = 5;
		}
		GUIManager.gs_style1.alignment = TextAnchor.UpperLeft;
		GUI.Label(new Rect(64f, 72f, 256f, 32f), Lang.GetLabel(118), GUIManager.gs_style1);
		GUIManager.gs_style1.alignment = TextAnchor.MiddleCenter;
		this.mapid = GUIManager.DrawEdit(new Rect(256f, 72f, 96f, 20f), mpos, new Color(1f, 1f, 1f, 1f), this.imapid.ToString(), 6);
		if (this.mapid.Length == 0)
		{
			this.imapid = 0;
		}
		else
		{
			int num = 0;
			if (int.TryParse(this.mapid, out num))
			{
				this.imapid = num;
			}
		}
		if (GUIManager.DrawButton(new Rect(356f, 72f, 96f, 20f), mpos, new Color(1f, 0f, 0f, 1f), Lang.GetLabel(119)))
		{
			if (this.cscl == null)
			{
				this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
			}
			this.active = false;
			MainGUI.ForceCursor = this.active;
			int num2 = 0;
			int.TryParse(this.mapid, out num2);
			int num3 = this.gamemode;
			if (num3 == 1)
			{
				num3 = 2;
			}
			this.cscl.send_private_settings(num3, num2);
		}
		if ((this.gamemode == 0 || this.gamemode == 5) && GUIManager.DrawButton(new Rect(454f, 72f, 96f, 20f), mpos, new Color(1f, 1f, 0f, 1f), Lang.GetLabel(504)))
		{
			this.active = false;
			base.gameObject.GetComponent<MapListGUI>().SetActive(true, this.gamemode);
		}
		GUIManager.gs_style1.alignment = TextAnchor.UpperLeft;
		GUI.Label(new Rect(64f, 104f, 256f, 32f), Lang.GetLabel(120), GUIManager.gs_style1);
		GUIManager.gs_style1.alignment = TextAnchor.MiddleCenter;
		this.password = GUIManager.DrawEdit(new Rect(256f, 104f, 96f, 20f), mpos, new Color(1f, 1f, 1f, 1f), this.ipassword.ToString(), 6);
		if (this.password.Length == 0)
		{
			this.ipassword = 0;
		}
		else
		{
			int num4 = 0;
			if (int.TryParse(this.password, out num4))
			{
				this.ipassword = num4;
			}
		}
		if (GUIManager.DrawButton(new Rect(356f, 104f, 96f, 20f), mpos, new Color(1f, 0f, 0f, 1f), Lang.GetLabel(121)))
		{
			if (this.cscl == null)
			{
				this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
			}
			this.cscl.send_private_command(2, 0, this.ipassword);
		}
		if (GUIManager.DrawButton(new Rect(454f, 104f, 96f, 20f), mpos, new Color(1f, 0f, 0f, 1f), Lang.GetLabel(122)))
		{
			if (this.cscl == null)
			{
				this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
			}
			this.cscl.send_private_command(3, 0, 0);
		}
		this.DrawWindowPlayers(20, 140, mpos);
		if (GUIManager.DrawButton(new Rect(462f, 358f, 128f, 32f), mpos, new Color(0f, 1f, 0f, 1f), Lang.GetLabel(123)))
		{
			this.active = false;
			MainGUI.ForceCursor = this.active;
		}
	}

	// Token: 0x06000361 RID: 865 RVA: 0x0003B0C8 File Offset: 0x000392C8
	private void DrawWindowPlayers(int x, int y, Vector2 mpos)
	{
		GUI.color = new Color(1f, 1f, 1f, 0.25f);
		GUI.DrawTexture(new Rect((float)x, (float)y, 560f, 206f), GUIManager.tex_black);
		GUI.color = new Color(1f, 1f, 1f, 1f);
		mpos.x -= (float)x;
		mpos.y -= (float)y;
		int num = 0;
		for (int i = 0; i < 32; i++)
		{
			if (RemotePlayersUpdater.Instance.Bots[i].Active)
			{
				num++;
			}
		}
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect((float)x, (float)y, 560f, 206f), this.scrollViewVector, new Rect(0f, 0f, 0f, (float)(num * 24 + 8)));
		int num2 = 0;
		for (int j = 0; j < 32; j++)
		{
			if (RemotePlayersUpdater.Instance.Bots[j].Active)
			{
				this.DrawPlayer(8, num2 * 24 + 4, j, mpos + this.scrollViewVector);
				num2++;
			}
		}
		GUIManager.EndScrollView();
	}

	// Token: 0x06000362 RID: 866 RVA: 0x0003B1F4 File Offset: 0x000393F4
	private void DrawPlayer(int x, int y, int id, Vector2 mpos)
	{
		GUIManager.gs_style1.alignment = TextAnchor.UpperLeft;
		GUI.Label(new Rect((float)x, (float)y, 256f, 32f), RemotePlayersUpdater.Instance.Bots[id].Name, GUIManager.gs_style1);
		if (id != PlayerProfile.myindex)
		{
			GUIManager.gs_style1.alignment = TextAnchor.MiddleCenter;
			if (GUIManager.DrawButton(new Rect((float)(x + 330), (float)y, 96f, 20f), mpos, this.buttoncolors[0], Lang.GetLabel(124)))
			{
				if (this.cscl == null)
				{
					this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
				}
				this.cscl.send_private_command(0, id, 0);
			}
			if (GUIManager.DrawButton(new Rect((float)(x + 430), (float)y, 96f, 20f), mpos, this.buttoncolors[0], Lang.GetLabel(125)))
			{
				if (this.cscl == null)
				{
					this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
				}
				this.cscl.send_private_command(1, id, 0);
			}
		}
		GUIManager.gs_style1.alignment = TextAnchor.MiddleCenter;
	}

	// Token: 0x0400061C RID: 1564
	public bool active;

	// Token: 0x0400061D RID: 1565
	private Rect r_window;

	// Token: 0x0400061E RID: 1566
	private PlayerControl cspc;

	// Token: 0x0400061F RID: 1567
	private Client cscl;

	// Token: 0x04000620 RID: 1568
	private SaveGUI cssg;

	// Token: 0x04000621 RID: 1569
	private int gamemode;

	// Token: 0x04000622 RID: 1570
	private string mapid = "";

	// Token: 0x04000623 RID: 1571
	private string password = "";

	// Token: 0x04000624 RID: 1572
	private int imapid;

	// Token: 0x04000625 RID: 1573
	private int ipassword;

	// Token: 0x04000626 RID: 1574
	private Color[] buttoncolors = new Color[10];

	// Token: 0x04000627 RID: 1575
	private Vector2 scrollViewVector = Vector2.zero;
}
