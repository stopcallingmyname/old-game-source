using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class vp_SimpleInventory : MonoBehaviour
{
	// Token: 0x0600089B RID: 2203 RVA: 0x0007BE8D File Offset: 0x0007A08D
	protected virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x0007BEA9 File Offset: 0x0007A0A9
	protected virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x0007BEC5 File Offset: 0x0007A0C5
	private void Awake()
	{
		this.m_Player = (vp_FPPlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_FPPlayerEventHandler));
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x0600089E RID: 2206 RVA: 0x0007BEEC File Offset: 0x0007A0EC
	protected Dictionary<string, vp_SimpleInventory.InventoryItemStatus> ItemStatusDictionary
	{
		get
		{
			if (this.m_ItemStatusDictionary == null)
			{
				this.m_ItemStatusDictionary = new Dictionary<string, vp_SimpleInventory.InventoryItemStatus>();
				for (int i = this.m_ItemTypes.Count - 1; i > -1; i--)
				{
					if (!this.m_ItemStatusDictionary.ContainsKey(this.m_ItemTypes[i].Name))
					{
						this.m_ItemStatusDictionary.Add(this.m_ItemTypes[i].Name, this.m_ItemTypes[i]);
					}
					else
					{
						this.m_ItemTypes.Remove(this.m_ItemTypes[i]);
					}
				}
				for (int j = this.m_WeaponTypes.Count - 1; j > -1; j--)
				{
					if (!this.m_ItemStatusDictionary.ContainsKey(this.m_WeaponTypes[j].Name))
					{
						this.m_ItemStatusDictionary.Add(this.m_WeaponTypes[j].Name, this.m_WeaponTypes[j]);
					}
					else
					{
						this.m_WeaponTypes.Remove(this.m_WeaponTypes[j]);
					}
				}
			}
			return this.m_ItemStatusDictionary;
		}
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x0007C008 File Offset: 0x0007A208
	public bool HaveItem(object name)
	{
		vp_SimpleInventory.InventoryItemStatus inventoryItemStatus;
		return this.ItemStatusDictionary.TryGetValue((string)name, out inventoryItemStatus) && inventoryItemStatus.Have >= 1;
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x0007C038 File Offset: 0x0007A238
	private vp_SimpleInventory.InventoryItemStatus GetItemStatus(string name)
	{
		vp_SimpleInventory.InventoryItemStatus result;
		if (!this.ItemStatusDictionary.TryGetValue(name, out result))
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Error: (",
				this,
				"). Unknown item type: '",
				name,
				"'."
			}));
		}
		return result;
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x0007C088 File Offset: 0x0007A288
	private vp_SimpleInventory.InventoryWeaponStatus GetWeaponStatus(string name)
	{
		if (name == null)
		{
			return null;
		}
		vp_SimpleInventory.InventoryItemStatus inventoryItemStatus;
		if (!this.ItemStatusDictionary.TryGetValue(name, out inventoryItemStatus))
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Error: (",
				this,
				"). Unknown item type: '",
				name,
				"'."
			}));
			return null;
		}
		if (inventoryItemStatus.GetType() != typeof(vp_SimpleInventory.InventoryWeaponStatus))
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Error: (",
				this,
				"). Item is not a weapon: '",
				name,
				"'."
			}));
			return null;
		}
		return (vp_SimpleInventory.InventoryWeaponStatus)inventoryItemStatus;
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x0007C12C File Offset: 0x0007A32C
	protected void RefreshWeaponStatus()
	{
		if (!this.m_Player.CurrentWeaponWielded.Get() && this.m_RefreshWeaponStatusIterations < 50)
		{
			this.m_RefreshWeaponStatusIterations++;
			vp_Timer.In(0.1f, new vp_Timer.Callback(this.RefreshWeaponStatus), null);
			return;
		}
		this.m_RefreshWeaponStatusIterations = 0;
		string text = this.m_Player.CurrentWeaponName.Get();
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		this.m_CurrentWeaponStatus = this.GetWeaponStatus(text);
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x060008A3 RID: 2211 RVA: 0x0007C1B3 File Offset: 0x0007A3B3
	// (set) Token: 0x060008A4 RID: 2212 RVA: 0x0007C1CA File Offset: 0x0007A3CA
	protected virtual int OnValue_CurrentWeaponAmmoCount
	{
		get
		{
			if (this.m_CurrentWeaponStatus == null)
			{
				return 0;
			}
			return this.m_CurrentWeaponStatus.LoadedAmmo;
		}
		set
		{
			if (this.m_CurrentWeaponStatus == null)
			{
				return;
			}
			this.m_CurrentWeaponStatus.LoadedAmmo = value;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x060008A5 RID: 2213 RVA: 0x0007C1E4 File Offset: 0x0007A3E4
	protected virtual int OnValue_CurrentWeaponClipCount
	{
		get
		{
			if (this.m_CurrentWeaponStatus == null)
			{
				return 0;
			}
			vp_SimpleInventory.InventoryItemStatus inventoryItemStatus;
			if (!this.ItemStatusDictionary.TryGetValue(this.m_CurrentWeaponStatus.ClipType, out inventoryItemStatus))
			{
				return 0;
			}
			return inventoryItemStatus.Have;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x060008A6 RID: 2214 RVA: 0x0007C21D File Offset: 0x0007A41D
	protected virtual string OnValue_CurrentWeaponClipType
	{
		get
		{
			if (this.m_CurrentWeaponStatus == null)
			{
				return "";
			}
			return this.m_CurrentWeaponStatus.ClipType;
		}
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x0007C238 File Offset: 0x0007A438
	protected virtual int OnMessage_GetItemCount(string name)
	{
		vp_SimpleInventory.InventoryItemStatus inventoryItemStatus;
		if (!this.ItemStatusDictionary.TryGetValue(name, out inventoryItemStatus))
		{
			return 0;
		}
		return inventoryItemStatus.Have;
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x0007C25D File Offset: 0x0007A45D
	protected virtual bool OnAttempt_DepleteAmmo()
	{
		if (this.m_CurrentWeaponStatus == null)
		{
			return false;
		}
		if (this.m_CurrentWeaponStatus.LoadedAmmo < 1)
		{
			return this.m_CurrentWeaponStatus.MaxAmmo == 0;
		}
		this.m_CurrentWeaponStatus.LoadedAmmo--;
		return true;
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x0007C29C File Offset: 0x0007A49C
	protected virtual bool OnAttempt_AddAmmo(object arg)
	{
		object[] array = (object[])arg;
		string name = (string)array[0];
		int num = (array.Length == 2) ? ((int)array[1]) : -1;
		vp_SimpleInventory.InventoryWeaponStatus weaponStatus = this.GetWeaponStatus(name);
		if (weaponStatus == null)
		{
			return false;
		}
		if (num == -1)
		{
			weaponStatus.LoadedAmmo = weaponStatus.MaxAmmo;
		}
		else
		{
			weaponStatus.LoadedAmmo = Mathf.Min(num, weaponStatus.MaxAmmo);
		}
		return true;
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x0007C300 File Offset: 0x0007A500
	protected virtual bool OnAttempt_AddItem(object args)
	{
		object[] array = (object[])args;
		string name = (string)array[0];
		int num = (array.Length == 2) ? ((int)array[1]) : 1;
		vp_SimpleInventory.InventoryItemStatus itemStatus = this.GetItemStatus(name);
		if (itemStatus == null)
		{
			return false;
		}
		itemStatus.CanHave = Mathf.Max(1, itemStatus.CanHave);
		if (itemStatus.Have >= itemStatus.CanHave)
		{
			return false;
		}
		itemStatus.Have = Mathf.Min(itemStatus.Have + num, itemStatus.CanHave);
		return true;
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x0007C378 File Offset: 0x0007A578
	protected virtual bool OnAttempt_RemoveItem(object args)
	{
		object[] array = (object[])args;
		string name = (string)array[0];
		int num = (array.Length == 2) ? ((int)array[1]) : 1;
		vp_SimpleInventory.InventoryItemStatus itemStatus = this.GetItemStatus(name);
		if (itemStatus == null)
		{
			return false;
		}
		if (itemStatus.Have <= 0)
		{
			return false;
		}
		itemStatus.Have = Mathf.Max(itemStatus.Have - num, 0);
		return true;
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x0007C3D4 File Offset: 0x0007A5D4
	protected virtual bool OnAttempt_RemoveClip()
	{
		return this.m_CurrentWeaponStatus != null && this.GetItemStatus(this.m_CurrentWeaponStatus.ClipType) != null && this.m_CurrentWeaponStatus.LoadedAmmo < this.m_CurrentWeaponStatus.MaxAmmo && this.m_Player.RemoveItem.Try(new object[]
		{
			this.m_CurrentWeaponStatus.ClipType
		});
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x0007C448 File Offset: 0x0007A648
	protected virtual bool CanStart_SetWeapon()
	{
		int num = (int)this.m_Player.SetWeapon.Argument;
		return num == 0 || (num >= 0 && num <= this.m_WeaponTypes.Count && this.HaveItem(this.m_WeaponTypes[num - 1].Name));
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x0007C49D File Offset: 0x0007A69D
	protected virtual void OnStop_SetWeapon()
	{
		this.RefreshWeaponStatus();
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x0007C4A8 File Offset: 0x0007A6A8
	protected virtual void OnStart_Dead()
	{
		foreach (vp_SimpleInventory.InventoryItemStatus inventoryItemStatus in this.m_ItemStatusDictionary.Values)
		{
			if (inventoryItemStatus.ClearOnDeath)
			{
				inventoryItemStatus.Have = 0;
				if (inventoryItemStatus.GetType() == typeof(vp_SimpleInventory.InventoryWeaponStatus))
				{
					((vp_SimpleInventory.InventoryWeaponStatus)inventoryItemStatus).LoadedAmmo = 0;
				}
			}
		}
	}

	// Token: 0x04000F1E RID: 3870
	protected vp_FPPlayerEventHandler m_Player;

	// Token: 0x04000F1F RID: 3871
	[SerializeField]
	protected List<vp_SimpleInventory.InventoryItemStatus> m_ItemTypes;

	// Token: 0x04000F20 RID: 3872
	[SerializeField]
	protected List<vp_SimpleInventory.InventoryWeaponStatus> m_WeaponTypes;

	// Token: 0x04000F21 RID: 3873
	protected Dictionary<string, vp_SimpleInventory.InventoryItemStatus> m_ItemStatusDictionary;

	// Token: 0x04000F22 RID: 3874
	protected vp_SimpleInventory.InventoryWeaponStatus m_CurrentWeaponStatus;

	// Token: 0x04000F23 RID: 3875
	protected int m_RefreshWeaponStatusIterations;

	// Token: 0x020008BC RID: 2236
	[Serializable]
	public class InventoryItemStatus
	{
		// Token: 0x040033D0 RID: 13264
		public string Name = "Unnamed";

		// Token: 0x040033D1 RID: 13265
		public int Have;

		// Token: 0x040033D2 RID: 13266
		public int CanHave = 1;

		// Token: 0x040033D3 RID: 13267
		public bool ClearOnDeath = true;
	}

	// Token: 0x020008BD RID: 2237
	[Serializable]
	public class InventoryWeaponStatus : vp_SimpleInventory.InventoryItemStatus
	{
		// Token: 0x040033D4 RID: 13268
		public string ClipType = "";

		// Token: 0x040033D5 RID: 13269
		public int LoadedAmmo;

		// Token: 0x040033D6 RID: 13270
		public int MaxAmmo = 10;
	}
}
