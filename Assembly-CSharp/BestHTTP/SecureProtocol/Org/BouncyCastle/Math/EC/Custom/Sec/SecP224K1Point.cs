using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200036C RID: 876
	internal class SecP224K1Point : AbstractFpPoint
	{
		// Token: 0x06002213 RID: 8723 RVA: 0x000F5C05 File Offset: 0x000F3E05
		public SecP224K1Point(ECCurve curve, ECFieldElement x, ECFieldElement y) : this(curve, x, y, false)
		{
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x000E45C8 File Offset: 0x000E27C8
		public SecP224K1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
		{
			if (x == null != (y == null))
			{
				throw new ArgumentException("Exactly one of the field elements is null");
			}
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x000E45EA File Offset: 0x000E27EA
		internal SecP224K1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
		{
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x000F5C11 File Offset: 0x000F3E11
		protected override ECPoint Detach()
		{
			return new SecP224K1Point(null, this.AffineXCoord, this.AffineYCoord);
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x000F5C28 File Offset: 0x000F3E28
		public override ECPoint Add(ECPoint b)
		{
			if (base.IsInfinity)
			{
				return b;
			}
			if (b.IsInfinity)
			{
				return this;
			}
			if (this == b)
			{
				return this.Twice();
			}
			ECCurve curve = this.Curve;
			SecP224K1FieldElement secP224K1FieldElement = (SecP224K1FieldElement)base.RawXCoord;
			SecP224K1FieldElement secP224K1FieldElement2 = (SecP224K1FieldElement)base.RawYCoord;
			SecP224K1FieldElement secP224K1FieldElement3 = (SecP224K1FieldElement)b.RawXCoord;
			SecP224K1FieldElement secP224K1FieldElement4 = (SecP224K1FieldElement)b.RawYCoord;
			SecP224K1FieldElement secP224K1FieldElement5 = (SecP224K1FieldElement)base.RawZCoords[0];
			SecP224K1FieldElement secP224K1FieldElement6 = (SecP224K1FieldElement)b.RawZCoords[0];
			uint[] array = Nat224.CreateExt();
			uint[] array2 = Nat224.Create();
			uint[] array3 = Nat224.Create();
			uint[] array4 = Nat224.Create();
			bool isOne = secP224K1FieldElement5.IsOne;
			uint[] array5;
			uint[] array6;
			if (isOne)
			{
				array5 = secP224K1FieldElement3.x;
				array6 = secP224K1FieldElement4.x;
			}
			else
			{
				array6 = array3;
				SecP224K1Field.Square(secP224K1FieldElement5.x, array6);
				array5 = array2;
				SecP224K1Field.Multiply(array6, secP224K1FieldElement3.x, array5);
				SecP224K1Field.Multiply(array6, secP224K1FieldElement5.x, array6);
				SecP224K1Field.Multiply(array6, secP224K1FieldElement4.x, array6);
			}
			bool isOne2 = secP224K1FieldElement6.IsOne;
			uint[] array7;
			uint[] array8;
			if (isOne2)
			{
				array7 = secP224K1FieldElement.x;
				array8 = secP224K1FieldElement2.x;
			}
			else
			{
				array8 = array4;
				SecP224K1Field.Square(secP224K1FieldElement6.x, array8);
				array7 = array;
				SecP224K1Field.Multiply(array8, secP224K1FieldElement.x, array7);
				SecP224K1Field.Multiply(array8, secP224K1FieldElement6.x, array8);
				SecP224K1Field.Multiply(array8, secP224K1FieldElement2.x, array8);
			}
			uint[] array9 = Nat224.Create();
			SecP224K1Field.Subtract(array7, array5, array9);
			uint[] array10 = array2;
			SecP224K1Field.Subtract(array8, array6, array10);
			if (!Nat224.IsZero(array9))
			{
				uint[] array11 = array3;
				SecP224K1Field.Square(array9, array11);
				uint[] array12 = Nat224.Create();
				SecP224K1Field.Multiply(array11, array9, array12);
				uint[] array13 = array3;
				SecP224K1Field.Multiply(array11, array7, array13);
				SecP224K1Field.Negate(array12, array12);
				Nat224.Mul(array8, array12, array);
				SecP224K1Field.Reduce32(Nat224.AddBothTo(array13, array13, array12), array12);
				SecP224K1FieldElement secP224K1FieldElement7 = new SecP224K1FieldElement(array4);
				SecP224K1Field.Square(array10, secP224K1FieldElement7.x);
				SecP224K1Field.Subtract(secP224K1FieldElement7.x, array12, secP224K1FieldElement7.x);
				SecP224K1FieldElement secP224K1FieldElement8 = new SecP224K1FieldElement(array12);
				SecP224K1Field.Subtract(array13, secP224K1FieldElement7.x, secP224K1FieldElement8.x);
				SecP224K1Field.MultiplyAddToExt(secP224K1FieldElement8.x, array10, array);
				SecP224K1Field.Reduce(array, secP224K1FieldElement8.x);
				SecP224K1FieldElement secP224K1FieldElement9 = new SecP224K1FieldElement(array9);
				if (!isOne)
				{
					SecP224K1Field.Multiply(secP224K1FieldElement9.x, secP224K1FieldElement5.x, secP224K1FieldElement9.x);
				}
				if (!isOne2)
				{
					SecP224K1Field.Multiply(secP224K1FieldElement9.x, secP224K1FieldElement6.x, secP224K1FieldElement9.x);
				}
				ECFieldElement[] zs = new ECFieldElement[]
				{
					secP224K1FieldElement9
				};
				return new SecP224K1Point(curve, secP224K1FieldElement7, secP224K1FieldElement8, zs, base.IsCompressed);
			}
			if (Nat224.IsZero(array10))
			{
				return this.Twice();
			}
			return curve.Infinity;
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x000F5EF0 File Offset: 0x000F40F0
		public override ECPoint Twice()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			ECCurve curve = this.Curve;
			SecP224K1FieldElement secP224K1FieldElement = (SecP224K1FieldElement)base.RawYCoord;
			if (secP224K1FieldElement.IsZero)
			{
				return curve.Infinity;
			}
			SecP224K1FieldElement secP224K1FieldElement2 = (SecP224K1FieldElement)base.RawXCoord;
			SecP224K1FieldElement secP224K1FieldElement3 = (SecP224K1FieldElement)base.RawZCoords[0];
			uint[] array = Nat224.Create();
			SecP224K1Field.Square(secP224K1FieldElement.x, array);
			uint[] array2 = Nat224.Create();
			SecP224K1Field.Square(array, array2);
			uint[] array3 = Nat224.Create();
			SecP224K1Field.Square(secP224K1FieldElement2.x, array3);
			SecP224K1Field.Reduce32(Nat224.AddBothTo(array3, array3, array3), array3);
			uint[] array4 = array;
			SecP224K1Field.Multiply(array, secP224K1FieldElement2.x, array4);
			SecP224K1Field.Reduce32(Nat.ShiftUpBits(7, array4, 2, 0U), array4);
			uint[] array5 = Nat224.Create();
			SecP224K1Field.Reduce32(Nat.ShiftUpBits(7, array2, 3, 0U, array5), array5);
			SecP224K1FieldElement secP224K1FieldElement4 = new SecP224K1FieldElement(array2);
			SecP224K1Field.Square(array3, secP224K1FieldElement4.x);
			SecP224K1Field.Subtract(secP224K1FieldElement4.x, array4, secP224K1FieldElement4.x);
			SecP224K1Field.Subtract(secP224K1FieldElement4.x, array4, secP224K1FieldElement4.x);
			SecP224K1FieldElement secP224K1FieldElement5 = new SecP224K1FieldElement(array4);
			SecP224K1Field.Subtract(array4, secP224K1FieldElement4.x, secP224K1FieldElement5.x);
			SecP224K1Field.Multiply(secP224K1FieldElement5.x, array3, secP224K1FieldElement5.x);
			SecP224K1Field.Subtract(secP224K1FieldElement5.x, array5, secP224K1FieldElement5.x);
			SecP224K1FieldElement secP224K1FieldElement6 = new SecP224K1FieldElement(array3);
			SecP224K1Field.Twice(secP224K1FieldElement.x, secP224K1FieldElement6.x);
			if (!secP224K1FieldElement3.IsOne)
			{
				SecP224K1Field.Multiply(secP224K1FieldElement6.x, secP224K1FieldElement3.x, secP224K1FieldElement6.x);
			}
			return new SecP224K1Point(curve, secP224K1FieldElement4, secP224K1FieldElement5, new ECFieldElement[]
			{
				secP224K1FieldElement6
			}, base.IsCompressed);
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x000F60AC File Offset: 0x000F42AC
		public override ECPoint TwicePlus(ECPoint b)
		{
			if (this == b)
			{
				return this.ThreeTimes();
			}
			if (base.IsInfinity)
			{
				return b;
			}
			if (b.IsInfinity)
			{
				return this.Twice();
			}
			if (base.RawYCoord.IsZero)
			{
				return b;
			}
			return this.Twice().Add(b);
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x000F13B4 File Offset: 0x000EF5B4
		public override ECPoint ThreeTimes()
		{
			if (base.IsInfinity || base.RawYCoord.IsZero)
			{
				return this;
			}
			return this.Twice().Add(this);
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x000F60F8 File Offset: 0x000F42F8
		public override ECPoint Negate()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			return new SecP224K1Point(this.Curve, base.RawXCoord, base.RawYCoord.Negate(), base.RawZCoords, base.IsCompressed);
		}
	}
}
