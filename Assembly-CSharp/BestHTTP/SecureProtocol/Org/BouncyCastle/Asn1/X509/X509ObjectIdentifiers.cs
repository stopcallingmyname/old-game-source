using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006C9 RID: 1737
	public abstract class X509ObjectIdentifiers
	{
		// Token: 0x040028B9 RID: 10425
		internal const string ID = "2.5.4";

		// Token: 0x040028BA RID: 10426
		public static readonly DerObjectIdentifier CommonName = new DerObjectIdentifier("2.5.4.3");

		// Token: 0x040028BB RID: 10427
		public static readonly DerObjectIdentifier CountryName = new DerObjectIdentifier("2.5.4.6");

		// Token: 0x040028BC RID: 10428
		public static readonly DerObjectIdentifier LocalityName = new DerObjectIdentifier("2.5.4.7");

		// Token: 0x040028BD RID: 10429
		public static readonly DerObjectIdentifier StateOrProvinceName = new DerObjectIdentifier("2.5.4.8");

		// Token: 0x040028BE RID: 10430
		public static readonly DerObjectIdentifier Organization = new DerObjectIdentifier("2.5.4.10");

		// Token: 0x040028BF RID: 10431
		public static readonly DerObjectIdentifier OrganizationalUnitName = new DerObjectIdentifier("2.5.4.11");

		// Token: 0x040028C0 RID: 10432
		public static readonly DerObjectIdentifier id_at_telephoneNumber = new DerObjectIdentifier("2.5.4.20");

		// Token: 0x040028C1 RID: 10433
		public static readonly DerObjectIdentifier id_at_name = new DerObjectIdentifier("2.5.4.41");

		// Token: 0x040028C2 RID: 10434
		public static readonly DerObjectIdentifier id_at_organizationIdentifier = new DerObjectIdentifier("2.5.4.97");

		// Token: 0x040028C3 RID: 10435
		public static readonly DerObjectIdentifier IdSha1 = new DerObjectIdentifier("1.3.14.3.2.26");

		// Token: 0x040028C4 RID: 10436
		public static readonly DerObjectIdentifier RipeMD160 = new DerObjectIdentifier("1.3.36.3.2.1");

		// Token: 0x040028C5 RID: 10437
		public static readonly DerObjectIdentifier RipeMD160WithRsaEncryption = new DerObjectIdentifier("1.3.36.3.3.1.2");

		// Token: 0x040028C6 RID: 10438
		public static readonly DerObjectIdentifier IdEARsa = new DerObjectIdentifier("2.5.8.1.1");

		// Token: 0x040028C7 RID: 10439
		public static readonly DerObjectIdentifier IdPkix = new DerObjectIdentifier("1.3.6.1.5.5.7");

		// Token: 0x040028C8 RID: 10440
		public static readonly DerObjectIdentifier IdPE = new DerObjectIdentifier(X509ObjectIdentifiers.IdPkix + ".1");

		// Token: 0x040028C9 RID: 10441
		public static readonly DerObjectIdentifier IdAD = new DerObjectIdentifier(X509ObjectIdentifiers.IdPkix + ".48");

		// Token: 0x040028CA RID: 10442
		public static readonly DerObjectIdentifier IdADCAIssuers = new DerObjectIdentifier(X509ObjectIdentifiers.IdAD + ".2");

		// Token: 0x040028CB RID: 10443
		public static readonly DerObjectIdentifier IdADOcsp = new DerObjectIdentifier(X509ObjectIdentifiers.IdAD + ".1");

		// Token: 0x040028CC RID: 10444
		public static readonly DerObjectIdentifier OcspAccessMethod = X509ObjectIdentifiers.IdADOcsp;

		// Token: 0x040028CD RID: 10445
		public static readonly DerObjectIdentifier CrlAccessMethod = X509ObjectIdentifiers.IdADCAIssuers;
	}
}
