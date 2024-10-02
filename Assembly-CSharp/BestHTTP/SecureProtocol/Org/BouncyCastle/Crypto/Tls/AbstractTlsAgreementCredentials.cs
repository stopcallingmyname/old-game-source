using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F0 RID: 1008
	public abstract class AbstractTlsAgreementCredentials : AbstractTlsCredentials, TlsAgreementCredentials, TlsCredentials
	{
		// Token: 0x060028CD RID: 10445
		public abstract byte[] GenerateAgreement(AsymmetricKeyParameter peerPublicKey);
	}
}
