using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000454 RID: 1108
	public interface TlsAuthentication
	{
		// Token: 0x06002B66 RID: 11110
		void NotifyServerCertificate(Certificate serverCertificate);

		// Token: 0x06002B67 RID: 11111
		TlsCredentials GetClientCredentials(TlsContext context, CertificateRequest certificateRequest);
	}
}
