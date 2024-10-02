using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates
{
	// Token: 0x020002F1 RID: 753
	[Serializable]
	public class CertificateParsingException : CertificateException
	{
		// Token: 0x06001B72 RID: 7026 RVA: 0x000D0233 File Offset: 0x000CE433
		public CertificateParsingException()
		{
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x000D023B File Offset: 0x000CE43B
		public CertificateParsingException(string message) : base(message)
		{
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x000D0244 File Offset: 0x000CE444
		public CertificateParsingException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
