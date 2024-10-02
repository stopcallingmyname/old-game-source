using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200046F RID: 1135
	public interface TlsHandshakeHash : IDigest
	{
		// Token: 0x06002C5E RID: 11358
		void Init(TlsContext context);

		// Token: 0x06002C5F RID: 11359
		TlsHandshakeHash NotifyPrfDetermined();

		// Token: 0x06002C60 RID: 11360
		void TrackHashAlgorithm(byte hashAlgorithm);

		// Token: 0x06002C61 RID: 11361
		void SealHashAlgorithms();

		// Token: 0x06002C62 RID: 11362
		TlsHandshakeHash StopTracking();

		// Token: 0x06002C63 RID: 11363
		IDigest ForkPrfHash();

		// Token: 0x06002C64 RID: 11364
		byte[] GetFinalHash(byte hashAlgorithm);
	}
}
