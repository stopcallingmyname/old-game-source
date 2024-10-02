using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003DF RID: 991
	public interface IRsa
	{
		// Token: 0x0600285B RID: 10331
		void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x0600285C RID: 10332
		int GetInputBlockSize();

		// Token: 0x0600285D RID: 10333
		int GetOutputBlockSize();

		// Token: 0x0600285E RID: 10334
		BigInteger ConvertInput(byte[] buf, int off, int len);

		// Token: 0x0600285F RID: 10335
		BigInteger ProcessBlock(BigInteger input);

		// Token: 0x06002860 RID: 10336
		byte[] ConvertOutput(BigInteger result);
	}
}
