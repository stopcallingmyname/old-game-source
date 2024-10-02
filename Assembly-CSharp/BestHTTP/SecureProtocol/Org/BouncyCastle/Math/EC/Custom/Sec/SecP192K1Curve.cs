using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000361 RID: 865
	internal class SecP192K1Curve : AbstractFpCurve
	{
		// Token: 0x06002166 RID: 8550 RVA: 0x000F3664 File Offset: 0x000F1864
		public SecP192K1Curve() : base(SecP192K1Curve.q)
		{
			this.m_infinity = new SecP192K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.ValueOf(3L));
			this.m_order = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFE26F2FC170F69466A74DEFD8D"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x000F36D6 File Offset: 0x000F18D6
		protected override ECCurve CloneCurve()
		{
			return new SecP192K1Curve();
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x000F0641 File Offset: 0x000EE841
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06002169 RID: 8553 RVA: 0x000F36DD File Offset: 0x000F18DD
		public virtual BigInteger Q
		{
			get
			{
				return SecP192K1Curve.q;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x0600216A RID: 8554 RVA: 0x000F36E4 File Offset: 0x000F18E4
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x0600216B RID: 8555 RVA: 0x000F36EC File Offset: 0x000F18EC
		public override int FieldSize
		{
			get
			{
				return SecP192K1Curve.q.BitLength;
			}
		}

		// Token: 0x0600216C RID: 8556 RVA: 0x000F36F8 File Offset: 0x000F18F8
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP192K1FieldElement(x);
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x000F3700 File Offset: 0x000F1900
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP192K1Point(this, x, y, withCompression);
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x000F370B File Offset: 0x000F190B
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP192K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x000F3718 File Offset: 0x000F1918
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			uint[] array = new uint[len * 6 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat192.Copy(((SecP192K1FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 6;
				Nat192.Copy(((SecP192K1FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 6;
			}
			return new SecP192K1Curve.SecP192K1LookupTable(this, array, len);
		}

		// Token: 0x04001A20 RID: 6688
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFEE37"));

		// Token: 0x04001A21 RID: 6689
		private const int SECP192K1_DEFAULT_COORDS = 2;

		// Token: 0x04001A22 RID: 6690
		private const int SECP192K1_FE_INTS = 6;

		// Token: 0x04001A23 RID: 6691
		protected readonly SecP192K1Point m_infinity;

		// Token: 0x02000920 RID: 2336
		private class SecP192K1LookupTable : ECLookupTable
		{
			// Token: 0x06004E71 RID: 20081 RVA: 0x001B1D99 File Offset: 0x001AFF99
			internal SecP192K1LookupTable(SecP192K1Curve outer, uint[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C37 RID: 3127
			// (get) Token: 0x06004E72 RID: 20082 RVA: 0x001B1DB6 File Offset: 0x001AFFB6
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E73 RID: 20083 RVA: 0x001B1DC0 File Offset: 0x001AFFC0
			public virtual ECPoint Lookup(int index)
			{
				uint[] array = Nat192.Create();
				uint[] array2 = Nat192.Create();
				int num = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					uint num2 = (uint)((i ^ index) - 1 >> 31);
					for (int j = 0; j < 6; j++)
					{
						array[j] ^= (this.m_table[num + j] & num2);
						array2[j] ^= (this.m_table[num + 6 + j] & num2);
					}
					num += 12;
				}
				return this.m_outer.CreateRawPoint(new SecP192K1FieldElement(array), new SecP192K1FieldElement(array2), false);
			}

			// Token: 0x04003596 RID: 13718
			private readonly SecP192K1Curve m_outer;

			// Token: 0x04003597 RID: 13719
			private readonly uint[] m_table;

			// Token: 0x04003598 RID: 13720
			private readonly int m_size;
		}
	}
}
