using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000373 RID: 883
	internal class SecP256K1FieldElement : AbstractFpFieldElement
	{
		// Token: 0x0600227B RID: 8827 RVA: 0x000F76C9 File Offset: 0x000F58C9
		public SecP256K1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP256K1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP256K1FieldElement", "x");
			}
			this.x = SecP256K1Field.FromBigInteger(x);
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x000F7707 File Offset: 0x000F5907
		public SecP256K1FieldElement()
		{
			this.x = Nat256.Create();
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x000F771A File Offset: 0x000F591A
		protected internal SecP256K1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x0600227E RID: 8830 RVA: 0x000F7729 File Offset: 0x000F5929
		public override bool IsZero
		{
			get
			{
				return Nat256.IsZero(this.x);
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x0600227F RID: 8831 RVA: 0x000F7736 File Offset: 0x000F5936
		public override bool IsOne
		{
			get
			{
				return Nat256.IsOne(this.x);
			}
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x000F7743 File Offset: 0x000F5943
		public override bool TestBitZero()
		{
			return Nat256.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x000F7754 File Offset: 0x000F5954
		public override BigInteger ToBigInteger()
		{
			return Nat256.ToBigInteger(this.x);
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06002282 RID: 8834 RVA: 0x000F7761 File Offset: 0x000F5961
		public override string FieldName
		{
			get
			{
				return "SecP256K1Field";
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06002283 RID: 8835 RVA: 0x000F7768 File Offset: 0x000F5968
		public override int FieldSize
		{
			get
			{
				return SecP256K1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x000F7774 File Offset: 0x000F5974
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			SecP256K1Field.Add(this.x, ((SecP256K1FieldElement)b).x, z);
			return new SecP256K1FieldElement(z);
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x000F77A4 File Offset: 0x000F59A4
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat256.Create();
			SecP256K1Field.AddOne(this.x, z);
			return new SecP256K1FieldElement(z);
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x000F77CC File Offset: 0x000F59CC
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			SecP256K1Field.Subtract(this.x, ((SecP256K1FieldElement)b).x, z);
			return new SecP256K1FieldElement(z);
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x000F77FC File Offset: 0x000F59FC
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			SecP256K1Field.Multiply(this.x, ((SecP256K1FieldElement)b).x, z);
			return new SecP256K1FieldElement(z);
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x000F782C File Offset: 0x000F5A2C
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			Mod.Invert(SecP256K1Field.P, ((SecP256K1FieldElement)b).x, z);
			SecP256K1Field.Multiply(z, this.x, z);
			return new SecP256K1FieldElement(z);
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x000F7868 File Offset: 0x000F5A68
		public override ECFieldElement Negate()
		{
			uint[] z = Nat256.Create();
			SecP256K1Field.Negate(this.x, z);
			return new SecP256K1FieldElement(z);
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x000F7890 File Offset: 0x000F5A90
		public override ECFieldElement Square()
		{
			uint[] z = Nat256.Create();
			SecP256K1Field.Square(this.x, z);
			return new SecP256K1FieldElement(z);
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x000F78B8 File Offset: 0x000F5AB8
		public override ECFieldElement Invert()
		{
			uint[] z = Nat256.Create();
			Mod.Invert(SecP256K1Field.P, this.x, z);
			return new SecP256K1FieldElement(z);
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x000F78E4 File Offset: 0x000F5AE4
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat256.IsZero(y) || Nat256.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat256.Create();
			SecP256K1Field.Square(y, array);
			SecP256K1Field.Multiply(array, y, array);
			uint[] array2 = Nat256.Create();
			SecP256K1Field.Square(array, array2);
			SecP256K1Field.Multiply(array2, y, array2);
			uint[] array3 = Nat256.Create();
			SecP256K1Field.SquareN(array2, 3, array3);
			SecP256K1Field.Multiply(array3, array2, array3);
			uint[] array4 = array3;
			SecP256K1Field.SquareN(array3, 3, array4);
			SecP256K1Field.Multiply(array4, array2, array4);
			uint[] array5 = array4;
			SecP256K1Field.SquareN(array4, 2, array5);
			SecP256K1Field.Multiply(array5, array, array5);
			uint[] array6 = Nat256.Create();
			SecP256K1Field.SquareN(array5, 11, array6);
			SecP256K1Field.Multiply(array6, array5, array6);
			uint[] array7 = array5;
			SecP256K1Field.SquareN(array6, 22, array7);
			SecP256K1Field.Multiply(array7, array6, array7);
			uint[] array8 = Nat256.Create();
			SecP256K1Field.SquareN(array7, 44, array8);
			SecP256K1Field.Multiply(array8, array7, array8);
			uint[] z = Nat256.Create();
			SecP256K1Field.SquareN(array8, 88, z);
			SecP256K1Field.Multiply(z, array8, z);
			uint[] z2 = array8;
			SecP256K1Field.SquareN(z, 44, z2);
			SecP256K1Field.Multiply(z2, array7, z2);
			uint[] array9 = array7;
			SecP256K1Field.SquareN(z2, 3, array9);
			SecP256K1Field.Multiply(array9, array2, array9);
			uint[] z3 = array9;
			SecP256K1Field.SquareN(z3, 23, z3);
			SecP256K1Field.Multiply(z3, array6, z3);
			SecP256K1Field.SquareN(z3, 6, z3);
			SecP256K1Field.Multiply(z3, array, z3);
			SecP256K1Field.SquareN(z3, 2, z3);
			uint[] array10 = array;
			SecP256K1Field.Square(z3, array10);
			if (!Nat256.Eq(y, array10))
			{
				return null;
			}
			return new SecP256K1FieldElement(z3);
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x000F7A72 File Offset: 0x000F5C72
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP256K1FieldElement);
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x000F7A72 File Offset: 0x000F5C72
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP256K1FieldElement);
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x000F7A80 File Offset: 0x000F5C80
		public virtual bool Equals(SecP256K1FieldElement other)
		{
			return this == other || (other != null && Nat256.Eq(this.x, other.x));
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x000F7A9E File Offset: 0x000F5C9E
		public override int GetHashCode()
		{
			return SecP256K1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 8);
		}

		// Token: 0x04001A59 RID: 6745
		public static readonly BigInteger Q = SecP256K1Curve.q;

		// Token: 0x04001A5A RID: 6746
		protected internal readonly uint[] x;
	}
}
