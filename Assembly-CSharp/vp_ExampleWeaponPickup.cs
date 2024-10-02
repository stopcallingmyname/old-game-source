using System;

// Token: 0x020000DF RID: 223
public class vp_ExampleWeaponPickup : vp_Pickup
{
	// Token: 0x06000815 RID: 2069 RVA: 0x000782E8 File Offset: 0x000764E8
	protected override bool TryGive(vp_FPPlayerEventHandler player)
	{
		if (player.Dead.Active)
		{
			return false;
		}
		if (!base.TryGive(player))
		{
			return false;
		}
		player.SetWeaponByName.Try(this.InventoryName);
		if (this.AmmoIncluded > 0)
		{
			player.AddAmmo.Try(new object[]
			{
				this.InventoryName,
				this.AmmoIncluded
			});
		}
		return true;
	}

	// Token: 0x04000E5C RID: 3676
	public int AmmoIncluded;
}
