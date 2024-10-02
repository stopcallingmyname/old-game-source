using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw
{
	// Token: 0x02000308 RID: 776
	internal abstract class Mod
	{
		// Token: 0x06001C7E RID: 7294 RVA: 0x000D6714 File Offset: 0x000D4914
		public static void Invert(uint[] p, uint[] x, uint[] z)
		{
			int num = p.Length;
			if (Nat.IsZero(num, x))
			{
				throw new ArgumentException("cannot be 0", "x");
			}
			if (Nat.IsOne(num, x))
			{
				Array.Copy(x, 0, z, 0, num);
				return;
			}
			uint[] array = Nat.Copy(num, x);
			uint[] array2 = Nat.Create(num);
			array2[0] = 1U;
			int num2 = 0;
			if ((array[0] & 1U) == 0U)
			{
				Mod.InversionStep(p, array, num, array2, ref num2);
			}
			if (Nat.IsOne(num, array))
			{
				Mod.InversionResult(p, num2, array2, z);
				return;
			}
			uint[] array3 = Nat.Copy(num, p);
			uint[] array4 = Nat.Create(num);
			int num3 = 0;
			int num4 = num;
			for (;;)
			{
				if (array[num4 - 1] != 0U || array3[num4 - 1] != 0U)
				{
					if (Nat.Gte(num, array, array3))
					{
						Nat.SubFrom(num, array3, array);
						num2 += Nat.SubFrom(num, array4, array2) - num3;
						Mod.InversionStep(p, array, num4, array2, ref num2);
						if (Nat.IsOne(num, array))
						{
							break;
						}
					}
					else
					{
						Nat.SubFrom(num, array, array3);
						num3 += Nat.SubFrom(num, array2, array4) - num2;
						Mod.InversionStep(p, array3, num4, array4, ref num3);
						if (Nat.IsOne(num, array3))
						{
							goto Block_8;
						}
					}
				}
				else
				{
					num4--;
				}
			}
			Mod.InversionResult(p, num2, array2, z);
			return;
			Block_8:
			Mod.InversionResult(p, num3, array4, z);
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x000D6840 File Offset: 0x000D4A40
		public static uint[] Random(uint[] p)
		{
			int num = p.Length;
			uint[] array = Nat.Create(num);
			uint num2 = p[num - 1];
			num2 |= num2 >> 1;
			num2 |= num2 >> 2;
			num2 |= num2 >> 4;
			num2 |= num2 >> 8;
			num2 |= num2 >> 16;
			do
			{
				byte[] array2 = new byte[num << 2];
				Mod.RandomSource.NextBytes(array2);
				Pack.BE_To_UInt32(array2, 0, array);
				array[num - 1] &= num2;
			}
			while (Nat.Gte(num, array, p));
			return array;
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x000D68B4 File Offset: 0x000D4AB4
		public static void Add(uint[] p, uint[] x, uint[] y, uint[] z)
		{
			int len = p.Length;
			if (Nat.Add(len, x, y, z) != 0U)
			{
				Nat.SubFrom(len, p, z);
			}
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x000D68DC File Offset: 0x000D4ADC
		public static void Subtract(uint[] p, uint[] x, uint[] y, uint[] z)
		{
			int len = p.Length;
			if (Nat.Sub(len, x, y, z) != 0)
			{
				Nat.AddTo(len, p, z);
			}
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x000D6901 File Offset: 0x000D4B01
		private static void InversionResult(uint[] p, int ac, uint[] a, uint[] z)
		{
			if (ac < 0)
			{
				Nat.Add(p.Length, a, p, z);
				return;
			}
			Array.Copy(a, 0, z, 0, p.Length);
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x000D6920 File Offset: 0x000D4B20
		private static void InversionStep(uint[] p, uint[] u, int uLen, uint[] x, ref int xc)
		{
			int len = p.Length;
			int num = 0;
			while (u[0] == 0U)
			{
				Nat.ShiftDownWord(uLen, u, 0U);
				num += 32;
			}
			int trailingZeroes = Mod.GetTrailingZeroes(u[0]);
			if (trailingZeroes > 0)
			{
				Nat.ShiftDownBits(uLen, u, trailingZeroes, 0U);
				num += trailingZeroes;
			}
			for (int i = 0; i < num; i++)
			{
				if ((x[0] & 1U) != 0U)
				{
					if (xc < 0)
					{
						xc += (int)Nat.AddTo(len, p, x);
					}
					else
					{
						xc += Nat.SubFrom(len, p, x);
					}
				}
				Nat.ShiftDownBit(len, x, (uint)xc);
			}
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x000D69A8 File Offset: 0x000D4BA8
		private static int GetTrailingZeroes(uint x)
		{
			int num = 0;
			while ((x & 1U) == 0U)
			{
				x >>= 1;
				num++;
			}
			return num;
		}

		// Token: 0x04001940 RID: 6464
		private static readonly SecureRandom RandomSource = new SecureRandom();
	}
}
