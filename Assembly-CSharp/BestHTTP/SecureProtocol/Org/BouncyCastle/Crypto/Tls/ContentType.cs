using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000412 RID: 1042
	public abstract class ContentType
	{
		// Token: 0x04001CA2 RID: 7330
		public const byte change_cipher_spec = 20;

		// Token: 0x04001CA3 RID: 7331
		public const byte alert = 21;

		// Token: 0x04001CA4 RID: 7332
		public const byte handshake = 22;

		// Token: 0x04001CA5 RID: 7333
		public const byte application_data = 23;

		// Token: 0x04001CA6 RID: 7334
		public const byte heartbeat = 24;
	}
}
