using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Bsi
{
	// Token: 0x020007D1 RID: 2001
	public abstract class BsiObjectIdentifiers
	{
		// Token: 0x04002E66 RID: 11878
		public static readonly DerObjectIdentifier bsi_de = new DerObjectIdentifier("0.4.0.127.0.7");

		// Token: 0x04002E67 RID: 11879
		public static readonly DerObjectIdentifier id_ecc = BsiObjectIdentifiers.bsi_de.Branch("1.1");

		// Token: 0x04002E68 RID: 11880
		public static readonly DerObjectIdentifier ecdsa_plain_signatures = BsiObjectIdentifiers.id_ecc.Branch("4.1");

		// Token: 0x04002E69 RID: 11881
		public static readonly DerObjectIdentifier ecdsa_plain_SHA1 = BsiObjectIdentifiers.ecdsa_plain_signatures.Branch("1");

		// Token: 0x04002E6A RID: 11882
		public static readonly DerObjectIdentifier ecdsa_plain_SHA224 = BsiObjectIdentifiers.ecdsa_plain_signatures.Branch("2");

		// Token: 0x04002E6B RID: 11883
		public static readonly DerObjectIdentifier ecdsa_plain_SHA256 = BsiObjectIdentifiers.ecdsa_plain_signatures.Branch("3");

		// Token: 0x04002E6C RID: 11884
		public static readonly DerObjectIdentifier ecdsa_plain_SHA384 = BsiObjectIdentifiers.ecdsa_plain_signatures.Branch("4");

		// Token: 0x04002E6D RID: 11885
		public static readonly DerObjectIdentifier ecdsa_plain_SHA512 = BsiObjectIdentifiers.ecdsa_plain_signatures.Branch("5");

		// Token: 0x04002E6E RID: 11886
		public static readonly DerObjectIdentifier ecdsa_plain_RIPEMD160 = BsiObjectIdentifiers.ecdsa_plain_signatures.Branch("6");

		// Token: 0x04002E6F RID: 11887
		public static readonly DerObjectIdentifier algorithm = BsiObjectIdentifiers.bsi_de.Branch("1");

		// Token: 0x04002E70 RID: 11888
		public static readonly DerObjectIdentifier ecka_eg = BsiObjectIdentifiers.id_ecc.Branch("5.1");

		// Token: 0x04002E71 RID: 11889
		public static readonly DerObjectIdentifier ecka_eg_X963kdf = BsiObjectIdentifiers.ecka_eg.Branch("1");

		// Token: 0x04002E72 RID: 11890
		public static readonly DerObjectIdentifier ecka_eg_X963kdf_SHA1 = BsiObjectIdentifiers.ecka_eg_X963kdf.Branch("1");

		// Token: 0x04002E73 RID: 11891
		public static readonly DerObjectIdentifier ecka_eg_X963kdf_SHA224 = BsiObjectIdentifiers.ecka_eg_X963kdf.Branch("2");

		// Token: 0x04002E74 RID: 11892
		public static readonly DerObjectIdentifier ecka_eg_X963kdf_SHA256 = BsiObjectIdentifiers.ecka_eg_X963kdf.Branch("3");

		// Token: 0x04002E75 RID: 11893
		public static readonly DerObjectIdentifier ecka_eg_X963kdf_SHA384 = BsiObjectIdentifiers.ecka_eg_X963kdf.Branch("4");

		// Token: 0x04002E76 RID: 11894
		public static readonly DerObjectIdentifier ecka_eg_X963kdf_SHA512 = BsiObjectIdentifiers.ecka_eg_X963kdf.Branch("5");

		// Token: 0x04002E77 RID: 11895
		public static readonly DerObjectIdentifier ecka_eg_X963kdf_RIPEMD160 = BsiObjectIdentifiers.ecka_eg_X963kdf.Branch("6");

		// Token: 0x04002E78 RID: 11896
		public static readonly DerObjectIdentifier ecka_eg_SessionKDF = BsiObjectIdentifiers.ecka_eg.Branch("2");

		// Token: 0x04002E79 RID: 11897
		public static readonly DerObjectIdentifier ecka_eg_SessionKDF_3DES = BsiObjectIdentifiers.ecka_eg_SessionKDF.Branch("1");

		// Token: 0x04002E7A RID: 11898
		public static readonly DerObjectIdentifier ecka_eg_SessionKDF_AES128 = BsiObjectIdentifiers.ecka_eg_SessionKDF.Branch("2");

		// Token: 0x04002E7B RID: 11899
		public static readonly DerObjectIdentifier ecka_eg_SessionKDF_AES192 = BsiObjectIdentifiers.ecka_eg_SessionKDF.Branch("3");

		// Token: 0x04002E7C RID: 11900
		public static readonly DerObjectIdentifier ecka_eg_SessionKDF_AES256 = BsiObjectIdentifiers.ecka_eg_SessionKDF.Branch("4");
	}
}
