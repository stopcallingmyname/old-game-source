using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Eac
{
	// Token: 0x0200075C RID: 1884
	public abstract class EacObjectIdentifiers
	{
		// Token: 0x04002C63 RID: 11363
		public static readonly DerObjectIdentifier bsi_de = new DerObjectIdentifier("0.4.0.127.0.7");

		// Token: 0x04002C64 RID: 11364
		public static readonly DerObjectIdentifier id_PK = new DerObjectIdentifier(EacObjectIdentifiers.bsi_de + ".2.2.1");

		// Token: 0x04002C65 RID: 11365
		public static readonly DerObjectIdentifier id_PK_DH = new DerObjectIdentifier(EacObjectIdentifiers.id_PK + ".1");

		// Token: 0x04002C66 RID: 11366
		public static readonly DerObjectIdentifier id_PK_ECDH = new DerObjectIdentifier(EacObjectIdentifiers.id_PK + ".2");

		// Token: 0x04002C67 RID: 11367
		public static readonly DerObjectIdentifier id_CA = new DerObjectIdentifier(EacObjectIdentifiers.bsi_de + ".2.2.3");

		// Token: 0x04002C68 RID: 11368
		public static readonly DerObjectIdentifier id_CA_DH = new DerObjectIdentifier(EacObjectIdentifiers.id_CA + ".1");

		// Token: 0x04002C69 RID: 11369
		public static readonly DerObjectIdentifier id_CA_DH_3DES_CBC_CBC = new DerObjectIdentifier(EacObjectIdentifiers.id_CA_DH + ".1");

		// Token: 0x04002C6A RID: 11370
		public static readonly DerObjectIdentifier id_CA_ECDH = new DerObjectIdentifier(EacObjectIdentifiers.id_CA + ".2");

		// Token: 0x04002C6B RID: 11371
		public static readonly DerObjectIdentifier id_CA_ECDH_3DES_CBC_CBC = new DerObjectIdentifier(EacObjectIdentifiers.id_CA_ECDH + ".1");

		// Token: 0x04002C6C RID: 11372
		public static readonly DerObjectIdentifier id_TA = new DerObjectIdentifier(EacObjectIdentifiers.bsi_de + ".2.2.2");

		// Token: 0x04002C6D RID: 11373
		public static readonly DerObjectIdentifier id_TA_RSA = new DerObjectIdentifier(EacObjectIdentifiers.id_TA + ".1");

		// Token: 0x04002C6E RID: 11374
		public static readonly DerObjectIdentifier id_TA_RSA_v1_5_SHA_1 = new DerObjectIdentifier(EacObjectIdentifiers.id_TA_RSA + ".1");

		// Token: 0x04002C6F RID: 11375
		public static readonly DerObjectIdentifier id_TA_RSA_v1_5_SHA_256 = new DerObjectIdentifier(EacObjectIdentifiers.id_TA_RSA + ".2");

		// Token: 0x04002C70 RID: 11376
		public static readonly DerObjectIdentifier id_TA_RSA_PSS_SHA_1 = new DerObjectIdentifier(EacObjectIdentifiers.id_TA_RSA + ".3");

		// Token: 0x04002C71 RID: 11377
		public static readonly DerObjectIdentifier id_TA_RSA_PSS_SHA_256 = new DerObjectIdentifier(EacObjectIdentifiers.id_TA_RSA + ".4");

		// Token: 0x04002C72 RID: 11378
		public static readonly DerObjectIdentifier id_TA_ECDSA = new DerObjectIdentifier(EacObjectIdentifiers.id_TA + ".2");

		// Token: 0x04002C73 RID: 11379
		public static readonly DerObjectIdentifier id_TA_ECDSA_SHA_1 = new DerObjectIdentifier(EacObjectIdentifiers.id_TA_ECDSA + ".1");

		// Token: 0x04002C74 RID: 11380
		public static readonly DerObjectIdentifier id_TA_ECDSA_SHA_224 = new DerObjectIdentifier(EacObjectIdentifiers.id_TA_ECDSA + ".2");

		// Token: 0x04002C75 RID: 11381
		public static readonly DerObjectIdentifier id_TA_ECDSA_SHA_256 = new DerObjectIdentifier(EacObjectIdentifiers.id_TA_ECDSA + ".3");

		// Token: 0x04002C76 RID: 11382
		public static readonly DerObjectIdentifier id_TA_ECDSA_SHA_384 = new DerObjectIdentifier(EacObjectIdentifiers.id_TA_ECDSA + ".4");

		// Token: 0x04002C77 RID: 11383
		public static readonly DerObjectIdentifier id_TA_ECDSA_SHA_512 = new DerObjectIdentifier(EacObjectIdentifiers.id_TA_ECDSA + ".5");
	}
}
