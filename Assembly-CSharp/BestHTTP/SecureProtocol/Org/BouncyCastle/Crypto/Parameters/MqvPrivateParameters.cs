using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004EE RID: 1262
	public class MqvPrivateParameters : ICipherParameters
	{
		// Token: 0x0600306E RID: 12398 RVA: 0x001277D8 File Offset: 0x001259D8
		public MqvPrivateParameters(ECPrivateKeyParameters staticPrivateKey, ECPrivateKeyParameters ephemeralPrivateKey) : this(staticPrivateKey, ephemeralPrivateKey, null)
		{
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x001277E4 File Offset: 0x001259E4
		public MqvPrivateParameters(ECPrivateKeyParameters staticPrivateKey, ECPrivateKeyParameters ephemeralPrivateKey, ECPublicKeyParameters ephemeralPublicKey)
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
			if (ephemeralPublicKey == null)
			{
				ephemeralPublicKey = new ECPublicKeyParameters(parameters.G.Multiply(ephemeralPrivateKey.D), parameters);
			}
			else if (!parameters.Equals(ephemeralPublicKey.Parameters))
			{
				throw new ArgumentException("Ephemeral public key has different domain parameters");
			}
			this.staticPrivateKey = staticPrivateKey;
			this.ephemeralPrivateKey = ephemeralPrivateKey;
			this.ephemeralPublicKey = ephemeralPublicKey;
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06003070 RID: 12400 RVA: 0x0012787F File Offset: 0x00125A7F
		public virtual ECPrivateKeyParameters StaticPrivateKey
		{
			get
			{
				return this.staticPrivateKey;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06003071 RID: 12401 RVA: 0x00127887 File Offset: 0x00125A87
		public virtual ECPrivateKeyParameters EphemeralPrivateKey
		{
			get
			{
				return this.ephemeralPrivateKey;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06003072 RID: 12402 RVA: 0x0012788F File Offset: 0x00125A8F
		public virtual ECPublicKeyParameters EphemeralPublicKey
		{
			get
			{
				return this.ephemeralPublicKey;
			}
		}

		// Token: 0x0400200B RID: 8203
		private readonly ECPrivateKeyParameters staticPrivateKey;

		// Token: 0x0400200C RID: 8204
		private readonly ECPrivateKeyParameters ephemeralPrivateKey;

		// Token: 0x0400200D RID: 8205
		private readonly ECPublicKeyParameters ephemeralPublicKey;
	}
}
