using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class SkinManager
{
	// Token: 0x0600021F RID: 543 RVA: 0x00029A30 File Offset: 0x00027C30
	public static Texture GetBadge(int id)
	{
		int num = 0;
		if (id == 240)
		{
			num = 1;
		}
		else if (id == 241)
		{
			num = 6;
		}
		else if (id == 242)
		{
			num = 2;
		}
		else if (id == 243)
		{
			num = 3;
		}
		else if (id == 244)
		{
			num = 4;
		}
		else if (id == 245)
		{
			num = 5;
		}
		else if (id == 246)
		{
			num = 7;
		}
		else if (id == 247)
		{
			num = 8;
		}
		else if (id == 248)
		{
			num = 9;
		}
		else if (id == 249)
		{
			num = 10;
		}
		else if (id == 250)
		{
			num = 11;
		}
		else if (id == 251)
		{
			num = 12;
		}
		return SkinManager.badges[num];
	}

	// Token: 0x06000220 RID: 544 RVA: 0x00029ADB File Offset: 0x00027CDB
	public static Texture GetSkin(int team, int skin)
	{
		if (team > 3)
		{
			team = 0;
		}
		if (SkinManager.new_tex_skin == null)
		{
			return null;
		}
		if (!SkinManager.new_tex_skin.ContainsKey(skin))
		{
			return SkinManager.new_tex_skin[0][team];
		}
		return SkinManager.new_tex_skin[skin][team];
	}

	// Token: 0x06000221 RID: 545 RVA: 0x00029B15 File Offset: 0x00027D15
	public static Texture GetTankSkin(int skin, int team)
	{
		if (team > 2)
		{
			team = 0;
		}
		if (SkinManager.new_tank_skin == null)
		{
			return null;
		}
		if (!SkinManager.new_tank_skin.ContainsKey(skin))
		{
			return SkinManager.new_tank_skin[0][team];
		}
		return SkinManager.new_tank_skin[skin][team];
	}

	// Token: 0x06000222 RID: 546 RVA: 0x00029B50 File Offset: 0x00027D50
	public static void Init()
	{
		SkinManager.badges[0] = ContentLoader.LoadTexture("BADGE_DEFAULT");
		SkinManager.badges[1] = ContentLoader.LoadTexture("BADGE_70YEARS_WWII");
		SkinManager.badges[6] = ContentLoader.LoadTexture("BADGE_GLENTA");
		SkinManager.badges[2] = ContentLoader.LoadTexture("BADGE_BRITAN");
		SkinManager.badges[3] = ContentLoader.LoadTexture("BADGE_FRENCH");
		SkinManager.badges[4] = ContentLoader.LoadTexture("BADGE_GERMANY");
		SkinManager.badges[5] = ContentLoader.LoadTexture("BADGE_JAPONIA");
		SkinManager.badges[7] = ContentLoader.LoadTexture("BADGE_RUSSIA");
		SkinManager.badges[8] = ContentLoader.LoadTexture("BADGE_USA");
		SkinManager.badges[9] = ContentLoader.LoadTexture("BADGE_USSR");
		SkinManager.badges[10] = ContentLoader.LoadTexture("BADGE_UKRAINA");
		SkinManager.badges[11] = ContentLoader.LoadTexture("BADGE_BELORUSSIA");
		SkinManager.badges[12] = ContentLoader.LoadTexture("BADGE_B3D_3YEARS");
		if (SkinManager.new_tex_skin.ContainsKey(0))
		{
			return;
		}
		SkinManager.new_tex_skin.Add(0, new Texture[4]);
		SkinManager.new_tex_skin[0][0] = ContentLoader.LoadTexture("SKIN_DEFAULT_BLUE");
		SkinManager.new_tex_skin[0][1] = ContentLoader.LoadTexture("SKIN_DEFAULT_RED");
		SkinManager.new_tex_skin[0][2] = ContentLoader.LoadTexture("SKIN_DEFAULT_GREEN");
		SkinManager.new_tex_skin[0][3] = ContentLoader.LoadTexture("SKIN_DEFAULT_YELLOW");
		SkinManager.new_tex_skin.Add(1, new Texture[4]);
		SkinManager.new_tex_skin[1][0] = ContentLoader.LoadTexture("SKIN_ZOMBIE");
		SkinManager.new_tex_skin[1][1] = ContentLoader.LoadTexture("SKIN_ZOMBIE_DEFAULT");
		SkinManager.new_tex_skin[1][2] = ContentLoader.LoadTexture("SKIN_ZOMBIE_RIDER");
		SkinManager.new_tex_skin[1][3] = ContentLoader.LoadTexture("SKIN_ZOMBIE_SLENDER");
		SkinManager.new_tank_skin.Add(0, new Texture[4]);
		SkinManager.new_tank_skin[0][0] = ContentLoader.LoadTexture("SKIN_LT_DEFAULT_BLUE");
		SkinManager.new_tank_skin[0][1] = ContentLoader.LoadTexture("SKIN_LT_DEFAULT_RED");
		SkinManager.new_tank_skin[0][2] = ContentLoader.LoadTexture("SKIN_LT_DEFAULT");
		SkinManager.new_tank_skin.Add(1, new Texture[4]);
		SkinManager.new_tank_skin[1][0] = ContentLoader.LoadTexture("SKIN_MT_DEFAULT_BLUE");
		SkinManager.new_tank_skin[1][1] = ContentLoader.LoadTexture("SKIN_MT_DEFAULT_RED");
		SkinManager.new_tank_skin[1][2] = ContentLoader.LoadTexture("SKIN_MT_DEFAULT");
		SkinManager.new_tank_skin.Add(2, new Texture[4]);
		SkinManager.new_tank_skin[2][0] = ContentLoader.LoadTexture("SKIN_HT_DEFAULT_BLUE");
		SkinManager.new_tank_skin[2][1] = ContentLoader.LoadTexture("SKIN_HT_DEFAULT_RED");
		SkinManager.new_tank_skin[2][2] = ContentLoader.LoadTexture("SKIN_HT_DEFAULT");
		SkinManager.new_tank_skin.Add(3, new Texture[4]);
		SkinManager.new_tank_skin[3][0] = ContentLoader.LoadTexture("SKIN_JEEP_DEFAULT_BLUE");
		SkinManager.new_tank_skin[3][1] = ContentLoader.LoadTexture("SKIN_JEEP_DEFAULT_RED");
		SkinManager.new_tank_skin[3][2] = ContentLoader.LoadTexture("SKIN_JEEP_DEFAULT");
		foreach (ItemData itemData in ItemsDB.Items)
		{
			if (itemData != null && itemData.Category == 14 && !SkinManager.new_tex_skin.ContainsKey(itemData.ItemID))
			{
				SkinManager.new_tex_skin.Add(itemData.ItemID, new Texture[4]);
				Texture[] array = SkinManager.new_tex_skin[itemData.ItemID];
				int num = 0;
				ITEM itemID = (ITEM)itemData.ItemID;
				array[num] = ContentLoader.LoadTexture(itemID.ToString() + "_BLUE");
				Texture[] array2 = SkinManager.new_tex_skin[itemData.ItemID];
				int num2 = 1;
				itemID = (ITEM)itemData.ItemID;
				array2[num2] = ContentLoader.LoadTexture(itemID.ToString() + "_RED");
				Texture[] array3 = SkinManager.new_tex_skin[itemData.ItemID];
				int num3 = 2;
				itemID = (ITEM)itemData.ItemID;
				array3[num3] = ContentLoader.LoadTexture(itemID.ToString() + "_GREEN");
				Texture[] array4 = SkinManager.new_tex_skin[itemData.ItemID];
				int num4 = 3;
				itemID = (ITEM)itemData.ItemID;
				array4[num4] = ContentLoader.LoadTexture(itemID.ToString() + "_YELLOW");
				if (SkinManager.new_tex_skin[itemData.ItemID][0] == null)
				{
					string str = "SKIN ";
					itemID = (ITEM)itemData.ItemID;
					Debug.Log(str + itemID.ToString() + " NOT FOUND!!!!");
				}
			}
		}
		foreach (ItemData itemData2 in ItemsDB.Items)
		{
			if (itemData2 != null && itemData2.Category == 15)
			{
				if (SkinManager.new_tank_skin.ContainsKey(itemData2.ItemID))
				{
					string str2 = "SKIN ";
					ITEM itemID = (ITEM)itemData2.ItemID;
					Debug.Log(str2 + itemID.ToString() + " REGISTERED!!!!");
				}
				else
				{
					SkinManager.new_tank_skin.Add(itemData2.ItemID, new Texture[4]);
					Texture[] array5 = SkinManager.new_tank_skin[itemData2.ItemID];
					int num5 = 0;
					ITEM itemID = (ITEM)itemData2.ItemID;
					array5[num5] = ContentLoader.LoadTexture(itemID.ToString() + "_BLUE");
					Texture[] array6 = SkinManager.new_tank_skin[itemData2.ItemID];
					int num6 = 1;
					itemID = (ITEM)itemData2.ItemID;
					array6[num6] = ContentLoader.LoadTexture(itemID.ToString() + "_RED");
					if (SkinManager.new_tank_skin[itemData2.ItemID][0] == null)
					{
						string str3 = "SKIN ";
						itemID = (ITEM)itemData2.ItemID;
						Debug.Log(str3 + itemID.ToString() + " NOT FOUND!!!!");
					}
				}
			}
		}
	}

	// Token: 0x04000228 RID: 552
	private static Texture[] badges = new Texture[15];

	// Token: 0x04000229 RID: 553
	private static Dictionary<int, Texture[]> new_tex_skin = new Dictionary<int, Texture[]>();

	// Token: 0x0400022A RID: 554
	private static Dictionary<int, Texture[]> new_tank_skin = new Dictionary<int, Texture[]>();
}
