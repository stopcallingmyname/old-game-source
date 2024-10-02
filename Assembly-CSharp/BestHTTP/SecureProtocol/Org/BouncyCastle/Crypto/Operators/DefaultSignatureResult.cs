using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators
{
	// Token: 0x02000516 RID: 1302
	public class DefaultSignatureResult : IBlockResult
	{
		// Token: 0x0600312D RID: 12589 RVA: 0x00129497 File Offset: 0x00127697
		public DefaultSignatureResult(ISigner signer)
		{
			this.mSigner = signer;
		}

		// Token: 0x0600312E RID: 12590 RVA: 0x001294A6 File Offset: 0x001276A6
		public byte[] Collect()
		{
			return this.mSigner.GenerateSignature();
		}

		// Token: 0x0600312F RID: 12591 RVA: 0x001294B3 File Offset: 0x001276B3
		public int Collect(byte[] sig, int sigOff)
		{
			byte[] array = this.Collect();
			array.CopyTo(sig, sigOff);
			return array.Length;
		}

		// Token: 0x0400205D RID: 8285
		private readonly ISigner mSigner;
	}
}
