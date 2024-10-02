using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200068B RID: 1675
	public class AttributeTable
	{
		// Token: 0x06003E0B RID: 15883 RVA: 0x00175D74 File Offset: 0x00173F74
		public AttributeTable(IDictionary attrs)
		{
			this.attributes = Platform.CreateHashtable(attrs);
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x00175D74 File Offset: 0x00173F74
		[Obsolete]
		public AttributeTable(Hashtable attrs)
		{
			this.attributes = Platform.CreateHashtable(attrs);
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x00175D88 File Offset: 0x00173F88
		public AttributeTable(Asn1EncodableVector v)
		{
			this.attributes = Platform.CreateHashtable(v.Count);
			for (int num = 0; num != v.Count; num++)
			{
				AttributeX509 instance = AttributeX509.GetInstance(v[num]);
				this.attributes.Add(instance.AttrType, instance);
			}
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x00175DDC File Offset: 0x00173FDC
		public AttributeTable(Asn1Set s)
		{
			this.attributes = Platform.CreateHashtable(s.Count);
			for (int num = 0; num != s.Count; num++)
			{
				AttributeX509 instance = AttributeX509.GetInstance(s[num]);
				this.attributes.Add(instance.AttrType, instance);
			}
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x00175E30 File Offset: 0x00174030
		public AttributeX509 Get(DerObjectIdentifier oid)
		{
			return (AttributeX509)this.attributes[oid];
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x00175E43 File Offset: 0x00174043
		[Obsolete("Use 'ToDictionary' instead")]
		public Hashtable ToHashtable()
		{
			return new Hashtable(this.attributes);
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x00175E50 File Offset: 0x00174050
		public IDictionary ToDictionary()
		{
			return Platform.CreateHashtable(this.attributes);
		}

		// Token: 0x0400278A RID: 10122
		private readonly IDictionary attributes;
	}
}
