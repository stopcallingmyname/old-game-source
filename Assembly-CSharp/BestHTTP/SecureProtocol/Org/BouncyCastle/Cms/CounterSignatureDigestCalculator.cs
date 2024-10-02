using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000606 RID: 1542
	internal class CounterSignatureDigestCalculator : IDigestCalculator
	{
		// Token: 0x06003AA5 RID: 15013 RVA: 0x0016B9C2 File Offset: 0x00169BC2
		internal CounterSignatureDigestCalculator(string alg, byte[] data)
		{
			this.alg = alg;
			this.data = data;
		}

		// Token: 0x06003AA6 RID: 15014 RVA: 0x0016B9D8 File Offset: 0x00169BD8
		public byte[] GetDigest()
		{
			return DigestUtilities.DoFinal(CmsSignedHelper.Instance.GetDigestInstance(this.alg), this.data);
		}

		// Token: 0x04002654 RID: 9812
		private readonly string alg;

		// Token: 0x04002655 RID: 9813
		private readonly byte[] data;
	}
}
