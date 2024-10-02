using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro
{
	// Token: 0x0200075D RID: 1885
	public abstract class CryptoProObjectIdentifiers
	{
		// Token: 0x04002C78 RID: 11384
		public const string GostID = "1.2.643.2.2";

		// Token: 0x04002C79 RID: 11385
		public static readonly DerObjectIdentifier GostR3411 = new DerObjectIdentifier("1.2.643.2.2.9");

		// Token: 0x04002C7A RID: 11386
		public static readonly DerObjectIdentifier GostR3411Hmac = new DerObjectIdentifier("1.2.643.2.2.10");

		// Token: 0x04002C7B RID: 11387
		public static readonly DerObjectIdentifier GostR28147Cbc = new DerObjectIdentifier("1.2.643.2.2.21");

		// Token: 0x04002C7C RID: 11388
		public static readonly DerObjectIdentifier ID_Gost28147_89_CryptoPro_A_ParamSet = new DerObjectIdentifier("1.2.643.2.2.31.1");

		// Token: 0x04002C7D RID: 11389
		public static readonly DerObjectIdentifier GostR3410x94 = new DerObjectIdentifier("1.2.643.2.2.20");

		// Token: 0x04002C7E RID: 11390
		public static readonly DerObjectIdentifier GostR3410x2001 = new DerObjectIdentifier("1.2.643.2.2.19");

		// Token: 0x04002C7F RID: 11391
		public static readonly DerObjectIdentifier GostR3411x94WithGostR3410x94 = new DerObjectIdentifier("1.2.643.2.2.4");

		// Token: 0x04002C80 RID: 11392
		public static readonly DerObjectIdentifier GostR3411x94WithGostR3410x2001 = new DerObjectIdentifier("1.2.643.2.2.3");

		// Token: 0x04002C81 RID: 11393
		public static readonly DerObjectIdentifier GostR3411x94CryptoProParamSet = new DerObjectIdentifier("1.2.643.2.2.30.1");

		// Token: 0x04002C82 RID: 11394
		public static readonly DerObjectIdentifier GostR3410x94CryptoProA = new DerObjectIdentifier("1.2.643.2.2.32.2");

		// Token: 0x04002C83 RID: 11395
		public static readonly DerObjectIdentifier GostR3410x94CryptoProB = new DerObjectIdentifier("1.2.643.2.2.32.3");

		// Token: 0x04002C84 RID: 11396
		public static readonly DerObjectIdentifier GostR3410x94CryptoProC = new DerObjectIdentifier("1.2.643.2.2.32.4");

		// Token: 0x04002C85 RID: 11397
		public static readonly DerObjectIdentifier GostR3410x94CryptoProD = new DerObjectIdentifier("1.2.643.2.2.32.5");

		// Token: 0x04002C86 RID: 11398
		public static readonly DerObjectIdentifier GostR3410x94CryptoProXchA = new DerObjectIdentifier("1.2.643.2.2.33.1");

		// Token: 0x04002C87 RID: 11399
		public static readonly DerObjectIdentifier GostR3410x94CryptoProXchB = new DerObjectIdentifier("1.2.643.2.2.33.2");

		// Token: 0x04002C88 RID: 11400
		public static readonly DerObjectIdentifier GostR3410x94CryptoProXchC = new DerObjectIdentifier("1.2.643.2.2.33.3");

		// Token: 0x04002C89 RID: 11401
		public static readonly DerObjectIdentifier GostR3410x2001CryptoProA = new DerObjectIdentifier("1.2.643.2.2.35.1");

		// Token: 0x04002C8A RID: 11402
		public static readonly DerObjectIdentifier GostR3410x2001CryptoProB = new DerObjectIdentifier("1.2.643.2.2.35.2");

		// Token: 0x04002C8B RID: 11403
		public static readonly DerObjectIdentifier GostR3410x2001CryptoProC = new DerObjectIdentifier("1.2.643.2.2.35.3");

		// Token: 0x04002C8C RID: 11404
		public static readonly DerObjectIdentifier GostR3410x2001CryptoProXchA = new DerObjectIdentifier("1.2.643.2.2.36.0");

		// Token: 0x04002C8D RID: 11405
		public static readonly DerObjectIdentifier GostR3410x2001CryptoProXchB = new DerObjectIdentifier("1.2.643.2.2.36.1");

		// Token: 0x04002C8E RID: 11406
		public static readonly DerObjectIdentifier GostElSgDH3410Default = new DerObjectIdentifier("1.2.643.2.2.36.0");

		// Token: 0x04002C8F RID: 11407
		public static readonly DerObjectIdentifier GostElSgDH3410x1 = new DerObjectIdentifier("1.2.643.2.2.36.1");
	}
}
