using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000463 RID: 1123
	public interface TlsDHVerifier
	{
		// Token: 0x06002BD8 RID: 11224
		bool Accept(DHParameters dhParameters);
	}
}
