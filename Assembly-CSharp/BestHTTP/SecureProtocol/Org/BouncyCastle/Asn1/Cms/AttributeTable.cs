using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200077C RID: 1916
	public class AttributeTable
	{
		// Token: 0x060044AB RID: 17579 RVA: 0x0018E431 File Offset: 0x0018C631
		[Obsolete]
		public AttributeTable(Hashtable attrs)
		{
			this.attributes = Platform.CreateHashtable(attrs);
		}

		// Token: 0x060044AC RID: 17580 RVA: 0x0018E431 File Offset: 0x0018C631
		public AttributeTable(IDictionary attrs)
		{
			this.attributes = Platform.CreateHashtable(attrs);
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x0018E448 File Offset: 0x0018C648
		public AttributeTable(Asn1EncodableVector v)
		{
			this.attributes = Platform.CreateHashtable(v.Count);
			foreach (object obj in v)
			{
				Attribute instance = Attribute.GetInstance((Asn1Encodable)obj);
				this.AddAttribute(instance);
			}
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x0018E4B8 File Offset: 0x0018C6B8
		public AttributeTable(Asn1Set s)
		{
			this.attributes = Platform.CreateHashtable(s.Count);
			for (int num = 0; num != s.Count; num++)
			{
				Attribute instance = Attribute.GetInstance(s[num]);
				this.AddAttribute(instance);
			}
		}

		// Token: 0x060044AF RID: 17583 RVA: 0x0018E501 File Offset: 0x0018C701
		public AttributeTable(Attributes attrs) : this(Asn1Set.GetInstance(attrs.ToAsn1Object()))
		{
		}

		// Token: 0x060044B0 RID: 17584 RVA: 0x0018E514 File Offset: 0x0018C714
		private void AddAttribute(Attribute a)
		{
			DerObjectIdentifier attrType = a.AttrType;
			object obj = this.attributes[attrType];
			if (obj == null)
			{
				this.attributes[attrType] = a;
				return;
			}
			IList list;
			if (obj is Attribute)
			{
				list = Platform.CreateArrayList();
				list.Add(obj);
				list.Add(a);
			}
			else
			{
				list = (IList)obj;
				list.Add(a);
			}
			this.attributes[attrType] = list;
		}

		// Token: 0x170009B4 RID: 2484
		public Attribute this[DerObjectIdentifier oid]
		{
			get
			{
				object obj = this.attributes[oid];
				if (obj is IList)
				{
					return (Attribute)((IList)obj)[0];
				}
				return (Attribute)obj;
			}
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x0018E5BE File Offset: 0x0018C7BE
		[Obsolete("Use 'object[oid]' syntax instead")]
		public Attribute Get(DerObjectIdentifier oid)
		{
			return this[oid];
		}

		// Token: 0x060044B3 RID: 17587 RVA: 0x0018E5C8 File Offset: 0x0018C7C8
		public Asn1EncodableVector GetAll(DerObjectIdentifier oid)
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			object obj = this.attributes[oid];
			if (obj is IList)
			{
				using (IEnumerator enumerator = ((IList)obj).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						Attribute attribute = (Attribute)obj2;
						asn1EncodableVector.Add(new Asn1Encodable[]
						{
							attribute
						});
					}
					return asn1EncodableVector;
				}
			}
			if (obj != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					(Attribute)obj
				});
			}
			return asn1EncodableVector;
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x060044B4 RID: 17588 RVA: 0x0018E668 File Offset: 0x0018C868
		public int Count
		{
			get
			{
				int num = 0;
				foreach (object obj in this.attributes.Values)
				{
					if (obj is IList)
					{
						num += ((IList)obj).Count;
					}
					else
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x0018E6DC File Offset: 0x0018C8DC
		public IDictionary ToDictionary()
		{
			return Platform.CreateHashtable(this.attributes);
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x0018E6E9 File Offset: 0x0018C8E9
		[Obsolete("Use 'ToDictionary' instead")]
		public Hashtable ToHashtable()
		{
			return new Hashtable(this.attributes);
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x0018E6F8 File Offset: 0x0018C8F8
		public Asn1EncodableVector ToAsn1EncodableVector()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in this.attributes.Values)
			{
				if (obj is IList)
				{
					using (IEnumerator enumerator2 = ((IList)obj).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							asn1EncodableVector.Add(new Asn1Encodable[]
							{
								Attribute.GetInstance(obj2)
							});
						}
						continue;
					}
				}
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					Attribute.GetInstance(obj)
				});
			}
			return asn1EncodableVector;
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x0018E7CC File Offset: 0x0018C9CC
		public Attributes ToAttributes()
		{
			return new Attributes(this.ToAsn1EncodableVector());
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x0018E7D9 File Offset: 0x0018C9D9
		public AttributeTable Add(DerObjectIdentifier attrType, Asn1Encodable attrValue)
		{
			AttributeTable attributeTable = new AttributeTable(this.attributes);
			attributeTable.AddAttribute(new Attribute(attrType, new DerSet(attrValue)));
			return attributeTable;
		}

		// Token: 0x060044BA RID: 17594 RVA: 0x0018E7F8 File Offset: 0x0018C9F8
		public AttributeTable Remove(DerObjectIdentifier attrType)
		{
			AttributeTable attributeTable = new AttributeTable(this.attributes);
			attributeTable.attributes.Remove(attrType);
			return attributeTable;
		}

		// Token: 0x04002CFE RID: 11518
		private readonly IDictionary attributes;
	}
}
