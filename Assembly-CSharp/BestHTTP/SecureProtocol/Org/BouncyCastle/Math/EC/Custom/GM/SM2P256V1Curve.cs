using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.GM
{
	// Token: 0x020003B7 RID: 951
	internal class SM2P256V1Curve : AbstractFpCurve
	{
		// Token: 0x060026F7 RID: 9975 RVA: 0x00108D94 File Offset: 0x00106F94
		public SM2P256V1Curve() : base(SM2P256V1Curve.q)
		{
			this.m_infinity = new SM2P256V1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFC")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("28E9FA9E9D9F5E344D5A9E4BCF6509A7F39789F515AB8F92DDBCBD414D940E93")));
			this.m_order = new BigInteger(1, Hex.Decode("FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFF7203DF6B21C6052B53BBF40939D54123"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x00108E1A File Offset: 0x0010701A
		protected override ECCurve CloneCurve()
		{
			return new SM2P256V1Curve();
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x000F0641 File Offset: 0x000EE841
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x060026FA RID: 9978 RVA: 0x00108E21 File Offset: 0x00107021
		public virtual BigInteger Q
		{
			get
			{
				return SM2P256V1Curve.q;
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x060026FB RID: 9979 RVA: 0x00108E28 File Offset: 0x00107028
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x060026FC RID: 9980 RVA: 0x00108E30 File Offset: 0x00107030
		public override int FieldSize
		{
			get
			{
				return SM2P256V1Curve.q.BitLength;
			}
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x00108E3C File Offset: 0x0010703C
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SM2P256V1FieldElement(x);
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x00108E44 File Offset: 0x00107044
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SM2P256V1Point(this, x, y, withCompression);
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x00108E4F File Offset: 0x0010704F
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SM2P256V1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x00108E5C File Offset: 0x0010705C
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			uint[] array = new uint[len * 8 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat256.Copy(((SM2P256V1FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 8;
				Nat256.Copy(((SM2P256V1FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 8;
			}
			return new SM2P256V1Curve.SM2P256V1LookupTable(this, array, len);
		}

		// Token: 0x04001ACF RID: 6863
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFF"));

		// Token: 0x04001AD0 RID: 6864
		private const int SM2P256V1_DEFAULT_COORDS = 2;

		// Token: 0x04001AD1 RID: 6865
		private const int SM2P256V1_FE_INTS = 8;

		// Token: 0x04001AD2 RID: 6866
		protected readonly SM2P256V1Point m_infinity;

		// Token: 0x0200093A RID: 2362
		private class SM2P256V1LookupTable : ECLookupTable
		{
			// Token: 0x06004EBF RID: 20159 RVA: 0x001B3128 File Offset: 0x001B1328
			internal SM2P256V1LookupTable(SM2P256V1Curve outer, uint[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C51 RID: 3153
			// (get) Token: 0x06004EC0 RID: 20160 RVA: 0x001B3145 File Offset: 0x001B1345
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004EC1 RID: 20161 RVA: 0x001B3150 File Offset: 0x001B1350
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
				return this.m_outer.CreateRawPoint(new SM2P256V1FieldElement(array), new SM2P256V1FieldElement(array2), false);
			}

			// Token: 0x040035E4 RID: 13796
			private readonly SM2P256V1Curve m_outer;

			// Token: 0x040035E5 RID: 13797
			private readonly uint[] m_table;

			// Token: 0x040035E6 RID: 13798
			private readonly int m_size;
		}
	}
}
