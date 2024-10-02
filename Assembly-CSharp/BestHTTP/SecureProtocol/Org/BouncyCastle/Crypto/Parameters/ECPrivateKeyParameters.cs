using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004D4 RID: 1236
	public class ECPrivateKeyParameters : ECKeyParameters
	{
		// Token: 0x06002FEE RID: 12270 RVA: 0x00126891 File Offset: 0x00124A91
		public ECPrivateKeyParameters(BigInteger d, ECDomainParameters parameters) : this("EC", d, parameters)
		{
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x001268A0 File Offset: 0x00124AA0
		[Obsolete("Use version with explicit 'algorithm' parameter")]
		public ECPrivateKeyParameters(BigInteger d, DerObjectIdentifier publicKeyParamSet) : base("ECGOST3410", true, publicKeyParamSet)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			this.d = d;
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x001268C4 File Offset: 0x00124AC4
		public ECPrivateKeyParameters(string algorithm, BigInteger d, ECDomainParameters parameters) : base(algorithm, true, parameters)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			this.d = d;
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x001268E4 File Offset: 0x00124AE4
		public ECPrivateKeyParameters(string algorithm, BigInteger d, DerObjectIdentifier publicKeyParamSet) : base(algorithm, true, publicKeyParamSet)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			this.d = d;
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06002FF2 RID: 12274 RVA: 0x00126904 File Offset: 0x00124B04
		public BigInteger D
		{
			get
			{
				return this.d;
			}
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x0012690C File Offset: 0x00124B0C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ECPrivateKeyParameters ecprivateKeyParameters = obj as ECPrivateKeyParameters;
			return ecprivateKeyParameters != null && this.Equals(ecprivateKeyParameters);
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x00126932 File Offset: 0x00124B32
		protected bool Equals(ECPrivateKeyParameters other)
		{
			return this.d.Equals(other.d) && base.Equals(other);
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x00126950 File Offset: 0x00124B50
		public override int GetHashCode()
		{
			return this.d.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001FDD RID: 8157
		private readonly BigInteger d;
	}
}
