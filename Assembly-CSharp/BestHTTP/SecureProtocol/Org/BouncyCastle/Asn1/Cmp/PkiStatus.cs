using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007C4 RID: 1988
	public enum PkiStatus
	{
		// Token: 0x04002E3E RID: 11838
		Granted,
		// Token: 0x04002E3F RID: 11839
		GrantedWithMods,
		// Token: 0x04002E40 RID: 11840
		Rejection,
		// Token: 0x04002E41 RID: 11841
		Waiting,
		// Token: 0x04002E42 RID: 11842
		RevocationWarning,
		// Token: 0x04002E43 RID: 11843
		RevocationNotification,
		// Token: 0x04002E44 RID: 11844
		KeyUpdateWarning
	}
}
