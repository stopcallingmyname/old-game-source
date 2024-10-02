using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006A9 RID: 1705
	public class PolicyMappings : Asn1Encodable
	{
		// Token: 0x06003EFD RID: 16125 RVA: 0x00179155 File Offset: 0x00177355
		public PolicyMappings(Asn1Sequence seq)
		{
			this.seq = seq;
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x00179164 File Offset: 0x00177364
		public PolicyMappings(Hashtable mappings) : this(mappings)
		{
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x00179170 File Offset: 0x00177370
		public PolicyMappings(IDictionary mappings)
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in mappings.Keys)
			{
				string text = (string)obj;
				string identifier = (string)mappings[text];
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerSequence(new Asn1Encodable[]
					{
						new DerObjectIdentifier(text),
						new DerObjectIdentifier(identifier)
					})
				});
			}
			this.seq = new DerSequence(asn1EncodableVector);
		}

		// Token: 0x06003F00 RID: 16128 RVA: 0x0017921C File Offset: 0x0017741C
		public override Asn1Object ToAsn1Object()
		{
			return this.seq;
		}

		// Token: 0x040027FE RID: 10238
		private readonly Asn1Sequence seq;
	}
}
