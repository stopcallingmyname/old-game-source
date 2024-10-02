using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004E4 RID: 1252
	public class Gost3410PrivateKeyParameters : Gost3410KeyParameters
	{
		// Token: 0x06003047 RID: 12359 RVA: 0x0012739C File Offset: 0x0012559C
		public Gost3410PrivateKeyParameters(BigInteger x, Gost3410Parameters parameters) : base(true, parameters)
		{
			if (x.SignValue < 1 || x.BitLength > 256 || x.CompareTo(base.Parameters.Q) >= 0)
			{
				throw new ArgumentException("Invalid x for GOST3410 private key", "x");
			}
			this.x = x;
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x001273F4 File Offset: 0x001255F4
		public Gost3410PrivateKeyParameters(BigInteger x, DerObjectIdentifier publicKeyParamSet) : base(true, publicKeyParamSet)
		{
			if (x.SignValue < 1 || x.BitLength > 256 || x.CompareTo(base.Parameters.Q) >= 0)
			{
				throw new ArgumentException("Invalid x for GOST3410 private key", "x");
			}
			this.x = x;
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06003049 RID: 12361 RVA: 0x0012744A File Offset: 0x0012564A
		public BigInteger X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x04001FF8 RID: 8184
		private readonly BigInteger x;
	}
}
