using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003E0 RID: 992
	public interface ISignatureFactory
	{
		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06002861 RID: 10337
		object AlgorithmDetails { get; }

		// Token: 0x06002862 RID: 10338
		IStreamCalculator CreateCalculator();
	}
}
