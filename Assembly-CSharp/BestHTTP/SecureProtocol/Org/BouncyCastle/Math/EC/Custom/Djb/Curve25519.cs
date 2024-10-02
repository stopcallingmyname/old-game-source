using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Djb
{
	// Token: 0x020003BB RID: 955
	internal class Curve25519 : AbstractFpCurve
	{
		// Token: 0x06002735 RID: 10037 RVA: 0x00109DD4 File Offset: 0x00107FD4
		public Curve25519() : base(Curve25519.q)
		{
			this.m_infinity = new Curve25519Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("2AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA984914A144")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("7B425ED097B425ED097B425ED097B425ED097B425ED097B4260B5E9C7710C864")));
			this.m_order = new BigInteger(1, Hex.Decode("1000000000000000000000000000000014DEF9DEA2F79CD65812631A5CF5D3ED"));
			this.m_cofactor = BigInteger.ValueOf(8L);
			this.m_coord = 4;
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x00109E5C File Offset: 0x0010805C
		protected override ECCurve CloneCurve()
		{
			return new Curve25519();
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x00109E63 File Offset: 0x00108063
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 4;
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06002738 RID: 10040 RVA: 0x00109E6C File Offset: 0x0010806C
		public virtual BigInteger Q
		{
			get
			{
				return Curve25519.q;
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06002739 RID: 10041 RVA: 0x00109E73 File Offset: 0x00108073
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x0600273A RID: 10042 RVA: 0x00109E7B File Offset: 0x0010807B
		public override int FieldSize
		{
			get
			{
				return Curve25519.q.BitLength;
			}
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x00109E87 File Offset: 0x00108087
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new Curve25519FieldElement(x);
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x00109E8F File Offset: 0x0010808F
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new Curve25519Point(this, x, y, withCompression);
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x00109E9A File Offset: 0x0010809A
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new Curve25519Point(this, x, y, zs, withCompression);
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x00109EA8 File Offset: 0x001080A8
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			uint[] array = new uint[len * 8 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat256.Copy(((Curve25519FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 8;
				Nat256.Copy(((Curve25519FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 8;
			}
			return new Curve25519.Curve25519LookupTable(this, array, len);
		}

		// Token: 0x04001AD9 RID: 6873
		public static readonly BigInteger q = Nat256.ToBigInteger(Curve25519Field.P);

		// Token: 0x04001ADA RID: 6874
		private const int Curve25519_DEFAULT_COORDS = 4;

		// Token: 0x04001ADB RID: 6875
		private const int CURVE25519_FE_INTS = 8;

		// Token: 0x04001ADC RID: 6876
		protected readonly Curve25519Point m_infinity;

		// Token: 0x0200093B RID: 2363
		private class Curve25519LookupTable : ECLookupTable
		{
			// Token: 0x06004EC2 RID: 20162 RVA: 0x001B31E9 File Offset: 0x001B13E9
			internal Curve25519LookupTable(Curve25519 outer, uint[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C52 RID: 3154
			// (get) Token: 0x06004EC3 RID: 20163 RVA: 0x001B3206 File Offset: 0x001B1406
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004EC4 RID: 20164 RVA: 0x001B3210 File Offset: 0x001B1410
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
				return this.m_outer.CreateRawPoint(new Curve25519FieldElement(array), new Curve25519FieldElement(array2), false);
			}

			// Token: 0x040035E7 RID: 13799
			private readonly Curve25519 m_outer;

			// Token: 0x040035E8 RID: 13800
			private readonly uint[] m_table;

			// Token: 0x040035E9 RID: 13801
			private readonly int m_size;
		}
	}
}
