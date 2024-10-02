using System;
using UnityEngine;

// Token: 0x020000DD RID: 221
public class vp_ExampleSlomoPickup : vp_Pickup
{
	// Token: 0x0600080F RID: 2063 RVA: 0x0007818C File Offset: 0x0007638C
	protected override void Update()
	{
		this.UpdateMotion();
		if (this.m_Depleted)
		{
			if (this.m_Player != null && this.m_Player.Dead.Active && !this.m_RespawnTimer.Active)
			{
				this.Respawn();
				return;
			}
			if (Time.timeScale > 0.2f && !vp_TimeUtility.Paused)
			{
				vp_TimeUtility.FadeTimeScale(0.2f, 0.1f);
				return;
			}
			if (!this.m_Audio.isPlaying)
			{
				this.Remove();
				return;
			}
		}
		else if (Time.timeScale < 1f && !vp_TimeUtility.Paused)
		{
			vp_TimeUtility.FadeTimeScale(1f, 0.05f);
		}
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x00078234 File Offset: 0x00076434
	protected override bool TryGive(vp_FPPlayerEventHandler player)
	{
		this.m_Player = player;
		return !this.m_Depleted && Time.timeScale == 1f;
	}

	// Token: 0x04000E5A RID: 3674
	private vp_FPPlayerEventHandler m_Player;
}
