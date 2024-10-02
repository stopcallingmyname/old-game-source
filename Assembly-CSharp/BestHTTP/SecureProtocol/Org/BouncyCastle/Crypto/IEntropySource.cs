using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003DA RID: 986
	public interface IEntropySource
	{
		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x0600284A RID: 10314
		bool IsPredictionResistant { get; }

		// Token: 0x0600284B RID: 10315
		byte[] GetEntropy();

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x0600284C RID: 10316
		int EntropySize { get; }
	}
}
