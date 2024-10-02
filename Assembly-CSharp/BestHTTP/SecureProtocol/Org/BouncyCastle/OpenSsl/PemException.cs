using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.OpenSsl
{
	// Token: 0x020002D2 RID: 722
	[Serializable]
	public class PemException : IOException
	{
		// Token: 0x06001A81 RID: 6785 RVA: 0x000B7FE6 File Offset: 0x000B61E6
		public PemException(string message) : base(message)
		{
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x000B7FEF File Offset: 0x000B61EF
		public PemException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
