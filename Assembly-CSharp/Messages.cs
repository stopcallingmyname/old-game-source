using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000085 RID: 133
public class Messages : MonoBehaviour
{
	// Token: 0x060003BE RID: 958 RVA: 0x00048FB7 File Offset: 0x000471B7
	private void Awake()
	{
		this.NLIcon = ContentLoader.LoadTexture("net_nl");
	}

	// Token: 0x060003BF RID: 959 RVA: 0x00048FCC File Offset: 0x000471CC
	private void OnGUI()
	{
		GUI.depth = -2;
		if (this.msglist.Count > 0)
		{
			if (this.msglist[0].time > Time.time)
			{
				GUIManager.DrawColorText((float)Screen.width / 2f - 128f, (float)Screen.height / 4f, this.msglist[0].text, TextAnchor.MiddleCenter);
			}
			else
			{
				this.msglist.RemoveAt(0);
			}
		}
		if (this.sysmsglist.Count > 0)
		{
			int num = 0;
			int num2 = 140;
			int num3 = -1;
			foreach (Messages.CMsg cmsg in this.sysmsglist)
			{
				if (cmsg.time > Time.time)
				{
					GUI.DrawTexture(new Rect(GUIManager.XRES(4f), GUIManager.YRES((float)num2), GUIManager.YRES(400f), GUIManager.YRES(40f)), GUIManager.tex_half_black);
					GUI.DrawTexture(new Rect(GUIManager.XRES(9f), GUIManager.YRES((float)(num2 + 5)), GUIManager.YRES(30f), GUIManager.YRES(30f)), this.NLIcon);
					GUIManager.DrawColorText(GUIManager.XRES(55f), GUIManager.YRES((float)(num2 + 3)), Lang.GetLabel(665), TextAnchor.MiddleLeft);
					GUIManager.DrawColorText(GUIManager.XRES(55f), GUIManager.YRES((float)(num2 + 23)), cmsg.text, TextAnchor.MiddleLeft);
					num2 += (int)GUIManager.YRES(40f);
				}
				else if (num3 < 0)
				{
					num3 = num;
				}
				num++;
			}
			if (num3 >= 0)
			{
				this.sysmsglist.RemoveAt(num3);
			}
		}
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x00049198 File Offset: 0x00047398
	public void Add(string text, float time, bool clear = false)
	{
		if (clear)
		{
			this.msglist.Clear();
		}
		if (this.msglist.Count != 0)
		{
			this.msglist.Add(new Messages.CMsg(text, this.msglist[this.msglist.Count - 1].time, time));
			return;
		}
		this.msglist.Add(new Messages.CMsg(text, time));
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x00049204 File Offset: 0x00047404
	private string getTextBySec(int sec)
	{
		if (sec > 10 && sec < 20)
		{
			switch (sec % 10)
			{
			case 0:
				return Lang.GetLabel(304);
			case 1:
				return Lang.GetLabel(304);
			case 2:
				return Lang.GetLabel(304);
			case 3:
				return Lang.GetLabel(304);
			case 4:
				return Lang.GetLabel(304);
			case 5:
				return Lang.GetLabel(304);
			case 6:
				return Lang.GetLabel(304);
			case 7:
				return Lang.GetLabel(304);
			case 8:
				return Lang.GetLabel(304);
			case 9:
				return Lang.GetLabel(304);
			}
		}
		else
		{
			switch (sec % 10)
			{
			case 0:
				return Lang.GetLabel(304);
			case 1:
				return Lang.GetLabel(305);
			case 2:
				return Lang.GetLabel(306);
			case 3:
				return Lang.GetLabel(306);
			case 4:
				return Lang.GetLabel(306);
			case 5:
				return Lang.GetLabel(304);
			case 6:
				return Lang.GetLabel(304);
			case 7:
				return Lang.GetLabel(304);
			case 8:
				return Lang.GetLabel(304);
			case 9:
				return Lang.GetLabel(304);
			}
		}
		return Lang.GetLabel(304);
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x00049374 File Offset: 0x00047574
	public void set_message(int msgid, int param)
	{
		if (CONST.GetGameMode() == MODE.ZOMBIE)
		{
			if (msgid == 0)
			{
				this.Add("^0" + Lang.GetLabel(136), 3f, false);
			}
			if (msgid == 1)
			{
				this.Add("^1" + Lang.GetLabel(137), 3f, false);
			}
			if (msgid == 2)
			{
				this.Add("^8" + Lang.GetLabel(138), 3f, false);
			}
			if (msgid == 3)
			{
				this.Add("^3" + Lang.GetLabel(134), 2.5f, false);
				this.Add("^810", 1f, false);
				this.Add("^89", 1f, false);
				this.Add("^88", 1f, false);
				this.Add("^87", 1f, false);
				this.Add("^86", 1f, false);
				this.Add("^85", 1f, false);
				this.Add("^84", 1f, false);
				this.Add("^13", 1f, false);
				this.Add("^12", 1f, false);
				this.Add("^11", 1f, false);
				this.Add("^1" + Lang.GetLabel(135), 2.5f, false);
			}
		}
		else if (CONST.GetGameMode() == MODE.SURVIVAL)
		{
			if (msgid == 0)
			{
				this.Add("^8" + Lang.GetLabel(153) + " " + param.ToString(), 2f, true);
			}
			if (msgid == 1)
			{
				this.Add("^1" + Lang.GetLabel(154) + param.ToString() + ")", 3f, true);
			}
			if (msgid == 2)
			{
				this.Add("^8" + Lang.GetLabel(155), 3f, true);
				Batch batch = (Batch)Object.FindObjectOfType(typeof(Batch));
				if (batch)
				{
					batch.Combine();
				}
				Resources.UnloadUnusedAssets();
				GC.Collect();
			}
		}
		else if (CONST.GetGameMode() == MODE.CLEAR)
		{
			if (msgid == 0)
			{
				this.Add("^8" + Lang.GetLabel(307) + param.ToString() + Lang.GetLabel(304), 0.8f, false);
			}
			if (msgid == 2)
			{
				this.Add("^1" + Lang.GetLabel(308) + param.ToString(), 3f, false);
			}
			if (msgid == 3)
			{
				this.Add("^8" + Lang.GetLabel(309), 3f, false);
			}
			if (msgid == 4)
			{
				this.Add("^8" + Lang.GetLabel(310), 3f, false);
			}
			if (msgid == 5)
			{
				this.Add("^8" + Lang.GetLabel(311), 3f, false);
			}
			if (msgid == 6)
			{
				this.Add("^8" + Lang.GetLabel(312), 3f, false);
			}
			if (msgid == 7)
			{
				this.Add("^8" + Lang.GetLabel(313), 3f, false);
			}
			if (msgid == 8)
			{
				this.Add("^8" + Lang.GetLabel(314), 3f, false);
			}
		}
		else if (CONST.GetGameMode() == MODE.PRORIV)
		{
			if (msgid == 26)
			{
				this.Add("^3" + Lang.GetLabel(330), 3f, true);
				return;
			}
			if (msgid == 1)
			{
				this.Add("^3" + Lang.GetLabel(315), 2.5f, true);
			}
			if (msgid == 3)
			{
				this.Add("^3" + Lang.GetLabel(316), 2.5f, false);
			}
			if (msgid == 4)
			{
				if (param > 0)
				{
					this.Add(string.Concat(new string[]
					{
						"^3",
						Lang.GetLabel(317),
						param.ToString(),
						this.getTextBySec(param).ToUpper(),
						"!"
					}), 1f, true);
				}
				else
				{
					this.Add("^1" + Lang.GetLabel(318), 2.5f, false);
				}
			}
			if (msgid != 66 && msgid != 101 && msgid != 100 && msgid != 99 && msgid != 2 && msgid != 3 && msgid != 21 && msgid != 22 && msgid != 23 && msgid != 5 && msgid != 20 && msgid != 24 && msgid != 25 && msgid != 4 && msgid != 26)
			{
				this.Add("^89", 0.9f, false);
				this.Add("^88", 0.9f, false);
				this.Add("^87", 0.9f, false);
				this.Add("^86", 0.9f, false);
				this.Add("^85", 0.9f, false);
			}
			if (msgid != 66 && msgid != 101 && msgid != 100 && msgid != 99 && msgid != 2 && msgid != 21 && msgid != 22 && msgid != 23 && msgid != 5 && msgid != 20 && msgid != 24 && msgid != 25 && msgid != 4 && msgid != 26)
			{
				this.Add("^14", 0.9f, false);
				this.Add("^13", 0.9f, false);
				this.Add("^12", 0.9f, false);
				this.Add("^11", 0.9f, false);
			}
			if (msgid == 1)
			{
				this.Add(((PlayerProfile.myteam == 0) ? "^0" : "^1") + Lang.GetLabel(319) + ((PlayerProfile.myteam == 0) ? Lang.GetLabel(497) : Lang.GetLabel(498)), 5f, false);
			}
			if (msgid == 2)
			{
				this.Add(((PlayerProfile.myteam == 0) ? "^0" : "^1") + Lang.GetLabel(499) + ((PlayerProfile.myteam == 0) ? Lang.GetLabel(500) : Lang.GetLabel(501)), 5f, false);
			}
			if (msgid == 99)
			{
				this.Add("^3 " + param.ToString() + Lang.GetLabel(502), 3f, false);
			}
			if (msgid == 100)
			{
				this.Add("^3 " + param.ToString() + Lang.GetLabel(503), 3f, false);
			}
			if (msgid == 101)
			{
				this.Add("^0 " + Lang.GetLabel(320), 3f, false);
			}
			if (msgid == 5)
			{
				if (param == 0)
				{
					this.Add("^1" + Lang.GetLabel(321), 5f, true);
				}
				else if (param == 1)
				{
					this.Add("^1" + Lang.GetLabel(322), 5f, true);
				}
				else if (param == 2)
				{
					this.Add("^0" + Lang.GetLabel(323), 5f, true);
				}
			}
			if (msgid == 20)
			{
				this.Add("^3" + Lang.GetLabel(324), 3f, false);
			}
			if (msgid == 21)
			{
				this.Add("^8" + Lang.GetLabel(325), 3f, false);
			}
			if (msgid == 22)
			{
				this.Add("^8" + Lang.GetLabel(326), 3f, false);
			}
			if (msgid == 23)
			{
				this.Add("^3" + Lang.GetLabel(327), 3f, false);
			}
			if (msgid == 24)
			{
				this.Add("^3 " + param.ToString() + Lang.GetLabel(328), 3f, false);
			}
			if (msgid == 25)
			{
				if (this.cscl == null)
				{
					this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
				}
				if (this.cscl == null)
				{
					return;
				}
				int myindex = this.cscl.myindex;
				this.Add("^8" + param.ToString() + this.getTextBySec(param) + Lang.GetLabel(329), 1f, true);
			}
		}
		else if (CONST.GetGameMode() == MODE.TANK)
		{
			if (msgid == 0)
			{
				this.Add("^8" + Lang.GetLabel(331) + param.ToString() + Lang.GetLabel(304), 1.5f, false);
			}
			if (msgid == 10)
			{
				this.Add("^8" + Lang.GetLabel(479), 1.5f, false);
			}
		}
		if (msgid == 777777)
		{
			this.Add("^1" + Lang.GetLabel(369), (float)param, false);
		}
		if (msgid == 77)
		{
			((Sound)Object.FindObjectOfType(typeof(Sound))).PlaySound_Present(GameObject.Find("Player").GetComponent<AudioSource>());
			bool flag = param > 100000;
			if (flag)
			{
				param -= 100000;
			}
			this.Add("^3" + Lang.GetLabel(370) + param.ToString() + (flag ? Lang.GetLabel(536) : Lang.GetLabel(371)), 5f, false);
		}
		if (msgid == 66)
		{
			this.Add("^1" + Lang.GetLabel(382) + param.ToString() + Lang.GetLabel(383), 10f, false);
		}
		if (msgid == 222)
		{
			this.AddSysMsg(222, param, 15f, false);
		}
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x00049D47 File Offset: 0x00047F47
	public void AddSysMsg(int mID, int param, float time, bool clear = false)
	{
		if (clear)
		{
			this.sysmsglist.Clear();
		}
		this.sysmsglist.Add(new Messages.CMsg(mID, param, time));
	}

	// Token: 0x04000835 RID: 2101
	private Texture2D NLIcon;

	// Token: 0x04000836 RID: 2102
	private List<Messages.CMsg> msglist = new List<Messages.CMsg>();

	// Token: 0x04000837 RID: 2103
	private List<Messages.CMsg> sysmsglist = new List<Messages.CMsg>();

	// Token: 0x04000838 RID: 2104
	private Client cscl;

	// Token: 0x0200086C RID: 2156
	public class CMsg
	{
		// Token: 0x06004BE4 RID: 19428 RVA: 0x001AA14E File Offset: 0x001A834E
		public CMsg(string _text, float _time)
		{
			this.text = _text;
			this.time = Time.time + _time;
		}

		// Token: 0x06004BE5 RID: 19429 RVA: 0x001AA16A File Offset: 0x001A836A
		public CMsg(string _text, float _time, float _duration)
		{
			this.text = _text;
			this.time = _time + _duration;
		}

		// Token: 0x06004BE6 RID: 19430 RVA: 0x001AA184 File Offset: 0x001A8384
		public CMsg(int _msgID, int _param, float _time)
		{
			this.time = Time.time + _time;
			this.msgID = _msgID;
			this.param = _param;
			if (this.msgID == 222)
			{
				string str = "";
				switch (RemotePlayersUpdater.Instance.Bots[_param].Team)
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
				default:
					str += "^8";
					break;
				}
				this.text = str + RemotePlayersUpdater.Instance.Bots[_param].Name + Lang.GetLabel(666);
			}
		}

		// Token: 0x06004BE7 RID: 19431 RVA: 0x001AA260 File Offset: 0x001A8460
		~CMsg()
		{
		}

		// Token: 0x040032CD RID: 13005
		public string text;

		// Token: 0x040032CE RID: 13006
		public float time;

		// Token: 0x040032CF RID: 13007
		public int msgID;

		// Token: 0x040032D0 RID: 13008
		public int param;
	}
}
