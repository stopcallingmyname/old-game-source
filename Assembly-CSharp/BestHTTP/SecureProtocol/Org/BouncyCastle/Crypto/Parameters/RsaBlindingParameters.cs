using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004FA RID: 1274
	public class RsaBlindingParameters : ICipherParameters
	{
		// Token: 0x0600309F RID: 12447 RVA: 0x00127BBC File Offset: 0x00125DBC
		public RsaBlindingParameters(RsaKeyParameters publicKey, BigInteger blindingFactor)
		{
			if (publicKey.IsPrivate)
			{
				throw new ArgumentException("RSA parameters should be for a public key");
			}
			this.publicKey = publicKey;
			this.blindingFactor = blindingFactor;
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060030A0 RID: 12448 RVA: 0x00127BE5 File Offset: 0x00125DE5
		public RsaKeyParameters PublicKey
		{
			get
			{
				return this.publicKey;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060030A1 RID: 12449 RVA: 0x00127BED File Offset: 0x00125DED
		public BigInteger BlindingFactor
		{
			get
			{
				return this.blindingFactor;
			}
		}

		// Token: 0x04002023 RID: 8227
		private readonly RsaKeyParameters publicKey;

		// Token: 0x04002024 RID: 8228
		private readonly BigInteger blindingFactor;
	}
}
