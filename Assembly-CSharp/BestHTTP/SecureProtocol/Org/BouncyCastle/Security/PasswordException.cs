using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002D8 RID: 728
	[Serializable]
	public class PasswordException : IOException
	{
		// Token: 0x06001AA1 RID: 6817 RVA: 0x000B7FE6 File Offset: 0x000B61E6
		public PasswordException(string message) : base(message)
		{
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x000B7FEF File Offset: 0x000B61EF
		public PasswordException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
