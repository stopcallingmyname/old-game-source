using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Djb
{
	// Token: 0x020003BD RID: 957
	internal class Curve25519FieldElement : AbstractFpFieldElement
	{
		// Token: 0x06002755 RID: 10069 RVA: 0x0010A35A File Offset: 0x0010855A
		public Curve25519FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(Curve25519FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for Curve25519FieldElement", "x");
			}
			this.x = Curve25519Field.FromBigInteger(x);
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x0010A398 File Offset: 0x00108598
		public Curve25519FieldElement()
		{
			this.x = Nat256.Create();
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x0010A3AB File Offset: 0x001085AB
		protected internal Curve25519FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06002758 RID: 10072 RVA: 0x0010A3BA File Offset: 0x001085BA
		public override bool IsZero
		{
			get
			{
				return Nat256.IsZero(this.x);
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06002759 RID: 10073 RVA: 0x0010A3C7 File Offset: 0x001085C7
		public override bool IsOne
		{
			get
			{
				return Nat256.IsOne(this.x);
			}
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x0010A3D4 File Offset: 0x001085D4
		public override bool TestBitZero()
		{
			return Nat256.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x0010A3E5 File Offset: 0x001085E5
		public override BigInteger ToBigInteger()
		{
			return Nat256.ToBigInteger(this.x);
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x0600275C RID: 10076 RVA: 0x0010A3F2 File Offset: 0x001085F2
		public override string FieldName
		{
			get
			{
				return "Curve25519Field";
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x0600275D RID: 10077 RVA: 0x0010A3F9 File Offset: 0x001085F9
		public override int FieldSize
		{
			get
			{
				return Curve25519FieldElement.Q.BitLength;
			}
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x0010A408 File Offset: 0x00108608
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			Curve25519Field.Add(this.x, ((Curve25519FieldElement)b).x, z);
			return new Curve25519FieldElement(z);
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x0010A438 File Offset: 0x00108638
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat256.Create();
			Curve25519Field.AddOne(this.x, z);
			return new Curve25519FieldElement(z);
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x0010A460 File Offset: 0x00108660
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			Curve25519Field.Subtract(this.x, ((Curve25519FieldElement)b).x, z);
			return new Curve25519FieldElement(z);
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x0010A490 File Offset: 0x00108690
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			Curve25519Field.Multiply(this.x, ((Curve25519FieldElement)b).x, z);
			return new Curve25519FieldElement(z);
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x0010A4C0 File Offset: 0x001086C0
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			Mod.Invert(Curve25519Field.P, ((Curve25519FieldElement)b).x, z);
			Curve25519Field.Multiply(z, this.x, z);
			return new Curve25519FieldElement(z);
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x0010A4FC File Offset: 0x001086FC
		public override ECFieldElement Negate()
		{
			uint[] z = Nat256.Create();
			Curve25519Field.Negate(this.x, z);
			return new Curve25519FieldElement(z);
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x0010A524 File Offset: 0x00108724
		public override ECFieldElement Square()
		{
			uint[] z = Nat256.Create();
			Curve25519Field.Square(this.x, z);
			return new Curve25519FieldElement(z);
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x0010A54C File Offset: 0x0010874C
		public override ECFieldElement Invert()
		{
			uint[] z = Nat256.Create();
			Mod.Invert(Curve25519Field.P, this.x, z);
			return new Curve25519FieldElement(z);
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x0010A578 File Offset: 0x00108778
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat256.IsZero(y) || Nat256.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat256.Create();
			Curve25519Field.Square(y, array);
			Curve25519Field.Multiply(array, y, array);
			uint[] array2 = array;
			Curve25519Field.Square(array, array2);
			Curve25519Field.Multiply(array2, y, array2);
			uint[] array3 = Nat256.Create();
			Curve25519Field.Square(array2, array3);
			Curve25519Field.Multiply(array3, y, array3);
			uint[] array4 = Nat256.Create();
			Curve25519Field.SquareN(array3, 3, array4);
			Curve25519Field.Multiply(array4, array2, array4);
			uint[] array5 = array2;
			Curve25519Field.SquareN(array4, 4, array5);
			Curve25519Field.Multiply(array5, array3, array5);
			uint[] array6 = array4;
			Curve25519Field.SquareN(array5, 4, array6);
			Curve25519Field.Multiply(array6, array3, array6);
			uint[] array7 = array3;
			Curve25519Field.SquareN(array6, 15, array7);
			Curve25519Field.Multiply(array7, array6, array7);
			uint[] array8 = array6;
			Curve25519Field.SquareN(array7, 30, array8);
			Curve25519Field.Multiply(array8, array7, array8);
			uint[] array9 = array7;
			Curve25519Field.SquareN(array8, 60, array9);
			Curve25519Field.Multiply(array9, array8, array9);
			uint[] z = array8;
			Curve25519Field.SquareN(array9, 11, z);
			Curve25519Field.Multiply(z, array5, z);
			uint[] array10 = array5;
			Curve25519Field.SquareN(z, 120, array10);
			Curve25519Field.Multiply(array10, array9, array10);
			uint[] z2 = array10;
			Curve25519Field.Square(z2, z2);
			uint[] array11 = array9;
			Curve25519Field.Square(z2, array11);
			if (Nat256.Eq(y, array11))
			{
				return new Curve25519FieldElement(z2);
			}
			Curve25519Field.Multiply(z2, Curve25519FieldElement.PRECOMP_POW2, z2);
			Curve25519Field.Square(z2, array11);
			if (Nat256.Eq(y, array11))
			{
				return new Curve25519FieldElement(z2);
			}
			return null;
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x0010A6F9 File Offset: 0x001088F9
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Curve25519FieldElement);
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x0010A6F9 File Offset: 0x001088F9
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as Curve25519FieldElement);
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x0010A707 File Offset: 0x00108907
		public virtual bool Equals(Curve25519FieldElement other)
		{
			return this == other || (other != null && Nat256.Eq(this.x, other.x));
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x0010A725 File Offset: 0x00108925
		public override int GetHashCode()
		{
			return Curve25519FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 8);
		}

		// Token: 0x04001AE1 RID: 6881
		public static readonly BigInteger Q = Curve25519.q;

		// Token: 0x04001AE2 RID: 6882
		private static readonly uint[] PRECOMP_POW2 = new uint[]
		{
			1242472624U,
			3303938855U,
			2905597048U,
			792926214U,
			1039914919U,
			726466713U,
			1338105611U,
			730014848U
		};

		// Token: 0x04001AE3 RID: 6883
		protected internal readonly uint[] x;
	}
}
