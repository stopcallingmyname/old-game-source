using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004FF RID: 1279
	public class SM2KeyExchangePrivateParameters : ICipherParameters
	{
		// Token: 0x060030C1 RID: 12481 RVA: 0x00128110 File Offset: 0x00126310
		public SM2KeyExchangePrivateParameters(bool initiator, ECPrivateKeyParameters staticPrivateKey, ECPrivateKeyParameters ephemeralPrivateKey)
		{
			if (staticPrivateKey == null)
			{
				throw new ArgumentNullException("staticPrivateKey");
			}
			if (ephemeralPrivateKey == null)
			{
				throw new ArgumentNullException("ephemeralPrivateKey");
			}
			ECDomainParameters parameters = staticPrivateKey.Parameters;
			if (!parameters.Equals(ephemeralPrivateKey.Parameters))
			{
				throw new ArgumentException("Static and ephemeral private keys have different domain parameters");
			}
			this.mInitiator = initiator;
			this.mStaticPrivateKey = staticPrivateKey;
			this.mStaticPublicPoint = parameters.G.Multiply(staticPrivateKey.D).Normalize();
			this.mEphemeralPrivateKey = ephemeralPrivateKey;
			this.mEphemeralPublicPoint = parameters.G.Multiply(ephemeralPrivateKey.D).Normalize();
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x060030C2 RID: 12482 RVA: 0x001281AC File Offset: 0x001263AC
		public virtual bool IsInitiator
		{
			get
			{
				return this.mInitiator;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x060030C3 RID: 12483 RVA: 0x001281B4 File Offset: 0x001263B4
		public virtual ECPrivateKeyParameters StaticPrivateKey
		{
			get
			{
				return this.mStaticPrivateKey;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x060030C4 RID: 12484 RVA: 0x001281BC File Offset: 0x001263BC
		public virtual ECPoint StaticPublicPoint
		{
			get
			{
				return this.mStaticPublicPoint;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x060030C5 RID: 12485 RVA: 0x001281C4 File Offset: 0x001263C4
		public virtual ECPrivateKeyParameters EphemeralPrivateKey
		{
			get
			{
				return this.mEphemeralPrivateKey;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x060030C6 RID: 12486 RVA: 0x001281CC File Offset: 0x001263CC
		public virtual ECPoint EphemeralPublicPoint
		{
			get
			{
				return this.mEphemeralPublicPoint;
			}
		}

		// Token: 0x04002039 RID: 8249
		private readonly bool mInitiator;

		// Token: 0x0400203A RID: 8250
		private readonly ECPrivateKeyParameters mStaticPrivateKey;

		// Token: 0x0400203B RID: 8251
		private readonly ECPoint mStaticPublicPoint;

		// Token: 0x0400203C RID: 8252
		private readonly ECPrivateKeyParameters mEphemeralPrivateKey;

		// Token: 0x0400203D RID: 8253
		private readonly ECPoint mEphemeralPublicPoint;
	}
}
