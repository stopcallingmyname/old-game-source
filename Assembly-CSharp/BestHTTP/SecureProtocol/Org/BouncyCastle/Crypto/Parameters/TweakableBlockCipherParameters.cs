using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000502 RID: 1282
	public class TweakableBlockCipherParameters : ICipherParameters
	{
		// Token: 0x060030CD RID: 12493 RVA: 0x00128265 File Offset: 0x00126465
		public TweakableBlockCipherParameters(KeyParameter key, byte[] tweak)
		{
			this.key = key;
			this.tweak = Arrays.Clone(tweak);
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x060030CE RID: 12494 RVA: 0x00128280 File Offset: 0x00126480
		public KeyParameter Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x060030CF RID: 12495 RVA: 0x00128288 File Offset: 0x00126488
		public byte[] Tweak
		{
			get
			{
				return this.tweak;
			}
		}

		// Token: 0x04002042 RID: 8258
		private readonly byte[] tweak;

		// Token: 0x04002043 RID: 8259
		private readonly KeyParameter key;
	}
}
