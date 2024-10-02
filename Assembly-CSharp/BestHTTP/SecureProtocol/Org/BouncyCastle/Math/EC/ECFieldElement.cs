using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x02000322 RID: 802
	public abstract class ECFieldElement
	{
		// Token: 0x06001E6D RID: 7789
		public abstract BigInteger ToBigInteger();

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001E6E RID: 7790
		public abstract string FieldName { get; }

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06001E6F RID: 7791
		public abstract int FieldSize { get; }

		// Token: 0x06001E70 RID: 7792
		public abstract ECFieldElement Add(ECFieldElement b);

		// Token: 0x06001E71 RID: 7793
		public abstract ECFieldElement AddOne();

		// Token: 0x06001E72 RID: 7794
		public abstract ECFieldElement Subtract(ECFieldElement b);

		// Token: 0x06001E73 RID: 7795
		public abstract ECFieldElement Multiply(ECFieldElement b);

		// Token: 0x06001E74 RID: 7796
		public abstract ECFieldElement Divide(ECFieldElement b);

		// Token: 0x06001E75 RID: 7797
		public abstract ECFieldElement Negate();

		// Token: 0x06001E76 RID: 7798
		public abstract ECFieldElement Square();

		// Token: 0x06001E77 RID: 7799
		public abstract ECFieldElement Invert();

		// Token: 0x06001E78 RID: 7800
		public abstract ECFieldElement Sqrt();

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06001E79 RID: 7801 RVA: 0x000E2BA9 File Offset: 0x000E0DA9
		public virtual int BitLength
		{
			get
			{
				return this.ToBigInteger().BitLength;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06001E7A RID: 7802 RVA: 0x000E2BB6 File Offset: 0x000E0DB6
		public virtual bool IsOne
		{
			get
			{
				return this.BitLength == 1;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001E7B RID: 7803 RVA: 0x000E2BC1 File Offset: 0x000E0DC1
		public virtual bool IsZero
		{
			get
			{
				return this.ToBigInteger().SignValue == 0;
			}
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x000E2BD1 File Offset: 0x000E0DD1
		public virtual ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.Multiply(b).Subtract(x.Multiply(y));
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x000E2BE6 File Offset: 0x000E0DE6
		public virtual ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.Multiply(b).Add(x.Multiply(y));
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x000E2BFB File Offset: 0x000E0DFB
		public virtual ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.Square().Subtract(x.Multiply(y));
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x000E2C0F File Offset: 0x000E0E0F
		public virtual ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.Square().Add(x.Multiply(y));
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x000E2C24 File Offset: 0x000E0E24
		public virtual ECFieldElement SquarePow(int pow)
		{
			ECFieldElement ecfieldElement = this;
			for (int i = 0; i < pow; i++)
			{
				ecfieldElement = ecfieldElement.Square();
			}
			return ecfieldElement;
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x000E2C47 File Offset: 0x000E0E47
		public virtual bool TestBitZero()
		{
			return this.ToBigInteger().TestBit(0);
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x000E2C55 File Offset: 0x000E0E55
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ECFieldElement);
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x000E2C63 File Offset: 0x000E0E63
		public virtual bool Equals(ECFieldElement other)
		{
			return this == other || (other != null && this.ToBigInteger().Equals(other.ToBigInteger()));
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x000E2C81 File Offset: 0x000E0E81
		public override int GetHashCode()
		{
			return this.ToBigInteger().GetHashCode();
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x000E2C8E File Offset: 0x000E0E8E
		public override string ToString()
		{
			return this.ToBigInteger().ToString(16);
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x000E2C9D File Offset: 0x000E0E9D
		public virtual byte[] GetEncoded()
		{
			return BigIntegers.AsUnsignedByteArray((this.FieldSize + 7) / 8, this.ToBigInteger());
		}
	}
}
