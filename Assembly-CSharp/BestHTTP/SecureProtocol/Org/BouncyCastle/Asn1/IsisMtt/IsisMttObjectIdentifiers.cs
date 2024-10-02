using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.IsisMtt
{
	// Token: 0x02000724 RID: 1828
	public abstract class IsisMttObjectIdentifiers
	{
		// Token: 0x04002B4F RID: 11087
		public static readonly DerObjectIdentifier IdIsisMtt = new DerObjectIdentifier("1.3.36.8");

		// Token: 0x04002B50 RID: 11088
		public static readonly DerObjectIdentifier IdIsisMttCP = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMtt + ".1");

		// Token: 0x04002B51 RID: 11089
		public static readonly DerObjectIdentifier IdIsisMttCPAccredited = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttCP + ".1");

		// Token: 0x04002B52 RID: 11090
		public static readonly DerObjectIdentifier IdIsisMttAT = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMtt + ".3");

		// Token: 0x04002B53 RID: 11091
		public static readonly DerObjectIdentifier IdIsisMttATDateOfCertGen = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttAT + ".1");

		// Token: 0x04002B54 RID: 11092
		public static readonly DerObjectIdentifier IdIsisMttATProcuration = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttAT + ".2");

		// Token: 0x04002B55 RID: 11093
		public static readonly DerObjectIdentifier IdIsisMttATAdmission = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttAT + ".3");

		// Token: 0x04002B56 RID: 11094
		public static readonly DerObjectIdentifier IdIsisMttATMonetaryLimit = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttAT + ".4");

		// Token: 0x04002B57 RID: 11095
		public static readonly DerObjectIdentifier IdIsisMttATDeclarationOfMajority = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttAT + ".5");

		// Token: 0x04002B58 RID: 11096
		public static readonly DerObjectIdentifier IdIsisMttATIccsn = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttAT + ".6");

		// Token: 0x04002B59 RID: 11097
		public static readonly DerObjectIdentifier IdIsisMttATPKReference = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttAT + ".7");

		// Token: 0x04002B5A RID: 11098
		public static readonly DerObjectIdentifier IdIsisMttATRestriction = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttAT + ".8");

		// Token: 0x04002B5B RID: 11099
		public static readonly DerObjectIdentifier IdIsisMttATRetrieveIfAllowed = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttAT + ".9");

		// Token: 0x04002B5C RID: 11100
		public static readonly DerObjectIdentifier IdIsisMttATRequestedCertificate = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttAT + ".10");

		// Token: 0x04002B5D RID: 11101
		public static readonly DerObjectIdentifier IdIsisMttATNamingAuthorities = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttAT + ".11");

		// Token: 0x04002B5E RID: 11102
		public static readonly DerObjectIdentifier IdIsisMttATCertInDirSince = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttAT + ".12");

		// Token: 0x04002B5F RID: 11103
		public static readonly DerObjectIdentifier IdIsisMttATCertHash = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttAT + ".13");

		// Token: 0x04002B60 RID: 11104
		public static readonly DerObjectIdentifier IdIsisMttATNameAtBirth = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttAT + ".14");

		// Token: 0x04002B61 RID: 11105
		public static readonly DerObjectIdentifier IdIsisMttATAdditionalInformation = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttAT + ".15");

		// Token: 0x04002B62 RID: 11106
		public static readonly DerObjectIdentifier IdIsisMttATLiabilityLimitationFlag = new DerObjectIdentifier("0.2.262.1.10.12.0");
	}
}
