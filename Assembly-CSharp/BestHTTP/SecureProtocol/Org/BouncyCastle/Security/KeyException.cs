using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002E1 RID: 737
	[Serializable]
	public class KeyException : GeneralSecurityException
	{
		// Token: 0x06001AF1 RID: 6897 RVA: 0x000BC908 File Offset: 0x000BAB08
		public KeyException()
		{
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x000BC910 File Offset: 0x000BAB10
		public KeyException(string message) : base(message)
		{
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x000BC919 File Offset: 0x000BAB19
		public KeyException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
