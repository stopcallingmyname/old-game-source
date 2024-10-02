using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002D7 RID: 727
	[Serializable]
	public class EncryptionException : IOException
	{
		// Token: 0x06001A9F RID: 6815 RVA: 0x000B7FE6 File Offset: 0x000B61E6
		public EncryptionException(string message) : base(message)
		{
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x000B7FEF File Offset: 0x000B61EF
		public EncryptionException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
