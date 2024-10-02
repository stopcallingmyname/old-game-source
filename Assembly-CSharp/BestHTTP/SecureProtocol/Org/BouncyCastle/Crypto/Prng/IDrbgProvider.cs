using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng.Drbg;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x020004B1 RID: 1201
	internal interface IDrbgProvider
	{
		// Token: 0x06002F00 RID: 12032
		ISP80090Drbg Get(IEntropySource entropySource);
	}
}
