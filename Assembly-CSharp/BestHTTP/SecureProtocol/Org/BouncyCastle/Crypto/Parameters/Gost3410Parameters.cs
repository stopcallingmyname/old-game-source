using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004E3 RID: 1251
	public class Gost3410Parameters : ICipherParameters
	{
		// Token: 0x0600303E RID: 12350 RVA: 0x0012728C File Offset: 0x0012548C
		public Gost3410Parameters(BigInteger p, BigInteger q, BigInteger a) : this(p, q, a, null)
		{
		}

		// Token: 0x0600303F RID: 12351 RVA: 0x00127298 File Offset: 0x00125498
		public Gost3410Parameters(BigInteger p, BigInteger q, BigInteger a, Gost3410ValidationParameters validation)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			if (q == null)
			{
				throw new ArgumentNullException("q");
			}
			if (a == null)
			{
				throw new ArgumentNullException("a");
			}
			this.p = p;
			this.q = q;
			this.a = a;
			this.validation = validation;
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06003040 RID: 12352 RVA: 0x001272F2 File Offset: 0x001254F2
		public BigInteger P
		{
			get
			{
				return this.p;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06003041 RID: 12353 RVA: 0x001272FA File Offset: 0x001254FA
		public BigInteger Q
		{
			get
			{
				return this.q;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06003042 RID: 12354 RVA: 0x00127302 File Offset: 0x00125502
		public BigInteger A
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06003043 RID: 12355 RVA: 0x0012730A File Offset: 0x0012550A
		public Gost3410ValidationParameters ValidationParameters
		{
			get
			{
				return this.validation;
			}
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x00127314 File Offset: 0x00125514
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			Gost3410Parameters gost3410Parameters = obj as Gost3410Parameters;
			return gost3410Parameters != null && this.Equals(gost3410Parameters);
		}

		// Token: 0x06003045 RID: 12357 RVA: 0x0012733A File Offset: 0x0012553A
		protected bool Equals(Gost3410Parameters other)
		{
			return this.p.Equals(other.p) && this.q.Equals(other.q) && this.a.Equals(other.a);
		}

		// Token: 0x06003046 RID: 12358 RVA: 0x00127375 File Offset: 0x00125575
		public override int GetHashCode()
		{
			return this.p.GetHashCode() ^ this.q.GetHashCode() ^ this.a.GetHashCode();
		}

		// Token: 0x04001FF4 RID: 8180
		private readonly BigInteger p;

		// Token: 0x04001FF5 RID: 8181
		private readonly BigInteger q;

		// Token: 0x04001FF6 RID: 8182
		private readonly BigInteger a;

		// Token: 0x04001FF7 RID: 8183
		private readonly Gost3410ValidationParameters validation;
	}
}
