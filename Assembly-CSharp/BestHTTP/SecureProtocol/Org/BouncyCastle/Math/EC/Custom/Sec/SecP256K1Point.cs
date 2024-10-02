using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000374 RID: 884
	internal class SecP256K1Point : AbstractFpPoint
	{
		// Token: 0x06002292 RID: 8850 RVA: 0x000F7AC4 File Offset: 0x000F5CC4
		public SecP256K1Point(ECCurve curve, ECFieldElement x, ECFieldElement y) : this(curve, x, y, false)
		{
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x000F7AD0 File Offset: 0x000F5CD0
		public SecP256K1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
		{
			if (x == null != (y == null))
			{
				throw new ArgumentException("Exactly one of the field elements is null");
			}
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x000E45EA File Offset: 0x000E27EA
		internal SecP256K1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
		{
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x000F7AF2 File Offset: 0x000F5CF2
		protected override ECPoint Detach()
		{
			return new SecP256K1Point(null, this.AffineXCoord, this.AffineYCoord);
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x000F7B08 File Offset: 0x000F5D08
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
			SecP256K1FieldElement secP256K1FieldElement = (SecP256K1FieldElement)base.RawXCoord;
			SecP256K1FieldElement secP256K1FieldElement2 = (SecP256K1FieldElement)base.RawYCoord;
			SecP256K1FieldElement secP256K1FieldElement3 = (SecP256K1FieldElement)b.RawXCoord;
			SecP256K1FieldElement secP256K1FieldElement4 = (SecP256K1FieldElement)b.RawYCoord;
			SecP256K1FieldElement secP256K1FieldElement5 = (SecP256K1FieldElement)base.RawZCoords[0];
			SecP256K1FieldElement secP256K1FieldElement6 = (SecP256K1FieldElement)b.RawZCoords[0];
			uint[] array = Nat256.CreateExt();
			uint[] array2 = Nat256.Create();
			uint[] array3 = Nat256.Create();
			uint[] array4 = Nat256.Create();
			bool isOne = secP256K1FieldElement5.IsOne;
			uint[] array5;
			uint[] array6;
			if (isOne)
			{
				array5 = secP256K1FieldElement3.x;
				array6 = secP256K1FieldElement4.x;
			}
			else
			{
				array6 = array3;
				SecP256K1Field.Square(secP256K1FieldElement5.x, array6);
				array5 = array2;
				SecP256K1Field.Multiply(array6, secP256K1FieldElement3.x, array5);
				SecP256K1Field.Multiply(array6, secP256K1FieldElement5.x, array6);
				SecP256K1Field.Multiply(array6, secP256K1FieldElement4.x, array6);
			}
			bool isOne2 = secP256K1FieldElement6.IsOne;
			uint[] array7;
			uint[] array8;
			if (isOne2)
			{
				array7 = secP256K1FieldElement.x;
				array8 = secP256K1FieldElement2.x;
			}
			else
			{
				array8 = array4;
				SecP256K1Field.Square(secP256K1FieldElement6.x, array8);
				array7 = array;
				SecP256K1Field.Multiply(array8, secP256K1FieldElement.x, array7);
				SecP256K1Field.Multiply(array8, secP256K1FieldElement6.x, array8);
				SecP256K1Field.Multiply(array8, secP256K1FieldElement2.x, array8);
			}
			uint[] array9 = Nat256.Create();
			SecP256K1Field.Subtract(array7, array5, array9);
			uint[] array10 = array2;
			SecP256K1Field.Subtract(array8, array6, array10);
			if (!Nat256.IsZero(array9))
			{
				uint[] array11 = array3;
				SecP256K1Field.Square(array9, array11);
				uint[] array12 = Nat256.Create();
				SecP256K1Field.Multiply(array11, array9, array12);
				uint[] array13 = array3;
				SecP256K1Field.Multiply(array11, array7, array13);
				SecP256K1Field.Negate(array12, array12);
				Nat256.Mul(array8, array12, array);
				SecP256K1Field.Reduce32(Nat256.AddBothTo(array13, array13, array12), array12);
				SecP256K1FieldElement secP256K1FieldElement7 = new SecP256K1FieldElement(array4);
				SecP256K1Field.Square(array10, secP256K1FieldElement7.x);
				SecP256K1Field.Subtract(secP256K1FieldElement7.x, array12, secP256K1FieldElement7.x);
				SecP256K1FieldElement secP256K1FieldElement8 = new SecP256K1FieldElement(array12);
				SecP256K1Field.Subtract(array13, secP256K1FieldElement7.x, secP256K1FieldElement8.x);
				SecP256K1Field.MultiplyAddToExt(secP256K1FieldElement8.x, array10, array);
				SecP256K1Field.Reduce(array, secP256K1FieldElement8.x);
				SecP256K1FieldElement secP256K1FieldElement9 = new SecP256K1FieldElement(array9);
				if (!isOne)
				{
					SecP256K1Field.Multiply(secP256K1FieldElement9.x, secP256K1FieldElement5.x, secP256K1FieldElement9.x);
				}
				if (!isOne2)
				{
					SecP256K1Field.Multiply(secP256K1FieldElement9.x, secP256K1FieldElement6.x, secP256K1FieldElement9.x);
				}
				ECFieldElement[] zs = new ECFieldElement[]
				{
					secP256K1FieldElement9
				};
				return new SecP256K1Point(curve, secP256K1FieldElement7, secP256K1FieldElement8, zs, base.IsCompressed);
			}
			if (Nat256.IsZero(array10))
			{
				return this.Twice();
			}
			return curve.Infinity;
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x000F7DD0 File Offset: 0x000F5FD0
		public override ECPoint Twice()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			ECCurve curve = this.Curve;
			SecP256K1FieldElement secP256K1FieldElement = (SecP256K1FieldElement)base.RawYCoord;
			if (secP256K1FieldElement.IsZero)
			{
				return curve.Infinity;
			}
			SecP256K1FieldElement secP256K1FieldElement2 = (SecP256K1FieldElement)base.RawXCoord;
			SecP256K1FieldElement secP256K1FieldElement3 = (SecP256K1FieldElement)base.RawZCoords[0];
			uint[] array = Nat256.Create();
			SecP256K1Field.Square(secP256K1FieldElement.x, array);
			uint[] array2 = Nat256.Create();
			SecP256K1Field.Square(array, array2);
			uint[] array3 = Nat256.Create();
			SecP256K1Field.Square(secP256K1FieldElement2.x, array3);
			SecP256K1Field.Reduce32(Nat256.AddBothTo(array3, array3, array3), array3);
			uint[] array4 = array;
			SecP256K1Field.Multiply(array, secP256K1FieldElement2.x, array4);
			SecP256K1Field.Reduce32(Nat.ShiftUpBits(8, array4, 2, 0U), array4);
			uint[] array5 = Nat256.Create();
			SecP256K1Field.Reduce32(Nat.ShiftUpBits(8, array2, 3, 0U, array5), array5);
			SecP256K1FieldElement secP256K1FieldElement4 = new SecP256K1FieldElement(array2);
			SecP256K1Field.Square(array3, secP256K1FieldElement4.x);
			SecP256K1Field.Subtract(secP256K1FieldElement4.x, array4, secP256K1FieldElement4.x);
			SecP256K1Field.Subtract(secP256K1FieldElement4.x, array4, secP256K1FieldElement4.x);
			SecP256K1FieldElement secP256K1FieldElement5 = new SecP256K1FieldElement(array4);
			SecP256K1Field.Subtract(array4, secP256K1FieldElement4.x, secP256K1FieldElement5.x);
			SecP256K1Field.Multiply(secP256K1FieldElement5.x, array3, secP256K1FieldElement5.x);
			SecP256K1Field.Subtract(secP256K1FieldElement5.x, array5, secP256K1FieldElement5.x);
			SecP256K1FieldElement secP256K1FieldElement6 = new SecP256K1FieldElement(array3);
			SecP256K1Field.Twice(secP256K1FieldElement.x, secP256K1FieldElement6.x);
			if (!secP256K1FieldElement3.IsOne)
			{
				SecP256K1Field.Multiply(secP256K1FieldElement6.x, secP256K1FieldElement3.x, secP256K1FieldElement6.x);
			}
			return new SecP256K1Point(curve, secP256K1FieldElement4, secP256K1FieldElement5, new ECFieldElement[]
			{
				secP256K1FieldElement6
			}, base.IsCompressed);
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x000F7F8C File Offset: 0x000F618C
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

		// Token: 0x06002299 RID: 8857 RVA: 0x000F13B4 File Offset: 0x000EF5B4
		public override ECPoint ThreeTimes()
		{
			if (base.IsInfinity || base.RawYCoord.IsZero)
			{
				return this;
			}
			return this.Twice().Add(this);
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x000F7FD8 File Offset: 0x000F61D8
		public override ECPoint Negate()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			return new SecP256K1Point(this.Curve, base.RawXCoord, base.RawYCoord.Negate(), base.RawZCoords, base.IsCompressed);
		}
	}
}
