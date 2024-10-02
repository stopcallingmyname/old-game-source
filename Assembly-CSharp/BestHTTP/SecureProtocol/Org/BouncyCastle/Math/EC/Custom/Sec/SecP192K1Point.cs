﻿using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000364 RID: 868
	internal class SecP192K1Point : AbstractFpPoint
	{
		// Token: 0x06002199 RID: 8601 RVA: 0x000F3EE3 File Offset: 0x000F20E3
		public SecP192K1Point(ECCurve curve, ECFieldElement x, ECFieldElement y) : this(curve, x, y, false)
		{
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x000E45C8 File Offset: 0x000E27C8
		public SecP192K1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
		{
			if (x == null != (y == null))
			{
				throw new ArgumentException("Exactly one of the field elements is null");
			}
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x000E45EA File Offset: 0x000E27EA
		internal SecP192K1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
		{
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x000F3EEF File Offset: 0x000F20EF
		protected override ECPoint Detach()
		{
			return new SecP192K1Point(null, this.AffineXCoord, this.AffineYCoord);
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x000F3F04 File Offset: 0x000F2104
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
			SecP192K1FieldElement secP192K1FieldElement = (SecP192K1FieldElement)base.RawXCoord;
			SecP192K1FieldElement secP192K1FieldElement2 = (SecP192K1FieldElement)base.RawYCoord;
			SecP192K1FieldElement secP192K1FieldElement3 = (SecP192K1FieldElement)b.RawXCoord;
			SecP192K1FieldElement secP192K1FieldElement4 = (SecP192K1FieldElement)b.RawYCoord;
			SecP192K1FieldElement secP192K1FieldElement5 = (SecP192K1FieldElement)base.RawZCoords[0];
			SecP192K1FieldElement secP192K1FieldElement6 = (SecP192K1FieldElement)b.RawZCoords[0];
			uint[] array = Nat192.CreateExt();
			uint[] array2 = Nat192.Create();
			uint[] array3 = Nat192.Create();
			uint[] array4 = Nat192.Create();
			bool isOne = secP192K1FieldElement5.IsOne;
			uint[] array5;
			uint[] array6;
			if (isOne)
			{
				array5 = secP192K1FieldElement3.x;
				array6 = secP192K1FieldElement4.x;
			}
			else
			{
				array6 = array3;
				SecP192K1Field.Square(secP192K1FieldElement5.x, array6);
				array5 = array2;
				SecP192K1Field.Multiply(array6, secP192K1FieldElement3.x, array5);
				SecP192K1Field.Multiply(array6, secP192K1FieldElement5.x, array6);
				SecP192K1Field.Multiply(array6, secP192K1FieldElement4.x, array6);
			}
			bool isOne2 = secP192K1FieldElement6.IsOne;
			uint[] array7;
			uint[] array8;
			if (isOne2)
			{
				array7 = secP192K1FieldElement.x;
				array8 = secP192K1FieldElement2.x;
			}
			else
			{
				array8 = array4;
				SecP192K1Field.Square(secP192K1FieldElement6.x, array8);
				array7 = array;
				SecP192K1Field.Multiply(array8, secP192K1FieldElement.x, array7);
				SecP192K1Field.Multiply(array8, secP192K1FieldElement6.x, array8);
				SecP192K1Field.Multiply(array8, secP192K1FieldElement2.x, array8);
			}
			uint[] array9 = Nat192.Create();
			SecP192K1Field.Subtract(array7, array5, array9);
			uint[] array10 = array2;
			SecP192K1Field.Subtract(array8, array6, array10);
			if (!Nat192.IsZero(array9))
			{
				uint[] array11 = array3;
				SecP192K1Field.Square(array9, array11);
				uint[] array12 = Nat192.Create();
				SecP192K1Field.Multiply(array11, array9, array12);
				uint[] array13 = array3;
				SecP192K1Field.Multiply(array11, array7, array13);
				SecP192K1Field.Negate(array12, array12);
				Nat192.Mul(array8, array12, array);
				SecP192K1Field.Reduce32(Nat192.AddBothTo(array13, array13, array12), array12);
				SecP192K1FieldElement secP192K1FieldElement7 = new SecP192K1FieldElement(array4);
				SecP192K1Field.Square(array10, secP192K1FieldElement7.x);
				SecP192K1Field.Subtract(secP192K1FieldElement7.x, array12, secP192K1FieldElement7.x);
				SecP192K1FieldElement secP192K1FieldElement8 = new SecP192K1FieldElement(array12);
				SecP192K1Field.Subtract(array13, secP192K1FieldElement7.x, secP192K1FieldElement8.x);
				SecP192K1Field.MultiplyAddToExt(secP192K1FieldElement8.x, array10, array);
				SecP192K1Field.Reduce(array, secP192K1FieldElement8.x);
				SecP192K1FieldElement secP192K1FieldElement9 = new SecP192K1FieldElement(array9);
				if (!isOne)
				{
					SecP192K1Field.Multiply(secP192K1FieldElement9.x, secP192K1FieldElement5.x, secP192K1FieldElement9.x);
				}
				if (!isOne2)
				{
					SecP192K1Field.Multiply(secP192K1FieldElement9.x, secP192K1FieldElement6.x, secP192K1FieldElement9.x);
				}
				ECFieldElement[] zs = new ECFieldElement[]
				{
					secP192K1FieldElement9
				};
				return new SecP192K1Point(curve, secP192K1FieldElement7, secP192K1FieldElement8, zs, base.IsCompressed);
			}
			if (Nat192.IsZero(array10))
			{
				return this.Twice();
			}
			return curve.Infinity;
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x000F41CC File Offset: 0x000F23CC
		public override ECPoint Twice()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			ECCurve curve = this.Curve;
			SecP192K1FieldElement secP192K1FieldElement = (SecP192K1FieldElement)base.RawYCoord;
			if (secP192K1FieldElement.IsZero)
			{
				return curve.Infinity;
			}
			SecP192K1FieldElement secP192K1FieldElement2 = (SecP192K1FieldElement)base.RawXCoord;
			SecP192K1FieldElement secP192K1FieldElement3 = (SecP192K1FieldElement)base.RawZCoords[0];
			uint[] array = Nat192.Create();
			SecP192K1Field.Square(secP192K1FieldElement.x, array);
			uint[] array2 = Nat192.Create();
			SecP192K1Field.Square(array, array2);
			uint[] array3 = Nat192.Create();
			SecP192K1Field.Square(secP192K1FieldElement2.x, array3);
			SecP192K1Field.Reduce32(Nat192.AddBothTo(array3, array3, array3), array3);
			uint[] array4 = array;
			SecP192K1Field.Multiply(array, secP192K1FieldElement2.x, array4);
			SecP192K1Field.Reduce32(Nat.ShiftUpBits(6, array4, 2, 0U), array4);
			uint[] array5 = Nat192.Create();
			SecP192K1Field.Reduce32(Nat.ShiftUpBits(6, array2, 3, 0U, array5), array5);
			SecP192K1FieldElement secP192K1FieldElement4 = new SecP192K1FieldElement(array2);
			SecP192K1Field.Square(array3, secP192K1FieldElement4.x);
			SecP192K1Field.Subtract(secP192K1FieldElement4.x, array4, secP192K1FieldElement4.x);
			SecP192K1Field.Subtract(secP192K1FieldElement4.x, array4, secP192K1FieldElement4.x);
			SecP192K1FieldElement secP192K1FieldElement5 = new SecP192K1FieldElement(array4);
			SecP192K1Field.Subtract(array4, secP192K1FieldElement4.x, secP192K1FieldElement5.x);
			SecP192K1Field.Multiply(secP192K1FieldElement5.x, array3, secP192K1FieldElement5.x);
			SecP192K1Field.Subtract(secP192K1FieldElement5.x, array5, secP192K1FieldElement5.x);
			SecP192K1FieldElement secP192K1FieldElement6 = new SecP192K1FieldElement(array3);
			SecP192K1Field.Twice(secP192K1FieldElement.x, secP192K1FieldElement6.x);
			if (!secP192K1FieldElement3.IsOne)
			{
				SecP192K1Field.Multiply(secP192K1FieldElement6.x, secP192K1FieldElement3.x, secP192K1FieldElement6.x);
			}
			return new SecP192K1Point(curve, secP192K1FieldElement4, secP192K1FieldElement5, new ECFieldElement[]
			{
				secP192K1FieldElement6
			}, base.IsCompressed);
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x000F4388 File Offset: 0x000F2588
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

		// Token: 0x060021A0 RID: 8608 RVA: 0x000F13B4 File Offset: 0x000EF5B4
		public override ECPoint ThreeTimes()
		{
			if (base.IsInfinity || base.RawYCoord.IsZero)
			{
				return this;
			}
			return this.Twice().Add(this);
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x000F43D4 File Offset: 0x000F25D4
		public override ECPoint Negate()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			return new SecP192K1Point(this.Curve, base.RawXCoord, base.RawYCoord.Negate(), base.RawZCoords, base.IsCompressed);
		}
	}
}
