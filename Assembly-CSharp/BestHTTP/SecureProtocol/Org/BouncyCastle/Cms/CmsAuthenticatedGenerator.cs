using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005E6 RID: 1510
	public class CmsAuthenticatedGenerator : CmsEnvelopedGenerator
	{
		// Token: 0x0600399B RID: 14747 RVA: 0x001676CF File Offset: 0x001658CF
		public CmsAuthenticatedGenerator()
		{
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x001676D7 File Offset: 0x001658D7
		public CmsAuthenticatedGenerator(SecureRandom rand) : base(rand)
		{
		}
	}
}
