using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000375 RID: 885
	internal class SecP256R1Curve : AbstractFpCurve
	{
		// Token: 0x0600229B RID: 8859 RVA: 0x000F800C File Offset: 0x000F620C
		public SecP256R1Curve() : base(SecP256R1Curve.q)
		{
			this.m_infinity = new SecP256R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFC")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("5AC635D8AA3A93E7B3EBBD55769886BC651D06B0CC53B0F63BCE3C3E27D2604B")));
			this.m_order = new BigInteger(1, Hex.Decode("FFFFFFFF00000000FFFFFFFFFFFFFFFFBCE6FAADA7179E84F3B9CAC2FC632551"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x000F8092 File Offset: 0x000F6292
		protected override ECCurve CloneCurve()
		{
			return new SecP256R1Curve();
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x000F0641 File Offset: 0x000EE841
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x0600229E RID: 8862 RVA: 0x000F8099 File Offset: 0x000F6299
		public virtual BigInteger Q
		{
			get
			{
				return SecP256R1Curve.q;
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600229F RID: 8863 RVA: 0x000F80A0 File Offset: 0x000F62A0
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x060022A0 RID: 8864 RVA: 0x000F80A8 File Offset: 0x000F62A8
		public override int FieldSize
		{
			get
			{
				return SecP256R1Curve.q.BitLength;
			}
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x000F80B4 File Offset: 0x000F62B4
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP256R1FieldElement(x);
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x000F80BC File Offset: 0x000F62BC
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP256R1Point(this, x, y, withCompression);
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x000F80C7 File Offset: 0x000F62C7
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP256R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x000F80D4 File Offset: 0x000F62D4
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			uint[] array = new uint[len * 8 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat256.Copy(((SecP256R1FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 8;
				Nat256.Copy(((SecP256R1FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 8;
			}
			return new SecP256R1Curve.SecP256R1LookupTable(this, array, len);
		}

		// Token: 0x04001A5B RID: 6747
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFF"));

		// Token: 0x04001A5C RID: 6748
		private const int SECP256R1_DEFAULT_COORDS = 2;

		// Token: 0x04001A5D RID: 6749
		private const int SECP256R1_FE_INTS = 8;

		// Token: 0x04001A5E RID: 6750
		protected readonly SecP256R1Point m_infinity;

		// Token: 0x02000925 RID: 2341
		private class SecP256R1LookupTable : ECLookupTable
		{
			// Token: 0x06004E80 RID: 20096 RVA: 0x001B2159 File Offset: 0x001B0359
			internal SecP256R1LookupTable(SecP256R1Curve outer, uint[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C3C RID: 3132
			// (get) Token: 0x06004E81 RID: 20097 RVA: 0x001B2176 File Offset: 0x001B0376
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E82 RID: 20098 RVA: 0x001B2180 File Offset: 0x001B0380
			public virtual ECPoint Lookup(int index)
			{
				uint[] array = Nat256.Create();
				uint[] array2 = Nat256.Create();
				int num = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					uint num2 = (uint)((i ^ index) - 1 >> 31);
					for (int j = 0; j < 8; j++)
					{
						array[j] ^= (this.m_table[num + j] & num2);
						array2[j] ^= (this.m_table[num + 8 + j] & num2);
					}
					num += 16;
				}
				return this.m_outer.CreateRawPoint(new SecP256R1FieldElement(array), new SecP256R1FieldElement(array2), false);
			}

			// Token: 0x040035A5 RID: 13733
			private readonly SecP256R1Curve m_outer;

			// Token: 0x040035A6 RID: 13734
			private readonly uint[] m_table;

			// Token: 0x040035A7 RID: 13735
			private readonly int m_size;
		}
	}
}
