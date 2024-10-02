using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B1 RID: 177
public class PlayerProfile
{
	// Token: 0x04000BF9 RID: 3065
	public static string PlayerName = "Player";

	// Token: 0x04000BFA RID: 3066
	public static string id = "0";

	// Token: 0x04000BFB RID: 3067
	public static string authkey = "nokey";

	// Token: 0x04000BFC RID: 3068
	public static string session = "nosession";

	// Token: 0x04000BFD RID: 3069
	public static string gameSession = "nosession";

	// Token: 0x04000BFE RID: 3070
	public static int country = 0;

	// Token: 0x04000BFF RID: 3071
	public static string countrySTR = "";

	// Token: 0x04000C00 RID: 3072
	public static int admin = 0;

	// Token: 0x04000C01 RID: 3073
	public static byte loh = 0;

	// Token: 0x04000C02 RID: 3074
	public static bool NY2017Q = false;

	// Token: 0x04000C03 RID: 3075
	public static bool NY2018Q = false;

	// Token: 0x04000C04 RID: 3076
	public static bool VD2017Q = false;

	// Token: 0x04000C05 RID: 3077
	public static int currentMission = 0;

	// Token: 0x04000C06 RID: 3078
	public static string screenShotURL;

	// Token: 0x04000C07 RID: 3079
	public static string myInventory = "";

	// Token: 0x04000C08 RID: 3080
	public static bool load_player = false;

	// Token: 0x04000C09 RID: 3081
	public static bool get_bonus_day = false;

	// Token: 0x04000C0A RID: 3082
	public static bool get_player_stats = false;

	// Token: 0x04000C0B RID: 3083
	public static int money = 0;

	// Token: 0x04000C0C RID: 3084
	public static int moneypay = 0;

	// Token: 0x04000C0D RID: 3085
	public static int premium = 0;

	// Token: 0x04000C0E RID: 3086
	public static int exp = 0;

	// Token: 0x04000C0F RID: 3087
	public static int tykva = 0;

	// Token: 0x04000C10 RID: 3088
	public static int kolpak = 0;

	// Token: 0x04000C11 RID: 3089
	public static int roga = 0;

	// Token: 0x04000C12 RID: 3090
	public static int mask_bear = 0;

	// Token: 0x04000C13 RID: 3091
	public static int mask_fox = 0;

	// Token: 0x04000C14 RID: 3092
	public static int mask_rabbit = 0;

	// Token: 0x04000C15 RID: 3093
	public static int level = 0;

	// Token: 0x04000C16 RID: 3094
	public static int skin = 0;

	// Token: 0x04000C17 RID: 3095
	public static NETWORK network = NETWORK.ST;

	// Token: 0x04000C18 RID: 3096
	public static string friends = "";

	// Token: 0x04000C19 RID: 3097
	public static string friendServers = "";

	// Token: 0x04000C1A RID: 3098
	public static Dictionary<string, string> friendsOnline = new Dictionary<string, string>();

	// Token: 0x04000C1B RID: 3099
	public static string[] friendsOnlineServers = new string[0];

	// Token: 0x04000C1C RID: 3100
	public static string[] friendsRating = new string[0];

	// Token: 0x04000C1D RID: 3101
	public static int myindex = 0;

	// Token: 0x04000C1E RID: 3102
	public static int myteam = -1;

	// Token: 0x04000C1F RID: 3103
	public static Dictionary<int, Texture2D> crossList = new Dictionary<int, Texture2D>();

	// Token: 0x04000C20 RID: 3104
	public static Dictionary<int, Texture2D> crossDot = new Dictionary<int, Texture2D>();

	// Token: 0x04000C21 RID: 3105
	public static Color crossColor;
}
