using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000436 RID: 1078
	public abstract class KeyExchangeAlgorithm
	{
		// Token: 0x04001D58 RID: 7512
		public const int NULL = 0;

		// Token: 0x04001D59 RID: 7513
		public const int RSA = 1;

		// Token: 0x04001D5A RID: 7514
		public const int RSA_EXPORT = 2;

		// Token: 0x04001D5B RID: 7515
		public const int DHE_DSS = 3;

		// Token: 0x04001D5C RID: 7516
		public const int DHE_DSS_EXPORT = 4;

		// Token: 0x04001D5D RID: 7517
		public const int DHE_RSA = 5;

		// Token: 0x04001D5E RID: 7518
		public const int DHE_RSA_EXPORT = 6;

		// Token: 0x04001D5F RID: 7519
		public const int DH_DSS = 7;

		// Token: 0x04001D60 RID: 7520
		public const int DH_DSS_EXPORT = 8;

		// Token: 0x04001D61 RID: 7521
		public const int DH_RSA = 9;

		// Token: 0x04001D62 RID: 7522
		public const int DH_RSA_EXPORT = 10;

		// Token: 0x04001D63 RID: 7523
		public const int DH_anon = 11;

		// Token: 0x04001D64 RID: 7524
		public const int DH_anon_EXPORT = 12;

		// Token: 0x04001D65 RID: 7525
		public const int PSK = 13;

		// Token: 0x04001D66 RID: 7526
		public const int DHE_PSK = 14;

		// Token: 0x04001D67 RID: 7527
		public const int RSA_PSK = 15;

		// Token: 0x04001D68 RID: 7528
		public const int ECDH_ECDSA = 16;

		// Token: 0x04001D69 RID: 7529
		public const int ECDHE_ECDSA = 17;

		// Token: 0x04001D6A RID: 7530
		public const int ECDH_RSA = 18;

		// Token: 0x04001D6B RID: 7531
		public const int ECDHE_RSA = 19;

		// Token: 0x04001D6C RID: 7532
		public const int ECDH_anon = 20;

		// Token: 0x04001D6D RID: 7533
		public const int SRP = 21;

		// Token: 0x04001D6E RID: 7534
		public const int SRP_DSS = 22;

		// Token: 0x04001D6F RID: 7535
		public const int SRP_RSA = 23;

		// Token: 0x04001D70 RID: 7536
		public const int ECDHE_PSK = 24;
	}
}
