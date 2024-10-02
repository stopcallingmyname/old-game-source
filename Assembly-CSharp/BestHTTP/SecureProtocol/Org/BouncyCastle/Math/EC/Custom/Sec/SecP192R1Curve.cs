using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000365 RID: 869
	internal class SecP192R1Curve : AbstractFpCurve
	{
		// Token: 0x060021A2 RID: 8610 RVA: 0x000F4408 File Offset: 0x000F2608
		public SecP192R1Curve() : base(SecP192R1Curve.q)
		{
			this.m_infinity = new SecP192R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFFFFFFFC")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("64210519E59C80E70FA7E9AB72243049FEB8DEECC146B9B1")));
			this.m_order = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFF99DEF836146BC9B1B4D22831"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x000F448E File Offset: 0x000F268E
		protected override ECCurve CloneCurve()
		{
			return new SecP192R1Curve();
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x000F0641 File Offset: 0x000EE841
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060021A5 RID: 8613 RVA: 0x000F4495 File Offset: 0x000F2695
		public virtual BigInteger Q
		{
			get
			{
				return SecP192R1Curve.q;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060021A6 RID: 8614 RVA: 0x000F449C File Offset: 0x000F269C
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060021A7 RID: 8615 RVA: 0x000F44A4 File Offset: 0x000F26A4
		public override int FieldSize
		{
			get
			{
				return SecP192R1Curve.q.BitLength;
			}
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x000F44B0 File Offset: 0x000F26B0
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP192R1FieldElement(x);
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x000F44B8 File Offset: 0x000F26B8
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP192R1Point(this, x, y, withCompression);
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x000F44C3 File Offset: 0x000F26C3
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP192R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x000F44D0 File Offset: 0x000F26D0
		public override ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			uint[] array = new uint[len * 6 * 2];
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				Nat192.Copy(((SecP192R1FieldElement)ecpoint.RawXCoord).x, 0, array, num);
				num += 6;
				Nat192.Copy(((SecP192R1FieldElement)ecpoint.RawYCoord).x, 0, array, num);
				num += 6;
			}
			return new SecP192R1Curve.SecP192R1LookupTable(this, array, len);
		}

		// Token: 0x04001A2C RID: 6700
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFFFFFFFF"));

		// Token: 0x04001A2D RID: 6701
		private const int SECP192R1_DEFAULT_COORDS = 2;

		// Token: 0x04001A2E RID: 6702
		private const int SECP192R1_FE_INTS = 6;

		// Token: 0x04001A2F RID: 6703
		protected readonly SecP192R1Point m_infinity;

		// Token: 0x02000921 RID: 2337
		private class SecP192R1LookupTable : ECLookupTable
		{
			// Token: 0x06004E74 RID: 20084 RVA: 0x001B1E59 File Offset: 0x001B0059
			internal SecP192R1LookupTable(SecP192R1Curve outer, uint[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C38 RID: 3128
			// (get) Token: 0x06004E75 RID: 20085 RVA: 0x001B1E76 File Offset: 0x001B0076
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E76 RID: 20086 RVA: 0x001B1E80 File Offset: 0x001B0080
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
				return this.m_outer.CreateRawPoint(new SecP192R1FieldElement(array), new SecP192R1FieldElement(array2), false);
			}

			// Token: 0x04003599 RID: 13721
			private readonly SecP192R1Curve m_outer;

			// Token: 0x0400359A RID: 13722
			private readonly uint[] m_table;

			// Token: 0x0400359B RID: 13723
			private readonly int m_size;
		}
	}
}
