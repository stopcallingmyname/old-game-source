using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200036D RID: 877
	internal class SecP224R1Curve : AbstractFpCurve
	{
		// Token: 0x0600221C RID: 8732 RVA: 0x000F612C File Offset: 0x000F432C
		public SecP224R1Curve() : base(SecP224R1Curve.q)
		{
			this.m_infinity = new SecP224R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFE")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("B4050A850C04B3ABF54132565044B0B7D7BFD8BA270B39432355FFB4")));
			this.m_order = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFF16A2E0B8F03E13DD29455C5C2A3D"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x000F61B2 File Offset: 0x000F43B2
		protected override ECCurve CloneCurve()
		{
			return new SecP224R1Curve();
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x000F0641 File Offset: 0x000EE841
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x0600221F RID: 8735 RVA: 0x000F61B9 File Offset: 0x000F43B9
		public virtual BigInteger Q
		{
			get
			{
				return SecP224R1Curve.q;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06002220 RID: 8736 RVA: 0x000F61C0 File Offset: 0x000F43C0
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06002221 RID: 8737 RVA: 0x000F61C8 File Offset: 0x000F43C8
		public override int FieldSize
		{
			get
			{
				return SecP224R1Curve.q.BitLength;
			}
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x000F61D4 File Offset: 0x000F43D4
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP224R1FieldElement(x);
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x000F61DC File Offset: 0x000F43DC
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP224R1Point(this, x, y, withCompression);
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x000F61E7 File Offset: 0x000F43E7
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP224R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x000F61F4 File Offset: 0x000F43F4
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			uint[] array = new uint[len * 7 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat224.Copy(((SecP224R1FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 7;
				Nat224.Copy(((SecP224R1FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 7;
			}
			return new SecP224R1Curve.SecP224R1LookupTable(this, array, len);
		}

		// Token: 0x04001A44 RID: 6724
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000001"));

		// Token: 0x04001A45 RID: 6725
		private const int SECP224R1_DEFAULT_COORDS = 2;

		// Token: 0x04001A46 RID: 6726
		private const int SECP224R1_FE_INTS = 7;

		// Token: 0x04001A47 RID: 6727
		protected readonly SecP224R1Point m_infinity;

		// Token: 0x02000923 RID: 2339
		private class SecP224R1LookupTable : ECLookupTable
		{
			// Token: 0x06004E7A RID: 20090 RVA: 0x001B1FD9 File Offset: 0x001B01D9
			internal SecP224R1LookupTable(SecP224R1Curve outer, uint[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C3A RID: 3130
			// (get) Token: 0x06004E7B RID: 20091 RVA: 0x001B1FF6 File Offset: 0x001B01F6
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E7C RID: 20092 RVA: 0x001B2000 File Offset: 0x001B0200
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
				return this.m_outer.CreateRawPoint(new SecP224R1FieldElement(array), new SecP224R1FieldElement(array2), false);
			}

			// Token: 0x0400359F RID: 13727
			private readonly SecP224R1Curve m_outer;

			// Token: 0x040035A0 RID: 13728
			private readonly uint[] m_table;

			// Token: 0x040035A1 RID: 13729
			private readonly int m_size;
		}
	}
}
