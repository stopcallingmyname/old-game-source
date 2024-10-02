using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000381 RID: 897
	internal class SecT113Field
	{
		// Token: 0x06002351 RID: 9041 RVA: 0x000FAE14 File Offset: 0x000F9014
		public static void Add(ulong[] x, ulong[] y, ulong[] z)
		{
			z[0] = (x[0] ^ y[0]);
			z[1] = (x[1] ^ y[1]);
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x000FAE2A File Offset: 0x000F902A
		public static void AddExt(ulong[] xx, ulong[] yy, ulong[] zz)
		{
			zz[0] = (xx[0] ^ yy[0]);
			zz[1] = (xx[1] ^ yy[1]);
			zz[2] = (xx[2] ^ yy[2]);
			zz[3] = (xx[3] ^ yy[3]);
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x000FAE54 File Offset: 0x000F9054
		public static void AddOne(ulong[] x, ulong[] z)
		{
			z[0] = (x[0] ^ 1UL);
			z[1] = x[1];
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x000FAE65 File Offset: 0x000F9065
		public static ulong[] FromBigInteger(BigInteger x)
		{
			ulong[] array = Nat128.FromBigInteger64(x);
			SecT113Field.Reduce15(array, 0);
			return array;
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x000FAE74 File Offset: 0x000F9074
		public static void Invert(ulong[] x, ulong[] z)
		{
			if (Nat128.IsZero64(x))
			{
				throw new InvalidOperationException();
			}
			ulong[] array = Nat128.Create64();
			ulong[] array2 = Nat128.Create64();
			SecT113Field.Square(x, array);
			SecT113Field.Multiply(array, x, array);
			SecT113Field.Square(array, array);
			SecT113Field.Multiply(array, x, array);
			SecT113Field.SquareN(array, 3, array2);
			SecT113Field.Multiply(array2, array, array2);
			SecT113Field.Square(array2, array2);
			SecT113Field.Multiply(array2, x, array2);
			SecT113Field.SquareN(array2, 7, array);
			SecT113Field.Multiply(array, array2, array);
			SecT113Field.SquareN(array, 14, array2);
			SecT113Field.Multiply(array2, array, array2);
			SecT113Field.SquareN(array2, 28, array);
			SecT113Field.Multiply(array, array2, array);
			SecT113Field.SquareN(array, 56, array2);
			SecT113Field.Multiply(array2, array, array2);
			SecT113Field.Square(array2, z);
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x000FAF24 File Offset: 0x000F9124
		public static void Multiply(ulong[] x, ulong[] y, ulong[] z)
		{
			ulong[] array = Nat128.CreateExt64();
			SecT113Field.ImplMultiply(x, y, array);
			SecT113Field.Reduce(array, z);
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x000FAF48 File Offset: 0x000F9148
		public static void MultiplyAddToExt(ulong[] x, ulong[] y, ulong[] zz)
		{
			ulong[] array = Nat128.CreateExt64();
			SecT113Field.ImplMultiply(x, y, array);
			SecT113Field.AddExt(zz, array, zz);
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x000FAF6C File Offset: 0x000F916C
		public static void Reduce(ulong[] xx, ulong[] z)
		{
			ulong num = xx[0];
			ulong num2 = xx[1];
			ulong num3 = xx[2];
			ulong num4 = xx[3];
			num2 ^= (num4 << 15 ^ num4 << 24);
			num3 ^= (num4 >> 49 ^ num4 >> 40);
			num ^= (num3 << 15 ^ num3 << 24);
			num2 ^= (num3 >> 49 ^ num3 >> 40);
			ulong num5 = num2 >> 49;
			z[0] = (num ^ num5 ^ num5 << 9);
			z[1] = (num2 & 562949953421311UL);
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x000FAFDC File Offset: 0x000F91DC
		public static void Reduce15(ulong[] z, int zOff)
		{
			ulong num = z[zOff + 1];
			ulong num2 = num >> 49;
			z[zOff] ^= (num2 ^ num2 << 9);
			z[zOff + 1] = (num & 562949953421311UL);
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x000FB018 File Offset: 0x000F9218
		public static void Sqrt(ulong[] x, ulong[] z)
		{
			ulong num = Interleave.Unshuffle(x[0]);
			ulong num2 = Interleave.Unshuffle(x[1]);
			ulong num3 = (num & (ulong)-1) | num2 << 32;
			ulong num4 = num >> 32 | (num2 & 18446744069414584320UL);
			z[0] = (num3 ^ num4 << 57 ^ num4 << 5);
			z[1] = (num4 >> 7 ^ num4 >> 59);
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x000FB068 File Offset: 0x000F9268
		public static void Square(ulong[] x, ulong[] z)
		{
			ulong[] array = Nat128.CreateExt64();
			SecT113Field.ImplSquare(x, array);
			SecT113Field.Reduce(array, z);
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x000FB08C File Offset: 0x000F928C
		public static void SquareAddToExt(ulong[] x, ulong[] zz)
		{
			ulong[] array = Nat128.CreateExt64();
			SecT113Field.ImplSquare(x, array);
			SecT113Field.AddExt(zz, array, zz);
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x000FB0B0 File Offset: 0x000F92B0
		public static void SquareN(ulong[] x, int n, ulong[] z)
		{
			ulong[] array = Nat128.CreateExt64();
			SecT113Field.ImplSquare(x, array);
			SecT113Field.Reduce(array, z);
			while (--n > 0)
			{
				SecT113Field.ImplSquare(z, array);
				SecT113Field.Reduce(array, z);
			}
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x000FB0EA File Offset: 0x000F92EA
		public static uint Trace(ulong[] x)
		{
			return (uint)x[0] & 1U;
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x000FB0F4 File Offset: 0x000F92F4
		protected static void ImplMultiply(ulong[] x, ulong[] y, ulong[] zz)
		{
			ulong num = x[0];
			ulong num2 = x[1];
			num2 = ((num >> 57 ^ num2 << 7) & 144115188075855871UL);
			ulong num3 = num & 144115188075855871UL;
			ulong num4 = y[0];
			ulong num5 = y[1];
			num5 = ((num4 >> 57 ^ num5 << 7) & 144115188075855871UL);
			num4 &= 144115188075855871UL;
			ulong[] array = new ulong[6];
			SecT113Field.ImplMulw(num3, num4, array, 0);
			SecT113Field.ImplMulw(num2, num5, array, 2);
			SecT113Field.ImplMulw(num3 ^ num2, num4 ^ num5, array, 4);
			ulong num6 = array[1] ^ array[2];
			ulong num7 = array[0];
			ulong num8 = array[3];
			ulong num9 = array[4] ^ num7 ^ num6;
			ulong num10 = array[5] ^ num8 ^ num6;
			zz[0] = (num7 ^ num9 << 57);
			zz[1] = (num9 >> 7 ^ num10 << 50);
			zz[2] = (num10 >> 14 ^ num8 << 43);
			zz[3] = num8 >> 21;
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x000FB1C8 File Offset: 0x000F93C8
		protected static void ImplMulw(ulong x, ulong y, ulong[] z, int zOff)
		{
			ulong[] array = new ulong[8];
			array[1] = y;
			array[2] = array[1] << 1;
			array[3] = (array[2] ^ y);
			array[4] = array[2] << 1;
			array[5] = (array[4] ^ y);
			array[6] = array[3] << 1;
			array[7] = (array[6] ^ y);
			uint num = (uint)x;
			ulong num2 = 0UL;
			ulong num3 = array[(int)(num & 7U)];
			int num4 = 48;
			do
			{
				num = (uint)(x >> num4);
				ulong num5 = array[(int)(num & 7U)] ^ array[(int)(num >> 3 & 7U)] << 3 ^ array[(int)(num >> 6 & 7U)] << 6;
				num3 ^= num5 << num4;
				num2 ^= num5 >> -num4;
			}
			while ((num4 -= 9) > 0);
			num2 ^= (x & 72198606942111744UL & y << 7 >> 63) >> 8;
			z[zOff] = (num3 & 144115188075855871UL);
			z[zOff + 1] = (num3 >> 57 ^ num2 << 7);
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x000FB29A File Offset: 0x000F949A
		protected static void ImplSquare(ulong[] x, ulong[] zz)
		{
			Interleave.Expand64To128(x[0], zz, 0);
			Interleave.Expand64To128(x[1], zz, 2);
		}

		// Token: 0x04001A78 RID: 6776
		private const ulong M49 = 562949953421311UL;

		// Token: 0x04001A79 RID: 6777
		private const ulong M57 = 144115188075855871UL;
	}
}
