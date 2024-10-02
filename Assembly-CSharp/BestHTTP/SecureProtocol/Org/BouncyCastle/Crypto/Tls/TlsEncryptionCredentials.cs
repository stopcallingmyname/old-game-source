using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200046A RID: 1130
	public interface TlsEncryptionCredentials : TlsCredentials
	{
		// Token: 0x06002C27 RID: 11303
		byte[] DecryptPreMasterSecret(byte[] encryptedPreMasterSecret);
	}
}
