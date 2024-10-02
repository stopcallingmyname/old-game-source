using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x020003B3 RID: 947
	internal class SecT571K1Curve : AbstractF2mCurve
	{
		// Token: 0x060026C3 RID: 9923 RVA: 0x00107F5C File Offset: 0x0010615C
		public SecT571K1Curve() : base(571, 2, 5, 10)
		{
			this.m_infinity = new SecT571K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.One);
			this.m_order = new BigInteger(1, Hex.Decode("020000000000000000000000000000000000000000000000000000000000000000000000131850E1F19A63E4B391A8DB917F4138B630D84BE5D639381E91DEB45CFE778F637C1001"));
			this.m_cofactor = BigInteger.ValueOf(4L);
			this.m_coord = 6;
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x00107FD2 File Offset: 0x001061D2
		protected override ECCurve CloneCurve()
		{
			return new SecT571K1Curve();
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x000FE921 File Offset: 0x000FCB21
		protected override ECMultiplier CreateDefaultMultiplier()
		{
			return new WTauNafMultiplier();
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x060026C7 RID: 9927 RVA: 0x00107FD9 File Offset: 0x001061D9
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x060026C8 RID: 9928 RVA: 0x00107D1D File Offset: 0x00105F1D
		public override int FieldSize
		{
			get
			{
				return 571;
			}
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x00107FE1 File Offset: 0x001061E1
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT571FieldElement(x);
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x00107FE9 File Offset: 0x001061E9
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT571K1Point(this, x, y, withCompression);
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x00107FF4 File Offset: 0x001061F4
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT571K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x060026CC RID: 9932 RVA: 0x0006AE98 File Offset: 0x00069098
		public override bool IsKoblitz
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x060026CD RID: 9933 RVA: 0x00107D1D File Offset: 0x00105F1D
		public virtual int M
		{
			get
			{
				return 571;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x060026CE RID: 9934 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x060026CF RID: 9935 RVA: 0x000A7398 File Offset: 0x000A5598
		public virtual int K1
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x060026D0 RID: 9936 RVA: 0x000A4E1E File Offset: 0x000A301E
		public virtual int K2
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x060026D1 RID: 9937 RVA: 0x00107F15 File Offset: 0x00106115
		public virtual int K3
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x00108004 File Offset: 0x00106204
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
			return new SecT571K1Curve.SecT571K1LookupTable(this, array, len);
		}

		// Token: 0x04001AC7 RID: 6855
		private const int SECT571K1_DEFAULT_COORDS = 6;

		// Token: 0x04001AC8 RID: 6856
		private const int SECT571K1_FE_LONGS = 9;

		// Token: 0x04001AC9 RID: 6857
		protected readonly SecT571K1Point m_infinity;

		// Token: 0x02000938 RID: 2360
		private class SecT571K1LookupTable : ECLookupTable
		{
			// Token: 0x06004EB9 RID: 20153 RVA: 0x001B2FA2 File Offset: 0x001B11A2
			internal SecT571K1LookupTable(SecT571K1Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C4F RID: 3151
			// (get) Token: 0x06004EBA RID: 20154 RVA: 0x001B2FBF File Offset: 0x001B11BF
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004EBB RID: 20155 RVA: 0x001B2FC8 File Offset: 0x001B11C8
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

			// Token: 0x040035DE RID: 13790
			private readonly SecT571K1Curve m_outer;

			// Token: 0x040035DF RID: 13791
			private readonly ulong[] m_table;

			// Token: 0x040035E0 RID: 13792
			private readonly int m_size;
		}
	}
}
