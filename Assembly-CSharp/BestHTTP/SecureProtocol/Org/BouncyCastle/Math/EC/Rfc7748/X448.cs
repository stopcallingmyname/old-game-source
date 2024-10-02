using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc8032;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc7748
{
	// Token: 0x02000337 RID: 823
	public abstract class X448
	{
		// Token: 0x06002010 RID: 8208 RVA: 0x000ED413 File Offset: 0x000EB613
		public static bool CalculateAgreement(byte[] k, int kOff, byte[] u, int uOff, byte[] r, int rOff)
		{
			X448.ScalarMult(k, kOff, u, uOff, r, rOff);
			return !Arrays.AreAllZeroes(r, rOff, 56);
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x000E8468 File Offset: 0x000E6668
		private static uint Decode32(byte[] bs, int off)
		{
			return (uint)((int)bs[off] | (int)bs[++off] << 8 | (int)bs[++off] << 16 | (int)bs[++off] << 24);
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x000ED430 File Offset: 0x000EB630
		private static void DecodeScalar(byte[] k, int kOff, uint[] n)
		{
			for (int i = 0; i < 14; i++)
			{
				n[i] = X448.Decode32(k, kOff + i * 4);
			}
			n[0] &= 4294967292U;
			n[13] |= 2147483648U;
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x000ED476 File Offset: 0x000EB676
		public static void GeneratePrivateKey(SecureRandom random, byte[] k)
		{
			random.NextBytes(k);
			int num = 0;
			k[num] &= 252;
			int num2 = 55;
			k[num2] |= 128;
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x000ED4A2 File Offset: 0x000EB6A2
		public static void GeneratePublicKey(byte[] k, int kOff, byte[] r, int rOff)
		{
			X448.ScalarMultBase(k, kOff, r, rOff);
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x000ED4B0 File Offset: 0x000EB6B0
		private static void PointDouble(uint[] x, uint[] z)
		{
			uint[] array = X448Field.Create();
			uint[] array2 = X448Field.Create();
			X448Field.Add(x, z, array);
			X448Field.Sub(x, z, array2);
			X448Field.Sqr(array, array);
			X448Field.Sqr(array2, array2);
			X448Field.Mul(array, array2, x);
			X448Field.Sub(array, array2, array);
			X448Field.Mul(array, 39082U, z);
			X448Field.Add(z, array2, z);
			X448Field.Mul(z, array, z);
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x000ED513 File Offset: 0x000EB713
		public static void Precompute()
		{
			Ed448.Precompute();
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x000ED51C File Offset: 0x000EB71C
		public static void ScalarMult(byte[] k, int kOff, byte[] u, int uOff, byte[] r, int rOff)
		{
			uint[] array = new uint[14];
			X448.DecodeScalar(k, kOff, array);
			uint[] array2 = X448Field.Create();
			X448Field.Decode(u, uOff, array2);
			uint[] array3 = X448Field.Create();
			X448Field.Copy(array2, 0, array3, 0);
			uint[] array4 = X448Field.Create();
			array4[0] = 1U;
			uint[] array5 = X448Field.Create();
			array5[0] = 1U;
			uint[] array6 = X448Field.Create();
			uint[] array7 = X448Field.Create();
			uint[] array8 = X448Field.Create();
			int num = 447;
			int num2 = 1;
			do
			{
				X448Field.Add(array5, array6, array7);
				X448Field.Sub(array5, array6, array5);
				X448Field.Add(array3, array4, array6);
				X448Field.Sub(array3, array4, array3);
				X448Field.Mul(array7, array3, array7);
				X448Field.Mul(array5, array6, array5);
				X448Field.Sqr(array6, array6);
				X448Field.Sqr(array3, array3);
				X448Field.Sub(array6, array3, array8);
				X448Field.Mul(array8, 39082U, array4);
				X448Field.Add(array4, array3, array4);
				X448Field.Mul(array4, array8, array4);
				X448Field.Mul(array3, array6, array3);
				X448Field.Sub(array7, array5, array6);
				X448Field.Add(array7, array5, array5);
				X448Field.Sqr(array5, array5);
				X448Field.Sqr(array6, array6);
				X448Field.Mul(array6, array2, array6);
				num--;
				int num3 = num >> 5;
				int num4 = num & 31;
				int num5 = (int)(array[num3] >> num4 & 1U);
				num2 ^= num5;
				X448Field.CSwap(num2, array3, array5);
				X448Field.CSwap(num2, array4, array6);
				num2 = num5;
			}
			while (num >= 2);
			for (int i = 0; i < 2; i++)
			{
				X448.PointDouble(array3, array4);
			}
			X448Field.Inv(array4, array4);
			X448Field.Mul(array3, array4, array3);
			X448Field.Normalize(array3);
			X448Field.Encode(array3, r, rOff);
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x000ED6B8 File Offset: 0x000EB8B8
		public static void ScalarMultBase(byte[] k, int kOff, byte[] r, int rOff)
		{
			uint[] array = X448Field.Create();
			uint[] y = X448Field.Create();
			Ed448.ScalarMultBaseXY(k, kOff, array, y);
			X448Field.Inv(array, array);
			X448Field.Mul(array, y, array);
			X448Field.Sqr(array, array);
			X448Field.Normalize(array);
			X448Field.Encode(array, r, rOff);
		}

		// Token: 0x040019D5 RID: 6613
		public const int PointSize = 56;

		// Token: 0x040019D6 RID: 6614
		public const int ScalarSize = 56;

		// Token: 0x040019D7 RID: 6615
		private const uint C_A = 156326U;

		// Token: 0x040019D8 RID: 6616
		private const uint C_A24 = 39082U;
	}
}
