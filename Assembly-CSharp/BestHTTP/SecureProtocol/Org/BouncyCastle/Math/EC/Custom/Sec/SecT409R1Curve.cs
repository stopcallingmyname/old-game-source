using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x020003AF RID: 943
	internal class SecT409R1Curve : AbstractF2mCurve
	{
		// Token: 0x06002674 RID: 9844 RVA: 0x00106E74 File Offset: 0x00105074
		public SecT409R1Curve() : base(409, 87, 0, 0)
		{
			this.m_infinity = new SecT409R1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.One);
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0021A5C2C8EE9FEB5C4B9A753B7B476B7FD6422EF1F3DD674761FA99D6AC27C8A9A197B272822F6CD57A55AA4F50AE317B13545F")));
			this.m_order = new BigInteger(1, Hex.Decode("010000000000000000000000000000000000000000000000000001E2AAD6A612F33307BE5FA47C3C9E052F838164CD37D9A21173"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x00106EF3 File Offset: 0x001050F3
		protected override ECCurve CloneCurve()
		{
			return new SecT409R1Curve();
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06002677 RID: 9847 RVA: 0x00106EFA File Offset: 0x001050FA
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06002678 RID: 9848 RVA: 0x0010651C File Offset: 0x0010471C
		public override int FieldSize
		{
			get
			{
				return 409;
			}
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x001067E1 File Offset: 0x001049E1
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT409FieldElement(x);
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x00106F02 File Offset: 0x00105102
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT409R1Point(this, x, y, withCompression);
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x00106F0D File Offset: 0x0010510D
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT409R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x0600267C RID: 9852 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x0600267D RID: 9853 RVA: 0x0010651C File Offset: 0x0010471C
		public virtual int M
		{
			get
			{
				return 409;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x0600267E RID: 9854 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x0600267F RID: 9855 RVA: 0x00106715 File Offset: 0x00104915
		public virtual int K1
		{
			get
			{
				return 87;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06002680 RID: 9856 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06002681 RID: 9857 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x00106F1C File Offset: 0x0010511C
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			ulong[] array = new ulong[len * 7 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat448.Copy64(((SecT409FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 7;
				Nat448.Copy64(((SecT409FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 7;
			}
			return new SecT409R1Curve.SecT409R1LookupTable(this, array, len);
		}

		// Token: 0x04001AC0 RID: 6848
		private const int SECT409R1_DEFAULT_COORDS = 6;

		// Token: 0x04001AC1 RID: 6849
		private const int SECT409R1_FE_LONGS = 7;

		// Token: 0x04001AC2 RID: 6850
		protected readonly SecT409R1Point m_infinity;

		// Token: 0x02000937 RID: 2359
		private class SecT409R1LookupTable : ECLookupTable
		{
			// Token: 0x06004EB6 RID: 20150 RVA: 0x001B2EE2 File Offset: 0x001B10E2
			internal SecT409R1LookupTable(SecT409R1Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C4E RID: 3150
			// (get) Token: 0x06004EB7 RID: 20151 RVA: 0x001B2EFF File Offset: 0x001B10FF
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004EB8 RID: 20152 RVA: 0x001B2F08 File Offset: 0x001B1108
			public virtual ECPoint Lookup(int index)
			{
				ulong[] array = Nat448.Create64();
				ulong[] array2 = Nat448.Create64();
				int num = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					ulong num2 = (ulong)((long)((i ^ index) - 1 >> 31));
					for (int j = 0; j < 7; j++)
					{
						array[j] ^= (this.m_table[num + j] & num2);
						array2[j] ^= (this.m_table[num + 7 + j] & num2);
					}
					num += 14;
				}
				return this.m_outer.CreateRawPoint(new SecT409FieldElement(array), new SecT409FieldElement(array2), false);
			}

			// Token: 0x040035DB RID: 13787
			private readonly SecT409R1Curve m_outer;

			// Token: 0x040035DC RID: 13788
			private readonly ulong[] m_table;

			// Token: 0x040035DD RID: 13789
			private readonly int m_size;
		}
	}
}
