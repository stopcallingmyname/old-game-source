using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004DF RID: 1247
	public class ElGamalPrivateKeyParameters : ElGamalKeyParameters
	{
		// Token: 0x0600302A RID: 12330 RVA: 0x00127072 File Offset: 0x00125272
		public ElGamalPrivateKeyParameters(BigInteger x, ElGamalParameters parameters) : base(true, parameters)
		{
			if (x == null)
			{
				throw new ArgumentNullException("x");
			}
			this.x = x;
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x0600302B RID: 12331 RVA: 0x00127091 File Offset: 0x00125291
		public BigInteger X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x0012709C File Offset: 0x0012529C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ElGamalPrivateKeyParameters elGamalPrivateKeyParameters = obj as ElGamalPrivateKeyParameters;
			return elGamalPrivateKeyParameters != null && this.Equals(elGamalPrivateKeyParameters);
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x001270C2 File Offset: 0x001252C2
		protected bool Equals(ElGamalPrivateKeyParameters other)
		{
			return other.x.Equals(this.x) && base.Equals(other);
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x001270E0 File Offset: 0x001252E0
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001FEE RID: 8174
		private readonly BigInteger x;
	}
}
