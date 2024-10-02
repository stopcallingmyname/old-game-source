using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005FB RID: 1531
	internal interface CmsSecureReadable
	{
		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06003A0A RID: 14858
		AlgorithmIdentifier Algorithm { get; }

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06003A0B RID: 14859
		object CryptoObject { get; }

		// Token: 0x06003A0C RID: 14860
		CmsReadable GetReadable(KeyParameter key);
	}
}
