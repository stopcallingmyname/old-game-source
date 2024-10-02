using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200035E RID: 862
	internal class SecP160R2Field
	{
		// Token: 0x06002135 RID: 8501 RVA: 0x000F2999 File Offset: 0x000F0B99
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat160.Add(x, y, z) != 0U || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R2Field.P)))
			{
				Nat.Add33To(5, 21389U, z);
			}
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x000F29C8 File Offset: 0x000F0BC8
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if ((Nat.Add(10, xx, yy, zz) != 0U || (zz[9] == 4294967295U && Nat.Gte(10, zz, SecP160R2Field.PExt))) && Nat.AddTo(SecP160R2Field.PExtInv.Length, SecP160R2Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(10, zz, SecP160R2Field.PExtInv.Length);
			}
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x000F2A1B File Offset: 0x000F0C1B
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(5, x, z) != 0U || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R2Field.P)))
			{
				Nat.Add33To(5, 21389U, z);
			}
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x000F2A48 File Offset: 0x000F0C48
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat160.FromBigInteger(x);
			if (array[4] == 4294967295U && Nat160.Gte(array, SecP160R2Field.P))
			{
				Nat160.SubFrom(SecP160R2Field.P, array);
			}
			return array;
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x000F2A7C File Offset: 0x000F0C7C
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(5, x, 0U, z);
				return;
			}
			uint c = Nat160.Add(x, SecP160R2Field.P, z);
			Nat.ShiftDownBit(5, z, c);
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x000F2AB4 File Offset: 0x000F0CB4
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat160.CreateExt();
			Nat160.Mul(x, y, array);
			SecP160R2Field.Reduce(array, z);
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x000F2AD8 File Offset: 0x000F0CD8
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if ((Nat160.MulAddTo(x, y, zz) != 0U || (zz[9] == 4294967295U && Nat.Gte(10, zz, SecP160R2Field.PExt))) && Nat.AddTo(SecP160R2Field.PExtInv.Length, SecP160R2Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(10, zz, SecP160R2Field.PExtInv.Length);
			}
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x000F2B29 File Offset: 0x000F0D29
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat160.IsZero(x))
			{
				Nat160.Zero(z);
				return;
			}
			Nat160.Sub(SecP160R2Field.P, x, z);
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x000F2B48 File Offset: 0x000F0D48
		public static void Reduce(uint[] xx, uint[] z)
		{
			ulong y = Nat160.Mul33Add(21389U, xx, 5, xx, 0, z, 0);
			if (Nat160.Mul33DWordAdd(21389U, y, z, 0) != 0U || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R2Field.P)))
			{
				Nat.Add33To(5, 21389U, z);
			}
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x000F2B95 File Offset: 0x000F0D95
		public static void Reduce32(uint x, uint[] z)
		{
			if ((x != 0U && Nat160.Mul33WordAdd(21389U, x, z, 0) != 0U) || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R2Field.P)))
			{
				Nat.Add33To(5, 21389U, z);
			}
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x000F2BCC File Offset: 0x000F0DCC
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat160.CreateExt();
			Nat160.Square(x, array);
			SecP160R2Field.Reduce(array, z);
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x000F2BF0 File Offset: 0x000F0DF0
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat160.CreateExt();
			Nat160.Square(x, array);
			SecP160R2Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat160.Square(z, array);
				SecP160R2Field.Reduce(array, z);
			}
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x000F2C2A File Offset: 0x000F0E2A
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat160.Sub(x, y, z) != 0)
			{
				Nat.Sub33From(5, 21389U, z);
			}
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x000F2C43 File Offset: 0x000F0E43
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(10, xx, yy, zz) != 0 && Nat.SubFrom(SecP160R2Field.PExtInv.Length, SecP160R2Field.PExtInv, zz) != 0)
			{
				Nat.DecAt(10, zz, SecP160R2Field.PExtInv.Length);
			}
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x000F2C75 File Offset: 0x000F0E75
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(5, x, 0U, z) != 0U || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R2Field.P)))
			{
				Nat.Add33To(5, 21389U, z);
			}
		}

		// Token: 0x04001A18 RID: 6680
		internal static readonly uint[] P = new uint[]
		{
			4294945907U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x04001A19 RID: 6681
		internal static readonly uint[] PExt = new uint[]
		{
			457489321U,
			42778U,
			1U,
			0U,
			0U,
			4294924518U,
			4294967293U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x04001A1A RID: 6682
		private static readonly uint[] PExtInv = new uint[]
		{
			3837477975U,
			4294924517U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			42777U,
			2U
		};

		// Token: 0x04001A1B RID: 6683
		private const uint P4 = 4294967295U;

		// Token: 0x04001A1C RID: 6684
		private const uint PExt9 = 4294967295U;

		// Token: 0x04001A1D RID: 6685
		private const uint PInv33 = 21389U;
	}
}
