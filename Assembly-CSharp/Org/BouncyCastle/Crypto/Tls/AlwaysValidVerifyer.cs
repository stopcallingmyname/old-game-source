using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000166 RID: 358
	public class AlwaysValidVerifyer : ICertificateVerifyer
	{
		// Token: 0x06000CA2 RID: 3234 RVA: 0x0006AE98 File Offset: 0x00069098
		public bool IsValid(Uri targetUri, X509CertificateStructure[] certs)
		{
			return true;
		}
	}
}
