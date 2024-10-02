using System;

// Token: 0x02000092 RID: 146
public class jscall
{
	// Token: 0x06000450 RID: 1104 RVA: 0x00054D7C File Offset: 0x00052F7C
	public static void cb_get_auth_id(string _id)
	{
		PlayerProfile.id = _id;
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x00054D84 File Offset: 0x00052F84
	public static void cb_get_network(int _network)
	{
		PlayerProfile.network = (NETWORK)_network;
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00054D8C File Offset: 0x00052F8C
	public static void cb_get_auth_key(string _key)
	{
		PlayerProfile.authkey = _key;
		if (PlayerProfile.authkey.Length > 32 && PlayerProfile.network != NETWORK.FB && PlayerProfile.network != NETWORK.KG)
		{
			string[] array = PlayerProfile.authkey.Split(new char[]
			{
				'|'
			});
			PlayerProfile.authkey = array[0];
			PlayerProfile.session = array[1];
			PlayerProfile.network = NETWORK.OK;
		}
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00054DE7 File Offset: 0x00052FE7
	public static void cb_set_key(string _key)
	{
		PlayerProfile.authkey = _key;
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x00054DEF File Offset: 0x00052FEF
	public static void cb_get_auth_country(string _country)
	{
		int.TryParse(_country, out PlayerProfile.country);
		if (PlayerProfile.country > 255 || PlayerProfile.country < 0)
		{
			PlayerProfile.country = 0;
		}
	}
}
