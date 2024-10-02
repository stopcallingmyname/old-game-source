using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates
{
	// Token: 0x020002ED RID: 749
	[Serializable]
	public class CertificateEncodingException : CertificateException
	{
		// Token: 0x06001B66 RID: 7014 RVA: 0x000D0233 File Offset: 0x000CE433
		public CertificateEncodingException()
		{
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x000D023B File Offset: 0x000CE43B
		public CertificateEncodingException(string msg) : base(msg)
		{
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x000D0244 File Offset: 0x000CE444
		public CertificateEncodingException(string msg, Exception e) : base(msg, e)
		{
		}
	}
}
