using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200040E RID: 1038
	public abstract class ClientCertificateType
	{
		// Token: 0x04001C91 RID: 7313
		public const byte rsa_sign = 1;

		// Token: 0x04001C92 RID: 7314
		public const byte dss_sign = 2;

		// Token: 0x04001C93 RID: 7315
		public const byte rsa_fixed_dh = 3;

		// Token: 0x04001C94 RID: 7316
		public const byte dss_fixed_dh = 4;

		// Token: 0x04001C95 RID: 7317
		public const byte rsa_ephemeral_dh_RESERVED = 5;

		// Token: 0x04001C96 RID: 7318
		public const byte dss_ephemeral_dh_RESERVED = 6;

		// Token: 0x04001C97 RID: 7319
		public const byte fortezza_dms_RESERVED = 20;

		// Token: 0x04001C98 RID: 7320
		public const byte ecdsa_sign = 64;

		// Token: 0x04001C99 RID: 7321
		public const byte rsa_fixed_ecdh = 65;

		// Token: 0x04001C9A RID: 7322
		public const byte ecdsa_fixed_ecdh = 66;
	}
}
