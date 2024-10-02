using System;

namespace BestHTTP.SignalRCore
{
	// Token: 0x020001E5 RID: 485
	public sealed class HubOptions
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060011D4 RID: 4564 RVA: 0x000A2C2C File Offset: 0x000A0E2C
		// (set) Token: 0x060011D5 RID: 4565 RVA: 0x000A2C34 File Offset: 0x000A0E34
		public bool SkipNegotiation { get; set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060011D6 RID: 4566 RVA: 0x000A2C3D File Offset: 0x000A0E3D
		// (set) Token: 0x060011D7 RID: 4567 RVA: 0x000A2C45 File Offset: 0x000A0E45
		public TransportTypes PreferedTransport { get; set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060011D8 RID: 4568 RVA: 0x000A2C4E File Offset: 0x000A0E4E
		// (set) Token: 0x060011D9 RID: 4569 RVA: 0x000A2C56 File Offset: 0x000A0E56
		public TimeSpan PingInterval { get; set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060011DA RID: 4570 RVA: 0x000A2C5F File Offset: 0x000A0E5F
		// (set) Token: 0x060011DB RID: 4571 RVA: 0x000A2C67 File Offset: 0x000A0E67
		public int MaxRedirects { get; set; }

		// Token: 0x060011DC RID: 4572 RVA: 0x000A2C70 File Offset: 0x000A0E70
		public HubOptions()
		{
			this.SkipNegotiation = false;
			this.PreferedTransport = TransportTypes.WebSocket;
			this.PingInterval = TimeSpan.FromSeconds(15.0);
			this.MaxRedirects = 100;
		}
	}
}
