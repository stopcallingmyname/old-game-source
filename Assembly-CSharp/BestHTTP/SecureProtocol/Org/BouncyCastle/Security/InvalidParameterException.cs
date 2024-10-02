using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002E0 RID: 736
	[Serializable]
	public class InvalidParameterException : KeyException
	{
		// Token: 0x06001AEE RID: 6894 RVA: 0x000CAF8F File Offset: 0x000C918F
		public InvalidParameterException()
		{
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x000CAF97 File Offset: 0x000C9197
		public InvalidParameterException(string message) : base(message)
		{
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x000CAFA0 File Offset: 0x000C91A0
		public InvalidParameterException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
