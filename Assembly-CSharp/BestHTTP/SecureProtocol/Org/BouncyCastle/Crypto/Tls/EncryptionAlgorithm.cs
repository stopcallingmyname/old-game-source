using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200042C RID: 1068
	public abstract class EncryptionAlgorithm
	{
		// Token: 0x04001CF6 RID: 7414
		public const int NULL = 0;

		// Token: 0x04001CF7 RID: 7415
		public const int RC4_40 = 1;

		// Token: 0x04001CF8 RID: 7416
		public const int RC4_128 = 2;

		// Token: 0x04001CF9 RID: 7417
		public const int RC2_CBC_40 = 3;

		// Token: 0x04001CFA RID: 7418
		public const int IDEA_CBC = 4;

		// Token: 0x04001CFB RID: 7419
		public const int DES40_CBC = 5;

		// Token: 0x04001CFC RID: 7420
		public const int DES_CBC = 6;

		// Token: 0x04001CFD RID: 7421
		public const int cls_3DES_EDE_CBC = 7;

		// Token: 0x04001CFE RID: 7422
		public const int AES_128_CBC = 8;

		// Token: 0x04001CFF RID: 7423
		public const int AES_256_CBC = 9;

		// Token: 0x04001D00 RID: 7424
		public const int AES_128_GCM = 10;

		// Token: 0x04001D01 RID: 7425
		public const int AES_256_GCM = 11;

		// Token: 0x04001D02 RID: 7426
		public const int CAMELLIA_128_CBC = 12;

		// Token: 0x04001D03 RID: 7427
		public const int CAMELLIA_256_CBC = 13;

		// Token: 0x04001D04 RID: 7428
		public const int SEED_CBC = 14;

		// Token: 0x04001D05 RID: 7429
		public const int AES_128_CCM = 15;

		// Token: 0x04001D06 RID: 7430
		public const int AES_128_CCM_8 = 16;

		// Token: 0x04001D07 RID: 7431
		public const int AES_256_CCM = 17;

		// Token: 0x04001D08 RID: 7432
		public const int AES_256_CCM_8 = 18;

		// Token: 0x04001D09 RID: 7433
		public const int CAMELLIA_128_GCM = 19;

		// Token: 0x04001D0A RID: 7434
		public const int CAMELLIA_256_GCM = 20;

		// Token: 0x04001D0B RID: 7435
		public const int CHACHA20_POLY1305 = 21;

		// Token: 0x04001D0C RID: 7436
		public const int AES_128_OCB_TAGLEN96 = 103;

		// Token: 0x04001D0D RID: 7437
		public const int AES_256_OCB_TAGLEN96 = 104;
	}
}
