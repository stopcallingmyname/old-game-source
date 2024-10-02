using System;
using BestHTTP;
using UnityEngine;

// Token: 0x020000B6 RID: 182
public class WEB_HANDLER
{
	// Token: 0x060005C9 RID: 1481 RVA: 0x0006851A File Offset: 0x0006671A
	public static void START_GAME(OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(50, ""), _callback).Send();
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x00068534 File Offset: 0x00066734
	public static void AUTH(OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(1, "&DID=" + SystemInfo.deviceUniqueIdentifier), _callback).Send();
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x00068557 File Offset: 0x00066757
	public static void CHANGE_NICKNAME(string newName, OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(7, "&NEW_NAME=" + newName + "&ID=" + PlayerProfile.id), _callback).Send();
	}

	// Token: 0x060005CC RID: 1484 RVA: 0x00068580 File Offset: 0x00066780
	public static void GET_SERVER_LIST(OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(4, ""), _callback).Send();
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x00068599 File Offset: 0x00066799
	public static void GET_FAST_SERVER(string filters, OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(5, "&FILTERS=" + filters), _callback).Send();
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x000685B8 File Offset: 0x000667B8
	public static void GET_USER_ITEMS(OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(2, "&ACC_ID=" + PlayerProfile.id + "&GAME_SESSION=" + PlayerProfile.session), _callback).Send();
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x000685E5 File Offset: 0x000667E5
	public static void GET_USER_MISSIONS(OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(8, "&ACC_ID=" + PlayerProfile.id + "&GAME_SESSION=" + PlayerProfile.session), _callback).Send();
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x00068612 File Offset: 0x00066812
	public static void GET_DAILY_BONUS(OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(10, "&ACC_ID=" + PlayerProfile.id + "&GAME_SESSION=" + PlayerProfile.session), _callback).Send();
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x00068640 File Offset: 0x00066840
	public static void GET_MY_LVL_BONUS(OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(11, "&ACC_ID=" + PlayerProfile.id + "&GAME_SESSION=" + PlayerProfile.session), _callback).Send();
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x0006866E File Offset: 0x0006686E
	public static void BUY_ITEM(int CURRENT_ITEM_ID, OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(3, "&ACC_ID=" + PlayerProfile.id + "&ITEM_ID=" + CURRENT_ITEM_ID.ToString()), _callback).Send();
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x0006869D File Offset: 0x0006689D
	public static void OPEN_ITEM(int CURRENT_ITEM_ID, OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(9, "&ACC_ID=" + PlayerProfile.id + "&ITEM_ID=" + CURRENT_ITEM_ID.ToString()), _callback).Send();
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x000686CD File Offset: 0x000668CD
	public static void SET_SKIN(int SKIN_ID, OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(6, "&ACC_ID=" + PlayerProfile.id + "&SET=" + SKIN_ID.ToString()), _callback).Send();
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x000686FC File Offset: 0x000668FC
	public static void CHECK_USER_TICKET(OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(51, "&SESSION=" + PlayerProfile.session), _callback).Send();
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x00068720 File Offset: 0x00066920
	public static void STEAM_BUY_ITEM(ulong ITEM_ID, int REQ, OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(52, string.Concat(new string[]
		{
			"&LANG=",
			SteamHandler.SteamLang,
			"&ITEM_ID=",
			ITEM_ID.ToString(),
			"&REQ=",
			REQ.ToString()
		})), _callback).Send();
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x00068780 File Offset: 0x00066980
	public static void GET_PURCHASE_HISTORY(bool INCLUDE_CANCELED, OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(53, "&INCLUDE_CANCELED=" + (INCLUDE_CANCELED ? 1 : 0).ToString()), _callback).Send();
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x000687BC File Offset: 0x000669BC
	public static void GET_STATISTIC_TOP(int page, string search, int mode, OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(250, string.Concat(new object[]
		{
			"&acc_id=",
			PlayerProfile.id,
			"&page=",
			page,
			"&search=",
			search,
			"&mode=",
			mode
		})), _callback).Send();
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x00068828 File Offset: 0x00066A28
	public static void GET_ANGAR_GAMESCORE(int gameId, int page, OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(200, string.Concat(new object[]
		{
			"&acc_id=",
			PlayerProfile.id,
			"&game_id=",
			gameId,
			"&page=",
			page
		})), _callback).Send();
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x00068888 File Offset: 0x00066A88
	public static void SET_ANGAR_GAMESCORE(int gameId, int score, OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(201, string.Concat(new object[]
		{
			"&acc_id=",
			PlayerProfile.id,
			"&game_id=",
			gameId,
			"&score=",
			score
		})), _callback).Send();
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x000688E8 File Offset: 0x00066AE8
	public static void GET_ANGAR_GAMELIVES(int gameId, OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(202, string.Concat(new object[]
		{
			"&acc_id=",
			PlayerProfile.id,
			"&game_id=",
			gameId
		})), _callback).Send();
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x00068938 File Offset: 0x00066B38
	public static void SET_ANGAR_GAMELIVE(int gameId, int live, OnRequestFinishedDelegate _callback)
	{
		new HTTPRequest(WEB_HANDLER.URL_BUILDER(203, string.Concat(new object[]
		{
			"&acc_id=",
			PlayerProfile.id,
			"&game_id=",
			gameId,
			"&live=",
			live
		})), _callback).Send();
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x00068998 File Offset: 0x00066B98
	public static int CHECK_ERROR(string errMsg)
	{
		if (errMsg.Contains("BANNED"))
		{
			GameController.STATE = GAME_STATES.NULL;
		}
		return 1;
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x000689B0 File Offset: 0x00066BB0
	private static Uri URL_BUILDER(int cmd, string paramsLine = "")
	{
		string text = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			cmd,
			WEB_HANDLER.WRITE_STANDART(),
			paramsLine
		});
		text = text + "&SID=" + CONST.MD5(text + "a891a7d64f3f57354b6d93a89aac29ae");
		return new Uri(text);
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x00068A08 File Offset: 0x00066C08
	private static string WRITE_STANDART()
	{
		return string.Concat(new object[]
		{
			"&SOC_ID=",
			PlayerProfile.id,
			"&AUTH_KEY=",
			PlayerProfile.authkey,
			"&CURRENT_TIME=",
			Time.time
		});
	}
}
