using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003CC RID: 972
	[Serializable]
	public class CryptoException : Exception
	{
		// Token: 0x06002811 RID: 10257 RVA: 0x0008BF1D File Offset: 0x0008A11D
		public CryptoException()
		{
		}

		// Token: 0x06002812 RID: 10258 RVA: 0x0008BF89 File Offset: 0x0008A189
		public CryptoException(string message) : base(message)
		{
		}

		// Token: 0x06002813 RID: 10259 RVA: 0x0008BF92 File Offset: 0x0008A192
		public CryptoException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
