using System;

// Token: 0x020000DB RID: 219
public class vp_ExampleAmmoPickup : vp_Pickup
{
	// Token: 0x0600080A RID: 2058 RVA: 0x00078096 File Offset: 0x00076296
	protected override bool TryGive(vp_FPPlayerEventHandler player)
	{
		if (player.Dead.Active)
		{
			return false;
		}
		if (base.TryGive(player))
		{
			this.TryReloadIfEmpty(player);
			return true;
		}
		if (this.TryReloadIfEmpty(player))
		{
			base.TryGive(player);
			return true;
		}
		return false;
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x000780CE File Offset: 0x000762CE
	private bool TryReloadIfEmpty(vp_FPPlayerEventHandler player)
	{
		return !(player.CurrentWeaponClipType.Get() != this.InventoryName) && player.Reload.TryStart(true);
	}
}
