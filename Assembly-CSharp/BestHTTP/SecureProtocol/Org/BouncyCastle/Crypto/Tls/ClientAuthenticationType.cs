using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200040D RID: 1037
	public abstract class ClientAuthenticationType
	{
		// Token: 0x04001C8E RID: 7310
		public const byte anonymous = 0;

		// Token: 0x04001C8F RID: 7311
		public const byte certificate_based = 1;

		// Token: 0x04001C90 RID: 7312
		public const byte psk = 2;
	}
}
