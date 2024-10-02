using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000089 RID: 137
public class SaveGUI : MonoBehaviour
{
	// Token: 0x060003DC RID: 988 RVA: 0x0004BE94 File Offset: 0x0004A094
	private void Awake()
	{
		this.r_window = new Rect(0f, 0f, 600f, 400f);
		this.r_window.center = new Vector2((float)Screen.width, (float)Screen.height) / 2f;
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0004BEE6 File Offset: 0x0004A0E6
	public void SetActive(bool val)
	{
		this.active = val;
		if (val && !this.dataload)
		{
			base.StartCoroutine(this.get_mymaps());
		}
		MainGUI.ForceCursor = this.active;
	}

	// Token: 0x060003DE RID: 990 RVA: 0x0004BF12 File Offset: 0x0004A112
	private void OnGUI()
	{
		if (!this.active)
		{
			return;
		}
		GUI.Window(0, this.r_window, new GUI.WindowFunction(this.DrawWindow), "", GUIManager.gs_empty);
	}

	// Token: 0x060003DF RID: 991 RVA: 0x0004BF40 File Offset: 0x0004A140
	private void DrawWindow(int wid)
	{
		Vector2 mpos = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
		mpos.x -= this.r_window.x;
		mpos.y -= this.r_window.y;
		GUI.color = new Color(1f, 1f, 1f, 0.8f);
		GUI.DrawTexture(new Rect(0f, 0f, 600f, 32f), GUIManager.tex_black);
		GUI.color = new Color(1f, 1f, 1f, 1f);
		GUI.Label(new Rect(0f, 0f, 600f, 32f), Lang.GetLabel(129), GUIManager.gs_style1);
		GUI.color = new Color(1f, 1f, 1f, 0.6f);
		GUI.DrawTexture(new Rect(0f, 34f, 600f, 366f), GUIManager.tex_black);
		GUI.color = new Color(1f, 1f, 1f, 1f);
		int num = 172;
		int num2 = 60;
		foreach (int mapid in this.map)
		{
			if (GUIManager.DrawButton(new Rect((float)num, (float)num2, 256f, 32f), mpos, new Color(0f, 1f, 0f, 1f), Lang.GetLabel(143) + mapid.ToString()))
			{
				this.SetActive(false);
				if (this.cscl == null)
				{
					this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
				}
				this.cscl.send_private_settings(2, mapid);
			}
			num2 += 40;
		}
		if (CONST.GetGameMode() == MODE.BUILD)
		{
			if (GUIManager.DrawButton(new Rect((float)num, 298f, 256f, 32f), mpos, new Color(0f, 1f, 0f, 1f), Lang.GetLabel(144)))
			{
				this.SetActive(false);
				base.gameObject.GetComponent<MapSize>().OnActive();
			}
			if (GUIManager.DrawButton(new Rect((float)num, 358f, 256f, 32f), mpos, new Color(1f, 0f, 0f, 1f), Lang.GetLabel(145)))
			{
				this.SetActive(false);
				if (this.cscl == null)
				{
					this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
				}
				this.cscl.send_savemap();
			}
		}
		if (GUIManager.DrawButton(new Rect(462f, 358f, 128f, 32f), mpos, new Color(0f, 1f, 0f, 1f), Lang.GetLabel(123)))
		{
			this.SetActive(false);
		}
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x0004C288 File Offset: 0x0004A488
	public void Init()
	{
		base.StartCoroutine(this.get_mymaps());
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x0004C297 File Offset: 0x0004A497
	private IEnumerator get_mymaps()
	{
		this.dataload = true;
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"18&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&time=",
			DateTime.Now.Second
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			this.map.Clear();
			string[] array = www.text.Split(new char[]
			{
				'|'
			});
			int num = 0;
			string[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				string s = array2[i];
				if (num == 0)
				{
					if (!int.TryParse(s, out this.csmode))
					{
						this.csmode = 0;
						goto IL_135;
					}
					goto IL_135;
				}
				else
				{
					int item = 0;
					if (int.TryParse(s, out item))
					{
						this.map.Add(item);
						goto IL_135;
					}
				}
				IL_13B:
				i++;
				continue;
				IL_135:
				num++;
				goto IL_13B;
			}
		}
		else
		{
			this.dataload = false;
		}
		yield break;
	}

	// Token: 0x04000863 RID: 2147
	public bool active;

	// Token: 0x04000864 RID: 2148
	private Rect r_window;

	// Token: 0x04000865 RID: 2149
	private PlayerControl cspc;

	// Token: 0x04000866 RID: 2150
	private Client cscl;

	// Token: 0x04000867 RID: 2151
	private List<int> map = new List<int>();

	// Token: 0x04000868 RID: 2152
	private bool dataload;

	// Token: 0x04000869 RID: 2153
	public int csmode;
}
