using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x020003A3 RID: 931
	internal class SecT239K1Curve : AbstractF2mCurve
	{
		// Token: 0x060025A2 RID: 9634 RVA: 0x00103AD4 File Offset: 0x00101CD4
		public SecT239K1Curve() : base(239, 158, 0, 0)
		{
			this.m_infinity = new SecT239K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.One);
			this.m_order = new BigInteger(1, Hex.Decode("2000000000000000000000000000005A79FEC67CB6E91F1C1DA800E478A5"));
			this.m_cofactor = BigInteger.ValueOf(4L);
			this.m_coord = 6;
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x00103B4D File Offset: 0x00101D4D
		protected override ECCurve CloneCurve()
		{
			return new SecT239K1Curve();
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x000FE921 File Offset: 0x000FCB21
		protected override ECMultiplier CreateDefaultMultiplier()
		{
			return new WTauNafMultiplier();
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x060025A6 RID: 9638 RVA: 0x00103B54 File Offset: 0x00101D54
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x060025A7 RID: 9639 RVA: 0x00103891 File Offset: 0x00101A91
		public override int FieldSize
		{
			get
			{
				return 239;
			}
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x00103B5C File Offset: 0x00101D5C
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT239FieldElement(x);
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x00103B64 File Offset: 0x00101D64
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT239K1Point(this, x, y, withCompression);
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x00103B6F File Offset: 0x00101D6F
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT239K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x060025AB RID: 9643 RVA: 0x0006AE98 File Offset: 0x00069098
		public override bool IsKoblitz
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x060025AC RID: 9644 RVA: 0x00103891 File Offset: 0x00101A91
		public virtual int M
		{
			get
			{
				return 239;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x060025AD RID: 9645 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x060025AE RID: 9646 RVA: 0x00103A89 File Offset: 0x00101C89
		public virtual int K1
		{
			get
			{
				return 158;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x060025AF RID: 9647 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x060025B0 RID: 9648 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x00103B7C File Offset: 0x00101D7C
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			ulong[] array = new ulong[len * 4 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat256.Copy64(((SecT239FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 4;
				Nat256.Copy64(((SecT239FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 4;
			}
			return new SecT239K1Curve.SecT239K1LookupTable(this, array, len);
		}

		// Token: 0x04001AAD RID: 6829
		private const int SECT239K1_DEFAULT_COORDS = 6;

		// Token: 0x04001AAE RID: 6830
		private const int SECT239K1_FE_LONGS = 4;

		// Token: 0x04001AAF RID: 6831
		protected readonly SecT239K1Point m_infinity;

		// Token: 0x02000933 RID: 2355
		private class SecT239K1LookupTable : ECLookupTable
		{
			// Token: 0x06004EAA RID: 20138 RVA: 0x001B2BE1 File Offset: 0x001B0DE1
			internal SecT239K1LookupTable(SecT239K1Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C4A RID: 3146
			// (get) Token: 0x06004EAB RID: 20139 RVA: 0x001B2BFE File Offset: 0x001B0DFE
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004EAC RID: 20140 RVA: 0x001B2C08 File Offset: 0x001B0E08
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
				return this.m_outer.CreateRawPoint(new SecT239FieldElement(array), new SecT239FieldElement(array2), false);
			}

			// Token: 0x040035CF RID: 13775
			private readonly SecT239K1Curve m_outer;

			// Token: 0x040035D0 RID: 13776
			private readonly ulong[] m_table;

			// Token: 0x040035D1 RID: 13777
			private readonly int m_size;
		}
	}
}
