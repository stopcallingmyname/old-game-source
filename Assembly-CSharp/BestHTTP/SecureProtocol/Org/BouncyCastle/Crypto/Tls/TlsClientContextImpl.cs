using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200045A RID: 1114
	internal class TlsClientContextImpl : AbstractTlsContext, TlsClientContext, TlsContext
	{
		// Token: 0x06002B89 RID: 11145 RVA: 0x001155D3 File Offset: 0x001137D3
		internal TlsClientContextImpl(SecureRandom secureRandom, SecurityParameters securityParameters) : base(secureRandom, securityParameters)
		{
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06002B8A RID: 11146 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool IsServer
		{
			get
			{
				return false;
			}
		}
	}
}
