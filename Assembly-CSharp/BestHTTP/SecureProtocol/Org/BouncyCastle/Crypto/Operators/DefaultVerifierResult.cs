using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators
{
	// Token: 0x02000518 RID: 1304
	public class DefaultVerifierResult : IVerifier
	{
		// Token: 0x06003133 RID: 12595 RVA: 0x001294F3 File Offset: 0x001276F3
		public DefaultVerifierResult(ISigner signer)
		{
			this.mSigner = signer;
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x00129502 File Offset: 0x00127702
		public bool IsVerified(byte[] signature)
		{
			return this.mSigner.VerifySignature(signature);
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x00129510 File Offset: 0x00127710
		public bool IsVerified(byte[] sig, int sigOff, int sigLen)
		{
			byte[] signature = Arrays.CopyOfRange(sig, sigOff, sigOff + sigLen);
			return this.IsVerified(signature);
		}

		// Token: 0x0400205F RID: 8287
		private readonly ISigner mSigner;
	}
}
