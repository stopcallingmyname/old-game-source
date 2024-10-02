﻿using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x020003A1 RID: 929
	internal class SecT239Field
	{
		// Token: 0x0600256D RID: 9581 RVA: 0x000FAE2A File Offset: 0x000F902A
		public static void Add(ulong[] x, ulong[] y, ulong[] z)
		{
			z[0] = (x[0] ^ y[0]);
			z[1] = (x[1] ^ y[1]);
			z[2] = (x[2] ^ y[2]);
			z[3] = (x[3] ^ y[3]);
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x00102F94 File Offset: 0x00101194
		public static void AddExt(ulong[] xx, ulong[] yy, ulong[] zz)
		{
			zz[0] = (xx[0] ^ yy[0]);
			zz[1] = (xx[1] ^ yy[1]);
			zz[2] = (xx[2] ^ yy[2]);
			zz[3] = (xx[3] ^ yy[3]);
			zz[4] = (xx[4] ^ yy[4]);
			zz[5] = (xx[5] ^ yy[5]);
			zz[6] = (xx[6] ^ yy[6]);
			zz[7] = (xx[7] ^ yy[7]);
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x000FFE67 File Offset: 0x000FE067
		public static void AddOne(ulong[] x, ulong[] z)
		{
			z[0] = (x[0] ^ 1UL);
			z[1] = x[1];
			z[2] = x[2];
			z[3] = x[3];
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x00102FF1 File Offset: 0x001011F1
		public static ulong[] FromBigInteger(BigInteger x)
		{
			ulong[] array = Nat256.FromBigInteger64(x);
			SecT239Field.Reduce17(array, 0);
			return array;
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x00103000 File Offset: 0x00101200
		public static void Invert(ulong[] x, ulong[] z)
		{
			if (Nat256.IsZero64(x))
			{
				throw new InvalidOperationException();
			}
			ulong[] array = Nat256.Create64();
			ulong[] array2 = Nat256.Create64();
			SecT239Field.Square(x, array);
			SecT239Field.Multiply(array, x, array);
			SecT239Field.Square(array, array);
			SecT239Field.Multiply(array, x, array);
			SecT239Field.SquareN(array, 3, array2);
			SecT239Field.Multiply(array2, array, array2);
			SecT239Field.Square(array2, array2);
			SecT239Field.Multiply(array2, x, array2);
			SecT239Field.SquareN(array2, 7, array);
			SecT239Field.Multiply(array, array2, array);
			SecT239Field.SquareN(array, 14, array2);
			SecT239Field.Multiply(array2, array, array2);
			SecT239Field.Square(array2, array2);
			SecT239Field.Multiply(array2, x, array2);
			SecT239Field.SquareN(array2, 29, array);
			SecT239Field.Multiply(array, array2, array);
			SecT239Field.Square(array, array);
			SecT239Field.Multiply(array, x, array);
			SecT239Field.SquareN(array, 59, array2);
			SecT239Field.Multiply(array2, array, array2);
			SecT239Field.Square(array2, array2);
			SecT239Field.Multiply(array2, x, array2);
			SecT239Field.SquareN(array2, 119, array);
			SecT239Field.Multiply(array, array2, array);
			SecT239Field.Square(array, z);
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x001030EC File Offset: 0x001012EC
		public static void Multiply(ulong[] x, ulong[] y, ulong[] z)
		{
			ulong[] array = Nat256.CreateExt64();
			SecT239Field.ImplMultiply(x, y, array);
			SecT239Field.Reduce(array, z);
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x00103110 File Offset: 0x00101310
		public static void MultiplyAddToExt(ulong[] x, ulong[] y, ulong[] zz)
		{
			ulong[] array = Nat256.CreateExt64();
			SecT239Field.ImplMultiply(x, y, array);
			SecT239Field.AddExt(zz, array, zz);
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x00103134 File Offset: 0x00101334
		public static void Reduce(ulong[] xx, ulong[] z)
		{
			ulong num = xx[0];
			ulong num2 = xx[1];
			ulong num3 = xx[2];
			ulong num4 = xx[3];
			ulong num5 = xx[4];
			ulong num6 = xx[5];
			ulong num7 = xx[6];
			ulong num8 = xx[7];
			num4 ^= num8 << 17;
			num5 ^= num8 >> 47;
			num6 ^= num8 << 47;
			num7 ^= num8 >> 17;
			num3 ^= num7 << 17;
			num4 ^= num7 >> 47;
			num5 ^= num7 << 47;
			num6 ^= num7 >> 17;
			num2 ^= num6 << 17;
			num3 ^= num6 >> 47;
			num4 ^= num6 << 47;
			num5 ^= num6 >> 17;
			num ^= num5 << 17;
			num2 ^= num5 >> 47;
			num3 ^= num5 << 47;
			num4 ^= num5 >> 17;
			ulong num9 = num4 >> 47;
			z[0] = (num ^ num9);
			z[1] = num2;
			z[2] = (num3 ^ num9 << 30);
			z[3] = (num4 & 140737488355327UL);
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x0010321C File Offset: 0x0010141C
		public static void Reduce17(ulong[] z, int zOff)
		{
			ulong num = z[zOff + 3];
			ulong num2 = num >> 47;
			z[zOff] ^= num2;
			z[zOff + 2] ^= num2 << 30;
			z[zOff + 3] = (num & 140737488355327UL);
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x00103264 File Offset: 0x00101464
		public static void Sqrt(ulong[] x, ulong[] z)
		{
			ulong num = Interleave.Unshuffle(x[0]);
			ulong num2 = Interleave.Unshuffle(x[1]);
			ulong num3 = (num & (ulong)-1) | num2 << 32;
			ulong num4 = num >> 32 | (num2 & 18446744069414584320UL);
			ulong num5 = Interleave.Unshuffle(x[2]);
			num2 = Interleave.Unshuffle(x[3]);
			ulong num6 = (num5 & (ulong)-1) | num2 << 32;
			ulong num7 = num5 >> 32 | (num2 & 18446744069414584320UL);
			ulong num8 = num7 >> 49;
			ulong num9 = num4 >> 49 | num7 << 15;
			num7 ^= num4 << 15;
			ulong[] array = Nat256.CreateExt64();
			int[] array2 = new int[]
			{
				39,
				120
			};
			for (int i = 0; i < array2.Length; i++)
			{
				int num10 = array2[i] >> 6;
				int num11 = array2[i] & 63;
				array[num10] ^= num4 << num11;
				array[num10 + 1] ^= (num7 << num11 | num4 >> -num11);
				array[num10 + 2] ^= (num9 << num11 | num7 >> -num11);
				array[num10 + 3] ^= (num8 << num11 | num9 >> -num11);
				array[num10 + 4] ^= num8 >> -num11;
			}
			SecT239Field.Reduce(array, z);
			z[0] ^= num3;
			z[1] ^= num6;
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x001033D8 File Offset: 0x001015D8
		public static void Square(ulong[] x, ulong[] z)
		{
			ulong[] array = Nat256.CreateExt64();
			SecT239Field.ImplSquare(x, array);
			SecT239Field.Reduce(array, z);
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x001033FC File Offset: 0x001015FC
		public static void SquareAddToExt(ulong[] x, ulong[] zz)
		{
			ulong[] array = Nat256.CreateExt64();
			SecT239Field.ImplSquare(x, array);
			SecT239Field.AddExt(zz, array, zz);
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x00103420 File Offset: 0x00101620
		public static void SquareN(ulong[] x, int n, ulong[] z)
		{
			ulong[] array = Nat256.CreateExt64();
			SecT239Field.ImplSquare(x, array);
			SecT239Field.Reduce(array, z);
			while (--n > 0)
			{
				SecT239Field.ImplSquare(z, array);
				SecT239Field.Reduce(array, z);
			}
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x0010345A File Offset: 0x0010165A
		public static uint Trace(ulong[] x)
		{
			return (uint)(x[0] ^ x[1] >> 17 ^ x[2] >> 34) & 1U;
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x00103470 File Offset: 0x00101670
		protected static void ImplCompactExt(ulong[] zz)
		{
			ulong num = zz[0];
			ulong num2 = zz[1];
			ulong num3 = zz[2];
			ulong num4 = zz[3];
			ulong num5 = zz[4];
			ulong num6 = zz[5];
			ulong num7 = zz[6];
			ulong num8 = zz[7];
			zz[0] = (num ^ num2 << 60);
			zz[1] = (num2 >> 4 ^ num3 << 56);
			zz[2] = (num3 >> 8 ^ num4 << 52);
			zz[3] = (num4 >> 12 ^ num5 << 48);
			zz[4] = (num5 >> 16 ^ num6 << 44);
			zz[5] = (num6 >> 20 ^ num7 << 40);
			zz[6] = (num7 >> 24 ^ num8 << 36);
			zz[7] = num8 >> 28;
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x00103500 File Offset: 0x00101700
		protected static void ImplExpand(ulong[] x, ulong[] z)
		{
			ulong num = x[0];
			ulong num2 = x[1];
			ulong num3 = x[2];
			ulong num4 = x[3];
			z[0] = (num & 1152921504606846975UL);
			z[1] = ((num >> 60 ^ num2 << 4) & 1152921504606846975UL);
			z[2] = ((num2 >> 56 ^ num3 << 8) & 1152921504606846975UL);
			z[3] = (num3 >> 52 ^ num4 << 12);
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x00103564 File Offset: 0x00101764
		protected static void ImplMultiply(ulong[] x, ulong[] y, ulong[] zz)
		{
			ulong[] array = new ulong[4];
			ulong[] array2 = new ulong[4];
			SecT239Field.ImplExpand(x, array);
			SecT239Field.ImplExpand(y, array2);
			SecT239Field.ImplMulwAcc(array[0], array2[0], zz, 0);
			SecT239Field.ImplMulwAcc(array[1], array2[1], zz, 1);
			SecT239Field.ImplMulwAcc(array[2], array2[2], zz, 2);
			SecT239Field.ImplMulwAcc(array[3], array2[3], zz, 3);
			for (int i = 5; i > 0; i--)
			{
				zz[i] ^= zz[i - 1];
			}
			SecT239Field.ImplMulwAcc(array[0] ^ array[1], array2[0] ^ array2[1], zz, 1);
			SecT239Field.ImplMulwAcc(array[2] ^ array[3], array2[2] ^ array2[3], zz, 3);
			for (int j = 7; j > 1; j--)
			{
				zz[j] ^= zz[j - 2];
			}
			ulong num = array[0] ^ array[2];
			ulong num2 = array[1] ^ array[3];
			ulong num3 = array2[0] ^ array2[2];
			ulong num4 = array2[1] ^ array2[3];
			SecT239Field.ImplMulwAcc(num ^ num2, num3 ^ num4, zz, 3);
			ulong[] array3 = new ulong[3];
			SecT239Field.ImplMulwAcc(num, num3, array3, 0);
			SecT239Field.ImplMulwAcc(num2, num4, array3, 1);
			ulong num5 = array3[0];
			ulong num6 = array3[1];
			ulong num7 = array3[2];
			zz[2] ^= num5;
			zz[3] ^= (num5 ^ num6);
			zz[4] ^= (num7 ^ num6);
			zz[5] ^= num7;
			SecT239Field.ImplCompactExt(zz);
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x001036C8 File Offset: 0x001018C8
		protected static void ImplMulwAcc(ulong x, ulong y, ulong[] z, int zOff)
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
			ulong num3 = array[(int)(num & 7U)] ^ array[(int)(num >> 3 & 7U)] << 3;
			int num4 = 54;
			do
			{
				num = (uint)(x >> num4);
				ulong num5 = array[(int)(num & 7U)] ^ array[(int)(num >> 3 & 7U)] << 3;
				num3 ^= num5 << num4;
				num2 ^= num5 >> -num4;
			}
			while ((num4 -= 6) > 0);
			num2 ^= (x & 585610922974906400UL & y << 4 >> 63) >> 5;
			z[zOff] ^= (num3 & 1152921504606846975UL);
			z[zOff + 1] ^= (num3 >> 60 ^ num2 << 4);
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x001037AC File Offset: 0x001019AC
		protected static void ImplSquare(ulong[] x, ulong[] zz)
		{
			Interleave.Expand64To128(x[0], zz, 0);
			Interleave.Expand64To128(x[1], zz, 2);
			Interleave.Expand64To128(x[2], zz, 4);
			ulong num = x[3];
			zz[6] = Interleave.Expand32to64((uint)num);
			zz[7] = (ulong)Interleave.Expand16to32((uint)(num >> 32));
		}

		// Token: 0x04001AAA RID: 6826
		private const ulong M47 = 140737488355327UL;

		// Token: 0x04001AAB RID: 6827
		private const ulong M60 = 1152921504606846975UL;
	}
}
