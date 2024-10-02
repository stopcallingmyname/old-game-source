using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004EA RID: 1258
	public class Iso18033KdfParameters : IDerivationParameters
	{
		// Token: 0x06003063 RID: 12387 RVA: 0x001276B7 File Offset: 0x001258B7
		public Iso18033KdfParameters(byte[] seed)
		{
			this.seed = seed;
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x001276C6 File Offset: 0x001258C6
		public byte[] GetSeed()
		{
			return this.seed;
		}

		// Token: 0x04002006 RID: 8198
		private byte[] seed;
	}
}
