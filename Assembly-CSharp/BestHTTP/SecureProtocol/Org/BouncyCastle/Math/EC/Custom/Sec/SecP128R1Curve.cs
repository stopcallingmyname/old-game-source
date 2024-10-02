using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000353 RID: 851
	internal class SecP128R1Curve : AbstractFpCurve
	{
		// Token: 0x0600209C RID: 8348 RVA: 0x000F05B4 File Offset: 0x000EE7B4
		public SecP128R1Curve() : base(SecP128R1Curve.q)
		{
			this.m_infinity = new SecP128R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFDFFFFFFFFFFFFFFFFFFFFFFFC")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("E87579C11079F43DD824993C2CEE5ED3")));
			this.m_order = new BigInteger(1, Hex.Decode("FFFFFFFE0000000075A30D1B9038A115"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x000F063A File Offset: 0x000EE83A
		protected override ECCurve CloneCurve()
		{
			return new SecP128R1Curve();
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x000F0641 File Offset: 0x000EE841
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x0600209F RID: 8351 RVA: 0x000F064A File Offset: 0x000EE84A
		public virtual BigInteger Q
		{
			get
			{
				return SecP128R1Curve.q;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x060020A0 RID: 8352 RVA: 0x000F0651 File Offset: 0x000EE851
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x060020A1 RID: 8353 RVA: 0x000F0659 File Offset: 0x000EE859
		public override int FieldSize
		{
			get
			{
				return SecP128R1Curve.q.BitLength;
			}
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x000F0665 File Offset: 0x000EE865
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP128R1FieldElement(x);
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x000F066D File Offset: 0x000EE86D
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP128R1Point(this, x, y, withCompression);
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x000F0678 File Offset: 0x000EE878
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP128R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x000F0688 File Offset: 0x000EE888
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			uint[] array = new uint[len * 4 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat128.Copy(((SecP128R1FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 4;
				Nat128.Copy(((SecP128R1FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 4;
			}
			return new SecP128R1Curve.SecP128R1LookupTable(this, array, len);
		}

		// Token: 0x040019F9 RID: 6649
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFDFFFFFFFFFFFFFFFFFFFFFFFF"));

		// Token: 0x040019FA RID: 6650
		private const int SECP128R1_DEFAULT_COORDS = 2;

		// Token: 0x040019FB RID: 6651
		private const int SECP128R1_FE_INTS = 4;

		// Token: 0x040019FC RID: 6652
		protected readonly SecP128R1Point m_infinity;

		// Token: 0x0200091C RID: 2332
		private class SecP128R1LookupTable : ECLookupTable
		{
			// Token: 0x06004E65 RID: 20069 RVA: 0x001B1A98 File Offset: 0x001AFC98
			internal SecP128R1LookupTable(SecP128R1Curve outer, uint[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C33 RID: 3123
			// (get) Token: 0x06004E66 RID: 20070 RVA: 0x001B1AB5 File Offset: 0x001AFCB5
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E67 RID: 20071 RVA: 0x001B1AC0 File Offset: 0x001AFCC0
			public virtual ECPoint Lookup(int index)
			{
				uint[] array = Nat128.Create();
				uint[] array2 = Nat128.Create();
				int num = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					uint num2 = (uint)((i ^ index) - 1 >> 31);
					for (int j = 0; j < 4; j++)
					{
						array[j] ^= (this.m_table[num + j] & num2);
						array2[j] ^= (this.m_table[num + 4 + j] & num2);
					}
					num += 8;
				}
				return this.m_outer.CreateRawPoint(new SecP128R1FieldElement(array), new SecP128R1FieldElement(array2), false);
			}

			// Token: 0x0400358A RID: 13706
			private readonly SecP128R1Curve m_outer;

			// Token: 0x0400358B RID: 13707
			private readonly uint[] m_table;

			// Token: 0x0400358C RID: 13708
			private readonly int m_size;
		}
	}
}
