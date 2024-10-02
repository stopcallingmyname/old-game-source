using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.EdEC
{
	// Token: 0x0200075B RID: 1883
	public abstract class EdECObjectIdentifiers
	{
		// Token: 0x04002C5E RID: 11358
		public static readonly DerObjectIdentifier id_edwards_curve_algs = new DerObjectIdentifier("1.3.101");

		// Token: 0x04002C5F RID: 11359
		public static readonly DerObjectIdentifier id_X25519 = EdECObjectIdentifiers.id_edwards_curve_algs.Branch("110");

		// Token: 0x04002C60 RID: 11360
		public static readonly DerObjectIdentifier id_X448 = EdECObjectIdentifiers.id_edwards_curve_algs.Branch("111");

		// Token: 0x04002C61 RID: 11361
		public static readonly DerObjectIdentifier id_Ed25519 = EdECObjectIdentifiers.id_edwards_curve_algs.Branch("112");

		// Token: 0x04002C62 RID: 11362
		public static readonly DerObjectIdentifier id_Ed448 = EdECObjectIdentifiers.id_edwards_curve_algs.Branch("113");
	}
}
