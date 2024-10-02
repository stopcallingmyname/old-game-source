using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Endo
{
	// Token: 0x0200034F RID: 847
	public interface ECEndomorphism
	{
		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x0600208C RID: 8332
		ECPointMap PointMap { get; }

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x0600208D RID: 8333
		bool HasEfficientPointMap { get; }
	}
}
