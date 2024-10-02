using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates
{
	// Token: 0x020002F0 RID: 752
	[Serializable]
	public class CertificateNotYetValidException : CertificateException
	{
		// Token: 0x06001B6F RID: 7023 RVA: 0x000D0233 File Offset: 0x000CE433
		public CertificateNotYetValidException()
		{
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x000D023B File Offset: 0x000CE43B
		public CertificateNotYetValidException(string message) : base(message)
		{
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x000D0244 File Offset: 0x000CE444
		public CertificateNotYetValidException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
