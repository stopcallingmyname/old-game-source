using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x02000342 RID: 834
	public class MontgomeryLadderMultiplier : AbstractECMultiplier
	{
		// Token: 0x06002055 RID: 8277 RVA: 0x000EF89C File Offset: 0x000EDA9C
		protected override ECPoint MultiplyPositive(ECPoint p, BigInteger k)
		{
			ECPoint[] array = new ECPoint[]
			{
				p.Curve.Infinity,
				p
			};
			int num = k.BitLength;
			while (--num >= 0)
			{
				int num2 = k.TestBit(num) ? 1 : 0;
				int num3 = 1 - num2;
				array[num3] = array[num3].Add(array[num2]);
				array[num2] = array[num2].Twice();
			}
			return array[0];
		}
	}
}
