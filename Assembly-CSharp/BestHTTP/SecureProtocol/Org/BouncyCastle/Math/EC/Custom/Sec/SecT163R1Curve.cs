using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000391 RID: 913
	internal class SecT163R1Curve : AbstractF2mCurve
	{
		// Token: 0x0600246C RID: 9324 RVA: 0x000FEFAC File Offset: 0x000FD1AC
		public SecT163R1Curve() : base(163, 3, 6, 7)
		{
			this.m_infinity = new SecT163R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("07B6882CAAEFA84F9554FF8428BD88E246D2782AE2")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0713612DCDDCB40AAB946BDA29CA91F73AF958AFD9")));
			this.m_order = new BigInteger(1, Hex.Decode("03FFFFFFFFFFFFFFFFFFFF48AAB689C29CA710279B"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x000FF035 File Offset: 0x000FD235
		protected override ECCurve CloneCurve()
		{
			return new SecT163R1Curve();
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x0600246F RID: 9327 RVA: 0x000FF03C File Offset: 0x000FD23C
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06002470 RID: 9328 RVA: 0x000FE66B File Offset: 0x000FC86B
		public override int FieldSize
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x000FE930 File Offset: 0x000FCB30
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT163FieldElement(x);
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x000FF044 File Offset: 0x000FD244
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT163R1Point(this, x, y, withCompression);
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x000FF04F File Offset: 0x000FD24F
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT163R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06002474 RID: 9332 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06002475 RID: 9333 RVA: 0x000FE66B File Offset: 0x000FC86B
		public virtual int M
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06002476 RID: 9334 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06002477 RID: 9335 RVA: 0x000A89A4 File Offset: 0x000A6BA4
		public virtual int K1
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06002478 RID: 9336 RVA: 0x000A4E21 File Offset: 0x000A3021
		public virtual int K2
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06002479 RID: 9337 RVA: 0x000FE865 File Offset: 0x000FCA65
		public virtual int K3
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x000FF05C File Offset: 0x000FD25C
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
			return new SecT163R1Curve.SecT163R1LookupTable(this, array, len);
		}

		// Token: 0x04001A92 RID: 6802
		private const int SECT163R1_DEFAULT_COORDS = 6;

		// Token: 0x04001A93 RID: 6803
		private const int SECT163R1_FE_LONGS = 3;

		// Token: 0x04001A94 RID: 6804
		protected readonly SecT163R1Point m_infinity;

		// Token: 0x0200092D RID: 2349
		private class SecT163R1LookupTable : ECLookupTable
		{
			// Token: 0x06004E98 RID: 20120 RVA: 0x001B2761 File Offset: 0x001B0961
			internal SecT163R1LookupTable(SecT163R1Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C44 RID: 3140
			// (get) Token: 0x06004E99 RID: 20121 RVA: 0x001B277E File Offset: 0x001B097E
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E9A RID: 20122 RVA: 0x001B2788 File Offset: 0x001B0988
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

			// Token: 0x040035BD RID: 13757
			private readonly SecT163R1Curve m_outer;

			// Token: 0x040035BE RID: 13758
			private readonly ulong[] m_table;

			// Token: 0x040035BF RID: 13759
			private readonly int m_size;
		}
	}
}
