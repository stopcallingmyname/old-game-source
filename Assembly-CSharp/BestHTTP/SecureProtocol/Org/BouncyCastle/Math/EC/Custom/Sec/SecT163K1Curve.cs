using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200038F RID: 911
	internal class SecT163K1Curve : AbstractF2mCurve
	{
		// Token: 0x06002452 RID: 9298 RVA: 0x000FE8AC File Offset: 0x000FCAAC
		public SecT163K1Curve() : base(163, 3, 6, 7)
		{
			this.m_infinity = new SecT163K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.One);
			this.m_b = this.m_a;
			this.m_order = new BigInteger(1, Hex.Decode("04000000000000000000020108A2E0CC0D99F8A5EF"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x000FE91A File Offset: 0x000FCB1A
		protected override ECCurve CloneCurve()
		{
			return new SecT163K1Curve();
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x000FE921 File Offset: 0x000FCB21
		protected override ECMultiplier CreateDefaultMultiplier()
		{
			return new WTauNafMultiplier();
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06002456 RID: 9302 RVA: 0x000FE928 File Offset: 0x000FCB28
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06002457 RID: 9303 RVA: 0x000FE66B File Offset: 0x000FC86B
		public override int FieldSize
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x000FE930 File Offset: 0x000FCB30
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT163FieldElement(x);
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x000FE938 File Offset: 0x000FCB38
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT163K1Point(this, x, y, withCompression);
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000FE943 File Offset: 0x000FCB43
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT163K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x0600245B RID: 9307 RVA: 0x0006AE98 File Offset: 0x00069098
		public override bool IsKoblitz
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x0600245C RID: 9308 RVA: 0x000FE66B File Offset: 0x000FC86B
		public virtual int M
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x0600245D RID: 9309 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x0600245E RID: 9310 RVA: 0x000A89A4 File Offset: 0x000A6BA4
		public virtual int K1
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x0600245F RID: 9311 RVA: 0x000A4E21 File Offset: 0x000A3021
		public virtual int K2
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06002460 RID: 9312 RVA: 0x000FE865 File Offset: 0x000FCA65
		public virtual int K3
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x000FE950 File Offset: 0x000FCB50
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
			return new SecT163K1Curve.SecT163K1LookupTable(this, array, len);
		}

		// Token: 0x04001A8F RID: 6799
		private const int SECT163K1_DEFAULT_COORDS = 6;

		// Token: 0x04001A90 RID: 6800
		private const int SECT163K1_FE_LONGS = 3;

		// Token: 0x04001A91 RID: 6801
		protected readonly SecT163K1Point m_infinity;

		// Token: 0x0200092C RID: 2348
		private class SecT163K1LookupTable : ECLookupTable
		{
			// Token: 0x06004E95 RID: 20117 RVA: 0x001B26A1 File Offset: 0x001B08A1
			internal SecT163K1LookupTable(SecT163K1Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C43 RID: 3139
			// (get) Token: 0x06004E96 RID: 20118 RVA: 0x001B26BE File Offset: 0x001B08BE
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E97 RID: 20119 RVA: 0x001B26C8 File Offset: 0x001B08C8
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

			// Token: 0x040035BA RID: 13754
			private readonly SecT163K1Curve m_outer;

			// Token: 0x040035BB RID: 13755
			private readonly ulong[] m_table;

			// Token: 0x040035BC RID: 13756
			private readonly int m_size;
		}
	}
}
