using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200035A RID: 858
	internal class SecP160R1Field
	{
		// Token: 0x060020F9 RID: 8441 RVA: 0x000F1BA9 File Offset: 0x000EFDA9
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat160.Add(x, y, z) != 0U || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R1Field.P)))
			{
				Nat.AddWordTo(5, 2147483649U, z);
			}
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x000F1BD8 File Offset: 0x000EFDD8
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if ((Nat.Add(10, xx, yy, zz) != 0U || (zz[9] == 4294967295U && Nat.Gte(10, zz, SecP160R1Field.PExt))) && Nat.AddTo(SecP160R1Field.PExtInv.Length, SecP160R1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(10, zz, SecP160R1Field.PExtInv.Length);
			}
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x000F1C2B File Offset: 0x000EFE2B
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(5, x, z) != 0U || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R1Field.P)))
			{
				Nat.AddWordTo(5, 2147483649U, z);
			}
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x000F1C58 File Offset: 0x000EFE58
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat160.FromBigInteger(x);
			if (array[4] == 4294967295U && Nat160.Gte(array, SecP160R1Field.P))
			{
				Nat160.SubFrom(SecP160R1Field.P, array);
			}
			return array;
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x000F1C8C File Offset: 0x000EFE8C
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(5, x, 0U, z);
				return;
			}
			uint c = Nat160.Add(x, SecP160R1Field.P, z);
			Nat.ShiftDownBit(5, z, c);
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x000F1CC4 File Offset: 0x000EFEC4
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat160.CreateExt();
			Nat160.Mul(x, y, array);
			SecP160R1Field.Reduce(array, z);
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x000F1CE8 File Offset: 0x000EFEE8
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if ((Nat160.MulAddTo(x, y, zz) != 0U || (zz[9] == 4294967295U && Nat.Gte(10, zz, SecP160R1Field.PExt))) && Nat.AddTo(SecP160R1Field.PExtInv.Length, SecP160R1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(10, zz, SecP160R1Field.PExtInv.Length);
			}
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x000F1D39 File Offset: 0x000EFF39
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat160.IsZero(x))
			{
				Nat160.Zero(z);
				return;
			}
			Nat160.Sub(SecP160R1Field.P, x, z);
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x000F1D58 File Offset: 0x000EFF58
		public static void Reduce(uint[] xx, uint[] z)
		{
			ulong num = (ulong)xx[5];
			ulong num2 = (ulong)xx[6];
			ulong num3 = (ulong)xx[7];
			ulong num4 = (ulong)xx[8];
			ulong num5 = (ulong)xx[9];
			ulong num6 = 0UL;
			num6 += (ulong)xx[0] + num + (num << 31);
			z[0] = (uint)num6;
			num6 >>= 32;
			num6 += (ulong)xx[1] + num2 + (num2 << 31);
			z[1] = (uint)num6;
			num6 >>= 32;
			num6 += (ulong)xx[2] + num3 + (num3 << 31);
			z[2] = (uint)num6;
			num6 >>= 32;
			num6 += (ulong)xx[3] + num4 + (num4 << 31);
			z[3] = (uint)num6;
			num6 >>= 32;
			num6 += (ulong)xx[4] + num5 + (num5 << 31);
			z[4] = (uint)num6;
			num6 >>= 32;
			SecP160R1Field.Reduce32((uint)num6, z);
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x000F1E20 File Offset: 0x000F0020
		public static void Reduce32(uint x, uint[] z)
		{
			if ((x != 0U && Nat160.MulWordsAdd(2147483649U, x, z, 0) != 0U) || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R1Field.P)))
			{
				Nat.AddWordTo(5, 2147483649U, z);
			}
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x000F1E54 File Offset: 0x000F0054
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat160.CreateExt();
			Nat160.Square(x, array);
			SecP160R1Field.Reduce(array, z);
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x000F1E78 File Offset: 0x000F0078
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat160.CreateExt();
			Nat160.Square(x, array);
			SecP160R1Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat160.Square(z, array);
				SecP160R1Field.Reduce(array, z);
			}
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x000F1EB2 File Offset: 0x000F00B2
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat160.Sub(x, y, z) != 0)
			{
				Nat.SubWordFrom(5, 2147483649U, z);
			}
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000F1ECB File Offset: 0x000F00CB
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(10, xx, yy, zz) != 0 && Nat.SubFrom(SecP160R1Field.PExtInv.Length, SecP160R1Field.PExtInv, zz) != 0)
			{
				Nat.DecAt(10, zz, SecP160R1Field.PExtInv.Length);
			}
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x000F1EFD File Offset: 0x000F00FD
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(5, x, 0U, z) != 0U || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R1Field.P)))
			{
				Nat.AddWordTo(5, 2147483649U, z);
			}
		}

		// Token: 0x04001A0C RID: 6668
		internal static readonly uint[] P = new uint[]
		{
			2147483647U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x04001A0D RID: 6669
		internal static readonly uint[] PExt = new uint[]
		{
			1U,
			1073741825U,
			0U,
			0U,
			0U,
			4294967294U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x04001A0E RID: 6670
		private static readonly uint[] PExtInv = new uint[]
		{
			uint.MaxValue,
			3221225470U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			1U,
			1U
		};

		// Token: 0x04001A0F RID: 6671
		private const uint P4 = 4294967295U;

		// Token: 0x04001A10 RID: 6672
		private const uint PExt9 = 4294967295U;

		// Token: 0x04001A11 RID: 6673
		private const uint PInv = 2147483649U;
	}
}
