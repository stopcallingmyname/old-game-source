using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x02000346 RID: 838
	public class ReferenceMultiplier : AbstractECMultiplier
	{
		// Token: 0x0600205B RID: 8283 RVA: 0x000EF9DE File Offset: 0x000EDBDE
		protected override ECPoint MultiplyPositive(ECPoint p, BigInteger k)
		{
			return ECAlgorithms.ReferenceMultiply(p, k);
		}
	}
}
