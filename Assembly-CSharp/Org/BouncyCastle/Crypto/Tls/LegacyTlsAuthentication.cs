using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000169 RID: 361
	public class LegacyTlsAuthentication : TlsAuthentication
	{
		// Token: 0x06000CA6 RID: 3238 RVA: 0x0008F95B File Offset: 0x0008DB5B
		public LegacyTlsAuthentication(Uri targetUri, ICertificateVerifyer verifyer, IClientCredentialsProvider prov)
		{
			this.TargetUri = targetUri;
			this.verifyer = verifyer;
			this.credProvider = prov;
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0008F978 File Offset: 0x0008DB78
		public virtual void NotifyServerCertificate(Certificate serverCertificate)
		{
			if (!this.verifyer.IsValid(this.TargetUri, serverCertificate.GetCertificateList()))
			{
				throw new TlsFatalAlert(90);
			}
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x0008F99B File Offset: 0x0008DB9B
		public virtual TlsCredentials GetClientCredentials(TlsContext context, CertificateRequest certificateRequest)
		{
			if (this.credProvider != null)
			{
				return this.credProvider.GetClientCredentials(context, certificateRequest);
			}
			return null;
		}

		// Token: 0x04001278 RID: 4728
		protected ICertificateVerifyer verifyer;

		// Token: 0x04001279 RID: 4729
		protected IClientCredentialsProvider credProvider;

		// Token: 0x0400127A RID: 4730
		protected Uri TargetUri;
	}
}
