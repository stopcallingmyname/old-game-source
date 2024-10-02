using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000354 RID: 852
	internal class SecP128R1Field
	{
		// Token: 0x060020A7 RID: 8359 RVA: 0x000F0709 File Offset: 0x000EE909
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat128.Add(x, y, z) != 0U || (z[3] >= 4294967293U && Nat128.Gte(z, SecP128R1Field.P)))
			{
				SecP128R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x000F072F File Offset: 0x000EE92F
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat256.Add(xx, yy, zz) != 0U || (zz[7] >= 4294967292U && Nat256.Gte(zz, SecP128R1Field.PExt)))
			{
				Nat.AddTo(SecP128R1Field.PExtInv.Length, SecP128R1Field.PExtInv, zz);
			}
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x000F0762 File Offset: 0x000EE962
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(4, x, z) != 0U || (z[3] >= 4294967293U && Nat128.Gte(z, SecP128R1Field.P)))
			{
				SecP128R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x000F0788 File Offset: 0x000EE988
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat128.FromBigInteger(x);
			if (array[3] >= 4294967293U && Nat128.Gte(array, SecP128R1Field.P))
			{
				Nat128.SubFrom(SecP128R1Field.P, array);
			}
			return array;
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x000F07C0 File Offset: 0x000EE9C0
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(4, x, 0U, z);
				return;
			}
			uint c = Nat128.Add(x, SecP128R1Field.P, z);
			Nat.ShiftDownBit(4, z, c);
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x000F07F8 File Offset: 0x000EE9F8
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat128.CreateExt();
			Nat128.Mul(x, y, array);
			SecP128R1Field.Reduce(array, z);
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x000F081A File Offset: 0x000EEA1A
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if (Nat128.MulAddTo(x, y, zz) != 0U || (zz[7] >= 4294967292U && Nat256.Gte(zz, SecP128R1Field.PExt)))
			{
				Nat.AddTo(SecP128R1Field.PExtInv.Length, SecP128R1Field.PExtInv, zz);
			}
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x000F084D File Offset: 0x000EEA4D
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat128.IsZero(x))
			{
				Nat128.Zero(z);
				return;
			}
			Nat128.Sub(SecP128R1Field.P, x, z);
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x000F086C File Offset: 0x000EEA6C
		public static void Reduce(uint[] xx, uint[] z)
		{
			ulong num = (ulong)xx[0];
			ulong num2 = (ulong)xx[1];
			ulong num3 = (ulong)xx[2];
			ulong num4 = (ulong)xx[3];
			ulong num5 = (ulong)xx[4];
			ulong num6 = (ulong)xx[5];
			ulong num7 = (ulong)xx[6];
			ulong num8 = (ulong)xx[7];
			num4 += num8;
			num7 += num8 << 1;
			num3 += num7;
			num6 += num7 << 1;
			num2 += num6;
			num5 += num6 << 1;
			num += num5;
			num4 += num5 << 1;
			z[0] = (uint)num;
			num2 += num >> 32;
			z[1] = (uint)num2;
			num3 += num2 >> 32;
			z[2] = (uint)num3;
			num4 += num3 >> 32;
			z[3] = (uint)num4;
			SecP128R1Field.Reduce32((uint)(num4 >> 32), z);
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x000F0910 File Offset: 0x000EEB10
		public static void Reduce32(uint x, uint[] z)
		{
			while (x != 0U)
			{
				ulong num = (ulong)x;
				ulong num2 = (ulong)z[0] + num;
				z[0] = (uint)num2;
				num2 >>= 32;
				if (num2 != 0UL)
				{
					num2 += (ulong)z[1];
					z[1] = (uint)num2;
					num2 >>= 32;
					num2 += (ulong)z[2];
					z[2] = (uint)num2;
					num2 >>= 32;
				}
				num2 += (ulong)z[3] + (num << 1);
				z[3] = (uint)num2;
				num2 >>= 32;
				x = (uint)num2;
			}
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x000F0974 File Offset: 0x000EEB74
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat128.CreateExt();
			Nat128.Square(x, array);
			SecP128R1Field.Reduce(array, z);
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x000F0998 File Offset: 0x000EEB98
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat128.CreateExt();
			Nat128.Square(x, array);
			SecP128R1Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat128.Square(z, array);
				SecP128R1Field.Reduce(array, z);
			}
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x000F09D2 File Offset: 0x000EEBD2
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat128.Sub(x, y, z) != 0)
			{
				SecP128R1Field.SubPInvFrom(z);
			}
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x000F09E4 File Offset: 0x000EEBE4
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(10, xx, yy, zz) != 0)
			{
				Nat.SubFrom(SecP128R1Field.PExtInv.Length, SecP128R1Field.PExtInv, zz);
			}
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x000F0A05 File Offset: 0x000EEC05
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(4, x, 0U, z) != 0U || (z[3] >= 4294967293U && Nat128.Gte(z, SecP128R1Field.P)))
			{
				SecP128R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x000F0A2C File Offset: 0x000EEC2C
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
				num += (long)((ulong)z[2]);
				z[2] = (uint)num;
				num >>= 32;
			}
			num += (long)((ulong)z[3] + 2UL);
			z[3] = (uint)num;
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x000F0A80 File Offset: 0x000EEC80
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
				num += (long)((ulong)z[2]);
				z[2] = (uint)num;
				num >>= 32;
			}
			num += (long)((ulong)z[3] - 2UL);
			z[3] = (uint)num;
		}

		// Token: 0x040019FD RID: 6653
		internal static readonly uint[] P = new uint[]
		{
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			4294967293U
		};

		// Token: 0x040019FE RID: 6654
		internal static readonly uint[] PExt = new uint[]
		{
			1U,
			0U,
			0U,
			4U,
			4294967294U,
			uint.MaxValue,
			3U,
			4294967292U
		};

		// Token: 0x040019FF RID: 6655
		private static readonly uint[] PExtInv = new uint[]
		{
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			4294967291U,
			1U,
			0U,
			4294967292U,
			3U
		};

		// Token: 0x04001A00 RID: 6656
		private const uint P3 = 4294967293U;

		// Token: 0x04001A01 RID: 6657
		private const uint PExt7 = 4294967292U;
	}
}
