using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix
{
	// Token: 0x020002B6 RID: 694
	[Serializable]
	public class PkixCertPathBuilderException : GeneralSecurityException
	{
		// Token: 0x06001928 RID: 6440 RVA: 0x000BC908 File Offset: 0x000BAB08
		public PkixCertPathBuilderException()
		{
		}

		// Token: 0x06001929 RID: 6441 RVA: 0x000BC910 File Offset: 0x000BAB10
		public PkixCertPathBuilderException(string message) : base(message)
		{
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x000BC919 File Offset: 0x000BAB19
		public PkixCertPathBuilderException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
