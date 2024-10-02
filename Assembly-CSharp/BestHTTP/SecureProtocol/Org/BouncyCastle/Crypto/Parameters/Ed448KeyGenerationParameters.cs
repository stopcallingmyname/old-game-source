using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004D9 RID: 1241
	public class Ed448KeyGenerationParameters : KeyGenerationParameters
	{
		// Token: 0x0600300C RID: 12300 RVA: 0x00126CB5 File Offset: 0x00124EB5
		public Ed448KeyGenerationParameters(SecureRandom random) : base(random, 448)
		{
		}
	}
}
