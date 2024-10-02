using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators
{
	// Token: 0x02000517 RID: 1303
	public class DefaultVerifierCalculator : IStreamCalculator
	{
		// Token: 0x06003130 RID: 12592 RVA: 0x001294C5 File Offset: 0x001276C5
		public DefaultVerifierCalculator(ISigner signer)
		{
			this.mSignerSink = new SignerSink(signer);
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06003131 RID: 12593 RVA: 0x001294D9 File Offset: 0x001276D9
		public Stream Stream
		{
			get
			{
				return this.mSignerSink;
			}
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x001294E1 File Offset: 0x001276E1
		public object GetResult()
		{
			return new DefaultVerifierResult(this.mSignerSink.Signer);
		}

		// Token: 0x0400205E RID: 8286
		private readonly SignerSink mSignerSink;
	}
}
