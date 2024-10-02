using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200039C RID: 924
	internal class SecT233FieldElement : AbstractF2mFieldElement
	{
		// Token: 0x06002519 RID: 9497 RVA: 0x00101E8B File Offset: 0x0010008B
		public SecT233FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 233)
			{
				throw new ArgumentException("value invalid for SecT233FieldElement", "x");
			}
			this.x = SecT233Field.FromBigInteger(x);
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x00101EC8 File Offset: 0x001000C8
		public SecT233FieldElement()
		{
			this.x = Nat256.Create64();
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x00101EDB File Offset: 0x001000DB
		protected internal SecT233FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x0600251C RID: 9500 RVA: 0x00101EEA File Offset: 0x001000EA
		public override bool IsOne
		{
			get
			{
				return Nat256.IsOne64(this.x);
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x0600251D RID: 9501 RVA: 0x00101EF7 File Offset: 0x001000F7
		public override bool IsZero
		{
			get
			{
				return Nat256.IsZero64(this.x);
			}
		}

		// Token: 0x0600251E RID: 9502 RVA: 0x00101F04 File Offset: 0x00100104
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x00101F15 File Offset: 0x00100115
		public override BigInteger ToBigInteger()
		{
			return Nat256.ToBigInteger64(this.x);
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06002520 RID: 9504 RVA: 0x00101F22 File Offset: 0x00100122
		public override string FieldName
		{
			get
			{
				return "SecT233Field";
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06002521 RID: 9505 RVA: 0x00101F29 File Offset: 0x00100129
		public override int FieldSize
		{
			get
			{
				return 233;
			}
		}

		// Token: 0x06002522 RID: 9506 RVA: 0x00101F30 File Offset: 0x00100130
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat256.Create64();
			SecT233Field.Add(this.x, ((SecT233FieldElement)b).x, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x06002523 RID: 9507 RVA: 0x00101F60 File Offset: 0x00100160
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat256.Create64();
			SecT233Field.AddOne(this.x, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x000E3A75 File Offset: 0x000E1C75
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x06002525 RID: 9509 RVA: 0x00101F88 File Offset: 0x00100188
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat256.Create64();
			SecT233Field.Multiply(this.x, ((SecT233FieldElement)b).x, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x000E3AB3 File Offset: 0x000E1CB3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x06002527 RID: 9511 RVA: 0x00101FB8 File Offset: 0x001001B8
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT233FieldElement)b).x;
			ulong[] array2 = ((SecT233FieldElement)x).x;
			ulong[] y3 = ((SecT233FieldElement)y).x;
			ulong[] array3 = Nat256.CreateExt64();
			SecT233Field.MultiplyAddToExt(array, y2, array3);
			SecT233Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat256.Create64();
			SecT233Field.Reduce(array3, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x000FB43A File Offset: 0x000F963A
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x000947CE File Offset: 0x000929CE
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x0010201C File Offset: 0x0010021C
		public override ECFieldElement Square()
		{
			ulong[] z = Nat256.Create64();
			SecT233Field.Square(this.x, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x000E3BA1 File Offset: 0x000E1DA1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x00102044 File Offset: 0x00100244
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT233FieldElement)x).x;
			ulong[] y2 = ((SecT233FieldElement)y).x;
			ulong[] array3 = Nat256.CreateExt64();
			SecT233Field.SquareAddToExt(array, array3);
			SecT233Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat256.Create64();
			SecT233Field.Reduce(array3, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x00102098 File Offset: 0x00100298
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat256.Create64();
			SecT233Field.SquareN(this.x, pow, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x001020C4 File Offset: 0x001002C4
		public override int Trace()
		{
			return (int)SecT233Field.Trace(this.x);
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x001020D4 File Offset: 0x001002D4
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat256.Create64();
			SecT233Field.Invert(this.x, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x001020FC File Offset: 0x001002FC
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat256.Create64();
			SecT233Field.Sqrt(this.x, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06002531 RID: 9521 RVA: 0x000A7398 File Offset: 0x000A5598
		public virtual int Representation
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06002532 RID: 9522 RVA: 0x00101F29 File Offset: 0x00100129
		public virtual int M
		{
			get
			{
				return 233;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06002533 RID: 9523 RVA: 0x00102121 File Offset: 0x00100321
		public virtual int K1
		{
			get
			{
				return 74;
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06002534 RID: 9524 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06002535 RID: 9525 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x00102125 File Offset: 0x00100325
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT233FieldElement);
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x00102125 File Offset: 0x00100325
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT233FieldElement);
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x00102133 File Offset: 0x00100333
		public virtual bool Equals(SecT233FieldElement other)
		{
			return this == other || (other != null && Nat256.Eq64(this.x, other.x));
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x00102151 File Offset: 0x00100351
		public override int GetHashCode()
		{
			return 2330074 ^ Arrays.GetHashCode(this.x, 0, 4);
		}

		// Token: 0x04001AA3 RID: 6819
		protected internal readonly ulong[] x;
	}
}
