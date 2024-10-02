using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000478 RID: 1144
	public interface TlsPskIdentity
	{
		// Token: 0x06002CCE RID: 11470
		void SkipIdentityHint();

		// Token: 0x06002CCF RID: 11471
		void NotifyIdentityHint(byte[] psk_identity_hint);

		// Token: 0x06002CD0 RID: 11472
		byte[] GetPskIdentity();

		// Token: 0x06002CD1 RID: 11473
		byte[] GetPsk();
	}
}
