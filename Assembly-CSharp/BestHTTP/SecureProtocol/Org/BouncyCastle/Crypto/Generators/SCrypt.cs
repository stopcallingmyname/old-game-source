using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000563 RID: 1379
	public class SCrypt
	{
		// Token: 0x060033C3 RID: 13251 RVA: 0x00136F3C File Offset: 0x0013513C
		public static byte[] Generate(byte[] P, byte[] S, int N, int r, int p, int dkLen)
		{
			if (P == null)
			{
				throw new ArgumentNullException("Passphrase P must be provided.");
			}
			if (S == null)
			{
				throw new ArgumentNullException("Salt S must be provided.");
			}
			if (N <= 1 || !SCrypt.IsPowerOf2(N))
			{
				throw new ArgumentException("Cost parameter N must be > 1 and a power of 2.");
			}
			if (r == 1 && N >= 65536)
			{
				throw new ArgumentException("Cost parameter N must be > 1 and < 65536.");
			}
			if (r < 1)
			{
				throw new ArgumentException("Block size r must be >= 1.");
			}
			int num = int.MaxValue / (128 * r * 8);
			if (p < 1 || p > num)
			{
				throw new ArgumentException(string.Concat(new object[]
				{
					"Parallelisation parameter p must be >= 1 and <= ",
					num,
					" (based on block size r of ",
					r,
					")"
				}));
			}
			if (dkLen < 1)
			{
				throw new ArgumentException("Generated key length dkLen must be >= 1.");
			}
			return SCrypt.MFcrypt(P, S, N, r, p, dkLen);
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x00137014 File Offset: 0x00135214
		private static byte[] MFcrypt(byte[] P, byte[] S, int N, int r, int p, int dkLen)
		{
			int num = r * 128;
			byte[] array = SCrypt.SingleIterationPBKDF2(P, S, p * num);
			uint[] array2 = null;
			byte[] result;
			try
			{
				int num2 = array.Length >> 2;
				array2 = new uint[num2];
				Pack.LE_To_UInt32(array, 0, array2);
				int num3 = num >> 2;
				for (int i = 0; i < num2; i += num3)
				{
					SCrypt.SMix(array2, i, N, r);
				}
				Pack.UInt32_To_LE(array2, array, 0);
				result = SCrypt.SingleIterationPBKDF2(P, array, dkLen);
			}
			finally
			{
				SCrypt.ClearAll(new Array[]
				{
					array,
					array2
				});
			}
			return result;
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x001370A8 File Offset: 0x001352A8
		private static byte[] SingleIterationPBKDF2(byte[] P, byte[] S, int dkLen)
		{
			Pkcs5S2ParametersGenerator pkcs5S2ParametersGenerator = new Pkcs5S2ParametersGenerator(new Sha256Digest());
			pkcs5S2ParametersGenerator.Init(P, S, 1);
			return ((KeyParameter)pkcs5S2ParametersGenerator.GenerateDerivedMacParameters(dkLen * 8)).GetKey();
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x001370D0 File Offset: 0x001352D0
		private static void SMix(uint[] B, int BOff, int N, int r)
		{
			int num = r * 32;
			uint[] array = new uint[16];
			uint[] array2 = new uint[16];
			uint[] array3 = new uint[num];
			uint[] array4 = new uint[num];
			uint[][] array5 = new uint[N][];
			try
			{
				Array.Copy(B, BOff, array4, 0, num);
				for (int i = 0; i < N; i++)
				{
					array5[i] = (uint[])array4.Clone();
					SCrypt.BlockMix(array4, array, array2, array3, r);
				}
				uint num2 = (uint)(N - 1);
				for (int j = 0; j < N; j++)
				{
					uint num3 = array4[num - 16] & num2;
					SCrypt.Xor(array4, array5[(int)num3], 0, array4);
					SCrypt.BlockMix(array4, array, array2, array3, r);
				}
				Array.Copy(array4, 0, B, BOff, num);
			}
			finally
			{
				Array[] arrays = array5;
				SCrypt.ClearAll(arrays);
				SCrypt.ClearAll(new Array[]
				{
					array4,
					array,
					array2,
					array3
				});
			}
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x001371C0 File Offset: 0x001353C0
		private static void BlockMix(uint[] B, uint[] X1, uint[] X2, uint[] Y, int r)
		{
			Array.Copy(B, B.Length - 16, X1, 0, 16);
			int num = 0;
			int num2 = 0;
			int num3 = B.Length >> 1;
			for (int i = 2 * r; i > 0; i--)
			{
				SCrypt.Xor(X1, B, num, X2);
				Salsa20Engine.SalsaCore(8, X2, X1);
				Array.Copy(X1, 0, Y, num2, 16);
				num2 = num3 + num - num2;
				num += 16;
			}
			Array.Copy(Y, 0, B, 0, Y.Length);
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x0013722C File Offset: 0x0013542C
		private static void Xor(uint[] a, uint[] b, int bOff, uint[] output)
		{
			for (int i = output.Length - 1; i >= 0; i--)
			{
				output[i] = (a[i] ^ b[bOff + i]);
			}
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x00137255 File Offset: 0x00135455
		private static void Clear(Array array)
		{
			if (array != null)
			{
				Array.Clear(array, 0, array.Length);
			}
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x00137268 File Offset: 0x00135468
		private static void ClearAll(params Array[] arrays)
		{
			for (int i = 0; i < arrays.Length; i++)
			{
				SCrypt.Clear(arrays[i]);
			}
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x0013728D File Offset: 0x0013548D
		private static bool IsPowerOf2(int x)
		{
			return (x & x - 1) == 0;
		}
	}
}
