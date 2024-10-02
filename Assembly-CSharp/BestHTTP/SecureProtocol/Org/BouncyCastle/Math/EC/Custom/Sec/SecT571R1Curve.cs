using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x020003B5 RID: 949
	internal class SecT571R1Curve : AbstractF2mCurve
	{
		// Token: 0x060026DD RID: 9949 RVA: 0x00108678 File Offset: 0x00106878
		public SecT571R1Curve() : base(571, 2, 5, 10)
		{
			this.m_infinity = new SecT571R1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.One);
			this.m_b = SecT571R1Curve.SecT571R1_B;
			this.m_order = new BigInteger(1, Hex.Decode("03FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE661CE18FF55987308059B186823851EC7DD9CA1161DE93D5174D66E8382E9BB2FE84E47"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x001086E6 File Offset: 0x001068E6
		protected override ECCurve CloneCurve()
		{
			return new SecT571R1Curve();
		}

		// Token: 0x060026DF RID: 9951 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x060026E0 RID: 9952 RVA: 0x001086ED File Offset: 0x001068ED
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x060026E1 RID: 9953 RVA: 0x00107D1D File Offset: 0x00105F1D
		public override int FieldSize
		{
			get
			{
				return 571;
			}
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x00107FE1 File Offset: 0x001061E1
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT571FieldElement(x);
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x001086F5 File Offset: 0x001068F5
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT571R1Point(this, x, y, withCompression);
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x00108700 File Offset: 0x00106900
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT571R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x060026E5 RID: 9957 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x060026E6 RID: 9958 RVA: 0x00107D1D File Offset: 0x00105F1D
		public virtual int M
		{
			get
			{
				return 571;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x060026E7 RID: 9959 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x060026E8 RID: 9960 RVA: 0x000A7398 File Offset: 0x000A5598
		public virtual int K1
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x060026E9 RID: 9961 RVA: 0x000A4E1E File Offset: 0x000A301E
		public virtual int K2
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x060026EA RID: 9962 RVA: 0x00107F15 File Offset: 0x00106115
		public virtual int K3
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x00108710 File Offset: 0x00106910
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			ulong[] array = new ulong[len * 9 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat576.Copy64(((SecT571FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 9;
				Nat576.Copy64(((SecT571FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 9;
			}
			return new SecT571R1Curve.SecT571R1LookupTable(this, array, len);
		}

		// Token: 0x04001ACA RID: 6858
		private const int SECT571R1_DEFAULT_COORDS = 6;

		// Token: 0x04001ACB RID: 6859
		private const int SECT571R1_FE_LONGS = 9;

		// Token: 0x04001ACC RID: 6860
		protected readonly SecT571R1Point m_infinity;

		// Token: 0x04001ACD RID: 6861
		internal static readonly SecT571FieldElement SecT571R1_B = new SecT571FieldElement(new BigInteger(1, Hex.Decode("02F40E7E2221F295DE297117B7F3D62F5C6A97FFCB8CEFF1CD6BA8CE4A9A18AD84FFABBD8EFA59332BE7AD6756A66E294AFD185A78FF12AA520E4DE739BACA0C7FFEFF7F2955727A")));

		// Token: 0x04001ACE RID: 6862
		internal static readonly SecT571FieldElement SecT571R1_B_SQRT = (SecT571FieldElement)SecT571R1Curve.SecT571R1_B.Sqrt();

		// Token: 0x02000939 RID: 2361
		private class SecT571R1LookupTable : ECLookupTable
		{
			// Token: 0x06004EBC RID: 20156 RVA: 0x001B3064 File Offset: 0x001B1264
			internal SecT571R1LookupTable(SecT571R1Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C50 RID: 3152
			// (get) Token: 0x06004EBD RID: 20157 RVA: 0x001B3081 File Offset: 0x001B1281
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004EBE RID: 20158 RVA: 0x001B308C File Offset: 0x001B128C
			public virtual ECPoint Lookup(int index)
			{
				ulong[] array = Nat576.Create64();
				ulong[] array2 = Nat576.Create64();
				int num = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					ulong num2 = (ulong)((long)((i ^ index) - 1 >> 31));
					for (int j = 0; j < 9; j++)
					{
						array[j] ^= (this.m_table[num + j] & num2);
						array2[j] ^= (this.m_table[num + 9 + j] & num2);
					}
					num += 18;
				}
				return this.m_outer.CreateRawPoint(new SecT571FieldElement(array), new SecT571FieldElement(array2), false);
			}

			// Token: 0x040035E1 RID: 13793
			private readonly SecT571R1Curve m_outer;

			// Token: 0x040035E2 RID: 13794
			private readonly ulong[] m_table;

			// Token: 0x040035E3 RID: 13795
			private readonly int m_size;
		}
	}
}
