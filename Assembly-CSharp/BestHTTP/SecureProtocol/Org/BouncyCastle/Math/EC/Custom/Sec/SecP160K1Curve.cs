using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000357 RID: 855
	internal class SecP160K1Curve : AbstractFpCurve
	{
		// Token: 0x060020DA RID: 8410 RVA: 0x000F1410 File Offset: 0x000EF610
		public SecP160K1Curve() : base(SecP160K1Curve.q)
		{
			this.m_infinity = new SecP160K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.ValueOf(7L));
			this.m_order = new BigInteger(1, Hex.Decode("0100000000000000000001B8FA16DFAB9ACA16B6B3"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x000F1482 File Offset: 0x000EF682
		protected override ECCurve CloneCurve()
		{
			return new SecP160K1Curve();
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x000F0641 File Offset: 0x000EE841
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x060020DD RID: 8413 RVA: 0x000F1489 File Offset: 0x000EF689
		public virtual BigInteger Q
		{
			get
			{
				return SecP160K1Curve.q;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x060020DE RID: 8414 RVA: 0x000F1490 File Offset: 0x000EF690
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x060020DF RID: 8415 RVA: 0x000F1498 File Offset: 0x000EF698
		public override int FieldSize
		{
			get
			{
				return SecP160K1Curve.q.BitLength;
			}
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x000F14A4 File Offset: 0x000EF6A4
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP160R2FieldElement(x);
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x000F14AC File Offset: 0x000EF6AC
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP160K1Point(this, x, y, withCompression);
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x000F14B7 File Offset: 0x000EF6B7
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP160K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x000F14C4 File Offset: 0x000EF6C4
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			uint[] array = new uint[len * 5 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat160.Copy(((SecP160R2FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 5;
				Nat160.Copy(((SecP160R2FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 5;
			}
			return new SecP160K1Curve.SecP160K1LookupTable(this, array, len);
		}

		// Token: 0x04001A04 RID: 6660
		public static readonly BigInteger q = SecP160R2Curve.q;

		// Token: 0x04001A05 RID: 6661
		private const int SECP160K1_DEFAULT_COORDS = 2;

		// Token: 0x04001A06 RID: 6662
		private const int SECP160K1_FE_INTS = 5;

		// Token: 0x04001A07 RID: 6663
		protected readonly SecP160K1Point m_infinity;

		// Token: 0x0200091D RID: 2333
		private class SecP160K1LookupTable : ECLookupTable
		{
			// Token: 0x06004E68 RID: 20072 RVA: 0x001B1B58 File Offset: 0x001AFD58
			internal SecP160K1LookupTable(SecP160K1Curve outer, uint[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C34 RID: 3124
			// (get) Token: 0x06004E69 RID: 20073 RVA: 0x001B1B75 File Offset: 0x001AFD75
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E6A RID: 20074 RVA: 0x001B1B80 File Offset: 0x001AFD80
			public virtual ECPoint Lookup(int index)
			{
				uint[] array = Nat256.Create();
				uint[] array2 = Nat256.Create();
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
				return this.m_outer.CreateRawPoint(new SecP160R2FieldElement(array), new SecP160R2FieldElement(array2), false);
			}

			// Token: 0x0400358D RID: 13709
			private readonly SecP160K1Curve m_outer;

			// Token: 0x0400358E RID: 13710
			private readonly uint[] m_table;

			// Token: 0x0400358F RID: 13711
			private readonly int m_size;
		}
	}
}
