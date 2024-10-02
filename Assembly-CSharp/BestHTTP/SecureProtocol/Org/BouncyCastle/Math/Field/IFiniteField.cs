using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Field
{
	// Token: 0x02000318 RID: 792
	public interface IFiniteField
	{
		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06001DFA RID: 7674
		BigInteger Characteristic { get; }

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06001DFB RID: 7675
		int Dimension { get; }
	}
}
