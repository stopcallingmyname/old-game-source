using System;

namespace BestHTTP
{
	// Token: 0x02000178 RID: 376
	public enum HTTPConnectionStates
	{
		// Token: 0x040012A6 RID: 4774
		Initial,
		// Token: 0x040012A7 RID: 4775
		Processing,
		// Token: 0x040012A8 RID: 4776
		Redirected,
		// Token: 0x040012A9 RID: 4777
		Upgraded,
		// Token: 0x040012AA RID: 4778
		WaitForProtocolShutdown,
		// Token: 0x040012AB RID: 4779
		WaitForRecycle,
		// Token: 0x040012AC RID: 4780
		Free,
		// Token: 0x040012AD RID: 4781
		AbortRequested,
		// Token: 0x040012AE RID: 4782
		TimedOut,
		// Token: 0x040012AF RID: 4783
		Closed
	}
}
