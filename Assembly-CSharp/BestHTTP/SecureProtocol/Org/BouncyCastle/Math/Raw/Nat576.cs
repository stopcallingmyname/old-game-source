using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw
{
	// Token: 0x02000313 RID: 787
	internal abstract class Nat576
	{
		// Token: 0x06001DDD RID: 7645 RVA: 0x000E0DFA File Offset: 0x000DEFFA
		public static void Copy64(ulong[] x, ulong[] z)
		{
			z[0] = x[0];
			z[1] = x[1];
			z[2] = x[2];
			z[3] = x[3];
			z[4] = x[4];
			z[5] = x[5];
			z[6] = x[6];
			z[7] = x[7];
			z[8] = x[8];
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x000E0E34 File Offset: 0x000DF034
		public static void Copy64(ulong[] x, int xOff, ulong[] z, int zOff)
		{
			z[zOff] = x[xOff];
			z[zOff + 1] = x[xOff + 1];
			z[zOff + 2] = x[xOff + 2];
			z[zOff + 3] = x[xOff + 3];
			z[zOff + 4] = x[xOff + 4];
			z[zOff + 5] = x[xOff + 5];
			z[zOff + 6] = x[xOff + 6];
			z[zOff + 7] = x[xOff + 7];
			z[zOff + 8] = x[xOff + 8];
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x000E0E97 File Offset: 0x000DF097
		public static ulong[] Create64()
		{
			return new ulong[9];
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x000E0EA0 File Offset: 0x000DF0A0
		public static ulong[] CreateExt64()
		{
			return new ulong[18];
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x000E0EAC File Offset: 0x000DF0AC
		public static bool Eq64(ulong[] x, ulong[] y)
		{
			for (int i = 8; i >= 0; i--)
			{
				if (x[i] != y[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x000E0ED0 File Offset: 0x000DF0D0
		public static ulong[] FromBigInteger64(BigInteger x)
		{
			if (x.SignValue < 0 || x.BitLength > 576)
			{
				throw new ArgumentException();
			}
			ulong[] array = Nat576.Create64();
			int num = 0;
			while (x.SignValue != 0)
			{
				array[num++] = (ulong)x.LongValue;
				x = x.ShiftRight(64);
			}
			return array;
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x000E0F24 File Offset: 0x000DF124
		public static bool IsOne64(ulong[] x)
		{
			if (x[0] != 1UL)
			{
				return false;
			}
			for (int i = 1; i < 9; i++)
			{
				if (x[i] != 0UL)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x000E0F50 File Offset: 0x000DF150
		public static bool IsZero64(ulong[] x)
		{
			for (int i = 0; i < 9; i++)
			{
				if (x[i] != 0UL)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x000E0F74 File Offset: 0x000DF174
		public static BigInteger ToBigInteger64(ulong[] x)
		{
			byte[] array = new byte[72];
			for (int i = 0; i < 9; i++)
			{
				ulong num = x[i];
				if (num != 0UL)
				{
					Pack.UInt64_To_BE(num, array, 8 - i << 3);
				}
			}
			return new BigInteger(1, array);
		}
	}
}
