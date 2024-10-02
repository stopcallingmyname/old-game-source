using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000359 RID: 857
	internal class SecP160R1Curve : AbstractFpCurve
	{
		// Token: 0x060020EE RID: 8430 RVA: 0x000F1A60 File Offset: 0x000EFC60
		public SecP160R1Curve() : base(SecP160R1Curve.q)
		{
			this.m_infinity = new SecP160R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF7FFFFFFC")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("1C97BEFC54BD7A8B65ACF89F81D4D4ADC565FA45")));
			this.m_order = new BigInteger(1, Hex.Decode("0100000000000000000001F4C8F927AED3CA752257"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x000F1AE6 File Offset: 0x000EFCE6
		protected override ECCurve CloneCurve()
		{
			return new SecP160R1Curve();
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x000F0641 File Offset: 0x000EE841
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x060020F1 RID: 8433 RVA: 0x000F1AED File Offset: 0x000EFCED
		public virtual BigInteger Q
		{
			get
			{
				return SecP160R1Curve.q;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x060020F2 RID: 8434 RVA: 0x000F1AF4 File Offset: 0x000EFCF4
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x060020F3 RID: 8435 RVA: 0x000F1AFC File Offset: 0x000EFCFC
		public override int FieldSize
		{
			get
			{
				return SecP160R1Curve.q.BitLength;
			}
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x000F1B08 File Offset: 0x000EFD08
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP160R1FieldElement(x);
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x000F1B10 File Offset: 0x000EFD10
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP160R1Point(this, x, y, withCompression);
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x000F1B1B File Offset: 0x000EFD1B
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP160R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x000F1B28 File Offset: 0x000EFD28
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			uint[] array = new uint[len * 5 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat160.Copy(((SecP160R1FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 5;
				Nat160.Copy(((SecP160R1FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 5;
			}
			return new SecP160R1Curve.SecP160R1LookupTable(this, array, len);
		}

		// Token: 0x04001A08 RID: 6664
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF7FFFFFFF"));

		// Token: 0x04001A09 RID: 6665
		private const int SECP160R1_DEFAULT_COORDS = 2;

		// Token: 0x04001A0A RID: 6666
		private const int SECP160R1_FE_INTS = 5;

		// Token: 0x04001A0B RID: 6667
		protected readonly SecP160R1Point m_infinity;

		// Token: 0x0200091E RID: 2334
		private class SecP160R1LookupTable : ECLookupTable
		{
			// Token: 0x06004E6B RID: 20075 RVA: 0x001B1C19 File Offset: 0x001AFE19
			internal SecP160R1LookupTable(SecP160R1Curve outer, uint[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C35 RID: 3125
			// (get) Token: 0x06004E6C RID: 20076 RVA: 0x001B1C36 File Offset: 0x001AFE36
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E6D RID: 20077 RVA: 0x001B1C40 File Offset: 0x001AFE40
			public virtual ECPoint Lookup(int index)
			{
				uint[] array = Nat160.Create();
				uint[] array2 = Nat160.Create();
				int num = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					uint num2 = (uint)((i ^ index) - 1 >> 31);
					for (int j = 0; j < 5; j++)
					{
						array[j] ^= (this.m_table[num + j] & num2);
						array2[j] ^= (this.m_table[num + 5 + j] & num2);
					}
					num += 10;
				}
				return this.m_outer.CreateRawPoint(new SecP160R1FieldElement(array), new SecP160R1FieldElement(array2), false);
			}

			// Token: 0x04003590 RID: 13712
			private readonly SecP160R1Curve m_outer;

			// Token: 0x04003591 RID: 13713
			private readonly uint[] m_table;

			// Token: 0x04003592 RID: 13714
			private readonly int m_size;
		}
	}
}
