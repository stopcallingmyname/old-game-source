using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.Srp
{
	// Token: 0x020005D1 RID: 1489
	public class Srp6Utilities
	{
		// Token: 0x06003914 RID: 14612 RVA: 0x00165879 File Offset: 0x00163A79
		public static BigInteger CalculateK(IDigest digest, BigInteger N, BigInteger g)
		{
			return Srp6Utilities.HashPaddedPair(digest, N, N, g);
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x00165884 File Offset: 0x00163A84
		public static BigInteger CalculateU(IDigest digest, BigInteger N, BigInteger A, BigInteger B)
		{
			return Srp6Utilities.HashPaddedPair(digest, N, A, B);
		}

		// Token: 0x06003916 RID: 14614 RVA: 0x00165890 File Offset: 0x00163A90
		public static BigInteger CalculateX(IDigest digest, BigInteger N, byte[] salt, byte[] identity, byte[] password)
		{
			byte[] array = new byte[digest.GetDigestSize()];
			digest.BlockUpdate(identity, 0, identity.Length);
			digest.Update(58);
			digest.BlockUpdate(password, 0, password.Length);
			digest.DoFinal(array, 0);
			digest.BlockUpdate(salt, 0, salt.Length);
			digest.BlockUpdate(array, 0, array.Length);
			digest.DoFinal(array, 0);
			return new BigInteger(1, array);
		}

		// Token: 0x06003917 RID: 14615 RVA: 0x001658F8 File Offset: 0x00163AF8
		public static BigInteger GeneratePrivateValue(IDigest digest, BigInteger N, BigInteger g, SecureRandom random)
		{
			int num = Math.Min(256, N.BitLength / 2);
			BigInteger min = BigInteger.One.ShiftLeft(num - 1);
			BigInteger max = N.Subtract(BigInteger.One);
			return BigIntegers.CreateRandomInRange(min, max, random);
		}

		// Token: 0x06003918 RID: 14616 RVA: 0x00165938 File Offset: 0x00163B38
		public static BigInteger ValidatePublicValue(BigInteger N, BigInteger val)
		{
			val = val.Mod(N);
			if (val.Equals(BigInteger.Zero))
			{
				throw new CryptoException("Invalid public value: 0");
			}
			return val;
		}

		// Token: 0x06003919 RID: 14617 RVA: 0x0016595C File Offset: 0x00163B5C
		public static BigInteger CalculateM1(IDigest digest, BigInteger N, BigInteger A, BigInteger B, BigInteger S)
		{
			return Srp6Utilities.HashPaddedTriplet(digest, N, A, B, S);
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x0016595C File Offset: 0x00163B5C
		public static BigInteger CalculateM2(IDigest digest, BigInteger N, BigInteger A, BigInteger M1, BigInteger S)
		{
			return Srp6Utilities.HashPaddedTriplet(digest, N, A, M1, S);
		}

		// Token: 0x0600391B RID: 14619 RVA: 0x0016596C File Offset: 0x00163B6C
		public static BigInteger CalculateKey(IDigest digest, BigInteger N, BigInteger S)
		{
			int length = (N.BitLength + 7) / 8;
			byte[] padded = Srp6Utilities.GetPadded(S, length);
			digest.BlockUpdate(padded, 0, padded.Length);
			byte[] array = new byte[digest.GetDigestSize()];
			digest.DoFinal(array, 0);
			return new BigInteger(1, array);
		}

		// Token: 0x0600391C RID: 14620 RVA: 0x001659B4 File Offset: 0x00163BB4
		private static BigInteger HashPaddedTriplet(IDigest digest, BigInteger N, BigInteger n1, BigInteger n2, BigInteger n3)
		{
			int length = (N.BitLength + 7) / 8;
			byte[] padded = Srp6Utilities.GetPadded(n1, length);
			byte[] padded2 = Srp6Utilities.GetPadded(n2, length);
			byte[] padded3 = Srp6Utilities.GetPadded(n3, length);
			digest.BlockUpdate(padded, 0, padded.Length);
			digest.BlockUpdate(padded2, 0, padded2.Length);
			digest.BlockUpdate(padded3, 0, padded3.Length);
			byte[] array = new byte[digest.GetDigestSize()];
			digest.DoFinal(array, 0);
			return new BigInteger(1, array);
		}

		// Token: 0x0600391D RID: 14621 RVA: 0x00165A28 File Offset: 0x00163C28
		private static BigInteger HashPaddedPair(IDigest digest, BigInteger N, BigInteger n1, BigInteger n2)
		{
			int length = (N.BitLength + 7) / 8;
			byte[] padded = Srp6Utilities.GetPadded(n1, length);
			byte[] padded2 = Srp6Utilities.GetPadded(n2, length);
			digest.BlockUpdate(padded, 0, padded.Length);
			digest.BlockUpdate(padded2, 0, padded2.Length);
			byte[] array = new byte[digest.GetDigestSize()];
			digest.DoFinal(array, 0);
			return new BigInteger(1, array);
		}

		// Token: 0x0600391E RID: 14622 RVA: 0x00165A84 File Offset: 0x00163C84
		private static byte[] GetPadded(BigInteger n, int length)
		{
			byte[] array = BigIntegers.AsUnsignedByteArray(n);
			if (array.Length < length)
			{
				byte[] array2 = new byte[length];
				Array.Copy(array, 0, array2, length - array.Length, array.Length);
				array = array2;
			}
			return array;
		}
	}
}
