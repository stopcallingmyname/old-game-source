using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000457 RID: 1111
	public interface TlsCipherFactory
	{
		// Token: 0x06002B74 RID: 11124
		TlsCipher CreateCipher(TlsContext context, int encryptionAlgorithm, int macAlgorithm);
	}
}
