using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003CE RID: 974
	public interface IAsymmetricBlockCipher
	{
		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06002817 RID: 10263
		string AlgorithmName { get; }

		// Token: 0x06002818 RID: 10264
		void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x06002819 RID: 10265
		int GetInputBlockSize();

		// Token: 0x0600281A RID: 10266
		int GetOutputBlockSize();

		// Token: 0x0600281B RID: 10267
		byte[] ProcessBlock(byte[] inBuf, int inOff, int inLen);
	}
}
