using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates
{
	// Token: 0x020002EE RID: 750
	[Serializable]
	public class CertificateException : GeneralSecurityException
	{
		// Token: 0x06001B69 RID: 7017 RVA: 0x000BC908 File Offset: 0x000BAB08
		public CertificateException()
		{
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x000BC910 File Offset: 0x000BAB10
		public CertificateException(string message) : base(message)
		{
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x000BC919 File Offset: 0x000BAB19
		public CertificateException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
