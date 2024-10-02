using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x020003A7 RID: 935
	internal class SecT283K1Curve : AbstractF2mCurve
	{
		// Token: 0x060025F2 RID: 9714 RVA: 0x00104E28 File Offset: 0x00103028
		public SecT283K1Curve() : base(283, 5, 7, 12)
		{
			this.m_infinity = new SecT283K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.One);
			this.m_order = new BigInteger(1, Hex.Decode("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE9AE2ED07577265DFF7F94451E061E163C61"));
			this.m_cofactor = BigInteger.ValueOf(4L);
			this.m_coord = 6;
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x00104E9E File Offset: 0x0010309E
		protected override ECCurve CloneCurve()
		{
			return new SecT283K1Curve();
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x000FE921 File Offset: 0x000FCB21
		protected override ECMultiplier CreateDefaultMultiplier()
		{
			return new WTauNafMultiplier();
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x060025F6 RID: 9718 RVA: 0x00104EA5 File Offset: 0x001030A5
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060025F7 RID: 9719 RVA: 0x00104BE7 File Offset: 0x00102DE7
		public override int FieldSize
		{
			get
			{
				return 283;
			}
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x00104EAD File Offset: 0x001030AD
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT283FieldElement(x);
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x00104EB5 File Offset: 0x001030B5
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT283K1Point(this, x, y, withCompression);
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x00104EC0 File Offset: 0x001030C0
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT283K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x060025FB RID: 9723 RVA: 0x0006AE98 File Offset: 0x00069098
		public override bool IsKoblitz
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x060025FC RID: 9724 RVA: 0x00104BE7 File Offset: 0x00102DE7
		public virtual int M
		{
			get
			{
				return 283;
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x060025FD RID: 9725 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x060025FE RID: 9726 RVA: 0x000A4E1E File Offset: 0x000A301E
		public virtual int K1
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x060025FF RID: 9727 RVA: 0x000FE865 File Offset: 0x000FCA65
		public virtual int K2
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06002600 RID: 9728 RVA: 0x00104DE1 File Offset: 0x00102FE1
		public virtual int K3
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x00104ED0 File Offset: 0x001030D0
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
			return new SecT283K1Curve.SecT283K1LookupTable(this, array, len);
		}

		// Token: 0x04001AB4 RID: 6836
		private const int SECT283K1_DEFAULT_COORDS = 6;

		// Token: 0x04001AB5 RID: 6837
		private const int SECT283K1_FE_LONGS = 5;

		// Token: 0x04001AB6 RID: 6838
		protected readonly SecT283K1Point m_infinity;

		// Token: 0x02000934 RID: 2356
		private class SecT283K1LookupTable : ECLookupTable
		{
			// Token: 0x06004EAD RID: 20141 RVA: 0x001B2CA1 File Offset: 0x001B0EA1
			internal SecT283K1LookupTable(SecT283K1Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C4B RID: 3147
			// (get) Token: 0x06004EAE RID: 20142 RVA: 0x001B2CBE File Offset: 0x001B0EBE
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004EAF RID: 20143 RVA: 0x001B2CC8 File Offset: 0x001B0EC8
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

			// Token: 0x040035D2 RID: 13778
			private readonly SecT283K1Curve m_outer;

			// Token: 0x040035D3 RID: 13779
			private readonly ulong[] m_table;

			// Token: 0x040035D4 RID: 13780
			private readonly int m_size;
		}
	}
}
