using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003CD RID: 973
	[Serializable]
	public class DataLengthException : CryptoException
	{
		// Token: 0x06002814 RID: 10260 RVA: 0x0010CACF File Offset: 0x0010ACCF
		public DataLengthException()
		{
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x0010CAD7 File Offset: 0x0010ACD7
		public DataLengthException(string message) : base(message)
		{
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x0010CAE0 File Offset: 0x0010ACE0
		public DataLengthException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
