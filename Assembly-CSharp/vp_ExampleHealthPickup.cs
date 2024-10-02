using System;
using UnityEngine;

// Token: 0x020000DC RID: 220
public class vp_ExampleHealthPickup : vp_Pickup
{
	// Token: 0x0600080D RID: 2061 RVA: 0x00078108 File Offset: 0x00076308
	protected override bool TryGive(vp_FPPlayerEventHandler player)
	{
		if (player.Health.Get() < 0f)
		{
			return false;
		}
		if (player.Health.Get() >= 1f)
		{
			return false;
		}
		player.Health.Set(Mathf.Min(1f, player.Health.Get() + this.Health));
		return true;
	}

	// Token: 0x04000E59 RID: 3673
	public float Health = 0.1f;
}
