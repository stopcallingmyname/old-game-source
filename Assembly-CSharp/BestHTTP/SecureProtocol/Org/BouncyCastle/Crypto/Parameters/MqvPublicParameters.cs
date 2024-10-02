using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004EF RID: 1263
	public class MqvPublicParameters : ICipherParameters
	{
		// Token: 0x06003073 RID: 12403 RVA: 0x00127898 File Offset: 0x00125A98
		public MqvPublicParameters(ECPublicKeyParameters staticPublicKey, ECPublicKeyParameters ephemeralPublicKey)
		{
			if (staticPublicKey == null)
			{
				throw new ArgumentNullException("staticPublicKey");
			}
			if (ephemeralPublicKey == null)
			{
				throw new ArgumentNullException("ephemeralPublicKey");
			}
			if (!staticPublicKey.Parameters.Equals(ephemeralPublicKey.Parameters))
			{
				throw new ArgumentException("Static and ephemeral public keys have different domain parameters");
			}
			this.staticPublicKey = staticPublicKey;
			this.ephemeralPublicKey = ephemeralPublicKey;
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06003074 RID: 12404 RVA: 0x001278F3 File Offset: 0x00125AF3
		public virtual ECPublicKeyParameters StaticPublicKey
		{
			get
			{
				return this.staticPublicKey;
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06003075 RID: 12405 RVA: 0x001278FB File Offset: 0x00125AFB
		public virtual ECPublicKeyParameters EphemeralPublicKey
		{
			get
			{
				return this.ephemeralPublicKey;
			}
		}

		// Token: 0x0400200E RID: 8206
		private readonly ECPublicKeyParameters staticPublicKey;

		// Token: 0x0400200F RID: 8207
		private readonly ECPublicKeyParameters ephemeralPublicKey;
	}
}
