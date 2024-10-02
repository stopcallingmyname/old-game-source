using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000362 RID: 866
	internal class SecP192K1Field
	{
		// Token: 0x06002171 RID: 8561 RVA: 0x000F3799 File Offset: 0x000F1999
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat192.Add(x, y, z) != 0U || (z[5] == 4294967295U && Nat192.Gte(z, SecP192K1Field.P)))
			{
				Nat.Add33To(6, 4553U, z);
			}
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x000F37C8 File Offset: 0x000F19C8
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if ((Nat.Add(12, xx, yy, zz) != 0U || (zz[11] == 4294967295U && Nat.Gte(12, zz, SecP192K1Field.PExt))) && Nat.AddTo(SecP192K1Field.PExtInv.Length, SecP192K1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(12, zz, SecP192K1Field.PExtInv.Length);
			}
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x000F381B File Offset: 0x000F1A1B
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(6, x, z) != 0U || (z[5] == 4294967295U && Nat192.Gte(z, SecP192K1Field.P)))
			{
				Nat.Add33To(6, 4553U, z);
			}
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x000F3848 File Offset: 0x000F1A48
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat192.FromBigInteger(x);
			if (array[5] == 4294967295U && Nat192.Gte(array, SecP192K1Field.P))
			{
				Nat192.SubFrom(SecP192K1Field.P, array);
			}
			return array;
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x000F387C File Offset: 0x000F1A7C
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(6, x, 0U, z);
				return;
			}
			uint c = Nat192.Add(x, SecP192K1Field.P, z);
			Nat.ShiftDownBit(6, z, c);
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x000F38B4 File Offset: 0x000F1AB4
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat192.CreateExt();
			Nat192.Mul(x, y, array);
			SecP192K1Field.Reduce(array, z);
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x000F38D8 File Offset: 0x000F1AD8
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if ((Nat192.MulAddTo(x, y, zz) != 0U || (zz[11] == 4294967295U && Nat.Gte(12, zz, SecP192K1Field.PExt))) && Nat.AddTo(SecP192K1Field.PExtInv.Length, SecP192K1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(12, zz, SecP192K1Field.PExtInv.Length);
			}
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x000F3929 File Offset: 0x000F1B29
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat192.IsZero(x))
			{
				Nat192.Zero(z);
				return;
			}
			Nat192.Sub(SecP192K1Field.P, x, z);
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x000F3948 File Offset: 0x000F1B48
		public static void Reduce(uint[] xx, uint[] z)
		{
			ulong y = Nat192.Mul33Add(4553U, xx, 6, xx, 0, z, 0);
			if (Nat192.Mul33DWordAdd(4553U, y, z, 0) != 0U || (z[5] == 4294967295U && Nat192.Gte(z, SecP192K1Field.P)))
			{
				Nat.Add33To(6, 4553U, z);
			}
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x000F3995 File Offset: 0x000F1B95
		public static void Reduce32(uint x, uint[] z)
		{
			if ((x != 0U && Nat192.Mul33WordAdd(4553U, x, z, 0) != 0U) || (z[5] == 4294967295U && Nat192.Gte(z, SecP192K1Field.P)))
			{
				Nat.Add33To(6, 4553U, z);
			}
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x000F39CC File Offset: 0x000F1BCC
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat192.CreateExt();
			Nat192.Square(x, array);
			SecP192K1Field.Reduce(array, z);
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x000F39F0 File Offset: 0x000F1BF0
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat192.CreateExt();
			Nat192.Square(x, array);
			SecP192K1Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat192.Square(z, array);
				SecP192K1Field.Reduce(array, z);
			}
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x000F3A2A File Offset: 0x000F1C2A
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat192.Sub(x, y, z) != 0)
			{
				Nat.Sub33From(6, 4553U, z);
			}
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x000F3A43 File Offset: 0x000F1C43
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(12, xx, yy, zz) != 0 && Nat.SubFrom(SecP192K1Field.PExtInv.Length, SecP192K1Field.PExtInv, zz) != 0)
			{
				Nat.DecAt(12, zz, SecP192K1Field.PExtInv.Length);
			}
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x000F3A75 File Offset: 0x000F1C75
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(6, x, 0U, z) != 0U || (z[5] == 4294967295U && Nat192.Gte(z, SecP192K1Field.P)))
			{
				Nat.Add33To(6, 4553U, z);
			}
		}

		// Token: 0x04001A24 RID: 6692
		internal static readonly uint[] P = new uint[]
		{
			4294962743U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x04001A25 RID: 6693
		internal static readonly uint[] PExt = new uint[]
		{
			20729809U,
			9106U,
			1U,
			0U,
			0U,
			0U,
			4294958190U,
			4294967293U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x04001A26 RID: 6694
		private static readonly uint[] PExtInv = new uint[]
		{
			4274237487U,
			4294958189U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			9105U,
			2U
		};

		// Token: 0x04001A27 RID: 6695
		private const uint P5 = 4294967295U;

		// Token: 0x04001A28 RID: 6696
		private const uint PExt11 = 4294967295U;

		// Token: 0x04001A29 RID: 6697
		private const uint PInv33 = 4553U;
	}
}
