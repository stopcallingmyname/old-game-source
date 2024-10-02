using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200038E RID: 910
	internal class SecT163FieldElement : AbstractF2mFieldElement
	{
		// Token: 0x06002431 RID: 9265 RVA: 0x000FE5CD File Offset: 0x000FC7CD
		public SecT163FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 163)
			{
				throw new ArgumentException("value invalid for SecT163FieldElement", "x");
			}
			this.x = SecT163Field.FromBigInteger(x);
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x000FE60A File Offset: 0x000FC80A
		public SecT163FieldElement()
		{
			this.x = Nat192.Create64();
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x000FE61D File Offset: 0x000FC81D
		protected internal SecT163FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06002434 RID: 9268 RVA: 0x000FE62C File Offset: 0x000FC82C
		public override bool IsOne
		{
			get
			{
				return Nat192.IsOne64(this.x);
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06002435 RID: 9269 RVA: 0x000FE639 File Offset: 0x000FC839
		public override bool IsZero
		{
			get
			{
				return Nat192.IsZero64(this.x);
			}
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x000FE646 File Offset: 0x000FC846
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x000FE657 File Offset: 0x000FC857
		public override BigInteger ToBigInteger()
		{
			return Nat192.ToBigInteger64(this.x);
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06002438 RID: 9272 RVA: 0x000FE664 File Offset: 0x000FC864
		public override string FieldName
		{
			get
			{
				return "SecT163Field";
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06002439 RID: 9273 RVA: 0x000FE66B File Offset: 0x000FC86B
		public override int FieldSize
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x000FE674 File Offset: 0x000FC874
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat192.Create64();
			SecT163Field.Add(this.x, ((SecT163FieldElement)b).x, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x000FE6A4 File Offset: 0x000FC8A4
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat192.Create64();
			SecT163Field.AddOne(this.x, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x000E3A75 File Offset: 0x000E1C75
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x000FE6CC File Offset: 0x000FC8CC
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat192.Create64();
			SecT163Field.Multiply(this.x, ((SecT163FieldElement)b).x, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x000E3AB3 File Offset: 0x000E1CB3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x000FE6FC File Offset: 0x000FC8FC
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT163FieldElement)b).x;
			ulong[] array2 = ((SecT163FieldElement)x).x;
			ulong[] y3 = ((SecT163FieldElement)y).x;
			ulong[] array3 = Nat192.CreateExt64();
			SecT163Field.MultiplyAddToExt(array, y2, array3);
			SecT163Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat192.Create64();
			SecT163Field.Reduce(array3, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x000FB43A File Offset: 0x000F963A
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x000947CE File Offset: 0x000929CE
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x000FE760 File Offset: 0x000FC960
		public override ECFieldElement Square()
		{
			ulong[] z = Nat192.Create64();
			SecT163Field.Square(this.x, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x000E3BA1 File Offset: 0x000E1DA1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x000FE788 File Offset: 0x000FC988
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT163FieldElement)x).x;
			ulong[] y2 = ((SecT163FieldElement)y).x;
			ulong[] array3 = Nat192.CreateExt64();
			SecT163Field.SquareAddToExt(array, array3);
			SecT163Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat192.Create64();
			SecT163Field.Reduce(array3, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x000FE7DC File Offset: 0x000FC9DC
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat192.Create64();
			SecT163Field.SquareN(this.x, pow, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x000FE808 File Offset: 0x000FCA08
		public override int Trace()
		{
			return (int)SecT163Field.Trace(this.x);
		}

		// Token: 0x06002447 RID: 9287 RVA: 0x000FE818 File Offset: 0x000FCA18
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat192.Create64();
			SecT163Field.Invert(this.x, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x06002448 RID: 9288 RVA: 0x000FE840 File Offset: 0x000FCA40
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat192.Create64();
			SecT163Field.Sqrt(this.x, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06002449 RID: 9289 RVA: 0x000A89A4 File Offset: 0x000A6BA4
		public virtual int Representation
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x0600244A RID: 9290 RVA: 0x000FE66B File Offset: 0x000FC86B
		public virtual int M
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x0600244B RID: 9291 RVA: 0x000A89A4 File Offset: 0x000A6BA4
		public virtual int K1
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x0600244C RID: 9292 RVA: 0x000A4E21 File Offset: 0x000A3021
		public virtual int K2
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x0600244D RID: 9293 RVA: 0x000FE865 File Offset: 0x000FCA65
		public virtual int K3
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x000FE868 File Offset: 0x000FCA68
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT163FieldElement);
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x000FE868 File Offset: 0x000FCA68
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT163FieldElement);
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x000FE876 File Offset: 0x000FCA76
		public virtual bool Equals(SecT163FieldElement other)
		{
			return this == other || (other != null && Nat192.Eq64(this.x, other.x));
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x000FE894 File Offset: 0x000FCA94
		public override int GetHashCode()
		{
			return 163763 ^ Arrays.GetHashCode(this.x, 0, 3);
		}

		// Token: 0x04001A8E RID: 6798
		protected internal readonly ulong[] x;
	}
}
