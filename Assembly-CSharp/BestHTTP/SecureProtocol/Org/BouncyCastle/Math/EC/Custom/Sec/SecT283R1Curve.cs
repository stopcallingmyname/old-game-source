using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x020003A9 RID: 937
	internal class SecT283R1Curve : AbstractF2mCurve
	{
		// Token: 0x0600260C RID: 9740 RVA: 0x00105540 File Offset: 0x00103740
		public SecT283R1Curve() : base(283, 5, 7, 12)
		{
			this.m_infinity = new SecT283R1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.One);
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("027B680AC8B8596DA5A4AF8A19A0303FCA97FD7645309FA2A581485AF6263E313B79A2F5")));
			this.m_order = new BigInteger(1, Hex.Decode("03FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEF90399660FC938A90165B042A7CEFADB307"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x001055BF File Offset: 0x001037BF
		protected override ECCurve CloneCurve()
		{
			return new SecT283R1Curve();
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x0600260F RID: 9743 RVA: 0x001055C6 File Offset: 0x001037C6
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06002610 RID: 9744 RVA: 0x00104BE7 File Offset: 0x00102DE7
		public override int FieldSize
		{
			get
			{
				return 283;
			}
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x00104EAD File Offset: 0x001030AD
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT283FieldElement(x);
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x001055CE File Offset: 0x001037CE
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT283R1Point(this, x, y, withCompression);
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x001055D9 File Offset: 0x001037D9
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT283R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06002614 RID: 9748 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06002615 RID: 9749 RVA: 0x00104BE7 File Offset: 0x00102DE7
		public virtual int M
		{
			get
			{
				return 283;
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06002616 RID: 9750 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06002617 RID: 9751 RVA: 0x000A4E1E File Offset: 0x000A301E
		public virtual int K1
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06002618 RID: 9752 RVA: 0x000FE865 File Offset: 0x000FCA65
		public virtual int K2
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06002619 RID: 9753 RVA: 0x00104DE1 File Offset: 0x00102FE1
		public virtual int K3
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x001055E8 File Offset: 0x001037E8
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			ulong[] array = new ulong[len * 5 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat320.Copy64(((SecT283FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 5;
				Nat320.Copy64(((SecT283FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 5;
			}
			return new SecT283R1Curve.SecT283R1LookupTable(this, array, len);
		}

		// Token: 0x04001AB7 RID: 6839
		private const int SECT283R1_DEFAULT_COORDS = 6;

		// Token: 0x04001AB8 RID: 6840
		private const int SECT283R1_FE_LONGS = 5;

		// Token: 0x04001AB9 RID: 6841
		protected readonly SecT283R1Point m_infinity;

		// Token: 0x02000935 RID: 2357
		private class SecT283R1LookupTable : ECLookupTable
		{
			// Token: 0x06004EB0 RID: 20144 RVA: 0x001B2D62 File Offset: 0x001B0F62
			internal SecT283R1LookupTable(SecT283R1Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C4C RID: 3148
			// (get) Token: 0x06004EB1 RID: 20145 RVA: 0x001B2D7F File Offset: 0x001B0F7F
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004EB2 RID: 20146 RVA: 0x001B2D88 File Offset: 0x001B0F88
			public virtual ECPoint Lookup(int index)
			{
				ulong[] array = Nat320.Create64();
				ulong[] array2 = Nat320.Create64();
				int num = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					ulong num2 = (ulong)((long)((i ^ index) - 1 >> 31));
					for (int j = 0; j < 5; j++)
					{
						array[j] ^= (this.m_table[num + j] & num2);
						array2[j] ^= (this.m_table[num + 5 + j] & num2);
					}
					num += 10;
				}
				return this.m_outer.CreateRawPoint(new SecT283FieldElement(array), new SecT283FieldElement(array2), false);
			}

			// Token: 0x040035D5 RID: 13781
			private readonly SecT283R1Curve m_outer;

			// Token: 0x040035D6 RID: 13782
			private readonly ulong[] m_table;

			// Token: 0x040035D7 RID: 13783
			private readonly int m_size;
		}
	}
}
