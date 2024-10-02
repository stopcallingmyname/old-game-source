using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x020003AD RID: 941
	internal class SecT409K1Curve : AbstractF2mCurve
	{
		// Token: 0x0600265A RID: 9818 RVA: 0x0010675C File Offset: 0x0010495C
		public SecT409K1Curve() : base(409, 87, 0, 0)
		{
			this.m_infinity = new SecT409K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.One);
			this.m_order = new BigInteger(1, Hex.Decode("7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE5F83B2D4EA20400EC4557D5ED3E3E7CA5B4B5C83B8E01E5FCF"));
			this.m_cofactor = BigInteger.ValueOf(4L);
			this.m_coord = 6;
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x001067D2 File Offset: 0x001049D2
		protected override ECCurve CloneCurve()
		{
			return new SecT409K1Curve();
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x000FE921 File Offset: 0x000FCB21
		protected override ECMultiplier CreateDefaultMultiplier()
		{
			return new WTauNafMultiplier();
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x0600265E RID: 9822 RVA: 0x001067D9 File Offset: 0x001049D9
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x0600265F RID: 9823 RVA: 0x0010651C File Offset: 0x0010471C
		public override int FieldSize
		{
			get
			{
				return 409;
			}
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x001067E1 File Offset: 0x001049E1
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT409FieldElement(x);
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x001067E9 File Offset: 0x001049E9
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT409K1Point(this, x, y, withCompression);
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x001067F4 File Offset: 0x001049F4
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT409K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06002663 RID: 9827 RVA: 0x0006AE98 File Offset: 0x00069098
		public override bool IsKoblitz
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06002664 RID: 9828 RVA: 0x0010651C File Offset: 0x0010471C
		public virtual int M
		{
			get
			{
				return 409;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06002665 RID: 9829 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06002666 RID: 9830 RVA: 0x00106715 File Offset: 0x00104915
		public virtual int K1
		{
			get
			{
				return 87;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06002667 RID: 9831 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06002668 RID: 9832 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x00106804 File Offset: 0x00104A04
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			ulong[] array = new ulong[len * 7 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat448.Copy64(((SecT409FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 7;
				Nat448.Copy64(((SecT409FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 7;
			}
			return new SecT409K1Curve.SecT409K1LookupTable(this, array, len);
		}

		// Token: 0x04001ABD RID: 6845
		private const int SECT409K1_DEFAULT_COORDS = 6;

		// Token: 0x04001ABE RID: 6846
		private const int SECT409K1_FE_LONGS = 7;

		// Token: 0x04001ABF RID: 6847
		protected readonly SecT409K1Point m_infinity;

		// Token: 0x02000936 RID: 2358
		private class SecT409K1LookupTable : ECLookupTable
		{
			// Token: 0x06004EB3 RID: 20147 RVA: 0x001B2E22 File Offset: 0x001B1022
			internal SecT409K1LookupTable(SecT409K1Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C4D RID: 3149
			// (get) Token: 0x06004EB4 RID: 20148 RVA: 0x001B2E3F File Offset: 0x001B103F
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004EB5 RID: 20149 RVA: 0x001B2E48 File Offset: 0x001B1048
			public virtual ECPoint Lookup(int index)
			{
				ulong[] array = Nat448.Create64();
				ulong[] array2 = Nat448.Create64();
				int num = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					ulong num2 = (ulong)((long)((i ^ index) - 1 >> 31));
					for (int j = 0; j < 7; j++)
					{
						array[j] ^= (this.m_table[num + j] & num2);
						array2[j] ^= (this.m_table[num + 7 + j] & num2);
					}
					num += 14;
				}
				return this.m_outer.CreateRawPoint(new SecT409FieldElement(array), new SecT409FieldElement(array2), false);
			}

			// Token: 0x040035D8 RID: 13784
			private readonly SecT409K1Curve m_outer;

			// Token: 0x040035D9 RID: 13785
			private readonly ulong[] m_table;

			// Token: 0x040035DA RID: 13786
			private readonly int m_size;
		}
	}
}
