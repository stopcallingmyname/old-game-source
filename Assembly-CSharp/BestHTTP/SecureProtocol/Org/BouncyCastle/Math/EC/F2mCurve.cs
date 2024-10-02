using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x02000321 RID: 801
	public class F2mCurve : AbstractF2mCurve
	{
		// Token: 0x06001E5A RID: 7770 RVA: 0x000E28E4 File Offset: 0x000E0AE4
		[Obsolete("Use constructor taking order/cofactor")]
		public F2mCurve(int m, int k, BigInteger a, BigInteger b) : this(m, k, 0, 0, a, b, null, null)
		{
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x000E2900 File Offset: 0x000E0B00
		public F2mCurve(int m, int k, BigInteger a, BigInteger b, BigInteger order, BigInteger cofactor) : this(m, k, 0, 0, a, b, order, cofactor)
		{
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x000E2920 File Offset: 0x000E0B20
		[Obsolete("Use constructor taking order/cofactor")]
		public F2mCurve(int m, int k1, int k2, int k3, BigInteger a, BigInteger b) : this(m, k1, k2, k3, a, b, null, null)
		{
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x000E2940 File Offset: 0x000E0B40
		public F2mCurve(int m, int k1, int k2, int k3, BigInteger a, BigInteger b, BigInteger order, BigInteger cofactor) : base(m, k1, k2, k3)
		{
			this.m = m;
			this.k1 = k1;
			this.k2 = k2;
			this.k3 = k3;
			this.m_order = order;
			this.m_cofactor = cofactor;
			this.m_infinity = new F2mPoint(this, null, null, false);
			if (k1 == 0)
			{
				throw new ArgumentException("k1 must be > 0");
			}
			if (k2 == 0)
			{
				if (k3 != 0)
				{
					throw new ArgumentException("k3 must be 0 if k2 == 0");
				}
			}
			else
			{
				if (k2 <= k1)
				{
					throw new ArgumentException("k2 must be > k1");
				}
				if (k3 <= k2)
				{
					throw new ArgumentException("k3 must be > k2");
				}
			}
			this.m_a = this.FromBigInteger(a);
			this.m_b = this.FromBigInteger(b);
			this.m_coord = 6;
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x000E29F8 File Offset: 0x000E0BF8
		protected F2mCurve(int m, int k1, int k2, int k3, ECFieldElement a, ECFieldElement b, BigInteger order, BigInteger cofactor) : base(m, k1, k2, k3)
		{
			this.m = m;
			this.k1 = k1;
			this.k2 = k2;
			this.k3 = k3;
			this.m_order = order;
			this.m_cofactor = cofactor;
			this.m_infinity = new F2mPoint(this, null, null, false);
			this.m_a = a;
			this.m_b = b;
			this.m_coord = 6;
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x000E2A63 File Offset: 0x000E0C63
		protected override ECCurve CloneCurve()
		{
			return new F2mCurve(this.m, this.k1, this.k2, this.k3, this.m_a, this.m_b, this.m_order, this.m_cofactor);
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x000E2A9A File Offset: 0x000E0C9A
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord <= 1 || coord == 6;
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x000E2AA7 File Offset: 0x000E0CA7
		protected override ECMultiplier CreateDefaultMultiplier()
		{
			if (this.IsKoblitz)
			{
				return new WTauNafMultiplier();
			}
			return base.CreateDefaultMultiplier();
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001E62 RID: 7778 RVA: 0x000E2ABD File Offset: 0x000E0CBD
		public override int FieldSize
		{
			get
			{
				return this.m;
			}
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x000E2AC5 File Offset: 0x000E0CC5
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new F2mFieldElement(this.m, this.k1, this.k2, this.k3, x);
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x000E2AE5 File Offset: 0x000E0CE5
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new F2mPoint(this, x, y, withCompression);
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x000E2AF0 File Offset: 0x000E0CF0
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new F2mPoint(this, x, y, zs, withCompression);
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001E66 RID: 7782 RVA: 0x000E2AFD File Offset: 0x000E0CFD
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001E67 RID: 7783 RVA: 0x000E2ABD File Offset: 0x000E0CBD
		public int M
		{
			get
			{
				return this.m;
			}
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x000E2B05 File Offset: 0x000E0D05
		public bool IsTrinomial()
		{
			return this.k2 == 0 && this.k3 == 0;
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001E69 RID: 7785 RVA: 0x000E2B1A File Offset: 0x000E0D1A
		public int K1
		{
			get
			{
				return this.k1;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001E6A RID: 7786 RVA: 0x000E2B22 File Offset: 0x000E0D22
		public int K2
		{
			get
			{
				return this.k2;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001E6B RID: 7787 RVA: 0x000E2B2A File Offset: 0x000E0D2A
		public int K3
		{
			get
			{
				return this.k3;
			}
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x000E2B34 File Offset: 0x000E0D34
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			int num = (this.m + 63) / 64;
			long[] array = new long[len * num * 2];
			int num2 = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				((F2mFieldElement)ecpoint.RawXCoord).x.CopyTo(array, num2);
				num2 += num;
				((F2mFieldElement)ecpoint.RawYCoord).x.CopyTo(array, num2);
				num2 += num;
			}
			return new F2mCurve.DefaultF2mLookupTable(this, array, len);
		}

		// Token: 0x04001962 RID: 6498
		private const int F2M_DEFAULT_COORDS = 6;

		// Token: 0x04001963 RID: 6499
		private readonly int m;

		// Token: 0x04001964 RID: 6500
		private readonly int k1;

		// Token: 0x04001965 RID: 6501
		private readonly int k2;

		// Token: 0x04001966 RID: 6502
		private readonly int k3;

		// Token: 0x04001967 RID: 6503
		protected readonly F2mPoint m_infinity;

		// Token: 0x0200090F RID: 2319
		private class DefaultF2mLookupTable : ECLookupTable
		{
			// Token: 0x06004E4F RID: 20047 RVA: 0x001B12DE File Offset: 0x001AF4DE
			internal DefaultF2mLookupTable(F2mCurve outer, long[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C32 RID: 3122
			// (get) Token: 0x06004E50 RID: 20048 RVA: 0x001B12FB File Offset: 0x001AF4FB
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E51 RID: 20049 RVA: 0x001B1304 File Offset: 0x001AF504
			public virtual ECPoint Lookup(int index)
			{
				int m = this.m_outer.m;
				int[] array2;
				if (!this.m_outer.IsTrinomial())
				{
					int[] array = new int[3];
					array[0] = this.m_outer.k1;
					array[1] = this.m_outer.k2;
					array2 = array;
					array[2] = this.m_outer.k3;
				}
				else
				{
					(array2 = new int[1])[0] = this.m_outer.k1;
				}
				int[] ks = array2;
				int num = (this.m_outer.m + 63) / 64;
				long[] array3 = new long[num];
				long[] array4 = new long[num];
				int num2 = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					long num3 = (long)((i ^ index) - 1 >> 31);
					for (int j = 0; j < num; j++)
					{
						array3[j] ^= (this.m_table[num2 + j] & num3);
						array4[j] ^= (this.m_table[num2 + num + j] & num3);
					}
					num2 += num * 2;
				}
				ECFieldElement x = new F2mFieldElement(m, ks, new LongArray(array3));
				ECFieldElement y = new F2mFieldElement(m, ks, new LongArray(array4));
				return this.m_outer.CreateRawPoint(x, y, false);
			}

			// Token: 0x04003563 RID: 13667
			private readonly F2mCurve m_outer;

			// Token: 0x04003564 RID: 13668
			private readonly long[] m_table;

			// Token: 0x04003565 RID: 13669
			private readonly int m_size;
		}
	}
}
