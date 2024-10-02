using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000590 RID: 1424
	public class SeedWrapEngine : Rfc3394WrapEngine
	{
		// Token: 0x060035D9 RID: 13785 RVA: 0x00149374 File Offset: 0x00147574
		public SeedWrapEngine() : base(new SeedEngine())
		{
		}
	}
}
