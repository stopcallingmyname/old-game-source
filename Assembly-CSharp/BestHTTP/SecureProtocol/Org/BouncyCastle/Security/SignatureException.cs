using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002EA RID: 746
	[Serializable]
	public class SignatureException : GeneralSecurityException
	{
		// Token: 0x06001B52 RID: 6994 RVA: 0x000BC908 File Offset: 0x000BAB08
		public SignatureException()
		{
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x000BC910 File Offset: 0x000BAB10
		public SignatureException(string message) : base(message)
		{
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x000BC919 File Offset: 0x000BAB19
		public SignatureException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
