using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009A RID: 154
public class NewYearQuest : MonoBehaviour
{
	// Token: 0x06000521 RID: 1313 RVA: 0x00060300 File Offset: 0x0005E500
	private void myGlobalInit()
	{
		if (NewYearQuest.THIS == null)
		{
			NewYearQuest.THIS = this;
		}
		if (NewYearQuest.MissionsList.Count == 0)
		{
			NewYearQuest.MissionsList.Add(1, new NYMission(1, new Vector2(-280f, 125f), Color.blue, 46f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(2, new NYMission(2, new Vector2(-293f, 94f), Color.yellow, 30f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(3, new NYMission(3, new Vector2(-267f, 76f), Color.red, 36f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(4, new NYMission(4, new Vector2(-232f, 63f), Color.green, 42f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(5, new NYMission(5, new Vector2(-193f, 52f), Color.red, 48f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(6, new NYMission(6, new Vector2(-153f, 43f), Color.green, 40f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(7, new NYMission(7, new Vector2(-115f, 33f), Color.blue, 34f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(8, new NYMission(8, new Vector2(-89f, 18f), Color.yellow, 28f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(9, new NYMission(9, new Vector2(-103f, -11f), Color.blue, 32f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(10, new NYMission(10, new Vector2(-144f, -26f), Color.yellow, 46f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(11, new NYMission(11, new Vector2(-190f, -28f), Color.red, 42f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(12, new NYMission(12, new Vector2(-232f, -29f), Color.green, 32f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(13, new NYMission(13, new Vector2(-265f, -41f), Color.red, 34f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(14, new NYMission(14, new Vector2(-234f, -71f), Color.green, 44f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(15, new NYMission(15, new Vector2(-190f, -94f), Color.blue, 42f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(16, new NYMission(16, new Vector2(-167f, -123f), Color.yellow, 26f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(17, new NYMission(17, new Vector2(-205f, -131f), Color.blue, 44f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(18, new NYMission(18, new Vector2(-248f, -126f), Color.yellow, 32f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(19, new NYMission(19, new Vector2(-244f, -154f), Color.red, 24f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(20, new NYMission(20, new Vector2(-220f, -180f), Color.green, 30f, this.AS, false, false));
			NewYearQuest.MissionsList.Add(21, new NYMission(21, new Vector2(-241f, -219f), Color.red, 64f, this.AS, true, true));
		}
		base.StartCoroutine(this.GetPassiveStatus());
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x00060770 File Offset: 0x0005E970
	public void ReloadMissionStat()
	{
		base.StartCoroutine(this.GetPassiveStatus());
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x0006077F File Offset: 0x0005E97F
	public void ShowQuest()
	{
		GM.currGUIState = GUIGS.QUEST;
		this.missions_status = false;
		base.StartCoroutine(this.GetMissionStatus());
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x0006079C File Offset: 0x0005E99C
	public void CloseQuest()
	{
		base.GetComponent<MainMenu>().OpenServerList();
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x000607AC File Offset: 0x0005E9AC
	private void OnGUI()
	{
		if (GM.currGUIState != GUIGS.QUEST)
		{
			return;
		}
		GUI.depth = -99;
		Vector2 mpos = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
		if (!this.DrawBase(mpos))
		{
			return;
		}
		if (!NewYearQuest.uploading_screenshot && this.closeButton.DrawButton(mpos))
		{
			this.CloseQuest();
		}
		if (this.selectedMission < 21 && NewYearQuest.currentMission != 21 && !NewYearQuest.uploading_screenshot && this.HelpButton.DrawButton(mpos))
		{
			PopUp.ShowBonus(2017, 1);
		}
		this.missionRect = new Rect((float)(Screen.width / 2 - 50), (float)(Screen.height / 2 + 75), 460f, 195f);
		if (NewYearQuest.MissionsList.Count > 0)
		{
			foreach (int num in NewYearQuest.MissionsList.Keys)
			{
				if (num < NewYearQuest.currentMission)
				{
					NewYearQuest.MissionsList[num].MS = PERFORMANCE_STATUS.COMPLITED;
				}
				else if (num == NewYearQuest.currentMission)
				{
					NewYearQuest.MissionsList[num].MS = PERFORMANCE_STATUS.PENDING;
				}
				else
				{
					NewYearQuest.MissionsList[num].MS = PERFORMANCE_STATUS.INACTIVE;
				}
				if (NewYearQuest.MissionsList[num].DrawMissionPoint(mpos, this.selectedMission == num) && NewYearQuest.MissionsList[num].MS != PERFORMANCE_STATUS.INACTIVE)
				{
					this.selectedMission = num;
				}
			}
			if (NewYearQuest.currentMission != 21)
			{
				NewYearQuest.MissionsList[this.selectedMission].DrawMissionText(this.missionRect);
			}
		}
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x00060968 File Offset: 0x0005EB68
	private bool DrawBase(Vector2 mpos)
	{
		Color color = GUI.color;
		GUI.color = new Color(color.r, color.g, color.b, 0.8f);
		GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), GUIManager.tex_black);
		GUI.color = color;
		if (!this.missions_status)
		{
			GUIManager.DrawLoading();
			return false;
		}
		this.mainRect = new Rect((float)(Screen.width / 2 - this.background.width / 2), (float)(Screen.height / 2 - this.background.height / 2), (float)this.background.width, (float)this.background.height);
		this.headerRect = new Rect(this.mainRect.xMax - 85f - (float)this.header.width, this.mainRect.y + 60f, (float)this.header.width, (float)this.header.height);
		this.missionRect = new Rect((float)(Screen.width / 2 - 65), (float)(Screen.height / 2 + 50), 460f, 195f);
		GUI.DrawTexture(this.mainRect, this.background);
		TextAnchor alignment = GUIManager.gs_style1.alignment;
		GUIManager.gs_style1.richText = true;
		GUIManager.gs_style1.alignment = TextAnchor.MiddleCenter;
		GUIManager.gs_style1.fontSize -= 5;
		if (NewYearQuest.currentMission < 21)
		{
			this.SkinCircle.DrawButton(mpos);
			this.PlusIcon.DrawButton(mpos);
			this.Gifts.DrawButton(mpos);
			GUI.color = Color.black;
			GUI.Label(new Rect((float)(Screen.width / 2 - 60 + 1), (float)(Screen.height / 2 + 1), 320f, 50f), Lang.GetLabel(664), GUIManager.gs_style1);
			GUI.color = Color.white;
			GUI.Label(new Rect((float)(Screen.width / 2 - 60), (float)(Screen.height / 2), 320f, 50f), Lang.GetLabel(663), GUIManager.gs_style1);
		}
		else if (NewYearQuest.currentMission > 21)
		{
			GUIManager.gs_style1.fontSize += 8;
			GUI.color = Color.black;
			GUIManager.gs_style1.alignment = TextAnchor.UpperCenter;
			GUI.Label(new Rect((float)(Screen.width / 2 - 80 + 1), (float)(Screen.height / 2 - 130 + 1), 345f, 170f), Lang.GetLabel(545), GUIManager.gs_style1);
			GUIManager.gs_style1.fontSize -= 2;
			GUIManager.gs_style1.alignment = TextAnchor.LowerCenter;
			GUI.Label(new Rect((float)(Screen.width / 2 - 80 + 1), (float)(Screen.height / 2 - 130 + 1), 345f, 170f), Lang.GetLabel(546), GUIManager.gs_style1);
			GUIManager.gs_style1.fontSize += 2;
			GUI.color = Color.white;
			GUIManager.gs_style1.alignment = TextAnchor.UpperCenter;
			GUI.Label(new Rect((float)(Screen.width / 2 - 80), (float)(Screen.height / 2 - 130), 345f, 170f), Lang.GetLabel(545), GUIManager.gs_style1);
			GUIManager.gs_style1.fontSize -= 2;
			GUIManager.gs_style1.alignment = TextAnchor.LowerCenter;
			GUI.Label(new Rect((float)(Screen.width / 2 - 80), (float)(Screen.height / 2 - 130), 345f, 170f), Lang.GetLabel(546), GUIManager.gs_style1);
			GUIManager.gs_style1.fontSize -= 6;
			if (this.selectedMission == 21)
			{
				GUI.DrawTexture(new Rect((float)(Screen.width / 2 + 50), (float)(Screen.height / 2 + 30), 195f, 180f), this.reward);
				if ((PlayerProfile.network == NETWORK.VK || PlayerProfile.network == NETWORK.FB) && !NewYearQuest.uploading_screenshot && GUIManager.DrawButton2(new Vector2(this.missionRect.xMax - 300f, this.missionRect.yMax - 40f), Vector2.zero, Lang.GetLabel(229), 1))
				{
					NewYearQuest.uploading_screenshot = true;
					if (Screen.fullScreen)
					{
						Config.ChangeMode();
					}
					if (PlayerProfile.network == NETWORK.VK)
					{
						Application.ExternalCall("getfotoserver", Array.Empty<object>());
					}
					else
					{
						base.StartCoroutine(Handler.TakeScreenshot(2));
					}
				}
			}
		}
		else if (this.GetReward.DrawButton(mpos))
		{
			base.StartCoroutine(this.GetMyReward());
		}
		GUIManager.gs_style1.fontSize += 5;
		GUIManager.gs_style1.richText = false;
		GUIManager.gs_style1.alignment = alignment;
		GUI.DrawTexture(this.headerRect, this.header);
		if (this.selectedMission <= 20 && NewYearQuest.currentMission != 21)
		{
			GUI.DrawTexture(this.missionRect, this.missionBackground);
			if ((PlayerProfile.network == NETWORK.VK || PlayerProfile.network == NETWORK.FB) && !NewYearQuest.uploading_screenshot && GUIManager.DrawButton2(new Vector2(this.missionRect.xMax - 180f, this.missionRect.yMax - 25f), Vector2.zero, Lang.GetLabel(229), 1))
			{
				NewYearQuest.uploading_screenshot = true;
				if (Screen.fullScreen)
				{
					Config.ChangeMode();
				}
				if (PlayerProfile.network == NETWORK.VK)
				{
					Application.ExternalCall("getfotoserver", Array.Empty<object>());
				}
				else
				{
					base.StartCoroutine(Handler.TakeScreenshot(2));
				}
			}
		}
		return true;
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x00060F1E File Offset: 0x0005F11E
	private IEnumerator GetMissionStatus()
	{
		string url = string.Concat(new string[]
		{
			CONST.HANDLER_SERVER,
			"300&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&questID=3"
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			if (!www.text.Contains("ERROR"))
			{
				string[] array = www.text.Split(new char[]
				{
					'^'
				});
				int.TryParse(array[0], out NewYearQuest.currentMission);
				if (NewYearQuest.currentMission > 21)
				{
					this.selectedMission = 21;
				}
				else
				{
					this.selectedMission = NewYearQuest.currentMission;
				}
				if (NewYearQuest.currentMission < 21)
				{
					NewYearQuest.MissionsList[NewYearQuest.currentMission].SetStats(array[1]);
				}
			}
			else
			{
				Debug.Log("ERROR");
			}
		}
		if (NewYearQuest.MissionsList.Count > 0)
		{
			foreach (NYMission nymission in NewYearQuest.MissionsList.Values)
			{
				nymission.DropButtonsSize();
			}
		}
		yield return null;
		this.missions_status = true;
		yield break;
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x00060F2D File Offset: 0x0005F12D
	private IEnumerator GetPassiveStatus()
	{
		string url = string.Concat(new string[]
		{
			CONST.HANDLER_SERVER,
			"300&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&questID=1"
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			www.text.Contains("ERROR");
		}
		if (NewYearQuest.MissionsList.Count > 0)
		{
			foreach (NYMission nymission in NewYearQuest.MissionsList.Values)
			{
				nymission.DropButtonsSize();
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00060F35 File Offset: 0x0005F135
	private IEnumerator GetMyReward()
	{
		string url = string.Concat(new string[]
		{
			CONST.HANDLER_SERVER,
			"302&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&questID=1"
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			if (!www.text.Contains("ERROR"))
			{
				int.TryParse(www.text, out NewYearQuest.currentMission);
				if (NewYearQuest.currentMission > 21)
				{
					this.selectedMission = 21;
					PopUp.ShowBonus(2017, 2);
					PlayerProfile.money += 50;
					Inv.needRefresh = true;
				}
			}
			else
			{
				Debug.Log("ERROR");
			}
		}
		if (NewYearQuest.MissionsList.Count > 0)
		{
			foreach (NYMission nymission in NewYearQuest.MissionsList.Values)
			{
				nymission.DropButtonsSize();
			}
		}
		yield return null;
		this.missions_status = true;
		yield break;
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x00060F44 File Offset: 0x0005F144
	public static void IncMissionTaskValueRuntime(int mID, int tID)
	{
		if (NewYearQuest.currentMission != mID)
		{
			return;
		}
		NewYearQuest.MissionsList[mID].TasksList[tID].current_value++;
		if (NewYearQuest.MissionsList[mID].TasksList[tID].current_value > NewYearQuest.MissionsList[mID].TasksList[tID].target_value)
		{
			NewYearQuest.MissionsList[mID].TasksList[tID].current_value = NewYearQuest.MissionsList[mID].TasksList[tID].target_value;
		}
	}

	// Token: 0x04000980 RID: 2432
	public static NewYearQuest THIS;

	// Token: 0x04000981 RID: 2433
	public AudioSource AS;

	// Token: 0x04000982 RID: 2434
	private Rect mainRect;

	// Token: 0x04000983 RID: 2435
	private Rect headerRect;

	// Token: 0x04000984 RID: 2436
	private Texture2D background;

	// Token: 0x04000985 RID: 2437
	private Texture2D missionBackground;

	// Token: 0x04000986 RID: 2438
	private Texture2D header;

	// Token: 0x04000987 RID: 2439
	private Texture2D reward;

	// Token: 0x04000988 RID: 2440
	private TweenButton closeButton;

	// Token: 0x04000989 RID: 2441
	private TweenButton HelpButton;

	// Token: 0x0400098A RID: 2442
	private TweenButton SkinCircle;

	// Token: 0x0400098B RID: 2443
	private TweenButton PlusIcon;

	// Token: 0x0400098C RID: 2444
	private TweenButton Gifts;

	// Token: 0x0400098D RID: 2445
	private TweenButton GetReward;

	// Token: 0x0400098E RID: 2446
	public static int currentMission = 0;

	// Token: 0x0400098F RID: 2447
	private int selectedMission;

	// Token: 0x04000990 RID: 2448
	private Rect missionRect;

	// Token: 0x04000991 RID: 2449
	public static Dictionary<int, NYMission> MissionsList = new Dictionary<int, NYMission>();

	// Token: 0x04000992 RID: 2450
	private bool missions_status;

	// Token: 0x04000993 RID: 2451
	public static bool uploading_screenshot = false;
}
