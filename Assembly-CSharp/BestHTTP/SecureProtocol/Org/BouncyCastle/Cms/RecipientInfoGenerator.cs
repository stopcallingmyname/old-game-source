using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000618 RID: 1560
	internal interface RecipientInfoGenerator
	{
		// Token: 0x06003AF8 RID: 15096
		RecipientInfo Generate(KeyParameter contentEncryptionKey, SecureRandom random);
	}
}
