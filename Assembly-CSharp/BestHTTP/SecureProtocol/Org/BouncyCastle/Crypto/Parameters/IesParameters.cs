using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004E8 RID: 1256
	public class IesParameters : ICipherParameters
	{
		// Token: 0x0600305D RID: 12381 RVA: 0x00127667 File Offset: 0x00125867
		public IesParameters(byte[] derivation, byte[] encoding, int macKeySize)
		{
			this.derivation = derivation;
			this.encoding = encoding;
			this.macKeySize = macKeySize;
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x00127684 File Offset: 0x00125884
		public byte[] GetDerivationV()
		{
			return this.derivation;
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x0012768C File Offset: 0x0012588C
		public byte[] GetEncodingV()
		{
			return this.encoding;
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06003060 RID: 12384 RVA: 0x00127694 File Offset: 0x00125894
		public int MacKeySize
		{
			get
			{
				return this.macKeySize;
			}
		}

		// Token: 0x04002002 RID: 8194
		private byte[] derivation;

		// Token: 0x04002003 RID: 8195
		private byte[] encoding;

		// Token: 0x04002004 RID: 8196
		private int macKeySize;
	}
}
