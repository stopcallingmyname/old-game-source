using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x0200034E RID: 846
	public class ZSignedDigitR2LMultiplier : AbstractECMultiplier
	{
		// Token: 0x0600208A RID: 8330 RVA: 0x000F03A4 File Offset: 0x000EE5A4
		protected override ECPoint MultiplyPositive(ECPoint p, BigInteger k)
		{
			ECPoint ecpoint = p.Curve.Infinity;
			int bitLength = k.BitLength;
			int lowestSetBit = k.GetLowestSetBit();
			ECPoint ecpoint2 = p.TimesPow2(lowestSetBit);
			int num = lowestSetBit;
			while (++num < bitLength)
			{
				ecpoint = ecpoint.Add(k.TestBit(num) ? ecpoint2 : ecpoint2.Negate());
				ecpoint2 = ecpoint2.Twice();
			}
			return ecpoint.Add(ecpoint2);
		}
	}
}
