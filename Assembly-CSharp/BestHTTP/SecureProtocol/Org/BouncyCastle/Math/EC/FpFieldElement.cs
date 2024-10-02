using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x02000324 RID: 804
	public class FpFieldElement : AbstractFpFieldElement
	{
		// Token: 0x06001E89 RID: 7817 RVA: 0x000E2CBC File Offset: 0x000E0EBC
		internal static BigInteger CalculateResidue(BigInteger p)
		{
			int bitLength = p.BitLength;
			if (bitLength >= 96)
			{
				if (p.ShiftRight(bitLength - 64).LongValue == -1L)
				{
					return BigInteger.One.ShiftLeft(bitLength).Subtract(p);
				}
				if ((bitLength & 7) == 0)
				{
					return BigInteger.One.ShiftLeft(bitLength << 1).Divide(p).Negate();
				}
			}
			return null;
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x000E2D19 File Offset: 0x000E0F19
		[Obsolete("Use ECCurve.FromBigInteger to construct field elements")]
		public FpFieldElement(BigInteger q, BigInteger x) : this(q, FpFieldElement.CalculateResidue(q), x)
		{
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x000E2D2C File Offset: 0x000E0F2C
		internal FpFieldElement(BigInteger q, BigInteger r, BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(q) >= 0)
			{
				throw new ArgumentException("value invalid in Fp field element", "x");
			}
			this.q = q;
			this.r = r;
			this.x = x;
		}

		// Token: 0x06001E8C RID: 7820 RVA: 0x000E2D7A File Offset: 0x000E0F7A
		public override BigInteger ToBigInteger()
		{
			return this.x;
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x000E2D82 File Offset: 0x000E0F82
		public override string FieldName
		{
			get
			{
				return "Fp";
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06001E8E RID: 7822 RVA: 0x000E2D89 File Offset: 0x000E0F89
		public override int FieldSize
		{
			get
			{
				return this.q.BitLength;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001E8F RID: 7823 RVA: 0x000E2D96 File Offset: 0x000E0F96
		public BigInteger Q
		{
			get
			{
				return this.q;
			}
		}

		// Token: 0x06001E90 RID: 7824 RVA: 0x000E2D9E File Offset: 0x000E0F9E
		public override ECFieldElement Add(ECFieldElement b)
		{
			return new FpFieldElement(this.q, this.r, this.ModAdd(this.x, b.ToBigInteger()));
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x000E2DC4 File Offset: 0x000E0FC4
		public override ECFieldElement AddOne()
		{
			BigInteger bigInteger = this.x.Add(BigInteger.One);
			if (bigInteger.CompareTo(this.q) == 0)
			{
				bigInteger = BigInteger.Zero;
			}
			return new FpFieldElement(this.q, this.r, bigInteger);
		}

		// Token: 0x06001E92 RID: 7826 RVA: 0x000E2E08 File Offset: 0x000E1008
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return new FpFieldElement(this.q, this.r, this.ModSubtract(this.x, b.ToBigInteger()));
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x000E2E2D File Offset: 0x000E102D
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			return new FpFieldElement(this.q, this.r, this.ModMult(this.x, b.ToBigInteger()));
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x000E2E54 File Offset: 0x000E1054
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			BigInteger bigInteger = this.x;
			BigInteger val = b.ToBigInteger();
			BigInteger bigInteger2 = x.ToBigInteger();
			BigInteger val2 = y.ToBigInteger();
			BigInteger bigInteger3 = bigInteger.Multiply(val);
			BigInteger n = bigInteger2.Multiply(val2);
			return new FpFieldElement(this.q, this.r, this.ModReduce(bigInteger3.Subtract(n)));
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x000E2EAC File Offset: 0x000E10AC
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			BigInteger bigInteger = this.x;
			BigInteger val = b.ToBigInteger();
			BigInteger bigInteger2 = x.ToBigInteger();
			BigInteger val2 = y.ToBigInteger();
			BigInteger bigInteger3 = bigInteger.Multiply(val);
			BigInteger value = bigInteger2.Multiply(val2);
			BigInteger bigInteger4 = bigInteger3.Add(value);
			if (this.r != null && this.r.SignValue < 0 && bigInteger4.BitLength > this.q.BitLength << 1)
			{
				bigInteger4 = bigInteger4.Subtract(this.q.ShiftLeft(this.q.BitLength));
			}
			return new FpFieldElement(this.q, this.r, this.ModReduce(bigInteger4));
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x000E2F4E File Offset: 0x000E114E
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return new FpFieldElement(this.q, this.r, this.ModMult(this.x, this.ModInverse(b.ToBigInteger())));
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x000E2F79 File Offset: 0x000E1179
		public override ECFieldElement Negate()
		{
			if (this.x.SignValue != 0)
			{
				return new FpFieldElement(this.q, this.r, this.q.Subtract(this.x));
			}
			return this;
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x000E2FAC File Offset: 0x000E11AC
		public override ECFieldElement Square()
		{
			return new FpFieldElement(this.q, this.r, this.ModMult(this.x, this.x));
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x000E2FD4 File Offset: 0x000E11D4
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			BigInteger bigInteger = this.x;
			BigInteger bigInteger2 = x.ToBigInteger();
			BigInteger val = y.ToBigInteger();
			BigInteger bigInteger3 = bigInteger.Multiply(bigInteger);
			BigInteger n = bigInteger2.Multiply(val);
			return new FpFieldElement(this.q, this.r, this.ModReduce(bigInteger3.Subtract(n)));
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x000E3024 File Offset: 0x000E1224
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			BigInteger bigInteger = this.x;
			BigInteger bigInteger2 = x.ToBigInteger();
			BigInteger val = y.ToBigInteger();
			BigInteger bigInteger3 = bigInteger.Multiply(bigInteger);
			BigInteger value = bigInteger2.Multiply(val);
			BigInteger bigInteger4 = bigInteger3.Add(value);
			if (this.r != null && this.r.SignValue < 0 && bigInteger4.BitLength > this.q.BitLength << 1)
			{
				bigInteger4 = bigInteger4.Subtract(this.q.ShiftLeft(this.q.BitLength));
			}
			return new FpFieldElement(this.q, this.r, this.ModReduce(bigInteger4));
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x000E30BA File Offset: 0x000E12BA
		public override ECFieldElement Invert()
		{
			return new FpFieldElement(this.q, this.r, this.ModInverse(this.x));
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x000E30DC File Offset: 0x000E12DC
		public override ECFieldElement Sqrt()
		{
			if (this.IsZero || this.IsOne)
			{
				return this;
			}
			if (!this.q.TestBit(0))
			{
				throw Platform.CreateNotImplementedException("even value of q");
			}
			if (this.q.TestBit(1))
			{
				BigInteger e = this.q.ShiftRight(2).Add(BigInteger.One);
				return this.CheckSqrt(new FpFieldElement(this.q, this.r, this.x.ModPow(e, this.q)));
			}
			if (this.q.TestBit(2))
			{
				BigInteger bigInteger = this.x.ModPow(this.q.ShiftRight(3), this.q);
				BigInteger x = this.ModMult(bigInteger, this.x);
				if (this.ModMult(x, bigInteger).Equals(BigInteger.One))
				{
					return this.CheckSqrt(new FpFieldElement(this.q, this.r, x));
				}
				BigInteger x2 = BigInteger.Two.ModPow(this.q.ShiftRight(2), this.q);
				BigInteger bigInteger2 = this.ModMult(x, x2);
				return this.CheckSqrt(new FpFieldElement(this.q, this.r, bigInteger2));
			}
			else
			{
				BigInteger bigInteger3 = this.q.ShiftRight(1);
				if (!this.x.ModPow(bigInteger3, this.q).Equals(BigInteger.One))
				{
					return null;
				}
				BigInteger bigInteger4 = this.x;
				BigInteger bigInteger5 = this.ModDouble(this.ModDouble(bigInteger4));
				BigInteger k = bigInteger3.Add(BigInteger.One);
				BigInteger obj = this.q.Subtract(BigInteger.One);
				BigInteger bigInteger8;
				for (;;)
				{
					BigInteger bigInteger6 = BigInteger.Arbitrary(this.q.BitLength);
					if (bigInteger6.CompareTo(this.q) < 0 && this.ModReduce(bigInteger6.Multiply(bigInteger6).Subtract(bigInteger5)).ModPow(bigInteger3, this.q).Equals(obj))
					{
						BigInteger[] array = this.LucasSequence(bigInteger6, bigInteger4, k);
						BigInteger bigInteger7 = array[0];
						bigInteger8 = array[1];
						if (this.ModMult(bigInteger8, bigInteger8).Equals(bigInteger5))
						{
							break;
						}
						if (!bigInteger7.Equals(BigInteger.One) && !bigInteger7.Equals(obj))
						{
							goto Block_11;
						}
					}
				}
				return new FpFieldElement(this.q, this.r, this.ModHalfAbs(bigInteger8));
				Block_11:
				return null;
			}
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x000E3324 File Offset: 0x000E1524
		private ECFieldElement CheckSqrt(ECFieldElement z)
		{
			if (!z.Square().Equals(this))
			{
				return null;
			}
			return z;
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x000E3338 File Offset: 0x000E1538
		private BigInteger[] LucasSequence(BigInteger P, BigInteger Q, BigInteger k)
		{
			int bitLength = k.BitLength;
			int lowestSetBit = k.GetLowestSetBit();
			BigInteger bigInteger = BigInteger.One;
			BigInteger bigInteger2 = BigInteger.Two;
			BigInteger bigInteger3 = P;
			BigInteger bigInteger4 = BigInteger.One;
			BigInteger bigInteger5 = BigInteger.One;
			for (int i = bitLength - 1; i >= lowestSetBit + 1; i--)
			{
				bigInteger4 = this.ModMult(bigInteger4, bigInteger5);
				if (k.TestBit(i))
				{
					bigInteger5 = this.ModMult(bigInteger4, Q);
					bigInteger = this.ModMult(bigInteger, bigInteger3);
					bigInteger2 = this.ModReduce(bigInteger3.Multiply(bigInteger2).Subtract(P.Multiply(bigInteger4)));
					bigInteger3 = this.ModReduce(bigInteger3.Multiply(bigInteger3).Subtract(bigInteger5.ShiftLeft(1)));
				}
				else
				{
					bigInteger5 = bigInteger4;
					bigInteger = this.ModReduce(bigInteger.Multiply(bigInteger2).Subtract(bigInteger4));
					bigInteger3 = this.ModReduce(bigInteger3.Multiply(bigInteger2).Subtract(P.Multiply(bigInteger4)));
					bigInteger2 = this.ModReduce(bigInteger2.Multiply(bigInteger2).Subtract(bigInteger4.ShiftLeft(1)));
				}
			}
			bigInteger4 = this.ModMult(bigInteger4, bigInteger5);
			bigInteger5 = this.ModMult(bigInteger4, Q);
			bigInteger = this.ModReduce(bigInteger.Multiply(bigInteger2).Subtract(bigInteger4));
			bigInteger2 = this.ModReduce(bigInteger3.Multiply(bigInteger2).Subtract(P.Multiply(bigInteger4)));
			bigInteger4 = this.ModMult(bigInteger4, bigInteger5);
			for (int j = 1; j <= lowestSetBit; j++)
			{
				bigInteger = this.ModMult(bigInteger, bigInteger2);
				bigInteger2 = this.ModReduce(bigInteger2.Multiply(bigInteger2).Subtract(bigInteger4.ShiftLeft(1)));
				bigInteger4 = this.ModMult(bigInteger4, bigInteger4);
			}
			return new BigInteger[]
			{
				bigInteger,
				bigInteger2
			};
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x000E34DC File Offset: 0x000E16DC
		protected virtual BigInteger ModAdd(BigInteger x1, BigInteger x2)
		{
			BigInteger bigInteger = x1.Add(x2);
			if (bigInteger.CompareTo(this.q) >= 0)
			{
				bigInteger = bigInteger.Subtract(this.q);
			}
			return bigInteger;
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x000E3510 File Offset: 0x000E1710
		protected virtual BigInteger ModDouble(BigInteger x)
		{
			BigInteger bigInteger = x.ShiftLeft(1);
			if (bigInteger.CompareTo(this.q) >= 0)
			{
				bigInteger = bigInteger.Subtract(this.q);
			}
			return bigInteger;
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x000E3542 File Offset: 0x000E1742
		protected virtual BigInteger ModHalf(BigInteger x)
		{
			if (x.TestBit(0))
			{
				x = this.q.Add(x);
			}
			return x.ShiftRight(1);
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x000E3562 File Offset: 0x000E1762
		protected virtual BigInteger ModHalfAbs(BigInteger x)
		{
			if (x.TestBit(0))
			{
				x = this.q.Subtract(x);
			}
			return x.ShiftRight(1);
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x000E3584 File Offset: 0x000E1784
		protected virtual BigInteger ModInverse(BigInteger x)
		{
			int fieldSize = this.FieldSize;
			int len = fieldSize + 31 >> 5;
			uint[] p = Nat.FromBigInteger(fieldSize, this.q);
			uint[] array = Nat.FromBigInteger(fieldSize, x);
			uint[] z = Nat.Create(len);
			Mod.Invert(p, array, z);
			return Nat.ToBigInteger(len, z);
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x000E35C8 File Offset: 0x000E17C8
		protected virtual BigInteger ModMult(BigInteger x1, BigInteger x2)
		{
			return this.ModReduce(x1.Multiply(x2));
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x000E35D8 File Offset: 0x000E17D8
		protected virtual BigInteger ModReduce(BigInteger x)
		{
			if (this.r == null)
			{
				x = x.Mod(this.q);
			}
			else
			{
				bool flag = x.SignValue < 0;
				if (flag)
				{
					x = x.Abs();
				}
				int bitLength = this.q.BitLength;
				if (this.r.SignValue > 0)
				{
					BigInteger n = BigInteger.One.ShiftLeft(bitLength);
					bool flag2 = this.r.Equals(BigInteger.One);
					while (x.BitLength > bitLength + 1)
					{
						BigInteger bigInteger = x.ShiftRight(bitLength);
						BigInteger value = x.Remainder(n);
						if (!flag2)
						{
							bigInteger = bigInteger.Multiply(this.r);
						}
						x = bigInteger.Add(value);
					}
				}
				else
				{
					int num = (bitLength - 1 & 31) + 1;
					BigInteger bigInteger2 = this.r.Negate().Multiply(x.ShiftRight(bitLength - num)).ShiftRight(bitLength + num).Multiply(this.q);
					BigInteger bigInteger3 = BigInteger.One.ShiftLeft(bitLength + num);
					bigInteger2 = bigInteger2.Remainder(bigInteger3);
					x = x.Remainder(bigInteger3);
					x = x.Subtract(bigInteger2);
					if (x.SignValue < 0)
					{
						x = x.Add(bigInteger3);
					}
				}
				while (x.CompareTo(this.q) >= 0)
				{
					x = x.Subtract(this.q);
				}
				if (flag && x.SignValue != 0)
				{
					x = this.q.Subtract(x);
				}
			}
			return x;
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x000E3744 File Offset: 0x000E1944
		protected virtual BigInteger ModSubtract(BigInteger x1, BigInteger x2)
		{
			BigInteger bigInteger = x1.Subtract(x2);
			if (bigInteger.SignValue < 0)
			{
				bigInteger = bigInteger.Add(this.q);
			}
			return bigInteger;
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x000E3770 File Offset: 0x000E1970
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			FpFieldElement fpFieldElement = obj as FpFieldElement;
			return fpFieldElement != null && this.Equals(fpFieldElement);
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x000E3796 File Offset: 0x000E1996
		public virtual bool Equals(FpFieldElement other)
		{
			return this.q.Equals(other.q) && base.Equals(other);
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x000E37B4 File Offset: 0x000E19B4
		public override int GetHashCode()
		{
			return this.q.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001968 RID: 6504
		private readonly BigInteger q;

		// Token: 0x04001969 RID: 6505
		private readonly BigInteger r;

		// Token: 0x0400196A RID: 6506
		private readonly BigInteger x;
	}
}
