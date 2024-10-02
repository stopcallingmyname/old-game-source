using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp
{
	// Token: 0x020002FD RID: 765
	public abstract class OcspRespStatus
	{
		// Token: 0x04001908 RID: 6408
		public const int Successful = 0;

		// Token: 0x04001909 RID: 6409
		public const int MalformedRequest = 1;

		// Token: 0x0400190A RID: 6410
		public const int InternalError = 2;

		// Token: 0x0400190B RID: 6411
		public const int TryLater = 3;

		// Token: 0x0400190C RID: 6412
		public const int SigRequired = 5;

		// Token: 0x0400190D RID: 6413
		public const int Unauthorized = 6;
	}
}
