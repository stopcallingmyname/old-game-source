using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000607 RID: 1543
	public class DefaultAuthenticatedAttributeTableGenerator : CmsAttributeTableGenerator
	{
		// Token: 0x06003AA7 RID: 15015 RVA: 0x0016B9F5 File Offset: 0x00169BF5
		public DefaultAuthenticatedAttributeTableGenerator()
		{
			this.table = Platform.CreateHashtable();
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x0016BA08 File Offset: 0x00169C08
		public DefaultAuthenticatedAttributeTableGenerator(AttributeTable attributeTable)
		{
			if (attributeTable != null)
			{
				this.table = attributeTable.ToDictionary();
				return;
			}
			this.table = Platform.CreateHashtable();
		}

		// Token: 0x06003AA9 RID: 15017 RVA: 0x0016BA2C File Offset: 0x00169C2C
		protected virtual IDictionary CreateStandardAttributeTable(IDictionary parameters)
		{
			IDictionary dictionary = Platform.CreateHashtable(this.table);
			if (!dictionary.Contains(CmsAttributes.ContentType))
			{
				DerObjectIdentifier obj = (DerObjectIdentifier)parameters[CmsAttributeTableParameter.ContentType];
				Attribute attribute = new Attribute(CmsAttributes.ContentType, new DerSet(obj));
				dictionary[attribute.AttrType] = attribute;
			}
			if (!dictionary.Contains(CmsAttributes.MessageDigest))
			{
				byte[] str = (byte[])parameters[CmsAttributeTableParameter.Digest];
				Attribute attribute2 = new Attribute(CmsAttributes.MessageDigest, new DerSet(new DerOctetString(str)));
				dictionary[attribute2.AttrType] = attribute2;
			}
			return dictionary;
		}

		// Token: 0x06003AAA RID: 15018 RVA: 0x0016BAC8 File Offset: 0x00169CC8
		public virtual AttributeTable GetAttributes(IDictionary parameters)
		{
			return new AttributeTable(this.CreateStandardAttributeTable(parameters));
		}

		// Token: 0x04002656 RID: 9814
		private readonly IDictionary table;
	}
}
