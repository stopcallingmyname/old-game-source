using System;
using UnityEngine;

// Token: 0x020000A3 RID: 163
public class Config
{
	// Token: 0x0600058A RID: 1418 RVA: 0x00066F38 File Offset: 0x00065138
	public static void Init()
	{
		Application.runInBackground = true;
		if (PlayerPrefs.HasKey(CONST.MD5("Sensitivity")))
		{
			Config.Sensitivity = PlayerPrefs.GetFloat(CONST.MD5("Sensitivity"));
		}
		else
		{
			Config.Sensitivity = 3f;
		}
		if (PlayerPrefs.HasKey(CONST.MD5("Tileset")))
		{
			Config.Tileset = PlayerPrefs.GetInt(CONST.MD5("Tileset"));
		}
		else
		{
			Config.Tileset = 0;
		}
		if (PlayerPrefs.HasKey(CONST.MD5("Dlight")))
		{
			Config.Dlight = PlayerPrefs.GetInt(CONST.MD5("Dlight"));
		}
		else
		{
			Config.Dlight = 0;
		}
		if (PlayerPrefs.HasKey(CONST.MD5("Res")))
		{
			Config.respos = PlayerPrefs.GetInt(CONST.MD5("Res"));
		}
		else
		{
			Config.respos = 0;
		}
		if (PlayerPrefs.HasKey(CONST.MD5("MenuVolume")))
		{
			Config.menuvolume = PlayerPrefs.GetFloat(CONST.MD5("MenuVolume"));
		}
		else
		{
			Config.menuvolume = 0.6f;
		}
		if (PlayerPrefs.HasKey(CONST.MD5("GameVolume")))
		{
			Config.gamevolume = PlayerPrefs.GetFloat(CONST.MD5("GameVolume"));
		}
		else
		{
			Config.gamevolume = 1f;
		}
		if (PlayerPrefs.HasKey(CONST.MD5("Dist")))
		{
			Config.distpos = PlayerPrefs.GetInt(CONST.MD5("Dist"));
		}
		else
		{
			Config.distpos = 2;
		}
		if (PlayerPrefs.HasKey(CONST.MD5("CrosshairDot")))
		{
			Config.dot = PlayerPrefs.GetInt(CONST.MD5("CrosshairDot"));
		}
		else
		{
			Config.dot = 0;
		}
		if (PlayerPrefs.HasKey(CONST.MD5("CrosshairID")))
		{
			Config.cross = PlayerPrefs.GetInt(CONST.MD5("CrosshairID"));
		}
		else
		{
			Config.cross = 0;
		}
		if (PlayerPrefs.HasKey(CONST.MD5("CrosshairR")))
		{
			Config.crossR = PlayerPrefs.GetFloat(CONST.MD5("CrosshairR"));
		}
		else
		{
			Config.crossR = 1f;
		}
		if (PlayerPrefs.HasKey(CONST.MD5("CrosshairG")))
		{
			Config.crossG = PlayerPrefs.GetFloat(CONST.MD5("CrosshairG"));
		}
		else
		{
			Config.crossG = 1f;
		}
		if (PlayerPrefs.HasKey(CONST.MD5("CrosshairB")))
		{
			Config.crossB = PlayerPrefs.GetFloat(CONST.MD5("CrosshairB"));
		}
		else
		{
			Config.crossB = 1f;
		}
		if (PlayerPrefs.HasKey(CONST.MD5("Fscreen")))
		{
			Config.Fscreen = PlayerPrefs.GetInt(CONST.MD5("Fscreen"));
		}
		else
		{
			Config.Fscreen = 0;
		}
		bool flag = Config.Fscreen > 0;
		if (Screen.fullScreen != flag)
		{
			Config.ChangeMode();
		}
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x000671C2 File Offset: 0x000653C2
	public static void SaveChatFilter()
	{
		if (Config.chat_filter)
		{
			PlayerPrefs.SetInt("ChatFilter", 1);
			PlayerPrefs.Save();
			return;
		}
		PlayerPrefs.SetInt("ChatFilter", 0);
		PlayerPrefs.Save();
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x000671EC File Offset: 0x000653EC
	public static void ChangeMode()
	{
		bool flag = !Screen.fullScreen;
		if (flag)
		{
			int num = Config.respos;
			if (num < 0)
			{
				Screen.fullScreen = !Screen.fullScreen;
			}
			else
			{
				int num2 = Screen.resolutions[num].width;
				int height = Screen.resolutions[num].height;
				if (num2 < 1024)
				{
					num2 = 1024;
					height = 768;
				}
				Screen.SetResolution(num2, height, true);
			}
		}
		else
		{
			Screen.fullScreen = !Screen.fullScreen;
		}
		if (flag)
		{
			PlayerPrefs.SetInt(CONST.MD5("Fscreen"), 1);
			Config.Fscreen = 1;
			return;
		}
		PlayerPrefs.SetInt(CONST.MD5("Fscreen"), 0);
		Config.Fscreen = 0;
	}

	// Token: 0x04000A0B RID: 2571
	public static float Sensitivity = 3f;

	// Token: 0x04000A0C RID: 2572
	public static int Dlight = 0;

	// Token: 0x04000A0D RID: 2573
	public static int Fscreen = 0;

	// Token: 0x04000A0E RID: 2574
	public static int Tileset = 0;

	// Token: 0x04000A0F RID: 2575
	public static int respos;

	// Token: 0x04000A10 RID: 2576
	public static int distpos;

	// Token: 0x04000A11 RID: 2577
	public static float menuvolume;

	// Token: 0x04000A12 RID: 2578
	public static float gamevolume;

	// Token: 0x04000A13 RID: 2579
	public static bool chat_filter;

	// Token: 0x04000A14 RID: 2580
	public static int dot = 0;

	// Token: 0x04000A15 RID: 2581
	public static int cross = 0;

	// Token: 0x04000A16 RID: 2582
	public static float crossR = 1f;

	// Token: 0x04000A17 RID: 2583
	public static float crossG = 1f;

	// Token: 0x04000A18 RID: 2584
	public static float crossB = 1f;
}
