using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004E9 RID: 1257
	public class IesWithCipherParameters : IesParameters
	{
		// Token: 0x06003061 RID: 12385 RVA: 0x0012769C File Offset: 0x0012589C
		public IesWithCipherParameters(byte[] derivation, byte[] encoding, int macKeySize, int cipherKeySize) : base(derivation, encoding, macKeySize)
		{
			this.cipherKeySize = cipherKeySize;
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06003062 RID: 12386 RVA: 0x001276AF File Offset: 0x001258AF
		public int CipherKeySize
		{
			get
			{
				return this.cipherKeySize;
			}
		}

		// Token: 0x04002005 RID: 8197
		private int cipherKeySize;
	}
}
