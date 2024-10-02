using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003E8 RID: 1000
	public interface IWrapper
	{
		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06002879 RID: 10361
		string AlgorithmName { get; }

		// Token: 0x0600287A RID: 10362
		void Init(bool forWrapping, ICipherParameters parameters);

		// Token: 0x0600287B RID: 10363
		byte[] Wrap(byte[] input, int inOff, int length);

		// Token: 0x0600287C RID: 10364
		byte[] Unwrap(byte[] input, int inOff, int length);
	}
}
