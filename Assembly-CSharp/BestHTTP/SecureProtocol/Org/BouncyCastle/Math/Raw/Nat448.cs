using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw
{
	// Token: 0x02000311 RID: 785
	internal abstract class Nat448
	{
		// Token: 0x06001DD0 RID: 7632 RVA: 0x000E0B3A File Offset: 0x000DED3A
		public static void Copy64(ulong[] x, ulong[] z)
		{
			z[0] = x[0];
			z[1] = x[1];
			z[2] = x[2];
			z[3] = x[3];
			z[4] = x[4];
			z[5] = x[5];
			z[6] = x[6];
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x000E0B68 File Offset: 0x000DED68
		public static void Copy64(ulong[] x, int xOff, ulong[] z, int zOff)
		{
			z[zOff] = x[xOff];
			z[zOff + 1] = x[xOff + 1];
			z[zOff + 2] = x[xOff + 2];
			z[zOff + 3] = x[xOff + 3];
			z[zOff + 4] = x[xOff + 4];
			z[zOff + 5] = x[xOff + 5];
			z[zOff + 6] = x[xOff + 6];
		}

		// Token: 0x06001DD2 RID: 7634 RVA: 0x000E0BB7 File Offset: 0x000DEDB7
		public static ulong[] Create64()
		{
			return new ulong[7];
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x000E0BBF File Offset: 0x000DEDBF
		public static ulong[] CreateExt64()
		{
			return new ulong[14];
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x000E0BC8 File Offset: 0x000DEDC8
		public static bool Eq64(ulong[] x, ulong[] y)
		{
			for (int i = 6; i >= 0; i--)
			{
				if (x[i] != y[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x000E0BEC File Offset: 0x000DEDEC
		public static ulong[] FromBigInteger64(BigInteger x)
		{
			if (x.SignValue < 0 || x.BitLength > 448)
			{
				throw new ArgumentException();
			}
			ulong[] array = Nat448.Create64();
			int num = 0;
			while (x.SignValue != 0)
			{
				array[num++] = (ulong)x.LongValue;
				x = x.ShiftRight(64);
			}
			return array;
		}

		// Token: 0x06001DD6 RID: 7638 RVA: 0x000E0C40 File Offset: 0x000DEE40
		public static bool IsOne64(ulong[] x)
		{
			if (x[0] != 1UL)
			{
				return false;
			}
			for (int i = 1; i < 7; i++)
			{
				if (x[i] != 0UL)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x000E0C6C File Offset: 0x000DEE6C
		public static bool IsZero64(ulong[] x)
		{
			for (int i = 0; i < 7; i++)
			{
				if (x[i] != 0UL)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x000E0C90 File Offset: 0x000DEE90
		public static BigInteger ToBigInteger64(ulong[] x)
		{
			byte[] array = new byte[56];
			for (int i = 0; i < 7; i++)
			{
				ulong num = x[i];
				if (num != 0UL)
				{
					Pack.UInt64_To_BE(num, array, 6 - i << 3);
				}
			}
			return new BigInteger(1, array);
		}
	}
}
