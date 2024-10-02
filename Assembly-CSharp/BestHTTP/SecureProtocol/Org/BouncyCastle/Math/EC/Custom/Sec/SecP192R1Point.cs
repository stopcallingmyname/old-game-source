﻿using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000368 RID: 872
	internal class SecP192R1Point : AbstractFpPoint
	{
		// Token: 0x060021D7 RID: 8663 RVA: 0x000F4DB7 File Offset: 0x000F2FB7
		public SecP192R1Point(ECCurve curve, ECFieldElement x, ECFieldElement y) : this(curve, x, y, false)
		{
		}

		// Token: 0x060021D8 RID: 8664 RVA: 0x000E45C8 File Offset: 0x000E27C8
		public SecP192R1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
		{
			if (x == null != (y == null))
			{
				throw new ArgumentException("Exactly one of the field elements is null");
			}
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x000E45EA File Offset: 0x000E27EA
		internal SecP192R1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
		{
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x000F4DC3 File Offset: 0x000F2FC3
		protected override ECPoint Detach()
		{
			return new SecP192R1Point(null, this.AffineXCoord, this.AffineYCoord);
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x000F4DD8 File Offset: 0x000F2FD8
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
			SecP192R1FieldElement secP192R1FieldElement = (SecP192R1FieldElement)base.RawXCoord;
			SecP192R1FieldElement secP192R1FieldElement2 = (SecP192R1FieldElement)base.RawYCoord;
			SecP192R1FieldElement secP192R1FieldElement3 = (SecP192R1FieldElement)b.RawXCoord;
			SecP192R1FieldElement secP192R1FieldElement4 = (SecP192R1FieldElement)b.RawYCoord;
			SecP192R1FieldElement secP192R1FieldElement5 = (SecP192R1FieldElement)base.RawZCoords[0];
			SecP192R1FieldElement secP192R1FieldElement6 = (SecP192R1FieldElement)b.RawZCoords[0];
			uint[] array = Nat192.CreateExt();
			uint[] array2 = Nat192.Create();
			uint[] array3 = Nat192.Create();
			uint[] array4 = Nat192.Create();
			bool isOne = secP192R1FieldElement5.IsOne;
			uint[] array5;
			uint[] array6;
			if (isOne)
			{
				array5 = secP192R1FieldElement3.x;
				array6 = secP192R1FieldElement4.x;
			}
			else
			{
				array6 = array3;
				SecP192R1Field.Square(secP192R1FieldElement5.x, array6);
				array5 = array2;
				SecP192R1Field.Multiply(array6, secP192R1FieldElement3.x, array5);
				SecP192R1Field.Multiply(array6, secP192R1FieldElement5.x, array6);
				SecP192R1Field.Multiply(array6, secP192R1FieldElement4.x, array6);
			}
			bool isOne2 = secP192R1FieldElement6.IsOne;
			uint[] array7;
			uint[] array8;
			if (isOne2)
			{
				array7 = secP192R1FieldElement.x;
				array8 = secP192R1FieldElement2.x;
			}
			else
			{
				array8 = array4;
				SecP192R1Field.Square(secP192R1FieldElement6.x, array8);
				array7 = array;
				SecP192R1Field.Multiply(array8, secP192R1FieldElement.x, array7);
				SecP192R1Field.Multiply(array8, secP192R1FieldElement6.x, array8);
				SecP192R1Field.Multiply(array8, secP192R1FieldElement2.x, array8);
			}
			uint[] array9 = Nat192.Create();
			SecP192R1Field.Subtract(array7, array5, array9);
			uint[] array10 = array2;
			SecP192R1Field.Subtract(array8, array6, array10);
			if (!Nat192.IsZero(array9))
			{
				uint[] array11 = array3;
				SecP192R1Field.Square(array9, array11);
				uint[] array12 = Nat192.Create();
				SecP192R1Field.Multiply(array11, array9, array12);
				uint[] array13 = array3;
				SecP192R1Field.Multiply(array11, array7, array13);
				SecP192R1Field.Negate(array12, array12);
				Nat192.Mul(array8, array12, array);
				SecP192R1Field.Reduce32(Nat192.AddBothTo(array13, array13, array12), array12);
				SecP192R1FieldElement secP192R1FieldElement7 = new SecP192R1FieldElement(array4);
				SecP192R1Field.Square(array10, secP192R1FieldElement7.x);
				SecP192R1Field.Subtract(secP192R1FieldElement7.x, array12, secP192R1FieldElement7.x);
				SecP192R1FieldElement secP192R1FieldElement8 = new SecP192R1FieldElement(array12);
				SecP192R1Field.Subtract(array13, secP192R1FieldElement7.x, secP192R1FieldElement8.x);
				SecP192R1Field.MultiplyAddToExt(secP192R1FieldElement8.x, array10, array);
				SecP192R1Field.Reduce(array, secP192R1FieldElement8.x);
				SecP192R1FieldElement secP192R1FieldElement9 = new SecP192R1FieldElement(array9);
				if (!isOne)
				{
					SecP192R1Field.Multiply(secP192R1FieldElement9.x, secP192R1FieldElement5.x, secP192R1FieldElement9.x);
				}
				if (!isOne2)
				{
					SecP192R1Field.Multiply(secP192R1FieldElement9.x, secP192R1FieldElement6.x, secP192R1FieldElement9.x);
				}
				ECFieldElement[] zs = new ECFieldElement[]
				{
					secP192R1FieldElement9
				};
				return new SecP192R1Point(curve, secP192R1FieldElement7, secP192R1FieldElement8, zs, base.IsCompressed);
			}
			if (Nat192.IsZero(array10))
			{
				return this.Twice();
			}
			return curve.Infinity;
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x000F50A0 File Offset: 0x000F32A0
		public override ECPoint Twice()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			ECCurve curve = this.Curve;
			SecP192R1FieldElement secP192R1FieldElement = (SecP192R1FieldElement)base.RawYCoord;
			if (secP192R1FieldElement.IsZero)
			{
				return curve.Infinity;
			}
			SecP192R1FieldElement secP192R1FieldElement2 = (SecP192R1FieldElement)base.RawXCoord;
			SecP192R1FieldElement secP192R1FieldElement3 = (SecP192R1FieldElement)base.RawZCoords[0];
			uint[] array = Nat192.Create();
			uint[] array2 = Nat192.Create();
			uint[] array3 = Nat192.Create();
			SecP192R1Field.Square(secP192R1FieldElement.x, array3);
			uint[] array4 = Nat192.Create();
			SecP192R1Field.Square(array3, array4);
			bool isOne = secP192R1FieldElement3.IsOne;
			uint[] array5 = secP192R1FieldElement3.x;
			if (!isOne)
			{
				array5 = array2;
				SecP192R1Field.Square(secP192R1FieldElement3.x, array5);
			}
			SecP192R1Field.Subtract(secP192R1FieldElement2.x, array5, array);
			uint[] array6 = array2;
			SecP192R1Field.Add(secP192R1FieldElement2.x, array5, array6);
			SecP192R1Field.Multiply(array6, array, array6);
			SecP192R1Field.Reduce32(Nat192.AddBothTo(array6, array6, array6), array6);
			uint[] array7 = array3;
			SecP192R1Field.Multiply(array3, secP192R1FieldElement2.x, array7);
			SecP192R1Field.Reduce32(Nat.ShiftUpBits(6, array7, 2, 0U), array7);
			SecP192R1Field.Reduce32(Nat.ShiftUpBits(6, array4, 3, 0U, array), array);
			SecP192R1FieldElement secP192R1FieldElement4 = new SecP192R1FieldElement(array4);
			SecP192R1Field.Square(array6, secP192R1FieldElement4.x);
			SecP192R1Field.Subtract(secP192R1FieldElement4.x, array7, secP192R1FieldElement4.x);
			SecP192R1Field.Subtract(secP192R1FieldElement4.x, array7, secP192R1FieldElement4.x);
			SecP192R1FieldElement secP192R1FieldElement5 = new SecP192R1FieldElement(array7);
			SecP192R1Field.Subtract(array7, secP192R1FieldElement4.x, secP192R1FieldElement5.x);
			SecP192R1Field.Multiply(secP192R1FieldElement5.x, array6, secP192R1FieldElement5.x);
			SecP192R1Field.Subtract(secP192R1FieldElement5.x, array, secP192R1FieldElement5.x);
			SecP192R1FieldElement secP192R1FieldElement6 = new SecP192R1FieldElement(array6);
			SecP192R1Field.Twice(secP192R1FieldElement.x, secP192R1FieldElement6.x);
			if (!isOne)
			{
				SecP192R1Field.Multiply(secP192R1FieldElement6.x, secP192R1FieldElement3.x, secP192R1FieldElement6.x);
			}
			return new SecP192R1Point(curve, secP192R1FieldElement4, secP192R1FieldElement5, new ECFieldElement[]
			{
				secP192R1FieldElement6
			}, base.IsCompressed);
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x000F5298 File Offset: 0x000F3498
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

		// Token: 0x060021DE RID: 8670 RVA: 0x000F13B4 File Offset: 0x000EF5B4
		public override ECPoint ThreeTimes()
		{
			if (base.IsInfinity || base.RawYCoord.IsZero)
			{
				return this;
			}
			return this.Twice().Add(this);
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x000F52E4 File Offset: 0x000F34E4
		public override ECPoint Negate()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			return new SecP192R1Point(this.Curve, base.RawXCoord, base.RawYCoord.Negate(), base.RawZCoords, base.IsCompressed);
		}
	}
}
