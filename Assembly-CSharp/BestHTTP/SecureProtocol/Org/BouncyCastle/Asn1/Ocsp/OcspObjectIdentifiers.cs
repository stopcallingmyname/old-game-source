using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x0200070B RID: 1803
	public abstract class OcspObjectIdentifiers
	{
		// Token: 0x04002AAD RID: 10925
		internal const string PkixOcspId = "1.3.6.1.5.5.7.48.1";

		// Token: 0x04002AAE RID: 10926
		public static readonly DerObjectIdentifier PkixOcsp = new DerObjectIdentifier("1.3.6.1.5.5.7.48.1");

		// Token: 0x04002AAF RID: 10927
		public static readonly DerObjectIdentifier PkixOcspBasic = new DerObjectIdentifier("1.3.6.1.5.5.7.48.1.1");

		// Token: 0x04002AB0 RID: 10928
		public static readonly DerObjectIdentifier PkixOcspNonce = new DerObjectIdentifier(OcspObjectIdentifiers.PkixOcsp + ".2");

		// Token: 0x04002AB1 RID: 10929
		public static readonly DerObjectIdentifier PkixOcspCrl = new DerObjectIdentifier(OcspObjectIdentifiers.PkixOcsp + ".3");

		// Token: 0x04002AB2 RID: 10930
		public static readonly DerObjectIdentifier PkixOcspResponse = new DerObjectIdentifier(OcspObjectIdentifiers.PkixOcsp + ".4");

		// Token: 0x04002AB3 RID: 10931
		public static readonly DerObjectIdentifier PkixOcspNocheck = new DerObjectIdentifier(OcspObjectIdentifiers.PkixOcsp + ".5");

		// Token: 0x04002AB4 RID: 10932
		public static readonly DerObjectIdentifier PkixOcspArchiveCutoff = new DerObjectIdentifier(OcspObjectIdentifiers.PkixOcsp + ".6");

		// Token: 0x04002AB5 RID: 10933
		public static readonly DerObjectIdentifier PkixOcspServiceLocator = new DerObjectIdentifier(OcspObjectIdentifiers.PkixOcsp + ".7");
	}
}
