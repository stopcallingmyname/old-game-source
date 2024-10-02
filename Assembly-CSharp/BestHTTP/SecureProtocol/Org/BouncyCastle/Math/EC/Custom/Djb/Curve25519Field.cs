using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Djb
{
	// Token: 0x020003BC RID: 956
	internal class Curve25519Field
	{
		// Token: 0x06002740 RID: 10048 RVA: 0x00109F23 File Offset: 0x00108123
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			Nat256.Add(x, y, z);
			if (Nat256.Gte(z, Curve25519Field.P))
			{
				Curve25519Field.SubPFrom(z);
			}
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x00109F42 File Offset: 0x00108142
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			Nat.Add(16, xx, yy, zz);
			if (Nat.Gte(16, zz, Curve25519Field.PExt))
			{
				Curve25519Field.SubPExtFrom(zz);
			}
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x00109F65 File Offset: 0x00108165
		public static void AddOne(uint[] x, uint[] z)
		{
			Nat.Inc(8, x, z);
			if (Nat256.Gte(z, Curve25519Field.P))
			{
				Curve25519Field.SubPFrom(z);
			}
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x00109F84 File Offset: 0x00108184
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat256.FromBigInteger(x);
			while (Nat256.Gte(array, Curve25519Field.P))
			{
				Nat256.SubFrom(Curve25519Field.P, array);
			}
			return array;
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x00109FB4 File Offset: 0x001081B4
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(8, x, 0U, z);
				return;
			}
			Nat256.Add(x, Curve25519Field.P, z);
			Nat.ShiftDownBit(8, z, 0U);
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x00109FE0 File Offset: 0x001081E0
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat256.CreateExt();
			Nat256.Mul(x, y, array);
			Curve25519Field.Reduce(array, z);
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x0010A002 File Offset: 0x00108202
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			Nat256.MulAddTo(x, y, zz);
			if (Nat.Gte(16, zz, Curve25519Field.PExt))
			{
				Curve25519Field.SubPExtFrom(zz);
			}
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x0010A023 File Offset: 0x00108223
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat256.IsZero(x))
			{
				Nat256.Zero(z);
				return;
			}
			Nat256.Sub(Curve25519Field.P, x, z);
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x0010A044 File Offset: 0x00108244
		public static void Reduce(uint[] xx, uint[] z)
		{
			uint num = xx[7];
			Nat.ShiftUpBit(8, xx, 8, num, z, 0);
			uint num2 = Nat256.MulByWordAddTo(19U, xx, z) << 1;
			uint num3 = z[7];
			num2 += (num3 >> 31) - (num >> 31);
			num3 &= 2147483647U;
			num3 += Nat.AddWordTo(7, num2 * 19U, z);
			z[7] = num3;
			if (num3 >= 2147483647U && Nat256.Gte(z, Curve25519Field.P))
			{
				Curve25519Field.SubPFrom(z);
			}
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x0010A0B4 File Offset: 0x001082B4
		public static void Reduce27(uint x, uint[] z)
		{
			uint num = z[7];
			uint num2 = x << 1 | num >> 31;
			num &= 2147483647U;
			num += Nat.AddWordTo(7, num2 * 19U, z);
			z[7] = num;
			if (num >= 2147483647U && Nat256.Gte(z, Curve25519Field.P))
			{
				Curve25519Field.SubPFrom(z);
			}
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x0010A104 File Offset: 0x00108304
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat256.CreateExt();
			Nat256.Square(x, array);
			Curve25519Field.Reduce(array, z);
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x0010A128 File Offset: 0x00108328
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat256.CreateExt();
			Nat256.Square(x, array);
			Curve25519Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat256.Square(z, array);
				Curve25519Field.Reduce(array, z);
			}
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x0010A162 File Offset: 0x00108362
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat256.Sub(x, y, z) != 0)
			{
				Curve25519Field.AddPTo(z);
			}
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x0010A175 File Offset: 0x00108375
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(16, xx, yy, zz) != 0)
			{
				Curve25519Field.AddPExtTo(zz);
			}
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x0010A18A File Offset: 0x0010838A
		public static void Twice(uint[] x, uint[] z)
		{
			Nat.ShiftUpBit(8, x, 0U, z);
			if (Nat256.Gte(z, Curve25519Field.P))
			{
				Curve25519Field.SubPFrom(z);
			}
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x0010A1AC File Offset: 0x001083AC
		private static uint AddPTo(uint[] z)
		{
			long num = (long)((ulong)z[0] - 19UL);
			z[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num = (long)Nat.DecAt(7, z, 1);
			}
			num += (long)((ulong)z[7] + (ulong)int.MinValue);
			z[7] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x0010A1F4 File Offset: 0x001083F4
		private static uint AddPExtTo(uint[] zz)
		{
			long num = (long)((ulong)zz[0] + (ulong)Curve25519Field.PExt[0]);
			zz[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num = (long)((ulong)Nat.IncAt(8, zz, 1));
			}
			num += (long)((ulong)zz[8] - 19UL);
			zz[8] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num = (long)Nat.DecAt(15, zz, 9);
			}
			num += (long)((ulong)zz[15] + (ulong)(Curve25519Field.PExt[15] + 1U));
			zz[15] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x0010A26C File Offset: 0x0010846C
		private static int SubPFrom(uint[] z)
		{
			long num = (long)((ulong)z[0] + 19UL);
			z[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num = (long)((ulong)Nat.IncAt(7, z, 1));
			}
			num += (long)((ulong)z[7] - (ulong)int.MinValue);
			z[7] = (uint)num;
			num >>= 32;
			return (int)num;
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x0010A2B4 File Offset: 0x001084B4
		private static int SubPExtFrom(uint[] zz)
		{
			long num = (long)((ulong)zz[0] - (ulong)Curve25519Field.PExt[0]);
			zz[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num = (long)Nat.DecAt(8, zz, 1);
			}
			num += (long)((ulong)zz[8] + 19UL);
			zz[8] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num = (long)((ulong)Nat.IncAt(15, zz, 9));
			}
			num += (long)((ulong)zz[15] - (ulong)(Curve25519Field.PExt[15] + 1U));
			zz[15] = (uint)num;
			num >>= 32;
			return (int)num;
		}

		// Token: 0x04001ADD RID: 6877
		internal static readonly uint[] P = new uint[]
		{
			4294967277U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			2147483647U
		};

		// Token: 0x04001ADE RID: 6878
		private const uint P7 = 2147483647U;

		// Token: 0x04001ADF RID: 6879
		private static readonly uint[] PExt = new uint[]
		{
			361U,
			0U,
			0U,
			0U,
			0U,
			0U,
			0U,
			0U,
			4294967277U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			1073741823U
		};

		// Token: 0x04001AE0 RID: 6880
		private const uint PInv = 19U;
	}
}
