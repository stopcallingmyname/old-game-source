using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000480 RID: 1152
	internal class TlsServerContextImpl : AbstractTlsContext, TlsServerContext, TlsContext
	{
		// Token: 0x06002D0A RID: 11530 RVA: 0x001155D3 File Offset: 0x001137D3
		internal TlsServerContextImpl(SecureRandom secureRandom, SecurityParameters securityParameters) : base(secureRandom, securityParameters)
		{
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06002D0B RID: 11531 RVA: 0x0006AE98 File Offset: 0x00069098
		public override bool IsServer
		{
			get
			{
				return true;
			}
		}
	}
}
