using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x02000326 RID: 806
	public class F2mFieldElement : AbstractF2mFieldElement
	{
		// Token: 0x06001EAD RID: 7853 RVA: 0x000E3864 File Offset: 0x000E1A64
		[Obsolete("Use ECCurve.FromBigInteger to construct field elements")]
		public F2mFieldElement(int m, int k1, int k2, int k3, BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > m)
			{
				throw new ArgumentException("value invalid in F2m field element", "x");
			}
			if (k2 == 0 && k3 == 0)
			{
				this.representation = 2;
				this.ks = new int[]
				{
					k1
				};
			}
			else
			{
				if (k2 >= k3)
				{
					throw new ArgumentException("k2 must be smaller than k3");
				}
				if (k2 <= 0)
				{
					throw new ArgumentException("k2 must be larger than 0");
				}
				this.representation = 3;
				this.ks = new int[]
				{
					k1,
					k2,
					k3
				};
			}
			this.m = m;
			this.x = new LongArray(x);
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x000E3912 File Offset: 0x000E1B12
		[Obsolete("Use ECCurve.FromBigInteger to construct field elements")]
		public F2mFieldElement(int m, int k, BigInteger x) : this(m, k, 0, 0, x)
		{
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x000E391F File Offset: 0x000E1B1F
		internal F2mFieldElement(int m, int[] ks, LongArray x)
		{
			this.m = m;
			this.representation = ((ks.Length == 1) ? 2 : 3);
			this.ks = ks;
			this.x = x;
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001EB0 RID: 7856 RVA: 0x000E394C File Offset: 0x000E1B4C
		public override int BitLength
		{
			get
			{
				return this.x.Degree();
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06001EB1 RID: 7857 RVA: 0x000E3959 File Offset: 0x000E1B59
		public override bool IsOne
		{
			get
			{
				return this.x.IsOne();
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001EB2 RID: 7858 RVA: 0x000E3966 File Offset: 0x000E1B66
		public override bool IsZero
		{
			get
			{
				return this.x.IsZero();
			}
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x000E3973 File Offset: 0x000E1B73
		public override bool TestBitZero()
		{
			return this.x.TestBitZero();
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x000E3980 File Offset: 0x000E1B80
		public override BigInteger ToBigInteger()
		{
			return this.x.ToBigInteger();
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x000E398D File Offset: 0x000E1B8D
		public override string FieldName
		{
			get
			{
				return "F2m";
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x000E3994 File Offset: 0x000E1B94
		public override int FieldSize
		{
			get
			{
				return this.m;
			}
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x000E399C File Offset: 0x000E1B9C
		public static void CheckFieldElements(ECFieldElement a, ECFieldElement b)
		{
			if (!(a is F2mFieldElement) || !(b is F2mFieldElement))
			{
				throw new ArgumentException("Field elements are not both instances of F2mFieldElement");
			}
			F2mFieldElement f2mFieldElement = (F2mFieldElement)a;
			F2mFieldElement f2mFieldElement2 = (F2mFieldElement)b;
			if (f2mFieldElement.representation != f2mFieldElement2.representation)
			{
				throw new ArgumentException("One of the F2m field elements has incorrect representation");
			}
			if (f2mFieldElement.m != f2mFieldElement2.m || !Arrays.AreEqual(f2mFieldElement.ks, f2mFieldElement2.ks))
			{
				throw new ArgumentException("Field elements are not elements of the same field F2m");
			}
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x000E3A18 File Offset: 0x000E1C18
		public override ECFieldElement Add(ECFieldElement b)
		{
			LongArray longArray = this.x.Copy();
			F2mFieldElement f2mFieldElement = (F2mFieldElement)b;
			longArray.AddShiftedByWords(f2mFieldElement.x, 0);
			return new F2mFieldElement(this.m, this.ks, longArray);
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x000E3A57 File Offset: 0x000E1C57
		public override ECFieldElement AddOne()
		{
			return new F2mFieldElement(this.m, this.ks, this.x.AddOne());
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x000E3A75 File Offset: 0x000E1C75
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x000E3A7E File Offset: 0x000E1C7E
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			return new F2mFieldElement(this.m, this.ks, this.x.ModMultiply(((F2mFieldElement)b).x, this.m, this.ks));
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x000E3AB3 File Offset: 0x000E1CB3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x000E3AC0 File Offset: 0x000E1CC0
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			LongArray longArray = this.x;
			LongArray longArray2 = ((F2mFieldElement)b).x;
			LongArray longArray3 = ((F2mFieldElement)x).x;
			LongArray other = ((F2mFieldElement)y).x;
			LongArray longArray4 = longArray.Multiply(longArray2, this.m, this.ks);
			LongArray other2 = longArray3.Multiply(other, this.m, this.ks);
			if (longArray4 == longArray || longArray4 == longArray2)
			{
				longArray4 = longArray4.Copy();
			}
			longArray4.AddShiftedByWords(other2, 0);
			longArray4.Reduce(this.m, this.ks);
			return new F2mFieldElement(this.m, this.ks, longArray4);
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x000E3B5C File Offset: 0x000E1D5C
		public override ECFieldElement Divide(ECFieldElement b)
		{
			ECFieldElement b2 = b.Invert();
			return this.Multiply(b2);
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x000947CE File Offset: 0x000929CE
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x000E3B77 File Offset: 0x000E1D77
		public override ECFieldElement Square()
		{
			return new F2mFieldElement(this.m, this.ks, this.x.ModSquare(this.m, this.ks));
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x000E3BA1 File Offset: 0x000E1DA1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x000E3BAC File Offset: 0x000E1DAC
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			LongArray longArray = this.x;
			LongArray longArray2 = ((F2mFieldElement)x).x;
			LongArray other = ((F2mFieldElement)y).x;
			LongArray longArray3 = longArray.Square(this.m, this.ks);
			LongArray other2 = longArray2.Multiply(other, this.m, this.ks);
			if (longArray3 == longArray)
			{
				longArray3 = longArray3.Copy();
			}
			longArray3.AddShiftedByWords(other2, 0);
			longArray3.Reduce(this.m, this.ks);
			return new F2mFieldElement(this.m, this.ks, longArray3);
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x000E3C34 File Offset: 0x000E1E34
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow >= 1)
			{
				return new F2mFieldElement(this.m, this.ks, this.x.ModSquareN(pow, this.m, this.ks));
			}
			return this;
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x000E3C65 File Offset: 0x000E1E65
		public override ECFieldElement Invert()
		{
			return new F2mFieldElement(this.m, this.ks, this.x.ModInverse(this.m, this.ks));
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x000E3C8F File Offset: 0x000E1E8F
		public override ECFieldElement Sqrt()
		{
			if (!this.x.IsZero() && !this.x.IsOne())
			{
				return this.SquarePow(this.m - 1);
			}
			return this;
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001EC6 RID: 7878 RVA: 0x000E3CBB File Offset: 0x000E1EBB
		public int Representation
		{
			get
			{
				return this.representation;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001EC7 RID: 7879 RVA: 0x000E3994 File Offset: 0x000E1B94
		public int M
		{
			get
			{
				return this.m;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001EC8 RID: 7880 RVA: 0x000E3CC3 File Offset: 0x000E1EC3
		public int K1
		{
			get
			{
				return this.ks[0];
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001EC9 RID: 7881 RVA: 0x000E3CCD File Offset: 0x000E1ECD
		public int K2
		{
			get
			{
				if (this.ks.Length < 2)
				{
					return 0;
				}
				return this.ks[1];
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001ECA RID: 7882 RVA: 0x000E3CE4 File Offset: 0x000E1EE4
		public int K3
		{
			get
			{
				if (this.ks.Length < 3)
				{
					return 0;
				}
				return this.ks[2];
			}
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x000E3CFC File Offset: 0x000E1EFC
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			F2mFieldElement f2mFieldElement = obj as F2mFieldElement;
			return f2mFieldElement != null && this.Equals(f2mFieldElement);
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x000E3D24 File Offset: 0x000E1F24
		public virtual bool Equals(F2mFieldElement other)
		{
			return this.m == other.m && this.representation == other.representation && Arrays.AreEqual(this.ks, other.ks) && this.x.Equals(other.x);
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x000E3D73 File Offset: 0x000E1F73
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.m ^ Arrays.GetHashCode(this.ks);
		}

		// Token: 0x0400196B RID: 6507
		public const int Gnb = 1;

		// Token: 0x0400196C RID: 6508
		public const int Tpb = 2;

		// Token: 0x0400196D RID: 6509
		public const int Ppb = 3;

		// Token: 0x0400196E RID: 6510
		private int representation;

		// Token: 0x0400196F RID: 6511
		private int m;

		// Token: 0x04001970 RID: 6512
		private int[] ks;

		// Token: 0x04001971 RID: 6513
		internal LongArray x;
	}
}
