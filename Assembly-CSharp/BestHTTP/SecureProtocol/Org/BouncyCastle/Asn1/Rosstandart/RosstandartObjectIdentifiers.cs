using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Rosstandart
{
	// Token: 0x020006E9 RID: 1769
	public abstract class RosstandartObjectIdentifiers
	{
		// Token: 0x0400299B RID: 10651
		public static readonly DerObjectIdentifier rosstandart = new DerObjectIdentifier("1.2.643.7");

		// Token: 0x0400299C RID: 10652
		public static readonly DerObjectIdentifier id_tc26 = RosstandartObjectIdentifiers.rosstandart.Branch("1");

		// Token: 0x0400299D RID: 10653
		public static readonly DerObjectIdentifier id_tc26_gost_3411_12_256 = RosstandartObjectIdentifiers.id_tc26.Branch("1.2.2");

		// Token: 0x0400299E RID: 10654
		public static readonly DerObjectIdentifier id_tc26_gost_3411_12_512 = RosstandartObjectIdentifiers.id_tc26.Branch("1.2.3");

		// Token: 0x0400299F RID: 10655
		public static readonly DerObjectIdentifier id_tc26_hmac_gost_3411_12_256 = RosstandartObjectIdentifiers.id_tc26.Branch("1.4.1");

		// Token: 0x040029A0 RID: 10656
		public static readonly DerObjectIdentifier id_tc26_hmac_gost_3411_12_512 = RosstandartObjectIdentifiers.id_tc26.Branch("1.4.2");

		// Token: 0x040029A1 RID: 10657
		public static readonly DerObjectIdentifier id_tc26_gost_3410_12_256 = RosstandartObjectIdentifiers.id_tc26.Branch("1.1.1");

		// Token: 0x040029A2 RID: 10658
		public static readonly DerObjectIdentifier id_tc26_gost_3410_12_512 = RosstandartObjectIdentifiers.id_tc26.Branch("1.1.2");

		// Token: 0x040029A3 RID: 10659
		public static readonly DerObjectIdentifier id_tc26_signwithdigest_gost_3410_12_256 = RosstandartObjectIdentifiers.id_tc26.Branch("1.3.2");

		// Token: 0x040029A4 RID: 10660
		public static readonly DerObjectIdentifier id_tc26_signwithdigest_gost_3410_12_512 = RosstandartObjectIdentifiers.id_tc26.Branch("1.3.3");

		// Token: 0x040029A5 RID: 10661
		public static readonly DerObjectIdentifier id_tc26_agreement = RosstandartObjectIdentifiers.id_tc26.Branch("1.6");

		// Token: 0x040029A6 RID: 10662
		public static readonly DerObjectIdentifier id_tc26_agreement_gost_3410_12_256 = RosstandartObjectIdentifiers.id_tc26_agreement.Branch("1");

		// Token: 0x040029A7 RID: 10663
		public static readonly DerObjectIdentifier id_tc26_agreement_gost_3410_12_512 = RosstandartObjectIdentifiers.id_tc26_agreement.Branch("2");

		// Token: 0x040029A8 RID: 10664
		public static readonly DerObjectIdentifier id_tc26_gost_3410_12_256_paramSet = RosstandartObjectIdentifiers.id_tc26.Branch("2.1.1");

		// Token: 0x040029A9 RID: 10665
		public static readonly DerObjectIdentifier id_tc26_gost_3410_12_256_paramSetA = RosstandartObjectIdentifiers.id_tc26_gost_3410_12_256_paramSet.Branch("1");

		// Token: 0x040029AA RID: 10666
		public static readonly DerObjectIdentifier id_tc26_gost_3410_12_512_paramSet = RosstandartObjectIdentifiers.id_tc26.Branch("2.1.2");

		// Token: 0x040029AB RID: 10667
		public static readonly DerObjectIdentifier id_tc26_gost_3410_12_512_paramSetA = RosstandartObjectIdentifiers.id_tc26_gost_3410_12_512_paramSet.Branch("1");

		// Token: 0x040029AC RID: 10668
		public static readonly DerObjectIdentifier id_tc26_gost_3410_12_512_paramSetB = RosstandartObjectIdentifiers.id_tc26_gost_3410_12_512_paramSet.Branch("2");

		// Token: 0x040029AD RID: 10669
		public static readonly DerObjectIdentifier id_tc26_gost_3410_12_512_paramSetC = RosstandartObjectIdentifiers.id_tc26_gost_3410_12_512_paramSet.Branch("3");

		// Token: 0x040029AE RID: 10670
		public static readonly DerObjectIdentifier id_tc26_gost_28147_param_Z = RosstandartObjectIdentifiers.id_tc26.Branch("2.5.1.1");
	}
}
