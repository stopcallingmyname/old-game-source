using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x0200033B RID: 827
	public interface ECMultiplier
	{
		// Token: 0x0600203F RID: 8255
		ECPoint Multiply(ECPoint p, BigInteger k);
	}
}
