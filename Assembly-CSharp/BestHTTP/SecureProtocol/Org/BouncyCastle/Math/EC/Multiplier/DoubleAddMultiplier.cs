using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x0200033A RID: 826
	public class DoubleAddMultiplier : AbstractECMultiplier
	{
		// Token: 0x0600203D RID: 8253 RVA: 0x000EF4F0 File Offset: 0x000ED6F0
		protected override ECPoint MultiplyPositive(ECPoint p, BigInteger k)
		{
			ECPoint[] array = new ECPoint[]
			{
				p.Curve.Infinity,
				p
			};
			int bitLength = k.BitLength;
			for (int i = 0; i < bitLength; i++)
			{
				int num = k.TestBit(i) ? 1 : 0;
				int num2 = 1 - num;
				array[num2] = array[num2].TwicePlus(array[num]);
			}
			return array[0];
		}
	}
}
