using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000439 RID: 1081
	public abstract class NamedCurve
	{
		// Token: 0x06002AB6 RID: 10934 RVA: 0x001133B5 File Offset: 0x001115B5
		public static bool IsValid(int namedCurve)
		{
			return (namedCurve >= 1 && namedCurve <= 28) || (namedCurve >= 65281 && namedCurve <= 65282);
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x001133D7 File Offset: 0x001115D7
		public static bool RefersToASpecificNamedCurve(int namedCurve)
		{
			return namedCurve - 65281 > 1;
		}

		// Token: 0x04001D7D RID: 7549
		public const int sect163k1 = 1;

		// Token: 0x04001D7E RID: 7550
		public const int sect163r1 = 2;

		// Token: 0x04001D7F RID: 7551
		public const int sect163r2 = 3;

		// Token: 0x04001D80 RID: 7552
		public const int sect193r1 = 4;

		// Token: 0x04001D81 RID: 7553
		public const int sect193r2 = 5;

		// Token: 0x04001D82 RID: 7554
		public const int sect233k1 = 6;

		// Token: 0x04001D83 RID: 7555
		public const int sect233r1 = 7;

		// Token: 0x04001D84 RID: 7556
		public const int sect239k1 = 8;

		// Token: 0x04001D85 RID: 7557
		public const int sect283k1 = 9;

		// Token: 0x04001D86 RID: 7558
		public const int sect283r1 = 10;

		// Token: 0x04001D87 RID: 7559
		public const int sect409k1 = 11;

		// Token: 0x04001D88 RID: 7560
		public const int sect409r1 = 12;

		// Token: 0x04001D89 RID: 7561
		public const int sect571k1 = 13;

		// Token: 0x04001D8A RID: 7562
		public const int sect571r1 = 14;

		// Token: 0x04001D8B RID: 7563
		public const int secp160k1 = 15;

		// Token: 0x04001D8C RID: 7564
		public const int secp160r1 = 16;

		// Token: 0x04001D8D RID: 7565
		public const int secp160r2 = 17;

		// Token: 0x04001D8E RID: 7566
		public const int secp192k1 = 18;

		// Token: 0x04001D8F RID: 7567
		public const int secp192r1 = 19;

		// Token: 0x04001D90 RID: 7568
		public const int secp224k1 = 20;

		// Token: 0x04001D91 RID: 7569
		public const int secp224r1 = 21;

		// Token: 0x04001D92 RID: 7570
		public const int secp256k1 = 22;

		// Token: 0x04001D93 RID: 7571
		public const int secp256r1 = 23;

		// Token: 0x04001D94 RID: 7572
		public const int secp384r1 = 24;

		// Token: 0x04001D95 RID: 7573
		public const int secp521r1 = 25;

		// Token: 0x04001D96 RID: 7574
		public const int brainpoolP256r1 = 26;

		// Token: 0x04001D97 RID: 7575
		public const int brainpoolP384r1 = 27;

		// Token: 0x04001D98 RID: 7576
		public const int brainpoolP512r1 = 28;

		// Token: 0x04001D99 RID: 7577
		public const int arbitrary_explicit_prime_curves = 65281;

		// Token: 0x04001D9A RID: 7578
		public const int arbitrary_explicit_char2_curves = 65282;
	}
}
