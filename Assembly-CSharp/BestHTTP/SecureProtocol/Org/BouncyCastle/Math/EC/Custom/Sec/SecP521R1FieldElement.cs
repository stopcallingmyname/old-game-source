using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200037F RID: 895
	internal class SecP521R1FieldElement : AbstractFpFieldElement
	{
		// Token: 0x06002331 RID: 9009 RVA: 0x000FA599 File Offset: 0x000F8799
		public SecP521R1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP521R1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP521R1FieldElement", "x");
			}
			this.x = SecP521R1Field.FromBigInteger(x);
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x000FA5D7 File Offset: 0x000F87D7
		public SecP521R1FieldElement()
		{
			this.x = Nat.Create(17);
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x000FA5EC File Offset: 0x000F87EC
		protected internal SecP521R1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06002334 RID: 9012 RVA: 0x000FA5FB File Offset: 0x000F87FB
		public override bool IsZero
		{
			get
			{
				return Nat.IsZero(17, this.x);
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06002335 RID: 9013 RVA: 0x000FA60A File Offset: 0x000F880A
		public override bool IsOne
		{
			get
			{
				return Nat.IsOne(17, this.x);
			}
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x000FA619 File Offset: 0x000F8819
		public override bool TestBitZero()
		{
			return Nat.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x000FA62A File Offset: 0x000F882A
		public override BigInteger ToBigInteger()
		{
			return Nat.ToBigInteger(17, this.x);
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06002338 RID: 9016 RVA: 0x000FA639 File Offset: 0x000F8839
		public override string FieldName
		{
			get
			{
				return "SecP521R1Field";
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06002339 RID: 9017 RVA: 0x000FA640 File Offset: 0x000F8840
		public override int FieldSize
		{
			get
			{
				return SecP521R1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x000FA64C File Offset: 0x000F884C
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat.Create(17);
			SecP521R1Field.Add(this.x, ((SecP521R1FieldElement)b).x, z);
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x000FA680 File Offset: 0x000F8880
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat.Create(17);
			SecP521R1Field.AddOne(this.x, z);
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x000FA6A8 File Offset: 0x000F88A8
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat.Create(17);
			SecP521R1Field.Subtract(this.x, ((SecP521R1FieldElement)b).x, z);
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x000FA6DC File Offset: 0x000F88DC
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat.Create(17);
			SecP521R1Field.Multiply(this.x, ((SecP521R1FieldElement)b).x, z);
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x000FA710 File Offset: 0x000F8910
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat.Create(17);
			Mod.Invert(SecP521R1Field.P, ((SecP521R1FieldElement)b).x, z);
			SecP521R1Field.Multiply(z, this.x, z);
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x000FA750 File Offset: 0x000F8950
		public override ECFieldElement Negate()
		{
			uint[] z = Nat.Create(17);
			SecP521R1Field.Negate(this.x, z);
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x000FA778 File Offset: 0x000F8978
		public override ECFieldElement Square()
		{
			uint[] z = Nat.Create(17);
			SecP521R1Field.Square(this.x, z);
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x000FA7A0 File Offset: 0x000F89A0
		public override ECFieldElement Invert()
		{
			uint[] z = Nat.Create(17);
			Mod.Invert(SecP521R1Field.P, this.x, z);
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x000FA7CC File Offset: 0x000F89CC
		public override ECFieldElement Sqrt()
		{
			uint[] array = this.x;
			if (Nat.IsZero(17, array) || Nat.IsOne(17, array))
			{
				return this;
			}
			uint[] z = Nat.Create(17);
			uint[] array2 = Nat.Create(17);
			SecP521R1Field.SquareN(array, 519, z);
			SecP521R1Field.Square(z, array2);
			if (!Nat.Eq(17, array, array2))
			{
				return null;
			}
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x000FA82C File Offset: 0x000F8A2C
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP521R1FieldElement);
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x000FA82C File Offset: 0x000F8A2C
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP521R1FieldElement);
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x000FA83A File Offset: 0x000F8A3A
		public virtual bool Equals(SecP521R1FieldElement other)
		{
			return this == other || (other != null && Nat.Eq(17, this.x, other.x));
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x000FA85A File Offset: 0x000F8A5A
		public override int GetHashCode()
		{
			return SecP521R1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 17);
		}

		// Token: 0x04001A76 RID: 6774
		public static readonly BigInteger Q = SecP521R1Curve.q;

		// Token: 0x04001A77 RID: 6775
		protected internal readonly uint[] x;
	}
}
