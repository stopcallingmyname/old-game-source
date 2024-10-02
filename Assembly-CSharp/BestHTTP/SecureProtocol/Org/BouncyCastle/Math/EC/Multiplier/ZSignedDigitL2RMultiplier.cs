using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x0200034D RID: 845
	public class ZSignedDigitL2RMultiplier : AbstractECMultiplier
	{
		// Token: 0x06002088 RID: 8328 RVA: 0x000F034C File Offset: 0x000EE54C
		protected override ECPoint MultiplyPositive(ECPoint p, BigInteger k)
		{
			ECPoint ecpoint = p.Normalize();
			ECPoint ecpoint2 = ecpoint.Negate();
			ECPoint ecpoint3 = ecpoint;
			int bitLength = k.BitLength;
			int lowestSetBit = k.GetLowestSetBit();
			int num = bitLength;
			while (--num > lowestSetBit)
			{
				ecpoint3 = ecpoint3.TwicePlus(k.TestBit(num) ? ecpoint : ecpoint2);
			}
			return ecpoint3.TimesPow2(lowestSetBit);
		}
	}
}
