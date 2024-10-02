using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003E3 RID: 995
	public interface IStreamCalculator
	{
		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x0600286D RID: 10349
		Stream Stream { get; }

		// Token: 0x0600286E RID: 10350
		object GetResult();
	}
}
