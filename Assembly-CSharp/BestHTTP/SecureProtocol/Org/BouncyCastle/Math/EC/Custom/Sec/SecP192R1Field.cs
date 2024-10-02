using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000366 RID: 870
	internal class SecP192R1Field
	{
		// Token: 0x060021AD RID: 8621 RVA: 0x000F4551 File Offset: 0x000F2751
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat192.Add(x, y, z) != 0U || (z[5] == 4294967295U && Nat192.Gte(z, SecP192R1Field.P)))
			{
				SecP192R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x000F4578 File Offset: 0x000F2778
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if ((Nat.Add(12, xx, yy, zz) != 0U || (zz[11] == 4294967295U && Nat.Gte(12, zz, SecP192R1Field.PExt))) && Nat.AddTo(SecP192R1Field.PExtInv.Length, SecP192R1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(12, zz, SecP192R1Field.PExtInv.Length);
			}
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x000F45CB File Offset: 0x000F27CB
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(6, x, z) != 0U || (z[5] == 4294967295U && Nat192.Gte(z, SecP192R1Field.P)))
			{
				SecP192R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x000F45F0 File Offset: 0x000F27F0
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat192.FromBigInteger(x);
			if (array[5] == 4294967295U && Nat192.Gte(array, SecP192R1Field.P))
			{
				Nat192.SubFrom(SecP192R1Field.P, array);
			}
			return array;
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x000F4624 File Offset: 0x000F2824
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(6, x, 0U, z);
				return;
			}
			uint c = Nat192.Add(x, SecP192R1Field.P, z);
			Nat.ShiftDownBit(6, z, c);
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x000F465C File Offset: 0x000F285C
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat192.CreateExt();
			Nat192.Mul(x, y, array);
			SecP192R1Field.Reduce(array, z);
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x000F4680 File Offset: 0x000F2880
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if ((Nat192.MulAddTo(x, y, zz) != 0U || (zz[11] == 4294967295U && Nat.Gte(12, zz, SecP192R1Field.PExt))) && Nat.AddTo(SecP192R1Field.PExtInv.Length, SecP192R1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(12, zz, SecP192R1Field.PExtInv.Length);
			}
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x000F46D1 File Offset: 0x000F28D1
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat192.IsZero(x))
			{
				Nat192.Zero(z);
				return;
			}
			Nat192.Sub(SecP192R1Field.P, x, z);
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x000F46F0 File Offset: 0x000F28F0
		public static void Reduce(uint[] xx, uint[] z)
		{
			ulong num = (ulong)xx[6];
			ulong num2 = (ulong)xx[7];
			ulong num3 = (ulong)xx[8];
			ulong num4 = (ulong)xx[9];
			ulong num5 = (ulong)xx[10];
			ulong num6 = (ulong)xx[11];
			ulong num7 = num + num5;
			ulong num8 = num2 + num6;
			ulong num9 = 0UL;
			num9 += (ulong)xx[0] + num7;
			uint num10 = (uint)num9;
			num9 >>= 32;
			num9 += (ulong)xx[1] + num8;
			z[1] = (uint)num9;
			num9 >>= 32;
			num7 += num3;
			num8 += num4;
			num9 += (ulong)xx[2] + num7;
			ulong num11 = (ulong)((uint)num9);
			num9 >>= 32;
			num9 += (ulong)xx[3] + num8;
			z[3] = (uint)num9;
			num9 >>= 32;
			num7 -= num;
			num8 -= num2;
			num9 += (ulong)xx[4] + num7;
			z[4] = (uint)num9;
			num9 >>= 32;
			num9 += (ulong)xx[5] + num8;
			z[5] = (uint)num9;
			num9 >>= 32;
			num11 += num9;
			num9 += (ulong)num10;
			z[0] = (uint)num9;
			num9 >>= 32;
			if (num9 != 0UL)
			{
				num9 += (ulong)z[1];
				z[1] = (uint)num9;
				num11 += num9 >> 32;
			}
			z[2] = (uint)num11;
			num9 = num11 >> 32;
			if ((num9 != 0UL && Nat.IncAt(6, z, 3) != 0U) || (z[5] == 4294967295U && Nat192.Gte(z, SecP192R1Field.P)))
			{
				SecP192R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x000F484C File Offset: 0x000F2A4C
		public static void Reduce32(uint x, uint[] z)
		{
			ulong num = 0UL;
			if (x != 0U)
			{
				num += (ulong)z[0] + (ulong)x;
				z[0] = (uint)num;
				num >>= 32;
				if (num != 0UL)
				{
					num += (ulong)z[1];
					z[1] = (uint)num;
					num >>= 32;
				}
				num += (ulong)z[2] + (ulong)x;
				z[2] = (uint)num;
				num >>= 32;
			}
			if ((num != 0UL && Nat.IncAt(6, z, 3) != 0U) || (z[5] == 4294967295U && Nat192.Gte(z, SecP192R1Field.P)))
			{
				SecP192R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x000F48C4 File Offset: 0x000F2AC4
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat192.CreateExt();
			Nat192.Square(x, array);
			SecP192R1Field.Reduce(array, z);
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x000F48E8 File Offset: 0x000F2AE8
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat192.CreateExt();
			Nat192.Square(x, array);
			SecP192R1Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat192.Square(z, array);
				SecP192R1Field.Reduce(array, z);
			}
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x000F4922 File Offset: 0x000F2B22
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat192.Sub(x, y, z) != 0)
			{
				SecP192R1Field.SubPInvFrom(z);
			}
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x000F4934 File Offset: 0x000F2B34
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(12, xx, yy, zz) != 0 && Nat.SubFrom(SecP192R1Field.PExtInv.Length, SecP192R1Field.PExtInv, zz) != 0)
			{
				Nat.DecAt(12, zz, SecP192R1Field.PExtInv.Length);
			}
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x000F4966 File Offset: 0x000F2B66
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(6, x, 0U, z) != 0U || (z[5] == 4294967295U && Nat192.Gte(z, SecP192R1Field.P)))
			{
				SecP192R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x000F498C File Offset: 0x000F2B8C
		private static void AddPInvTo(uint[] z)
		{
			long num = (long)((ulong)z[0] + 1UL);
			z[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num += (long)((ulong)z[1]);
				z[1] = (uint)num;
				num >>= 32;
			}
			num += (long)((ulong)z[2] + 1UL);
			z[2] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				Nat.IncAt(6, z, 3);
			}
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x000F49E0 File Offset: 0x000F2BE0
		private static void SubPInvFrom(uint[] z)
		{
			long num = (long)((ulong)z[0] - 1UL);
			z[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num += (long)((ulong)z[1]);
				z[1] = (uint)num;
				num >>= 32;
			}
			num += (long)((ulong)z[2] - 1UL);
			z[2] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				Nat.DecAt(6, z, 3);
			}
		}

		// Token: 0x04001A30 RID: 6704
		internal static readonly uint[] P = new uint[]
		{
			uint.MaxValue,
			uint.MaxValue,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x04001A31 RID: 6705
		internal static readonly uint[] PExt = new uint[]
		{
			1U,
			0U,
			2U,
			0U,
			1U,
			0U,
			4294967294U,
			uint.MaxValue,
			4294967293U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x04001A32 RID: 6706
		private static readonly uint[] PExtInv = new uint[]
		{
			uint.MaxValue,
			uint.MaxValue,
			4294967293U,
			uint.MaxValue,
			4294967294U,
			uint.MaxValue,
			1U,
			0U,
			2U
		};

		// Token: 0x04001A33 RID: 6707
		private const uint P5 = 4294967295U;

		// Token: 0x04001A34 RID: 6708
		private const uint PExt11 = 4294967295U;
	}
}
