using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200038B RID: 907
	internal class SecT131R2Curve : AbstractF2mCurve
	{
		// Token: 0x06002404 RID: 9220 RVA: 0x000FD68C File Offset: 0x000FB88C
		public SecT131R2Curve() : base(131, 2, 3, 8)
		{
			this.m_infinity = new SecT131R2Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("03E5A88919D7CAFCBF415F07C2176573B2")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("04B8266A46C55657AC734CE38F018F2192")));
			this.m_order = new BigInteger(1, Hex.Decode("0400000000000000016954A233049BA98F"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x000FD715 File Offset: 0x000FB915
		protected override ECCurve CloneCurve()
		{
			return new SecT131R2Curve();
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06002407 RID: 9223 RVA: 0x000FCCF1 File Offset: 0x000FAEF1
		public override int FieldSize
		{
			get
			{
				return 131;
			}
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x000FCFC8 File Offset: 0x000FB1C8
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT131FieldElement(x);
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x000FD71C File Offset: 0x000FB91C
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT131R2Point(this, x, y, withCompression);
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x000FD727 File Offset: 0x000FB927
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT131R2Point(this, x, y, zs, withCompression);
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x0600240B RID: 9227 RVA: 0x000FD734 File Offset: 0x000FB934
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x0600240C RID: 9228 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x0600240D RID: 9229 RVA: 0x000FCCF1 File Offset: 0x000FAEF1
		public virtual int M
		{
			get
			{
				return 131;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x0600240E RID: 9230 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x0600240F RID: 9231 RVA: 0x000A7398 File Offset: 0x000A5598
		public virtual int K1
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06002410 RID: 9232 RVA: 0x000A89A4 File Offset: 0x000A6BA4
		public virtual int K2
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06002411 RID: 9233 RVA: 0x000FCEE9 File Offset: 0x000FB0E9
		public virtual int K3
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x000FD73C File Offset: 0x000FB93C
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
			return new SecT131R2Curve.SecT131R2LookupTable(this, array, len);
		}

		// Token: 0x04001A88 RID: 6792
		private const int SECT131R2_DEFAULT_COORDS = 6;

		// Token: 0x04001A89 RID: 6793
		private const int SECT131R2_FE_LONGS = 3;

		// Token: 0x04001A8A RID: 6794
		protected readonly SecT131R2Point m_infinity;

		// Token: 0x0200092B RID: 2347
		private class SecT131R2LookupTable : ECLookupTable
		{
			// Token: 0x06004E92 RID: 20114 RVA: 0x001B25E1 File Offset: 0x001B07E1
			internal SecT131R2LookupTable(SecT131R2Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C42 RID: 3138
			// (get) Token: 0x06004E93 RID: 20115 RVA: 0x001B25FE File Offset: 0x001B07FE
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E94 RID: 20116 RVA: 0x001B2608 File Offset: 0x001B0808
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

			// Token: 0x040035B7 RID: 13751
			private readonly SecT131R2Curve m_outer;

			// Token: 0x040035B8 RID: 13752
			private readonly ulong[] m_table;

			// Token: 0x040035B9 RID: 13753
			private readonly int m_size;
		}
	}
}
