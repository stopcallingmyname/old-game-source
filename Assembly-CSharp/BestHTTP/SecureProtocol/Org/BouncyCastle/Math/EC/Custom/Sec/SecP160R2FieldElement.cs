using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200035F RID: 863
	internal class SecP160R2FieldElement : AbstractFpFieldElement
	{
		// Token: 0x06002146 RID: 8518 RVA: 0x000F2CF4 File Offset: 0x000F0EF4
		public SecP160R2FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP160R2FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP160R2FieldElement", "x");
			}
			this.x = SecP160R2Field.FromBigInteger(x);
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x000F2D32 File Offset: 0x000F0F32
		public SecP160R2FieldElement()
		{
			this.x = Nat160.Create();
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x000F2D45 File Offset: 0x000F0F45
		protected internal SecP160R2FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06002149 RID: 8521 RVA: 0x000F2D54 File Offset: 0x000F0F54
		public override bool IsZero
		{
			get
			{
				return Nat160.IsZero(this.x);
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x0600214A RID: 8522 RVA: 0x000F2D61 File Offset: 0x000F0F61
		public override bool IsOne
		{
			get
			{
				return Nat160.IsOne(this.x);
			}
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x000F2D6E File Offset: 0x000F0F6E
		public override bool TestBitZero()
		{
			return Nat160.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x000F2D7F File Offset: 0x000F0F7F
		public override BigInteger ToBigInteger()
		{
			return Nat160.ToBigInteger(this.x);
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x0600214D RID: 8525 RVA: 0x000F2D8C File Offset: 0x000F0F8C
		public override string FieldName
		{
			get
			{
				return "SecP160R2Field";
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x0600214E RID: 8526 RVA: 0x000F2D93 File Offset: 0x000F0F93
		public override int FieldSize
		{
			get
			{
				return SecP160R2FieldElement.Q.BitLength;
			}
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x000F2DA0 File Offset: 0x000F0FA0
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat160.Create();
			SecP160R2Field.Add(this.x, ((SecP160R2FieldElement)b).x, z);
			return new SecP160R2FieldElement(z);
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x000F2DD0 File Offset: 0x000F0FD0
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat160.Create();
			SecP160R2Field.AddOne(this.x, z);
			return new SecP160R2FieldElement(z);
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x000F2DF8 File Offset: 0x000F0FF8
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat160.Create();
			SecP160R2Field.Subtract(this.x, ((SecP160R2FieldElement)b).x, z);
			return new SecP160R2FieldElement(z);
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x000F2E28 File Offset: 0x000F1028
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat160.Create();
			SecP160R2Field.Multiply(this.x, ((SecP160R2FieldElement)b).x, z);
			return new SecP160R2FieldElement(z);
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x000F2E58 File Offset: 0x000F1058
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat160.Create();
			Mod.Invert(SecP160R2Field.P, ((SecP160R2FieldElement)b).x, z);
			SecP160R2Field.Multiply(z, this.x, z);
			return new SecP160R2FieldElement(z);
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x000F2E94 File Offset: 0x000F1094
		public override ECFieldElement Negate()
		{
			uint[] z = Nat160.Create();
			SecP160R2Field.Negate(this.x, z);
			return new SecP160R2FieldElement(z);
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x000F2EBC File Offset: 0x000F10BC
		public override ECFieldElement Square()
		{
			uint[] z = Nat160.Create();
			SecP160R2Field.Square(this.x, z);
			return new SecP160R2FieldElement(z);
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x000F2EE4 File Offset: 0x000F10E4
		public override ECFieldElement Invert()
		{
			uint[] z = Nat160.Create();
			Mod.Invert(SecP160R2Field.P, this.x, z);
			return new SecP160R2FieldElement(z);
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x000F2F10 File Offset: 0x000F1110
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat160.IsZero(y) || Nat160.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat160.Create();
			SecP160R2Field.Square(y, array);
			SecP160R2Field.Multiply(array, y, array);
			uint[] array2 = Nat160.Create();
			SecP160R2Field.Square(array, array2);
			SecP160R2Field.Multiply(array2, y, array2);
			uint[] array3 = Nat160.Create();
			SecP160R2Field.Square(array2, array3);
			SecP160R2Field.Multiply(array3, y, array3);
			uint[] array4 = Nat160.Create();
			SecP160R2Field.SquareN(array3, 3, array4);
			SecP160R2Field.Multiply(array4, array2, array4);
			uint[] array5 = array3;
			SecP160R2Field.SquareN(array4, 7, array5);
			SecP160R2Field.Multiply(array5, array4, array5);
			uint[] array6 = array4;
			SecP160R2Field.SquareN(array5, 3, array6);
			SecP160R2Field.Multiply(array6, array2, array6);
			uint[] array7 = Nat160.Create();
			SecP160R2Field.SquareN(array6, 14, array7);
			SecP160R2Field.Multiply(array7, array5, array7);
			uint[] array8 = array5;
			SecP160R2Field.SquareN(array7, 31, array8);
			SecP160R2Field.Multiply(array8, array7, array8);
			uint[] z = array7;
			SecP160R2Field.SquareN(array8, 62, z);
			SecP160R2Field.Multiply(z, array8, z);
			uint[] array9 = array8;
			SecP160R2Field.SquareN(z, 3, array9);
			SecP160R2Field.Multiply(array9, array2, array9);
			uint[] z2 = array9;
			SecP160R2Field.SquareN(z2, 18, z2);
			SecP160R2Field.Multiply(z2, array6, z2);
			SecP160R2Field.SquareN(z2, 2, z2);
			SecP160R2Field.Multiply(z2, y, z2);
			SecP160R2Field.SquareN(z2, 3, z2);
			SecP160R2Field.Multiply(z2, array, z2);
			SecP160R2Field.SquareN(z2, 6, z2);
			SecP160R2Field.Multiply(z2, array2, z2);
			SecP160R2Field.SquareN(z2, 2, z2);
			SecP160R2Field.Multiply(z2, y, z2);
			uint[] array10 = array;
			SecP160R2Field.Square(z2, array10);
			if (!Nat160.Eq(y, array10))
			{
				return null;
			}
			return new SecP160R2FieldElement(z2);
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x000F30B1 File Offset: 0x000F12B1
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP160R2FieldElement);
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x000F30B1 File Offset: 0x000F12B1
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP160R2FieldElement);
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x000F30BF File Offset: 0x000F12BF
		public virtual bool Equals(SecP160R2FieldElement other)
		{
			return this == other || (other != null && Nat160.Eq(this.x, other.x));
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x000F30DD File Offset: 0x000F12DD
		public override int GetHashCode()
		{
			return SecP160R2FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 5);
		}

		// Token: 0x04001A1E RID: 6686
		public static readonly BigInteger Q = SecP160R2Curve.q;

		// Token: 0x04001A1F RID: 6687
		protected internal readonly uint[] x;
	}
}
