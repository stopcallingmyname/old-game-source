using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x020004B2 RID: 1202
	public interface IRandomGenerator
	{
		// Token: 0x06002F01 RID: 12033
		void AddSeedMaterial(byte[] seed);

		// Token: 0x06002F02 RID: 12034
		void AddSeedMaterial(long seed);

		// Token: 0x06002F03 RID: 12035
		void NextBytes(byte[] bytes);

		// Token: 0x06002F04 RID: 12036
		void NextBytes(byte[] bytes, int start, int len);
	}
}
