using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003E4 RID: 996
	public interface IStreamCipher
	{
		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x0600286F RID: 10351
		string AlgorithmName { get; }

		// Token: 0x06002870 RID: 10352
		void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x06002871 RID: 10353
		byte ReturnByte(byte input);

		// Token: 0x06002872 RID: 10354
		void ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff);

		// Token: 0x06002873 RID: 10355
		void Reset();
	}
}
