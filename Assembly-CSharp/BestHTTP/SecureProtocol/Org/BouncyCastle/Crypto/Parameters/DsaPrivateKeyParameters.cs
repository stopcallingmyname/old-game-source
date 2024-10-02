using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004CE RID: 1230
	public class DsaPrivateKeyParameters : DsaKeyParameters
	{
		// Token: 0x06002FBE RID: 12222 RVA: 0x001261E6 File Offset: 0x001243E6
		public DsaPrivateKeyParameters(BigInteger x, DsaParameters parameters) : base(true, parameters)
		{
			if (x == null)
			{
				throw new ArgumentNullException("x");
			}
			this.x = x;
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06002FBF RID: 12223 RVA: 0x00126205 File Offset: 0x00124405
		public BigInteger X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x00126210 File Offset: 0x00124410
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DsaPrivateKeyParameters dsaPrivateKeyParameters = obj as DsaPrivateKeyParameters;
			return dsaPrivateKeyParameters != null && this.Equals(dsaPrivateKeyParameters);
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x00126236 File Offset: 0x00124436
		protected bool Equals(DsaPrivateKeyParameters other)
		{
			return this.x.Equals(other.x) && base.Equals(other);
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x00126254 File Offset: 0x00124454
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001FCC RID: 8140
		private readonly BigInteger x;
	}
}
