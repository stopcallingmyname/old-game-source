using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.GM
{
	// Token: 0x020003B9 RID: 953
	internal class SM2P256V1FieldElement : AbstractFpFieldElement
	{
		// Token: 0x06002715 RID: 10005 RVA: 0x0010949F File Offset: 0x0010769F
		public SM2P256V1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SM2P256V1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SM2P256V1FieldElement", "x");
			}
			this.x = SM2P256V1Field.FromBigInteger(x);
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x001094DD File Offset: 0x001076DD
		public SM2P256V1FieldElement()
		{
			this.x = Nat256.Create();
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x001094F0 File Offset: 0x001076F0
		protected internal SM2P256V1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06002718 RID: 10008 RVA: 0x001094FF File Offset: 0x001076FF
		public override bool IsZero
		{
			get
			{
				return Nat256.IsZero(this.x);
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06002719 RID: 10009 RVA: 0x0010950C File Offset: 0x0010770C
		public override bool IsOne
		{
			get
			{
				return Nat256.IsOne(this.x);
			}
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x00109519 File Offset: 0x00107719
		public override bool TestBitZero()
		{
			return Nat256.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x0010952A File Offset: 0x0010772A
		public override BigInteger ToBigInteger()
		{
			return Nat256.ToBigInteger(this.x);
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x0600271C RID: 10012 RVA: 0x00109537 File Offset: 0x00107737
		public override string FieldName
		{
			get
			{
				return "SM2P256V1Field";
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x0600271D RID: 10013 RVA: 0x0010953E File Offset: 0x0010773E
		public override int FieldSize
		{
			get
			{
				return SM2P256V1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x0010954C File Offset: 0x0010774C
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			SM2P256V1Field.Add(this.x, ((SM2P256V1FieldElement)b).x, z);
			return new SM2P256V1FieldElement(z);
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x0010957C File Offset: 0x0010777C
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat256.Create();
			SM2P256V1Field.AddOne(this.x, z);
			return new SM2P256V1FieldElement(z);
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x001095A4 File Offset: 0x001077A4
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			SM2P256V1Field.Subtract(this.x, ((SM2P256V1FieldElement)b).x, z);
			return new SM2P256V1FieldElement(z);
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x001095D4 File Offset: 0x001077D4
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			SM2P256V1Field.Multiply(this.x, ((SM2P256V1FieldElement)b).x, z);
			return new SM2P256V1FieldElement(z);
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x00109604 File Offset: 0x00107804
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			Mod.Invert(SM2P256V1Field.P, ((SM2P256V1FieldElement)b).x, z);
			SM2P256V1Field.Multiply(z, this.x, z);
			return new SM2P256V1FieldElement(z);
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x00109640 File Offset: 0x00107840
		public override ECFieldElement Negate()
		{
			uint[] z = Nat256.Create();
			SM2P256V1Field.Negate(this.x, z);
			return new SM2P256V1FieldElement(z);
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x00109668 File Offset: 0x00107868
		public override ECFieldElement Square()
		{
			uint[] z = Nat256.Create();
			SM2P256V1Field.Square(this.x, z);
			return new SM2P256V1FieldElement(z);
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x00109690 File Offset: 0x00107890
		public override ECFieldElement Invert()
		{
			uint[] z = Nat256.Create();
			Mod.Invert(SM2P256V1Field.P, this.x, z);
			return new SM2P256V1FieldElement(z);
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x001096BC File Offset: 0x001078BC
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat256.IsZero(y) || Nat256.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat256.Create();
			SM2P256V1Field.Square(y, array);
			SM2P256V1Field.Multiply(array, y, array);
			uint[] array2 = Nat256.Create();
			SM2P256V1Field.SquareN(array, 2, array2);
			SM2P256V1Field.Multiply(array2, array, array2);
			uint[] array3 = Nat256.Create();
			SM2P256V1Field.SquareN(array2, 2, array3);
			SM2P256V1Field.Multiply(array3, array, array3);
			uint[] array4 = array;
			SM2P256V1Field.SquareN(array3, 6, array4);
			SM2P256V1Field.Multiply(array4, array3, array4);
			uint[] array5 = Nat256.Create();
			SM2P256V1Field.SquareN(array4, 12, array5);
			SM2P256V1Field.Multiply(array5, array4, array5);
			uint[] array6 = array4;
			SM2P256V1Field.SquareN(array5, 6, array6);
			SM2P256V1Field.Multiply(array6, array3, array6);
			uint[] array7 = array3;
			SM2P256V1Field.Square(array6, array7);
			SM2P256V1Field.Multiply(array7, y, array7);
			uint[] z = array5;
			SM2P256V1Field.SquareN(array7, 31, z);
			uint[] array8 = array6;
			SM2P256V1Field.Multiply(z, array7, array8);
			SM2P256V1Field.SquareN(z, 32, z);
			SM2P256V1Field.Multiply(z, array8, z);
			SM2P256V1Field.SquareN(z, 62, z);
			SM2P256V1Field.Multiply(z, array8, z);
			SM2P256V1Field.SquareN(z, 4, z);
			SM2P256V1Field.Multiply(z, array2, z);
			SM2P256V1Field.SquareN(z, 32, z);
			SM2P256V1Field.Multiply(z, y, z);
			SM2P256V1Field.SquareN(z, 62, z);
			uint[] array9 = array2;
			SM2P256V1Field.Square(z, array9);
			if (!Nat256.Eq(y, array9))
			{
				return null;
			}
			return new SM2P256V1FieldElement(z);
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x00109821 File Offset: 0x00107A21
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SM2P256V1FieldElement);
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x00109821 File Offset: 0x00107A21
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SM2P256V1FieldElement);
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x0010982F File Offset: 0x00107A2F
		public virtual bool Equals(SM2P256V1FieldElement other)
		{
			return this == other || (other != null && Nat256.Eq(this.x, other.x));
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x0010984D File Offset: 0x00107A4D
		public override int GetHashCode()
		{
			return SM2P256V1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 8);
		}

		// Token: 0x04001AD7 RID: 6871
		public static readonly BigInteger Q = SM2P256V1Curve.q;

		// Token: 0x04001AD8 RID: 6872
		protected internal readonly uint[] x;
	}
}
