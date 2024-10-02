using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000363 RID: 867
	internal class SecP192K1FieldElement : AbstractFpFieldElement
	{
		// Token: 0x06002182 RID: 8578 RVA: 0x000F3AF4 File Offset: 0x000F1CF4
		public SecP192K1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP192K1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP192K1FieldElement", "x");
			}
			this.x = SecP192K1Field.FromBigInteger(x);
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x000F3B32 File Offset: 0x000F1D32
		public SecP192K1FieldElement()
		{
			this.x = Nat192.Create();
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x000F3B45 File Offset: 0x000F1D45
		protected internal SecP192K1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06002185 RID: 8581 RVA: 0x000F3B54 File Offset: 0x000F1D54
		public override bool IsZero
		{
			get
			{
				return Nat192.IsZero(this.x);
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06002186 RID: 8582 RVA: 0x000F3B61 File Offset: 0x000F1D61
		public override bool IsOne
		{
			get
			{
				return Nat192.IsOne(this.x);
			}
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x000F3B6E File Offset: 0x000F1D6E
		public override bool TestBitZero()
		{
			return Nat192.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x000F3B7F File Offset: 0x000F1D7F
		public override BigInteger ToBigInteger()
		{
			return Nat192.ToBigInteger(this.x);
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06002189 RID: 8585 RVA: 0x000F3B8C File Offset: 0x000F1D8C
		public override string FieldName
		{
			get
			{
				return "SecP192K1Field";
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x0600218A RID: 8586 RVA: 0x000F3B93 File Offset: 0x000F1D93
		public override int FieldSize
		{
			get
			{
				return SecP192K1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x000F3BA0 File Offset: 0x000F1DA0
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat192.Create();
			SecP192K1Field.Add(this.x, ((SecP192K1FieldElement)b).x, z);
			return new SecP192K1FieldElement(z);
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x000F3BD0 File Offset: 0x000F1DD0
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat192.Create();
			SecP192K1Field.AddOne(this.x, z);
			return new SecP192K1FieldElement(z);
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x000F3BF8 File Offset: 0x000F1DF8
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat192.Create();
			SecP192K1Field.Subtract(this.x, ((SecP192K1FieldElement)b).x, z);
			return new SecP192K1FieldElement(z);
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x000F3C28 File Offset: 0x000F1E28
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat192.Create();
			SecP192K1Field.Multiply(this.x, ((SecP192K1FieldElement)b).x, z);
			return new SecP192K1FieldElement(z);
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x000F3C58 File Offset: 0x000F1E58
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat192.Create();
			Mod.Invert(SecP192K1Field.P, ((SecP192K1FieldElement)b).x, z);
			SecP192K1Field.Multiply(z, this.x, z);
			return new SecP192K1FieldElement(z);
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x000F3C94 File Offset: 0x000F1E94
		public override ECFieldElement Negate()
		{
			uint[] z = Nat192.Create();
			SecP192K1Field.Negate(this.x, z);
			return new SecP192K1FieldElement(z);
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x000F3CBC File Offset: 0x000F1EBC
		public override ECFieldElement Square()
		{
			uint[] z = Nat192.Create();
			SecP192K1Field.Square(this.x, z);
			return new SecP192K1FieldElement(z);
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x000F3CE4 File Offset: 0x000F1EE4
		public override ECFieldElement Invert()
		{
			uint[] z = Nat192.Create();
			Mod.Invert(SecP192K1Field.P, this.x, z);
			return new SecP192K1FieldElement(z);
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x000F3D10 File Offset: 0x000F1F10
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat192.IsZero(y) || Nat192.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat192.Create();
			SecP192K1Field.Square(y, array);
			SecP192K1Field.Multiply(array, y, array);
			uint[] array2 = Nat192.Create();
			SecP192K1Field.Square(array, array2);
			SecP192K1Field.Multiply(array2, y, array2);
			uint[] array3 = Nat192.Create();
			SecP192K1Field.SquareN(array2, 3, array3);
			SecP192K1Field.Multiply(array3, array2, array3);
			uint[] array4 = array3;
			SecP192K1Field.SquareN(array3, 2, array4);
			SecP192K1Field.Multiply(array4, array, array4);
			uint[] array5 = array;
			SecP192K1Field.SquareN(array4, 8, array5);
			SecP192K1Field.Multiply(array5, array4, array5);
			uint[] array6 = array4;
			SecP192K1Field.SquareN(array5, 3, array6);
			SecP192K1Field.Multiply(array6, array2, array6);
			uint[] array7 = Nat192.Create();
			SecP192K1Field.SquareN(array6, 16, array7);
			SecP192K1Field.Multiply(array7, array5, array7);
			uint[] array8 = array5;
			SecP192K1Field.SquareN(array7, 35, array8);
			SecP192K1Field.Multiply(array8, array7, array8);
			uint[] z = array7;
			SecP192K1Field.SquareN(array8, 70, z);
			SecP192K1Field.Multiply(z, array8, z);
			uint[] array9 = array8;
			SecP192K1Field.SquareN(z, 19, array9);
			SecP192K1Field.Multiply(array9, array6, array9);
			uint[] z2 = array9;
			SecP192K1Field.SquareN(z2, 20, z2);
			SecP192K1Field.Multiply(z2, array6, z2);
			SecP192K1Field.SquareN(z2, 4, z2);
			SecP192K1Field.Multiply(z2, array2, z2);
			SecP192K1Field.SquareN(z2, 6, z2);
			SecP192K1Field.Multiply(z2, array2, z2);
			SecP192K1Field.Square(z2, z2);
			uint[] array10 = array2;
			SecP192K1Field.Square(z2, array10);
			if (!Nat192.Eq(y, array10))
			{
				return null;
			}
			return new SecP192K1FieldElement(z2);
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x000F3E91 File Offset: 0x000F2091
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP192K1FieldElement);
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x000F3E91 File Offset: 0x000F2091
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP192K1FieldElement);
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x000F3E9F File Offset: 0x000F209F
		public virtual bool Equals(SecP192K1FieldElement other)
		{
			return this == other || (other != null && Nat192.Eq(this.x, other.x));
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x000F3EBD File Offset: 0x000F20BD
		public override int GetHashCode()
		{
			return SecP192K1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 6);
		}

		// Token: 0x04001A2A RID: 6698
		public static readonly BigInteger Q = SecP192K1Curve.q;

		// Token: 0x04001A2B RID: 6699
		protected internal readonly uint[] x;
	}
}
