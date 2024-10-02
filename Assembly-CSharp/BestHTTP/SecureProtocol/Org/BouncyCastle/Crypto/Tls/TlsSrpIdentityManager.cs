using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000487 RID: 1159
	public interface TlsSrpIdentityManager
	{
		// Token: 0x06002D38 RID: 11576
		TlsSrpLoginParameters GetLoginParameters(byte[] identity);
	}
}
