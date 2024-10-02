using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000397 RID: 919
	internal class SecT193R1Curve : AbstractF2mCurve
	{
		// Token: 0x060024D3 RID: 9427 RVA: 0x001007DC File Offset: 0x000FE9DC
		public SecT193R1Curve() : base(193, 15, 0, 0)
		{
			this.m_infinity = new SecT193R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("0017858FEB7A98975169E171F77B4087DE098AC8A911DF7B01")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("00FDFB49BFE6C3A89FACADAA7A1E5BBC7CC1C2E5D831478814")));
			this.m_order = new BigInteger(1, Hex.Decode("01000000000000000000000000C7F34A778F443ACC920EBA49"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x00100866 File Offset: 0x000FEA66
		protected override ECCurve CloneCurve()
		{
			return new SecT193R1Curve();
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x060024D6 RID: 9430 RVA: 0x0010086D File Offset: 0x000FEA6D
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x060024D7 RID: 9431 RVA: 0x0010059D File Offset: 0x000FE79D
		public override int FieldSize
		{
			get
			{
				return 193;
			}
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x00100875 File Offset: 0x000FEA75
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT193FieldElement(x);
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x0010087D File Offset: 0x000FEA7D
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT193R1Point(this, x, y, withCompression);
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x00100888 File Offset: 0x000FEA88
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT193R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x060024DB RID: 9435 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060024DC RID: 9436 RVA: 0x0010059D File Offset: 0x000FE79D
		public virtual int M
		{
			get
			{
				return 193;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060024DD RID: 9437 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060024DE RID: 9438 RVA: 0x00100795 File Offset: 0x000FE995
		public virtual int K1
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060024DF RID: 9439 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060024E0 RID: 9440 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x00100898 File Offset: 0x000FEA98
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			ulong[] array = new ulong[len * 4 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat256.Copy64(((SecT193FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 4;
				Nat256.Copy64(((SecT193FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 4;
			}
			return new SecT193R1Curve.SecT193R1LookupTable(this, array, len);
		}

		// Token: 0x04001A9B RID: 6811
		private const int SECT193R1_DEFAULT_COORDS = 6;

		// Token: 0x04001A9C RID: 6812
		private const int SECT193R1_FE_LONGS = 4;

		// Token: 0x04001A9D RID: 6813
		protected readonly SecT193R1Point m_infinity;

		// Token: 0x0200092F RID: 2351
		private class SecT193R1LookupTable : ECLookupTable
		{
			// Token: 0x06004E9E RID: 20126 RVA: 0x001B28E1 File Offset: 0x001B0AE1
			internal SecT193R1LookupTable(SecT193R1Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C46 RID: 3142
			// (get) Token: 0x06004E9F RID: 20127 RVA: 0x001B28FE File Offset: 0x001B0AFE
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004EA0 RID: 20128 RVA: 0x001B2908 File Offset: 0x001B0B08
			public virtual ECPoint Lookup(int index)
			{
				ulong[] array = Nat256.Create64();
				ulong[] array2 = Nat256.Create64();
				int num = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					ulong num2 = (ulong)((long)((i ^ index) - 1 >> 31));
					for (int j = 0; j < 4; j++)
					{
						array[j] ^= (this.m_table[num + j] & num2);
						array2[j] ^= (this.m_table[num + 4 + j] & num2);
					}
					num += 8;
				}
				return this.m_outer.CreateRawPoint(new SecT193FieldElement(array), new SecT193FieldElement(array2), false);
			}

			// Token: 0x040035C3 RID: 13763
			private readonly SecT193R1Curve m_outer;

			// Token: 0x040035C4 RID: 13764
			private readonly ulong[] m_table;

			// Token: 0x040035C5 RID: 13765
			private readonly int m_size;
		}
	}
}
