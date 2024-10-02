using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509.Qualified
{
	// Token: 0x020006D2 RID: 1746
	public sealed class Rfc3739QCObjectIdentifiers
	{
		// Token: 0x06004050 RID: 16464 RVA: 0x00022F1F File Offset: 0x0002111F
		private Rfc3739QCObjectIdentifiers()
		{
		}

		// Token: 0x040028F0 RID: 10480
		public static readonly DerObjectIdentifier IdQcs = new DerObjectIdentifier("1.3.6.1.5.5.7.11");

		// Token: 0x040028F1 RID: 10481
		public static readonly DerObjectIdentifier IdQcsPkixQCSyntaxV1 = new DerObjectIdentifier(Rfc3739QCObjectIdentifiers.IdQcs + ".1");

		// Token: 0x040028F2 RID: 10482
		public static readonly DerObjectIdentifier IdQcsPkixQCSyntaxV2 = new DerObjectIdentifier(Rfc3739QCObjectIdentifiers.IdQcs + ".2");
	}
}
