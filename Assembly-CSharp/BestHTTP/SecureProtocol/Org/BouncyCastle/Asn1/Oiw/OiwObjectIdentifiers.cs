using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw
{
	// Token: 0x02000706 RID: 1798
	public abstract class OiwObjectIdentifiers
	{
		// Token: 0x04002A94 RID: 10900
		public static readonly DerObjectIdentifier MD4WithRsa = new DerObjectIdentifier("1.3.14.3.2.2");

		// Token: 0x04002A95 RID: 10901
		public static readonly DerObjectIdentifier MD5WithRsa = new DerObjectIdentifier("1.3.14.3.2.3");

		// Token: 0x04002A96 RID: 10902
		public static readonly DerObjectIdentifier MD4WithRsaEncryption = new DerObjectIdentifier("1.3.14.3.2.4");

		// Token: 0x04002A97 RID: 10903
		public static readonly DerObjectIdentifier DesEcb = new DerObjectIdentifier("1.3.14.3.2.6");

		// Token: 0x04002A98 RID: 10904
		public static readonly DerObjectIdentifier DesCbc = new DerObjectIdentifier("1.3.14.3.2.7");

		// Token: 0x04002A99 RID: 10905
		public static readonly DerObjectIdentifier DesOfb = new DerObjectIdentifier("1.3.14.3.2.8");

		// Token: 0x04002A9A RID: 10906
		public static readonly DerObjectIdentifier DesCfb = new DerObjectIdentifier("1.3.14.3.2.9");

		// Token: 0x04002A9B RID: 10907
		public static readonly DerObjectIdentifier DesEde = new DerObjectIdentifier("1.3.14.3.2.17");

		// Token: 0x04002A9C RID: 10908
		public static readonly DerObjectIdentifier IdSha1 = new DerObjectIdentifier("1.3.14.3.2.26");

		// Token: 0x04002A9D RID: 10909
		public static readonly DerObjectIdentifier DsaWithSha1 = new DerObjectIdentifier("1.3.14.3.2.27");

		// Token: 0x04002A9E RID: 10910
		public static readonly DerObjectIdentifier Sha1WithRsa = new DerObjectIdentifier("1.3.14.3.2.29");

		// Token: 0x04002A9F RID: 10911
		public static readonly DerObjectIdentifier ElGamalAlgorithm = new DerObjectIdentifier("1.3.14.7.2.1.1");
	}
}
