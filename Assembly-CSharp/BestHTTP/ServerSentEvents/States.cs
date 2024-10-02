using System;

namespace BestHTTP.ServerSentEvents
{
	// Token: 0x0200022E RID: 558
	public enum States
	{
		// Token: 0x040015F6 RID: 5622
		Initial,
		// Token: 0x040015F7 RID: 5623
		Connecting,
		// Token: 0x040015F8 RID: 5624
		Open,
		// Token: 0x040015F9 RID: 5625
		Retrying,
		// Token: 0x040015FA RID: 5626
		Closing,
		// Token: 0x040015FB RID: 5627
		Closed
	}
}
