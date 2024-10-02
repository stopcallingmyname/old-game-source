using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004CF RID: 1231
	public class DsaPublicKeyParameters : DsaKeyParameters
	{
		// Token: 0x06002FC3 RID: 12227 RVA: 0x00126268 File Offset: 0x00124468
		private static BigInteger Validate(BigInteger y, DsaParameters parameters)
		{
			if (parameters != null && (y.CompareTo(BigInteger.Two) < 0 || y.CompareTo(parameters.P.Subtract(BigInteger.Two)) > 0 || !y.ModPow(parameters.Q, parameters.P).Equals(BigInteger.One)))
			{
				throw new ArgumentException("y value does not appear to be in correct group");
			}
			return y;
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x001262C9 File Offset: 0x001244C9
		public DsaPublicKeyParameters(BigInteger y, DsaParameters parameters) : base(false, parameters)
		{
			if (y == null)
			{
				throw new ArgumentNullException("y");
			}
			this.y = DsaPublicKeyParameters.Validate(y, parameters);
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06002FC5 RID: 12229 RVA: 0x001262EE File Offset: 0x001244EE
		public BigInteger Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x06002FC6 RID: 12230 RVA: 0x001262F8 File Offset: 0x001244F8
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DsaPublicKeyParameters dsaPublicKeyParameters = obj as DsaPublicKeyParameters;
			return dsaPublicKeyParameters != null && this.Equals(dsaPublicKeyParameters);
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x0012631E File Offset: 0x0012451E
		protected bool Equals(DsaPublicKeyParameters other)
		{
			return this.y.Equals(other.y) && base.Equals(other);
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x0012633C File Offset: 0x0012453C
		public override int GetHashCode()
		{
			return this.y.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001FCD RID: 8141
		private readonly BigInteger y;
	}
}
