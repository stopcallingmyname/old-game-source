using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003D5 RID: 981
	public interface IDerivationFunction
	{
		// Token: 0x0600283B RID: 10299
		void Init(IDerivationParameters parameters);

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x0600283C RID: 10300
		IDigest Digest { get; }

		// Token: 0x0600283D RID: 10301
		int GenerateBytes(byte[] output, int outOff, int length);
	}
}
