using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x0200032C RID: 812
	public abstract class AbstractF2mPoint : ECPointBase
	{
		// Token: 0x06001F1A RID: 7962 RVA: 0x000E4468 File Offset: 0x000E2668
		protected AbstractF2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
		{
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x000E4475 File Offset: 0x000E2675
		protected AbstractF2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
		{
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x000E55F4 File Offset: 0x000E37F4
		protected override bool SatisfiesCurveEquation()
		{
			ECCurve curve = this.Curve;
			ECFieldElement rawXCoord = base.RawXCoord;
			ECFieldElement rawYCoord = base.RawYCoord;
			ECFieldElement ecfieldElement = curve.A;
			ECFieldElement ecfieldElement2 = curve.B;
			int coordinateSystem = curve.CoordinateSystem;
			ECFieldElement ecfieldElement4;
			ECFieldElement ecfieldElement5;
			if (coordinateSystem == 6)
			{
				ECFieldElement ecfieldElement3 = base.RawZCoords[0];
				bool isOne = ecfieldElement3.IsOne;
				if (rawXCoord.IsZero)
				{
					ecfieldElement4 = rawYCoord.Square();
					ecfieldElement5 = ecfieldElement2;
					if (!isOne)
					{
						ECFieldElement b = ecfieldElement3.Square();
						ecfieldElement5 = ecfieldElement5.Multiply(b);
					}
				}
				else
				{
					ECFieldElement ecfieldElement6 = rawYCoord;
					ECFieldElement ecfieldElement7 = rawXCoord.Square();
					if (isOne)
					{
						ecfieldElement4 = ecfieldElement6.Square().Add(ecfieldElement6).Add(ecfieldElement);
						ecfieldElement5 = ecfieldElement7.Square().Add(ecfieldElement2);
					}
					else
					{
						ECFieldElement ecfieldElement8 = ecfieldElement3.Square();
						ECFieldElement y = ecfieldElement8.Square();
						ecfieldElement4 = ecfieldElement6.Add(ecfieldElement3).MultiplyPlusProduct(ecfieldElement6, ecfieldElement, ecfieldElement8);
						ecfieldElement5 = ecfieldElement7.SquarePlusProduct(ecfieldElement2, y);
					}
					ecfieldElement4 = ecfieldElement4.Multiply(ecfieldElement7);
				}
			}
			else
			{
				ecfieldElement4 = rawYCoord.Add(rawXCoord).Multiply(rawYCoord);
				if (coordinateSystem != 0)
				{
					if (coordinateSystem != 1)
					{
						throw new InvalidOperationException("unsupported coordinate system");
					}
					ECFieldElement ecfieldElement9 = base.RawZCoords[0];
					if (!ecfieldElement9.IsOne)
					{
						ECFieldElement b2 = ecfieldElement9.Square();
						ECFieldElement b3 = ecfieldElement9.Multiply(b2);
						ecfieldElement4 = ecfieldElement4.Multiply(ecfieldElement9);
						ecfieldElement = ecfieldElement.Multiply(ecfieldElement9);
						ecfieldElement2 = ecfieldElement2.Multiply(b3);
					}
				}
				ecfieldElement5 = rawXCoord.Add(ecfieldElement).Multiply(rawXCoord.Square()).Add(ecfieldElement2);
			}
			return ecfieldElement4.Equals(ecfieldElement5);
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x000E5780 File Offset: 0x000E3980
		protected override bool SatisfiesOrder()
		{
			ECCurve curve = this.Curve;
			BigInteger cofactor = curve.Cofactor;
			if (BigInteger.Two.Equals(cofactor))
			{
				return ((AbstractF2mFieldElement)this.Normalize().AffineXCoord.Add(curve.A)).Trace() == 0;
			}
			if (!BigInteger.ValueOf(4L).Equals(cofactor))
			{
				return base.SatisfiesOrder();
			}
			ECPoint ecpoint = this.Normalize();
			ECFieldElement affineXCoord = ecpoint.AffineXCoord;
			ECFieldElement ecfieldElement = ((AbstractF2mCurve)curve).SolveQuadraticEquation(affineXCoord.Add(curve.A));
			if (ecfieldElement == null)
			{
				return false;
			}
			ECFieldElement ecfieldElement2 = affineXCoord.Multiply(ecfieldElement).Add(ecpoint.AffineYCoord).Add(curve.A);
			return ((AbstractF2mFieldElement)ecfieldElement2).Trace() == 0 || ((AbstractF2mFieldElement)ecfieldElement2.Add(affineXCoord)).Trace() == 0;
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x000E5854 File Offset: 0x000E3A54
		public override ECPoint ScaleX(ECFieldElement scale)
		{
			if (base.IsInfinity)
			{
				return this;
			}
			int curveCoordinateSystem = this.CurveCoordinateSystem;
			if (curveCoordinateSystem == 5)
			{
				ECFieldElement rawXCoord = base.RawXCoord;
				ECFieldElement rawYCoord = base.RawYCoord;
				ECFieldElement b = rawXCoord.Multiply(scale);
				ECFieldElement y = rawYCoord.Add(rawXCoord).Divide(scale).Add(b);
				return this.Curve.CreateRawPoint(rawXCoord, y, base.RawZCoords, base.IsCompressed);
			}
			if (curveCoordinateSystem != 6)
			{
				return base.ScaleX(scale);
			}
			ECFieldElement rawXCoord2 = base.RawXCoord;
			ECFieldElement rawYCoord2 = base.RawYCoord;
			ECFieldElement ecfieldElement = base.RawZCoords[0];
			ECFieldElement b2 = rawXCoord2.Multiply(scale.Square());
			ECFieldElement y2 = rawYCoord2.Add(rawXCoord2).Add(b2);
			ECFieldElement ecfieldElement2 = ecfieldElement.Multiply(scale);
			return this.Curve.CreateRawPoint(rawXCoord2, y2, new ECFieldElement[]
			{
				ecfieldElement2
			}, base.IsCompressed);
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x000E592C File Offset: 0x000E3B2C
		public override ECPoint ScaleY(ECFieldElement scale)
		{
			if (base.IsInfinity)
			{
				return this;
			}
			int curveCoordinateSystem = this.CurveCoordinateSystem;
			if (curveCoordinateSystem - 5 <= 1)
			{
				ECFieldElement rawXCoord = base.RawXCoord;
				ECFieldElement y = base.RawYCoord.Add(rawXCoord).Multiply(scale).Add(rawXCoord);
				return this.Curve.CreateRawPoint(rawXCoord, y, base.RawZCoords, base.IsCompressed);
			}
			return base.ScaleY(scale);
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x000E45A4 File Offset: 0x000E27A4
		public override ECPoint Subtract(ECPoint b)
		{
			if (b.IsInfinity)
			{
				return this;
			}
			return this.Add(b.Negate());
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x000E5994 File Offset: 0x000E3B94
		public virtual AbstractF2mPoint Tau()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			ECCurve curve = this.Curve;
			int coordinateSystem = curve.CoordinateSystem;
			ECFieldElement rawXCoord = base.RawXCoord;
			switch (coordinateSystem)
			{
			case 0:
			case 5:
			{
				ECFieldElement rawYCoord = base.RawYCoord;
				return (AbstractF2mPoint)curve.CreateRawPoint(rawXCoord.Square(), rawYCoord.Square(), base.IsCompressed);
			}
			case 1:
			case 6:
			{
				ECFieldElement rawYCoord2 = base.RawYCoord;
				ECFieldElement ecfieldElement = base.RawZCoords[0];
				return (AbstractF2mPoint)curve.CreateRawPoint(rawXCoord.Square(), rawYCoord2.Square(), new ECFieldElement[]
				{
					ecfieldElement.Square()
				}, base.IsCompressed);
			}
			}
			throw new InvalidOperationException("unsupported coordinate system");
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x000E5A54 File Offset: 0x000E3C54
		public virtual AbstractF2mPoint TauPow(int pow)
		{
			if (base.IsInfinity)
			{
				return this;
			}
			ECCurve curve = this.Curve;
			int coordinateSystem = curve.CoordinateSystem;
			ECFieldElement rawXCoord = base.RawXCoord;
			switch (coordinateSystem)
			{
			case 0:
			case 5:
			{
				ECFieldElement rawYCoord = base.RawYCoord;
				return (AbstractF2mPoint)curve.CreateRawPoint(rawXCoord.SquarePow(pow), rawYCoord.SquarePow(pow), base.IsCompressed);
			}
			case 1:
			case 6:
			{
				ECFieldElement rawYCoord2 = base.RawYCoord;
				ECFieldElement ecfieldElement = base.RawZCoords[0];
				return (AbstractF2mPoint)curve.CreateRawPoint(rawXCoord.SquarePow(pow), rawYCoord2.SquarePow(pow), new ECFieldElement[]
				{
					ecfieldElement.SquarePow(pow)
				}, base.IsCompressed);
			}
			}
			throw new InvalidOperationException("unsupported coordinate system");
		}
	}
}
