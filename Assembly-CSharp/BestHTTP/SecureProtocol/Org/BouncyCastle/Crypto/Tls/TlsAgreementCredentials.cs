using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000453 RID: 1107
	public interface TlsAgreementCredentials : TlsCredentials
	{
		// Token: 0x06002B65 RID: 11109
		byte[] GenerateAgreement(AsymmetricKeyParameter peerPublicKey);
	}
}
