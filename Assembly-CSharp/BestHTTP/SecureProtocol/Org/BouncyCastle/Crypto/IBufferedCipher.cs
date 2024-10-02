using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003D3 RID: 979
	public interface IBufferedCipher
	{
		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06002829 RID: 10281
		string AlgorithmName { get; }

		// Token: 0x0600282A RID: 10282
		void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x0600282B RID: 10283
		int GetBlockSize();

		// Token: 0x0600282C RID: 10284
		int GetOutputSize(int inputLen);

		// Token: 0x0600282D RID: 10285
		int GetUpdateOutputSize(int inputLen);

		// Token: 0x0600282E RID: 10286
		byte[] ProcessByte(byte input);

		// Token: 0x0600282F RID: 10287
		int ProcessByte(byte input, byte[] output, int outOff);

		// Token: 0x06002830 RID: 10288
		byte[] ProcessBytes(byte[] input);

		// Token: 0x06002831 RID: 10289
		byte[] ProcessBytes(byte[] input, int inOff, int length);

		// Token: 0x06002832 RID: 10290
		int ProcessBytes(byte[] input, byte[] output, int outOff);

		// Token: 0x06002833 RID: 10291
		int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff);

		// Token: 0x06002834 RID: 10292
		byte[] DoFinal();

		// Token: 0x06002835 RID: 10293
		byte[] DoFinal(byte[] input);

		// Token: 0x06002836 RID: 10294
		byte[] DoFinal(byte[] input, int inOff, int length);

		// Token: 0x06002837 RID: 10295
		int DoFinal(byte[] output, int outOff);

		// Token: 0x06002838 RID: 10296
		int DoFinal(byte[] input, byte[] output, int outOff);

		// Token: 0x06002839 RID: 10297
		int DoFinal(byte[] input, int inOff, int length, byte[] output, int outOff);

		// Token: 0x0600283A RID: 10298
		void Reset();
	}
}
