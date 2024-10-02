using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Misc
{
	// Token: 0x0200071E RID: 1822
	public abstract class MiscObjectIdentifiers
	{
		// Token: 0x04002B19 RID: 11033
		public static readonly DerObjectIdentifier Netscape = new DerObjectIdentifier("2.16.840.1.113730.1");

		// Token: 0x04002B1A RID: 11034
		public static readonly DerObjectIdentifier NetscapeCertType = MiscObjectIdentifiers.Netscape.Branch("1");

		// Token: 0x04002B1B RID: 11035
		public static readonly DerObjectIdentifier NetscapeBaseUrl = MiscObjectIdentifiers.Netscape.Branch("2");

		// Token: 0x04002B1C RID: 11036
		public static readonly DerObjectIdentifier NetscapeRevocationUrl = MiscObjectIdentifiers.Netscape.Branch("3");

		// Token: 0x04002B1D RID: 11037
		public static readonly DerObjectIdentifier NetscapeCARevocationUrl = MiscObjectIdentifiers.Netscape.Branch("4");

		// Token: 0x04002B1E RID: 11038
		public static readonly DerObjectIdentifier NetscapeRenewalUrl = MiscObjectIdentifiers.Netscape.Branch("7");

		// Token: 0x04002B1F RID: 11039
		public static readonly DerObjectIdentifier NetscapeCAPolicyUrl = MiscObjectIdentifiers.Netscape.Branch("8");

		// Token: 0x04002B20 RID: 11040
		public static readonly DerObjectIdentifier NetscapeSslServerName = MiscObjectIdentifiers.Netscape.Branch("12");

		// Token: 0x04002B21 RID: 11041
		public static readonly DerObjectIdentifier NetscapeCertComment = MiscObjectIdentifiers.Netscape.Branch("13");

		// Token: 0x04002B22 RID: 11042
		public static readonly DerObjectIdentifier Verisign = new DerObjectIdentifier("2.16.840.1.113733.1");

		// Token: 0x04002B23 RID: 11043
		public static readonly DerObjectIdentifier VerisignCzagExtension = MiscObjectIdentifiers.Verisign.Branch("6.3");

		// Token: 0x04002B24 RID: 11044
		public static readonly DerObjectIdentifier VerisignPrivate_6_9 = MiscObjectIdentifiers.Verisign.Branch("6.9");

		// Token: 0x04002B25 RID: 11045
		public static readonly DerObjectIdentifier VerisignOnSiteJurisdictionHash = MiscObjectIdentifiers.Verisign.Branch("6.11");

		// Token: 0x04002B26 RID: 11046
		public static readonly DerObjectIdentifier VerisignBitString_6_13 = MiscObjectIdentifiers.Verisign.Branch("6.13");

		// Token: 0x04002B27 RID: 11047
		public static readonly DerObjectIdentifier VerisignDnbDunsNumber = MiscObjectIdentifiers.Verisign.Branch("6.15");

		// Token: 0x04002B28 RID: 11048
		public static readonly DerObjectIdentifier VerisignIssStrongCrypto = MiscObjectIdentifiers.Verisign.Branch("8.1");

		// Token: 0x04002B29 RID: 11049
		public static readonly string Novell = "2.16.840.1.113719";

		// Token: 0x04002B2A RID: 11050
		public static readonly DerObjectIdentifier NovellSecurityAttribs = new DerObjectIdentifier(MiscObjectIdentifiers.Novell + ".1.9.4.1");

		// Token: 0x04002B2B RID: 11051
		public static readonly string Entrust = "1.2.840.113533.7";

		// Token: 0x04002B2C RID: 11052
		public static readonly DerObjectIdentifier EntrustVersionExtension = new DerObjectIdentifier(MiscObjectIdentifiers.Entrust + ".65.0");

		// Token: 0x04002B2D RID: 11053
		public static readonly DerObjectIdentifier as_sys_sec_alg_ideaCBC = new DerObjectIdentifier("1.3.6.1.4.1.188.7.1.1.2");

		// Token: 0x04002B2E RID: 11054
		public static readonly DerObjectIdentifier cryptlib = new DerObjectIdentifier("1.3.6.1.4.1.3029");

		// Token: 0x04002B2F RID: 11055
		public static readonly DerObjectIdentifier cryptlib_algorithm = MiscObjectIdentifiers.cryptlib.Branch("1");

		// Token: 0x04002B30 RID: 11056
		public static readonly DerObjectIdentifier cryptlib_algorithm_blowfish_ECB = MiscObjectIdentifiers.cryptlib_algorithm.Branch("1.1");

		// Token: 0x04002B31 RID: 11057
		public static readonly DerObjectIdentifier cryptlib_algorithm_blowfish_CBC = MiscObjectIdentifiers.cryptlib_algorithm.Branch("1.2");

		// Token: 0x04002B32 RID: 11058
		public static readonly DerObjectIdentifier cryptlib_algorithm_blowfish_CFB = MiscObjectIdentifiers.cryptlib_algorithm.Branch("1.3");

		// Token: 0x04002B33 RID: 11059
		public static readonly DerObjectIdentifier cryptlib_algorithm_blowfish_OFB = MiscObjectIdentifiers.cryptlib_algorithm.Branch("1.4");

		// Token: 0x04002B34 RID: 11060
		public static readonly DerObjectIdentifier blake2 = new DerObjectIdentifier("1.3.6.1.4.1.1722.12.2");

		// Token: 0x04002B35 RID: 11061
		public static readonly DerObjectIdentifier id_blake2b160 = MiscObjectIdentifiers.blake2.Branch("1.5");

		// Token: 0x04002B36 RID: 11062
		public static readonly DerObjectIdentifier id_blake2b256 = MiscObjectIdentifiers.blake2.Branch("1.8");

		// Token: 0x04002B37 RID: 11063
		public static readonly DerObjectIdentifier id_blake2b384 = MiscObjectIdentifiers.blake2.Branch("1.12");

		// Token: 0x04002B38 RID: 11064
		public static readonly DerObjectIdentifier id_blake2b512 = MiscObjectIdentifiers.blake2.Branch("1.16");

		// Token: 0x04002B39 RID: 11065
		public static readonly DerObjectIdentifier id_blake2s128 = MiscObjectIdentifiers.blake2.Branch("2.4");

		// Token: 0x04002B3A RID: 11066
		public static readonly DerObjectIdentifier id_blake2s160 = MiscObjectIdentifiers.blake2.Branch("2.5");

		// Token: 0x04002B3B RID: 11067
		public static readonly DerObjectIdentifier id_blake2s224 = MiscObjectIdentifiers.blake2.Branch("2.7");

		// Token: 0x04002B3C RID: 11068
		public static readonly DerObjectIdentifier id_blake2s256 = MiscObjectIdentifiers.blake2.Branch("2.8");

		// Token: 0x04002B3D RID: 11069
		public static readonly DerObjectIdentifier id_scrypt = new DerObjectIdentifier("1.3.6.1.4.1.11591.4.11");
	}
}
