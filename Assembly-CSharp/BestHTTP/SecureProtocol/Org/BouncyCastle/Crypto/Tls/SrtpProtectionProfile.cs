using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200044E RID: 1102
	public abstract class SrtpProtectionProfile
	{
		// Token: 0x04001DF6 RID: 7670
		public const int SRTP_AES128_CM_HMAC_SHA1_80 = 1;

		// Token: 0x04001DF7 RID: 7671
		public const int SRTP_AES128_CM_HMAC_SHA1_32 = 2;

		// Token: 0x04001DF8 RID: 7672
		public const int SRTP_NULL_HMAC_SHA1_80 = 5;

		// Token: 0x04001DF9 RID: 7673
		public const int SRTP_NULL_HMAC_SHA1_32 = 6;

		// Token: 0x04001DFA RID: 7674
		public const int SRTP_AEAD_AES_128_GCM = 7;

		// Token: 0x04001DFB RID: 7675
		public const int SRTP_AEAD_AES_256_GCM = 8;
	}
}
