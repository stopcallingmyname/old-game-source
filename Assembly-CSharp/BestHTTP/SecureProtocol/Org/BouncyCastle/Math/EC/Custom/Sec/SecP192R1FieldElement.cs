using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000367 RID: 871
	internal class SecP192R1FieldElement : AbstractFpFieldElement
	{
		// Token: 0x060021C0 RID: 8640 RVA: 0x000F4A85 File Offset: 0x000F2C85
		public SecP192R1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP192R1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP192R1FieldElement", "x");
			}
			this.x = SecP192R1Field.FromBigInteger(x);
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x000F4AC3 File Offset: 0x000F2CC3
		public SecP192R1FieldElement()
		{
			this.x = Nat192.Create();
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x000F4AD6 File Offset: 0x000F2CD6
		protected internal SecP192R1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060021C3 RID: 8643 RVA: 0x000F4AE5 File Offset: 0x000F2CE5
		public override bool IsZero
		{
			get
			{
				return Nat192.IsZero(this.x);
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060021C4 RID: 8644 RVA: 0x000F4AF2 File Offset: 0x000F2CF2
		public override bool IsOne
		{
			get
			{
				return Nat192.IsOne(this.x);
			}
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x000F4AFF File Offset: 0x000F2CFF
		public override bool TestBitZero()
		{
			return Nat192.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x000F4B10 File Offset: 0x000F2D10
		public override BigInteger ToBigInteger()
		{
			return Nat192.ToBigInteger(this.x);
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060021C7 RID: 8647 RVA: 0x000F4B1D File Offset: 0x000F2D1D
		public override string FieldName
		{
			get
			{
				return "SecP192R1Field";
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060021C8 RID: 8648 RVA: 0x000F4B24 File Offset: 0x000F2D24
		public override int FieldSize
		{
			get
			{
				return SecP192R1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x000F4B30 File Offset: 0x000F2D30
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat192.Create();
			SecP192R1Field.Add(this.x, ((SecP192R1FieldElement)b).x, z);
			return new SecP192R1FieldElement(z);
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x000F4B60 File Offset: 0x000F2D60
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat192.Create();
			SecP192R1Field.AddOne(this.x, z);
			return new SecP192R1FieldElement(z);
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x000F4B88 File Offset: 0x000F2D88
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat192.Create();
			SecP192R1Field.Subtract(this.x, ((SecP192R1FieldElement)b).x, z);
			return new SecP192R1FieldElement(z);
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x000F4BB8 File Offset: 0x000F2DB8
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat192.Create();
			SecP192R1Field.Multiply(this.x, ((SecP192R1FieldElement)b).x, z);
			return new SecP192R1FieldElement(z);
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x000F4BE8 File Offset: 0x000F2DE8
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat192.Create();
			Mod.Invert(SecP192R1Field.P, ((SecP192R1FieldElement)b).x, z);
			SecP192R1Field.Multiply(z, this.x, z);
			return new SecP192R1FieldElement(z);
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x000F4C24 File Offset: 0x000F2E24
		public override ECFieldElement Negate()
		{
			uint[] z = Nat192.Create();
			SecP192R1Field.Negate(this.x, z);
			return new SecP192R1FieldElement(z);
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x000F4C4C File Offset: 0x000F2E4C
		public override ECFieldElement Square()
		{
			uint[] z = Nat192.Create();
			SecP192R1Field.Square(this.x, z);
			return new SecP192R1FieldElement(z);
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x000F4C74 File Offset: 0x000F2E74
		public override ECFieldElement Invert()
		{
			uint[] z = Nat192.Create();
			Mod.Invert(SecP192R1Field.P, this.x, z);
			return new SecP192R1FieldElement(z);
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x000F4CA0 File Offset: 0x000F2EA0
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat192.IsZero(y) || Nat192.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat192.Create();
			uint[] array2 = Nat192.Create();
			SecP192R1Field.Square(y, array);
			SecP192R1Field.Multiply(array, y, array);
			SecP192R1Field.SquareN(array, 2, array2);
			SecP192R1Field.Multiply(array2, array, array2);
			SecP192R1Field.SquareN(array2, 4, array);
			SecP192R1Field.Multiply(array, array2, array);
			SecP192R1Field.SquareN(array, 8, array2);
			SecP192R1Field.Multiply(array2, array, array2);
			SecP192R1Field.SquareN(array2, 16, array);
			SecP192R1Field.Multiply(array, array2, array);
			SecP192R1Field.SquareN(array, 32, array2);
			SecP192R1Field.Multiply(array2, array, array2);
			SecP192R1Field.SquareN(array2, 64, array);
			SecP192R1Field.Multiply(array, array2, array);
			SecP192R1Field.SquareN(array, 62, array);
			SecP192R1Field.Square(array, array2);
			if (!Nat192.Eq(y, array2))
			{
				return null;
			}
			return new SecP192R1FieldElement(array);
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x000F4D65 File Offset: 0x000F2F65
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP192R1FieldElement);
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x000F4D65 File Offset: 0x000F2F65
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP192R1FieldElement);
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x000F4D73 File Offset: 0x000F2F73
		public virtual bool Equals(SecP192R1FieldElement other)
		{
			return this == other || (other != null && Nat192.Eq(this.x, other.x));
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x000F4D91 File Offset: 0x000F2F91
		public override int GetHashCode()
		{
			return SecP192R1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 6);
		}

		// Token: 0x04001A35 RID: 6709
		public static readonly BigInteger Q = SecP192R1Curve.q;

		// Token: 0x04001A36 RID: 6710
		protected internal readonly uint[] x;
	}
}
