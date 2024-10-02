using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003E2 RID: 994
	public interface ISignerWithRecovery : ISigner
	{
		// Token: 0x0600286A RID: 10346
		bool HasFullMessage();

		// Token: 0x0600286B RID: 10347
		byte[] GetRecoveredMessage();

		// Token: 0x0600286C RID: 10348
		void UpdateWithRecoveredMessage(byte[] signature);
	}
}
