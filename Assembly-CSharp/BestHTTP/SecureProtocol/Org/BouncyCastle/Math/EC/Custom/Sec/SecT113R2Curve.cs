using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000385 RID: 901
	internal class SecT113R2Curve : AbstractF2mCurve
	{
		// Token: 0x0600239D RID: 9117 RVA: 0x000FBD18 File Offset: 0x000F9F18
		public SecT113R2Curve() : base(113, 9, 0, 0)
		{
			this.m_infinity = new SecT113R2Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("00689918DBEC7E5A0DD6DFC0AA55C7")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0095E9A9EC9B297BD4BF36E059184F")));
			this.m_order = new BigInteger(1, Hex.Decode("010000000000000108789B2496AF93"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x000FBD9F File Offset: 0x000F9F9F
		protected override ECCurve CloneCurve()
		{
			return new SecT113R2Curve();
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060023A0 RID: 9120 RVA: 0x000FBDA6 File Offset: 0x000F9FA6
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060023A1 RID: 9121 RVA: 0x000FB34B File Offset: 0x000F954B
		public override int FieldSize
		{
			get
			{
				return 113;
			}
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x000FB633 File Offset: 0x000F9833
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT113FieldElement(x);
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x000FBDAE File Offset: 0x000F9FAE
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT113R2Point(this, x, y, withCompression);
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x000FBDB9 File Offset: 0x000F9FB9
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT113R2Point(this, x, y, zs, withCompression);
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060023A5 RID: 9125 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x060023A6 RID: 9126 RVA: 0x000FB34B File Offset: 0x000F954B
		public virtual int M
		{
			get
			{
				return 113;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x060023A7 RID: 9127 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x060023A8 RID: 9128 RVA: 0x000FB54D File Offset: 0x000F974D
		public virtual int K1
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x060023A9 RID: 9129 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x060023AA RID: 9130 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x000FBDC8 File Offset: 0x000F9FC8
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			ulong[] array = new ulong[len * 2 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat128.Copy64(((SecT113FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 2;
				Nat128.Copy64(((SecT113FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 2;
			}
			return new SecT113R2Curve.SecT113R2LookupTable(this, array, len);
		}

		// Token: 0x04001A7E RID: 6782
		private const int SECT113R2_DEFAULT_COORDS = 6;

		// Token: 0x04001A7F RID: 6783
		private const int SECT113R2_FE_LONGS = 2;

		// Token: 0x04001A80 RID: 6784
		protected readonly SecT113R2Point m_infinity;

		// Token: 0x02000929 RID: 2345
		private class SecT113R2LookupTable : ECLookupTable
		{
			// Token: 0x06004E8C RID: 20108 RVA: 0x001B2461 File Offset: 0x001B0661
			internal SecT113R2LookupTable(SecT113R2Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C40 RID: 3136
			// (get) Token: 0x06004E8D RID: 20109 RVA: 0x001B247E File Offset: 0x001B067E
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E8E RID: 20110 RVA: 0x001B2488 File Offset: 0x001B0688
			public virtual ECPoint Lookup(int index)
			{
				ulong[] array = Nat128.Create64();
				ulong[] array2 = Nat128.Create64();
				int num = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					ulong num2 = (ulong)((long)((i ^ index) - 1 >> 31));
					for (int j = 0; j < 2; j++)
					{
						array[j] ^= (this.m_table[num + j] & num2);
						array2[j] ^= (this.m_table[num + 2 + j] & num2);
					}
					num += 4;
				}
				return this.m_outer.CreateRawPoint(new SecT113FieldElement(array), new SecT113FieldElement(array2), false);
			}

			// Token: 0x040035B1 RID: 13745
			private readonly SecT113R2Curve m_outer;

			// Token: 0x040035B2 RID: 13746
			private readonly ulong[] m_table;

			// Token: 0x040035B3 RID: 13747
			private readonly int m_size;
		}
	}
}
