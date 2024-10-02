using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000456 RID: 1110
	public interface TlsCipher
	{
		// Token: 0x06002B71 RID: 11121
		int GetPlaintextLimit(int ciphertextLimit);

		// Token: 0x06002B72 RID: 11122
		byte[] EncodePlaintext(long seqNo, byte type, byte[] plaintext, int offset, int len);

		// Token: 0x06002B73 RID: 11123
		byte[] DecodeCiphertext(long seqNo, byte type, byte[] ciphertext, int offset, int len);
	}
}
