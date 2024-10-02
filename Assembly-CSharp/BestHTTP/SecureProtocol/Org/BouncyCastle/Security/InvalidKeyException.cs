using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002DF RID: 735
	[Serializable]
	public class InvalidKeyException : KeyException
	{
		// Token: 0x06001AEB RID: 6891 RVA: 0x000CAF8F File Offset: 0x000C918F
		public InvalidKeyException()
		{
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x000CAF97 File Offset: 0x000C9197
		public InvalidKeyException(string message) : base(message)
		{
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x000CAFA0 File Offset: 0x000C91A0
		public InvalidKeyException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
