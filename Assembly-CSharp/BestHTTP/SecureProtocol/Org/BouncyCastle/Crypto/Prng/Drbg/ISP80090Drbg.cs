using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng.Drbg
{
	// Token: 0x020004BF RID: 1215
	public interface ISP80090Drbg
	{
		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06002F5E RID: 12126
		int BlockSize { get; }

		// Token: 0x06002F5F RID: 12127
		int Generate(byte[] output, byte[] additionalInput, bool predictionResistant);

		// Token: 0x06002F60 RID: 12128
		void Reseed(byte[] additionalInput);
	}
}
