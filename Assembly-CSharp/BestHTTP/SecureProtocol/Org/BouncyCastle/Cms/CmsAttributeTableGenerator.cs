using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005E1 RID: 1505
	public interface CmsAttributeTableGenerator
	{
		// Token: 0x0600397A RID: 14714
		AttributeTable GetAttributes(IDictionary parameters);
	}
}
