using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000037 RID: 55
public class GameController : MonoBehaviour
{
	// Token: 0x060001AD RID: 429 RVA: 0x00023374 File Offset: 0x00021574
	private void Awake()
	{
		if (GameController.THIS == null)
		{
			GameController.THIS = this;
		}
		if (GameController.STATE == GAME_STATES.GAME)
		{
			GM.currMainState = GAME_STATES.CONNECTING;
			return;
		}
		if (GameController.STATE == GAME_STATES.MAINMENU)
		{
			GM.currExtState = GAME_STATES.NULL;
			GM.currGUIState = GUIGS.SERVERLIST;
			this.BroadcastAll("myGlobalInit", "");
			ServerList.THIS.refresh_servers();
		}
	}

	// Token: 0x060001AE RID: 430 RVA: 0x000233D5 File Offset: 0x000215D5
	private void Init()
	{
		this.cb_refresh();
		GUIManager.Init(false);
		GUI3.Init();
		ItemsDB.LoadMissingIcons();
		this.BroadcastAll("myGlobalInit", "");
		ServerList.THIS.refresh_servers();
		GameController.STATE = GAME_STATES.MAINMENU;
		GM.currGUIState = GUIGS.SERVERLIST;
	}

	// Token: 0x060001AF RID: 431 RVA: 0x00023414 File Offset: 0x00021614
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F9))
		{
			Config.ChangeMode();
		}
		if (Input.GetKeyDown(KeyCode.Insert))
		{
			GM.currExtState = GAME_STATES.NULL;
			GM.currGUIState = GUIGS.NULL;
			GM.currMainState = GAME_STATES.BANNED;
			GameController.STATE = GAME_STATES.BANNED;
			SceneManager.LoadScene(0);
			return;
		}
		if (GameController.STATE == GAME_STATES.BREAK || GameController.STATE == GAME_STATES.BANNED || GameController.STATE == GAME_STATES.WAITING_FOR_GAME_DATA || GameController.STATE == GAME_STATES.LOADINGPROFILE || GameController.STATE == GAME_STATES.LOADING_BUNDLES || GameController.STATE == GAME_STATES.GET_ID_ERROR || GameController.STATE == GAME_STATES.CLIENTINITERROR || GameController.STATE == GAME_STATES.WAITING_FOR_AUTH_KEY || GameController.STATE == GAME_STATES.WAITING_FOR_FB_TOKEN || GameController.STATE == GAME_STATES.WAITING_FOR_AUTH || GameController.STATE == GAME_STATES.LOADING_BUNDLES)
		{
			return;
		}
		if (GameController.STATE == GAME_STATES.NULL)
		{
			Config.Init();
			Lang.Init();
			if (!SteamHandler.Init())
			{
				GameController.STATE = GAME_STATES.CLIENTINITERROR;
				return;
			}
			WEB_HANDLER.START_GAME(new OnRequestFinishedDelegate(this.OnRecvGameData));
			GameController.STATE = GAME_STATES.WAITING_FOR_GAME_DATA;
			return;
		}
		else if (GameController.STATE == GAME_STATES.STARTING)
		{
			if (!SteamHandler.CheckInit())
			{
				GameController.STATE = GAME_STATES.CLIENTINITERROR;
				return;
			}
			SteamHandler.GetUser();
			GameController.STATE = GAME_STATES.GETPLAYERID;
			return;
		}
		else if (GameController.STATE == GAME_STATES.WAITING_FOR_AUTH_TICKET)
		{
			if (!SteamHandler.CheckInit())
			{
				GameController.STATE = GAME_STATES.CLIENTINITERROR;
				return;
			}
			if (string.IsNullOrEmpty(PlayerProfile.authkey))
			{
				SteamHandler.GetAuthTicket();
				return;
			}
			WEB_HANDLER.CHECK_USER_TICKET(new OnRequestFinishedDelegate(this.OnRecvAuthKey));
			GameController.STATE = GAME_STATES.WAITING_FOR_AUTH_KEY;
			return;
		}
		else if (GameController.STATE == GAME_STATES.GETPLAYERID)
		{
			if (PlayerProfile.network == NETWORK.FB)
			{
				PlayerProfile.id = AccessToken.CurrentAccessToken.UserId;
				PlayerProfile.authkey = AccessToken.CurrentAccessToken.TokenString;
				Debug.Log("My Steam ID: " + PlayerProfile.id);
				Debug.Log("My AuthTicket: " + PlayerProfile.authkey);
				GameController.STATE = GAME_STATES.WAITING_FOR_AUTH_TICKET;
			}
			if (string.IsNullOrEmpty(PlayerProfile.id))
			{
				return;
			}
			if (string.IsNullOrEmpty(PlayerProfile.authkey))
			{
				SteamHandler.GetAuthTicket();
				GameController.STATE = GAME_STATES.WAITING_FOR_AUTH_TICKET;
				return;
			}
			Debug.Log("My Steam ID: " + PlayerProfile.id);
			Debug.Log("My AuthTicket: " + PlayerProfile.authkey);
			Debug.Log("My Steam Lang: " + SteamHandler.SteamLang);
			GameController.STATE = GAME_STATES.RECV_SOCIAL_DATA;
			return;
		}
		else
		{
			if (GameController.STATE == GAME_STATES.RECV_SOCIAL_DATA)
			{
				GameController.STATE = GAME_STATES.AUTH;
			}
			if (GameController.STATE == GAME_STATES.AUTH)
			{
				base.StartCoroutine(Handler.GetAUTH());
				GameController.STATE = GAME_STATES.WAITING_FOR_AUTH;
			}
			if (GameController.STATE == GAME_STATES.GETPROFILE)
			{
				base.StartCoroutine(Handler.GetProfile());
				GameController.STATE = GAME_STATES.LOADINGPROFILE;
			}
			if (GameController.STATE == GAME_STATES.INIT)
			{
				this.Init();
				return;
			}
			if (GameController.STATE == GAME_STATES.GAME)
			{
				if (Client.THIS == null)
				{
					return;
				}
				if (ZipLoader.THIS == null)
				{
					return;
				}
				if (LoadScreen.THIS == null)
				{
					return;
				}
				if (GM.currMainState == GAME_STATES.CONNECTING)
				{
					if (GM.currExtState == GAME_STATES.NULL)
					{
						LoadScreen.THIS.progress = 0;
						return;
					}
					if (GM.currExtState == GAME_STATES.GAMEAUTH)
					{
						return;
					}
					if (GM.currExtState == GAME_STATES.GAMEAUTHCOMPLITE)
					{
						GM.currExtState = GAME_STATES.GAMELOADINGMAP;
						LoadScreen.THIS.progress = 1;
						return;
					}
					if (GM.currExtState == GAME_STATES.GAMELOADINGMAP)
					{
						return;
					}
					if (GM.currExtState == GAME_STATES.GAMELOADMAPCOMPLITE)
					{
						LoadScreen.THIS.progress = 2;
						GM.currExtState = GAME_STATES.GAMELOADINGMAPCHANGES;
						Client.THIS.send_blockinfo();
						return;
					}
					if (GM.currExtState == GAME_STATES.GAMELOADINGMAPCHANGES)
					{
						return;
					}
					if (GM.currExtState == GAME_STATES.GAMELOADMAPCHANGESCOMPLITE)
					{
						LoadScreen.THIS.progress = 3;
						GM.currExtState = GAME_STATES.GAMEVISUALIZINGMAP;
						ZipLoader.THIS.WebLoadMapFinish();
						return;
					}
					if (GM.currExtState == GAME_STATES.GAMEVISUALIZINGMAP)
					{
						return;
					}
					if (GM.currExtState == GAME_STATES.GAMEVISUALIZINGMAPCOMPLITE)
					{
						LoadScreen.THIS.progress = 4;
						Client.THIS.send_myinfo();
						GM.currExtState = GAME_STATES.GAMELOADINGMYPROFILE;
						return;
					}
					if (GM.currExtState == GAME_STATES.GAMELOADINGMYPROFILE)
					{
						return;
					}
					if (GM.currExtState == GAME_STATES.GAMELOADMYPROFILECOMPLITE)
					{
						LoadScreen.THIS.progress = 5;
						base.gameObject.GetComponent<SpawnManager>().PreSpawn();
						return;
					}
				}
			}
			return;
		}
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x000237B8 File Offset: 0x000219B8
	private void BroadcastAll(string fun, object msg)
	{
		foreach (GameObject gameObject in (GameObject[])Object.FindObjectsOfType(typeof(GameObject)))
		{
			if (gameObject && gameObject.transform.parent == null)
			{
				gameObject.gameObject.BroadcastMessage(fun, msg, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x00023815 File Offset: 0x00021A15
	private void myGlobalInit()
	{
		SoundManager.Init();
		SkinManager.Init();
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x00023824 File Offset: 0x00021A24
	private void ShowPopup(int index, string _cost = "")
	{
		int param = 0;
		if (!string.IsNullOrEmpty(_cost))
		{
			int.TryParse(_cost, out param);
		}
		PopUp.ShowBonus(index, param);
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x0002384B File Offset: 0x00021A4B
	public void UpdatePlayerInfo()
	{
		base.StartCoroutine(Handler.ShareScreenshot());
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x0000248C File Offset: 0x0000068C
	private void PreAuth()
	{
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x0002385C File Offset: 0x00021A5C
	private void OnInitComplete()
	{
		FB.ActivateApp();
		Debug.Log("Success! Success Response: OnInitComplete Called " + string.Format("OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'", FB.IsLoggedIn, FB.IsInitialized));
		if (!FB.IsLoggedIn)
		{
			this.CallFBLogin();
			return;
		}
		this.OnLoggedIn();
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x000238AF File Offset: 0x00021AAF
	private void CallFBLogin()
	{
		FB.LogInWithReadPermissions(new List<string>
		{
			"user_friends"
		}, new FacebookDelegate<ILoginResult>(this.LogInWithPublishPermissionsCallback));
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x000238D2 File Offset: 0x00021AD2
	private void CallFBLoginForPublish()
	{
		FB.LogInWithPublishPermissions(new List<string>
		{
			"publish_actions"
		}, new FacebookDelegate<ILoginResult>(this.LogInWithPublishPermissionsCallback));
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x000238F5 File Offset: 0x00021AF5
	private void FBLoginCallback(IResult result)
	{
		if (FB.IsLoggedIn)
		{
			this.CallFBLoginForPublish();
		}
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x00023904 File Offset: 0x00021B04
	private void LogInWithPublishPermissionsCallback(IResult result)
	{
		if (FB.IsLoggedIn)
		{
			this.OnLoggedIn();
		}
	}

	// Token: 0x060001BA RID: 442 RVA: 0x00023913 File Offset: 0x00021B13
	private void OnLoggedIn()
	{
		GameController.STATE = GAME_STATES.GETPLAYERID;
	}

	// Token: 0x060001BB RID: 443 RVA: 0x0002391B File Offset: 0x00021B1B
	public void jsc_auth_id(string _id)
	{
		jscall.cb_get_auth_id(_id);
	}

	// Token: 0x060001BC RID: 444 RVA: 0x00023923 File Offset: 0x00021B23
	public void jsc_auth_key(string _key)
	{
		jscall.cb_get_auth_key(_key);
	}

	// Token: 0x060001BD RID: 445 RVA: 0x0002392B File Offset: 0x00021B2B
	public void jsc_upserver_url(string _url)
	{
		PlayerProfile.screenShotURL = _url;
		base.StartCoroutine(Handler.UploadPNG());
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0002393F File Offset: 0x00021B3F
	public void jsc_auth_country(string _country)
	{
		jscall.cb_get_auth_country(_country);
	}

	// Token: 0x060001BF RID: 447 RVA: 0x00023947 File Offset: 0x00021B47
	public void jsc_network(int _network)
	{
		jscall.cb_get_network(_network);
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0002394F File Offset: 0x00021B4F
	public void jsc_server_update_timeout(int _timeout)
	{
		CONST.CFG.SERVER_UPDATE_TIMEOUT = _timeout;
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x00023957 File Offset: 0x00021B57
	public void jsc_friends(string _friends)
	{
		PlayerProfile.friends = _friends;
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x00023960 File Offset: 0x00021B60
	public void jsc_friends_online(string _friendsOnline)
	{
		string[] array = _friendsOnline.Split(new char[]
		{
			','
		});
		if (array.Length != 0)
		{
			foreach (string text in array)
			{
				if (text != "")
				{
					PlayerProfile.friendsOnline.Add(text.Split(new char[]
					{
						'|'
					})[0], text.Split(new char[]
					{
						'|'
					})[1]);
				}
			}
		}
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x000239D4 File Offset: 0x00021BD4
	public void cb_refresh()
	{
		this.playerRefresh();
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x000239DC File Offset: 0x00021BDC
	public void cb_load_profile()
	{
		if (PlayerProfile.id == "0")
		{
			GameController.STATE = GAME_STATES.STARTING;
			return;
		}
		if (PlayerProfile.network == NETWORK.KG || PlayerProfile.network == NETWORK.OK)
		{
			GameController.STATE = GAME_STATES.WAITING_FOR_AUTH_TICKET;
			return;
		}
		GameController.STATE = GAME_STATES.AUTH;
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x00023A14 File Offset: 0x00021C14
	public void playerRefresh()
	{
		if (PlayerProfile.id == "0")
		{
			return;
		}
		base.StartCoroutine(Handler.get_stats_player());
		base.StartCoroutine(this.get_bonus_day());
		base.StartCoroutine(Handler.get_steam_user_info());
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x00023A4D File Offset: 0x00021C4D
	private IEnumerator get_bonus_day()
	{
		if (PlayerProfile.get_bonus_day)
		{
			yield break;
		}
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"10&id=",
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
			if (array.Length != 3)
			{
				yield break;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (!int.TryParse(array[0], out num))
			{
				yield break;
			}
			if (!int.TryParse(array[1], out num2))
			{
				yield break;
			}
			if (!int.TryParse(array[2], out num3))
			{
				yield break;
			}
			if (num == 2)
			{
				PopUp.ShowBonus(1, 0);
				PlayerProfile.money += 20;
			}
			else if (num == 1)
			{
				PopUp.ShowBonus(2, 0);
				PlayerProfile.money++;
			}
			if (num2 != 1)
			{
				NETWORK network = PlayerProfile.network;
			}
			if (num3 > 0)
			{
				PopUp.ShowBonus(3, num3);
				if (num3 == 3)
				{
					PlayerProfile.money++;
				}
				else if (num3 == 6)
				{
					PlayerProfile.money += 2;
				}
				else if (num3 == 7)
				{
					PlayerProfile.money += 10;
				}
			}
			PlayerProfile.get_bonus_day = true;
		}
		yield break;
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x00023A58 File Offset: 0x00021C58
	private void OnRecvGameData(HTTPRequest req, HTTPResponse resp)
	{
		switch (req.State)
		{
		case HTTPRequestStates.Finished:
			if (!resp.IsSuccess)
			{
				Debug.LogWarning(string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText));
				base.gameObject.SetActive(false);
				return;
			}
			if (resp.DataAsText.Contains("ERR"))
			{
				WEB_HANDLER.CHECK_ERROR(resp.DataAsText);
				return;
			}
			base.StartCoroutine(this.SoftStart(resp));
			return;
		case HTTPRequestStates.Error:
			Debug.LogError("Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
			base.gameObject.SetActive(false);
			return;
		case HTTPRequestStates.Aborted:
			Debug.LogWarning("Request Aborted!");
			base.gameObject.SetActive(false);
			return;
		case HTTPRequestStates.ConnectionTimedOut:
			Debug.LogError("Connection Timed Out!");
			base.gameObject.SetActive(false);
			return;
		case HTTPRequestStates.TimedOut:
			Debug.LogError("Processing the request Timed Out!");
			base.gameObject.SetActive(false);
			return;
		default:
			return;
		}
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x00023B81 File Offset: 0x00021D81
	private IEnumerator SoftStart(HTTPResponse response)
	{
		UTILS.BEGIN_READ(response.Data, 0);
		int items_count = UTILS.READ_LONG();
		ItemsDB.Items = new ItemData[1500];
		ItemData itemData = new ItemData(1000, 0, 0, 0, 2, 0, 0, 0, 0);
		for (int j = 0; j < 6; j++)
		{
			itemData.Upgrades[j][0] = new WeaponUpgrade(0, 0);
		}
		ItemsDB.Items[1000] = itemData;
		ItemsDB.Items[1001] = itemData;
		int num5;
		for (int i = 0; i < items_count; i = num5 + 1)
		{
			int num = UTILS.READ_LONG();
			if (num > ItemsDB.Items.Length)
			{
				Debug.LogError(string.Concat(new object[]
				{
					i,
					" ITEM ",
					num,
					" OVERFLOW!!!"
				}));
			}
			else
			{
				if (ItemsDB.CheckItem(num))
				{
					Debug.LogWarning(string.Concat(new object[]
					{
						i,
						" ITEM ",
						num,
						"ALREADY ADDED!!!"
					}));
				}
				itemData = new ItemData(num, (int)UTILS.READ_BYTE(), (int)UTILS.READ_BYTE(), (int)UTILS.READ_BYTE(), (int)UTILS.READ_BYTE(), (int)UTILS.READ_BYTE(), UTILS.READ_LONG(), UTILS.READ_LONG(), UTILS.READ_LONG());
				if (itemData.Type < 3)
				{
					for (int k = 0; k < 6; k++)
					{
						itemData.Upgrades[k][0] = new WeaponUpgrade(0, 0);
					}
					while (UTILS.READ_BYTE() != 255)
					{
						int num2 = (int)UTILS.READ_BYTE();
						int num3 = (int)UTILS.READ_BYTE();
						int num4 = UTILS.READ_LONG();
						int cost = UTILS.READ_LONG();
						itemData.Upgrades[num2][num3] = new WeaponUpgrade(num4, cost);
						if (itemData.Type == 1)
						{
							if (ItemsDB.Items[1000].Upgrades[num2][0].Val < num4)
							{
								ItemsDB.Items[1000].Upgrades[num2][0].Val = num4;
							}
						}
						else if (ItemsDB.Items[1001].Upgrades[num2][0].Val < num4)
						{
							ItemsDB.Items[1001].Upgrades[num2][0].Val = num4;
						}
					}
				}
				ItemsDB.Items[num] = itemData;
				yield return null;
			}
			num5 = i;
		}
		yield return null;
		GameController.STATE = GAME_STATES.STARTING;
		yield break;
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x00023B90 File Offset: 0x00021D90
	private void OnApplicationQuit()
	{
		SteamHandler.CloseConnection();
	}

	// Token: 0x060001CA RID: 458 RVA: 0x00023B98 File Offset: 0x00021D98
	private void OnRecvAuthKey(HTTPRequest req, HTTPResponse resp)
	{
		switch (req.State)
		{
		case HTTPRequestStates.Finished:
		{
			if (!resp.IsSuccess)
			{
				Debug.LogWarning(string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText));
				base.gameObject.SetActive(false);
				return;
			}
			if (resp.DataAsText.Contains("ERR_"))
			{
				WEB_HANDLER.CHECK_ERROR(resp.DataAsText);
				return;
			}
			UTILS.BEGIN_READ(resp.Data, 0);
			string text = UTILS.READ_STRING();
			if (string.IsNullOrEmpty(text))
			{
				GameController.STATE = GAME_STATES.STARTING;
				return;
			}
			PlayerPrefs.SetString(CONST.MD5("STEAM_USER_" + PlayerProfile.id.ToString() + "_AUTH_KEY"), text);
			PlayerPrefs.Save();
			PlayerProfile.authkey = text;
			if (PlayerProfile.network == NETWORK.KG || PlayerProfile.network == NETWORK.OK || PlayerProfile.network == NETWORK.FB)
			{
				GameController.STATE = GAME_STATES.AUTH;
				return;
			}
			GameController.STATE = GAME_STATES.GETPLAYERID;
			return;
		}
		case HTTPRequestStates.Error:
			Debug.LogError("Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
			base.gameObject.SetActive(false);
			return;
		case HTTPRequestStates.Aborted:
			Debug.LogWarning("Request Aborted!");
			base.gameObject.SetActive(false);
			return;
		case HTTPRequestStates.ConnectionTimedOut:
			Debug.LogError("Connection Timed Out!");
			base.gameObject.SetActive(false);
			return;
		case HTTPRequestStates.TimedOut:
			Debug.LogError("Processing the request Timed Out!");
			base.gameObject.SetActive(false);
			return;
		default:
			return;
		}
	}

	// Token: 0x060001CB RID: 459 RVA: 0x00023D2C File Offset: 0x00021F2C
	public void SetNewName()
	{
		base.StartCoroutine(Handler.set_name());
	}

	// Token: 0x04000178 RID: 376
	public static GAME_STATES STATE = GAME_STATES.NULL;

	// Token: 0x04000179 RID: 377
	public static GameController THIS;

	// Token: 0x0400017A RID: 378
	private float lastPone;
}
