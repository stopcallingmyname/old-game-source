using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000369 RID: 873
	internal class SecP224K1Curve : AbstractFpCurve
	{
		// Token: 0x060021E0 RID: 8672 RVA: 0x000F5318 File Offset: 0x000F3518
		public SecP224K1Curve() : base(SecP224K1Curve.q)
		{
			this.m_infinity = new SecP224K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.ValueOf(5L));
			this.m_order = new BigInteger(1, Hex.Decode("010000000000000000000000000001DCE8D2EC6184CAF0A971769FB1F7"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x000F538A File Offset: 0x000F358A
		protected override ECCurve CloneCurve()
		{
			return new SecP224K1Curve();
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x000F0641 File Offset: 0x000EE841
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060021E3 RID: 8675 RVA: 0x000F5391 File Offset: 0x000F3591
		public virtual BigInteger Q
		{
			get
			{
				return SecP224K1Curve.q;
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060021E4 RID: 8676 RVA: 0x000F5398 File Offset: 0x000F3598
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060021E5 RID: 8677 RVA: 0x000F53A0 File Offset: 0x000F35A0
		public override int FieldSize
		{
			get
			{
				return SecP224K1Curve.q.BitLength;
			}
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x000F53AC File Offset: 0x000F35AC
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP224K1FieldElement(x);
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x000F53B4 File Offset: 0x000F35B4
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP224K1Point(this, x, y, withCompression);
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x000F53BF File Offset: 0x000F35BF
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP224K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x000F53CC File Offset: 0x000F35CC
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			uint[] array = new uint[len * 7 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat224.Copy(((SecP224K1FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 7;
				Nat224.Copy(((SecP224K1FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 7;
			}
			return new SecP224K1Curve.SecP224K1LookupTable(this, array, len);
		}

		// Token: 0x04001A37 RID: 6711
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFE56D"));

		// Token: 0x04001A38 RID: 6712
		private const int SECP224K1_DEFAULT_COORDS = 2;

		// Token: 0x04001A39 RID: 6713
		private const int SECP224K1_FE_INTS = 7;

		// Token: 0x04001A3A RID: 6714
		protected readonly SecP224K1Point m_infinity;

		// Token: 0x02000922 RID: 2338
		private class SecP224K1LookupTable : ECLookupTable
		{
			// Token: 0x06004E77 RID: 20087 RVA: 0x001B1F19 File Offset: 0x001B0119
			internal SecP224K1LookupTable(SecP224K1Curve outer, uint[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C39 RID: 3129
			// (get) Token: 0x06004E78 RID: 20088 RVA: 0x001B1F36 File Offset: 0x001B0136
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E79 RID: 20089 RVA: 0x001B1F40 File Offset: 0x001B0140
			public virtual ECPoint Lookup(int index)
			{
				uint[] array = Nat224.Create();
				uint[] array2 = Nat224.Create();
				int num = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					uint num2 = (uint)((i ^ index) - 1 >> 31);
					for (int j = 0; j < 7; j++)
					{
						array[j] ^= (this.m_table[num + j] & num2);
						array2[j] ^= (this.m_table[num + 7 + j] & num2);
					}
					num += 14;
				}
				return this.m_outer.CreateRawPoint(new SecP224K1FieldElement(array), new SecP224K1FieldElement(array2), false);
			}

			// Token: 0x0400359C RID: 13724
			private readonly SecP224K1Curve m_outer;

			// Token: 0x0400359D RID: 13725
			private readonly uint[] m_table;

			// Token: 0x0400359E RID: 13726
			private readonly int m_size;
		}
	}
}
