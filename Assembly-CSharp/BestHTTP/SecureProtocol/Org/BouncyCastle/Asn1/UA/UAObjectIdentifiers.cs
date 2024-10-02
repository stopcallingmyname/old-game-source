using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.UA
{
	// Token: 0x020006D8 RID: 1752
	public abstract class UAObjectIdentifiers
	{
		// Token: 0x040028FD RID: 10493
		public static readonly DerObjectIdentifier UaOid = new DerObjectIdentifier("1.2.804.2.1.1.1");

		// Token: 0x040028FE RID: 10494
		public static readonly DerObjectIdentifier dstu4145le = UAObjectIdentifiers.UaOid.Branch("1.3.1.1");

		// Token: 0x040028FF RID: 10495
		public static readonly DerObjectIdentifier dstu4145be = UAObjectIdentifiers.UaOid.Branch("1.3.1.1.1.1");

		// Token: 0x04002900 RID: 10496
		public static readonly DerObjectIdentifier dstu7564digest_256 = UAObjectIdentifiers.UaOid.Branch("1.2.2.1");

		// Token: 0x04002901 RID: 10497
		public static readonly DerObjectIdentifier dstu7564digest_384 = UAObjectIdentifiers.UaOid.Branch("1.2.2.2");

		// Token: 0x04002902 RID: 10498
		public static readonly DerObjectIdentifier dstu7564digest_512 = UAObjectIdentifiers.UaOid.Branch("1.2.2.3");

		// Token: 0x04002903 RID: 10499
		public static readonly DerObjectIdentifier dstu7564mac_256 = UAObjectIdentifiers.UaOid.Branch("1.2.2.4");

		// Token: 0x04002904 RID: 10500
		public static readonly DerObjectIdentifier dstu7564mac_384 = UAObjectIdentifiers.UaOid.Branch("1.2.2.5");

		// Token: 0x04002905 RID: 10501
		public static readonly DerObjectIdentifier dstu7564mac_512 = UAObjectIdentifiers.UaOid.Branch("1.2.2.6");

		// Token: 0x04002906 RID: 10502
		public static readonly DerObjectIdentifier dstu7624ecb_128 = UAObjectIdentifiers.UaOid.Branch("1.1.3.1.1");

		// Token: 0x04002907 RID: 10503
		public static readonly DerObjectIdentifier dstu7624ecb_256 = UAObjectIdentifiers.UaOid.Branch("1.1.3.1.2");

		// Token: 0x04002908 RID: 10504
		public static readonly DerObjectIdentifier dstu7624ecb_512 = UAObjectIdentifiers.UaOid.Branch("1.1.3.1.3");

		// Token: 0x04002909 RID: 10505
		public static readonly DerObjectIdentifier dstu7624ctr_128 = UAObjectIdentifiers.UaOid.Branch("1.1.3.2.1");

		// Token: 0x0400290A RID: 10506
		public static readonly DerObjectIdentifier dstu7624ctr_256 = UAObjectIdentifiers.UaOid.Branch("1.1.3.2.2");

		// Token: 0x0400290B RID: 10507
		public static readonly DerObjectIdentifier dstu7624ctr_512 = UAObjectIdentifiers.UaOid.Branch("1.1.3.2.3");

		// Token: 0x0400290C RID: 10508
		public static readonly DerObjectIdentifier dstu7624cfb_128 = UAObjectIdentifiers.UaOid.Branch("1.1.3.3.1");

		// Token: 0x0400290D RID: 10509
		public static readonly DerObjectIdentifier dstu7624cfb_256 = UAObjectIdentifiers.UaOid.Branch("1.1.3.3.2");

		// Token: 0x0400290E RID: 10510
		public static readonly DerObjectIdentifier dstu7624cfb_512 = UAObjectIdentifiers.UaOid.Branch("1.1.3.3.3");

		// Token: 0x0400290F RID: 10511
		public static readonly DerObjectIdentifier dstu7624cmac_128 = UAObjectIdentifiers.UaOid.Branch("1.1.3.4.1");

		// Token: 0x04002910 RID: 10512
		public static readonly DerObjectIdentifier dstu7624cmac_256 = UAObjectIdentifiers.UaOid.Branch("1.1.3.4.2");

		// Token: 0x04002911 RID: 10513
		public static readonly DerObjectIdentifier dstu7624cmac_512 = UAObjectIdentifiers.UaOid.Branch("1.1.3.4.3");

		// Token: 0x04002912 RID: 10514
		public static readonly DerObjectIdentifier dstu7624cbc_128 = UAObjectIdentifiers.UaOid.Branch("1.1.3.5.1");

		// Token: 0x04002913 RID: 10515
		public static readonly DerObjectIdentifier dstu7624cbc_256 = UAObjectIdentifiers.UaOid.Branch("1.1.3.5.2");

		// Token: 0x04002914 RID: 10516
		public static readonly DerObjectIdentifier dstu7624cbc_512 = UAObjectIdentifiers.UaOid.Branch("1.1.3.5.3");

		// Token: 0x04002915 RID: 10517
		public static readonly DerObjectIdentifier dstu7624ofb_128 = UAObjectIdentifiers.UaOid.Branch("1.1.3.6.1");

		// Token: 0x04002916 RID: 10518
		public static readonly DerObjectIdentifier dstu7624ofb_256 = UAObjectIdentifiers.UaOid.Branch("1.1.3.6.2");

		// Token: 0x04002917 RID: 10519
		public static readonly DerObjectIdentifier dstu7624ofb_512 = UAObjectIdentifiers.UaOid.Branch("1.1.3.6.3");

		// Token: 0x04002918 RID: 10520
		public static readonly DerObjectIdentifier dstu7624gmac_128 = UAObjectIdentifiers.UaOid.Branch("1.1.3.7.1");

		// Token: 0x04002919 RID: 10521
		public static readonly DerObjectIdentifier dstu7624gmac_256 = UAObjectIdentifiers.UaOid.Branch("1.1.3.7.2");

		// Token: 0x0400291A RID: 10522
		public static readonly DerObjectIdentifier dstu7624gmac_512 = UAObjectIdentifiers.UaOid.Branch("1.1.3.7.3");

		// Token: 0x0400291B RID: 10523
		public static readonly DerObjectIdentifier dstu7624ccm_128 = UAObjectIdentifiers.UaOid.Branch("1.1.3.8.1");

		// Token: 0x0400291C RID: 10524
		public static readonly DerObjectIdentifier dstu7624ccm_256 = UAObjectIdentifiers.UaOid.Branch("1.1.3.8.2");

		// Token: 0x0400291D RID: 10525
		public static readonly DerObjectIdentifier dstu7624ccm_512 = UAObjectIdentifiers.UaOid.Branch("1.1.3.8.3");

		// Token: 0x0400291E RID: 10526
		public static readonly DerObjectIdentifier dstu7624xts_128 = UAObjectIdentifiers.UaOid.Branch("1.1.3.9.1");

		// Token: 0x0400291F RID: 10527
		public static readonly DerObjectIdentifier dstu7624xts_256 = UAObjectIdentifiers.UaOid.Branch("1.1.3.9.2");

		// Token: 0x04002920 RID: 10528
		public static readonly DerObjectIdentifier dstu7624xts_512 = UAObjectIdentifiers.UaOid.Branch("1.1.3.9.3");

		// Token: 0x04002921 RID: 10529
		public static readonly DerObjectIdentifier dstu7624kw_128 = UAObjectIdentifiers.UaOid.Branch("1.1.3.10.1");

		// Token: 0x04002922 RID: 10530
		public static readonly DerObjectIdentifier dstu7624kw_256 = UAObjectIdentifiers.UaOid.Branch("1.1.3.10.2");

		// Token: 0x04002923 RID: 10531
		public static readonly DerObjectIdentifier dstu7624kw_512 = UAObjectIdentifiers.UaOid.Branch("1.1.3.10.3");
	}
}
