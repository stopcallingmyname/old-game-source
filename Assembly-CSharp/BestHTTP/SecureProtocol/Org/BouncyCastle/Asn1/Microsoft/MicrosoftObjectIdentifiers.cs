using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Microsoft
{
	// Token: 0x02000722 RID: 1826
	public abstract class MicrosoftObjectIdentifiers
	{
		// Token: 0x04002B46 RID: 11078
		public static readonly DerObjectIdentifier Microsoft = new DerObjectIdentifier("1.3.6.1.4.1.311");

		// Token: 0x04002B47 RID: 11079
		public static readonly DerObjectIdentifier MicrosoftCertTemplateV1 = MicrosoftObjectIdentifiers.Microsoft.Branch("20.2");

		// Token: 0x04002B48 RID: 11080
		public static readonly DerObjectIdentifier MicrosoftCAVersion = MicrosoftObjectIdentifiers.Microsoft.Branch("21.1");

		// Token: 0x04002B49 RID: 11081
		public static readonly DerObjectIdentifier MicrosoftPrevCACertHash = MicrosoftObjectIdentifiers.Microsoft.Branch("21.2");

		// Token: 0x04002B4A RID: 11082
		public static readonly DerObjectIdentifier MicrosoftCrlNextPublish = MicrosoftObjectIdentifiers.Microsoft.Branch("21.4");

		// Token: 0x04002B4B RID: 11083
		public static readonly DerObjectIdentifier MicrosoftCertTemplateV2 = MicrosoftObjectIdentifiers.Microsoft.Branch("21.7");

		// Token: 0x04002B4C RID: 11084
		public static readonly DerObjectIdentifier MicrosoftAppPolicies = MicrosoftObjectIdentifiers.Microsoft.Branch("21.10");
	}
}
