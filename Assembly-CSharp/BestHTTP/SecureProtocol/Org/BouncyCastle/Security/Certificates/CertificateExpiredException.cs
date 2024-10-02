using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates
{
	// Token: 0x020002EF RID: 751
	[Serializable]
	public class CertificateExpiredException : CertificateException
	{
		// Token: 0x06001B6C RID: 7020 RVA: 0x000D0233 File Offset: 0x000CE433
		public CertificateExpiredException()
		{
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x000D023B File Offset: 0x000CE43B
		public CertificateExpiredException(string message) : base(message)
		{
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x000D0244 File Offset: 0x000CE444
		public CertificateExpiredException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
