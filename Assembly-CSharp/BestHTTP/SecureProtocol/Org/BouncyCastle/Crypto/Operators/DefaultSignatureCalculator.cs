using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators
{
	// Token: 0x02000515 RID: 1301
	public class DefaultSignatureCalculator : IStreamCalculator
	{
		// Token: 0x0600312A RID: 12586 RVA: 0x00129469 File Offset: 0x00127669
		public DefaultSignatureCalculator(ISigner signer)
		{
			this.mSignerSink = new SignerSink(signer);
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x0600312B RID: 12587 RVA: 0x0012947D File Offset: 0x0012767D
		public Stream Stream
		{
			get
			{
				return this.mSignerSink;
			}
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x00129485 File Offset: 0x00127685
		public object GetResult()
		{
			return new DefaultSignatureResult(this.mSignerSink.Signer);
		}

		// Token: 0x0400205C RID: 8284
		private readonly SignerSink mSignerSink;
	}
}
