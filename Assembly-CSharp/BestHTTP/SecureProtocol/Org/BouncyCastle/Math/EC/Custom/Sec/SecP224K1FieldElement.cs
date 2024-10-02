using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200036B RID: 875
	internal class SecP224K1FieldElement : AbstractFpFieldElement
	{
		// Token: 0x060021FC RID: 8700 RVA: 0x000F57A9 File Offset: 0x000F39A9
		public SecP224K1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP224K1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP224K1FieldElement", "x");
			}
			this.x = SecP224K1Field.FromBigInteger(x);
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x000F57E7 File Offset: 0x000F39E7
		public SecP224K1FieldElement()
		{
			this.x = Nat224.Create();
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x000F57FA File Offset: 0x000F39FA
		protected internal SecP224K1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060021FF RID: 8703 RVA: 0x000F5809 File Offset: 0x000F3A09
		public override bool IsZero
		{
			get
			{
				return Nat224.IsZero(this.x);
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06002200 RID: 8704 RVA: 0x000F5816 File Offset: 0x000F3A16
		public override bool IsOne
		{
			get
			{
				return Nat224.IsOne(this.x);
			}
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x000F5823 File Offset: 0x000F3A23
		public override bool TestBitZero()
		{
			return Nat224.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x000F5834 File Offset: 0x000F3A34
		public override BigInteger ToBigInteger()
		{
			return Nat224.ToBigInteger(this.x);
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06002203 RID: 8707 RVA: 0x000F5841 File Offset: 0x000F3A41
		public override string FieldName
		{
			get
			{
				return "SecP224K1Field";
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06002204 RID: 8708 RVA: 0x000F5848 File Offset: 0x000F3A48
		public override int FieldSize
		{
			get
			{
				return SecP224K1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x000F5854 File Offset: 0x000F3A54
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat224.Create();
			SecP224K1Field.Add(this.x, ((SecP224K1FieldElement)b).x, z);
			return new SecP224K1FieldElement(z);
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x000F5884 File Offset: 0x000F3A84
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat224.Create();
			SecP224K1Field.AddOne(this.x, z);
			return new SecP224K1FieldElement(z);
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x000F58AC File Offset: 0x000F3AAC
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat224.Create();
			SecP224K1Field.Subtract(this.x, ((SecP224K1FieldElement)b).x, z);
			return new SecP224K1FieldElement(z);
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x000F58DC File Offset: 0x000F3ADC
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat224.Create();
			SecP224K1Field.Multiply(this.x, ((SecP224K1FieldElement)b).x, z);
			return new SecP224K1FieldElement(z);
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x000F590C File Offset: 0x000F3B0C
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat224.Create();
			Mod.Invert(SecP224K1Field.P, ((SecP224K1FieldElement)b).x, z);
			SecP224K1Field.Multiply(z, this.x, z);
			return new SecP224K1FieldElement(z);
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x000F5948 File Offset: 0x000F3B48
		public override ECFieldElement Negate()
		{
			uint[] z = Nat224.Create();
			SecP224K1Field.Negate(this.x, z);
			return new SecP224K1FieldElement(z);
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x000F5970 File Offset: 0x000F3B70
		public override ECFieldElement Square()
		{
			uint[] z = Nat224.Create();
			SecP224K1Field.Square(this.x, z);
			return new SecP224K1FieldElement(z);
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x000F5998 File Offset: 0x000F3B98
		public override ECFieldElement Invert()
		{
			uint[] z = Nat224.Create();
			Mod.Invert(SecP224K1Field.P, this.x, z);
			return new SecP224K1FieldElement(z);
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x000F59C4 File Offset: 0x000F3BC4
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat224.IsZero(y) || Nat224.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat224.Create();
			SecP224K1Field.Square(y, array);
			SecP224K1Field.Multiply(array, y, array);
			uint[] array2 = array;
			SecP224K1Field.Square(array, array2);
			SecP224K1Field.Multiply(array2, y, array2);
			uint[] array3 = Nat224.Create();
			SecP224K1Field.Square(array2, array3);
			SecP224K1Field.Multiply(array3, y, array3);
			uint[] array4 = Nat224.Create();
			SecP224K1Field.SquareN(array3, 4, array4);
			SecP224K1Field.Multiply(array4, array3, array4);
			uint[] array5 = Nat224.Create();
			SecP224K1Field.SquareN(array4, 3, array5);
			SecP224K1Field.Multiply(array5, array2, array5);
			uint[] array6 = array5;
			SecP224K1Field.SquareN(array5, 8, array6);
			SecP224K1Field.Multiply(array6, array4, array6);
			uint[] array7 = array4;
			SecP224K1Field.SquareN(array6, 4, array7);
			SecP224K1Field.Multiply(array7, array3, array7);
			uint[] array8 = array3;
			SecP224K1Field.SquareN(array7, 19, array8);
			SecP224K1Field.Multiply(array8, array6, array8);
			uint[] array9 = Nat224.Create();
			SecP224K1Field.SquareN(array8, 42, array9);
			SecP224K1Field.Multiply(array9, array8, array9);
			uint[] z = array8;
			SecP224K1Field.SquareN(array9, 23, z);
			SecP224K1Field.Multiply(z, array7, z);
			uint[] array10 = array7;
			SecP224K1Field.SquareN(z, 84, array10);
			SecP224K1Field.Multiply(array10, array9, array10);
			uint[] z2 = array10;
			SecP224K1Field.SquareN(z2, 20, z2);
			SecP224K1Field.Multiply(z2, array6, z2);
			SecP224K1Field.SquareN(z2, 3, z2);
			SecP224K1Field.Multiply(z2, y, z2);
			SecP224K1Field.SquareN(z2, 2, z2);
			SecP224K1Field.Multiply(z2, y, z2);
			SecP224K1Field.SquareN(z2, 4, z2);
			SecP224K1Field.Multiply(z2, array2, z2);
			SecP224K1Field.Square(z2, z2);
			uint[] array11 = array9;
			SecP224K1Field.Square(z2, array11);
			if (Nat224.Eq(y, array11))
			{
				return new SecP224K1FieldElement(z2);
			}
			SecP224K1Field.Multiply(z2, SecP224K1FieldElement.PRECOMP_POW2, z2);
			SecP224K1Field.Square(z2, array11);
			if (Nat224.Eq(y, array11))
			{
				return new SecP224K1FieldElement(z2);
			}
			return null;
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x000F5B9D File Offset: 0x000F3D9D
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP224K1FieldElement);
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x000F5B9D File Offset: 0x000F3D9D
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP224K1FieldElement);
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x000F5BAB File Offset: 0x000F3DAB
		public virtual bool Equals(SecP224K1FieldElement other)
		{
			return this == other || (other != null && Nat224.Eq(this.x, other.x));
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x000F5BC9 File Offset: 0x000F3DC9
		public override int GetHashCode()
		{
			return SecP224K1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 7);
		}

		// Token: 0x04001A41 RID: 6721
		public static readonly BigInteger Q = SecP224K1Curve.q;

		// Token: 0x04001A42 RID: 6722
		private static readonly uint[] PRECOMP_POW2 = new uint[]
		{
			868209154U,
			3707425075U,
			579297866U,
			3280018344U,
			2824165628U,
			514782679U,
			2396984652U
		};

		// Token: 0x04001A43 RID: 6723
		protected internal readonly uint[] x;
	}
}
