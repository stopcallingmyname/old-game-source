using System;

namespace BestHTTP
{
	// Token: 0x02000180 RID: 384
	public enum HTTPRequestStates
	{
		// Token: 0x040012E7 RID: 4839
		Initial,
		// Token: 0x040012E8 RID: 4840
		Queued,
		// Token: 0x040012E9 RID: 4841
		Processing,
		// Token: 0x040012EA RID: 4842
		Finished,
		// Token: 0x040012EB RID: 4843
		Error,
		// Token: 0x040012EC RID: 4844
		Aborted,
		// Token: 0x040012ED RID: 4845
		ConnectionTimedOut,
		// Token: 0x040012EE RID: 4846
		TimedOut
	}
}
