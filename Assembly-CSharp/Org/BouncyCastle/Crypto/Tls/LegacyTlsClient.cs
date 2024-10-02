using System;
using System.Collections.Generic;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200016A RID: 362
	public sealed class LegacyTlsClient : DefaultTlsClient
	{
		// Token: 0x06000CA9 RID: 3241 RVA: 0x0008F9B4 File Offset: 0x0008DBB4
		public LegacyTlsClient(Uri targetUri, ICertificateVerifyer verifyer, IClientCredentialsProvider prov, List<string> hostNames)
		{
			this.TargetUri = targetUri;
			this.verifyer = verifyer;
			this.credProvider = prov;
			base.HostNames = hostNames;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0008F9D9 File Offset: 0x0008DBD9
		public override TlsAuthentication GetAuthentication()
		{
			return new LegacyTlsAuthentication(this.TargetUri, this.verifyer, this.credProvider);
		}

		// Token: 0x0400127B RID: 4731
		private readonly Uri TargetUri;

		// Token: 0x0400127C RID: 4732
		private readonly ICertificateVerifyer verifyer;

		// Token: 0x0400127D RID: 4733
		private readonly IClientCredentialsProvider credProvider;
	}
}
