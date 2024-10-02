using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003E5 RID: 997
	public interface IVerifier
	{
		// Token: 0x06002874 RID: 10356
		bool IsVerified(byte[] data);

		// Token: 0x06002875 RID: 10357
		bool IsVerified(byte[] source, int off, int length);
	}
}
