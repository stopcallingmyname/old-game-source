using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x0200023B RID: 571
	public interface IX509Extension
	{
		// Token: 0x060014A2 RID: 5282
		ISet GetCriticalExtensionOids();

		// Token: 0x060014A3 RID: 5283
		ISet GetNonCriticalExtensionOids();

		// Token: 0x060014A4 RID: 5284
		[Obsolete("Use version taking a DerObjectIdentifier instead")]
		Asn1OctetString GetExtensionValue(string oid);

		// Token: 0x060014A5 RID: 5285
		Asn1OctetString GetExtensionValue(DerObjectIdentifier oid);
	}
}
