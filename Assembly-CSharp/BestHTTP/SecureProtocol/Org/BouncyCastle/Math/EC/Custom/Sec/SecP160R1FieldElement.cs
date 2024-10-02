using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200035B RID: 859
	internal class SecP160R1FieldElement : AbstractFpFieldElement
	{
		// Token: 0x0600210A RID: 8458 RVA: 0x000F1F7C File Offset: 0x000F017C
		public SecP160R1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP160R1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP160R1FieldElement", "x");
			}
			this.x = SecP160R1Field.FromBigInteger(x);
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x000F1FBA File Offset: 0x000F01BA
		public SecP160R1FieldElement()
		{
			this.x = Nat160.Create();
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x000F1FCD File Offset: 0x000F01CD
		protected internal SecP160R1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x0600210D RID: 8461 RVA: 0x000F1FDC File Offset: 0x000F01DC
		public override bool IsZero
		{
			get
			{
				return Nat160.IsZero(this.x);
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600210E RID: 8462 RVA: 0x000F1FE9 File Offset: 0x000F01E9
		public override bool IsOne
		{
			get
			{
				return Nat160.IsOne(this.x);
			}
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x000F1FF6 File Offset: 0x000F01F6
		public override bool TestBitZero()
		{
			return Nat160.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x000F2007 File Offset: 0x000F0207
		public override BigInteger ToBigInteger()
		{
			return Nat160.ToBigInteger(this.x);
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06002111 RID: 8465 RVA: 0x000F2014 File Offset: 0x000F0214
		public override string FieldName
		{
			get
			{
				return "SecP160R1Field";
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06002112 RID: 8466 RVA: 0x000F201B File Offset: 0x000F021B
		public override int FieldSize
		{
			get
			{
				return SecP160R1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x000F2028 File Offset: 0x000F0228
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat160.Create();
			SecP160R1Field.Add(this.x, ((SecP160R1FieldElement)b).x, z);
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x000F2058 File Offset: 0x000F0258
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat160.Create();
			SecP160R1Field.AddOne(this.x, z);
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x000F2080 File Offset: 0x000F0280
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat160.Create();
			SecP160R1Field.Subtract(this.x, ((SecP160R1FieldElement)b).x, z);
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x000F20B0 File Offset: 0x000F02B0
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat160.Create();
			SecP160R1Field.Multiply(this.x, ((SecP160R1FieldElement)b).x, z);
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x000F20E0 File Offset: 0x000F02E0
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat160.Create();
			Mod.Invert(SecP160R1Field.P, ((SecP160R1FieldElement)b).x, z);
			SecP160R1Field.Multiply(z, this.x, z);
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x000F211C File Offset: 0x000F031C
		public override ECFieldElement Negate()
		{
			uint[] z = Nat160.Create();
			SecP160R1Field.Negate(this.x, z);
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x000F2144 File Offset: 0x000F0344
		public override ECFieldElement Square()
		{
			uint[] z = Nat160.Create();
			SecP160R1Field.Square(this.x, z);
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x000F216C File Offset: 0x000F036C
		public override ECFieldElement Invert()
		{
			uint[] z = Nat160.Create();
			Mod.Invert(SecP160R1Field.P, this.x, z);
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x000F2198 File Offset: 0x000F0398
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat160.IsZero(y) || Nat160.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat160.Create();
			SecP160R1Field.Square(y, array);
			SecP160R1Field.Multiply(array, y, array);
			uint[] array2 = Nat160.Create();
			SecP160R1Field.SquareN(array, 2, array2);
			SecP160R1Field.Multiply(array2, array, array2);
			uint[] array3 = array;
			SecP160R1Field.SquareN(array2, 4, array3);
			SecP160R1Field.Multiply(array3, array2, array3);
			uint[] array4 = array2;
			SecP160R1Field.SquareN(array3, 8, array4);
			SecP160R1Field.Multiply(array4, array3, array4);
			uint[] array5 = array3;
			SecP160R1Field.SquareN(array4, 16, array5);
			SecP160R1Field.Multiply(array5, array4, array5);
			uint[] array6 = array4;
			SecP160R1Field.SquareN(array5, 32, array6);
			SecP160R1Field.Multiply(array6, array5, array6);
			uint[] array7 = array5;
			SecP160R1Field.SquareN(array6, 64, array7);
			SecP160R1Field.Multiply(array7, array6, array7);
			uint[] array8 = array6;
			SecP160R1Field.Square(array7, array8);
			SecP160R1Field.Multiply(array8, y, array8);
			uint[] z = array8;
			SecP160R1Field.SquareN(z, 29, z);
			uint[] array9 = array7;
			SecP160R1Field.Square(z, array9);
			if (!Nat160.Eq(y, array9))
			{
				return null;
			}
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x000F22A4 File Offset: 0x000F04A4
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP160R1FieldElement);
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x000F22A4 File Offset: 0x000F04A4
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP160R1FieldElement);
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x000F22B2 File Offset: 0x000F04B2
		public virtual bool Equals(SecP160R1FieldElement other)
		{
			return this == other || (other != null && Nat160.Eq(this.x, other.x));
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x000F22D0 File Offset: 0x000F04D0
		public override int GetHashCode()
		{
			return SecP160R1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 5);
		}

		// Token: 0x04001A12 RID: 6674
		public static readonly BigInteger Q = SecP160R1Curve.q;

		// Token: 0x04001A13 RID: 6675
		protected internal readonly uint[] x;
	}
}
