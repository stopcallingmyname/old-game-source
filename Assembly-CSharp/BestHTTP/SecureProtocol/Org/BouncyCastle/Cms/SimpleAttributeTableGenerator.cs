using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000621 RID: 1569
	public class SimpleAttributeTableGenerator : CmsAttributeTableGenerator
	{
		// Token: 0x06003B3C RID: 15164 RVA: 0x0016E1BA File Offset: 0x0016C3BA
		public SimpleAttributeTableGenerator(AttributeTable attributes)
		{
			this.attributes = attributes;
		}

		// Token: 0x06003B3D RID: 15165 RVA: 0x0016E1C9 File Offset: 0x0016C3C9
		public virtual AttributeTable GetAttributes(IDictionary parameters)
		{
			return this.attributes;
		}

		// Token: 0x04002694 RID: 9876
		private readonly AttributeTable attributes;
	}
}
