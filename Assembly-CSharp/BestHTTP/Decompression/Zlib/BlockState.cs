using System;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x020007FB RID: 2043
	internal enum BlockState
	{
		// Token: 0x04002F29 RID: 12073
		NeedMore,
		// Token: 0x04002F2A RID: 12074
		BlockDone,
		// Token: 0x04002F2B RID: 12075
		FinishStarted,
		// Token: 0x04002F2C RID: 12076
		FinishDone
	}
}
