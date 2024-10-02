using System;

// Token: 0x020000DE RID: 222
public class vp_ExampleSpeedPickup : vp_Pickup
{
	// Token: 0x06000812 RID: 2066 RVA: 0x00078254 File Offset: 0x00076454
	protected override void Update()
	{
		this.UpdateMotion();
		if (this.m_Depleted && !this.m_Audio.isPlaying)
		{
			this.Remove();
		}
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x00078278 File Offset: 0x00076478
	protected override bool TryGive(vp_FPPlayerEventHandler player)
	{
		if (this.m_Timer.Active)
		{
			return false;
		}
		player.SetState("MegaSpeed", true, true, false);
		vp_Timer.In(this.RespawnDuration, delegate()
		{
			player.SetState("MegaSpeed", false, true, false);
		}, this.m_Timer);
		return true;
	}

	// Token: 0x04000E5B RID: 3675
	protected vp_Timer.Handle m_Timer = new vp_Timer.Handle();
}
