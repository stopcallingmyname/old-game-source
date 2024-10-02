using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000377 RID: 887
	internal class SecP256R1FieldElement : AbstractFpFieldElement
	{
		// Token: 0x060022B9 RID: 8889 RVA: 0x000F872B File Offset: 0x000F692B
		public SecP256R1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP256R1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP256R1FieldElement", "x");
			}
			this.x = SecP256R1Field.FromBigInteger(x);
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x000F8769 File Offset: 0x000F6969
		public SecP256R1FieldElement()
		{
			this.x = Nat256.Create();
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x000F877C File Offset: 0x000F697C
		protected internal SecP256R1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x060022BC RID: 8892 RVA: 0x000F878B File Offset: 0x000F698B
		public override bool IsZero
		{
			get
			{
				return Nat256.IsZero(this.x);
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x060022BD RID: 8893 RVA: 0x000F8798 File Offset: 0x000F6998
		public override bool IsOne
		{
			get
			{
				return Nat256.IsOne(this.x);
			}
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x000F87A5 File Offset: 0x000F69A5
		public override bool TestBitZero()
		{
			return Nat256.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x000F87B6 File Offset: 0x000F69B6
		public override BigInteger ToBigInteger()
		{
			return Nat256.ToBigInteger(this.x);
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x060022C0 RID: 8896 RVA: 0x000F87C3 File Offset: 0x000F69C3
		public override string FieldName
		{
			get
			{
				return "SecP256R1Field";
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x060022C1 RID: 8897 RVA: 0x000F87CA File Offset: 0x000F69CA
		public override int FieldSize
		{
			get
			{
				return SecP256R1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x000F87D8 File Offset: 0x000F69D8
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			SecP256R1Field.Add(this.x, ((SecP256R1FieldElement)b).x, z);
			return new SecP256R1FieldElement(z);
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x000F8808 File Offset: 0x000F6A08
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat256.Create();
			SecP256R1Field.AddOne(this.x, z);
			return new SecP256R1FieldElement(z);
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x000F8830 File Offset: 0x000F6A30
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			SecP256R1Field.Subtract(this.x, ((SecP256R1FieldElement)b).x, z);
			return new SecP256R1FieldElement(z);
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x000F8860 File Offset: 0x000F6A60
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			SecP256R1Field.Multiply(this.x, ((SecP256R1FieldElement)b).x, z);
			return new SecP256R1FieldElement(z);
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x000F8890 File Offset: 0x000F6A90
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			Mod.Invert(SecP256R1Field.P, ((SecP256R1FieldElement)b).x, z);
			SecP256R1Field.Multiply(z, this.x, z);
			return new SecP256R1FieldElement(z);
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x000F88CC File Offset: 0x000F6ACC
		public override ECFieldElement Negate()
		{
			uint[] z = Nat256.Create();
			SecP256R1Field.Negate(this.x, z);
			return new SecP256R1FieldElement(z);
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x000F88F4 File Offset: 0x000F6AF4
		public override ECFieldElement Square()
		{
			uint[] z = Nat256.Create();
			SecP256R1Field.Square(this.x, z);
			return new SecP256R1FieldElement(z);
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x000F891C File Offset: 0x000F6B1C
		public override ECFieldElement Invert()
		{
			uint[] z = Nat256.Create();
			Mod.Invert(SecP256R1Field.P, this.x, z);
			return new SecP256R1FieldElement(z);
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x000F8948 File Offset: 0x000F6B48
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat256.IsZero(y) || Nat256.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat256.Create();
			uint[] array2 = Nat256.Create();
			SecP256R1Field.Square(y, array);
			SecP256R1Field.Multiply(array, y, array);
			SecP256R1Field.SquareN(array, 2, array2);
			SecP256R1Field.Multiply(array2, array, array2);
			SecP256R1Field.SquareN(array2, 4, array);
			SecP256R1Field.Multiply(array, array2, array);
			SecP256R1Field.SquareN(array, 8, array2);
			SecP256R1Field.Multiply(array2, array, array2);
			SecP256R1Field.SquareN(array2, 16, array);
			SecP256R1Field.Multiply(array, array2, array);
			SecP256R1Field.SquareN(array, 32, array);
			SecP256R1Field.Multiply(array, y, array);
			SecP256R1Field.SquareN(array, 96, array);
			SecP256R1Field.Multiply(array, y, array);
			SecP256R1Field.SquareN(array, 94, array);
			SecP256R1Field.Multiply(array, array, array2);
			if (!Nat256.Eq(y, array2))
			{
				return null;
			}
			return new SecP256R1FieldElement(array);
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x000F8A0E File Offset: 0x000F6C0E
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP256R1FieldElement);
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x000F8A0E File Offset: 0x000F6C0E
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP256R1FieldElement);
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x000F8A1C File Offset: 0x000F6C1C
		public virtual bool Equals(SecP256R1FieldElement other)
		{
			return this == other || (other != null && Nat256.Eq(this.x, other.x));
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x000F8A3A File Offset: 0x000F6C3A
		public override int GetHashCode()
		{
			return SecP256R1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 8);
		}

		// Token: 0x04001A63 RID: 6755
		public static readonly BigInteger Q = SecP256R1Curve.q;

		// Token: 0x04001A64 RID: 6756
		protected internal readonly uint[] x;
	}
}
