using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.GM
{
	// Token: 0x020003BA RID: 954
	internal class SM2P256V1Point : AbstractFpPoint
	{
		// Token: 0x0600272C RID: 10028 RVA: 0x00109873 File Offset: 0x00107A73
		public SM2P256V1Point(ECCurve curve, ECFieldElement x, ECFieldElement y) : this(curve, x, y, false)
		{
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x000E45C8 File Offset: 0x000E27C8
		public SM2P256V1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
		{
			if (x == null != (y == null))
			{
				throw new ArgumentException("Exactly one of the field elements is null");
			}
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x000E45EA File Offset: 0x000E27EA
		internal SM2P256V1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
		{
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x0010987F File Offset: 0x00107A7F
		protected override ECPoint Detach()
		{
			return new SM2P256V1Point(null, this.AffineXCoord, this.AffineYCoord);
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x00109894 File Offset: 0x00107A94
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
			SM2P256V1FieldElement sm2P256V1FieldElement = (SM2P256V1FieldElement)base.RawXCoord;
			SM2P256V1FieldElement sm2P256V1FieldElement2 = (SM2P256V1FieldElement)base.RawYCoord;
			SM2P256V1FieldElement sm2P256V1FieldElement3 = (SM2P256V1FieldElement)b.RawXCoord;
			SM2P256V1FieldElement sm2P256V1FieldElement4 = (SM2P256V1FieldElement)b.RawYCoord;
			SM2P256V1FieldElement sm2P256V1FieldElement5 = (SM2P256V1FieldElement)base.RawZCoords[0];
			SM2P256V1FieldElement sm2P256V1FieldElement6 = (SM2P256V1FieldElement)b.RawZCoords[0];
			uint[] array = Nat256.CreateExt();
			uint[] array2 = Nat256.Create();
			uint[] array3 = Nat256.Create();
			uint[] array4 = Nat256.Create();
			bool isOne = sm2P256V1FieldElement5.IsOne;
			uint[] array5;
			uint[] array6;
			if (isOne)
			{
				array5 = sm2P256V1FieldElement3.x;
				array6 = sm2P256V1FieldElement4.x;
			}
			else
			{
				array6 = array3;
				SM2P256V1Field.Square(sm2P256V1FieldElement5.x, array6);
				array5 = array2;
				SM2P256V1Field.Multiply(array6, sm2P256V1FieldElement3.x, array5);
				SM2P256V1Field.Multiply(array6, sm2P256V1FieldElement5.x, array6);
				SM2P256V1Field.Multiply(array6, sm2P256V1FieldElement4.x, array6);
			}
			bool isOne2 = sm2P256V1FieldElement6.IsOne;
			uint[] array7;
			uint[] array8;
			if (isOne2)
			{
				array7 = sm2P256V1FieldElement.x;
				array8 = sm2P256V1FieldElement2.x;
			}
			else
			{
				array8 = array4;
				SM2P256V1Field.Square(sm2P256V1FieldElement6.x, array8);
				array7 = array;
				SM2P256V1Field.Multiply(array8, sm2P256V1FieldElement.x, array7);
				SM2P256V1Field.Multiply(array8, sm2P256V1FieldElement6.x, array8);
				SM2P256V1Field.Multiply(array8, sm2P256V1FieldElement2.x, array8);
			}
			uint[] array9 = Nat256.Create();
			SM2P256V1Field.Subtract(array7, array5, array9);
			uint[] array10 = array2;
			SM2P256V1Field.Subtract(array8, array6, array10);
			if (!Nat256.IsZero(array9))
			{
				uint[] array11 = array3;
				SM2P256V1Field.Square(array9, array11);
				uint[] array12 = Nat256.Create();
				SM2P256V1Field.Multiply(array11, array9, array12);
				uint[] array13 = array3;
				SM2P256V1Field.Multiply(array11, array7, array13);
				SM2P256V1Field.Negate(array12, array12);
				Nat256.Mul(array8, array12, array);
				SM2P256V1Field.Reduce32(Nat256.AddBothTo(array13, array13, array12), array12);
				SM2P256V1FieldElement sm2P256V1FieldElement7 = new SM2P256V1FieldElement(array4);
				SM2P256V1Field.Square(array10, sm2P256V1FieldElement7.x);
				SM2P256V1Field.Subtract(sm2P256V1FieldElement7.x, array12, sm2P256V1FieldElement7.x);
				SM2P256V1FieldElement sm2P256V1FieldElement8 = new SM2P256V1FieldElement(array12);
				SM2P256V1Field.Subtract(array13, sm2P256V1FieldElement7.x, sm2P256V1FieldElement8.x);
				SM2P256V1Field.MultiplyAddToExt(sm2P256V1FieldElement8.x, array10, array);
				SM2P256V1Field.Reduce(array, sm2P256V1FieldElement8.x);
				SM2P256V1FieldElement sm2P256V1FieldElement9 = new SM2P256V1FieldElement(array9);
				if (!isOne)
				{
					SM2P256V1Field.Multiply(sm2P256V1FieldElement9.x, sm2P256V1FieldElement5.x, sm2P256V1FieldElement9.x);
				}
				if (!isOne2)
				{
					SM2P256V1Field.Multiply(sm2P256V1FieldElement9.x, sm2P256V1FieldElement6.x, sm2P256V1FieldElement9.x);
				}
				ECFieldElement[] zs = new ECFieldElement[]
				{
					sm2P256V1FieldElement9
				};
				return new SM2P256V1Point(curve, sm2P256V1FieldElement7, sm2P256V1FieldElement8, zs, base.IsCompressed);
			}
			if (Nat256.IsZero(array10))
			{
				return this.Twice();
			}
			return curve.Infinity;
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x00109B5C File Offset: 0x00107D5C
		public override ECPoint Twice()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			ECCurve curve = this.Curve;
			SM2P256V1FieldElement sm2P256V1FieldElement = (SM2P256V1FieldElement)base.RawYCoord;
			if (sm2P256V1FieldElement.IsZero)
			{
				return curve.Infinity;
			}
			SM2P256V1FieldElement sm2P256V1FieldElement2 = (SM2P256V1FieldElement)base.RawXCoord;
			SM2P256V1FieldElement sm2P256V1FieldElement3 = (SM2P256V1FieldElement)base.RawZCoords[0];
			uint[] array = Nat256.Create();
			uint[] array2 = Nat256.Create();
			uint[] array3 = Nat256.Create();
			SM2P256V1Field.Square(sm2P256V1FieldElement.x, array3);
			uint[] array4 = Nat256.Create();
			SM2P256V1Field.Square(array3, array4);
			bool isOne = sm2P256V1FieldElement3.IsOne;
			uint[] array5 = sm2P256V1FieldElement3.x;
			if (!isOne)
			{
				array5 = array2;
				SM2P256V1Field.Square(sm2P256V1FieldElement3.x, array5);
			}
			SM2P256V1Field.Subtract(sm2P256V1FieldElement2.x, array5, array);
			uint[] array6 = array2;
			SM2P256V1Field.Add(sm2P256V1FieldElement2.x, array5, array6);
			SM2P256V1Field.Multiply(array6, array, array6);
			SM2P256V1Field.Reduce32(Nat256.AddBothTo(array6, array6, array6), array6);
			uint[] array7 = array3;
			SM2P256V1Field.Multiply(array3, sm2P256V1FieldElement2.x, array7);
			SM2P256V1Field.Reduce32(Nat.ShiftUpBits(8, array7, 2, 0U), array7);
			SM2P256V1Field.Reduce32(Nat.ShiftUpBits(8, array4, 3, 0U, array), array);
			SM2P256V1FieldElement sm2P256V1FieldElement4 = new SM2P256V1FieldElement(array4);
			SM2P256V1Field.Square(array6, sm2P256V1FieldElement4.x);
			SM2P256V1Field.Subtract(sm2P256V1FieldElement4.x, array7, sm2P256V1FieldElement4.x);
			SM2P256V1Field.Subtract(sm2P256V1FieldElement4.x, array7, sm2P256V1FieldElement4.x);
			SM2P256V1FieldElement sm2P256V1FieldElement5 = new SM2P256V1FieldElement(array7);
			SM2P256V1Field.Subtract(array7, sm2P256V1FieldElement4.x, sm2P256V1FieldElement5.x);
			SM2P256V1Field.Multiply(sm2P256V1FieldElement5.x, array6, sm2P256V1FieldElement5.x);
			SM2P256V1Field.Subtract(sm2P256V1FieldElement5.x, array, sm2P256V1FieldElement5.x);
			SM2P256V1FieldElement sm2P256V1FieldElement6 = new SM2P256V1FieldElement(array6);
			SM2P256V1Field.Twice(sm2P256V1FieldElement.x, sm2P256V1FieldElement6.x);
			if (!isOne)
			{
				SM2P256V1Field.Multiply(sm2P256V1FieldElement6.x, sm2P256V1FieldElement3.x, sm2P256V1FieldElement6.x);
			}
			return new SM2P256V1Point(curve, sm2P256V1FieldElement4, sm2P256V1FieldElement5, new ECFieldElement[]
			{
				sm2P256V1FieldElement6
			}, base.IsCompressed);
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x00109D54 File Offset: 0x00107F54
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

		// Token: 0x06002733 RID: 10035 RVA: 0x000F13B4 File Offset: 0x000EF5B4
		public override ECPoint ThreeTimes()
		{
			if (base.IsInfinity || base.RawYCoord.IsZero)
			{
				return this;
			}
			return this.Twice().Add(this);
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x00109DA0 File Offset: 0x00107FA0
		public override ECPoint Negate()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			return new SM2P256V1Point(this.Curve, base.RawXCoord, base.RawYCoord.Negate(), base.RawZCoords, base.IsCompressed);
		}
	}
}
