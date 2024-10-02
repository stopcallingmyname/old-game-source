using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200039D RID: 925
	internal class SecT233K1Curve : AbstractF2mCurve
	{
		// Token: 0x0600253A RID: 9530 RVA: 0x00102168 File Offset: 0x00100368
		public SecT233K1Curve() : base(233, 74, 0, 0)
		{
			this.m_infinity = new SecT233K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.One);
			this.m_order = new BigInteger(1, Hex.Decode("8000000000000000000000000000069D5BB915BCD46EFB1AD5F173ABDF"));
			this.m_cofactor = BigInteger.ValueOf(4L);
			this.m_coord = 6;
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x001021DE File Offset: 0x001003DE
		protected override ECCurve CloneCurve()
		{
			return new SecT233K1Curve();
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x000FE921 File Offset: 0x000FCB21
		protected override ECMultiplier CreateDefaultMultiplier()
		{
			return new WTauNafMultiplier();
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x0600253E RID: 9534 RVA: 0x00101F29 File Offset: 0x00100129
		public override int FieldSize
		{
			get
			{
				return 233;
			}
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x001021E5 File Offset: 0x001003E5
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT233FieldElement(x);
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x001021ED File Offset: 0x001003ED
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT233K1Point(this, x, y, withCompression);
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x001021F8 File Offset: 0x001003F8
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT233K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06002542 RID: 9538 RVA: 0x00102205 File Offset: 0x00100405
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06002543 RID: 9539 RVA: 0x0006AE98 File Offset: 0x00069098
		public override bool IsKoblitz
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06002544 RID: 9540 RVA: 0x00101F29 File Offset: 0x00100129
		public virtual int M
		{
			get
			{
				return 233;
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06002545 RID: 9541 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06002546 RID: 9542 RVA: 0x00102121 File Offset: 0x00100321
		public virtual int K1
		{
			get
			{
				return 74;
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06002547 RID: 9543 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06002548 RID: 9544 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x00102210 File Offset: 0x00100410
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
			return new SecT233K1Curve.SecT233K1LookupTable(this, array, len);
		}

		// Token: 0x04001AA4 RID: 6820
		private const int SECT233K1_DEFAULT_COORDS = 6;

		// Token: 0x04001AA5 RID: 6821
		private const int SECT233K1_FE_LONGS = 4;

		// Token: 0x04001AA6 RID: 6822
		protected readonly SecT233K1Point m_infinity;

		// Token: 0x02000931 RID: 2353
		private class SecT233K1LookupTable : ECLookupTable
		{
			// Token: 0x06004EA4 RID: 20132 RVA: 0x001B2A61 File Offset: 0x001B0C61
			internal SecT233K1LookupTable(SecT233K1Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C48 RID: 3144
			// (get) Token: 0x06004EA5 RID: 20133 RVA: 0x001B2A7E File Offset: 0x001B0C7E
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004EA6 RID: 20134 RVA: 0x001B2A88 File Offset: 0x001B0C88
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

			// Token: 0x040035C9 RID: 13769
			private readonly SecT233K1Curve m_outer;

			// Token: 0x040035CA RID: 13770
			private readonly ulong[] m_table;

			// Token: 0x040035CB RID: 13771
			private readonly int m_size;
		}
	}
}
