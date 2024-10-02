using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000399 RID: 921
	internal class SecT193R2Curve : AbstractF2mCurve
	{
		// Token: 0x060024EC RID: 9452 RVA: 0x00100F3C File Offset: 0x000FF13C
		public SecT193R2Curve() : base(193, 15, 0, 0)
		{
			this.m_infinity = new SecT193R2Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("0163F35A5137C2CE3EA6ED8667190B0BC43ECD69977702709B")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("00C9BB9E8927D4D64C377E2AB2856A5B16E3EFB7F61D4316AE")));
			this.m_order = new BigInteger(1, Hex.Decode("010000000000000000000000015AAB561B005413CCD4EE99D5"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x00100FC6 File Offset: 0x000FF1C6
		protected override ECCurve CloneCurve()
		{
			return new SecT193R2Curve();
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x000FB622 File Offset: 0x000F9822
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060024EF RID: 9455 RVA: 0x00100FCD File Offset: 0x000FF1CD
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060024F0 RID: 9456 RVA: 0x0010059D File Offset: 0x000FE79D
		public override int FieldSize
		{
			get
			{
				return 193;
			}
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x00100875 File Offset: 0x000FEA75
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT193FieldElement(x);
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x00100FD5 File Offset: 0x000FF1D5
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT193R2Point(this, x, y, withCompression);
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x00100FE0 File Offset: 0x000FF1E0
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT193R2Point(this, x, y, zs, withCompression);
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060024F4 RID: 9460 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x060024F5 RID: 9461 RVA: 0x0010059D File Offset: 0x000FE79D
		public virtual int M
		{
			get
			{
				return 193;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x060024F6 RID: 9462 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060024F7 RID: 9463 RVA: 0x00100795 File Offset: 0x000FE995
		public virtual int K1
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060024F8 RID: 9464 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060024F9 RID: 9465 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x00100FF0 File Offset: 0x000FF1F0
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
			return new SecT193R2Curve.SecT193R2LookupTable(this, array, len);
		}

		// Token: 0x04001A9E RID: 6814
		private const int SECT193R2_DEFAULT_COORDS = 6;

		// Token: 0x04001A9F RID: 6815
		private const int SECT193R2_FE_LONGS = 4;

		// Token: 0x04001AA0 RID: 6816
		protected readonly SecT193R2Point m_infinity;

		// Token: 0x02000930 RID: 2352
		private class SecT193R2LookupTable : ECLookupTable
		{
			// Token: 0x06004EA1 RID: 20129 RVA: 0x001B29A1 File Offset: 0x001B0BA1
			internal SecT193R2LookupTable(SecT193R2Curve outer, ulong[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C47 RID: 3143
			// (get) Token: 0x06004EA2 RID: 20130 RVA: 0x001B29BE File Offset: 0x001B0BBE
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004EA3 RID: 20131 RVA: 0x001B29C8 File Offset: 0x001B0BC8
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

			// Token: 0x040035C6 RID: 13766
			private readonly SecT193R2Curve m_outer;

			// Token: 0x040035C7 RID: 13767
			private readonly ulong[] m_table;

			// Token: 0x040035C8 RID: 13768
			private readonly int m_size;
		}
	}
}
