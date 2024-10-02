using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200036F RID: 879
	internal class SecP224R1FieldElement : AbstractFpFieldElement
	{
		// Token: 0x0600223A RID: 8762 RVA: 0x000F6801 File Offset: 0x000F4A01
		public SecP224R1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP224R1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP224R1FieldElement", "x");
			}
			this.x = SecP224R1Field.FromBigInteger(x);
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x000F683F File Offset: 0x000F4A3F
		public SecP224R1FieldElement()
		{
			this.x = Nat224.Create();
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x000F6852 File Offset: 0x000F4A52
		protected internal SecP224R1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x0600223D RID: 8765 RVA: 0x000F6861 File Offset: 0x000F4A61
		public override bool IsZero
		{
			get
			{
				return Nat224.IsZero(this.x);
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x0600223E RID: 8766 RVA: 0x000F686E File Offset: 0x000F4A6E
		public override bool IsOne
		{
			get
			{
				return Nat224.IsOne(this.x);
			}
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x000F687B File Offset: 0x000F4A7B
		public override bool TestBitZero()
		{
			return Nat224.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x000F688C File Offset: 0x000F4A8C
		public override BigInteger ToBigInteger()
		{
			return Nat224.ToBigInteger(this.x);
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06002241 RID: 8769 RVA: 0x000F6899 File Offset: 0x000F4A99
		public override string FieldName
		{
			get
			{
				return "SecP224R1Field";
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06002242 RID: 8770 RVA: 0x000F68A0 File Offset: 0x000F4AA0
		public override int FieldSize
		{
			get
			{
				return SecP224R1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x000F68AC File Offset: 0x000F4AAC
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat224.Create();
			SecP224R1Field.Add(this.x, ((SecP224R1FieldElement)b).x, z);
			return new SecP224R1FieldElement(z);
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x000F68DC File Offset: 0x000F4ADC
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat224.Create();
			SecP224R1Field.AddOne(this.x, z);
			return new SecP224R1FieldElement(z);
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x000F6904 File Offset: 0x000F4B04
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat224.Create();
			SecP224R1Field.Subtract(this.x, ((SecP224R1FieldElement)b).x, z);
			return new SecP224R1FieldElement(z);
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x000F6934 File Offset: 0x000F4B34
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat224.Create();
			SecP224R1Field.Multiply(this.x, ((SecP224R1FieldElement)b).x, z);
			return new SecP224R1FieldElement(z);
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x000F6964 File Offset: 0x000F4B64
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat224.Create();
			Mod.Invert(SecP224R1Field.P, ((SecP224R1FieldElement)b).x, z);
			SecP224R1Field.Multiply(z, this.x, z);
			return new SecP224R1FieldElement(z);
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x000F69A0 File Offset: 0x000F4BA0
		public override ECFieldElement Negate()
		{
			uint[] z = Nat224.Create();
			SecP224R1Field.Negate(this.x, z);
			return new SecP224R1FieldElement(z);
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x000F69C8 File Offset: 0x000F4BC8
		public override ECFieldElement Square()
		{
			uint[] z = Nat224.Create();
			SecP224R1Field.Square(this.x, z);
			return new SecP224R1FieldElement(z);
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x000F69F0 File Offset: 0x000F4BF0
		public override ECFieldElement Invert()
		{
			uint[] z = Nat224.Create();
			Mod.Invert(SecP224R1Field.P, this.x, z);
			return new SecP224R1FieldElement(z);
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x000F6A1C File Offset: 0x000F4C1C
		public override ECFieldElement Sqrt()
		{
			uint[] array = this.x;
			if (Nat224.IsZero(array) || Nat224.IsOne(array))
			{
				return this;
			}
			uint[] array2 = Nat224.Create();
			SecP224R1Field.Negate(array, array2);
			uint[] array3 = Mod.Random(SecP224R1Field.P);
			uint[] t = Nat224.Create();
			if (!SecP224R1FieldElement.IsSquare(array))
			{
				return null;
			}
			while (!SecP224R1FieldElement.TrySqrt(array2, array3, t))
			{
				SecP224R1Field.AddOne(array3, array3);
			}
			SecP224R1Field.Square(t, array3);
			if (!Nat224.Eq(array, array3))
			{
				return null;
			}
			return new SecP224R1FieldElement(t);
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x000F6A93 File Offset: 0x000F4C93
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP224R1FieldElement);
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x000F6A93 File Offset: 0x000F4C93
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP224R1FieldElement);
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x000F6AA1 File Offset: 0x000F4CA1
		public virtual bool Equals(SecP224R1FieldElement other)
		{
			return this == other || (other != null && Nat224.Eq(this.x, other.x));
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x000F6ABF File Offset: 0x000F4CBF
		public override int GetHashCode()
		{
			return SecP224R1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 7);
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x000F6ADC File Offset: 0x000F4CDC
		private static bool IsSquare(uint[] x)
		{
			uint[] z = Nat224.Create();
			uint[] array = Nat224.Create();
			Nat224.Copy(x, z);
			for (int i = 0; i < 7; i++)
			{
				Nat224.Copy(z, array);
				SecP224R1Field.SquareN(z, 1 << i, z);
				SecP224R1Field.Multiply(z, array, z);
			}
			SecP224R1Field.SquareN(z, 95, z);
			return Nat224.IsOne(z);
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x000F6B34 File Offset: 0x000F4D34
		private static void RM(uint[] nc, uint[] d0, uint[] e0, uint[] d1, uint[] e1, uint[] f1, uint[] t)
		{
			SecP224R1Field.Multiply(e1, e0, t);
			SecP224R1Field.Multiply(t, nc, t);
			SecP224R1Field.Multiply(d1, d0, f1);
			SecP224R1Field.Add(f1, t, f1);
			SecP224R1Field.Multiply(d1, e0, t);
			Nat224.Copy(f1, d1);
			SecP224R1Field.Multiply(e1, d0, e1);
			SecP224R1Field.Add(e1, t, e1);
			SecP224R1Field.Square(e1, f1);
			SecP224R1Field.Multiply(f1, nc, f1);
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x000F6BA4 File Offset: 0x000F4DA4
		private static void RP(uint[] nc, uint[] d1, uint[] e1, uint[] f1, uint[] t)
		{
			Nat224.Copy(nc, f1);
			uint[] array = Nat224.Create();
			uint[] array2 = Nat224.Create();
			for (int i = 0; i < 7; i++)
			{
				Nat224.Copy(d1, array);
				Nat224.Copy(e1, array2);
				int num = 1 << i;
				while (--num >= 0)
				{
					SecP224R1FieldElement.RS(d1, e1, f1, t);
				}
				SecP224R1FieldElement.RM(nc, array, array2, d1, e1, f1, t);
			}
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x000F6C06 File Offset: 0x000F4E06
		private static void RS(uint[] d, uint[] e, uint[] f, uint[] t)
		{
			SecP224R1Field.Multiply(e, d, e);
			SecP224R1Field.Twice(e, e);
			SecP224R1Field.Square(d, t);
			SecP224R1Field.Add(f, t, d);
			SecP224R1Field.Multiply(f, t, f);
			SecP224R1Field.Reduce32(Nat.ShiftUpBits(7, f, 2, 0U), f);
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x000F6C40 File Offset: 0x000F4E40
		private static bool TrySqrt(uint[] nc, uint[] r, uint[] t)
		{
			uint[] array = Nat224.Create();
			Nat224.Copy(r, array);
			uint[] array2 = Nat224.Create();
			array2[0] = 1U;
			uint[] array3 = Nat224.Create();
			SecP224R1FieldElement.RP(nc, array, array2, array3, t);
			uint[] array4 = Nat224.Create();
			uint[] z = Nat224.Create();
			for (int i = 1; i < 96; i++)
			{
				Nat224.Copy(array, array4);
				Nat224.Copy(array2, z);
				SecP224R1FieldElement.RS(array, array2, array3, t);
				if (Nat224.IsZero(array))
				{
					Mod.Invert(SecP224R1Field.P, z, t);
					SecP224R1Field.Multiply(t, array4, t);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001A4D RID: 6733
		public static readonly BigInteger Q = SecP224R1Curve.q;

		// Token: 0x04001A4E RID: 6734
		protected internal readonly uint[] x;
	}
}
