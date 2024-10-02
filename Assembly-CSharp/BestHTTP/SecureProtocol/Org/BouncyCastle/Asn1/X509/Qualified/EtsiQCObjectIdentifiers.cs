using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509.Qualified
{
	// Token: 0x020006CE RID: 1742
	public abstract class EtsiQCObjectIdentifiers
	{
		// Token: 0x040028E2 RID: 10466
		public static readonly DerObjectIdentifier IdEtsiQcs = new DerObjectIdentifier("0.4.0.1862.1");

		// Token: 0x040028E3 RID: 10467
		public static readonly DerObjectIdentifier IdEtsiQcsQcCompliance = new DerObjectIdentifier(EtsiQCObjectIdentifiers.IdEtsiQcs + ".1");

		// Token: 0x040028E4 RID: 10468
		public static readonly DerObjectIdentifier IdEtsiQcsLimitValue = new DerObjectIdentifier(EtsiQCObjectIdentifiers.IdEtsiQcs + ".2");

		// Token: 0x040028E5 RID: 10469
		public static readonly DerObjectIdentifier IdEtsiQcsRetentionPeriod = new DerObjectIdentifier(EtsiQCObjectIdentifiers.IdEtsiQcs + ".3");

		// Token: 0x040028E6 RID: 10470
		public static readonly DerObjectIdentifier IdEtsiQcsQcSscd = new DerObjectIdentifier(EtsiQCObjectIdentifiers.IdEtsiQcs + ".4");
	}
}
