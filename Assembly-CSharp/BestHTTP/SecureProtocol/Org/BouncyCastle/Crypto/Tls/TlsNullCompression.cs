using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000474 RID: 1140
	public class TlsNullCompression : TlsCompression
	{
		// Token: 0x06002C81 RID: 11393 RVA: 0x000A54F9 File Offset: 0x000A36F9
		public virtual Stream Compress(Stream output)
		{
			return output;
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x000A54F9 File Offset: 0x000A36F9
		public virtual Stream Decompress(Stream output)
		{
			return output;
		}
	}
}
