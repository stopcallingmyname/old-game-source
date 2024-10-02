using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004CD RID: 1229
	public class DsaParameters : ICipherParameters
	{
		// Token: 0x06002FB5 RID: 12213 RVA: 0x001260D6 File Offset: 0x001242D6
		public DsaParameters(BigInteger p, BigInteger q, BigInteger g) : this(p, q, g, null)
		{
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x001260E4 File Offset: 0x001242E4
		public DsaParameters(BigInteger p, BigInteger q, BigInteger g, DsaValidationParameters parameters)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			if (q == null)
			{
				throw new ArgumentNullException("q");
			}
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			this.p = p;
			this.q = q;
			this.g = g;
			this.validation = parameters;
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06002FB7 RID: 12215 RVA: 0x0012613E File Offset: 0x0012433E
		public BigInteger P
		{
			get
			{
				return this.p;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06002FB8 RID: 12216 RVA: 0x00126146 File Offset: 0x00124346
		public BigInteger Q
		{
			get
			{
				return this.q;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06002FB9 RID: 12217 RVA: 0x0012614E File Offset: 0x0012434E
		public BigInteger G
		{
			get
			{
				return this.g;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06002FBA RID: 12218 RVA: 0x00126156 File Offset: 0x00124356
		public DsaValidationParameters ValidationParameters
		{
			get
			{
				return this.validation;
			}
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x00126160 File Offset: 0x00124360
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DsaParameters dsaParameters = obj as DsaParameters;
			return dsaParameters != null && this.Equals(dsaParameters);
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x00126186 File Offset: 0x00124386
		protected bool Equals(DsaParameters other)
		{
			return this.p.Equals(other.p) && this.q.Equals(other.q) && this.g.Equals(other.g);
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x001261C1 File Offset: 0x001243C1
		public override int GetHashCode()
		{
			return this.p.GetHashCode() ^ this.q.GetHashCode() ^ this.g.GetHashCode();
		}

		// Token: 0x04001FC8 RID: 8136
		private readonly BigInteger p;

		// Token: 0x04001FC9 RID: 8137
		private readonly BigInteger q;

		// Token: 0x04001FCA RID: 8138
		private readonly BigInteger g;

		// Token: 0x04001FCB RID: 8139
		private readonly DsaValidationParameters validation;
	}
}
