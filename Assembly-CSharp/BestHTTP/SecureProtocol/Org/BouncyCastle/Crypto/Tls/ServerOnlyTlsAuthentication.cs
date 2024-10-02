using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000445 RID: 1093
	public abstract class ServerOnlyTlsAuthentication : TlsAuthentication
	{
		// Token: 0x06002B19 RID: 11033
		public abstract void NotifyServerCertificate(Certificate serverCertificate);

		// Token: 0x06002B1A RID: 11034 RVA: 0x0008D54A File Offset: 0x0008B74A
		public TlsCredentials GetClientCredentials(TlsContext context, CertificateRequest certificateRequest)
		{
			return null;
		}
	}
}
