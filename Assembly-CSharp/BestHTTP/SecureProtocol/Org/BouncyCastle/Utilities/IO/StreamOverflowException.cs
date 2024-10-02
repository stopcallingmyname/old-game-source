using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO
{
	// Token: 0x0200027D RID: 637
	[Serializable]
	public class StreamOverflowException : IOException
	{
		// Token: 0x06001787 RID: 6023 RVA: 0x000B7FDE File Offset: 0x000B61DE
		public StreamOverflowException()
		{
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x000B7FE6 File Offset: 0x000B61E6
		public StreamOverflowException(string message) : base(message)
		{
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x000B7FEF File Offset: 0x000B61EF
		public StreamOverflowException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
