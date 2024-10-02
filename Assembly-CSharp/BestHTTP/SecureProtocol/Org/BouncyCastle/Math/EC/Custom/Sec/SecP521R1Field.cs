using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200037E RID: 894
	internal class SecP521R1Field
	{
		// Token: 0x06002321 RID: 8993 RVA: 0x000FA220 File Offset: 0x000F8420
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			uint num = Nat.Add(16, x, y, z) + x[16] + y[16];
			if (num > 511U || (num == 511U && Nat.Eq(16, z, SecP521R1Field.P)))
			{
				num += Nat.Inc(16, z);
				num &= 511U;
			}
			z[16] = num;
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x000FA27C File Offset: 0x000F847C
		public static void AddOne(uint[] x, uint[] z)
		{
			uint num = Nat.Inc(16, x, z) + x[16];
			if (num > 511U || (num == 511U && Nat.Eq(16, z, SecP521R1Field.P)))
			{
				num += Nat.Inc(16, z);
				num &= 511U;
			}
			z[16] = num;
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x000FA2D0 File Offset: 0x000F84D0
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat.FromBigInteger(521, x);
			if (Nat.Eq(17, array, SecP521R1Field.P))
			{
				Nat.Zero(17, array);
			}
			return array;
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x000FA304 File Offset: 0x000F8504
		public static void Half(uint[] x, uint[] z)
		{
			uint num = x[16];
			uint num2 = Nat.ShiftDownBit(16, x, num, z);
			z[16] = (num >> 1 | num2 >> 23);
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x000FA330 File Offset: 0x000F8530
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat.Create(33);
			SecP521R1Field.ImplMultiply(x, y, array);
			SecP521R1Field.Reduce(array, z);
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x000FA354 File Offset: 0x000F8554
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat.IsZero(17, x))
			{
				Nat.Zero(17, z);
				return;
			}
			Nat.Sub(17, SecP521R1Field.P, x, z);
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x000FA378 File Offset: 0x000F8578
		public static void Reduce(uint[] xx, uint[] z)
		{
			uint num = xx[32];
			uint num2 = Nat.ShiftDownBits(16, xx, 16, 9, num, z, 0) >> 23;
			num2 += num >> 9;
			num2 += Nat.AddTo(16, xx, z);
			if (num2 > 511U || (num2 == 511U && Nat.Eq(16, z, SecP521R1Field.P)))
			{
				num2 += Nat.Inc(16, z);
				num2 &= 511U;
			}
			z[16] = num2;
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x000FA3E8 File Offset: 0x000F85E8
		public static void Reduce23(uint[] z)
		{
			uint num = z[16];
			uint num2 = Nat.AddWordTo(16, num >> 9, z) + (num & 511U);
			if (num2 > 511U || (num2 == 511U && Nat.Eq(16, z, SecP521R1Field.P)))
			{
				num2 += Nat.Inc(16, z);
				num2 &= 511U;
			}
			z[16] = num2;
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x000FA448 File Offset: 0x000F8648
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat.Create(33);
			SecP521R1Field.ImplSquare(x, array);
			SecP521R1Field.Reduce(array, z);
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x000FA46C File Offset: 0x000F866C
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat.Create(33);
			SecP521R1Field.ImplSquare(x, array);
			SecP521R1Field.Reduce(array, z);
			while (--n > 0)
			{
				SecP521R1Field.ImplSquare(z, array);
				SecP521R1Field.Reduce(array, z);
			}
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x000FA4A8 File Offset: 0x000F86A8
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			int num = Nat.Sub(16, x, y, z) + (int)(x[16] - y[16]);
			if (num < 0)
			{
				num += Nat.Dec(16, z);
				num &= 511;
			}
			z[16] = (uint)num;
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x000FA4E8 File Offset: 0x000F86E8
		public static void Twice(uint[] x, uint[] z)
		{
			uint num = x[16];
			uint num2 = Nat.ShiftUpBit(16, x, num << 23, z) | num << 1;
			z[16] = (num2 & 511U);
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x000FA518 File Offset: 0x000F8718
		protected static void ImplMultiply(uint[] x, uint[] y, uint[] zz)
		{
			Nat512.Mul(x, y, zz);
			uint num = x[16];
			uint num2 = y[16];
			zz[32] = Nat.Mul31BothAdd(16, num, y, num2, x, zz, 16) + num * num2;
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x000FA550 File Offset: 0x000F8750
		protected static void ImplSquare(uint[] x, uint[] zz)
		{
			Nat512.Square(x, zz);
			uint num = x[16];
			zz[32] = Nat.MulWordAddTo(16, num << 1, x, 0, zz, 16) + num * num;
		}

		// Token: 0x04001A74 RID: 6772
		internal static readonly uint[] P = new uint[]
		{
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			511U
		};

		// Token: 0x04001A75 RID: 6773
		private const int P16 = 511;
	}
}
