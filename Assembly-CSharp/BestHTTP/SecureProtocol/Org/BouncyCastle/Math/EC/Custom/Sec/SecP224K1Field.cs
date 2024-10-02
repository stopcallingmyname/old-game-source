using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200036A RID: 874
	internal class SecP224K1Field
	{
		// Token: 0x060021EB RID: 8683 RVA: 0x000F544D File Offset: 0x000F364D
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat224.Add(x, y, z) != 0U || (z[6] == 4294967295U && Nat224.Gte(z, SecP224K1Field.P)))
			{
				Nat.Add33To(7, 6803U, z);
			}
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x000F547C File Offset: 0x000F367C
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if ((Nat.Add(14, xx, yy, zz) != 0U || (zz[13] == 4294967295U && Nat.Gte(14, zz, SecP224K1Field.PExt))) && Nat.AddTo(SecP224K1Field.PExtInv.Length, SecP224K1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(14, zz, SecP224K1Field.PExtInv.Length);
			}
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x000F54CF File Offset: 0x000F36CF
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(7, x, z) != 0U || (z[6] == 4294967295U && Nat224.Gte(z, SecP224K1Field.P)))
			{
				Nat.Add33To(7, 6803U, z);
			}
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x000F54FC File Offset: 0x000F36FC
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat224.FromBigInteger(x);
			if (array[6] == 4294967295U && Nat224.Gte(array, SecP224K1Field.P))
			{
				Nat224.SubFrom(SecP224K1Field.P, array);
			}
			return array;
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x000F5530 File Offset: 0x000F3730
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(7, x, 0U, z);
				return;
			}
			uint c = Nat224.Add(x, SecP224K1Field.P, z);
			Nat.ShiftDownBit(7, z, c);
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x000F5568 File Offset: 0x000F3768
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat224.CreateExt();
			Nat224.Mul(x, y, array);
			SecP224K1Field.Reduce(array, z);
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x000F558C File Offset: 0x000F378C
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if ((Nat224.MulAddTo(x, y, zz) != 0U || (zz[13] == 4294967295U && Nat.Gte(14, zz, SecP224K1Field.PExt))) && Nat.AddTo(SecP224K1Field.PExtInv.Length, SecP224K1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(14, zz, SecP224K1Field.PExtInv.Length);
			}
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x000F55DD File Offset: 0x000F37DD
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat224.IsZero(x))
			{
				Nat224.Zero(z);
				return;
			}
			Nat224.Sub(SecP224K1Field.P, x, z);
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x000F55FC File Offset: 0x000F37FC
		public static void Reduce(uint[] xx, uint[] z)
		{
			ulong y = Nat224.Mul33Add(6803U, xx, 7, xx, 0, z, 0);
			if (Nat224.Mul33DWordAdd(6803U, y, z, 0) != 0U || (z[6] == 4294967295U && Nat224.Gte(z, SecP224K1Field.P)))
			{
				Nat.Add33To(7, 6803U, z);
			}
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x000F5649 File Offset: 0x000F3849
		public static void Reduce32(uint x, uint[] z)
		{
			if ((x != 0U && Nat224.Mul33WordAdd(6803U, x, z, 0) != 0U) || (z[6] == 4294967295U && Nat224.Gte(z, SecP224K1Field.P)))
			{
				Nat.Add33To(7, 6803U, z);
			}
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x000F5680 File Offset: 0x000F3880
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat224.CreateExt();
			Nat224.Square(x, array);
			SecP224K1Field.Reduce(array, z);
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x000F56A4 File Offset: 0x000F38A4
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat224.CreateExt();
			Nat224.Square(x, array);
			SecP224K1Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat224.Square(z, array);
				SecP224K1Field.Reduce(array, z);
			}
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x000F56DE File Offset: 0x000F38DE
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat224.Sub(x, y, z) != 0)
			{
				Nat.Sub33From(7, 6803U, z);
			}
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x000F56F7 File Offset: 0x000F38F7
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(14, xx, yy, zz) != 0 && Nat.SubFrom(SecP224K1Field.PExtInv.Length, SecP224K1Field.PExtInv, zz) != 0)
			{
				Nat.DecAt(14, zz, SecP224K1Field.PExtInv.Length);
			}
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x000F5729 File Offset: 0x000F3929
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(7, x, 0U, z) != 0U || (z[6] == 4294967295U && Nat224.Gte(z, SecP224K1Field.P)))
			{
				Nat.Add33To(7, 6803U, z);
			}
		}

		// Token: 0x04001A3B RID: 6715
		internal static readonly uint[] P = new uint[]
		{
			4294960493U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x04001A3C RID: 6716
		internal static readonly uint[] PExt = new uint[]
		{
			46280809U,
			13606U,
			1U,
			0U,
			0U,
			0U,
			0U,
			4294953690U,
			4294967293U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x04001A3D RID: 6717
		private static readonly uint[] PExtInv = new uint[]
		{
			4248686487U,
			4294953689U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			13605U,
			2U
		};

		// Token: 0x04001A3E RID: 6718
		private const uint P6 = 4294967295U;

		// Token: 0x04001A3F RID: 6719
		private const uint PExt13 = 4294967295U;

		// Token: 0x04001A40 RID: 6720
		private const uint PInv33 = 6803U;
	}
}
