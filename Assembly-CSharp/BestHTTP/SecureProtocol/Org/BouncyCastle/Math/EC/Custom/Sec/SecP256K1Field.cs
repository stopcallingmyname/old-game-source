using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000372 RID: 882
	internal class SecP256K1Field
	{
		// Token: 0x0600226A RID: 8810 RVA: 0x000F736D File Offset: 0x000F556D
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat256.Add(x, y, z) != 0U || (z[7] == 4294967295U && Nat256.Gte(z, SecP256K1Field.P)))
			{
				Nat.Add33To(8, 977U, z);
			}
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x000F739C File Offset: 0x000F559C
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if ((Nat.Add(16, xx, yy, zz) != 0U || (zz[15] == 4294967295U && Nat.Gte(16, zz, SecP256K1Field.PExt))) && Nat.AddTo(SecP256K1Field.PExtInv.Length, SecP256K1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(16, zz, SecP256K1Field.PExtInv.Length);
			}
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x000F73EF File Offset: 0x000F55EF
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(8, x, z) != 0U || (z[7] == 4294967295U && Nat256.Gte(z, SecP256K1Field.P)))
			{
				Nat.Add33To(8, 977U, z);
			}
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x000F741C File Offset: 0x000F561C
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat256.FromBigInteger(x);
			if (array[7] == 4294967295U && Nat256.Gte(array, SecP256K1Field.P))
			{
				Nat256.SubFrom(SecP256K1Field.P, array);
			}
			return array;
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x000F7450 File Offset: 0x000F5650
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(8, x, 0U, z);
				return;
			}
			uint c = Nat256.Add(x, SecP256K1Field.P, z);
			Nat.ShiftDownBit(8, z, c);
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x000F7488 File Offset: 0x000F5688
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat256.CreateExt();
			Nat256.Mul(x, y, array);
			SecP256K1Field.Reduce(array, z);
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x000F74AC File Offset: 0x000F56AC
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if ((Nat256.MulAddTo(x, y, zz) != 0U || (zz[15] == 4294967295U && Nat.Gte(16, zz, SecP256K1Field.PExt))) && Nat.AddTo(SecP256K1Field.PExtInv.Length, SecP256K1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(16, zz, SecP256K1Field.PExtInv.Length);
			}
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x000F74FD File Offset: 0x000F56FD
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat256.IsZero(x))
			{
				Nat256.Zero(z);
				return;
			}
			Nat256.Sub(SecP256K1Field.P, x, z);
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x000F751C File Offset: 0x000F571C
		public static void Reduce(uint[] xx, uint[] z)
		{
			ulong y = Nat256.Mul33Add(977U, xx, 8, xx, 0, z, 0);
			if (Nat256.Mul33DWordAdd(977U, y, z, 0) != 0U || (z[7] == 4294967295U && Nat256.Gte(z, SecP256K1Field.P)))
			{
				Nat.Add33To(8, 977U, z);
			}
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x000F7569 File Offset: 0x000F5769
		public static void Reduce32(uint x, uint[] z)
		{
			if ((x != 0U && Nat256.Mul33WordAdd(977U, x, z, 0) != 0U) || (z[7] == 4294967295U && Nat256.Gte(z, SecP256K1Field.P)))
			{
				Nat.Add33To(8, 977U, z);
			}
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x000F75A0 File Offset: 0x000F57A0
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat256.CreateExt();
			Nat256.Square(x, array);
			SecP256K1Field.Reduce(array, z);
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x000F75C4 File Offset: 0x000F57C4
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat256.CreateExt();
			Nat256.Square(x, array);
			SecP256K1Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat256.Square(z, array);
				SecP256K1Field.Reduce(array, z);
			}
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x000F75FE File Offset: 0x000F57FE
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat256.Sub(x, y, z) != 0)
			{
				Nat.Sub33From(8, 977U, z);
			}
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x000F7617 File Offset: 0x000F5817
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(16, xx, yy, zz) != 0 && Nat.SubFrom(SecP256K1Field.PExtInv.Length, SecP256K1Field.PExtInv, zz) != 0)
			{
				Nat.DecAt(16, zz, SecP256K1Field.PExtInv.Length);
			}
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x000F7649 File Offset: 0x000F5849
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(8, x, 0U, z) != 0U || (z[7] == 4294967295U && Nat256.Gte(z, SecP256K1Field.P)))
			{
				Nat.Add33To(8, 977U, z);
			}
		}

		// Token: 0x04001A53 RID: 6739
		internal static readonly uint[] P = new uint[]
		{
			4294966319U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x04001A54 RID: 6740
		internal static readonly uint[] PExt = new uint[]
		{
			954529U,
			1954U,
			1U,
			0U,
			0U,
			0U,
			0U,
			0U,
			4294965342U,
			4294967293U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x04001A55 RID: 6741
		private static readonly uint[] PExtInv = new uint[]
		{
			4294012767U,
			4294965341U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			1953U,
			2U
		};

		// Token: 0x04001A56 RID: 6742
		private const uint P7 = 4294967295U;

		// Token: 0x04001A57 RID: 6743
		private const uint PExt15 = 4294967295U;

		// Token: 0x04001A58 RID: 6744
		private const uint PInv33 = 977U;
	}
}
