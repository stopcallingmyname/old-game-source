using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200036E RID: 878
	internal class SecP224R1Field
	{
		// Token: 0x06002227 RID: 8743 RVA: 0x000F6275 File Offset: 0x000F4475
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat224.Add(x, y, z) != 0U || (z[6] == 4294967295U && Nat224.Gte(z, SecP224R1Field.P)))
			{
				SecP224R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x000F629C File Offset: 0x000F449C
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if ((Nat.Add(14, xx, yy, zz) != 0U || (zz[13] == 4294967295U && Nat.Gte(14, zz, SecP224R1Field.PExt))) && Nat.AddTo(SecP224R1Field.PExtInv.Length, SecP224R1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(14, zz, SecP224R1Field.PExtInv.Length);
			}
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x000F62EF File Offset: 0x000F44EF
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(7, x, z) != 0U || (z[6] == 4294967295U && Nat224.Gte(z, SecP224R1Field.P)))
			{
				SecP224R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x000F6314 File Offset: 0x000F4514
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat224.FromBigInteger(x);
			if (array[6] == 4294967295U && Nat224.Gte(array, SecP224R1Field.P))
			{
				Nat224.SubFrom(SecP224R1Field.P, array);
			}
			return array;
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x000F6348 File Offset: 0x000F4548
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(7, x, 0U, z);
				return;
			}
			uint c = Nat224.Add(x, SecP224R1Field.P, z);
			Nat.ShiftDownBit(7, z, c);
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x000F6380 File Offset: 0x000F4580
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat224.CreateExt();
			Nat224.Mul(x, y, array);
			SecP224R1Field.Reduce(array, z);
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x000F63A4 File Offset: 0x000F45A4
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if ((Nat224.MulAddTo(x, y, zz) != 0U || (zz[13] == 4294967295U && Nat.Gte(14, zz, SecP224R1Field.PExt))) && Nat.AddTo(SecP224R1Field.PExtInv.Length, SecP224R1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(14, zz, SecP224R1Field.PExtInv.Length);
			}
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x000F63F5 File Offset: 0x000F45F5
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat224.IsZero(x))
			{
				Nat224.Zero(z);
				return;
			}
			Nat224.Sub(SecP224R1Field.P, x, z);
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x000F6414 File Offset: 0x000F4614
		public static void Reduce(uint[] xx, uint[] z)
		{
			long num = (long)((ulong)xx[10]);
			long num2 = (long)((ulong)xx[11]);
			long num3 = (long)((ulong)xx[12]);
			long num4 = (long)((ulong)xx[13]);
			long num5 = (long)((ulong)xx[7] + (ulong)num2 - 1UL);
			long num6 = (long)((ulong)xx[8] + (ulong)num3);
			long num7 = (long)((ulong)xx[9] + (ulong)num4);
			long num8 = 0L;
			num8 += (long)((ulong)xx[0] - (ulong)num5);
			long num9 = (long)((ulong)((uint)num8));
			num8 >>= 32;
			num8 += (long)((ulong)xx[1] - (ulong)num6);
			z[1] = (uint)num8;
			num8 >>= 32;
			num8 += (long)((ulong)xx[2] - (ulong)num7);
			z[2] = (uint)num8;
			num8 >>= 32;
			num8 += (long)((ulong)xx[3] + (ulong)num5 - (ulong)num);
			long num10 = (long)((ulong)((uint)num8));
			num8 >>= 32;
			num8 += (long)((ulong)xx[4] + (ulong)num6 - (ulong)num2);
			z[4] = (uint)num8;
			num8 >>= 32;
			num8 += (long)((ulong)xx[5] + (ulong)num7 - (ulong)num3);
			z[5] = (uint)num8;
			num8 >>= 32;
			num8 += (long)((ulong)xx[6] + (ulong)num - (ulong)num4);
			z[6] = (uint)num8;
			num8 >>= 32;
			num8 += 1L;
			num10 += num8;
			num9 -= num8;
			z[0] = (uint)num9;
			num8 = num9 >> 32;
			if (num8 != 0L)
			{
				num8 += (long)((ulong)z[1]);
				z[1] = (uint)num8;
				num8 >>= 32;
				num8 += (long)((ulong)z[2]);
				z[2] = (uint)num8;
				num10 += num8 >> 32;
			}
			z[3] = (uint)num10;
			num8 = num10 >> 32;
			if ((num8 != 0L && Nat.IncAt(7, z, 4) != 0U) || (z[6] == 4294967295U && Nat224.Gte(z, SecP224R1Field.P)))
			{
				SecP224R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x000F6598 File Offset: 0x000F4798
		public static void Reduce32(uint x, uint[] z)
		{
			long num = 0L;
			if (x != 0U)
			{
				long num2 = (long)((ulong)x);
				num += (long)((ulong)z[0] - (ulong)num2);
				z[0] = (uint)num;
				num >>= 32;
				if (num != 0L)
				{
					num += (long)((ulong)z[1]);
					z[1] = (uint)num;
					num >>= 32;
					num += (long)((ulong)z[2]);
					z[2] = (uint)num;
					num >>= 32;
				}
				num += (long)((ulong)z[3] + (ulong)num2);
				z[3] = (uint)num;
				num >>= 32;
			}
			if ((num != 0L && Nat.IncAt(7, z, 4) != 0U) || (z[6] == 4294967295U && Nat224.Gte(z, SecP224R1Field.P)))
			{
				SecP224R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x000F6620 File Offset: 0x000F4820
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat224.CreateExt();
			Nat224.Square(x, array);
			SecP224R1Field.Reduce(array, z);
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x000F6644 File Offset: 0x000F4844
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat224.CreateExt();
			Nat224.Square(x, array);
			SecP224R1Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat224.Square(z, array);
				SecP224R1Field.Reduce(array, z);
			}
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x000F667E File Offset: 0x000F487E
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat224.Sub(x, y, z) != 0)
			{
				SecP224R1Field.SubPInvFrom(z);
			}
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x000F6690 File Offset: 0x000F4890
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(14, xx, yy, zz) != 0 && Nat.SubFrom(SecP224R1Field.PExtInv.Length, SecP224R1Field.PExtInv, zz) != 0)
			{
				Nat.DecAt(14, zz, SecP224R1Field.PExtInv.Length);
			}
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x000F66C2 File Offset: 0x000F48C2
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(7, x, 0U, z) != 0U || (z[6] == 4294967295U && Nat224.Gte(z, SecP224R1Field.P)))
			{
				SecP224R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x000F66E8 File Offset: 0x000F48E8
		private static void AddPInvTo(uint[] z)
		{
			long num = (long)((ulong)z[0] - 1UL);
			z[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num += (long)((ulong)z[1]);
				z[1] = (uint)num;
				num >>= 32;
				num += (long)((ulong)z[2]);
				z[2] = (uint)num;
				num >>= 32;
			}
			num += (long)((ulong)z[3] + 1UL);
			z[3] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				Nat.IncAt(7, z, 4);
			}
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x000F674C File Offset: 0x000F494C
		private static void SubPInvFrom(uint[] z)
		{
			long num = (long)((ulong)z[0] + 1UL);
			z[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num += (long)((ulong)z[1]);
				z[1] = (uint)num;
				num >>= 32;
				num += (long)((ulong)z[2]);
				z[2] = (uint)num;
				num >>= 32;
			}
			num += (long)((ulong)z[3] - 1UL);
			z[3] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				Nat.DecAt(7, z, 4);
			}
		}

		// Token: 0x04001A48 RID: 6728
		internal static readonly uint[] P = new uint[]
		{
			1U,
			0U,
			0U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x04001A49 RID: 6729
		internal static readonly uint[] PExt = new uint[]
		{
			1U,
			0U,
			0U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			0U,
			2U,
			0U,
			0U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x04001A4A RID: 6730
		private static readonly uint[] PExtInv = new uint[]
		{
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			1U,
			0U,
			0U,
			uint.MaxValue,
			4294967293U,
			uint.MaxValue,
			uint.MaxValue,
			1U
		};

		// Token: 0x04001A4B RID: 6731
		private const uint P6 = 4294967295U;

		// Token: 0x04001A4C RID: 6732
		private const uint PExt13 = 4294967295U;
	}
}
