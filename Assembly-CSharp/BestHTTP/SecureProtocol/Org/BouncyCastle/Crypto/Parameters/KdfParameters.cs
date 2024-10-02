using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004EB RID: 1259
	public class KdfParameters : IDerivationParameters
	{
		// Token: 0x06003065 RID: 12389 RVA: 0x001276CE File Offset: 0x001258CE
		public KdfParameters(byte[] shared, byte[] iv)
		{
			this.shared = shared;
			this.iv = iv;
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x001276E4 File Offset: 0x001258E4
		public byte[] GetSharedSecret()
		{
			return this.shared;
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x001276EC File Offset: 0x001258EC
		public byte[] GetIV()
		{
			return this.iv;
		}

		// Token: 0x04002007 RID: 8199
		private byte[] iv;

		// Token: 0x04002008 RID: 8200
		private byte[] shared;
	}
}
