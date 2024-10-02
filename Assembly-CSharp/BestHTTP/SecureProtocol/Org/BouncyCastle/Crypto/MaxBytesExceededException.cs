using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003EB RID: 1003
	[Serializable]
	public class MaxBytesExceededException : CryptoException
	{
		// Token: 0x06002882 RID: 10370 RVA: 0x0010CACF File Offset: 0x0010ACCF
		public MaxBytesExceededException()
		{
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x0010CAD7 File Offset: 0x0010ACD7
		public MaxBytesExceededException(string message) : base(message)
		{
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x0010CAE0 File Offset: 0x0010ACE0
		public MaxBytesExceededException(string message, Exception e) : base(message, e)
		{
		}
	}
}
