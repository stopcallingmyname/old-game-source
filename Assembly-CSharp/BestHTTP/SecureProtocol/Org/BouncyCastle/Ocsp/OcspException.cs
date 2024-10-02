using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp
{
	// Token: 0x020002F7 RID: 759
	[Serializable]
	public class OcspException : Exception
	{
		// Token: 0x06001BA5 RID: 7077 RVA: 0x0008BF1D File Offset: 0x0008A11D
		public OcspException()
		{
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x0008BF89 File Offset: 0x0008A189
		public OcspException(string message) : base(message)
		{
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x0008BF92 File Offset: 0x0008A192
		public OcspException(string message, Exception e) : base(message, e)
		{
		}
	}
}
