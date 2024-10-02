using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x02000240 RID: 576
	public class X509Attribute : Asn1Encodable
	{
		// Token: 0x060014B9 RID: 5305 RVA: 0x000AB77A File Offset: 0x000A997A
		internal X509Attribute(Asn1Encodable at)
		{
			this.attr = AttributeX509.GetInstance(at);
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x000AB78E File Offset: 0x000A998E
		public X509Attribute(string oid, Asn1Encodable value)
		{
			this.attr = new AttributeX509(new DerObjectIdentifier(oid), new DerSet(value));
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x000AB7AD File Offset: 0x000A99AD
		public X509Attribute(string oid, Asn1EncodableVector value)
		{
			this.attr = new AttributeX509(new DerObjectIdentifier(oid), new DerSet(value));
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060014BC RID: 5308 RVA: 0x000AB7CC File Offset: 0x000A99CC
		public string Oid
		{
			get
			{
				return this.attr.AttrType.Id;
			}
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x000AB7E0 File Offset: 0x000A99E0
		public Asn1Encodable[] GetValues()
		{
			Asn1Set attrValues = this.attr.AttrValues;
			Asn1Encodable[] array = new Asn1Encodable[attrValues.Count];
			for (int num = 0; num != attrValues.Count; num++)
			{
				array[num] = attrValues[num];
			}
			return array;
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x000AB821 File Offset: 0x000A9A21
		public override Asn1Object ToAsn1Object()
		{
			return this.attr.ToAsn1Object();
		}

		// Token: 0x0400161F RID: 5663
		private readonly AttributeX509 attr;
	}
}
