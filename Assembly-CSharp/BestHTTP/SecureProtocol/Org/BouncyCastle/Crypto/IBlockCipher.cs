using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003D1 RID: 977
	public interface IBlockCipher
	{
		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06002821 RID: 10273
		string AlgorithmName { get; }

		// Token: 0x06002822 RID: 10274
		void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x06002823 RID: 10275
		int GetBlockSize();

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06002824 RID: 10276
		bool IsPartialBlockOkay { get; }

		// Token: 0x06002825 RID: 10277
		int ProcessBlock(byte[] inBuf, int inOff, byte[] outBuf, int outOff);

		// Token: 0x06002826 RID: 10278
		void Reset();
	}
}
