using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004D5 RID: 1237
	public class ECPublicKeyParameters : ECKeyParameters
	{
		// Token: 0x06002FF6 RID: 12278 RVA: 0x00126964 File Offset: 0x00124B64
		public ECPublicKeyParameters(ECPoint q, ECDomainParameters parameters) : this("EC", q, parameters)
		{
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x00126973 File Offset: 0x00124B73
		[Obsolete("Use version with explicit 'algorithm' parameter")]
		public ECPublicKeyParameters(ECPoint q, DerObjectIdentifier publicKeyParamSet) : base("ECGOST3410", false, publicKeyParamSet)
		{
			if (q == null)
			{
				throw new ArgumentNullException("q");
			}
			this.q = ECDomainParameters.Validate(base.Parameters.Curve, q);
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x001269A7 File Offset: 0x00124BA7
		public ECPublicKeyParameters(string algorithm, ECPoint q, ECDomainParameters parameters) : base(algorithm, false, parameters)
		{
			if (q == null)
			{
				throw new ArgumentNullException("q");
			}
			this.q = ECDomainParameters.Validate(base.Parameters.Curve, q);
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x001269D7 File Offset: 0x00124BD7
		public ECPublicKeyParameters(string algorithm, ECPoint q, DerObjectIdentifier publicKeyParamSet) : base(algorithm, false, publicKeyParamSet)
		{
			if (q == null)
			{
				throw new ArgumentNullException("q");
			}
			this.q = ECDomainParameters.Validate(base.Parameters.Curve, q);
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06002FFA RID: 12282 RVA: 0x00126A07 File Offset: 0x00124C07
		public ECPoint Q
		{
			get
			{
				return this.q;
			}
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x00126A10 File Offset: 0x00124C10
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ECPublicKeyParameters ecpublicKeyParameters = obj as ECPublicKeyParameters;
			return ecpublicKeyParameters != null && this.Equals(ecpublicKeyParameters);
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x00126A36 File Offset: 0x00124C36
		protected bool Equals(ECPublicKeyParameters other)
		{
			return this.q.Equals(other.q) && base.Equals(other);
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x00126A54 File Offset: 0x00124C54
		public override int GetHashCode()
		{
			return this.q.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001FDE RID: 8158
		private readonly ECPoint q;
	}
}
