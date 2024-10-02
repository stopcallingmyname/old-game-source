using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities
{
	// Token: 0x02000261 RID: 609
	public abstract class BigIntegers
	{
		// Token: 0x0600165B RID: 5723 RVA: 0x000B09A8 File Offset: 0x000AEBA8
		public static byte[] AsUnsignedByteArray(BigInteger n)
		{
			return n.ToByteArrayUnsigned();
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x000B09B0 File Offset: 0x000AEBB0
		public static byte[] AsUnsignedByteArray(int length, BigInteger n)
		{
			byte[] array = n.ToByteArrayUnsigned();
			if (array.Length > length)
			{
				throw new ArgumentException("standard length exceeded", "n");
			}
			if (array.Length == length)
			{
				return array;
			}
			byte[] array2 = new byte[length];
			Array.Copy(array, 0, array2, array2.Length - array.Length, array.Length);
			return array2;
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x000B09FC File Offset: 0x000AEBFC
		public static BigInteger CreateRandomInRange(BigInteger min, BigInteger max, SecureRandom random)
		{
			int num = min.CompareTo(max);
			if (num >= 0)
			{
				if (num > 0)
				{
					throw new ArgumentException("'min' may not be greater than 'max'");
				}
				return min;
			}
			else
			{
				if (min.BitLength > max.BitLength / 2)
				{
					return BigIntegers.CreateRandomInRange(BigInteger.Zero, max.Subtract(min), random).Add(min);
				}
				for (int i = 0; i < 1000; i++)
				{
					BigInteger bigInteger = new BigInteger(max.BitLength, random);
					if (bigInteger.CompareTo(min) >= 0 && bigInteger.CompareTo(max) <= 0)
					{
						return bigInteger;
					}
				}
				return new BigInteger(max.Subtract(min).BitLength - 1, random).Add(min);
			}
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x000B0A9C File Offset: 0x000AEC9C
		public static int GetUnsignedByteLength(BigInteger n)
		{
			return (n.BitLength + 7) / 8;
		}

		// Token: 0x04001685 RID: 5765
		private const int MaxIterations = 1000;
	}
}
