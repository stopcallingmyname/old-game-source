using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000485 RID: 1157
	public interface TlsSignerCredentials : TlsCredentials
	{
		// Token: 0x06002D35 RID: 11573
		byte[] GenerateCertificateSignature(byte[] hash);

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06002D36 RID: 11574
		SignatureAndHashAlgorithm SignatureAndHashAlgorithm { get; }
	}
}
