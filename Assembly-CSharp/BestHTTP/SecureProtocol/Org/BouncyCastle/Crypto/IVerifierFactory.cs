using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003E6 RID: 998
	public interface IVerifierFactory
	{
		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06002876 RID: 10358
		object AlgorithmDetails { get; }

		// Token: 0x06002877 RID: 10359
		IStreamCalculator CreateCalculator();
	}
}
