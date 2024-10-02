using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004E5 RID: 1253
	public class Gost3410PublicKeyParameters : Gost3410KeyParameters
	{
		// Token: 0x0600304A RID: 12362 RVA: 0x00127452 File Offset: 0x00125652
		public Gost3410PublicKeyParameters(BigInteger y, Gost3410Parameters parameters) : base(false, parameters)
		{
			if (y.SignValue < 1 || y.CompareTo(base.Parameters.P) >= 0)
			{
				throw new ArgumentException("Invalid y for GOST3410 public key", "y");
			}
			this.y = y;
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x00127490 File Offset: 0x00125690
		public Gost3410PublicKeyParameters(BigInteger y, DerObjectIdentifier publicKeyParamSet) : base(false, publicKeyParamSet)
		{
			if (y.SignValue < 1 || y.CompareTo(base.Parameters.P) >= 0)
			{
				throw new ArgumentException("Invalid y for GOST3410 public key", "y");
			}
			this.y = y;
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x0600304C RID: 12364 RVA: 0x001274CE File Offset: 0x001256CE
		public BigInteger Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x04001FF9 RID: 8185
		private readonly BigInteger y;
	}
}
