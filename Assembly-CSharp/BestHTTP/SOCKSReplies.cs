using System;

namespace BestHTTP
{
	// Token: 0x0200018D RID: 397
	internal enum SOCKSReplies : byte
	{
		// Token: 0x04001359 RID: 4953
		Succeeded,
		// Token: 0x0400135A RID: 4954
		GeneralSOCKSServerFailure,
		// Token: 0x0400135B RID: 4955
		ConnectionNotAllowedByRuleset,
		// Token: 0x0400135C RID: 4956
		NetworkUnreachable,
		// Token: 0x0400135D RID: 4957
		HostUnreachable,
		// Token: 0x0400135E RID: 4958
		ConnectionRefused,
		// Token: 0x0400135F RID: 4959
		TTLExpired,
		// Token: 0x04001360 RID: 4960
		CommandNotSupported,
		// Token: 0x04001361 RID: 4961
		AddressTypeNotSupported
	}
}
