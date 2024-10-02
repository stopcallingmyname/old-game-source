using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003E9 RID: 1001
	public interface IXof : IDigest
	{
		// Token: 0x0600287D RID: 10365
		int DoFinal(byte[] output, int outOff, int outLen);

		// Token: 0x0600287E RID: 10366
		int DoOutput(byte[] output, int outOff, int outLen);
	}
}
