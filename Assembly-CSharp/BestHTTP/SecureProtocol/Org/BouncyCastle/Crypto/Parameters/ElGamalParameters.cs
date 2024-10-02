using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004DE RID: 1246
	public class ElGamalParameters : ICipherParameters
	{
		// Token: 0x06003022 RID: 12322 RVA: 0x00126F97 File Offset: 0x00125197
		public ElGamalParameters(BigInteger p, BigInteger g) : this(p, g, 0)
		{
		}

		// Token: 0x06003023 RID: 12323 RVA: 0x00126FA2 File Offset: 0x001251A2
		public ElGamalParameters(BigInteger p, BigInteger g, int l)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			this.p = p;
			this.g = g;
			this.l = l;
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06003024 RID: 12324 RVA: 0x00126FDB File Offset: 0x001251DB
		public BigInteger P
		{
			get
			{
				return this.p;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06003025 RID: 12325 RVA: 0x00126FE3 File Offset: 0x001251E3
		public BigInteger G
		{
			get
			{
				return this.g;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06003026 RID: 12326 RVA: 0x00126FEB File Offset: 0x001251EB
		public int L
		{
			get
			{
				return this.l;
			}
		}

		// Token: 0x06003027 RID: 12327 RVA: 0x00126FF4 File Offset: 0x001251F4
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ElGamalParameters elGamalParameters = obj as ElGamalParameters;
			return elGamalParameters != null && this.Equals(elGamalParameters);
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x0012701A File Offset: 0x0012521A
		protected bool Equals(ElGamalParameters other)
		{
			return this.p.Equals(other.p) && this.g.Equals(other.g) && this.l == other.l;
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x00127052 File Offset: 0x00125252
		public override int GetHashCode()
		{
			return this.p.GetHashCode() ^ this.g.GetHashCode() ^ this.l;
		}

		// Token: 0x04001FEB RID: 8171
		private readonly BigInteger p;

		// Token: 0x04001FEC RID: 8172
		private readonly BigInteger g;

		// Token: 0x04001FED RID: 8173
		private readonly int l;
	}
}
