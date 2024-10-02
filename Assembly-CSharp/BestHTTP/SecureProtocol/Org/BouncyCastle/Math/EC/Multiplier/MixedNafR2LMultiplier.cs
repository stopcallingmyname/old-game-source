using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x02000341 RID: 833
	public class MixedNafR2LMultiplier : AbstractECMultiplier
	{
		// Token: 0x06002051 RID: 8273 RVA: 0x000EF77D File Offset: 0x000ED97D
		public MixedNafR2LMultiplier() : this(2, 4)
		{
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x000EF787 File Offset: 0x000ED987
		public MixedNafR2LMultiplier(int additionCoord, int doublingCoord)
		{
			this.additionCoord = additionCoord;
			this.doublingCoord = doublingCoord;
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x000EF7A0 File Offset: 0x000ED9A0
		protected override ECPoint MultiplyPositive(ECPoint p, BigInteger k)
		{
			ECCurve curve = p.Curve;
			ECCurve eccurve = this.ConfigureCurve(curve, this.additionCoord);
			ECCurve eccurve2 = this.ConfigureCurve(curve, this.doublingCoord);
			int[] array = WNafUtilities.GenerateCompactNaf(k);
			ECPoint ecpoint = eccurve.Infinity;
			ECPoint ecpoint2 = eccurve2.ImportPoint(p);
			int num = 0;
			foreach (int num2 in array)
			{
				int num3 = num2 >> 16;
				num += (num2 & 65535);
				ecpoint2 = ecpoint2.TimesPow2(num);
				ECPoint ecpoint3 = eccurve.ImportPoint(ecpoint2);
				if (num3 < 0)
				{
					ecpoint3 = ecpoint3.Negate();
				}
				ecpoint = ecpoint.Add(ecpoint3);
				num = 1;
			}
			return curve.ImportPoint(ecpoint);
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x000EF848 File Offset: 0x000EDA48
		protected virtual ECCurve ConfigureCurve(ECCurve c, int coord)
		{
			if (c.CoordinateSystem == coord)
			{
				return c;
			}
			if (!c.SupportsCoordinateSystem(coord))
			{
				throw new ArgumentException("Coordinate system " + coord + " not supported by this curve", "coord");
			}
			return c.Configure().SetCoordinateSystem(coord).Create();
		}

		// Token: 0x040019E1 RID: 6625
		protected readonly int additionCoord;

		// Token: 0x040019E2 RID: 6626
		protected readonly int doublingCoord;
	}
}
