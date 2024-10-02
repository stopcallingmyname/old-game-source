using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200037B RID: 891
	internal class SecP384R1FieldElement : AbstractFpFieldElement
	{
		// Token: 0x060022F6 RID: 8950 RVA: 0x000F9752 File Offset: 0x000F7952
		public SecP384R1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP384R1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP384R1FieldElement", "x");
			}
			this.x = SecP384R1Field.FromBigInteger(x);
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x000F9790 File Offset: 0x000F7990
		public SecP384R1FieldElement()
		{
			this.x = Nat.Create(12);
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x000F97A5 File Offset: 0x000F79A5
		protected internal SecP384R1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x060022F9 RID: 8953 RVA: 0x000F97B4 File Offset: 0x000F79B4
		public override bool IsZero
		{
			get
			{
				return Nat.IsZero(12, this.x);
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x060022FA RID: 8954 RVA: 0x000F97C3 File Offset: 0x000F79C3
		public override bool IsOne
		{
			get
			{
				return Nat.IsOne(12, this.x);
			}
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x000F97D2 File Offset: 0x000F79D2
		public override bool TestBitZero()
		{
			return Nat.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x000F97E3 File Offset: 0x000F79E3
		public override BigInteger ToBigInteger()
		{
			return Nat.ToBigInteger(12, this.x);
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x060022FD RID: 8957 RVA: 0x000F97F2 File Offset: 0x000F79F2
		public override string FieldName
		{
			get
			{
				return "SecP384R1Field";
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x060022FE RID: 8958 RVA: 0x000F97F9 File Offset: 0x000F79F9
		public override int FieldSize
		{
			get
			{
				return SecP384R1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x000F9808 File Offset: 0x000F7A08
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat.Create(12);
			SecP384R1Field.Add(this.x, ((SecP384R1FieldElement)b).x, z);
			return new SecP384R1FieldElement(z);
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x000F983C File Offset: 0x000F7A3C
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat.Create(12);
			SecP384R1Field.AddOne(this.x, z);
			return new SecP384R1FieldElement(z);
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x000F9864 File Offset: 0x000F7A64
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat.Create(12);
			SecP384R1Field.Subtract(this.x, ((SecP384R1FieldElement)b).x, z);
			return new SecP384R1FieldElement(z);
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x000F9898 File Offset: 0x000F7A98
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat.Create(12);
			SecP384R1Field.Multiply(this.x, ((SecP384R1FieldElement)b).x, z);
			return new SecP384R1FieldElement(z);
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x000F98CC File Offset: 0x000F7ACC
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat.Create(12);
			Mod.Invert(SecP384R1Field.P, ((SecP384R1FieldElement)b).x, z);
			SecP384R1Field.Multiply(z, this.x, z);
			return new SecP384R1FieldElement(z);
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x000F990C File Offset: 0x000F7B0C
		public override ECFieldElement Negate()
		{
			uint[] z = Nat.Create(12);
			SecP384R1Field.Negate(this.x, z);
			return new SecP384R1FieldElement(z);
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x000F9934 File Offset: 0x000F7B34
		public override ECFieldElement Square()
		{
			uint[] z = Nat.Create(12);
			SecP384R1Field.Square(this.x, z);
			return new SecP384R1FieldElement(z);
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x000F995C File Offset: 0x000F7B5C
		public override ECFieldElement Invert()
		{
			uint[] z = Nat.Create(12);
			Mod.Invert(SecP384R1Field.P, this.x, z);
			return new SecP384R1FieldElement(z);
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x000F9988 File Offset: 0x000F7B88
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat.IsZero(12, y) || Nat.IsOne(12, y))
			{
				return this;
			}
			uint[] array = Nat.Create(12);
			uint[] array2 = Nat.Create(12);
			uint[] array3 = Nat.Create(12);
			uint[] array4 = Nat.Create(12);
			SecP384R1Field.Square(y, array);
			SecP384R1Field.Multiply(array, y, array);
			SecP384R1Field.SquareN(array, 2, array2);
			SecP384R1Field.Multiply(array2, array, array2);
			SecP384R1Field.Square(array2, array2);
			SecP384R1Field.Multiply(array2, y, array2);
			SecP384R1Field.SquareN(array2, 5, array3);
			SecP384R1Field.Multiply(array3, array2, array3);
			SecP384R1Field.SquareN(array3, 5, array4);
			SecP384R1Field.Multiply(array4, array2, array4);
			SecP384R1Field.SquareN(array4, 15, array2);
			SecP384R1Field.Multiply(array2, array4, array2);
			SecP384R1Field.SquareN(array2, 2, array3);
			SecP384R1Field.Multiply(array, array3, array);
			SecP384R1Field.SquareN(array3, 28, array3);
			SecP384R1Field.Multiply(array2, array3, array2);
			SecP384R1Field.SquareN(array2, 60, array3);
			SecP384R1Field.Multiply(array3, array2, array3);
			uint[] z = array2;
			SecP384R1Field.SquareN(array3, 120, z);
			SecP384R1Field.Multiply(z, array3, z);
			SecP384R1Field.SquareN(z, 15, z);
			SecP384R1Field.Multiply(z, array4, z);
			SecP384R1Field.SquareN(z, 33, z);
			SecP384R1Field.Multiply(z, array, z);
			SecP384R1Field.SquareN(z, 64, z);
			SecP384R1Field.Multiply(z, y, z);
			SecP384R1Field.SquareN(z, 30, array);
			SecP384R1Field.Square(array, array2);
			if (!Nat.Eq(12, y, array2))
			{
				return null;
			}
			return new SecP384R1FieldElement(array);
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x000F9AE4 File Offset: 0x000F7CE4
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP384R1FieldElement);
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x000F9AE4 File Offset: 0x000F7CE4
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP384R1FieldElement);
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x000F9AF2 File Offset: 0x000F7CF2
		public virtual bool Equals(SecP384R1FieldElement other)
		{
			return this == other || (other != null && Nat.Eq(12, this.x, other.x));
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x000F9B12 File Offset: 0x000F7D12
		public override int GetHashCode()
		{
			return SecP384R1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 12);
		}

		// Token: 0x04001A6E RID: 6766
		public static readonly BigInteger Q = SecP384R1Curve.q;

		// Token: 0x04001A6F RID: 6767
		protected internal readonly uint[] x;
	}
}
