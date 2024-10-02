using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

// Token: 0x020000B0 RID: 176
public class CONST
{
	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000591 RID: 1425 RVA: 0x000672FE File Offset: 0x000654FE
	public static string HANDLER_SERVER
	{
		get
		{
			return "https://blockade3d.com/api_classic/handler.php?NETWORK=" + (int)PlayerProfile.network + "&CMD=";
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000592 RID: 1426 RVA: 0x00067319 File Offset: 0x00065519
	public static string HANDLER_SERVER_LIST
	{
		get
		{
			return "https://nls5.xyz/api_classic/handler.php?NETWORK=" + (int)PlayerProfile.network + "&CMD=";
		}
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x00067334 File Offset: 0x00065534
	public static string GetServerName(string IP)
	{
		if (IP == "192.168.100.205")
		{
			return "localhost";
		}
		if (IP == "95.213.130.195")
		{
			return "nls1.xyz";
		}
		if (IP == "95.213.130.197")
		{
			return "nls2.xyz";
		}
		if (IP == "78.155.193.26")
		{
			return "nls3.xyz";
		}
		if (IP == "51.38.246.60")
		{
			return "nls4.xyz";
		}
		return IP;
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x000673A1 File Offset: 0x000655A1
	public static void SetGameMode(MODE _value)
	{
		CONST.gamemode = _value;
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x000673A9 File Offset: 0x000655A9
	public static MODE GetGameMode()
	{
		return CONST.gamemode;
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x000673B0 File Offset: 0x000655B0
	public static string GET_CONTENT_URL()
	{
		return "file://" + Directory.GetCurrentDirectory() + "\\AssetBundles\\";
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x000673C8 File Offset: 0x000655C8
	public static string MD5(string strToEncrypt)
	{
		byte[] bytes = new UTF8Encoding().GetBytes(strToEncrypt);
		byte[] array = new MD5CryptoServiceProvider().ComputeHash(bytes);
		string text = "";
		for (int i = 0; i < array.Length; i++)
		{
			text += Convert.ToString(array[i], 16).PadLeft(2, '0');
		}
		return text.PadLeft(32, '0');
	}

	// Token: 0x04000BF6 RID: 3062
	public static SteamHandler STEAM_HANDLER;

	// Token: 0x04000BF7 RID: 3063
	public const string SIMPLE = "a891a7d64f3f57354b6d93a89aac29ae";

	// Token: 0x04000BF8 RID: 3064
	private static MODE gamemode = MODE.NULL;

	// Token: 0x02000893 RID: 2195
	public class VEHICLES
	{
		// Token: 0x04003346 RID: 13126
		public static int NONE = 0;

		// Token: 0x04003347 RID: 13127
		public static int TANKS = 1;

		// Token: 0x04003348 RID: 13128
		public static int JEEP = 2;

		// Token: 0x04003349 RID: 13129
		public static int POSITION_NONE = 200;

		// Token: 0x0400334A RID: 13130
		public static int POSITION_JEEP_DRIVER = 0;

		// Token: 0x0400334B RID: 13131
		public static int POSITION_JEEP_GUNNER = 1;

		// Token: 0x0400334C RID: 13132
		public static int POSITION_JEEP_PASS = 2;

		// Token: 0x0400334D RID: 13133
		public static int VEHICLE_TANK_LIGHT = 200;

		// Token: 0x0400334E RID: 13134
		public static int VEHICLE_TANK_MEDIUM = 201;

		// Token: 0x0400334F RID: 13135
		public static int VEHICLE_TANK_HEAVY = 202;

		// Token: 0x04003350 RID: 13136
		public static int VEHICLE_JEEP = 203;

		// Token: 0x04003351 RID: 13137
		public static int VEHICLE_MODUL_TANK_MG = 220;

		// Token: 0x04003352 RID: 13138
		public static int VEHICLE_MODUL_REPAIR_KIT = 221;

		// Token: 0x04003353 RID: 13139
		public static int VEHICLE_MODUL_ANTI_MISSLE = 222;

		// Token: 0x04003354 RID: 13140
		public static int VEHICLE_MODUL_SMOKE = 223;
	}

	// Token: 0x02000894 RID: 2196
	public class ENTS
	{
		// Token: 0x04003355 RID: 13141
		public static int MAX_ENTS = 512;

		// Token: 0x04003356 RID: 13142
		public static int ENT_GRENADE = 1;

		// Token: 0x04003357 RID: 13143
		public static int ENT_SHMEL = 2;

		// Token: 0x04003358 RID: 13144
		public static int ENT_ZOMBIE = 3;

		// Token: 0x04003359 RID: 13145
		public static int ENT_GP = 4;

		// Token: 0x0400335A RID: 13146
		public static int ENT_BOAT = 5;

		// Token: 0x0400335B RID: 13147
		public static int ENT_SHTURM_MINEN = 6;

		// Token: 0x0400335C RID: 13148
		public static int ENT_TURRETS = 7;

		// Token: 0x0400335D RID: 13149
		public static int ENT_TNT_PLACE = 8;

		// Token: 0x0400335E RID: 13150
		public static int ENT_FENCE = 9;

		// Token: 0x0400335F RID: 13151
		public static int ENT_ZOMBIE2 = 10;

		// Token: 0x04003360 RID: 13152
		public static int ENT_ZOMBIE_BOSS = 11;

		// Token: 0x04003361 RID: 13153
		public static int ENT_EJ = 12;

		// Token: 0x04003362 RID: 13154
		public static int ENT_TANK = 13;

		// Token: 0x04003363 RID: 13155
		public static int ENT_TANK_SNARYAD = 14;

		// Token: 0x04003364 RID: 13156
		public static int ENT_RPG = 15;

		// Token: 0x04003365 RID: 13157
		public static int ENT_TANK_LIGHT = 16;

		// Token: 0x04003366 RID: 13158
		public static int ENT_TANK_MEDIUM = 17;

		// Token: 0x04003367 RID: 13159
		public static int ENT_TANK_HEAVY = 18;

		// Token: 0x04003368 RID: 13160
		public static int ENT_ZBK18M = 19;

		// Token: 0x04003369 RID: 13161
		public static int ENT_ZOF26 = 20;

		// Token: 0x0400336A RID: 13162
		public static int ENT_MINEFLY = 21;

		// Token: 0x0400336B RID: 13163
		public static int ENT_JAVELIN = 22;

		// Token: 0x0400336C RID: 13164
		public static int ENT_ARROW = 23;

		// Token: 0x0400336D RID: 13165
		public static int ENT_SMOKE_GRENADE = 24;

		// Token: 0x0400336E RID: 13166
		public static int ENT_HE_GRENADE = 25;

		// Token: 0x0400336F RID: 13167
		public static int ENT_RKG3 = 26;

		// Token: 0x04003370 RID: 13168
		public static int ENT_MINE = 27;

		// Token: 0x04003371 RID: 13169
		public static int ENT_C4 = 28;

		// Token: 0x04003372 RID: 13170
		public static int ENT_JEEP = 29;

		// Token: 0x04003373 RID: 13171
		public static int ENT_ANTI_MISSLE = 30;

		// Token: 0x04003374 RID: 13172
		public static int ENT_SMOKE = 31;

		// Token: 0x04003375 RID: 13173
		public static int ENT_AT_MINE = 32;

		// Token: 0x04003376 RID: 13174
		public static int ENT_MOLOTOV = 33;

		// Token: 0x04003377 RID: 13175
		public static int ENT_M202 = 34;

		// Token: 0x04003378 RID: 13176
		public static int ENT_GAZ_GRENADE = 35;

		// Token: 0x04003379 RID: 13177
		public static int ENT_SNOWBALL = 36;

		// Token: 0x0400337A RID: 13178
		public static int ENT_GHOST = 37;

		// Token: 0x0400337B RID: 13179
		public static int ENT_GHOST_BOSS = 38;

		// Token: 0x0400337C RID: 13180
		public static int ENT_STIELHANDGRANATE = 39;
	}

	// Token: 0x02000895 RID: 2197
	public class SKINS
	{
	}

	// Token: 0x02000896 RID: 2198
	public class TEAMS
	{
		// Token: 0x0400337D RID: 13181
		public static int TEAM_BLUE = 0;

		// Token: 0x0400337E RID: 13182
		public static int TEAM_RED = 1;

		// Token: 0x0400337F RID: 13183
		public static int TEAM_GREEN = 2;

		// Token: 0x04003380 RID: 13184
		public static int TEAM_YELLOW = 3;
	}

	// Token: 0x02000897 RID: 2199
	public class CFG
	{
		// Token: 0x04003381 RID: 13185
		public static Version VERSION = Version.RELEASE;

		// Token: 0x04003382 RID: 13186
		public static int SERVER_UPDATE_TIMEOUT = 15;

		// Token: 0x04003383 RID: 13187
		public static string[] SOCIAL_POSTFIX = new string[]
		{
			"_vk",
			"_ok",
			"_fb",
			"_mm",
			"_kg",
			"_steam"
		};

		// Token: 0x04003384 RID: 13188
		public static int[] GAME_PORTS_OFFSET = new int[]
		{
			50000,
			30000,
			50000,
			30000,
			50000,
			30000,
			50000,
			50000,
			50000,
			50000,
			50000
		};
	}

	// Token: 0x02000898 RID: 2200
	public class EXT
	{
		// Token: 0x04003385 RID: 13189
		public static DateTime TIME_START = new DateTime(2019, 1, 1, 0, 0, 0);

		// Token: 0x04003386 RID: 13190
		public static DateTime TIME_END = new DateTime(2019, 8, 31, 23, 59, 59);
	}
}
