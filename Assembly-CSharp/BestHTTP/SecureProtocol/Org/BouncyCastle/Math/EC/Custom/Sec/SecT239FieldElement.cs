using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x020003A2 RID: 930
	internal class SecT239FieldElement : AbstractF2mFieldElement
	{
		// Token: 0x06002581 RID: 9601 RVA: 0x001037F3 File Offset: 0x001019F3
		public SecT239FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 239)
			{
				throw new ArgumentException("value invalid for SecT239FieldElement", "x");
			}
			this.x = SecT239Field.FromBigInteger(x);
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x00103830 File Offset: 0x00101A30
		public SecT239FieldElement()
		{
			this.x = Nat256.Create64();
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x00103843 File Offset: 0x00101A43
		protected internal SecT239FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06002584 RID: 9604 RVA: 0x00103852 File Offset: 0x00101A52
		public override bool IsOne
		{
			get
			{
				return Nat256.IsOne64(this.x);
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06002585 RID: 9605 RVA: 0x0010385F File Offset: 0x00101A5F
		public override bool IsZero
		{
			get
			{
				return Nat256.IsZero64(this.x);
			}
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x0010386C File Offset: 0x00101A6C
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x0010387D File Offset: 0x00101A7D
		public override BigInteger ToBigInteger()
		{
			return Nat256.ToBigInteger64(this.x);
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06002588 RID: 9608 RVA: 0x0010388A File Offset: 0x00101A8A
		public override string FieldName
		{
			get
			{
				return "SecT239Field";
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06002589 RID: 9609 RVA: 0x00103891 File Offset: 0x00101A91
		public override int FieldSize
		{
			get
			{
				return 239;
			}
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x00103898 File Offset: 0x00101A98
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat256.Create64();
			SecT239Field.Add(this.x, ((SecT239FieldElement)b).x, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x001038C8 File Offset: 0x00101AC8
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat256.Create64();
			SecT239Field.AddOne(this.x, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x000E3A75 File Offset: 0x000E1C75
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x001038F0 File Offset: 0x00101AF0
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat256.Create64();
			SecT239Field.Multiply(this.x, ((SecT239FieldElement)b).x, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x000E3AB3 File Offset: 0x000E1CB3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x00103920 File Offset: 0x00101B20
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT239FieldElement)b).x;
			ulong[] array2 = ((SecT239FieldElement)x).x;
			ulong[] y3 = ((SecT239FieldElement)y).x;
			ulong[] array3 = Nat256.CreateExt64();
			SecT239Field.MultiplyAddToExt(array, y2, array3);
			SecT239Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat256.Create64();
			SecT239Field.Reduce(array3, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x000FB43A File Offset: 0x000F963A
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x000947CE File Offset: 0x000929CE
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x00103984 File Offset: 0x00101B84
		public override ECFieldElement Square()
		{
			ulong[] z = Nat256.Create64();
			SecT239Field.Square(this.x, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x000E3BA1 File Offset: 0x000E1DA1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x001039AC File Offset: 0x00101BAC
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT239FieldElement)x).x;
			ulong[] y2 = ((SecT239FieldElement)y).x;
			ulong[] array3 = Nat256.CreateExt64();
			SecT239Field.SquareAddToExt(array, array3);
			SecT239Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat256.Create64();
			SecT239Field.Reduce(array3, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x00103A00 File Offset: 0x00101C00
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat256.Create64();
			SecT239Field.SquareN(this.x, pow, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x00103A2C File Offset: 0x00101C2C
		public override int Trace()
		{
			return (int)SecT239Field.Trace(this.x);
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x00103A3C File Offset: 0x00101C3C
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat256.Create64();
			SecT239Field.Invert(this.x, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x00103A64 File Offset: 0x00101C64
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat256.Create64();
			SecT239Field.Sqrt(this.x, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06002599 RID: 9625 RVA: 0x000A7398 File Offset: 0x000A5598
		public virtual int Representation
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x0600259A RID: 9626 RVA: 0x00103891 File Offset: 0x00101A91
		public virtual int M
		{
			get
			{
				return 239;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x0600259B RID: 9627 RVA: 0x00103A89 File Offset: 0x00101C89
		public virtual int K1
		{
			get
			{
				return 158;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x0600259C RID: 9628 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x0600259D RID: 9629 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x00103A90 File Offset: 0x00101C90
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT239FieldElement);
		}

		// Token: 0x0600259F RID: 9631 RVA: 0x00103A90 File Offset: 0x00101C90
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT239FieldElement);
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x00103A9E File Offset: 0x00101C9E
		public virtual bool Equals(SecT239FieldElement other)
		{
			return this == other || (other != null && Nat256.Eq64(this.x, other.x));
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x00103ABC File Offset: 0x00101CBC
		public override int GetHashCode()
		{
			return 23900158 ^ Arrays.GetHashCode(this.x, 0, 4);
		}

		// Token: 0x04001AAC RID: 6828
		protected internal readonly ulong[] x;
	}
}
