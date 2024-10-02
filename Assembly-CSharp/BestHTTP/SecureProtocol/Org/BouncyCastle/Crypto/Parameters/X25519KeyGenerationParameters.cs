using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000503 RID: 1283
	public class X25519KeyGenerationParameters : KeyGenerationParameters
	{
		// Token: 0x060030D0 RID: 12496 RVA: 0x00128290 File Offset: 0x00126490
		public X25519KeyGenerationParameters(SecureRandom random) : base(random, 255)
		{
		}
	}
}
