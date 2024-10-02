using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000167 RID: 359
	public interface ICertificateVerifyer
	{
		// Token: 0x06000CA4 RID: 3236
		bool IsValid(Uri targetUri, X509CertificateStructure[] certs);
	}
}
