using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004E0 RID: 1248
	public class ElGamalPublicKeyParameters : ElGamalKeyParameters
	{
		// Token: 0x0600302F RID: 12335 RVA: 0x001270F4 File Offset: 0x001252F4
		public ElGamalPublicKeyParameters(BigInteger y, ElGamalParameters parameters) : base(false, parameters)
		{
			if (y == null)
			{
				throw new ArgumentNullException("y");
			}
			this.y = y;
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06003030 RID: 12336 RVA: 0x00127113 File Offset: 0x00125313
		public BigInteger Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x06003031 RID: 12337 RVA: 0x0012711C File Offset: 0x0012531C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ElGamalPublicKeyParameters elGamalPublicKeyParameters = obj as ElGamalPublicKeyParameters;
			return elGamalPublicKeyParameters != null && this.Equals(elGamalPublicKeyParameters);
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x00127142 File Offset: 0x00125342
		protected bool Equals(ElGamalPublicKeyParameters other)
		{
			return this.y.Equals(other.y) && base.Equals(other);
		}

		// Token: 0x06003033 RID: 12339 RVA: 0x00127160 File Offset: 0x00125360
		public override int GetHashCode()
		{
			return this.y.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001FEF RID: 8175
		private readonly BigInteger y;
	}
}
