using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x0200076C RID: 1900
	public abstract class CrmfObjectIdentifiers
	{
		// Token: 0x04002CC8 RID: 11464
		public static readonly DerObjectIdentifier id_pkix = new DerObjectIdentifier("1.3.6.1.5.5.7");

		// Token: 0x04002CC9 RID: 11465
		public static readonly DerObjectIdentifier id_pkip = CrmfObjectIdentifiers.id_pkix.Branch("5");

		// Token: 0x04002CCA RID: 11466
		public static readonly DerObjectIdentifier id_regCtrl = CrmfObjectIdentifiers.id_pkip.Branch("1");

		// Token: 0x04002CCB RID: 11467
		public static readonly DerObjectIdentifier id_regCtrl_regToken = CrmfObjectIdentifiers.id_regCtrl.Branch("1");

		// Token: 0x04002CCC RID: 11468
		public static readonly DerObjectIdentifier id_regCtrl_authenticator = CrmfObjectIdentifiers.id_regCtrl.Branch("2");

		// Token: 0x04002CCD RID: 11469
		public static readonly DerObjectIdentifier id_regCtrl_pkiPublicationInfo = CrmfObjectIdentifiers.id_regCtrl.Branch("3");

		// Token: 0x04002CCE RID: 11470
		public static readonly DerObjectIdentifier id_regCtrl_pkiArchiveOptions = CrmfObjectIdentifiers.id_regCtrl.Branch("4");

		// Token: 0x04002CCF RID: 11471
		public static readonly DerObjectIdentifier id_ct_encKeyWithID = new DerObjectIdentifier("1.2.840.113549.1.9.16.1.21");
	}
}
