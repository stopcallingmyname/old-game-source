using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x02000520 RID: 1312
	public interface IAeadBlockCipher
	{
		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x0600319A RID: 12698
		string AlgorithmName { get; }

		// Token: 0x0600319B RID: 12699
		IBlockCipher GetUnderlyingCipher();

		// Token: 0x0600319C RID: 12700
		void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x0600319D RID: 12701
		int GetBlockSize();

		// Token: 0x0600319E RID: 12702
		void ProcessAadByte(byte input);

		// Token: 0x0600319F RID: 12703
		void ProcessAadBytes(byte[] inBytes, int inOff, int len);

		// Token: 0x060031A0 RID: 12704
		int ProcessByte(byte input, byte[] outBytes, int outOff);

		// Token: 0x060031A1 RID: 12705
		int ProcessBytes(byte[] inBytes, int inOff, int len, byte[] outBytes, int outOff);

		// Token: 0x060031A2 RID: 12706
		int DoFinal(byte[] outBytes, int outOff);

		// Token: 0x060031A3 RID: 12707
		byte[] GetMac();

		// Token: 0x060031A4 RID: 12708
		int GetUpdateOutputSize(int len);

		// Token: 0x060031A5 RID: 12709
		int GetOutputSize(int len);

		// Token: 0x060031A6 RID: 12710
		void Reset();
	}
}
