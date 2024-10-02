using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000396 RID: 918
	internal class SecT193FieldElement : AbstractF2mFieldElement
	{
		// Token: 0x060024B2 RID: 9394 RVA: 0x001004FF File Offset: 0x000FE6FF
		public SecT193FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 193)
			{
				throw new ArgumentException("value invalid for SecT193FieldElement", "x");
			}
			this.x = SecT193Field.FromBigInteger(x);
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x0010053C File Offset: 0x000FE73C
		public SecT193FieldElement()
		{
			this.x = Nat256.Create64();
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x0010054F File Offset: 0x000FE74F
		protected internal SecT193FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x060024B5 RID: 9397 RVA: 0x0010055E File Offset: 0x000FE75E
		public override bool IsOne
		{
			get
			{
				return Nat256.IsOne64(this.x);
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x060024B6 RID: 9398 RVA: 0x0010056B File Offset: 0x000FE76B
		public override bool IsZero
		{
			get
			{
				return Nat256.IsZero64(this.x);
			}
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x00100578 File Offset: 0x000FE778
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x00100589 File Offset: 0x000FE789
		public override BigInteger ToBigInteger()
		{
			return Nat256.ToBigInteger64(this.x);
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x060024B9 RID: 9401 RVA: 0x00100596 File Offset: 0x000FE796
		public override string FieldName
		{
			get
			{
				return "SecT193Field";
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x060024BA RID: 9402 RVA: 0x0010059D File Offset: 0x000FE79D
		public override int FieldSize
		{
			get
			{
				return 193;
			}
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x001005A4 File Offset: 0x000FE7A4
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat256.Create64();
			SecT193Field.Add(this.x, ((SecT193FieldElement)b).x, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x001005D4 File Offset: 0x000FE7D4
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat256.Create64();
			SecT193Field.AddOne(this.x, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x000E3A75 File Offset: 0x000E1C75
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x001005FC File Offset: 0x000FE7FC
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat256.Create64();
			SecT193Field.Multiply(this.x, ((SecT193FieldElement)b).x, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x000E3AB3 File Offset: 0x000E1CB3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x0010062C File Offset: 0x000FE82C
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT193FieldElement)b).x;
			ulong[] array2 = ((SecT193FieldElement)x).x;
			ulong[] y3 = ((SecT193FieldElement)y).x;
			ulong[] array3 = Nat256.CreateExt64();
			SecT193Field.MultiplyAddToExt(array, y2, array3);
			SecT193Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat256.Create64();
			SecT193Field.Reduce(array3, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x000FB43A File Offset: 0x000F963A
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x000947CE File Offset: 0x000929CE
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x00100690 File Offset: 0x000FE890
		public override ECFieldElement Square()
		{
			ulong[] z = Nat256.Create64();
			SecT193Field.Square(this.x, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x000E3BA1 File Offset: 0x000E1DA1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x001006B8 File Offset: 0x000FE8B8
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT193FieldElement)x).x;
			ulong[] y2 = ((SecT193FieldElement)y).x;
			ulong[] array3 = Nat256.CreateExt64();
			SecT193Field.SquareAddToExt(array, array3);
			SecT193Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat256.Create64();
			SecT193Field.Reduce(array3, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x0010070C File Offset: 0x000FE90C
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat256.Create64();
			SecT193Field.SquareN(this.x, pow, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x00100738 File Offset: 0x000FE938
		public override int Trace()
		{
			return (int)SecT193Field.Trace(this.x);
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x00100748 File Offset: 0x000FE948
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat256.Create64();
			SecT193Field.Invert(this.x, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x00100770 File Offset: 0x000FE970
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat256.Create64();
			SecT193Field.Sqrt(this.x, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x060024CA RID: 9418 RVA: 0x000A7398 File Offset: 0x000A5598
		public virtual int Representation
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x060024CB RID: 9419 RVA: 0x0010059D File Offset: 0x000FE79D
		public virtual int M
		{
			get
			{
				return 193;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x060024CC RID: 9420 RVA: 0x00100795 File Offset: 0x000FE995
		public virtual int K1
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x060024CD RID: 9421 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x060024CE RID: 9422 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x00100799 File Offset: 0x000FE999
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT193FieldElement);
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x00100799 File Offset: 0x000FE999
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT193FieldElement);
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x001007A7 File Offset: 0x000FE9A7
		public virtual bool Equals(SecT193FieldElement other)
		{
			return this == other || (other != null && Nat256.Eq64(this.x, other.x));
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x001007C5 File Offset: 0x000FE9C5
		public override int GetHashCode()
		{
			return 1930015 ^ Arrays.GetHashCode(this.x, 0, 4);
		}

		// Token: 0x04001A9A RID: 6810
		protected internal readonly ulong[] x;
	}
}
