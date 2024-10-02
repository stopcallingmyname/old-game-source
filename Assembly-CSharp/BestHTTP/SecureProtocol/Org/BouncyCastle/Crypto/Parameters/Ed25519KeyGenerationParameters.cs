using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004D6 RID: 1238
	public class Ed25519KeyGenerationParameters : KeyGenerationParameters
	{
		// Token: 0x06002FFE RID: 12286 RVA: 0x00126A68 File Offset: 0x00124C68
		public Ed25519KeyGenerationParameters(SecureRandom random) : base(random, 256)
		{
		}
	}
}
