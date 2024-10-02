using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Djb
{
	// Token: 0x020003BE RID: 958
	internal class Curve25519Point : AbstractFpPoint
	{
		// Token: 0x0600276C RID: 10092 RVA: 0x0010A761 File Offset: 0x00108961
		public Curve25519Point(ECCurve curve, ECFieldElement x, ECFieldElement y) : this(curve, x, y, false)
		{
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000E45C8 File Offset: 0x000E27C8
		public Curve25519Point(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
		{
			if (x == null != (y == null))
			{
				throw new ArgumentException("Exactly one of the field elements is null");
			}
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000E45EA File Offset: 0x000E27EA
		internal Curve25519Point(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
		{
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x0010A76D File Offset: 0x0010896D
		protected override ECPoint Detach()
		{
			return new Curve25519Point(null, this.AffineXCoord, this.AffineYCoord);
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x0010A781 File Offset: 0x00108981
		public override ECFieldElement GetZCoord(int index)
		{
			if (index == 1)
			{
				return this.GetJacobianModifiedW();
			}
			return base.GetZCoord(index);
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x0010A798 File Offset: 0x00108998
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
			Curve25519FieldElement curve25519FieldElement = (Curve25519FieldElement)base.RawXCoord;
			Curve25519FieldElement curve25519FieldElement2 = (Curve25519FieldElement)base.RawYCoord;
			Curve25519FieldElement curve25519FieldElement3 = (Curve25519FieldElement)base.RawZCoords[0];
			Curve25519FieldElement curve25519FieldElement4 = (Curve25519FieldElement)b.RawXCoord;
			Curve25519FieldElement curve25519FieldElement5 = (Curve25519FieldElement)b.RawYCoord;
			Curve25519FieldElement curve25519FieldElement6 = (Curve25519FieldElement)b.RawZCoords[0];
			uint[] array = Nat256.CreateExt();
			uint[] array2 = Nat256.Create();
			uint[] array3 = Nat256.Create();
			uint[] array4 = Nat256.Create();
			bool isOne = curve25519FieldElement3.IsOne;
			uint[] array5;
			uint[] array6;
			if (isOne)
			{
				array5 = curve25519FieldElement4.x;
				array6 = curve25519FieldElement5.x;
			}
			else
			{
				array6 = array3;
				Curve25519Field.Square(curve25519FieldElement3.x, array6);
				array5 = array2;
				Curve25519Field.Multiply(array6, curve25519FieldElement4.x, array5);
				Curve25519Field.Multiply(array6, curve25519FieldElement3.x, array6);
				Curve25519Field.Multiply(array6, curve25519FieldElement5.x, array6);
			}
			bool isOne2 = curve25519FieldElement6.IsOne;
			uint[] array7;
			uint[] array8;
			if (isOne2)
			{
				array7 = curve25519FieldElement.x;
				array8 = curve25519FieldElement2.x;
			}
			else
			{
				array8 = array4;
				Curve25519Field.Square(curve25519FieldElement6.x, array8);
				array7 = array;
				Curve25519Field.Multiply(array8, curve25519FieldElement.x, array7);
				Curve25519Field.Multiply(array8, curve25519FieldElement6.x, array8);
				Curve25519Field.Multiply(array8, curve25519FieldElement2.x, array8);
			}
			uint[] array9 = Nat256.Create();
			Curve25519Field.Subtract(array7, array5, array9);
			uint[] array10 = array2;
			Curve25519Field.Subtract(array8, array6, array10);
			if (!Nat256.IsZero(array9))
			{
				uint[] array11 = Nat256.Create();
				Curve25519Field.Square(array9, array11);
				uint[] array12 = Nat256.Create();
				Curve25519Field.Multiply(array11, array9, array12);
				uint[] array13 = array3;
				Curve25519Field.Multiply(array11, array7, array13);
				Curve25519Field.Negate(array12, array12);
				Nat256.Mul(array8, array12, array);
				Curve25519Field.Reduce27(Nat256.AddBothTo(array13, array13, array12), array12);
				Curve25519FieldElement curve25519FieldElement7 = new Curve25519FieldElement(array4);
				Curve25519Field.Square(array10, curve25519FieldElement7.x);
				Curve25519Field.Subtract(curve25519FieldElement7.x, array12, curve25519FieldElement7.x);
				Curve25519FieldElement curve25519FieldElement8 = new Curve25519FieldElement(array12);
				Curve25519Field.Subtract(array13, curve25519FieldElement7.x, curve25519FieldElement8.x);
				Curve25519Field.MultiplyAddToExt(curve25519FieldElement8.x, array10, array);
				Curve25519Field.Reduce(array, curve25519FieldElement8.x);
				Curve25519FieldElement curve25519FieldElement9 = new Curve25519FieldElement(array9);
				if (!isOne)
				{
					Curve25519Field.Multiply(curve25519FieldElement9.x, curve25519FieldElement3.x, curve25519FieldElement9.x);
				}
				if (!isOne2)
				{
					Curve25519Field.Multiply(curve25519FieldElement9.x, curve25519FieldElement6.x, curve25519FieldElement9.x);
				}
				uint[] zsquared = (isOne && isOne2) ? array11 : null;
				Curve25519FieldElement curve25519FieldElement10 = this.CalculateJacobianModifiedW(curve25519FieldElement9, zsquared);
				ECFieldElement[] zs = new ECFieldElement[]
				{
					curve25519FieldElement9,
					curve25519FieldElement10
				};
				return new Curve25519Point(curve, curve25519FieldElement7, curve25519FieldElement8, zs, base.IsCompressed);
			}
			if (Nat256.IsZero(array10))
			{
				return this.Twice();
			}
			return curve.Infinity;
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x0010AA80 File Offset: 0x00108C80
		public override ECPoint Twice()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			ECCurve curve = this.Curve;
			if (base.RawYCoord.IsZero)
			{
				return curve.Infinity;
			}
			return this.TwiceJacobianModified(true);
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x0010AABC File Offset: 0x00108CBC
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
			return this.TwiceJacobianModified(false).Add(b);
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x0010AB09 File Offset: 0x00108D09
		public override ECPoint ThreeTimes()
		{
			if (base.IsInfinity || base.RawYCoord.IsZero)
			{
				return this;
			}
			return this.TwiceJacobianModified(false).Add(this);
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x0010AB2F File Offset: 0x00108D2F
		public override ECPoint Negate()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			return new Curve25519Point(this.Curve, base.RawXCoord, base.RawYCoord.Negate(), base.RawZCoords, base.IsCompressed);
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x0010AB64 File Offset: 0x00108D64
		protected virtual Curve25519FieldElement CalculateJacobianModifiedW(Curve25519FieldElement Z, uint[] ZSquared)
		{
			Curve25519FieldElement curve25519FieldElement = (Curve25519FieldElement)this.Curve.A;
			if (Z.IsOne)
			{
				return curve25519FieldElement;
			}
			Curve25519FieldElement curve25519FieldElement2 = new Curve25519FieldElement();
			if (ZSquared == null)
			{
				ZSquared = curve25519FieldElement2.x;
				Curve25519Field.Square(Z.x, ZSquared);
			}
			Curve25519Field.Square(ZSquared, curve25519FieldElement2.x);
			Curve25519Field.Multiply(curve25519FieldElement2.x, curve25519FieldElement.x, curve25519FieldElement2.x);
			return curve25519FieldElement2;
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x0010ABD0 File Offset: 0x00108DD0
		protected virtual Curve25519FieldElement GetJacobianModifiedW()
		{
			ECFieldElement[] rawZCoords = base.RawZCoords;
			Curve25519FieldElement curve25519FieldElement = (Curve25519FieldElement)rawZCoords[1];
			if (curve25519FieldElement == null)
			{
				curve25519FieldElement = (rawZCoords[1] = this.CalculateJacobianModifiedW((Curve25519FieldElement)rawZCoords[0], null));
			}
			return curve25519FieldElement;
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x0010AC08 File Offset: 0x00108E08
		protected virtual Curve25519Point TwiceJacobianModified(bool calculateW)
		{
			Curve25519FieldElement curve25519FieldElement = (Curve25519FieldElement)base.RawXCoord;
			Curve25519FieldElement curve25519FieldElement2 = (Curve25519FieldElement)base.RawYCoord;
			Curve25519FieldElement curve25519FieldElement3 = (Curve25519FieldElement)base.RawZCoords[0];
			Curve25519FieldElement jacobianModifiedW = this.GetJacobianModifiedW();
			uint[] array = Nat256.Create();
			Curve25519Field.Square(curve25519FieldElement.x, array);
			Curve25519Field.Reduce27(Nat256.AddBothTo(array, array, array) + Nat256.AddTo(jacobianModifiedW.x, array), array);
			uint[] array2 = Nat256.Create();
			Curve25519Field.Twice(curve25519FieldElement2.x, array2);
			uint[] array3 = Nat256.Create();
			Curve25519Field.Multiply(array2, curve25519FieldElement2.x, array3);
			uint[] array4 = Nat256.Create();
			Curve25519Field.Multiply(array3, curve25519FieldElement.x, array4);
			Curve25519Field.Twice(array4, array4);
			uint[] array5 = Nat256.Create();
			Curve25519Field.Square(array3, array5);
			Curve25519Field.Twice(array5, array5);
			Curve25519FieldElement curve25519FieldElement4 = new Curve25519FieldElement(array3);
			Curve25519Field.Square(array, curve25519FieldElement4.x);
			Curve25519Field.Subtract(curve25519FieldElement4.x, array4, curve25519FieldElement4.x);
			Curve25519Field.Subtract(curve25519FieldElement4.x, array4, curve25519FieldElement4.x);
			Curve25519FieldElement curve25519FieldElement5 = new Curve25519FieldElement(array4);
			Curve25519Field.Subtract(array4, curve25519FieldElement4.x, curve25519FieldElement5.x);
			Curve25519Field.Multiply(curve25519FieldElement5.x, array, curve25519FieldElement5.x);
			Curve25519Field.Subtract(curve25519FieldElement5.x, array5, curve25519FieldElement5.x);
			Curve25519FieldElement curve25519FieldElement6 = new Curve25519FieldElement(array2);
			if (!Nat256.IsOne(curve25519FieldElement3.x))
			{
				Curve25519Field.Multiply(curve25519FieldElement6.x, curve25519FieldElement3.x, curve25519FieldElement6.x);
			}
			Curve25519FieldElement curve25519FieldElement7 = null;
			if (calculateW)
			{
				curve25519FieldElement7 = new Curve25519FieldElement(array5);
				Curve25519Field.Multiply(curve25519FieldElement7.x, jacobianModifiedW.x, curve25519FieldElement7.x);
				Curve25519Field.Twice(curve25519FieldElement7.x, curve25519FieldElement7.x);
			}
			return new Curve25519Point(this.Curve, curve25519FieldElement4, curve25519FieldElement5, new ECFieldElement[]
			{
				curve25519FieldElement6,
				curve25519FieldElement7
			}, base.IsCompressed);
		}
	}
}
