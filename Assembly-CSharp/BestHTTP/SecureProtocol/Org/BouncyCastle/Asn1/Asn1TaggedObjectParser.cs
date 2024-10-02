using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000633 RID: 1587
	public interface Asn1TaggedObjectParser : IAsn1Convertible
	{
		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06003BB7 RID: 15287
		int TagNo { get; }

		// Token: 0x06003BB8 RID: 15288
		IAsn1Convertible GetObjectParser(int tag, bool isExplicit);
	}
}
