using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000475 RID: 1141
	public interface TlsPeer
	{
		// Token: 0x06002C84 RID: 11396
		bool RequiresExtendedMasterSecret();

		// Token: 0x06002C85 RID: 11397
		bool ShouldUseGmtUnixTime();

		// Token: 0x06002C86 RID: 11398
		void NotifySecureRenegotiation(bool secureRenegotiation);

		// Token: 0x06002C87 RID: 11399
		TlsCompression GetCompression();

		// Token: 0x06002C88 RID: 11400
		TlsCipher GetCipher();

		// Token: 0x06002C89 RID: 11401
		void NotifyAlertRaised(byte alertLevel, byte alertDescription, string message, Exception cause);

		// Token: 0x06002C8A RID: 11402
		void NotifyAlertReceived(byte alertLevel, byte alertDescription);

		// Token: 0x06002C8B RID: 11403
		void NotifyHandshakeComplete();
	}
}
