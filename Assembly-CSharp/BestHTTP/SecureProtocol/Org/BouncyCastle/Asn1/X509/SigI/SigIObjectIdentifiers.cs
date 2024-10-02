using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509.SigI
{
	// Token: 0x020006CC RID: 1740
	public sealed class SigIObjectIdentifiers
	{
		// Token: 0x0600402E RID: 16430 RVA: 0x00022F1F File Offset: 0x0002111F
		private SigIObjectIdentifiers()
		{
		}

		// Token: 0x040028D7 RID: 10455
		public static readonly DerObjectIdentifier IdSigI = new DerObjectIdentifier("1.3.36.8");

		// Token: 0x040028D8 RID: 10456
		public static readonly DerObjectIdentifier IdSigIKP = new DerObjectIdentifier(SigIObjectIdentifiers.IdSigI + ".2");

		// Token: 0x040028D9 RID: 10457
		public static readonly DerObjectIdentifier IdSigICP = new DerObjectIdentifier(SigIObjectIdentifiers.IdSigI + ".1");

		// Token: 0x040028DA RID: 10458
		public static readonly DerObjectIdentifier IdSigION = new DerObjectIdentifier(SigIObjectIdentifiers.IdSigI + ".4");

		// Token: 0x040028DB RID: 10459
		public static readonly DerObjectIdentifier IdSigIKPDirectoryService = new DerObjectIdentifier(SigIObjectIdentifiers.IdSigIKP + ".1");

		// Token: 0x040028DC RID: 10460
		public static readonly DerObjectIdentifier IdSigIONPersonalData = new DerObjectIdentifier(SigIObjectIdentifiers.IdSigION + ".1");

		// Token: 0x040028DD RID: 10461
		public static readonly DerObjectIdentifier IdSigICPSigConform = new DerObjectIdentifier(SigIObjectIdentifiers.IdSigICP + ".1");
	}
}
