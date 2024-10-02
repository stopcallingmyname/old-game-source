using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200035D RID: 861
	internal class SecP160R2Curve : AbstractFpCurve
	{
		// Token: 0x0600212A RID: 8490 RVA: 0x000F2858 File Offset: 0x000F0A58
		public SecP160R2Curve() : base(SecP160R2Curve.q)
		{
			this.m_infinity = new SecP160R2Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFAC70")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("B4E134D3FB59EB8BAB57274904664D5AF50388BA")));
			this.m_order = new BigInteger(1, Hex.Decode("0100000000000000000000351EE786A818F3A1A16B"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x000F28DE File Offset: 0x000F0ADE
		protected override ECCurve CloneCurve()
		{
			return new SecP160R2Curve();
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x000F0641 File Offset: 0x000EE841
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x0600212D RID: 8493 RVA: 0x000F28E5 File Offset: 0x000F0AE5
		public virtual BigInteger Q
		{
			get
			{
				return SecP160R2Curve.q;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x0600212E RID: 8494 RVA: 0x000F28EC File Offset: 0x000F0AEC
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x0600212F RID: 8495 RVA: 0x000F28F4 File Offset: 0x000F0AF4
		public override int FieldSize
		{
			get
			{
				return SecP160R2Curve.q.BitLength;
			}
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x000F14A4 File Offset: 0x000EF6A4
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP160R2FieldElement(x);
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x000F2900 File Offset: 0x000F0B00
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP160R2Point(this, x, y, withCompression);
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x000F290B File Offset: 0x000F0B0B
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP160R2Point(this, x, y, zs, withCompression);
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x000F2918 File Offset: 0x000F0B18
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
			return new SecP160R2Curve.SecP160R2LookupTable(this, array, len);
		}

		// Token: 0x04001A14 RID: 6676
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFAC73"));

		// Token: 0x04001A15 RID: 6677
		private const int SECP160R2_DEFAULT_COORDS = 2;

		// Token: 0x04001A16 RID: 6678
		private const int SECP160R2_FE_INTS = 5;

		// Token: 0x04001A17 RID: 6679
		protected readonly SecP160R2Point m_infinity;

		// Token: 0x0200091F RID: 2335
		private class SecP160R2LookupTable : ECLookupTable
		{
			// Token: 0x06004E6E RID: 20078 RVA: 0x001B1CD9 File Offset: 0x001AFED9
			internal SecP160R2LookupTable(SecP160R2Curve outer, uint[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C36 RID: 3126
			// (get) Token: 0x06004E6F RID: 20079 RVA: 0x001B1CF6 File Offset: 0x001AFEF6
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E70 RID: 20080 RVA: 0x001B1D00 File Offset: 0x001AFF00
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
				return this.m_outer.CreateRawPoint(new SecP160R2FieldElement(array), new SecP160R2FieldElement(array2), false);
			}

			// Token: 0x04003593 RID: 13715
			private readonly SecP160R2Curve m_outer;

			// Token: 0x04003594 RID: 13716
			private readonly uint[] m_table;

			// Token: 0x04003595 RID: 13717
			private readonly int m_size;
		}
	}
}
