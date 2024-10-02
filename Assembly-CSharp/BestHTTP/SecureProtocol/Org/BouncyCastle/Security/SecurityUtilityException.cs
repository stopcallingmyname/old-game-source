using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002E9 RID: 745
	[Serializable]
	public class SecurityUtilityException : Exception
	{
		// Token: 0x06001B4F RID: 6991 RVA: 0x0008BF1D File Offset: 0x0008A11D
		public SecurityUtilityException()
		{
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x0008BF89 File Offset: 0x0008A189
		public SecurityUtilityException(string message) : base(message)
		{
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x0008BF92 File Offset: 0x0008A192
		public SecurityUtilityException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
