using System;
using UnityEngine;

// Token: 0x0200003B RID: 59
public class ItemData
{
	// Token: 0x060001E2 RID: 482 RVA: 0x00024718 File Offset: 0x00022918
	public ItemData(int _ItemID, int _Type, int _Category, int _Lvl, int _ShowStatus, int _Theme, int _CostGold, int _CostSocial, int _Count)
	{
		this.ItemID = _ItemID;
		this.Type = _Type;
		this.Category = _Category;
		this.Lvl = _Lvl;
		this.CostGold = _CostGold;
		this.CostSocial = (float)_CostSocial;
		this.Count = _Count;
		this.ShowStatus = _ShowStatus;
		this.Theme = _Theme;
		this.LastCount = 0;
		if (this.Type < 3)
		{
			this.Upgrades = new WeaponUpgrade[6][];
			for (int i = 0; i < 6; i++)
			{
				this.Upgrades[i] = new WeaponUpgrade[6];
			}
			this.MyUpgrades = new int[6];
		}
		if (!ItemData.weaponCam)
		{
			ItemData.weaponCam = GameObject.Find("RenderWeapon");
		}
		int num = 512;
		int num2 = 256;
		ITEM itemID;
		if (this.Type != 3 || this.Category == 17)
		{
			if (this.weaponObject)
			{
				this.weaponObject.layer = 9;
				Renderer[] componentsInChildren = this.weaponObject.GetComponentsInChildren<Renderer>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].gameObject.layer = 9;
				}
			}
			string str = "/";
			string name = ItemData.weaponCam.name;
			string str2 = "Weapons/";
			itemID = (ITEM)this.ItemID;
			this.weaponObject = GameObject.Find(str + name + str2 + itemID.ToString());
			if (this.weaponObject)
			{
				this.weaponObject.layer = 28;
				Renderer[] componentsInChildren = this.weaponObject.GetComponentsInChildren<Renderer>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].gameObject.layer = 28;
				}
				RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 24);
				RenderTexture.active = temporary;
				ItemData.weaponCam.GetComponent<Camera>().targetTexture = temporary;
				ItemData.weaponCam.GetComponent<Camera>().Render();
				this.icon = new Texture2D(num, num2, TextureFormat.ARGB32, false);
				this.icon.ReadPixels(new Rect(0f, 0f, (float)num, (float)num2), 0, 0);
				this.icon.Apply();
				this.weaponObject.layer = 9;
				componentsInChildren = this.weaponObject.GetComponentsInChildren<Renderer>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].gameObject.layer = 9;
				}
			}
		}
		string str3 = this.refgo;
		itemID = (ITEM)this.ItemID;
		this.preview = GameObject.Find(str3 + itemID.ToString());
		if (this.preview)
		{
			this.preview.SetActive(false);
		}
		else
		{
			string str4 = this.refgoRoot;
			itemID = (ITEM)this.ItemID;
			this.preview = GameObject.Find(str4 + itemID.ToString());
			if (this.preview)
			{
				this.preview.SetActive(false);
			}
		}
		if (this.Category < 7 || this.Type == 2 || this.ItemID == 82 || this.ItemID == 315)
		{
			this.canUpgrade = true;
		}
		if (this.Category == 1 || (this.Category == 7 && this.ItemID != 210) || this.ItemID == 174 || this.ItemID == 304)
		{
			this.Animation = 1;
			return;
		}
		if (this.ItemID == 176 || this.ItemID == 302 || this.ItemID == 71 || this.ItemID == 142 || this.ItemID == 111 || this.ItemID == 107 || this.ItemID == 344 || this.ItemID == 69 || this.ItemID == 34 || this.ItemID == 47 || this.ItemID == 218 || this.ItemID == 3 || this.ItemID == 335 || this.ItemID == 336 || this.ItemID == 337 || this.ItemID == 338 || this.ItemID == 334)
		{
			this.Animation = 3;
			return;
		}
		if (this.Category == 3 || this.Category == 6 || this.Category == 4 || this.Category == 5 || this.Category == 2 || this.ItemID == 210 || this.ItemID == 82 || this.ItemID == 315)
		{
			this.Animation = 2;
		}
	}

	// Token: 0x04000193 RID: 403
	public int ItemID;

	// Token: 0x04000194 RID: 404
	public int Type;

	// Token: 0x04000195 RID: 405
	public int Category;

	// Token: 0x04000196 RID: 406
	public int ShowStatus;

	// Token: 0x04000197 RID: 407
	public int Theme;

	// Token: 0x04000198 RID: 408
	public int Animation;

	// Token: 0x04000199 RID: 409
	public int Lvl;

	// Token: 0x0400019A RID: 410
	public int CostGold;

	// Token: 0x0400019B RID: 411
	public float CostSocial;

	// Token: 0x0400019C RID: 412
	public int Count;

	// Token: 0x0400019D RID: 413
	public WeaponUpgrade[][] Upgrades;

	// Token: 0x0400019E RID: 414
	public int[] MyUpgrades;

	// Token: 0x0400019F RID: 415
	public int LastCount;

	// Token: 0x040001A0 RID: 416
	public int DateEnd;

	// Token: 0x040001A1 RID: 417
	public Texture2D icon;

	// Token: 0x040001A2 RID: 418
	public GameObject preview;

	// Token: 0x040001A3 RID: 419
	private GameObject weaponObject;

	// Token: 0x040001A4 RID: 420
	public string StoreDesc = "";

	// Token: 0x040001A5 RID: 421
	public string Desc = "";

	// Token: 0x040001A6 RID: 422
	public int module_index = -1;

	// Token: 0x040001A7 RID: 423
	public bool canUpgrade;

	// Token: 0x040001A8 RID: 424
	public string refgo = "Preview/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand/";

	// Token: 0x040001A9 RID: 425
	public string refgoRoot = "Preview";

	// Token: 0x040001AA RID: 426
	private static GameObject weaponCam;
}
