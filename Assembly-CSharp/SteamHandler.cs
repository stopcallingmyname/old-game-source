using System;
using System.Text;
using BestHTTP;
using Facepunch.Steamworks;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class SteamHandler : MonoBehaviour
{
	// Token: 0x06000232 RID: 562 RVA: 0x0002B3C8 File Offset: 0x000295C8
	private void Start()
	{
		if (CONST.STEAM_HANDLER != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		CONST.STEAM_HANDLER = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0002B3F4 File Offset: 0x000295F4
	public static bool Init()
	{
		Facepunch.Steamworks.Config.ForUnity(Application.platform.ToString());
		new Facepunch.Steamworks.Client(1049800U);
		if (Facepunch.Steamworks.Client.Instance == null)
		{
			Debug.LogError("Error starting Steam!");
			return false;
		}
		Facepunch.Steamworks.Client.Instance.MicroTransactions.OnAuthorizationResponse += delegate(bool authorized, int appId, ulong orderId)
		{
			WEB_HANDLER.STEAM_BUY_ITEM(orderId, 2, new OnRequestFinishedDelegate(Gold.OnSteamBuyItemFinish));
		};
		return true;
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0002B466 File Offset: 0x00029666
	public static bool CheckInit()
	{
		return Facepunch.Steamworks.Client.Instance != null;
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0002B472 File Offset: 0x00029672
	public static string GetUserName()
	{
		if (Facepunch.Steamworks.Client.Instance == null)
		{
			return "";
		}
		return Facepunch.Steamworks.Client.Instance.Username;
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0002B48C File Offset: 0x0002968C
	public static void GetUser()
	{
		if (Facepunch.Steamworks.Client.Instance == null)
		{
			return;
		}
		PlayerProfile.id = Facepunch.Steamworks.Client.Instance.SteamId.ToString();
		PlayerProfile.authkey = PlayerPrefs.GetString(CONST.MD5("STEAM_USER_" + PlayerProfile.id.ToString() + "_AUTH_KEY"), string.Empty);
		int num = 0;
		if (PlayerPrefs.HasKey(CONST.MD5("DefLanguage")))
		{
			num = PlayerPrefs.GetInt(CONST.MD5("DefLanguage"));
			if (num == 0)
			{
				SteamHandler.SteamLang = "ru";
			}
			else
			{
				SteamHandler.SteamLang = "en";
				num = 1;
			}
		}
		else
		{
			SteamHandler.SteamLang = Facepunch.Steamworks.Client.Instance.CurrentLanguage;
			if (SteamHandler.SteamLang.Contains("rus"))
			{
				SteamHandler.SteamLang = "ru";
			}
			else
			{
				SteamHandler.SteamLang = "en";
				num = 1;
			}
			PlayerPrefs.SetInt(CONST.MD5("DefLanguage"), num);
			PlayerPrefs.Save();
		}
		Lang.current = num;
	}

	// Token: 0x06000237 RID: 567 RVA: 0x0002B578 File Offset: 0x00029778
	public static void GetAuthTicket()
	{
		if (Facepunch.Steamworks.Client.Instance == null)
		{
			return;
		}
		PlayerProfile.authkey = SteamHandler.GetSteamAuthTicket();
	}

	// Token: 0x06000238 RID: 568 RVA: 0x0002B58C File Offset: 0x0002978C
	public static string GetSteamAuthTicket()
	{
		if (Facepunch.Steamworks.Client.Instance == null)
		{
			return string.Empty;
		}
		SteamHandler._appTicket = Facepunch.Steamworks.Client.Instance.Auth.GetAuthSessionTicket();
		if (SteamHandler._appTicket == null)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (byte b in SteamHandler._appTicket.Data)
		{
			stringBuilder.AppendFormat("{0:x2}", b);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000239 RID: 569 RVA: 0x0002B602 File Offset: 0x00029802
	public static void GetUserAvatar(ulong _steamID)
	{
		if (Facepunch.Steamworks.Client.Instance == null)
		{
			return;
		}
		Facepunch.Steamworks.Client.Instance.Friends.GetAvatar(Friends.AvatarSize.Medium, _steamID, new Action<Image>(SteamHandler.OnImage));
	}

	// Token: 0x0600023A RID: 570 RVA: 0x0002B62C File Offset: 0x0002982C
	public static bool GetFriends()
	{
		if (Facepunch.Steamworks.Client.Instance == null)
		{
			return true;
		}
		foreach (SteamFriend steamFriend in Facepunch.Steamworks.Client.Instance.Friends.AllFriends)
		{
			if (steamFriend.IsPlayingThisGame && !PlayerProfile.friendsOnline.ContainsKey(steamFriend.Id.ToString()))
			{
				PlayerProfile.friendsOnline.Add(steamFriend.Id.ToString(), steamFriend.Name);
			}
		}
		return true;
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0002B6C8 File Offset: 0x000298C8
	private static void OnImage(Image tmp)
	{
		Texture2D texture2D = new Texture2D(tmp.Width, tmp.Height, TextureFormat.ARGB32, false);
		int i = 0;
		int width = tmp.Width;
		while (i < width)
		{
			int j = 0;
			int height = tmp.Height;
			while (j < height)
			{
				Facepunch.Steamworks.Color pixel = tmp.GetPixel(i, j);
				texture2D.SetPixel(i, tmp.Height - j, new UnityEngine.Color((float)pixel.r / 255f, (float)pixel.g / 255f, (float)pixel.b / 255f, (float)pixel.a / 255f));
				j++;
			}
			i++;
		}
		texture2D.Apply(false);
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0002B76E File Offset: 0x0002996E
	private void OnDestroy()
	{
		if (Facepunch.Steamworks.Client.Instance != null)
		{
			Facepunch.Steamworks.Client.Instance.Dispose();
		}
	}

	// Token: 0x0600023D RID: 573 RVA: 0x0002B76E File Offset: 0x0002996E
	public static void CloseConnection()
	{
		if (Facepunch.Steamworks.Client.Instance != null)
		{
			Facepunch.Steamworks.Client.Instance.Dispose();
		}
	}

	// Token: 0x0600023E RID: 574 RVA: 0x0002B781 File Offset: 0x00029981
	private void Update()
	{
		if (Facepunch.Steamworks.Client.Instance != null)
		{
			Facepunch.Steamworks.Client.Instance.Update();
		}
	}

	// Token: 0x040002F0 RID: 752
	private static Auth.Ticket _appTicket;

	// Token: 0x040002F1 RID: 753
	public static string SteamLang;
}
