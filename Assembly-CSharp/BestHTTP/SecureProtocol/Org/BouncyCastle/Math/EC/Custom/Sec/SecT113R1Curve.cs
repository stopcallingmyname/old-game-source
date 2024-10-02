using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000383 RID: 899
	internal class SecT113R1Curve : AbstractF2mCurve
	{
		// Token: 0x06002384 RID: 9092 RVA: 0x000FB594 File Offset: 0x000F9794
		public SecT113R1Curve() : base(113, 9, 0, 0)
		{
			this.m_infinity = new SecT113R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("003088250CA6E7C7FE649CE85820F7")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("00E8BEE4D3E2260744188BE0E9C723")));
			this.m_order = new BigInteger(1, Hex.Decode("0100000000000000D9CCEC8A39E56F"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x000FB61B File Offset: 0x000F981B
		protected override ECCurve CloneCurve()
		{
			return new SecT113R1Curve();
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06002387 RID: 9095 RVA: 0x000FB62B File Offset: 0x000F982B
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06002388 RID: 9096 RVA: 0x000FB34B File Offset: 0x000F954B
		public override int FieldSize
		{
			get
			{
				return 113;
			}
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x000FB633 File Offset: 0x000F9833
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT113FieldElement(x);
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x000FB63B File Offset: 0x000F983B
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT113R1Point(this, x, y, withCompression);
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x000FB646 File Offset: 0x000F9846
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT113R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x0600238C RID: 9100 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x0600238D RID: 9101 RVA: 0x000FB34B File Offset: 0x000F954B
		public virtual int M
		{
			get
			{
				return 113;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x0600238E RID: 9102 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x0600238F RID: 9103 RVA: 0x000FB54D File Offset: 0x000F974D
		public virtual int K1
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06002390 RID: 9104 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06002391 RID: 9105 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x000FB654 File Offset: 0x000F9854
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
			return new SecT113R1Curve.SecT113R1LookupTable(this, array, len);
		}

		// Token: 0x04001A7B RID: 6779
		private const int SECT113R1_DEFAULT_COORDS = 6;

		// Token: 0x04001A7C RID: 6780
		private const int SECT113R1_FE_LONGS = 2;

		// Token: 0x04001A7D RID: 6781
		protected readonly SecT113R1Point m_infinity;

		// Token: 0x02000928 RID: 2344
		private class SecT113R1LookupTable : ECLookupTable
		{
			// Token: 0x06004E89 RID: 20105 RVA: 0x001B23A3 File Offset: 0x001B05A3
			internal SecT113R1LookupTable(SecT113R1Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C3F RID: 3135
			// (get) Token: 0x06004E8A RID: 20106 RVA: 0x001B23C0 File Offset: 0x001B05C0
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E8B RID: 20107 RVA: 0x001B23C8 File Offset: 0x001B05C8
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

			// Token: 0x040035AE RID: 13742
			private readonly SecT113R1Curve m_outer;

			// Token: 0x040035AF RID: 13743
			private readonly ulong[] m_table;

			// Token: 0x040035B0 RID: 13744
			private readonly int m_size;
		}
	}
}
