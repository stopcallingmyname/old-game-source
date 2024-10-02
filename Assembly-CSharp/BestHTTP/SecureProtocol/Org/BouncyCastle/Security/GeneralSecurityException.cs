using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002DD RID: 733
	[Serializable]
	public class GeneralSecurityException : Exception
	{
		// Token: 0x06001AD9 RID: 6873 RVA: 0x0008BF1D File Offset: 0x0008A11D
		public GeneralSecurityException()
		{
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x0008BF89 File Offset: 0x0008A189
		public GeneralSecurityException(string message) : base(message)
		{
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x0008BF92 File Offset: 0x0008A192
		public GeneralSecurityException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
