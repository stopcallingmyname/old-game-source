using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw
{
	// Token: 0x0200030F RID: 783
	internal abstract class Nat320
	{
		// Token: 0x06001DC3 RID: 7619 RVA: 0x000E08A5 File Offset: 0x000DEAA5
		public static void Copy64(ulong[] x, ulong[] z)
		{
			z[0] = x[0];
			z[1] = x[1];
			z[2] = x[2];
			z[3] = x[3];
			z[4] = x[4];
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x000E08C5 File Offset: 0x000DEAC5
		public static void Copy64(ulong[] x, int xOff, ulong[] z, int zOff)
		{
			z[zOff] = x[xOff];
			z[zOff + 1] = x[xOff + 1];
			z[zOff + 2] = x[xOff + 2];
			z[zOff + 3] = x[xOff + 3];
			z[zOff + 4] = x[xOff + 4];
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x000E08F5 File Offset: 0x000DEAF5
		public static ulong[] Create64()
		{
			return new ulong[5];
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x000E08FD File Offset: 0x000DEAFD
		public static ulong[] CreateExt64()
		{
			return new ulong[10];
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x000E0908 File Offset: 0x000DEB08
		public static bool Eq64(ulong[] x, ulong[] y)
		{
			for (int i = 4; i >= 0; i--)
			{
				if (x[i] != y[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x000E092C File Offset: 0x000DEB2C
		public static ulong[] FromBigInteger64(BigInteger x)
		{
			if (x.SignValue < 0 || x.BitLength > 320)
			{
				throw new ArgumentException();
			}
			ulong[] array = Nat320.Create64();
			int num = 0;
			while (x.SignValue != 0)
			{
				array[num++] = (ulong)x.LongValue;
				x = x.ShiftRight(64);
			}
			return array;
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x000E0980 File Offset: 0x000DEB80
		public static bool IsOne64(ulong[] x)
		{
			if (x[0] != 1UL)
			{
				return false;
			}
			for (int i = 1; i < 5; i++)
			{
				if (x[i] != 0UL)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x000E09AC File Offset: 0x000DEBAC
		public static bool IsZero64(ulong[] x)
		{
			for (int i = 0; i < 5; i++)
			{
				if (x[i] != 0UL)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x000E09D0 File Offset: 0x000DEBD0
		public static BigInteger ToBigInteger64(ulong[] x)
		{
			byte[] array = new byte[40];
			for (int i = 0; i < 5; i++)
			{
				ulong num = x[i];
				if (num != 0UL)
				{
					Pack.UInt64_To_BE(num, array, 4 - i << 3);
				}
			}
			return new BigInteger(1, array);
		}
	}
}
