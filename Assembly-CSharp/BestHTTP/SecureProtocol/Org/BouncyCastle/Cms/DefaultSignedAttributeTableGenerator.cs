using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000608 RID: 1544
	public class DefaultSignedAttributeTableGenerator : CmsAttributeTableGenerator
	{
		// Token: 0x06003AAB RID: 15019 RVA: 0x0016BAD6 File Offset: 0x00169CD6
		public DefaultSignedAttributeTableGenerator()
		{
			this.table = Platform.CreateHashtable();
		}

		// Token: 0x06003AAC RID: 15020 RVA: 0x0016BAE9 File Offset: 0x00169CE9
		public DefaultSignedAttributeTableGenerator(AttributeTable attributeTable)
		{
			if (attributeTable != null)
			{
				this.table = attributeTable.ToDictionary();
				return;
			}
			this.table = Platform.CreateHashtable();
		}

		// Token: 0x06003AAD RID: 15021 RVA: 0x0016BB0C File Offset: 0x00169D0C
		protected virtual Hashtable createStandardAttributeTable(IDictionary parameters)
		{
			Hashtable hashtable = new Hashtable(this.table);
			this.DoCreateStandardAttributeTable(parameters, hashtable);
			return hashtable;
		}

		// Token: 0x06003AAE RID: 15022 RVA: 0x0016BB30 File Offset: 0x00169D30
		private void DoCreateStandardAttributeTable(IDictionary parameters, IDictionary std)
		{
			if (parameters.Contains(CmsAttributeTableParameter.ContentType) && !std.Contains(CmsAttributes.ContentType))
			{
				DerObjectIdentifier obj = (DerObjectIdentifier)parameters[CmsAttributeTableParameter.ContentType];
				Attribute attribute = new Attribute(CmsAttributes.ContentType, new DerSet(obj));
				std[attribute.AttrType] = attribute;
			}
			if (!std.Contains(CmsAttributes.SigningTime))
			{
				Attribute attribute2 = new Attribute(CmsAttributes.SigningTime, new DerSet(new Time(DateTime.UtcNow)));
				std[attribute2.AttrType] = attribute2;
			}
			if (!std.Contains(CmsAttributes.MessageDigest))
			{
				byte[] str = (byte[])parameters[CmsAttributeTableParameter.Digest];
				Attribute attribute3 = new Attribute(CmsAttributes.MessageDigest, new DerSet(new DerOctetString(str)));
				std[attribute3.AttrType] = attribute3;
			}
		}

		// Token: 0x06003AAF RID: 15023 RVA: 0x0016BC01 File Offset: 0x00169E01
		public virtual AttributeTable GetAttributes(IDictionary parameters)
		{
			return new AttributeTable(this.createStandardAttributeTable(parameters));
		}

		// Token: 0x04002657 RID: 9815
		private readonly IDictionary table;
	}
}
