using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200037D RID: 893
	internal class SecP521R1Curve : AbstractFpCurve
	{
		// Token: 0x06002316 RID: 8982 RVA: 0x000FA0CC File Offset: 0x000F82CC
		public SecP521R1Curve() : base(SecP521R1Curve.q)
		{
			this.m_infinity = new SecP521R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0051953EB9618E1C9A1F929A21A0B68540EEA2DA725B99B315F3B8B489918EF109E156193951EC7E937B1652C0BD3BB1BF073573DF883D2C34F1EF451FD46B503F00")));
			this.m_order = new BigInteger(1, Hex.Decode("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFA51868783BF2F966B7FCC0148F709A5D03BB5C9B8899C47AEBB6FB71E91386409"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x000FA152 File Offset: 0x000F8352
		protected override ECCurve CloneCurve()
		{
			return new SecP521R1Curve();
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x000F0641 File Offset: 0x000EE841
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06002319 RID: 8985 RVA: 0x000FA159 File Offset: 0x000F8359
		public virtual BigInteger Q
		{
			get
			{
				return SecP521R1Curve.q;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x0600231A RID: 8986 RVA: 0x000FA160 File Offset: 0x000F8360
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x0600231B RID: 8987 RVA: 0x000FA168 File Offset: 0x000F8368
		public override int FieldSize
		{
			get
			{
				return SecP521R1Curve.q.BitLength;
			}
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x000FA174 File Offset: 0x000F8374
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP521R1FieldElement(x);
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x000FA17C File Offset: 0x000F837C
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP521R1Point(this, x, y, withCompression);
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x000FA187 File Offset: 0x000F8387
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP521R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x000FA194 File Offset: 0x000F8394
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			uint[] array = new uint[len * 17 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat.Copy(17, ((SecP521R1FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 17;
				Nat.Copy(17, ((SecP521R1FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 17;
			}
			return new SecP521R1Curve.SecP521R1LookupTable(this, array, len);
		}

		// Token: 0x04001A70 RID: 6768
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"));

		// Token: 0x04001A71 RID: 6769
		private const int SECP521R1_DEFAULT_COORDS = 2;

		// Token: 0x04001A72 RID: 6770
		private const int SECP521R1_FE_INTS = 17;

		// Token: 0x04001A73 RID: 6771
		protected readonly SecP521R1Point m_infinity;

		// Token: 0x02000927 RID: 2343
		private class SecP521R1LookupTable : ECLookupTable
		{
			// Token: 0x06004E86 RID: 20102 RVA: 0x001B22DF File Offset: 0x001B04DF
			internal SecP521R1LookupTable(SecP521R1Curve outer, uint[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C3E RID: 3134
			// (get) Token: 0x06004E87 RID: 20103 RVA: 0x001B22FC File Offset: 0x001B04FC
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E88 RID: 20104 RVA: 0x001B2304 File Offset: 0x001B0504
			public virtual ECPoint Lookup(int index)
			{
				uint[] array = Nat.Create(17);
				uint[] array2 = Nat.Create(17);
				int num = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					uint num2 = (uint)((i ^ index) - 1 >> 31);
					for (int j = 0; j < 17; j++)
					{
						array[j] ^= (this.m_table[num + j] & num2);
						array2[j] ^= (this.m_table[num + 17 + j] & num2);
					}
					num += 34;
				}
				return this.m_outer.CreateRawPoint(new SecP521R1FieldElement(array), new SecP521R1FieldElement(array2), false);
			}

			// Token: 0x040035AB RID: 13739
			private readonly SecP521R1Curve m_outer;

			// Token: 0x040035AC RID: 13740
			private readonly uint[] m_table;

			// Token: 0x040035AD RID: 13741
			private readonly int m_size;
		}
	}
}
