using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000500 RID: 1280
	public class SM2KeyExchangePublicParameters : ICipherParameters
	{
		// Token: 0x060030C7 RID: 12487 RVA: 0x001281D4 File Offset: 0x001263D4
		public SM2KeyExchangePublicParameters(ECPublicKeyParameters staticPublicKey, ECPublicKeyParameters ephemeralPublicKey)
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
			this.mStaticPublicKey = staticPublicKey;
			this.mEphemeralPublicKey = ephemeralPublicKey;
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x060030C8 RID: 12488 RVA: 0x0012822F File Offset: 0x0012642F
		public virtual ECPublicKeyParameters StaticPublicKey
		{
			get
			{
				return this.mStaticPublicKey;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x060030C9 RID: 12489 RVA: 0x00128237 File Offset: 0x00126437
		public virtual ECPublicKeyParameters EphemeralPublicKey
		{
			get
			{
				return this.mEphemeralPublicKey;
			}
		}

		// Token: 0x0400203E RID: 8254
		private readonly ECPublicKeyParameters mStaticPublicKey;

		// Token: 0x0400203F RID: 8255
		private readonly ECPublicKeyParameters mEphemeralPublicKey;
	}
}
