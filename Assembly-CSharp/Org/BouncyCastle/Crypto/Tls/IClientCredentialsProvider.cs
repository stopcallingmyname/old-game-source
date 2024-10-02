using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000168 RID: 360
	public interface IClientCredentialsProvider
	{
		// Token: 0x06000CA5 RID: 3237
		TlsCredentials GetClientCredentials(TlsContext context, CertificateRequest certificateRequest);
	}
}
