using System;
using System.Text;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Abc
{
	// Token: 0x020003BF RID: 959
	internal class SimpleBigDecimal
	{
		// Token: 0x06002779 RID: 10105 RVA: 0x0010ADF0 File Offset: 0x00108FF0
		public static SimpleBigDecimal GetInstance(BigInteger val, int scale)
		{
			return new SimpleBigDecimal(val.ShiftLeft(scale), scale);
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x0010ADFF File Offset: 0x00108FFF
		public SimpleBigDecimal(BigInteger bigInt, int scale)
		{
			if (scale < 0)
			{
				throw new ArgumentException("scale may not be negative");
			}
			this.bigInt = bigInt;
			this.scale = scale;
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x0010AE24 File Offset: 0x00109024
		private SimpleBigDecimal(SimpleBigDecimal limBigDec)
		{
			this.bigInt = limBigDec.bigInt;
			this.scale = limBigDec.scale;
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x0010AE44 File Offset: 0x00109044
		private void CheckScale(SimpleBigDecimal b)
		{
			if (this.scale != b.scale)
			{
				throw new ArgumentException("Only SimpleBigDecimal of same scale allowed in arithmetic operations");
			}
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x0010AE5F File Offset: 0x0010905F
		public SimpleBigDecimal AdjustScale(int newScale)
		{
			if (newScale < 0)
			{
				throw new ArgumentException("scale may not be negative");
			}
			if (newScale == this.scale)
			{
				return this;
			}
			return new SimpleBigDecimal(this.bigInt.ShiftLeft(newScale - this.scale), newScale);
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x0010AE94 File Offset: 0x00109094
		public SimpleBigDecimal Add(SimpleBigDecimal b)
		{
			this.CheckScale(b);
			return new SimpleBigDecimal(this.bigInt.Add(b.bigInt), this.scale);
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x0010AEB9 File Offset: 0x001090B9
		public SimpleBigDecimal Add(BigInteger b)
		{
			return new SimpleBigDecimal(this.bigInt.Add(b.ShiftLeft(this.scale)), this.scale);
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x0010AEDD File Offset: 0x001090DD
		public SimpleBigDecimal Negate()
		{
			return new SimpleBigDecimal(this.bigInt.Negate(), this.scale);
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x0010AEF5 File Offset: 0x001090F5
		public SimpleBigDecimal Subtract(SimpleBigDecimal b)
		{
			return this.Add(b.Negate());
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x0010AF03 File Offset: 0x00109103
		public SimpleBigDecimal Subtract(BigInteger b)
		{
			return new SimpleBigDecimal(this.bigInt.Subtract(b.ShiftLeft(this.scale)), this.scale);
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x0010AF27 File Offset: 0x00109127
		public SimpleBigDecimal Multiply(SimpleBigDecimal b)
		{
			this.CheckScale(b);
			return new SimpleBigDecimal(this.bigInt.Multiply(b.bigInt), this.scale + this.scale);
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x0010AF53 File Offset: 0x00109153
		public SimpleBigDecimal Multiply(BigInteger b)
		{
			return new SimpleBigDecimal(this.bigInt.Multiply(b), this.scale);
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x0010AF6C File Offset: 0x0010916C
		public SimpleBigDecimal Divide(SimpleBigDecimal b)
		{
			this.CheckScale(b);
			return new SimpleBigDecimal(this.bigInt.ShiftLeft(this.scale).Divide(b.bigInt), this.scale);
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x0010AF9C File Offset: 0x0010919C
		public SimpleBigDecimal Divide(BigInteger b)
		{
			return new SimpleBigDecimal(this.bigInt.Divide(b), this.scale);
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x0010AFB5 File Offset: 0x001091B5
		public SimpleBigDecimal ShiftLeft(int n)
		{
			return new SimpleBigDecimal(this.bigInt.ShiftLeft(n), this.scale);
		}

		// Token: 0x06002788 RID: 10120 RVA: 0x0010AFCE File Offset: 0x001091CE
		public int CompareTo(SimpleBigDecimal val)
		{
			this.CheckScale(val);
			return this.bigInt.CompareTo(val.bigInt);
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x0010AFE8 File Offset: 0x001091E8
		public int CompareTo(BigInteger val)
		{
			return this.bigInt.CompareTo(val.ShiftLeft(this.scale));
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x0010B001 File Offset: 0x00109201
		public BigInteger Floor()
		{
			return this.bigInt.ShiftRight(this.scale);
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x0010B014 File Offset: 0x00109214
		public BigInteger Round()
		{
			SimpleBigDecimal simpleBigDecimal = new SimpleBigDecimal(BigInteger.One, 1);
			return this.Add(simpleBigDecimal.AdjustScale(this.scale)).Floor();
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x0600278C RID: 10124 RVA: 0x0010B044 File Offset: 0x00109244
		public int IntValue
		{
			get
			{
				return this.Floor().IntValue;
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x0600278D RID: 10125 RVA: 0x0010B051 File Offset: 0x00109251
		public long LongValue
		{
			get
			{
				return this.Floor().LongValue;
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x0600278E RID: 10126 RVA: 0x0010B05E File Offset: 0x0010925E
		public int Scale
		{
			get
			{
				return this.scale;
			}
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x0010B068 File Offset: 0x00109268
		public override string ToString()
		{
			if (this.scale == 0)
			{
				return this.bigInt.ToString();
			}
			BigInteger bigInteger = this.Floor();
			BigInteger bigInteger2 = this.bigInt.Subtract(bigInteger.ShiftLeft(this.scale));
			if (this.bigInt.SignValue < 0)
			{
				bigInteger2 = BigInteger.One.ShiftLeft(this.scale).Subtract(bigInteger2);
			}
			if (bigInteger.SignValue == -1 && !bigInteger2.Equals(BigInteger.Zero))
			{
				bigInteger = bigInteger.Add(BigInteger.One);
			}
			string value = bigInteger.ToString();
			char[] array = new char[this.scale];
			string text = bigInteger2.ToString(2);
			int length = text.Length;
			int num = this.scale - length;
			for (int i = 0; i < num; i++)
			{
				array[i] = '0';
			}
			for (int j = 0; j < length; j++)
			{
				array[num + j] = text[j];
			}
			string value2 = new string(array);
			StringBuilder stringBuilder = new StringBuilder(value);
			stringBuilder.Append(".");
			stringBuilder.Append(value2);
			return stringBuilder.ToString();
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x0010B180 File Offset: 0x00109380
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			SimpleBigDecimal simpleBigDecimal = obj as SimpleBigDecimal;
			return simpleBigDecimal != null && this.bigInt.Equals(simpleBigDecimal.bigInt) && this.scale == simpleBigDecimal.scale;
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x0010B1C2 File Offset: 0x001093C2
		public override int GetHashCode()
		{
			return this.bigInt.GetHashCode() ^ this.scale;
		}

		// Token: 0x04001AE4 RID: 6884
		private readonly BigInteger bigInt;

		// Token: 0x04001AE5 RID: 6885
		private readonly int scale;
	}
}
