using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Field
{
	// Token: 0x02000319 RID: 793
	public interface IPolynomial
	{
		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06001DFC RID: 7676
		int Degree { get; }

		// Token: 0x06001DFD RID: 7677
		int[] GetExponentsPresent();
	}
}
