using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000371 RID: 881
	internal class SecP256K1Curve : AbstractFpCurve
	{
		// Token: 0x0600225F RID: 8799 RVA: 0x000F7238 File Offset: 0x000F5438
		public SecP256K1Curve() : base(SecP256K1Curve.q)
		{
			this.m_infinity = new SecP256K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.ValueOf(7L));
			this.m_order = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x000F72AA File Offset: 0x000F54AA
		protected override ECCurve CloneCurve()
		{
			return new SecP256K1Curve();
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x000F0641 File Offset: 0x000EE841
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06002262 RID: 8802 RVA: 0x000F72B1 File Offset: 0x000F54B1
		public virtual BigInteger Q
		{
			get
			{
				return SecP256K1Curve.q;
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06002263 RID: 8803 RVA: 0x000F72B8 File Offset: 0x000F54B8
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06002264 RID: 8804 RVA: 0x000F72C0 File Offset: 0x000F54C0
		public override int FieldSize
		{
			get
			{
				return SecP256K1Curve.q.BitLength;
			}
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x000F72CC File Offset: 0x000F54CC
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP256K1FieldElement(x);
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x000F72D4 File Offset: 0x000F54D4
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP256K1Point(this, x, y, withCompression);
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x000F72DF File Offset: 0x000F54DF
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP256K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x000F72EC File Offset: 0x000F54EC
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			uint[] array = new uint[len * 8 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat256.Copy(((SecP256K1FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 8;
				Nat256.Copy(((SecP256K1FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 8;
			}
			return new SecP256K1Curve.SecP256K1LookupTable(this, array, len);
		}

		// Token: 0x04001A4F RID: 6735
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F"));

		// Token: 0x04001A50 RID: 6736
		private const int SECP256K1_DEFAULT_COORDS = 2;

		// Token: 0x04001A51 RID: 6737
		private const int SECP256K1_FE_INTS = 8;

		// Token: 0x04001A52 RID: 6738
		protected readonly SecP256K1Point m_infinity;

		// Token: 0x02000924 RID: 2340
		private class SecP256K1LookupTable : ECLookupTable
		{
			// Token: 0x06004E7D RID: 20093 RVA: 0x001B2099 File Offset: 0x001B0299
			internal SecP256K1LookupTable(SecP256K1Curve outer, uint[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C3B RID: 3131
			// (get) Token: 0x06004E7E RID: 20094 RVA: 0x001B20B6 File Offset: 0x001B02B6
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E7F RID: 20095 RVA: 0x001B20C0 File Offset: 0x001B02C0
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
				return this.m_outer.CreateRawPoint(new SecP256K1FieldElement(array), new SecP256K1FieldElement(array2), false);
			}

			// Token: 0x040035A2 RID: 13730
			private readonly SecP256K1Curve m_outer;

			// Token: 0x040035A3 RID: 13731
			private readonly uint[] m_table;

			// Token: 0x040035A4 RID: 13732
			private readonly int m_size;
		}
	}
}
