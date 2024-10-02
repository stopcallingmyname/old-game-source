using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000388 RID: 904
	internal class SecT131FieldElement : AbstractF2mFieldElement
	{
		// Token: 0x060023CA RID: 9162 RVA: 0x000FCC53 File Offset: 0x000FAE53
		public SecT131FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 131)
			{
				throw new ArgumentException("value invalid for SecT131FieldElement", "x");
			}
			this.x = SecT131Field.FromBigInteger(x);
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x000FCC90 File Offset: 0x000FAE90
		public SecT131FieldElement()
		{
			this.x = Nat192.Create64();
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x000FCCA3 File Offset: 0x000FAEA3
		protected internal SecT131FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x060023CD RID: 9165 RVA: 0x000FCCB2 File Offset: 0x000FAEB2
		public override bool IsOne
		{
			get
			{
				return Nat192.IsOne64(this.x);
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x060023CE RID: 9166 RVA: 0x000FCCBF File Offset: 0x000FAEBF
		public override bool IsZero
		{
			get
			{
				return Nat192.IsZero64(this.x);
			}
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x000FCCCC File Offset: 0x000FAECC
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x000FCCDD File Offset: 0x000FAEDD
		public override BigInteger ToBigInteger()
		{
			return Nat192.ToBigInteger64(this.x);
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x060023D1 RID: 9169 RVA: 0x000FCCEA File Offset: 0x000FAEEA
		public override string FieldName
		{
			get
			{
				return "SecT131Field";
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x060023D2 RID: 9170 RVA: 0x000FCCF1 File Offset: 0x000FAEF1
		public override int FieldSize
		{
			get
			{
				return 131;
			}
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x000FCCF8 File Offset: 0x000FAEF8
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat192.Create64();
			SecT131Field.Add(this.x, ((SecT131FieldElement)b).x, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x000FCD28 File Offset: 0x000FAF28
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat192.Create64();
			SecT131Field.AddOne(this.x, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x000E3A75 File Offset: 0x000E1C75
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x060023D6 RID: 9174 RVA: 0x000FCD50 File Offset: 0x000FAF50
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat192.Create64();
			SecT131Field.Multiply(this.x, ((SecT131FieldElement)b).x, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x060023D7 RID: 9175 RVA: 0x000E3AB3 File Offset: 0x000E1CB3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x000FCD80 File Offset: 0x000FAF80
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT131FieldElement)b).x;
			ulong[] array2 = ((SecT131FieldElement)x).x;
			ulong[] y3 = ((SecT131FieldElement)y).x;
			ulong[] array3 = Nat.Create64(5);
			SecT131Field.MultiplyAddToExt(array, y2, array3);
			SecT131Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat192.Create64();
			SecT131Field.Reduce(array3, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x000FB43A File Offset: 0x000F963A
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x000947CE File Offset: 0x000929CE
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x000FCDE4 File Offset: 0x000FAFE4
		public override ECFieldElement Square()
		{
			ulong[] z = Nat192.Create64();
			SecT131Field.Square(this.x, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x000E3BA1 File Offset: 0x000E1DA1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x000FCE0C File Offset: 0x000FB00C
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT131FieldElement)x).x;
			ulong[] y2 = ((SecT131FieldElement)y).x;
			ulong[] array3 = Nat.Create64(5);
			SecT131Field.SquareAddToExt(array, array3);
			SecT131Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat192.Create64();
			SecT131Field.Reduce(array3, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x000FCE60 File Offset: 0x000FB060
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat192.Create64();
			SecT131Field.SquareN(this.x, pow, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x000FCE8C File Offset: 0x000FB08C
		public override int Trace()
		{
			return (int)SecT131Field.Trace(this.x);
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x000FCE9C File Offset: 0x000FB09C
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat192.Create64();
			SecT131Field.Invert(this.x, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x000FCEC4 File Offset: 0x000FB0C4
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat192.Create64();
			SecT131Field.Sqrt(this.x, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x060023E2 RID: 9186 RVA: 0x000A89A4 File Offset: 0x000A6BA4
		public virtual int Representation
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x060023E3 RID: 9187 RVA: 0x000FCCF1 File Offset: 0x000FAEF1
		public virtual int M
		{
			get
			{
				return 131;
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x060023E4 RID: 9188 RVA: 0x000A7398 File Offset: 0x000A5598
		public virtual int K1
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x060023E5 RID: 9189 RVA: 0x000A89A4 File Offset: 0x000A6BA4
		public virtual int K2
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x060023E6 RID: 9190 RVA: 0x000FCEE9 File Offset: 0x000FB0E9
		public virtual int K3
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x000FCEEC File Offset: 0x000FB0EC
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT131FieldElement);
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x000FCEEC File Offset: 0x000FB0EC
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT131FieldElement);
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x000FCEFA File Offset: 0x000FB0FA
		public virtual bool Equals(SecT131FieldElement other)
		{
			return this == other || (other != null && Nat192.Eq64(this.x, other.x));
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x000FCF18 File Offset: 0x000FB118
		public override int GetHashCode()
		{
			return 131832 ^ Arrays.GetHashCode(this.x, 0, 3);
		}

		// Token: 0x04001A84 RID: 6788
		protected internal readonly ulong[] x;
	}
}
