using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000095 RID: 149
public class Handler
{
	// Token: 0x060004F7 RID: 1271 RVA: 0x0005DE9A File Offset: 0x0005C09A
	public static IEnumerator GetAUTH()
	{
		Handler.www = new WWW(string.Concat(new string[]
		{
			CONST.HANDLER_SERVER,
			"3&id=",
			PlayerProfile.id,
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session
		}));
		yield return Handler.www;
		if (Handler.www.error != null)
		{
			Debug.Log("error: " + Handler.www.error);
			PlayerPrefs.DeleteKey(CONST.MD5("STEAM_USER_" + PlayerProfile.id.ToString() + "_AUTH_KEY"));
			GameController.STATE = GAME_STATES.AUTH_ERROR;
			yield break;
		}
		string text = Handler.www.text;
		if (text.Contains("ERROR"))
		{
			Handler.callBack = Handler.www.text;
			PlayerPrefs.DeleteKey(CONST.MD5("STEAM_USER_" + PlayerProfile.id.ToString() + "_AUTH_KEY"));
			GameController.STATE = GAME_STATES.AUTH_ERROR;
			yield break;
		}
		string[] array = text.Split(new char[]
		{
			'*'
		});
		if (array[0] == "OK")
		{
			PlayerProfile.gameSession = array[1];
			GameController.STATE = GAME_STATES.GETPROFILE;
			yield break;
		}
		Handler.callBack = Handler.www.text;
		PlayerPrefs.DeleteKey(CONST.MD5("STEAM_USER_" + PlayerProfile.id.ToString() + "_AUTH_KEY"));
		GameController.STATE = GAME_STATES.AUTH_ERROR;
		yield break;
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x0005DEA2 File Offset: 0x0005C0A2
	public static IEnumerator GetProfile()
	{
		yield return null;
		Handler.www = new WWW(string.Concat(new string[]
		{
			CONST.HANDLER_SERVER,
			"105&id=",
			PlayerProfile.id,
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session
		}));
		yield return Handler.www;
		if (Handler.www.error != null)
		{
			Debug.Log("error: " + Handler.www.error);
			GameController.STATE = GAME_STATES.AUTH_ERROR;
			yield break;
		}
		string[] array = Handler.www.text.Split(new char[]
		{
			'*'
		});
		if (array[0] == "OK")
		{
			PlayerProfile.myInventory = array[1];
			GameController.STATE = GAME_STATES.LOADING_BUNDLES;
			yield break;
		}
		Handler.callBack = Handler.www.text;
		PlayerProfile.loh = 1;
		GameController.STATE = GAME_STATES.BANNED;
		yield break;
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x0005DEAA File Offset: 0x0005C0AA
	public static IEnumerator set_name()
	{
		yield return null;
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"5&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&name=",
			PlayerProfile.PlayerName,
			"&time=",
			DateTime.Now.Second
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			PlayerProfile.PlayerName = www.text;
			Game.username = PlayerProfile.PlayerName;
			PlayerPrefs.SetString(CONST.MD5("NewU" + PlayerProfile.id + "Name"), PlayerProfile.PlayerName);
			PlayerPrefs.Save();
		}
		yield break;
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x0005DEB2 File Offset: 0x0005C0B2
	public static IEnumerator send_auth_ticket(string AuthTicket)
	{
		string url = string.Concat(new string[]
		{
			CONST.HANDLER_SERVER,
			"105&id=",
			PlayerProfile.id,
			"&ticket=",
			AuthTicket.ToString()
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error != null)
		{
			yield break;
		}
		if (www.text == "ERR_A" || www.text.Length < 20 || www.text.Length > 35)
		{
			yield break;
		}
		PlayerPrefs.SetString("STEAM_" + PlayerProfile.id + "_KEY", www.text);
		PlayerPrefs.Save();
		jscall.cb_set_key(www.text);
		yield break;
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x0005DEC1 File Offset: 0x0005C0C1
	public static IEnumerator get_steam_user_info()
	{
		if (PlayerPrefs.HasKey(CONST.MD5("STEAM_" + PlayerProfile.id + "_COUNTRY" + DateTime.Today.ToString())))
		{
			Handler.set_country(PlayerPrefs.GetString(CONST.MD5("STEAM_" + PlayerProfile.id + "_COUNTRY" + DateTime.Today.ToString())));
			yield break;
		}
		string url = CONST.HANDLER_SERVER + "200&id=" + PlayerProfile.id;
		WWW tmp_www = new WWW(url);
		yield return tmp_www;
		if (tmp_www.error == null)
		{
			Handler.set_country(tmp_www.text);
		}
		yield break;
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x0005DECC File Offset: 0x0005C0CC
	public static void set_country(string _country)
	{
		if (_country == "RU")
		{
			PlayerProfile.country = 1;
		}
		else if (_country == "UA")
		{
			PlayerProfile.country = 2;
		}
		else if (_country == "BY")
		{
			PlayerProfile.country = 3;
		}
		else if (_country == "KZ")
		{
			PlayerProfile.country = 4;
		}
		else if (_country == "MD")
		{
			PlayerProfile.country = 5;
		}
		else if (_country == "EE")
		{
			PlayerProfile.country = 6;
		}
		else if (_country == "LV")
		{
			PlayerProfile.country = 7;
		}
		else if (_country == "DE")
		{
			PlayerProfile.country = 8;
		}
		else if (_country == "AM")
		{
			PlayerProfile.country = 9;
		}
		else if (_country == "US")
		{
			PlayerProfile.country = 10;
		}
		else if (_country == "ad".ToUpper())
		{
			PlayerProfile.country = 11;
		}
		else if (_country == "ae".ToUpper())
		{
			PlayerProfile.country = 12;
		}
		else if (_country == "af".ToUpper())
		{
			PlayerProfile.country = 13;
		}
		else if (_country == "ag".ToUpper())
		{
			PlayerProfile.country = 14;
		}
		else if (_country == "ai".ToUpper())
		{
			PlayerProfile.country = 15;
		}
		else if (_country == "al".ToUpper())
		{
			PlayerProfile.country = 16;
		}
		else if (_country == "an".ToUpper())
		{
			PlayerProfile.country = 17;
		}
		else if (_country == "ao".ToUpper())
		{
			PlayerProfile.country = 18;
		}
		else if (_country == "ar".ToUpper())
		{
			PlayerProfile.country = 19;
		}
		else if (_country == "as".ToUpper())
		{
			PlayerProfile.country = 20;
		}
		else if (_country == "at".ToUpper())
		{
			PlayerProfile.country = 21;
		}
		else if (_country == "au".ToUpper())
		{
			PlayerProfile.country = 22;
		}
		else if (_country == "aw".ToUpper())
		{
			PlayerProfile.country = 23;
		}
		else if (_country == "ax".ToUpper())
		{
			PlayerProfile.country = 24;
		}
		else if (_country == "az".ToUpper())
		{
			PlayerProfile.country = 25;
		}
		else if (_country == "ba".ToUpper())
		{
			PlayerProfile.country = 26;
		}
		else if (_country == "bb".ToUpper())
		{
			PlayerProfile.country = 27;
		}
		else if (_country == "bd".ToUpper())
		{
			PlayerProfile.country = 28;
		}
		else if (_country == "be".ToUpper())
		{
			PlayerProfile.country = 29;
		}
		else if (_country == "bf".ToUpper())
		{
			PlayerProfile.country = 30;
		}
		else if (_country == "bg".ToUpper())
		{
			PlayerProfile.country = 31;
		}
		else if (_country == "bh".ToUpper())
		{
			PlayerProfile.country = 32;
		}
		else if (_country == "bi".ToUpper())
		{
			PlayerProfile.country = 33;
		}
		else if (_country == "bj".ToUpper())
		{
			PlayerProfile.country = 34;
		}
		else if (_country == "bm".ToUpper())
		{
			PlayerProfile.country = 35;
		}
		else if (_country == "bn".ToUpper())
		{
			PlayerProfile.country = 36;
		}
		else if (_country == "bo".ToUpper())
		{
			PlayerProfile.country = 37;
		}
		else if (_country == "br".ToUpper())
		{
			PlayerProfile.country = 38;
		}
		else if (_country == "bs".ToUpper())
		{
			PlayerProfile.country = 39;
		}
		else if (_country == "bt".ToUpper())
		{
			PlayerProfile.country = 40;
		}
		else if (_country == "bv".ToUpper())
		{
			PlayerProfile.country = 41;
		}
		else if (_country == "bw".ToUpper())
		{
			PlayerProfile.country = 42;
		}
		else if (_country == "bz".ToUpper())
		{
			PlayerProfile.country = 43;
		}
		else if (_country == "ca".ToUpper())
		{
			PlayerProfile.country = 44;
		}
		else if (_country == "cc".ToUpper())
		{
			PlayerProfile.country = 45;
		}
		else if (_country == "cd".ToUpper())
		{
			PlayerProfile.country = 46;
		}
		else if (_country == "cf".ToUpper())
		{
			PlayerProfile.country = 47;
		}
		else if (_country == "cg".ToUpper())
		{
			PlayerProfile.country = 48;
		}
		else if (_country == "ch".ToUpper())
		{
			PlayerProfile.country = 49;
		}
		else if (_country == "ci".ToUpper())
		{
			PlayerProfile.country = 50;
		}
		else if (_country == "ck".ToUpper())
		{
			PlayerProfile.country = 51;
		}
		else if (_country == "cl".ToUpper())
		{
			PlayerProfile.country = 52;
		}
		else if (_country == "cm".ToUpper())
		{
			PlayerProfile.country = 53;
		}
		else if (_country == "cn".ToUpper())
		{
			PlayerProfile.country = 54;
		}
		else if (_country == "co".ToUpper())
		{
			PlayerProfile.country = 55;
		}
		else if (_country == "cr".ToUpper())
		{
			PlayerProfile.country = 56;
		}
		else if (_country == "cs".ToUpper())
		{
			PlayerProfile.country = 57;
		}
		else if (_country == "cu".ToUpper())
		{
			PlayerProfile.country = 58;
		}
		else if (_country == "cv".ToUpper())
		{
			PlayerProfile.country = 59;
		}
		else if (_country == "cx".ToUpper())
		{
			PlayerProfile.country = 60;
		}
		else if (_country == "cy".ToUpper())
		{
			PlayerProfile.country = 61;
		}
		else if (_country == "cz".ToUpper())
		{
			PlayerProfile.country = 62;
		}
		else if (_country == "dj".ToUpper())
		{
			PlayerProfile.country = 63;
		}
		else if (_country == "dk".ToUpper())
		{
			PlayerProfile.country = 64;
		}
		else if (_country == "dm".ToUpper())
		{
			PlayerProfile.country = 65;
		}
		else if (_country == "do".ToUpper())
		{
			PlayerProfile.country = 66;
		}
		else if (_country == "dz".ToUpper())
		{
			PlayerProfile.country = 67;
		}
		else if (_country == "ec".ToUpper())
		{
			PlayerProfile.country = 68;
		}
		else if (_country == "eg".ToUpper())
		{
			PlayerProfile.country = 69;
		}
		else if (_country == "eh".ToUpper())
		{
			PlayerProfile.country = 70;
		}
		else if (_country == "er".ToUpper())
		{
			PlayerProfile.country = 71;
		}
		else if (_country == "es".ToUpper())
		{
			PlayerProfile.country = 72;
		}
		else if (_country == "et".ToUpper())
		{
			PlayerProfile.country = 73;
		}
		else if (_country == "fi".ToUpper())
		{
			PlayerProfile.country = 74;
		}
		else if (_country == "fj".ToUpper())
		{
			PlayerProfile.country = 75;
		}
		else if (_country == "fk".ToUpper())
		{
			PlayerProfile.country = 76;
		}
		else if (_country == "fm".ToUpper())
		{
			PlayerProfile.country = 77;
		}
		else if (_country == "fo".ToUpper())
		{
			PlayerProfile.country = 78;
		}
		else if (_country == "fr".ToUpper())
		{
			PlayerProfile.country = 79;
		}
		else if (_country == "ga".ToUpper())
		{
			PlayerProfile.country = 80;
		}
		else if (_country == "gb".ToUpper())
		{
			PlayerProfile.country = 81;
		}
		else if (_country == "gd".ToUpper())
		{
			PlayerProfile.country = 82;
		}
		else if (_country == "ge".ToUpper())
		{
			PlayerProfile.country = 83;
		}
		else if (_country == "gf".ToUpper())
		{
			PlayerProfile.country = 84;
		}
		else if (_country == "gh".ToUpper())
		{
			PlayerProfile.country = 85;
		}
		else if (_country == "gi".ToUpper())
		{
			PlayerProfile.country = 86;
		}
		else if (_country == "gl".ToUpper())
		{
			PlayerProfile.country = 87;
		}
		else if (_country == "gm".ToUpper())
		{
			PlayerProfile.country = 88;
		}
		else if (_country == "gn".ToUpper())
		{
			PlayerProfile.country = 89;
		}
		else if (_country == "gp".ToUpper())
		{
			PlayerProfile.country = 90;
		}
		else if (_country == "gq".ToUpper())
		{
			PlayerProfile.country = 91;
		}
		else if (_country == "gr".ToUpper())
		{
			PlayerProfile.country = 92;
		}
		else if (_country == "gs".ToUpper())
		{
			PlayerProfile.country = 93;
		}
		else if (_country == "gt".ToUpper())
		{
			PlayerProfile.country = 94;
		}
		else if (_country == "gu".ToUpper())
		{
			PlayerProfile.country = 95;
		}
		else if (_country == "gw".ToUpper())
		{
			PlayerProfile.country = 96;
		}
		else if (_country == "gy".ToUpper())
		{
			PlayerProfile.country = 97;
		}
		else if (_country == "hk".ToUpper())
		{
			PlayerProfile.country = 98;
		}
		else if (_country == "hm".ToUpper())
		{
			PlayerProfile.country = 99;
		}
		else if (_country == "hn".ToUpper())
		{
			PlayerProfile.country = 100;
		}
		else if (_country == "hr".ToUpper())
		{
			PlayerProfile.country = 101;
		}
		else if (_country == "ht".ToUpper())
		{
			PlayerProfile.country = 102;
		}
		else if (_country == "hu".ToUpper())
		{
			PlayerProfile.country = 103;
		}
		else if (_country == "id".ToUpper())
		{
			PlayerProfile.country = 104;
		}
		else if (_country == "ie".ToUpper())
		{
			PlayerProfile.country = 105;
		}
		else if (_country == "il".ToUpper())
		{
			PlayerProfile.country = 106;
		}
		else if (_country == "in".ToUpper())
		{
			PlayerProfile.country = 107;
		}
		else if (_country == "io".ToUpper())
		{
			PlayerProfile.country = 108;
		}
		else if (_country == "iq".ToUpper())
		{
			PlayerProfile.country = 109;
		}
		else if (_country == "ir".ToUpper())
		{
			PlayerProfile.country = 110;
		}
		else if (_country == "is".ToUpper())
		{
			PlayerProfile.country = 111;
		}
		else if (_country == "it".ToUpper())
		{
			PlayerProfile.country = 112;
		}
		else if (_country == "jm".ToUpper())
		{
			PlayerProfile.country = 113;
		}
		else if (_country == "jo".ToUpper())
		{
			PlayerProfile.country = 114;
		}
		else if (_country == "jp".ToUpper())
		{
			PlayerProfile.country = 115;
		}
		else if (_country == "ke".ToUpper())
		{
			PlayerProfile.country = 116;
		}
		else if (_country == "kg".ToUpper())
		{
			PlayerProfile.country = 117;
		}
		else if (_country == "kh".ToUpper())
		{
			PlayerProfile.country = 118;
		}
		else if (_country == "ki".ToUpper())
		{
			PlayerProfile.country = 119;
		}
		else if (_country == "km".ToUpper())
		{
			PlayerProfile.country = 120;
		}
		else if (_country == "kn".ToUpper())
		{
			PlayerProfile.country = 121;
		}
		else if (_country == "kp".ToUpper())
		{
			PlayerProfile.country = 122;
		}
		else if (_country == "kr".ToUpper())
		{
			PlayerProfile.country = 123;
		}
		else if (_country == "kw".ToUpper())
		{
			PlayerProfile.country = 124;
		}
		else if (_country == "ky".ToUpper())
		{
			PlayerProfile.country = 125;
		}
		else if (_country == "la".ToUpper())
		{
			PlayerProfile.country = 126;
		}
		else if (_country == "lb".ToUpper())
		{
			PlayerProfile.country = 127;
		}
		else if (_country == "lc".ToUpper())
		{
			PlayerProfile.country = 128;
		}
		else if (_country == "li".ToUpper())
		{
			PlayerProfile.country = 129;
		}
		else if (_country == "lk".ToUpper())
		{
			PlayerProfile.country = 130;
		}
		else if (_country == "lr".ToUpper())
		{
			PlayerProfile.country = 131;
		}
		else if (_country == "ls".ToUpper())
		{
			PlayerProfile.country = 132;
		}
		else if (_country == "lt".ToUpper())
		{
			PlayerProfile.country = 133;
		}
		else if (_country == "lu".ToUpper())
		{
			PlayerProfile.country = 134;
		}
		else if (_country == "ly".ToUpper())
		{
			PlayerProfile.country = 135;
		}
		else if (_country == "ma".ToUpper())
		{
			PlayerProfile.country = 136;
		}
		else if (_country == "mc".ToUpper())
		{
			PlayerProfile.country = 137;
		}
		else if (_country == "me".ToUpper())
		{
			PlayerProfile.country = 138;
		}
		else if (_country == "mg".ToUpper())
		{
			PlayerProfile.country = 139;
		}
		else if (_country == "mh".ToUpper())
		{
			PlayerProfile.country = 140;
		}
		else if (_country == "mk".ToUpper())
		{
			PlayerProfile.country = 141;
		}
		else if (_country == "ml".ToUpper())
		{
			PlayerProfile.country = 142;
		}
		else if (_country == "mm".ToUpper())
		{
			PlayerProfile.country = 143;
		}
		else if (_country == "mn".ToUpper())
		{
			PlayerProfile.country = 144;
		}
		else if (_country == "mo".ToUpper())
		{
			PlayerProfile.country = 145;
		}
		else if (_country == "mp".ToUpper())
		{
			PlayerProfile.country = 146;
		}
		else if (_country == "mq".ToUpper())
		{
			PlayerProfile.country = 147;
		}
		else if (_country == "mr".ToUpper())
		{
			PlayerProfile.country = 148;
		}
		else if (_country == "ms".ToUpper())
		{
			PlayerProfile.country = 149;
		}
		else if (_country == "mt".ToUpper())
		{
			PlayerProfile.country = 150;
		}
		else if (_country == "mu".ToUpper())
		{
			PlayerProfile.country = 151;
		}
		else if (_country == "mv".ToUpper())
		{
			PlayerProfile.country = 152;
		}
		else if (_country == "mw".ToUpper())
		{
			PlayerProfile.country = 153;
		}
		else if (_country == "mx".ToUpper())
		{
			PlayerProfile.country = 154;
		}
		else if (_country == "my".ToUpper())
		{
			PlayerProfile.country = 155;
		}
		else if (_country == "mz".ToUpper())
		{
			PlayerProfile.country = 156;
		}
		else if (_country == "na".ToUpper())
		{
			PlayerProfile.country = 157;
		}
		else if (_country == "nc".ToUpper())
		{
			PlayerProfile.country = 158;
		}
		else if (_country == "ne".ToUpper())
		{
			PlayerProfile.country = 159;
		}
		else if (_country == "nf".ToUpper())
		{
			PlayerProfile.country = 160;
		}
		else if (_country == "ng".ToUpper())
		{
			PlayerProfile.country = 161;
		}
		else if (_country == "ni".ToUpper())
		{
			PlayerProfile.country = 162;
		}
		else if (_country == "nl".ToUpper())
		{
			PlayerProfile.country = 163;
		}
		else if (_country == "no".ToUpper())
		{
			PlayerProfile.country = 164;
		}
		else if (_country == "np".ToUpper())
		{
			PlayerProfile.country = 165;
		}
		else if (_country == "nr".ToUpper())
		{
			PlayerProfile.country = 166;
		}
		else if (_country == "nu".ToUpper())
		{
			PlayerProfile.country = 167;
		}
		else if (_country == "nz".ToUpper())
		{
			PlayerProfile.country = 168;
		}
		else if (_country == "om".ToUpper())
		{
			PlayerProfile.country = 169;
		}
		else if (_country == "pa".ToUpper())
		{
			PlayerProfile.country = 170;
		}
		else if (_country == "pe".ToUpper())
		{
			PlayerProfile.country = 171;
		}
		else if (_country == "pf".ToUpper())
		{
			PlayerProfile.country = 172;
		}
		else if (_country == "pg".ToUpper())
		{
			PlayerProfile.country = 173;
		}
		else if (_country == "ph".ToUpper())
		{
			PlayerProfile.country = 174;
		}
		else if (_country == "pk".ToUpper())
		{
			PlayerProfile.country = 175;
		}
		else if (_country == "pl".ToUpper())
		{
			PlayerProfile.country = 176;
		}
		else if (_country == "pm".ToUpper())
		{
			PlayerProfile.country = 177;
		}
		else if (_country == "pn".ToUpper())
		{
			PlayerProfile.country = 178;
		}
		else if (_country == "pr".ToUpper())
		{
			PlayerProfile.country = 179;
		}
		else if (_country == "ps".ToUpper())
		{
			PlayerProfile.country = 180;
		}
		else if (_country == "pt".ToUpper())
		{
			PlayerProfile.country = 181;
		}
		else if (_country == "pw".ToUpper())
		{
			PlayerProfile.country = 182;
		}
		else if (_country == "py".ToUpper())
		{
			PlayerProfile.country = 183;
		}
		else if (_country == "qa".ToUpper())
		{
			PlayerProfile.country = 184;
		}
		else if (_country == "re".ToUpper())
		{
			PlayerProfile.country = 185;
		}
		else if (_country == "ro".ToUpper())
		{
			PlayerProfile.country = 186;
		}
		else if (_country == "rs".ToUpper())
		{
			PlayerProfile.country = 187;
		}
		else if (_country == "rw".ToUpper())
		{
			PlayerProfile.country = 188;
		}
		else if (_country == "sa".ToUpper())
		{
			PlayerProfile.country = 189;
		}
		else if (_country == "sb".ToUpper())
		{
			PlayerProfile.country = 190;
		}
		else if (_country == "sc".ToUpper())
		{
			PlayerProfile.country = 191;
		}
		else if (_country == "sd".ToUpper())
		{
			PlayerProfile.country = 192;
		}
		else if (_country == "se".ToUpper())
		{
			PlayerProfile.country = 193;
		}
		else if (_country == "sg".ToUpper())
		{
			PlayerProfile.country = 194;
		}
		else if (_country == "sh".ToUpper())
		{
			PlayerProfile.country = 195;
		}
		else if (_country == "si".ToUpper())
		{
			PlayerProfile.country = 196;
		}
		else if (_country == "sj".ToUpper())
		{
			PlayerProfile.country = 197;
		}
		else if (_country == "sk".ToUpper())
		{
			PlayerProfile.country = 198;
		}
		else if (_country == "sl".ToUpper())
		{
			PlayerProfile.country = 199;
		}
		else if (_country == "sm".ToUpper())
		{
			PlayerProfile.country = 200;
		}
		else if (_country == "sn".ToUpper())
		{
			PlayerProfile.country = 201;
		}
		else if (_country == "so".ToUpper())
		{
			PlayerProfile.country = 202;
		}
		else if (_country == "sr".ToUpper())
		{
			PlayerProfile.country = 203;
		}
		else if (_country == "st".ToUpper())
		{
			PlayerProfile.country = 204;
		}
		else if (_country == "sv".ToUpper())
		{
			PlayerProfile.country = 205;
		}
		else if (_country == "sy".ToUpper())
		{
			PlayerProfile.country = 206;
		}
		else if (_country == "sz".ToUpper())
		{
			PlayerProfile.country = 207;
		}
		else if (_country == "tc".ToUpper())
		{
			PlayerProfile.country = 208;
		}
		else if (_country == "td".ToUpper())
		{
			PlayerProfile.country = 209;
		}
		else if (_country == "tf".ToUpper())
		{
			PlayerProfile.country = 210;
		}
		else if (_country == "tg".ToUpper())
		{
			PlayerProfile.country = 211;
		}
		else if (_country == "th".ToUpper())
		{
			PlayerProfile.country = 212;
		}
		else if (_country == "tj".ToUpper())
		{
			PlayerProfile.country = 213;
		}
		else if (_country == "tk".ToUpper())
		{
			PlayerProfile.country = 214;
		}
		else if (_country == "tl".ToUpper())
		{
			PlayerProfile.country = 215;
		}
		else if (_country == "tm".ToUpper())
		{
			PlayerProfile.country = 216;
		}
		else if (_country == "tn".ToUpper())
		{
			PlayerProfile.country = 217;
		}
		else if (_country == "to".ToUpper())
		{
			PlayerProfile.country = 218;
		}
		else if (_country == "tr".ToUpper())
		{
			PlayerProfile.country = 219;
		}
		else if (_country == "tt".ToUpper())
		{
			PlayerProfile.country = 220;
		}
		else if (_country == "tv".ToUpper())
		{
			PlayerProfile.country = 221;
		}
		else if (_country == "tw".ToUpper())
		{
			PlayerProfile.country = 222;
		}
		else if (_country == "tz".ToUpper())
		{
			PlayerProfile.country = 223;
		}
		else if (_country == "ug".ToUpper())
		{
			PlayerProfile.country = 224;
		}
		else if (_country == "um".ToUpper())
		{
			PlayerProfile.country = 225;
		}
		else if (_country == "uy".ToUpper())
		{
			PlayerProfile.country = 226;
		}
		else if (_country == "uz".ToUpper())
		{
			PlayerProfile.country = 227;
		}
		else if (_country == "va".ToUpper())
		{
			PlayerProfile.country = 228;
		}
		else if (_country == "vc".ToUpper())
		{
			PlayerProfile.country = 229;
		}
		else if (_country == "ve".ToUpper())
		{
			PlayerProfile.country = 230;
		}
		else if (_country == "vg".ToUpper())
		{
			PlayerProfile.country = 231;
		}
		else if (_country == "vi".ToUpper())
		{
			PlayerProfile.country = 232;
		}
		else if (_country == "vn".ToUpper())
		{
			PlayerProfile.country = 233;
		}
		else if (_country == "vu".ToUpper())
		{
			PlayerProfile.country = 234;
		}
		else if (_country == "wf".ToUpper())
		{
			PlayerProfile.country = 235;
		}
		else if (_country == "ws".ToUpper())
		{
			PlayerProfile.country = 236;
		}
		else if (_country == "ye".ToUpper())
		{
			PlayerProfile.country = 237;
		}
		else if (_country == "yt".ToUpper())
		{
			PlayerProfile.country = 238;
		}
		else if (_country == "za".ToUpper())
		{
			PlayerProfile.country = 239;
		}
		else if (_country == "zm".ToUpper())
		{
			PlayerProfile.country = 240;
		}
		else if (_country == "zw".ToUpper())
		{
			PlayerProfile.country = 241;
		}
		else
		{
			PlayerProfile.country = 0;
		}
		if (PlayerProfile.country != 0)
		{
			PlayerPrefs.SetString(CONST.MD5("STEAM_" + PlayerProfile.id + "_COUNTRY" + DateTime.Today.ToString()), _country);
			PlayerPrefs.Save();
		}
		PlayerProfile.countrySTR = _country;
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x0005FC68 File Offset: 0x0005DE68
	public static IEnumerator WaitForRequest(WWW _www)
	{
		yield return _www;
		if (_www.error == null)
		{
			if (_www.text == "ERR_F" || _www.text == "ERR_S")
			{
				Debug.Log("WWW Error: " + _www.text);
			}
			else
			{
				string[] array = _www.text.Split(new char[]
				{
					'|'
				});
				if (!(array[0] == "AUTH") && array[0] == "REG")
				{
				}
			}
		}
		else
		{
			Debug.Log("WWW Error: " + _www.error);
		}
		yield break;
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x0005FC77 File Offset: 0x0005DE77
	public static IEnumerator GetFriendsRating()
	{
		if (PlayerPrefs.HasKey("BLOCKADE_FRIENDS_RATING_TIMER_U" + PlayerProfile.id.ToString()) && PlayerPrefs.GetFloat("BLOCKADE_FRIENDS_RATING_TIMER_U" + PlayerProfile.id.ToString()) > Time.time)
		{
			PlayerProfile.friendsRating = PlayerPrefs.GetString("BLOCKADE_FRIENDS_RATING_U" + PlayerProfile.id.ToString()).Split(new char[]
			{
				'^'
			});
			yield break;
		}
		float _timer = Time.time + 30f;
		while (PlayerProfile.friends == "")
		{
			if (_timer < Time.time)
			{
				yield break;
			}
			yield return null;
		}
		Handler.www = new WWW(string.Concat(new string[]
		{
			CONST.HANDLER_SERVER,
			"106&id=",
			PlayerProfile.id,
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&friends=",
			PlayerProfile.friends
		}));
		yield return Handler.www;
		if (Handler.www.error == null)
		{
			if (!(Handler.www.text != "ERROR"))
			{
				yield break;
			}
			PlayerPrefs.SetString("BLOCKADE_FRIENDS_RATING_U" + PlayerProfile.id.ToString(), Handler.www.text);
			PlayerPrefs.SetFloat("BLOCKADE_FRIENDS_RATING_TIMER_U" + PlayerProfile.id.ToString(), Time.time + 1800f);
			PlayerProfile.friendsRating = PlayerPrefs.GetString("BLOCKADE_FRIENDS_RATING_U" + PlayerProfile.id.ToString()).Split(new char[]
			{
				'^'
			});
		}
		yield break;
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x0005FC7F File Offset: 0x0005DE7F
	public static IEnumerator GetFriendServers()
	{
		float _timer = Time.time + 30f;
		while (PlayerProfile.friendServers == "")
		{
			if (_timer < Time.time)
			{
				yield break;
			}
			yield return null;
		}
		Handler.www = new WWW(string.Concat(new string[]
		{
			CONST.HANDLER_SERVER,
			"106&id=",
			PlayerProfile.id,
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&friends=",
			PlayerProfile.friends
		}));
		yield return Handler.www;
		if (Handler.www.error == null)
		{
			if (!(Handler.www.text != "ERROR"))
			{
				yield break;
			}
			PlayerPrefs.SetString("BLOCKADE_FRIENDS_RATING", Handler.www.text);
			PlayerPrefs.SetFloat("BLOCKADE_FRIENDS_RATING_TIMER", Time.time + 1800f);
			PlayerProfile.friendsRating = PlayerPrefs.GetString("BLOCKADE_FRIENDS_RATING").Split(new char[]
			{
				'^'
			});
		}
		yield break;
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x0005FC87 File Offset: 0x0005DE87
	public static IEnumerator reload_inv()
	{
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"9&id=",
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
			PlayerProfile.myInventory = www.text;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x0005FC8F File Offset: 0x0005DE8F
	public static IEnumerator set_attach(int _id, int _stat)
	{
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"500&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&itemid=",
			_id,
			"&stat=",
			_stat
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			if (www.text.Equals("OK"))
			{
				if (_id == 211)
				{
					if (_stat == 1)
					{
						PlayerProfile.tykva = 0;
					}
					else
					{
						PlayerProfile.tykva = 1;
					}
					PlayerProfile.kolpak = 0;
				}
				else if (_id == 222)
				{
					if (_stat == 1)
					{
						PlayerProfile.kolpak = 0;
					}
					else
					{
						PlayerProfile.kolpak = 1;
					}
					PlayerProfile.tykva = 0;
				}
				else if (_id == 223)
				{
					if (_stat == 1)
					{
						PlayerProfile.roga = 0;
					}
					else
					{
						PlayerProfile.roga = 1;
					}
				}
				else if (_id == 224)
				{
					if (_stat == 1)
					{
						PlayerProfile.mask_bear = 0;
					}
					else
					{
						PlayerProfile.mask_bear = 1;
					}
					PlayerProfile.mask_fox = 0;
					PlayerProfile.mask_rabbit = 0;
				}
				else if (_id == 225)
				{
					if (_stat == 1)
					{
						PlayerProfile.mask_fox = 0;
					}
					else
					{
						PlayerProfile.mask_fox = 1;
					}
					PlayerProfile.mask_bear = 0;
					PlayerProfile.mask_rabbit = 0;
				}
				else if (_id == 226)
				{
					if (_stat == 1)
					{
						PlayerProfile.mask_rabbit = 0;
					}
					else
					{
						PlayerProfile.mask_rabbit = 1;
					}
					PlayerProfile.mask_fox = 0;
					PlayerProfile.mask_bear = 0;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x0005FCA5 File Offset: 0x0005DEA5
	public static IEnumerator get_stats_player()
	{
		if (PlayerProfile.get_player_stats)
		{
			yield break;
		}
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"2&id=",
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
			string[] array = www.text.Split(new char[]
			{
				'|'
			});
			if (array.Length != 12)
			{
				yield break;
			}
			PlayerProfile.PlayerName = array[0];
			if (PlayerProfile.PlayerName == "Player")
			{
				PlayerProfile.PlayerName = SteamHandler.GetUserName();
				if (!string.IsNullOrEmpty(PlayerProfile.PlayerName))
				{
					GameController.THIS.SetNewName();
				}
			}
			Game.username = PlayerProfile.PlayerName;
			PlayerPrefs.SetString(CONST.MD5("NewU" + PlayerProfile.id + "Name"), PlayerProfile.PlayerName);
			PlayerPrefs.Save();
			int.TryParse(array[1], out PlayerProfile.money);
			int.TryParse(array[2], out PlayerProfile.moneypay);
			int.TryParse(array[3], out PlayerProfile.premium);
			int.TryParse(array[4], out PlayerProfile.exp);
			int.TryParse(array[5], out PlayerProfile.tykva);
			int.TryParse(array[6], out PlayerProfile.kolpak);
			int.TryParse(array[7], out PlayerProfile.roga);
			int.TryParse(array[8], out PlayerProfile.mask_bear);
			int.TryParse(array[9], out PlayerProfile.mask_fox);
			int.TryParse(array[10], out PlayerProfile.mask_rabbit);
			int.TryParse(array[11], out PlayerProfile.skin);
			PlayerProfile.level = CalcManager.CalcLevel(PlayerProfile.exp);
			PlayerProfile.get_player_stats = true;
		}
		yield break;
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x0005FCAD File Offset: 0x0005DEAD
	public static IEnumerator TakeScreenshot(int _type = 1)
	{
		yield return new WaitForEndOfFrame();
		int num = Screen.width;
		int num2 = Screen.height;
		if (_type == 2)
		{
			num = 800;
			num2 = 600;
		}
		Texture2D texture2D = new Texture2D(num, num2, TextureFormat.RGB24, false);
		texture2D.ReadPixels(new Rect((float)(Screen.width / 2 - 400), (float)(Screen.height / 2 - 300), (float)num, (float)num2), 0, 0);
		texture2D.Apply();
		byte[] contents = texture2D.EncodeToPNG();
		WWWForm wwwform = new WWWForm();
		string str = DateTime.Now.ToString("hhmmss");
		string str2 = DateTime.Now.ToString("MMddyyyy");
		wwwform.AddBinaryData("image", contents, "BLOCKADE3D " + str2 + str + ".png");
		NETWORK network = PlayerProfile.network;
		yield break;
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x0005FCBC File Offset: 0x0005DEBC
	public static IEnumerator ShareScreenshot()
	{
		yield return new WaitForEndOfFrame();
		int width = Screen.width;
		int height = Screen.height;
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, false);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
		texture2D.Apply();
		byte[] array = texture2D.EncodeToJPG();
		WWWForm wwwform = new WWWForm();
		string text = DateTime.Now.ToString("hhmmss");
		string text2 = DateTime.Now.ToString("MMddyyyy");
		WWWForm wwwform2 = wwwform;
		string fieldName = "image";
		byte[] contents = array;
		object[] array2 = new object[14];
		array2[0] = PlayerProfile.id;
		array2[1] = "_";
		int num = 2;
		int network = (int)PlayerProfile.network;
		array2[num] = network.ToString();
		array2[3] = "_";
		array2[4] = ConnectionInfo.mode;
		array2[5] = "_";
		array2[6] = RemotePlayersUpdater.Instance.Bots[Client.THIS.myindex].Stats_Kills.ToString();
		array2[7] = "_";
		array2[8] = RemotePlayersUpdater.Instance.Bots[Client.THIS.myindex].Stats_Deads.ToString();
		array2[9] = "_";
		array2[10] = text2;
		array2[11] = "_";
		array2[12] = text;
		array2[13] = ".jpg";
		wwwform2.AddBinaryData(fieldName, contents, string.Concat(array2), "image/jpg");
		wwwform.AddField("cmd", 150);
		wwwform.AddField("id", PlayerProfile.id);
		wwwform.AddField("key", PlayerProfile.authkey);
		wwwform.AddField("session", PlayerProfile.session);
		string url = CONST.HANDLER_SERVER + "150";
		WWW w = new WWW(url, wwwform);
		yield return w;
		if (w.error != null)
		{
			Debug.Log(w.error);
		}
		yield break;
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x0005FCC4 File Offset: 0x0005DEC4
	public static IEnumerator UploadPNG()
	{
		if (string.IsNullOrEmpty(PlayerProfile.screenShotURL))
		{
			yield return null;
		}
		yield return new WaitForEndOfFrame();
		int num = 800;
		int num2 = 600;
		Texture2D texture2D = new Texture2D(num, num2, TextureFormat.RGB24, false);
		texture2D.ReadPixels(new Rect((float)(Screen.width / 2 - 400), (float)(Screen.height / 2 - 300), (float)num, (float)num2), 0, 0);
		texture2D.Apply();
		byte[] contents = texture2D.EncodeToPNG();
		WWWForm wwwform = new WWWForm();
		wwwform.AddBinaryData("photo", contents);
		WWW w = new WWW(PlayerProfile.screenShotURL, wwwform);
		yield return w;
		if (!string.IsNullOrEmpty(w.error))
		{
			Debug.Log(w.error);
		}
		else
		{
			Application.ExternalCall("SocialVD2017", new object[]
			{
				w.text
			});
		}
		NewYearQuest.uploading_screenshot = false;
		yield break;
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x0005FCCC File Offset: 0x0005DECC
	public static IEnumerator GetOnlineFriends()
	{
		float _timer = Time.time + 30f;
		while (PlayerProfile.friendsOnline.Count == 0)
		{
			if (_timer < Time.time)
			{
				yield break;
			}
			yield return null;
		}
		string text = "";
		foreach (string str in PlayerProfile.friendsOnline.Keys)
		{
			text = text + str + ",";
		}
		Handler.www = new WWW(CONST.HANDLER_SERVER + "101&friends_list=" + text);
		yield return Handler.www;
		if (Handler.www.error == null)
		{
			if (!(Handler.www.text != "ERROR"))
			{
				yield break;
			}
			PlayerProfile.friendsOnlineServers = Handler.www.text.Split(new char[]
			{
				'^'
			});
		}
		yield break;
	}

	// Token: 0x0400095E RID: 2398
	private static WWW www;

	// Token: 0x0400095F RID: 2399
	public static string callBack;

	// Token: 0x02000878 RID: 2168
	private class GetUserInfoParams
	{
		// Token: 0x040032F0 RID: 13040
		public string state;

		// Token: 0x040032F1 RID: 13041
		public string country;

		// Token: 0x040032F2 RID: 13042
		public string currency;

		// Token: 0x040032F3 RID: 13043
		public string status;
	}

	// Token: 0x02000879 RID: 2169
	private class GetUserInfoResult
	{
		// Token: 0x040032F4 RID: 13044
		public string result;

		// Token: 0x040032F5 RID: 13045
		public Handler.GetUserInfoParams parameters;
	}

	// Token: 0x0200087A RID: 2170
	private class GetUserInfoAnswer
	{
		// Token: 0x040032F6 RID: 13046
		public Handler.GetUserInfoResult response;
	}
}
