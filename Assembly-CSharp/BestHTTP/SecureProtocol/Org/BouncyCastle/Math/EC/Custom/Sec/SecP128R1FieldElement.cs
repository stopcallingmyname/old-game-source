using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000355 RID: 853
	internal class SecP128R1FieldElement : AbstractFpFieldElement
	{
		// Token: 0x060020BA RID: 8378 RVA: 0x000F0B23 File Offset: 0x000EED23
		public SecP128R1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP128R1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP128R1FieldElement", "x");
			}
			this.x = SecP128R1Field.FromBigInteger(x);
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x000F0B61 File Offset: 0x000EED61
		public SecP128R1FieldElement()
		{
			this.x = Nat128.Create();
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x000F0B74 File Offset: 0x000EED74
		protected internal SecP128R1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x060020BD RID: 8381 RVA: 0x000F0B83 File Offset: 0x000EED83
		public override bool IsZero
		{
			get
			{
				return Nat128.IsZero(this.x);
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x060020BE RID: 8382 RVA: 0x000F0B90 File Offset: 0x000EED90
		public override bool IsOne
		{
			get
			{
				return Nat128.IsOne(this.x);
			}
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x000F0B9D File Offset: 0x000EED9D
		public override bool TestBitZero()
		{
			return Nat128.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x000F0BAE File Offset: 0x000EEDAE
		public override BigInteger ToBigInteger()
		{
			return Nat128.ToBigInteger(this.x);
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x060020C1 RID: 8385 RVA: 0x000F0BBB File Offset: 0x000EEDBB
		public override string FieldName
		{
			get
			{
				return "SecP128R1Field";
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x060020C2 RID: 8386 RVA: 0x000F0BC2 File Offset: 0x000EEDC2
		public override int FieldSize
		{
			get
			{
				return SecP128R1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x000F0BD0 File Offset: 0x000EEDD0
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat128.Create();
			SecP128R1Field.Add(this.x, ((SecP128R1FieldElement)b).x, z);
			return new SecP128R1FieldElement(z);
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x000F0C00 File Offset: 0x000EEE00
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat128.Create();
			SecP128R1Field.AddOne(this.x, z);
			return new SecP128R1FieldElement(z);
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x000F0C28 File Offset: 0x000EEE28
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat128.Create();
			SecP128R1Field.Subtract(this.x, ((SecP128R1FieldElement)b).x, z);
			return new SecP128R1FieldElement(z);
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x000F0C58 File Offset: 0x000EEE58
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat128.Create();
			SecP128R1Field.Multiply(this.x, ((SecP128R1FieldElement)b).x, z);
			return new SecP128R1FieldElement(z);
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x000F0C88 File Offset: 0x000EEE88
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat128.Create();
			Mod.Invert(SecP128R1Field.P, ((SecP128R1FieldElement)b).x, z);
			SecP128R1Field.Multiply(z, this.x, z);
			return new SecP128R1FieldElement(z);
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x000F0CC4 File Offset: 0x000EEEC4
		public override ECFieldElement Negate()
		{
			uint[] z = Nat128.Create();
			SecP128R1Field.Negate(this.x, z);
			return new SecP128R1FieldElement(z);
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x000F0CEC File Offset: 0x000EEEEC
		public override ECFieldElement Square()
		{
			uint[] z = Nat128.Create();
			SecP128R1Field.Square(this.x, z);
			return new SecP128R1FieldElement(z);
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x000F0D14 File Offset: 0x000EEF14
		public override ECFieldElement Invert()
		{
			uint[] z = Nat128.Create();
			Mod.Invert(SecP128R1Field.P, this.x, z);
			return new SecP128R1FieldElement(z);
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x000F0D40 File Offset: 0x000EEF40
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat128.IsZero(y) || Nat128.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat128.Create();
			SecP128R1Field.Square(y, array);
			SecP128R1Field.Multiply(array, y, array);
			uint[] array2 = Nat128.Create();
			SecP128R1Field.SquareN(array, 2, array2);
			SecP128R1Field.Multiply(array2, array, array2);
			uint[] array3 = Nat128.Create();
			SecP128R1Field.SquareN(array2, 4, array3);
			SecP128R1Field.Multiply(array3, array2, array3);
			uint[] array4 = array2;
			SecP128R1Field.SquareN(array3, 2, array4);
			SecP128R1Field.Multiply(array4, array, array4);
			uint[] z = array;
			SecP128R1Field.SquareN(array4, 10, z);
			SecP128R1Field.Multiply(z, array4, z);
			uint[] array5 = array3;
			SecP128R1Field.SquareN(z, 10, array5);
			SecP128R1Field.Multiply(array5, array4, array5);
			uint[] array6 = array4;
			SecP128R1Field.Square(array5, array6);
			SecP128R1Field.Multiply(array6, y, array6);
			uint[] z2 = array6;
			SecP128R1Field.SquareN(z2, 95, z2);
			uint[] array7 = array5;
			SecP128R1Field.Square(z2, array7);
			if (!Nat128.Eq(y, array7))
			{
				return null;
			}
			return new SecP128R1FieldElement(z2);
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x000F0E35 File Offset: 0x000EF035
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP128R1FieldElement);
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x000F0E35 File Offset: 0x000EF035
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP128R1FieldElement);
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x000F0E43 File Offset: 0x000EF043
		public virtual bool Equals(SecP128R1FieldElement other)
		{
			return this == other || (other != null && Nat128.Eq(this.x, other.x));
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x000F0E61 File Offset: 0x000EF061
		public override int GetHashCode()
		{
			return SecP128R1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 4);
		}

		// Token: 0x04001A02 RID: 6658
		public static readonly BigInteger Q = SecP128R1Curve.q;

		// Token: 0x04001A03 RID: 6659
		protected internal readonly uint[] x;
	}
}
