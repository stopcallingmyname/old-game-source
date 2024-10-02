using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004C6 RID: 1222
	public class DHParameters : ICipherParameters
	{
		// Token: 0x06002F83 RID: 12163 RVA: 0x00125AE7 File Offset: 0x00123CE7
		private static int GetDefaultMParam(int lParam)
		{
			if (lParam == 0)
			{
				return 160;
			}
			return Math.Min(lParam, 160);
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x00125AFD File Offset: 0x00123CFD
		public DHParameters(BigInteger p, BigInteger g) : this(p, g, null, 0)
		{
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x00125B09 File Offset: 0x00123D09
		public DHParameters(BigInteger p, BigInteger g, BigInteger q) : this(p, g, q, 0)
		{
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x00125B15 File Offset: 0x00123D15
		public DHParameters(BigInteger p, BigInteger g, BigInteger q, int l) : this(p, g, q, DHParameters.GetDefaultMParam(l), l, null, null)
		{
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x00125B2B File Offset: 0x00123D2B
		public DHParameters(BigInteger p, BigInteger g, BigInteger q, int m, int l) : this(p, g, q, m, l, null, null)
		{
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x00125B3C File Offset: 0x00123D3C
		public DHParameters(BigInteger p, BigInteger g, BigInteger q, BigInteger j, DHValidationParameters validation) : this(p, g, q, 160, 0, j, validation)
		{
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x00125B54 File Offset: 0x00123D54
		public DHParameters(BigInteger p, BigInteger g, BigInteger q, int m, int l, BigInteger j, DHValidationParameters validation)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			if (!p.TestBit(0))
			{
				throw new ArgumentException("field must be an odd prime", "p");
			}
			if (g.CompareTo(BigInteger.Two) < 0 || g.CompareTo(p.Subtract(BigInteger.Two)) > 0)
			{
				throw new ArgumentException("generator must in the range [2, p - 2]", "g");
			}
			if (q != null && q.BitLength >= p.BitLength)
			{
				throw new ArgumentException("q too big to be a factor of (p-1)", "q");
			}
			if (m >= p.BitLength)
			{
				throw new ArgumentException("m value must be < bitlength of p", "m");
			}
			if (l != 0)
			{
				if (l >= p.BitLength)
				{
					throw new ArgumentException("when l value specified, it must be less than bitlength(p)", "l");
				}
				if (l < m)
				{
					throw new ArgumentException("when l value specified, it may not be less than m value", "l");
				}
			}
			if (j != null && j.CompareTo(BigInteger.Two) < 0)
			{
				throw new ArgumentException("subgroup factor must be >= 2", "j");
			}
			this.p = p;
			this.g = g;
			this.q = q;
			this.m = m;
			this.l = l;
			this.j = j;
			this.validation = validation;
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06002F8A RID: 12170 RVA: 0x00125C95 File Offset: 0x00123E95
		public BigInteger P
		{
			get
			{
				return this.p;
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06002F8B RID: 12171 RVA: 0x00125C9D File Offset: 0x00123E9D
		public BigInteger G
		{
			get
			{
				return this.g;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06002F8C RID: 12172 RVA: 0x00125CA5 File Offset: 0x00123EA5
		public BigInteger Q
		{
			get
			{
				return this.q;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06002F8D RID: 12173 RVA: 0x00125CAD File Offset: 0x00123EAD
		public BigInteger J
		{
			get
			{
				return this.j;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06002F8E RID: 12174 RVA: 0x00125CB5 File Offset: 0x00123EB5
		public int M
		{
			get
			{
				return this.m;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06002F8F RID: 12175 RVA: 0x00125CBD File Offset: 0x00123EBD
		public int L
		{
			get
			{
				return this.l;
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06002F90 RID: 12176 RVA: 0x00125CC5 File Offset: 0x00123EC5
		public DHValidationParameters ValidationParameters
		{
			get
			{
				return this.validation;
			}
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x00125CD0 File Offset: 0x00123ED0
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DHParameters dhparameters = obj as DHParameters;
			return dhparameters != null && this.Equals(dhparameters);
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x00125CF6 File Offset: 0x00123EF6
		protected virtual bool Equals(DHParameters other)
		{
			return this.p.Equals(other.p) && this.g.Equals(other.g) && object.Equals(this.q, other.q);
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x00125D34 File Offset: 0x00123F34
		public override int GetHashCode()
		{
			int num = this.p.GetHashCode() ^ this.g.GetHashCode();
			if (this.q != null)
			{
				num ^= this.q.GetHashCode();
			}
			return num;
		}

		// Token: 0x04001FB3 RID: 8115
		private const int DefaultMinimumLength = 160;

		// Token: 0x04001FB4 RID: 8116
		private readonly BigInteger p;

		// Token: 0x04001FB5 RID: 8117
		private readonly BigInteger g;

		// Token: 0x04001FB6 RID: 8118
		private readonly BigInteger q;

		// Token: 0x04001FB7 RID: 8119
		private readonly BigInteger j;

		// Token: 0x04001FB8 RID: 8120
		private readonly int m;

		// Token: 0x04001FB9 RID: 8121
		private readonly int l;

		// Token: 0x04001FBA RID: 8122
		private readonly DHValidationParameters validation;
	}
}
