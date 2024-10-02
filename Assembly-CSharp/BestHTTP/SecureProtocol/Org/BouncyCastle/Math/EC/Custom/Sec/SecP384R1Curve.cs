using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000379 RID: 889
	internal class SecP384R1Curve : AbstractFpCurve
	{
		// Token: 0x060022D9 RID: 8921 RVA: 0x000F8FC0 File Offset: 0x000F71C0
		public SecP384R1Curve() : base(SecP384R1Curve.q)
		{
			this.m_infinity = new SecP384R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFF0000000000000000FFFFFFFC")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("B3312FA7E23EE7E4988E056BE3F82D19181D9C6EFE8141120314088F5013875AC656398D8A2ED19D2A85C8EDD3EC2AEF")));
			this.m_order = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC7634D81F4372DDF581A0DB248B0A77AECEC196ACCC52973"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x000F9046 File Offset: 0x000F7246
		protected override ECCurve CloneCurve()
		{
			return new SecP384R1Curve();
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x000F0641 File Offset: 0x000EE841
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x060022DC RID: 8924 RVA: 0x000F904D File Offset: 0x000F724D
		public virtual BigInteger Q
		{
			get
			{
				return SecP384R1Curve.q;
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x060022DD RID: 8925 RVA: 0x000F9054 File Offset: 0x000F7254
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x060022DE RID: 8926 RVA: 0x000F905C File Offset: 0x000F725C
		public override int FieldSize
		{
			get
			{
				return SecP384R1Curve.q.BitLength;
			}
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x000F9068 File Offset: 0x000F7268
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP384R1FieldElement(x);
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x000F9070 File Offset: 0x000F7270
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP384R1Point(this, x, y, withCompression);
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x000F907B File Offset: 0x000F727B
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP384R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x000F9088 File Offset: 0x000F7288
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			uint[] array = new uint[len * 12 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat.Copy(12, ((SecP384R1FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 12;
				Nat.Copy(12, ((SecP384R1FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 12;
			}
			return new SecP384R1Curve.SecP384R1LookupTable(this, array, len);
		}

		// Token: 0x04001A65 RID: 6757
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFF0000000000000000FFFFFFFF"));

		// Token: 0x04001A66 RID: 6758
		private const int SECP384R1_DEFAULT_COORDS = 2;

		// Token: 0x04001A67 RID: 6759
		private const int SECP384R1_FE_INTS = 12;

		// Token: 0x04001A68 RID: 6760
		protected readonly SecP384R1Point m_infinity;

		// Token: 0x02000926 RID: 2342
		private class SecP384R1LookupTable : ECLookupTable
		{
			// Token: 0x06004E83 RID: 20099 RVA: 0x001B2219 File Offset: 0x001B0419
			internal SecP384R1LookupTable(SecP384R1Curve outer, uint[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C3D RID: 3133
			// (get) Token: 0x06004E84 RID: 20100 RVA: 0x001B2236 File Offset: 0x001B0436
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E85 RID: 20101 RVA: 0x001B2240 File Offset: 0x001B0440
			public virtual ECPoint Lookup(int index)
			{
				uint[] array = Nat.Create(12);
				uint[] array2 = Nat.Create(12);
				int num = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					uint num2 = (uint)((i ^ index) - 1 >> 31);
					for (int j = 0; j < 12; j++)
					{
						array[j] ^= (this.m_table[num + j] & num2);
						array2[j] ^= (this.m_table[num + 12 + j] & num2);
					}
					num += 24;
				}
				return this.m_outer.CreateRawPoint(new SecP384R1FieldElement(array), new SecP384R1FieldElement(array2), false);
			}

			// Token: 0x040035A8 RID: 13736
			private readonly SecP384R1Curve m_outer;

			// Token: 0x040035A9 RID: 13737
			private readonly uint[] m_table;

			// Token: 0x040035AA RID: 13738
			private readonly int m_size;
		}
	}
}
