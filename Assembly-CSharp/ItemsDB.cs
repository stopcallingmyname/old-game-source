using System;

// Token: 0x0200003C RID: 60
public class ItemsDB
{
	// Token: 0x060001E3 RID: 483 RVA: 0x00024BCA File Offset: 0x00022DCA
	public static bool CheckItem(int ID)
	{
		return ID > 0 && ItemsDB.Items != null && ID < ItemsDB.Items.Length && ItemsDB.Items[ID] != null;
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x00024BF3 File Offset: 0x00022DF3
	public static ItemData GetItemData(int id)
	{
		if (!ItemsDB.CheckItem(id))
		{
			return null;
		}
		return ItemsDB.Items[id];
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x00024C08 File Offset: 0x00022E08
	public static void LoadMissingIcons()
	{
		int i = 0;
		int num = ItemsDB.Items.Length;
		while (i < num)
		{
			if (ItemsDB.Items[i] != null && !(ItemsDB.Items[i].icon != null))
			{
				if (ItemsDB.Items[i].Category == 29 || ItemsDB.Items[i].Category == 26 || ItemsDB.Items[i].Category == 25)
				{
					ItemData itemData = ItemsDB.Items[i];
					ITEM itemID = (ITEM)ItemsDB.Items[i].ItemID;
					itemData.icon = ContentLoader.LoadTexture(itemID.ToString() + Lang.current.ToString());
				}
				else
				{
					ItemData itemData2 = ItemsDB.Items[i];
					ITEM itemID = (ITEM)ItemsDB.Items[i].ItemID;
					itemData2.icon = ContentLoader.LoadTexture(itemID.ToString());
				}
			}
			i++;
		}
	}

	// Token: 0x040001AB RID: 427
	public static ItemData[] Items;
}
