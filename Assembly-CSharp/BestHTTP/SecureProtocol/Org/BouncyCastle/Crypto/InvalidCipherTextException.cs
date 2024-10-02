using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003DD RID: 989
	[Serializable]
	public class InvalidCipherTextException : CryptoException
	{
		// Token: 0x06002855 RID: 10325 RVA: 0x0010CACF File Offset: 0x0010ACCF
		public InvalidCipherTextException()
		{
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x0010CAD7 File Offset: 0x0010ACD7
		public InvalidCipherTextException(string message) : base(message)
		{
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x0010CAE0 File Offset: 0x0010ACE0
		public InvalidCipherTextException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
