using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs
{
	// Token: 0x020002CA RID: 714
	public abstract class Pkcs12Entry
	{
		// Token: 0x06001A47 RID: 6727 RVA: 0x000C4C5C File Offset: 0x000C2E5C
		protected internal Pkcs12Entry(IDictionary attributes)
		{
			this.attributes = attributes;
			foreach (object obj in attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if (!(dictionaryEntry.Key is string))
				{
					throw new ArgumentException("Attribute keys must be of type: " + typeof(string).FullName, "attributes");
				}
				if (!(dictionaryEntry.Value is Asn1Encodable))
				{
					throw new ArgumentException("Attribute values must be of type: " + typeof(Asn1Encodable).FullName, "attributes");
				}
			}
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x000C4D1C File Offset: 0x000C2F1C
		[Obsolete("Use 'object[index]' syntax instead")]
		public Asn1Encodable GetBagAttribute(DerObjectIdentifier oid)
		{
			return (Asn1Encodable)this.attributes[oid.Id];
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x000C4D34 File Offset: 0x000C2F34
		[Obsolete("Use 'object[index]' syntax instead")]
		public Asn1Encodable GetBagAttribute(string oid)
		{
			return (Asn1Encodable)this.attributes[oid];
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x000C4D47 File Offset: 0x000C2F47
		[Obsolete("Use 'BagAttributeKeys' property")]
		public IEnumerator GetBagAttributeKeys()
		{
			return this.attributes.Keys.GetEnumerator();
		}

		// Token: 0x17000359 RID: 857
		public Asn1Encodable this[DerObjectIdentifier oid]
		{
			get
			{
				return (Asn1Encodable)this.attributes[oid.Id];
			}
		}

		// Token: 0x1700035A RID: 858
		public Asn1Encodable this[string oid]
		{
			get
			{
				return (Asn1Encodable)this.attributes[oid];
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06001A4D RID: 6733 RVA: 0x000C4D59 File Offset: 0x000C2F59
		public IEnumerable BagAttributeKeys
		{
			get
			{
				return new EnumerableProxy(this.attributes.Keys);
			}
		}

		// Token: 0x040018BC RID: 6332
		private readonly IDictionary attributes;
	}
}
