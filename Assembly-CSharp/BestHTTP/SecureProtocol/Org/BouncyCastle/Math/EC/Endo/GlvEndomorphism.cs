using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Endo
{
	// Token: 0x02000350 RID: 848
	public interface GlvEndomorphism : ECEndomorphism
	{
		// Token: 0x0600208E RID: 8334
		BigInteger[] DecomposeScalar(BigInteger k);
	}
}
