using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200039F RID: 927
	internal class SecT233R1Curve : AbstractF2mCurve
	{
		// Token: 0x06002554 RID: 9556 RVA: 0x00102880 File Offset: 0x00100A80
		public SecT233R1Curve() : base(233, 74, 0, 0)
		{
			this.m_infinity = new SecT233R1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.One);
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0066647EDE6C332C7F8C0923BB58213B333B20E9CE4281FE115F7D8F90AD")));
			this.m_order = new BigInteger(1, Hex.Decode("01000000000000000000000000000013E974E72F8A6922031D2603CFE0D7"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x001028FF File Offset: 0x00100AFF
		protected override ECCurve CloneCurve()
		{
			return new SecT233R1Curve();
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06002557 RID: 9559 RVA: 0x00102906 File Offset: 0x00100B06
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06002558 RID: 9560 RVA: 0x00101F29 File Offset: 0x00100129
		public override int FieldSize
		{
			get
			{
				return 233;
			}
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x001021E5 File Offset: 0x001003E5
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT233FieldElement(x);
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x0010290E File Offset: 0x00100B0E
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT233R1Point(this, x, y, withCompression);
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x00102919 File Offset: 0x00100B19
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT233R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x0600255C RID: 9564 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x0600255D RID: 9565 RVA: 0x00101F29 File Offset: 0x00100129
		public virtual int M
		{
			get
			{
				return 233;
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x0600255E RID: 9566 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x0600255F RID: 9567 RVA: 0x00102121 File Offset: 0x00100321
		public virtual int K1
		{
			get
			{
				return 74;
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06002560 RID: 9568 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06002561 RID: 9569 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x00102928 File Offset: 0x00100B28
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			ulong[] array = new ulong[len * 4 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat256.Copy64(((SecT233FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 4;
				Nat256.Copy64(((SecT233FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 4;
			}
			return new SecT233R1Curve.SecT233R1LookupTable(this, array, len);
		}

		// Token: 0x04001AA7 RID: 6823
		private const int SECT233R1_DEFAULT_COORDS = 6;

		// Token: 0x04001AA8 RID: 6824
		private const int SECT233R1_FE_LONGS = 4;

		// Token: 0x04001AA9 RID: 6825
		protected readonly SecT233R1Point m_infinity;

		// Token: 0x02000932 RID: 2354
		private class SecT233R1LookupTable : ECLookupTable
		{
			// Token: 0x06004EA7 RID: 20135 RVA: 0x001B2B21 File Offset: 0x001B0D21
			internal SecT233R1LookupTable(SecT233R1Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C49 RID: 3145
			// (get) Token: 0x06004EA8 RID: 20136 RVA: 0x001B2B3E File Offset: 0x001B0D3E
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004EA9 RID: 20137 RVA: 0x001B2B48 File Offset: 0x001B0D48
			public virtual ECPoint Lookup(int index)
			{
				ulong[] array = Nat256.Create64();
				ulong[] array2 = Nat256.Create64();
				int num = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					ulong num2 = (ulong)((long)((i ^ index) - 1 >> 31));
					for (int j = 0; j < 4; j++)
					{
						array[j] ^= (this.m_table[num + j] & num2);
						array2[j] ^= (this.m_table[num + 4 + j] & num2);
					}
					num += 8;
				}
				return this.m_outer.CreateRawPoint(new SecT233FieldElement(array), new SecT233FieldElement(array2), false);
			}

			// Token: 0x040035CC RID: 13772
			private readonly SecT233R1Curve m_outer;

			// Token: 0x040035CD RID: 13773
			private readonly ulong[] m_table;

			// Token: 0x040035CE RID: 13774
			private readonly int m_size;
		}
	}
}
