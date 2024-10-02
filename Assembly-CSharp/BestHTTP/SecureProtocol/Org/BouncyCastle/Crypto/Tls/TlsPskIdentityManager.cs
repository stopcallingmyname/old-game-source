using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000479 RID: 1145
	public interface TlsPskIdentityManager
	{
		// Token: 0x06002CD2 RID: 11474
		byte[] GetHint();

		// Token: 0x06002CD3 RID: 11475
		byte[] GetPsk(byte[] identity);
	}
}
