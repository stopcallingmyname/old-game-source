using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x020003B2 RID: 946
	internal class SecT571FieldElement : AbstractF2mFieldElement
	{
		// Token: 0x060026A2 RID: 9890 RVA: 0x00107C7F File Offset: 0x00105E7F
		public SecT571FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 571)
			{
				throw new ArgumentException("value invalid for SecT571FieldElement", "x");
			}
			this.x = SecT571Field.FromBigInteger(x);
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x00107CBC File Offset: 0x00105EBC
		public SecT571FieldElement()
		{
			this.x = Nat576.Create64();
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x00107CCF File Offset: 0x00105ECF
		protected internal SecT571FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x060026A5 RID: 9893 RVA: 0x00107CDE File Offset: 0x00105EDE
		public override bool IsOne
		{
			get
			{
				return Nat576.IsOne64(this.x);
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x060026A6 RID: 9894 RVA: 0x00107CEB File Offset: 0x00105EEB
		public override bool IsZero
		{
			get
			{
				return Nat576.IsZero64(this.x);
			}
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x00107CF8 File Offset: 0x00105EF8
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x00107D09 File Offset: 0x00105F09
		public override BigInteger ToBigInteger()
		{
			return Nat576.ToBigInteger64(this.x);
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x060026A9 RID: 9897 RVA: 0x00107D16 File Offset: 0x00105F16
		public override string FieldName
		{
			get
			{
				return "SecT571Field";
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x060026AA RID: 9898 RVA: 0x00107D1D File Offset: 0x00105F1D
		public override int FieldSize
		{
			get
			{
				return 571;
			}
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x00107D24 File Offset: 0x00105F24
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat576.Create64();
			SecT571Field.Add(this.x, ((SecT571FieldElement)b).x, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x00107D54 File Offset: 0x00105F54
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat576.Create64();
			SecT571Field.AddOne(this.x, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x060026AD RID: 9901 RVA: 0x000E3A75 File Offset: 0x000E1C75
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x00107D7C File Offset: 0x00105F7C
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat576.Create64();
			SecT571Field.Multiply(this.x, ((SecT571FieldElement)b).x, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x000E3AB3 File Offset: 0x000E1CB3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x00107DAC File Offset: 0x00105FAC
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT571FieldElement)b).x;
			ulong[] array2 = ((SecT571FieldElement)x).x;
			ulong[] y3 = ((SecT571FieldElement)y).x;
			ulong[] array3 = Nat576.CreateExt64();
			SecT571Field.MultiplyAddToExt(array, y2, array3);
			SecT571Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat576.Create64();
			SecT571Field.Reduce(array3, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x000FB43A File Offset: 0x000F963A
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x000947CE File Offset: 0x000929CE
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x00107E10 File Offset: 0x00106010
		public override ECFieldElement Square()
		{
			ulong[] z = Nat576.Create64();
			SecT571Field.Square(this.x, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x000E3BA1 File Offset: 0x000E1DA1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x00107E38 File Offset: 0x00106038
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT571FieldElement)x).x;
			ulong[] y2 = ((SecT571FieldElement)y).x;
			ulong[] array3 = Nat576.CreateExt64();
			SecT571Field.SquareAddToExt(array, array3);
			SecT571Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat576.Create64();
			SecT571Field.Reduce(array3, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x00107E8C File Offset: 0x0010608C
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat576.Create64();
			SecT571Field.SquareN(this.x, pow, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x00107EB8 File Offset: 0x001060B8
		public override int Trace()
		{
			return (int)SecT571Field.Trace(this.x);
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x00107EC8 File Offset: 0x001060C8
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat576.Create64();
			SecT571Field.Invert(this.x, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x00107EF0 File Offset: 0x001060F0
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat576.Create64();
			SecT571Field.Sqrt(this.x, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x060026BA RID: 9914 RVA: 0x000A89A4 File Offset: 0x000A6BA4
		public virtual int Representation
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x060026BB RID: 9915 RVA: 0x00107D1D File Offset: 0x00105F1D
		public virtual int M
		{
			get
			{
				return 571;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x060026BC RID: 9916 RVA: 0x000A7398 File Offset: 0x000A5598
		public virtual int K1
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x060026BD RID: 9917 RVA: 0x000A4E1E File Offset: 0x000A301E
		public virtual int K2
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x060026BE RID: 9918 RVA: 0x00107F15 File Offset: 0x00106115
		public virtual int K3
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x00107F19 File Offset: 0x00106119
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT571FieldElement);
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x00107F19 File Offset: 0x00106119
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT571FieldElement);
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x00107F27 File Offset: 0x00106127
		public virtual bool Equals(SecT571FieldElement other)
		{
			return this == other || (other != null && Nat576.Eq64(this.x, other.x));
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x00107F45 File Offset: 0x00106145
		public override int GetHashCode()
		{
			return 5711052 ^ Arrays.GetHashCode(this.x, 0, 9);
		}

		// Token: 0x04001AC6 RID: 6854
		protected internal readonly ulong[] x;
	}
}
