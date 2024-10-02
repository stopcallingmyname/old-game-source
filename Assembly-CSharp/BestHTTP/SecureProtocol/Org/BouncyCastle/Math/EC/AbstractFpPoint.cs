using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x0200032A RID: 810
	public abstract class AbstractFpPoint : ECPointBase
	{
		// Token: 0x06001F02 RID: 7938 RVA: 0x000E4468 File Offset: 0x000E2668
		protected AbstractFpPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
		{
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x000E4475 File Offset: 0x000E2675
		protected AbstractFpPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
		{
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001F04 RID: 7940 RVA: 0x000E4484 File Offset: 0x000E2684
		protected internal override bool CompressionYTilde
		{
			get
			{
				return this.AffineYCoord.TestBitZero();
			}
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x000E4494 File Offset: 0x000E2694
		protected override bool SatisfiesCurveEquation()
		{
			ECFieldElement rawXCoord = base.RawXCoord;
			ECFieldElement rawYCoord = base.RawYCoord;
			ECFieldElement ecfieldElement = this.Curve.A;
			ECFieldElement ecfieldElement2 = this.Curve.B;
			ECFieldElement ecfieldElement3 = rawYCoord.Square();
			switch (this.CurveCoordinateSystem)
			{
			case 0:
				break;
			case 1:
			{
				ECFieldElement ecfieldElement4 = base.RawZCoords[0];
				if (!ecfieldElement4.IsOne)
				{
					ECFieldElement b = ecfieldElement4.Square();
					ECFieldElement b2 = ecfieldElement4.Multiply(b);
					ecfieldElement3 = ecfieldElement3.Multiply(ecfieldElement4);
					ecfieldElement = ecfieldElement.Multiply(b);
					ecfieldElement2 = ecfieldElement2.Multiply(b2);
				}
				break;
			}
			case 2:
			case 3:
			case 4:
			{
				ECFieldElement ecfieldElement5 = base.RawZCoords[0];
				if (!ecfieldElement5.IsOne)
				{
					ECFieldElement ecfieldElement6 = ecfieldElement5.Square();
					ECFieldElement b3 = ecfieldElement6.Square();
					ECFieldElement b4 = ecfieldElement6.Multiply(b3);
					ecfieldElement = ecfieldElement.Multiply(b3);
					ecfieldElement2 = ecfieldElement2.Multiply(b4);
				}
				break;
			}
			default:
				throw new InvalidOperationException("unsupported coordinate system");
			}
			ECFieldElement other = rawXCoord.Square().Add(ecfieldElement).Multiply(rawXCoord).Add(ecfieldElement2);
			return ecfieldElement3.Equals(other);
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x000E45A4 File Offset: 0x000E27A4
		public override ECPoint Subtract(ECPoint b)
		{
			if (b.IsInfinity)
			{
				return this;
			}
			return this.Add(b.Negate());
		}
	}
}
