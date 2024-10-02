using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x020003AC RID: 940
	internal class SecT409FieldElement : AbstractF2mFieldElement
	{
		// Token: 0x06002639 RID: 9785 RVA: 0x0010647E File Offset: 0x0010467E
		public SecT409FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 409)
			{
				throw new ArgumentException("value invalid for SecT409FieldElement", "x");
			}
			this.x = SecT409Field.FromBigInteger(x);
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x001064BB File Offset: 0x001046BB
		public SecT409FieldElement()
		{
			this.x = Nat448.Create64();
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x001064CE File Offset: 0x001046CE
		protected internal SecT409FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x0600263C RID: 9788 RVA: 0x001064DD File Offset: 0x001046DD
		public override bool IsOne
		{
			get
			{
				return Nat448.IsOne64(this.x);
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x0600263D RID: 9789 RVA: 0x001064EA File Offset: 0x001046EA
		public override bool IsZero
		{
			get
			{
				return Nat448.IsZero64(this.x);
			}
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x001064F7 File Offset: 0x001046F7
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x00106508 File Offset: 0x00104708
		public override BigInteger ToBigInteger()
		{
			return Nat448.ToBigInteger64(this.x);
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06002640 RID: 9792 RVA: 0x00106515 File Offset: 0x00104715
		public override string FieldName
		{
			get
			{
				return "SecT409Field";
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06002641 RID: 9793 RVA: 0x0010651C File Offset: 0x0010471C
		public override int FieldSize
		{
			get
			{
				return 409;
			}
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x00106524 File Offset: 0x00104724
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat448.Create64();
			SecT409Field.Add(this.x, ((SecT409FieldElement)b).x, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x00106554 File Offset: 0x00104754
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat448.Create64();
			SecT409Field.AddOne(this.x, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x000E3A75 File Offset: 0x000E1C75
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x0010657C File Offset: 0x0010477C
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat448.Create64();
			SecT409Field.Multiply(this.x, ((SecT409FieldElement)b).x, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x000E3AB3 File Offset: 0x000E1CB3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x001065AC File Offset: 0x001047AC
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT409FieldElement)b).x;
			ulong[] array2 = ((SecT409FieldElement)x).x;
			ulong[] y3 = ((SecT409FieldElement)y).x;
			ulong[] array3 = Nat.Create64(13);
			SecT409Field.MultiplyAddToExt(array, y2, array3);
			SecT409Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat448.Create64();
			SecT409Field.Reduce(array3, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x000FB43A File Offset: 0x000F963A
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x000947CE File Offset: 0x000929CE
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x00106610 File Offset: 0x00104810
		public override ECFieldElement Square()
		{
			ulong[] z = Nat448.Create64();
			SecT409Field.Square(this.x, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x000E3BA1 File Offset: 0x000E1DA1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x00106638 File Offset: 0x00104838
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT409FieldElement)x).x;
			ulong[] y2 = ((SecT409FieldElement)y).x;
			ulong[] array3 = Nat.Create64(13);
			SecT409Field.SquareAddToExt(array, array3);
			SecT409Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat448.Create64();
			SecT409Field.Reduce(array3, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x0010668C File Offset: 0x0010488C
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat448.Create64();
			SecT409Field.SquareN(this.x, pow, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x001066B8 File Offset: 0x001048B8
		public override int Trace()
		{
			return (int)SecT409Field.Trace(this.x);
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x001066C8 File Offset: 0x001048C8
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat448.Create64();
			SecT409Field.Invert(this.x, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x001066F0 File Offset: 0x001048F0
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat448.Create64();
			SecT409Field.Sqrt(this.x, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06002651 RID: 9809 RVA: 0x000A7398 File Offset: 0x000A5598
		public virtual int Representation
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06002652 RID: 9810 RVA: 0x0010651C File Offset: 0x0010471C
		public virtual int M
		{
			get
			{
				return 409;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06002653 RID: 9811 RVA: 0x00106715 File Offset: 0x00104915
		public virtual int K1
		{
			get
			{
				return 87;
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06002654 RID: 9812 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06002655 RID: 9813 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x00106719 File Offset: 0x00104919
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT409FieldElement);
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x00106719 File Offset: 0x00104919
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT409FieldElement);
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x00106727 File Offset: 0x00104927
		public virtual bool Equals(SecT409FieldElement other)
		{
			return this == other || (other != null && Nat448.Eq64(this.x, other.x));
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x00106745 File Offset: 0x00104945
		public override int GetHashCode()
		{
			return 4090087 ^ Arrays.GetHashCode(this.x, 0, 7);
		}

		// Token: 0x04001ABC RID: 6844
		protected internal readonly ulong[] x;
	}
}
