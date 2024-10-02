using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000389 RID: 905
	internal class SecT131R1Curve : AbstractF2mCurve
	{
		// Token: 0x060023EB RID: 9195 RVA: 0x000FCF30 File Offset: 0x000FB130
		public SecT131R1Curve() : base(131, 2, 3, 8)
		{
			this.m_infinity = new SecT131R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("07A11B09A76B562144418FF3FF8C2570B8")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0217C05610884B63B9C6C7291678F9D341")));
			this.m_order = new BigInteger(1, Hex.Decode("0400000000000000023123953A9464B54D"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x000FCFB9 File Offset: 0x000FB1B9
		protected override ECCurve CloneCurve()
		{
			return new SecT131R1Curve();
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x060023EE RID: 9198 RVA: 0x000FCFC0 File Offset: 0x000FB1C0
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x060023EF RID: 9199 RVA: 0x000FCCF1 File Offset: 0x000FAEF1
		public override int FieldSize
		{
			get
			{
				return 131;
			}
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x000FCFC8 File Offset: 0x000FB1C8
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT131FieldElement(x);
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x000FCFD0 File Offset: 0x000FB1D0
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT131R1Point(this, x, y, withCompression);
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x000FCFDB File Offset: 0x000FB1DB
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT131R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x060023F3 RID: 9203 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x060023F4 RID: 9204 RVA: 0x000FCCF1 File Offset: 0x000FAEF1
		public virtual int M
		{
			get
			{
				return 131;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x060023F5 RID: 9205 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x060023F6 RID: 9206 RVA: 0x000A7398 File Offset: 0x000A5598
		public virtual int K1
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x060023F7 RID: 9207 RVA: 0x000A89A4 File Offset: 0x000A6BA4
		public virtual int K2
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x060023F8 RID: 9208 RVA: 0x000FCEE9 File Offset: 0x000FB0E9
		public virtual int K3
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x000FCFE8 File Offset: 0x000FB1E8
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			ulong[] array = new ulong[len * 3 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat192.Copy64(((SecT131FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 3;
				Nat192.Copy64(((SecT131FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 3;
			}
			return new SecT131R1Curve.SecT131R1LookupTable(this, array, len);
		}

		// Token: 0x04001A85 RID: 6789
		private const int SECT131R1_DEFAULT_COORDS = 6;

		// Token: 0x04001A86 RID: 6790
		private const int SECT131R1_FE_LONGS = 3;

		// Token: 0x04001A87 RID: 6791
		protected readonly SecT131R1Point m_infinity;

		// Token: 0x0200092A RID: 2346
		private class SecT131R1LookupTable : ECLookupTable
		{
			// Token: 0x06004E8F RID: 20111 RVA: 0x001B2521 File Offset: 0x001B0721
			internal SecT131R1LookupTable(SecT131R1Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C41 RID: 3137
			// (get) Token: 0x06004E90 RID: 20112 RVA: 0x001B253E File Offset: 0x001B073E
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E91 RID: 20113 RVA: 0x001B2548 File Offset: 0x001B0748
			public virtual ECPoint Lookup(int index)
			{
				ulong[] array = Nat192.Create64();
				ulong[] array2 = Nat192.Create64();
				int num = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					ulong num2 = (ulong)((long)((i ^ index) - 1 >> 31));
					for (int j = 0; j < 3; j++)
					{
						array[j] ^= (this.m_table[num + j] & num2);
						array2[j] ^= (this.m_table[num + 3 + j] & num2);
					}
					num += 6;
				}
				return this.m_outer.CreateRawPoint(new SecT131FieldElement(array), new SecT131FieldElement(array2), false);
			}

			// Token: 0x040035B4 RID: 13748
			private readonly SecT131R1Curve m_outer;

			// Token: 0x040035B5 RID: 13749
			private readonly ulong[] m_table;

			// Token: 0x040035B6 RID: 13750
			private readonly int m_size;
		}
	}
}
