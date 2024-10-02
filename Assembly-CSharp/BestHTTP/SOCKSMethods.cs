using System;

namespace BestHTTP
{
	// Token: 0x0200018C RID: 396
	internal enum SOCKSMethods : byte
	{
		// Token: 0x04001354 RID: 4948
		NoAuthenticationRequired,
		// Token: 0x04001355 RID: 4949
		GSSAPI,
		// Token: 0x04001356 RID: 4950
		UsernameAndPassword,
		// Token: 0x04001357 RID: 4951
		NoAcceptableMethods = 255
	}
}
