﻿using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000392 RID: 914
	internal class SecT163R1Point : AbstractF2mPoint
	{
		// Token: 0x0600247B RID: 9339 RVA: 0x000FF0C6 File Offset: 0x000FD2C6
		public SecT163R1Point(ECCurve curve, ECFieldElement x, ECFieldElement y) : this(curve, x, y, false)
		{
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x000FB6CA File Offset: 0x000F98CA
		public SecT163R1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
		{
			if (x == null != (y == null))
			{
				throw new ArgumentException("Exactly one of the field elements is null");
			}
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x000E5B60 File Offset: 0x000E3D60
		internal SecT163R1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
		{
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x000FF0D2 File Offset: 0x000FD2D2
		protected override ECPoint Detach()
		{
			return new SecT163R1Point(null, this.AffineXCoord, this.AffineYCoord);
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x0600247F RID: 9343 RVA: 0x000FF0E8 File Offset: 0x000FD2E8
		public override ECFieldElement YCoord
		{
			get
			{
				ECFieldElement rawXCoord = base.RawXCoord;
				ECFieldElement rawYCoord = base.RawYCoord;
				if (base.IsInfinity || rawXCoord.IsZero)
				{
					return rawYCoord;
				}
				ECFieldElement ecfieldElement = rawYCoord.Add(rawXCoord).Multiply(rawXCoord);
				ECFieldElement ecfieldElement2 = base.RawZCoords[0];
				if (!ecfieldElement2.IsOne)
				{
					ecfieldElement = ecfieldElement.Divide(ecfieldElement2);
				}
				return ecfieldElement;
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06002480 RID: 9344 RVA: 0x000FF140 File Offset: 0x000FD340
		protected internal override bool CompressionYTilde
		{
			get
			{
				ECFieldElement rawXCoord = base.RawXCoord;
				return !rawXCoord.IsZero && base.RawYCoord.TestBitZero() != rawXCoord.TestBitZero();
			}
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x000FF174 File Offset: 0x000FD374
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
			ECCurve curve = this.Curve;
			ECFieldElement ecfieldElement = base.RawXCoord;
			ECFieldElement rawXCoord = b.RawXCoord;
			if (ecfieldElement.IsZero)
			{
				if (rawXCoord.IsZero)
				{
					return curve.Infinity;
				}
				return b.Add(this);
			}
			else
			{
				ECFieldElement rawYCoord = base.RawYCoord;
				ECFieldElement ecfieldElement2 = base.RawZCoords[0];
				ECFieldElement rawYCoord2 = b.RawYCoord;
				ECFieldElement ecfieldElement3 = b.RawZCoords[0];
				bool isOne = ecfieldElement2.IsOne;
				ECFieldElement ecfieldElement4 = rawXCoord;
				ECFieldElement ecfieldElement5 = rawYCoord2;
				if (!isOne)
				{
					ecfieldElement4 = ecfieldElement4.Multiply(ecfieldElement2);
					ecfieldElement5 = ecfieldElement5.Multiply(ecfieldElement2);
				}
				bool isOne2 = ecfieldElement3.IsOne;
				ECFieldElement ecfieldElement6 = ecfieldElement;
				ECFieldElement ecfieldElement7 = rawYCoord;
				if (!isOne2)
				{
					ecfieldElement6 = ecfieldElement6.Multiply(ecfieldElement3);
					ecfieldElement7 = ecfieldElement7.Multiply(ecfieldElement3);
				}
				ECFieldElement ecfieldElement8 = ecfieldElement7.Add(ecfieldElement5);
				ECFieldElement ecfieldElement9 = ecfieldElement6.Add(ecfieldElement4);
				if (!ecfieldElement9.IsZero)
				{
					ECFieldElement ecfieldElement11;
					ECFieldElement y;
					ECFieldElement ecfieldElement12;
					if (rawXCoord.IsZero)
					{
						ECPoint ecpoint = this.Normalize();
						ecfieldElement = ecpoint.XCoord;
						ECFieldElement ycoord = ecpoint.YCoord;
						ECFieldElement b2 = rawYCoord2;
						ECFieldElement ecfieldElement10 = ycoord.Add(b2).Divide(ecfieldElement);
						ecfieldElement11 = ecfieldElement10.Square().Add(ecfieldElement10).Add(ecfieldElement).Add(curve.A);
						if (ecfieldElement11.IsZero)
						{
							return new SecT163R1Point(curve, ecfieldElement11, curve.B.Sqrt(), base.IsCompressed);
						}
						y = ecfieldElement10.Multiply(ecfieldElement.Add(ecfieldElement11)).Add(ecfieldElement11).Add(ycoord).Divide(ecfieldElement11).Add(ecfieldElement11);
						ecfieldElement12 = curve.FromBigInteger(BigInteger.One);
					}
					else
					{
						ecfieldElement9 = ecfieldElement9.Square();
						ECFieldElement ecfieldElement13 = ecfieldElement8.Multiply(ecfieldElement6);
						ECFieldElement ecfieldElement14 = ecfieldElement8.Multiply(ecfieldElement4);
						ecfieldElement11 = ecfieldElement13.Multiply(ecfieldElement14);
						if (ecfieldElement11.IsZero)
						{
							return new SecT163R1Point(curve, ecfieldElement11, curve.B.Sqrt(), base.IsCompressed);
						}
						ECFieldElement ecfieldElement15 = ecfieldElement8.Multiply(ecfieldElement9);
						if (!isOne2)
						{
							ecfieldElement15 = ecfieldElement15.Multiply(ecfieldElement3);
						}
						y = ecfieldElement14.Add(ecfieldElement9).SquarePlusProduct(ecfieldElement15, rawYCoord.Add(ecfieldElement2));
						ecfieldElement12 = ecfieldElement15;
						if (!isOne)
						{
							ecfieldElement12 = ecfieldElement12.Multiply(ecfieldElement2);
						}
					}
					return new SecT163R1Point(curve, ecfieldElement11, y, new ECFieldElement[]
					{
						ecfieldElement12
					}, base.IsCompressed);
				}
				if (ecfieldElement8.IsZero)
				{
					return this.Twice();
				}
				return curve.Infinity;
			}
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x000FF3DC File Offset: 0x000FD5DC
		public override ECPoint Twice()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			ECCurve curve = this.Curve;
			ECFieldElement rawXCoord = base.RawXCoord;
			if (rawXCoord.IsZero)
			{
				return curve.Infinity;
			}
			ECFieldElement rawYCoord = base.RawYCoord;
			ECFieldElement ecfieldElement = base.RawZCoords[0];
			bool isOne = ecfieldElement.IsOne;
			ECFieldElement ecfieldElement2 = isOne ? rawYCoord : rawYCoord.Multiply(ecfieldElement);
			ECFieldElement b = isOne ? ecfieldElement : ecfieldElement.Square();
			ECFieldElement a = curve.A;
			ECFieldElement b2 = isOne ? a : a.Multiply(b);
			ECFieldElement ecfieldElement3 = rawYCoord.Square().Add(ecfieldElement2).Add(b2);
			if (ecfieldElement3.IsZero)
			{
				return new SecT163R1Point(curve, ecfieldElement3, curve.B.Sqrt(), base.IsCompressed);
			}
			ECFieldElement ecfieldElement4 = ecfieldElement3.Square();
			ECFieldElement ecfieldElement5 = isOne ? ecfieldElement3 : ecfieldElement3.Multiply(b);
			ECFieldElement y = (isOne ? rawXCoord : rawXCoord.Multiply(ecfieldElement)).SquarePlusProduct(ecfieldElement3, ecfieldElement2).Add(ecfieldElement4).Add(ecfieldElement5);
			return new SecT163R1Point(curve, ecfieldElement4, y, new ECFieldElement[]
			{
				ecfieldElement5
			}, base.IsCompressed);
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x000FF4FC File Offset: 0x000FD6FC
		public override ECPoint TwicePlus(ECPoint b)
		{
			if (base.IsInfinity)
			{
				return b;
			}
			if (b.IsInfinity)
			{
				return this.Twice();
			}
			ECCurve curve = this.Curve;
			ECFieldElement rawXCoord = base.RawXCoord;
			if (rawXCoord.IsZero)
			{
				return b;
			}
			ECFieldElement rawXCoord2 = b.RawXCoord;
			ECFieldElement ecfieldElement = b.RawZCoords[0];
			if (rawXCoord2.IsZero || !ecfieldElement.IsOne)
			{
				return this.Twice().Add(b);
			}
			ECFieldElement rawYCoord = base.RawYCoord;
			ECFieldElement ecfieldElement2 = base.RawZCoords[0];
			ECFieldElement rawYCoord2 = b.RawYCoord;
			ECFieldElement x = rawXCoord.Square();
			ECFieldElement b2 = rawYCoord.Square();
			ECFieldElement ecfieldElement3 = ecfieldElement2.Square();
			ECFieldElement b3 = rawYCoord.Multiply(ecfieldElement2);
			ECFieldElement b4 = curve.A.Multiply(ecfieldElement3).Add(b2).Add(b3);
			ECFieldElement ecfieldElement4 = rawYCoord2.AddOne();
			ECFieldElement ecfieldElement5 = curve.A.Add(ecfieldElement4).Multiply(ecfieldElement3).Add(b2).MultiplyPlusProduct(b4, x, ecfieldElement3);
			ECFieldElement ecfieldElement6 = rawXCoord2.Multiply(ecfieldElement3);
			ECFieldElement ecfieldElement7 = ecfieldElement6.Add(b4).Square();
			if (ecfieldElement7.IsZero)
			{
				if (ecfieldElement5.IsZero)
				{
					return b.Twice();
				}
				return curve.Infinity;
			}
			else
			{
				if (ecfieldElement5.IsZero)
				{
					return new SecT163R1Point(curve, ecfieldElement5, curve.B.Sqrt(), base.IsCompressed);
				}
				ECFieldElement x2 = ecfieldElement5.Square().Multiply(ecfieldElement6);
				ECFieldElement ecfieldElement8 = ecfieldElement5.Multiply(ecfieldElement7).Multiply(ecfieldElement3);
				ECFieldElement y = ecfieldElement5.Add(ecfieldElement7).Square().MultiplyPlusProduct(b4, ecfieldElement4, ecfieldElement8);
				return new SecT163R1Point(curve, x2, y, new ECFieldElement[]
				{
					ecfieldElement8
				}, base.IsCompressed);
			}
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x000FF6A4 File Offset: 0x000FD8A4
		public override ECPoint Negate()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			ECFieldElement rawXCoord = base.RawXCoord;
			if (rawXCoord.IsZero)
			{
				return this;
			}
			ECFieldElement rawYCoord = base.RawYCoord;
			ECFieldElement ecfieldElement = base.RawZCoords[0];
			return new SecT163R1Point(this.Curve, rawXCoord, rawYCoord.Add(ecfieldElement), new ECFieldElement[]
			{
				ecfieldElement
			}, base.IsCompressed);
		}
	}
}