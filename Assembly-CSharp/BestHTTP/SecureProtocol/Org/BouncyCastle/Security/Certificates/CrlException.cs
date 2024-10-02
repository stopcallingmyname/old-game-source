using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates
{
	// Token: 0x020002F2 RID: 754
	[Serializable]
	public class CrlException : GeneralSecurityException
	{
		// Token: 0x06001B75 RID: 7029 RVA: 0x000BC908 File Offset: 0x000BAB08
		public CrlException()
		{
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x000BC910 File Offset: 0x000BAB10
		public CrlException(string msg) : base(msg)
		{
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x000BC919 File Offset: 0x000BAB19
		public CrlException(string msg, Exception e) : base(msg, e)
		{
		}
	}
}
