using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000393 RID: 915
	internal class SecT163R2Curve : AbstractF2mCurve
	{
		// Token: 0x06002485 RID: 9349 RVA: 0x000FF700 File Offset: 0x000FD900
		public SecT163R2Curve() : base(163, 3, 6, 7)
		{
			this.m_infinity = new SecT163R2Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.One);
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("020A601907B8C953CA1481EB10512F78744A3205FD")));
			this.m_order = new BigInteger(1, Hex.Decode("040000000000000000000292FE77E70C12A4234C33"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x000FF77E File Offset: 0x000FD97E
		protected override ECCurve CloneCurve()
		{
			return new SecT163R2Curve();
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06002488 RID: 9352 RVA: 0x000FF785 File Offset: 0x000FD985
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06002489 RID: 9353 RVA: 0x000FE66B File Offset: 0x000FC86B
		public override int FieldSize
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x000FE930 File Offset: 0x000FCB30
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT163FieldElement(x);
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x000FF78D File Offset: 0x000FD98D
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT163R2Point(this, x, y, withCompression);
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x000FF798 File Offset: 0x000FD998
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT163R2Point(this, x, y, zs, withCompression);
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x0600248D RID: 9357 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x0600248E RID: 9358 RVA: 0x000FE66B File Offset: 0x000FC86B
		public virtual int M
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x0600248F RID: 9359 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06002490 RID: 9360 RVA: 0x000A89A4 File Offset: 0x000A6BA4
		public virtual int K1
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06002491 RID: 9361 RVA: 0x000A4E21 File Offset: 0x000A3021
		public virtual int K2
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06002492 RID: 9362 RVA: 0x000FE865 File Offset: 0x000FCA65
		public virtual int K3
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x000FF7A8 File Offset: 0x000FD9A8
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			ulong[] array = new ulong[len * 3 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat192.Copy64(((SecT163FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 3;
				Nat192.Copy64(((SecT163FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 3;
			}
			return new SecT163R2Curve.SecT163R2LookupTable(this, array, len);
		}

		// Token: 0x04001A95 RID: 6805
		private const int SECT163R2_DEFAULT_COORDS = 6;

		// Token: 0x04001A96 RID: 6806
		private const int SECT163R2_FE_LONGS = 3;

		// Token: 0x04001A97 RID: 6807
		protected readonly SecT163R2Point m_infinity;

		// Token: 0x0200092E RID: 2350
		private class SecT163R2LookupTable : ECLookupTable
		{
			// Token: 0x06004E9B RID: 20123 RVA: 0x001B2821 File Offset: 0x001B0A21
			internal SecT163R2LookupTable(SecT163R2Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C45 RID: 3141
			// (get) Token: 0x06004E9C RID: 20124 RVA: 0x001B283E File Offset: 0x001B0A3E
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E9D RID: 20125 RVA: 0x001B2848 File Offset: 0x001B0A48
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
				return this.m_outer.CreateRawPoint(new SecT163FieldElement(array), new SecT163FieldElement(array2), false);
			}

			// Token: 0x040035C0 RID: 13760
			private readonly SecT163R2Curve m_outer;

			// Token: 0x040035C1 RID: 13761
			private readonly ulong[] m_table;

			// Token: 0x040035C2 RID: 13762
			private readonly int m_size;
		}
	}
}
