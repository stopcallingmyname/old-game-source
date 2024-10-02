using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x020003A6 RID: 934
	internal class SecT283FieldElement : AbstractF2mFieldElement
	{
		// Token: 0x060025D1 RID: 9681 RVA: 0x00104B49 File Offset: 0x00102D49
		public SecT283FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 283)
			{
				throw new ArgumentException("value invalid for SecT283FieldElement", "x");
			}
			this.x = SecT283Field.FromBigInteger(x);
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x00104B86 File Offset: 0x00102D86
		public SecT283FieldElement()
		{
			this.x = Nat320.Create64();
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x00104B99 File Offset: 0x00102D99
		protected internal SecT283FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x060025D4 RID: 9684 RVA: 0x00104BA8 File Offset: 0x00102DA8
		public override bool IsOne
		{
			get
			{
				return Nat320.IsOne64(this.x);
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x060025D5 RID: 9685 RVA: 0x00104BB5 File Offset: 0x00102DB5
		public override bool IsZero
		{
			get
			{
				return Nat320.IsZero64(this.x);
			}
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x00104BC2 File Offset: 0x00102DC2
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x00104BD3 File Offset: 0x00102DD3
		public override BigInteger ToBigInteger()
		{
			return Nat320.ToBigInteger64(this.x);
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x060025D8 RID: 9688 RVA: 0x00104BE0 File Offset: 0x00102DE0
		public override string FieldName
		{
			get
			{
				return "SecT283Field";
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x060025D9 RID: 9689 RVA: 0x00104BE7 File Offset: 0x00102DE7
		public override int FieldSize
		{
			get
			{
				return 283;
			}
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x00104BF0 File Offset: 0x00102DF0
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat320.Create64();
			SecT283Field.Add(this.x, ((SecT283FieldElement)b).x, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x00104C20 File Offset: 0x00102E20
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat320.Create64();
			SecT283Field.AddOne(this.x, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x000E3A75 File Offset: 0x000E1C75
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x00104C48 File Offset: 0x00102E48
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat320.Create64();
			SecT283Field.Multiply(this.x, ((SecT283FieldElement)b).x, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x000E3AB3 File Offset: 0x000E1CB3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x00104C78 File Offset: 0x00102E78
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT283FieldElement)b).x;
			ulong[] array2 = ((SecT283FieldElement)x).x;
			ulong[] y3 = ((SecT283FieldElement)y).x;
			ulong[] array3 = Nat.Create64(9);
			SecT283Field.MultiplyAddToExt(array, y2, array3);
			SecT283Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat320.Create64();
			SecT283Field.Reduce(array3, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x000FB43A File Offset: 0x000F963A
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x000947CE File Offset: 0x000929CE
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x00104CDC File Offset: 0x00102EDC
		public override ECFieldElement Square()
		{
			ulong[] z = Nat320.Create64();
			SecT283Field.Square(this.x, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x000E3BA1 File Offset: 0x000E1DA1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x00104D04 File Offset: 0x00102F04
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT283FieldElement)x).x;
			ulong[] y2 = ((SecT283FieldElement)y).x;
			ulong[] array3 = Nat.Create64(9);
			SecT283Field.SquareAddToExt(array, array3);
			SecT283Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat320.Create64();
			SecT283Field.Reduce(array3, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x00104D58 File Offset: 0x00102F58
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat320.Create64();
			SecT283Field.SquareN(this.x, pow, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x00104D84 File Offset: 0x00102F84
		public override int Trace()
		{
			return (int)SecT283Field.Trace(this.x);
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x00104D94 File Offset: 0x00102F94
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat320.Create64();
			SecT283Field.Invert(this.x, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x00104DBC File Offset: 0x00102FBC
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat320.Create64();
			SecT283Field.Sqrt(this.x, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x060025E9 RID: 9705 RVA: 0x000A89A4 File Offset: 0x000A6BA4
		public virtual int Representation
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x060025EA RID: 9706 RVA: 0x00104BE7 File Offset: 0x00102DE7
		public virtual int M
		{
			get
			{
				return 283;
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x060025EB RID: 9707 RVA: 0x000A4E1E File Offset: 0x000A301E
		public virtual int K1
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060025EC RID: 9708 RVA: 0x000FE865 File Offset: 0x000FCA65
		public virtual int K2
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x060025ED RID: 9709 RVA: 0x00104DE1 File Offset: 0x00102FE1
		public virtual int K3
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x00104DE5 File Offset: 0x00102FE5
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT283FieldElement);
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x00104DE5 File Offset: 0x00102FE5
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT283FieldElement);
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x00104DF3 File Offset: 0x00102FF3
		public virtual bool Equals(SecT283FieldElement other)
		{
			return this == other || (other != null && Nat320.Eq64(this.x, other.x));
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x00104E11 File Offset: 0x00103011
		public override int GetHashCode()
		{
			return 2831275 ^ Arrays.GetHashCode(this.x, 0, 5);
		}

		// Token: 0x04001AB3 RID: 6835
		protected internal readonly ulong[] x;
	}
}
