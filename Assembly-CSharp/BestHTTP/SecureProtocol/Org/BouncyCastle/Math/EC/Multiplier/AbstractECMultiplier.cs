using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x02000339 RID: 825
	public abstract class AbstractECMultiplier : ECMultiplier
	{
		// Token: 0x06002039 RID: 8249 RVA: 0x000EF498 File Offset: 0x000ED698
		public virtual ECPoint Multiply(ECPoint p, BigInteger k)
		{
			int signValue = k.SignValue;
			if (signValue == 0 || p.IsInfinity)
			{
				return p.Curve.Infinity;
			}
			ECPoint ecpoint = this.MultiplyPositive(p, k.Abs());
			ECPoint p2 = (signValue > 0) ? ecpoint : ecpoint.Negate();
			return this.CheckResult(p2);
		}

		// Token: 0x0600203A RID: 8250
		protected abstract ECPoint MultiplyPositive(ECPoint p, BigInteger k);

		// Token: 0x0600203B RID: 8251 RVA: 0x000EF4E6 File Offset: 0x000ED6E6
		protected virtual ECPoint CheckResult(ECPoint p)
		{
			return ECAlgorithms.ImplCheckResult(p);
		}
	}
}
