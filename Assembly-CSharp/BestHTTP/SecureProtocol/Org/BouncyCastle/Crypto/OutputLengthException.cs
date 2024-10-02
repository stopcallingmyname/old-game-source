using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003EC RID: 1004
	[Serializable]
	public class OutputLengthException : DataLengthException
	{
		// Token: 0x06002885 RID: 10373 RVA: 0x0010CB32 File Offset: 0x0010AD32
		public OutputLengthException()
		{
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x0010CB3A File Offset: 0x0010AD3A
		public OutputLengthException(string message) : base(message)
		{
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x0010CB43 File Offset: 0x0010AD43
		public OutputLengthException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
