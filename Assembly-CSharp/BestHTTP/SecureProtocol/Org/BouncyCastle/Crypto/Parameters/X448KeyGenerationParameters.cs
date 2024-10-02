using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000506 RID: 1286
	public class X448KeyGenerationParameters : KeyGenerationParameters
	{
		// Token: 0x060030DE RID: 12510 RVA: 0x00126CB5 File Offset: 0x00124EB5
		public X448KeyGenerationParameters(SecureRandom random) : base(random, 448)
		{
		}
	}
}
