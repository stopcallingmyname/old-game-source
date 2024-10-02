using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x0200033C RID: 828
	public class FixedPointCombMultiplier : AbstractECMultiplier
	{
		// Token: 0x06002040 RID: 8256 RVA: 0x000EF558 File Offset: 0x000ED758
		protected override ECPoint MultiplyPositive(ECPoint p, BigInteger k)
		{
			ECCurve curve = p.Curve;
			int combSize = FixedPointUtilities.GetCombSize(curve);
			if (k.BitLength > combSize)
			{
				throw new InvalidOperationException("fixed-point comb doesn't support scalars larger than the curve order");
			}
			FixedPointPreCompInfo fixedPointPreCompInfo = FixedPointUtilities.Precompute(p);
			ECLookupTable lookupTable = fixedPointPreCompInfo.LookupTable;
			int width = fixedPointPreCompInfo.Width;
			int num = (combSize + width - 1) / width;
			ECPoint ecpoint = curve.Infinity;
			int num2 = num * width;
			uint[] array = Nat.FromBigInteger(num2, k);
			int num3 = num2 - 1;
			for (int i = 0; i < num; i++)
			{
				uint num4 = 0U;
				for (int j = num3 - i; j >= 0; j -= num)
				{
					uint num5 = array[j >> 5] >> j;
					num4 ^= num5 >> 1;
					num4 <<= 1;
					num4 ^= num5;
				}
				ECPoint b = lookupTable.Lookup((int)num4);
				ecpoint = ecpoint.TwicePlus(b);
			}
			return ecpoint.Add(fixedPointPreCompInfo.Offset);
		}
	}
}
