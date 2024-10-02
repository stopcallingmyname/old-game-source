using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004F8 RID: 1272
	public class RC2Parameters : KeyParameter
	{
		// Token: 0x06003098 RID: 12440 RVA: 0x00127B28 File Offset: 0x00125D28
		public RC2Parameters(byte[] key) : this(key, (key.Length > 128) ? 1024 : (key.Length * 8))
		{
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x00127B47 File Offset: 0x00125D47
		public RC2Parameters(byte[] key, int keyOff, int keyLen) : this(key, keyOff, keyLen, (keyLen > 128) ? 1024 : (keyLen * 8))
		{
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x00127B64 File Offset: 0x00125D64
		public RC2Parameters(byte[] key, int bits) : base(key)
		{
			this.bits = bits;
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x00127B74 File Offset: 0x00125D74
		public RC2Parameters(byte[] key, int keyOff, int keyLen, int bits) : base(key, keyOff, keyLen)
		{
			this.bits = bits;
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x0600309C RID: 12444 RVA: 0x00127B87 File Offset: 0x00125D87
		public int EffectiveKeyBits
		{
			get
			{
				return this.bits;
			}
		}

		// Token: 0x04002021 RID: 8225
		private readonly int bits;
	}
}
