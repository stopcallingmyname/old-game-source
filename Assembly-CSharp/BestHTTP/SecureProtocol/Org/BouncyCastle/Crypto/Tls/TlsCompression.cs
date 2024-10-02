using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200045C RID: 1116
	public interface TlsCompression
	{
		// Token: 0x06002B9A RID: 11162
		Stream Compress(Stream output);

		// Token: 0x06002B9B RID: 11163
		Stream Decompress(Stream output);
	}
}
