using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000486 RID: 1158
	public interface TlsSrpGroupVerifier
	{
		// Token: 0x06002D37 RID: 11575
		bool Accept(Srp6GroupParameters group);
	}
}
