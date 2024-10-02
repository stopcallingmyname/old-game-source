using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw
{
	// Token: 0x02000309 RID: 777
	internal abstract class Nat
	{
		// Token: 0x06001C87 RID: 7303 RVA: 0x000D69D4 File Offset: 0x000D4BD4
		public static uint Add(int len, uint[] x, uint[] y, uint[] z)
		{
			ulong num = 0UL;
			for (int i = 0; i < len; i++)
			{
				num += (ulong)x[i] + (ulong)y[i];
				z[i] = (uint)num;
				num >>= 32;
			}
			return (uint)num;
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x000D6A08 File Offset: 0x000D4C08
		public static uint Add33At(int len, uint x, uint[] z, int zPos)
		{
			ulong num = (ulong)z[zPos] + (ulong)x;
			z[zPos] = (uint)num;
			num >>= 32;
			num += (ulong)z[zPos + 1] + 1UL;
			z[zPos + 1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zPos + 2);
			}
			return 0U;
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x000D6A50 File Offset: 0x000D4C50
		public static uint Add33At(int len, uint x, uint[] z, int zOff, int zPos)
		{
			ulong num = (ulong)z[zOff + zPos] + (ulong)x;
			z[zOff + zPos] = (uint)num;
			num >>= 32;
			num += (ulong)z[zOff + zPos + 1] + 1UL;
			z[zOff + zPos + 1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zOff, zPos + 2);
			}
			return 0U;
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x000D6AA4 File Offset: 0x000D4CA4
		public static uint Add33To(int len, uint x, uint[] z)
		{
			ulong num = (ulong)z[0] + (ulong)x;
			z[0] = (uint)num;
			num >>= 32;
			num += (ulong)z[1] + 1UL;
			z[1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, 2);
			}
			return 0U;
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x000D6AE4 File Offset: 0x000D4CE4
		public static uint Add33To(int len, uint x, uint[] z, int zOff)
		{
			ulong num = (ulong)z[zOff] + (ulong)x;
			z[zOff] = (uint)num;
			num >>= 32;
			num += (ulong)z[zOff + 1] + 1UL;
			z[zOff + 1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zOff, 2);
			}
			return 0U;
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x000D6B2C File Offset: 0x000D4D2C
		public static uint AddBothTo(int len, uint[] x, uint[] y, uint[] z)
		{
			ulong num = 0UL;
			for (int i = 0; i < len; i++)
			{
				num += (ulong)x[i] + (ulong)y[i] + (ulong)z[i];
				z[i] = (uint)num;
				num >>= 32;
			}
			return (uint)num;
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x000D6B68 File Offset: 0x000D4D68
		public static uint AddBothTo(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
		{
			ulong num = 0UL;
			for (int i = 0; i < len; i++)
			{
				num += (ulong)x[xOff + i] + (ulong)y[yOff + i] + (ulong)z[zOff + i];
				z[zOff + i] = (uint)num;
				num >>= 32;
			}
			return (uint)num;
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x000D6BB0 File Offset: 0x000D4DB0
		public static uint AddDWordAt(int len, ulong x, uint[] z, int zPos)
		{
			ulong num = (ulong)z[zPos] + (x & (ulong)-1);
			z[zPos] = (uint)num;
			num >>= 32;
			num += (ulong)z[zPos + 1] + (x >> 32);
			z[zPos + 1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zPos + 2);
			}
			return 0U;
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x000D6BFC File Offset: 0x000D4DFC
		public static uint AddDWordAt(int len, ulong x, uint[] z, int zOff, int zPos)
		{
			ulong num = (ulong)z[zOff + zPos] + (x & (ulong)-1);
			z[zOff + zPos] = (uint)num;
			num >>= 32;
			num += (ulong)z[zOff + zPos + 1] + (x >> 32);
			z[zOff + zPos + 1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zOff, zPos + 2);
			}
			return 0U;
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x000D6C54 File Offset: 0x000D4E54
		public static uint AddDWordTo(int len, ulong x, uint[] z)
		{
			ulong num = (ulong)z[0] + (x & (ulong)-1);
			z[0] = (uint)num;
			num >>= 32;
			num += (ulong)z[1] + (x >> 32);
			z[1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, 2);
			}
			return 0U;
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x000D6C98 File Offset: 0x000D4E98
		public static uint AddDWordTo(int len, ulong x, uint[] z, int zOff)
		{
			ulong num = (ulong)z[zOff] + (x & (ulong)-1);
			z[zOff] = (uint)num;
			num >>= 32;
			num += (ulong)z[zOff + 1] + (x >> 32);
			z[zOff + 1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zOff, 2);
			}
			return 0U;
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x000D6CE4 File Offset: 0x000D4EE4
		public static uint AddTo(int len, uint[] x, uint[] z)
		{
			ulong num = 0UL;
			for (int i = 0; i < len; i++)
			{
				num += (ulong)x[i] + (ulong)z[i];
				z[i] = (uint)num;
				num >>= 32;
			}
			return (uint)num;
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x000D6D18 File Offset: 0x000D4F18
		public static uint AddTo(int len, uint[] x, int xOff, uint[] z, int zOff)
		{
			ulong num = 0UL;
			for (int i = 0; i < len; i++)
			{
				num += (ulong)x[xOff + i] + (ulong)z[zOff + i];
				z[zOff + i] = (uint)num;
				num >>= 32;
			}
			return (uint)num;
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x000D6D54 File Offset: 0x000D4F54
		public static uint AddWordAt(int len, uint x, uint[] z, int zPos)
		{
			ulong num = (ulong)x + (ulong)z[zPos];
			z[zPos] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zPos + 1);
			}
			return 0U;
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x000D6D84 File Offset: 0x000D4F84
		public static uint AddWordAt(int len, uint x, uint[] z, int zOff, int zPos)
		{
			ulong num = (ulong)x + (ulong)z[zOff + zPos];
			z[zOff + zPos] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zOff, zPos + 1);
			}
			return 0U;
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x000D6DBC File Offset: 0x000D4FBC
		public static uint AddWordTo(int len, uint x, uint[] z)
		{
			ulong num = (ulong)x + (ulong)z[0];
			z[0] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, 1);
			}
			return 0U;
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x000D6DE8 File Offset: 0x000D4FE8
		public static uint AddWordTo(int len, uint x, uint[] z, int zOff)
		{
			ulong num = (ulong)x + (ulong)z[zOff];
			z[zOff] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zOff, 1);
			}
			return 0U;
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x000D6E18 File Offset: 0x000D5018
		public static uint CAdd(int len, int mask, uint[] x, uint[] y, uint[] z)
		{
			uint num = (uint)(-(uint)(mask & 1));
			ulong num2 = 0UL;
			for (int i = 0; i < len; i++)
			{
				num2 += (ulong)x[i] + (ulong)(y[i] & num);
				z[i] = (uint)num2;
				num2 >>= 32;
			}
			return (uint)num2;
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x000D6E54 File Offset: 0x000D5054
		public static void CMov(int len, int mask, uint[] x, int xOff, uint[] z, int zOff)
		{
			uint num = (uint)(-(uint)(mask & 1));
			for (int i = 0; i < len; i++)
			{
				uint num2 = z[zOff + i];
				uint num3 = num2 ^ x[xOff + i];
				num2 ^= (num3 & num);
				z[zOff + i] = num2;
			}
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x000D6E90 File Offset: 0x000D5090
		public static void CMov(int len, int mask, int[] x, int xOff, int[] z, int zOff)
		{
			mask = -(mask & 1);
			for (int i = 0; i < len; i++)
			{
				int num = z[zOff + i];
				int num2 = num ^ x[xOff + i];
				num ^= (num2 & mask);
				z[zOff + i] = num;
			}
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x000D6ECD File Offset: 0x000D50CD
		public static void Copy(int len, uint[] x, uint[] z)
		{
			Array.Copy(x, 0, z, 0, len);
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x000D6EDC File Offset: 0x000D50DC
		public static uint[] Copy(int len, uint[] x)
		{
			uint[] array = new uint[len];
			Array.Copy(x, 0, array, 0, len);
			return array;
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x000D6EFB File Offset: 0x000D50FB
		public static void Copy(int len, uint[] x, int xOff, uint[] z, int zOff)
		{
			Array.Copy(x, xOff, z, zOff, len);
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x000D6F08 File Offset: 0x000D5108
		public static uint[] Create(int len)
		{
			return new uint[len];
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x000D6F10 File Offset: 0x000D5110
		public static ulong[] Create64(int len)
		{
			return new ulong[len];
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x000D6F18 File Offset: 0x000D5118
		public static int Dec(int len, uint[] z)
		{
			for (int i = 0; i < len; i++)
			{
				int num = i;
				uint num2 = z[num] - 1U;
				z[num] = num2;
				if (num2 != 4294967295U)
				{
					return 0;
				}
			}
			return -1;
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x000D6F48 File Offset: 0x000D5148
		public static int Dec(int len, uint[] x, uint[] z)
		{
			int i = 0;
			while (i < len)
			{
				uint num = x[i] - 1U;
				z[i] = num;
				i++;
				if (num != 4294967295U)
				{
					while (i < len)
					{
						z[i] = x[i];
						i++;
					}
					return 0;
				}
			}
			return -1;
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x000D6F84 File Offset: 0x000D5184
		public static int DecAt(int len, uint[] z, int zPos)
		{
			for (int i = zPos; i < len; i++)
			{
				int num = i;
				uint num2 = z[num] - 1U;
				z[num] = num2;
				if (num2 != 4294967295U)
				{
					return 0;
				}
			}
			return -1;
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x000D6FB4 File Offset: 0x000D51B4
		public static int DecAt(int len, uint[] z, int zOff, int zPos)
		{
			for (int i = zPos; i < len; i++)
			{
				int num = zOff + i;
				uint num2 = z[num] - 1U;
				z[num] = num2;
				if (num2 != 4294967295U)
				{
					return 0;
				}
			}
			return -1;
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x000D6FE4 File Offset: 0x000D51E4
		public static bool Eq(int len, uint[] x, uint[] y)
		{
			for (int i = len - 1; i >= 0; i--)
			{
				if (x[i] != y[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x000D700C File Offset: 0x000D520C
		public static uint[] FromBigInteger(int bits, BigInteger x)
		{
			if (x.SignValue < 0 || x.BitLength > bits)
			{
				throw new ArgumentException();
			}
			uint[] array = Nat.Create(bits + 31 >> 5);
			int num = 0;
			while (x.SignValue != 0)
			{
				array[num++] = (uint)x.IntValue;
				x = x.ShiftRight(32);
			}
			return array;
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x000D7064 File Offset: 0x000D5264
		public static uint GetBit(uint[] x, int bit)
		{
			if (bit == 0)
			{
				return x[0] & 1U;
			}
			int num = bit >> 5;
			if (num < 0 || num >= x.Length)
			{
				return 0U;
			}
			int num2 = bit & 31;
			return x[num] >> num2 & 1U;
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x000D709C File Offset: 0x000D529C
		public static bool Gte(int len, uint[] x, uint[] y)
		{
			for (int i = len - 1; i >= 0; i--)
			{
				uint num = x[i];
				uint num2 = y[i];
				if (num < num2)
				{
					return false;
				}
				if (num > num2)
				{
					return true;
				}
			}
			return true;
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x000D70CC File Offset: 0x000D52CC
		public static uint Inc(int len, uint[] z)
		{
			for (int i = 0; i < len; i++)
			{
				int num = i;
				uint num2 = z[num] + 1U;
				z[num] = num2;
				if (num2 != 0U)
				{
					return 0U;
				}
			}
			return 1U;
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x000D70FC File Offset: 0x000D52FC
		public static uint Inc(int len, uint[] x, uint[] z)
		{
			int i = 0;
			while (i < len)
			{
				uint num = x[i] + 1U;
				z[i] = num;
				i++;
				if (num != 0U)
				{
					while (i < len)
					{
						z[i] = x[i];
						i++;
					}
					return 0U;
				}
			}
			return 1U;
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x000D7138 File Offset: 0x000D5338
		public static uint IncAt(int len, uint[] z, int zPos)
		{
			for (int i = zPos; i < len; i++)
			{
				int num = i;
				uint num2 = z[num] + 1U;
				z[num] = num2;
				if (num2 != 0U)
				{
					return 0U;
				}
			}
			return 1U;
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x000D7168 File Offset: 0x000D5368
		public static uint IncAt(int len, uint[] z, int zOff, int zPos)
		{
			for (int i = zPos; i < len; i++)
			{
				int num = zOff + i;
				uint num2 = z[num] + 1U;
				z[num] = num2;
				if (num2 != 0U)
				{
					return 0U;
				}
			}
			return 1U;
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x000D7198 File Offset: 0x000D5398
		public static bool IsOne(int len, uint[] x)
		{
			if (x[0] != 1U)
			{
				return false;
			}
			for (int i = 1; i < len; i++)
			{
				if (x[i] != 0U)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x000D71C4 File Offset: 0x000D53C4
		public static bool IsZero(int len, uint[] x)
		{
			if (x[0] != 0U)
			{
				return false;
			}
			for (int i = 1; i < len; i++)
			{
				if (x[i] != 0U)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x000D71EC File Offset: 0x000D53EC
		public static void Mul(int len, uint[] x, uint[] y, uint[] zz)
		{
			zz[len] = Nat.MulWord(len, x[0], y, zz);
			for (int i = 1; i < len; i++)
			{
				zz[i + len] = Nat.MulWordAddTo(len, x[i], y, 0, zz, i);
			}
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x000D7228 File Offset: 0x000D5428
		public static void Mul(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff)
		{
			zz[zzOff + len] = Nat.MulWord(len, x[xOff], y, yOff, zz, zzOff);
			for (int i = 1; i < len; i++)
			{
				zz[zzOff + i + len] = Nat.MulWordAddTo(len, x[xOff + i], y, yOff, zz, zzOff + i);
			}
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x000D7278 File Offset: 0x000D5478
		public static void Mul(uint[] x, int xOff, int xLen, uint[] y, int yOff, int yLen, uint[] zz, int zzOff)
		{
			zz[zzOff + yLen] = Nat.MulWord(yLen, x[xOff], y, yOff, zz, zzOff);
			for (int i = 1; i < xLen; i++)
			{
				zz[zzOff + i + yLen] = Nat.MulWordAddTo(yLen, x[xOff + i], y, yOff, zz, zzOff + i);
			}
		}

		// Token: 0x06001CB1 RID: 7345 RVA: 0x000D72CC File Offset: 0x000D54CC
		public static uint MulAddTo(int len, uint[] x, uint[] y, uint[] zz)
		{
			ulong num = 0UL;
			for (int i = 0; i < len; i++)
			{
				ulong num2 = (ulong)Nat.MulWordAddTo(len, x[i], y, 0, zz, i) & (ulong)-1;
				num2 += num + ((ulong)zz[i + len] & (ulong)-1);
				zz[i + len] = (uint)num2;
				num = num2 >> 32;
			}
			return (uint)num;
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x000D7318 File Offset: 0x000D5518
		public static uint MulAddTo(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff)
		{
			ulong num = 0UL;
			for (int i = 0; i < len; i++)
			{
				ulong num2 = (ulong)Nat.MulWordAddTo(len, x[xOff + i], y, yOff, zz, zzOff) & (ulong)-1;
				num2 += num + ((ulong)zz[zzOff + len] & (ulong)-1);
				zz[zzOff + len] = (uint)num2;
				num = num2 >> 32;
				zzOff++;
			}
			return (uint)num;
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x000D7374 File Offset: 0x000D5574
		public static uint Mul31BothAdd(int len, uint a, uint[] x, uint b, uint[] y, uint[] z, int zOff)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)a;
			ulong num3 = (ulong)b;
			int num4 = 0;
			do
			{
				num += num2 * (ulong)x[num4] + num3 * (ulong)y[num4] + (ulong)z[zOff + num4];
				z[zOff + num4] = (uint)num;
				num >>= 32;
			}
			while (++num4 < len);
			return (uint)num;
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x000D73C0 File Offset: 0x000D55C0
		public static uint MulWord(int len, uint x, uint[] y, uint[] z)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x;
			int num3 = 0;
			do
			{
				num += num2 * (ulong)y[num3];
				z[num3] = (uint)num;
				num >>= 32;
			}
			while (++num3 < len);
			return (uint)num;
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x000D73F4 File Offset: 0x000D55F4
		public static uint MulWord(int len, uint x, uint[] y, int yOff, uint[] z, int zOff)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x;
			int num3 = 0;
			do
			{
				num += num2 * (ulong)y[yOff + num3];
				z[zOff + num3] = (uint)num;
				num >>= 32;
			}
			while (++num3 < len);
			return (uint)num;
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x000D742C File Offset: 0x000D562C
		public static uint MulWordAddTo(int len, uint x, uint[] y, int yOff, uint[] z, int zOff)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x;
			int num3 = 0;
			do
			{
				num += num2 * (ulong)y[yOff + num3] + (ulong)z[zOff + num3];
				z[zOff + num3] = (uint)num;
				num >>= 32;
			}
			while (++num3 < len);
			return (uint)num;
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x000D7470 File Offset: 0x000D5670
		public static uint MulWordDwordAddAt(int len, uint x, ulong y, uint[] z, int zPos)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x;
			num += num2 * (ulong)((uint)y) + (ulong)z[zPos];
			z[zPos] = (uint)num;
			num >>= 32;
			num += num2 * (y >> 32) + (ulong)z[zPos + 1];
			z[zPos + 1] = (uint)num;
			num >>= 32;
			num += (ulong)z[zPos + 2];
			z[zPos + 2] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zPos + 3);
			}
			return 0U;
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x000D74E4 File Offset: 0x000D56E4
		public static uint ShiftDownBit(int len, uint[] z, uint c)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = z[num];
				z[num] = (num2 >> 1 | c << 31);
				c = num2;
			}
			return c << 31;
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x000D7514 File Offset: 0x000D5714
		public static uint ShiftDownBit(int len, uint[] z, int zOff, uint c)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = z[zOff + num];
				z[zOff + num] = (num2 >> 1 | c << 31);
				c = num2;
			}
			return c << 31;
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x000D7548 File Offset: 0x000D5748
		public static uint ShiftDownBit(int len, uint[] x, uint c, uint[] z)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = x[num];
				z[num] = (num2 >> 1 | c << 31);
				c = num2;
			}
			return c << 31;
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x000D7578 File Offset: 0x000D5778
		public static uint ShiftDownBit(int len, uint[] x, int xOff, uint c, uint[] z, int zOff)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = x[xOff + num];
				z[zOff + num] = (num2 >> 1 | c << 31);
				c = num2;
			}
			return c << 31;
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x000D75B0 File Offset: 0x000D57B0
		public static uint ShiftDownBits(int len, uint[] z, int bits, uint c)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = z[num];
				z[num] = (num2 >> bits | c << -bits);
				c = num2;
			}
			return c << -bits;
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x000D75E8 File Offset: 0x000D57E8
		public static uint ShiftDownBits(int len, uint[] z, int zOff, int bits, uint c)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = z[zOff + num];
				z[zOff + num] = (num2 >> bits | c << -bits);
				c = num2;
			}
			return c << -bits;
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x000D7628 File Offset: 0x000D5828
		public static uint ShiftDownBits(int len, uint[] x, int bits, uint c, uint[] z)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = x[num];
				z[num] = (num2 >> bits | c << -bits);
				c = num2;
			}
			return c << -bits;
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x000D7664 File Offset: 0x000D5864
		public static uint ShiftDownBits(int len, uint[] x, int xOff, int bits, uint c, uint[] z, int zOff)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = x[xOff + num];
				z[zOff + num] = (num2 >> bits | c << -bits);
				c = num2;
			}
			return c << -bits;
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x000D76A4 File Offset: 0x000D58A4
		public static uint ShiftDownWord(int len, uint[] z, uint c)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = z[num];
				z[num] = c;
				c = num2;
			}
			return c;
		}

		// Token: 0x06001CC1 RID: 7361 RVA: 0x000D76C8 File Offset: 0x000D58C8
		public static uint ShiftUpBit(int len, uint[] z, uint c)
		{
			for (int i = 0; i < len; i++)
			{
				uint num = z[i];
				z[i] = (num << 1 | c >> 31);
				c = num;
			}
			return c >> 31;
		}

		// Token: 0x06001CC2 RID: 7362 RVA: 0x000D76F8 File Offset: 0x000D58F8
		public static uint ShiftUpBit(int len, uint[] z, int zOff, uint c)
		{
			for (int i = 0; i < len; i++)
			{
				uint num = z[zOff + i];
				z[zOff + i] = (num << 1 | c >> 31);
				c = num;
			}
			return c >> 31;
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x000D772C File Offset: 0x000D592C
		public static uint ShiftUpBit(int len, uint[] x, uint c, uint[] z)
		{
			for (int i = 0; i < len; i++)
			{
				uint num = x[i];
				z[i] = (num << 1 | c >> 31);
				c = num;
			}
			return c >> 31;
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x000D775C File Offset: 0x000D595C
		public static uint ShiftUpBit(int len, uint[] x, int xOff, uint c, uint[] z, int zOff)
		{
			for (int i = 0; i < len; i++)
			{
				uint num = x[xOff + i];
				z[zOff + i] = (num << 1 | c >> 31);
				c = num;
			}
			return c >> 31;
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x000D7794 File Offset: 0x000D5994
		public static ulong ShiftUpBit64(int len, ulong[] x, int xOff, ulong c, ulong[] z, int zOff)
		{
			for (int i = 0; i < len; i++)
			{
				ulong num = x[xOff + i];
				z[zOff + i] = (num << 1 | c >> 63);
				c = num;
			}
			return c >> 63;
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x000D77CC File Offset: 0x000D59CC
		public static uint ShiftUpBits(int len, uint[] z, int bits, uint c)
		{
			for (int i = 0; i < len; i++)
			{
				uint num = z[i];
				z[i] = (num << bits | c >> -bits);
				c = num;
			}
			return c >> -bits;
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x000D7804 File Offset: 0x000D5A04
		public static uint ShiftUpBits(int len, uint[] z, int zOff, int bits, uint c)
		{
			for (int i = 0; i < len; i++)
			{
				uint num = z[zOff + i];
				z[zOff + i] = (num << bits | c >> -bits);
				c = num;
			}
			return c >> -bits;
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x000D7844 File Offset: 0x000D5A44
		public static ulong ShiftUpBits64(int len, ulong[] z, int zOff, int bits, ulong c)
		{
			for (int i = 0; i < len; i++)
			{
				ulong num = z[zOff + i];
				z[zOff + i] = (num << bits | c >> -bits);
				c = num;
			}
			return c >> -bits;
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x000D7884 File Offset: 0x000D5A84
		public static uint ShiftUpBits(int len, uint[] x, int bits, uint c, uint[] z)
		{
			for (int i = 0; i < len; i++)
			{
				uint num = x[i];
				z[i] = (num << bits | c >> -bits);
				c = num;
			}
			return c >> -bits;
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x000D78C0 File Offset: 0x000D5AC0
		public static uint ShiftUpBits(int len, uint[] x, int xOff, int bits, uint c, uint[] z, int zOff)
		{
			for (int i = 0; i < len; i++)
			{
				uint num = x[xOff + i];
				z[zOff + i] = (num << bits | c >> -bits);
				c = num;
			}
			return c >> -bits;
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x000D7900 File Offset: 0x000D5B00
		public static ulong ShiftUpBits64(int len, ulong[] x, int xOff, int bits, ulong c, ulong[] z, int zOff)
		{
			for (int i = 0; i < len; i++)
			{
				ulong num = x[xOff + i];
				z[zOff + i] = (num << bits | c >> -bits);
				c = num;
			}
			return c >> -bits;
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x000D7940 File Offset: 0x000D5B40
		public static void Square(int len, uint[] x, uint[] zz)
		{
			int num = len << 1;
			uint num2 = 0U;
			int num3 = len;
			int num4 = num;
			do
			{
				ulong num5 = (ulong)x[--num3];
				ulong num6 = num5 * num5;
				zz[--num4] = (num2 << 31 | (uint)(num6 >> 33));
				zz[--num4] = (uint)(num6 >> 1);
				num2 = (uint)num6;
			}
			while (num3 > 0);
			for (int i = 1; i < len; i++)
			{
				num2 = Nat.SquareWordAdd(x, i, zz);
				Nat.AddWordAt(num, num2, zz, i << 1);
			}
			Nat.ShiftUpBit(num, zz, x[0] << 31);
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x000D79C0 File Offset: 0x000D5BC0
		public static void Square(int len, uint[] x, int xOff, uint[] zz, int zzOff)
		{
			int num = len << 1;
			uint num2 = 0U;
			int num3 = len;
			int num4 = num;
			do
			{
				ulong num5 = (ulong)x[xOff + --num3];
				ulong num6 = num5 * num5;
				zz[zzOff + --num4] = (num2 << 31 | (uint)(num6 >> 33));
				zz[zzOff + --num4] = (uint)(num6 >> 1);
				num2 = (uint)num6;
			}
			while (num3 > 0);
			for (int i = 1; i < len; i++)
			{
				num2 = Nat.SquareWordAdd(x, xOff, i, zz, zzOff);
				Nat.AddWordAt(num, num2, zz, zzOff, i << 1);
			}
			Nat.ShiftUpBit(num, zz, zzOff, x[xOff] << 31);
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x000D7A50 File Offset: 0x000D5C50
		public static uint SquareWordAdd(uint[] x, int xPos, uint[] z)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x[xPos];
			int num3 = 0;
			do
			{
				num += num2 * (ulong)x[num3] + (ulong)z[xPos + num3];
				z[xPos + num3] = (uint)num;
				num >>= 32;
			}
			while (++num3 < xPos);
			return (uint)num;
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x000D7A90 File Offset: 0x000D5C90
		public static uint SquareWordAdd(uint[] x, int xOff, int xPos, uint[] z, int zOff)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x[xOff + xPos];
			int num3 = 0;
			do
			{
				num += num2 * ((ulong)x[xOff + num3] & (ulong)-1) + ((ulong)z[xPos + zOff] & (ulong)-1);
				z[xPos + zOff] = (uint)num;
				num >>= 32;
				zOff++;
			}
			while (++num3 < xPos);
			return (uint)num;
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x000D7AE0 File Offset: 0x000D5CE0
		public static int Sub(int len, uint[] x, uint[] y, uint[] z)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num += (long)((ulong)x[i] - (ulong)y[i]);
				z[i] = (uint)num;
				num >>= 32;
			}
			return (int)num;
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x000D7B14 File Offset: 0x000D5D14
		public static int Sub(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num += (long)((ulong)x[xOff + i] - (ulong)y[yOff + i]);
				z[zOff + i] = (uint)num;
				num >>= 32;
			}
			return (int)num;
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x000D7B54 File Offset: 0x000D5D54
		public static int Sub33At(int len, uint x, uint[] z, int zPos)
		{
			long num = (long)((ulong)z[zPos] - (ulong)x);
			z[zPos] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[zPos + 1] - 1UL);
			z[zPos + 1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zPos + 2);
			}
			return 0;
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x000D7B9C File Offset: 0x000D5D9C
		public static int Sub33At(int len, uint x, uint[] z, int zOff, int zPos)
		{
			long num = (long)((ulong)z[zOff + zPos] - (ulong)x);
			z[zOff + zPos] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[zOff + zPos + 1] - 1UL);
			z[zOff + zPos + 1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zOff, zPos + 2);
			}
			return 0;
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x000D7BF0 File Offset: 0x000D5DF0
		public static int Sub33From(int len, uint x, uint[] z)
		{
			long num = (long)((ulong)z[0] - (ulong)x);
			z[0] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[1] - 1UL);
			z[1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, 2);
			}
			return 0;
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x000D7C30 File Offset: 0x000D5E30
		public static int Sub33From(int len, uint x, uint[] z, int zOff)
		{
			long num = (long)((ulong)z[zOff] - (ulong)x);
			z[zOff] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[zOff + 1] - 1UL);
			z[zOff + 1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zOff, 2);
			}
			return 0;
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x000D7C78 File Offset: 0x000D5E78
		public static int SubBothFrom(int len, uint[] x, uint[] y, uint[] z)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num += (long)((ulong)z[i] - (ulong)x[i] - (ulong)y[i]);
				z[i] = (uint)num;
				num >>= 32;
			}
			return (int)num;
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x000D7CB4 File Offset: 0x000D5EB4
		public static int SubBothFrom(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num += (long)((ulong)z[zOff + i] - (ulong)x[xOff + i] - (ulong)y[yOff + i]);
				z[zOff + i] = (uint)num;
				num >>= 32;
			}
			return (int)num;
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x000D7CFC File Offset: 0x000D5EFC
		public static int SubDWordAt(int len, ulong x, uint[] z, int zPos)
		{
			long num = (long)((ulong)z[zPos] - (x & (ulong)-1));
			z[zPos] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[zPos + 1] - (x >> 32));
			z[zPos + 1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zPos + 2);
			}
			return 0;
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x000D7D48 File Offset: 0x000D5F48
		public static int SubDWordAt(int len, ulong x, uint[] z, int zOff, int zPos)
		{
			long num = (long)((ulong)z[zOff + zPos] - (x & (ulong)-1));
			z[zOff + zPos] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[zOff + zPos + 1] - (x >> 32));
			z[zOff + zPos + 1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zOff, zPos + 2);
			}
			return 0;
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x000D7DA0 File Offset: 0x000D5FA0
		public static int SubDWordFrom(int len, ulong x, uint[] z)
		{
			long num = (long)((ulong)z[0] - (x & (ulong)-1));
			z[0] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[1] - (x >> 32));
			z[1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, 2);
			}
			return 0;
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x000D7DE4 File Offset: 0x000D5FE4
		public static int SubDWordFrom(int len, ulong x, uint[] z, int zOff)
		{
			long num = (long)((ulong)z[zOff] - (x & (ulong)-1));
			z[zOff] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[zOff + 1] - (x >> 32));
			z[zOff + 1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zOff, 2);
			}
			return 0;
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x000D7E30 File Offset: 0x000D6030
		public static int SubFrom(int len, uint[] x, uint[] z)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num += (long)((ulong)z[i] - (ulong)x[i]);
				z[i] = (uint)num;
				num >>= 32;
			}
			return (int)num;
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000D7E64 File Offset: 0x000D6064
		public static int SubFrom(int len, uint[] x, int xOff, uint[] z, int zOff)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num += (long)((ulong)z[zOff + i] - (ulong)x[xOff + i]);
				z[zOff + i] = (uint)num;
				num >>= 32;
			}
			return (int)num;
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000D7EA0 File Offset: 0x000D60A0
		public static int SubWordAt(int len, uint x, uint[] z, int zPos)
		{
			long num = (long)((ulong)z[zPos] - (ulong)x);
			z[zPos] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zPos + 1);
			}
			return 0;
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x000D7ED0 File Offset: 0x000D60D0
		public static int SubWordAt(int len, uint x, uint[] z, int zOff, int zPos)
		{
			long num = (long)((ulong)z[zOff + zPos] - (ulong)x);
			z[zOff + zPos] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zOff, zPos + 1);
			}
			return 0;
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x000D7F08 File Offset: 0x000D6108
		public static int SubWordFrom(int len, uint x, uint[] z)
		{
			long num = (long)((ulong)z[0] - (ulong)x);
			z[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, 1);
			}
			return 0;
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x000D7F34 File Offset: 0x000D6134
		public static int SubWordFrom(int len, uint x, uint[] z, int zOff)
		{
			long num = (long)((ulong)z[zOff] - (ulong)x);
			z[zOff] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zOff, 1);
			}
			return 0;
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x000D7F64 File Offset: 0x000D6164
		public static BigInteger ToBigInteger(int len, uint[] x)
		{
			byte[] array = new byte[len << 2];
			for (int i = 0; i < len; i++)
			{
				uint num = x[i];
				if (num != 0U)
				{
					Pack.UInt32_To_BE(num, array, len - 1 - i << 2);
				}
			}
			return new BigInteger(1, array);
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x000D7FA4 File Offset: 0x000D61A4
		public static void Zero(int len, uint[] z)
		{
			for (int i = 0; i < len; i++)
			{
				z[i] = 0U;
			}
		}

		// Token: 0x04001941 RID: 6465
		private const ulong M = 4294967295UL;
	}
}
