using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Icao
{
	// Token: 0x02000732 RID: 1842
	public abstract class IcaoObjectIdentifiers
	{
		// Token: 0x04002B97 RID: 11159
		public static readonly DerObjectIdentifier IdIcao = new DerObjectIdentifier("2.23.136");

		// Token: 0x04002B98 RID: 11160
		public static readonly DerObjectIdentifier IdIcaoMrtd = IcaoObjectIdentifiers.IdIcao.Branch("1");

		// Token: 0x04002B99 RID: 11161
		public static readonly DerObjectIdentifier IdIcaoMrtdSecurity = IcaoObjectIdentifiers.IdIcaoMrtd.Branch("1");

		// Token: 0x04002B9A RID: 11162
		public static readonly DerObjectIdentifier IdIcaoLdsSecurityObject = IcaoObjectIdentifiers.IdIcaoMrtdSecurity.Branch("1");

		// Token: 0x04002B9B RID: 11163
		public static readonly DerObjectIdentifier IdIcaoCscaMasterList = IcaoObjectIdentifiers.IdIcaoMrtdSecurity.Branch("2");

		// Token: 0x04002B9C RID: 11164
		public static readonly DerObjectIdentifier IdIcaoCscaMasterListSigningKey = IcaoObjectIdentifiers.IdIcaoMrtdSecurity.Branch("3");

		// Token: 0x04002B9D RID: 11165
		public static readonly DerObjectIdentifier IdIcaoDocumentTypeList = IcaoObjectIdentifiers.IdIcaoMrtdSecurity.Branch("4");

		// Token: 0x04002B9E RID: 11166
		public static readonly DerObjectIdentifier IdIcaoAAProtocolObject = IcaoObjectIdentifiers.IdIcaoMrtdSecurity.Branch("5");

		// Token: 0x04002B9F RID: 11167
		public static readonly DerObjectIdentifier IdIcaoExtensions = IcaoObjectIdentifiers.IdIcaoMrtdSecurity.Branch("6");

		// Token: 0x04002BA0 RID: 11168
		public static readonly DerObjectIdentifier IdIcaoExtensionsNamechangekeyrollover = IcaoObjectIdentifiers.IdIcaoExtensions.Branch("1");
	}
}
