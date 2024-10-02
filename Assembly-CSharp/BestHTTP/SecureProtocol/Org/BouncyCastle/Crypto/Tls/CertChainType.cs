using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000401 RID: 1025
	public abstract class CertChainType
	{
		// Token: 0x06002973 RID: 10611 RVA: 0x0010E467 File Offset: 0x0010C667
		public static bool IsValid(byte certChainType)
		{
			return certChainType >= 0 && certChainType <= 1;
		}

		// Token: 0x04001B65 RID: 7013
		public const byte individual_certs = 0;

		// Token: 0x04001B66 RID: 7014
		public const byte pkipath = 1;
	}
}
