using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200079C RID: 1948
	public class RecipientIdentifier : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x060045A6 RID: 17830 RVA: 0x00190F62 File Offset: 0x0018F162
		public RecipientIdentifier(IssuerAndSerialNumber id)
		{
			this.id = id;
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x00190F71 File Offset: 0x0018F171
		public RecipientIdentifier(Asn1OctetString id)
		{
			this.id = new DerTaggedObject(false, 0, id);
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x00190F62 File Offset: 0x0018F162
		public RecipientIdentifier(Asn1Object id)
		{
			this.id = id;
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x00190F88 File Offset: 0x0018F188
		public static RecipientIdentifier GetInstance(object o)
		{
			if (o == null || o is RecipientIdentifier)
			{
				return (RecipientIdentifier)o;
			}
			if (o is IssuerAndSerialNumber)
			{
				return new RecipientIdentifier((IssuerAndSerialNumber)o);
			}
			if (o is Asn1OctetString)
			{
				return new RecipientIdentifier((Asn1OctetString)o);
			}
			if (o is Asn1Object)
			{
				return new RecipientIdentifier((Asn1Object)o);
			}
			throw new ArgumentException("Illegal object in RecipientIdentifier: " + Platform.GetTypeName(o));
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x060045AA RID: 17834 RVA: 0x00190FF8 File Offset: 0x0018F1F8
		public bool IsTagged
		{
			get
			{
				return this.id is Asn1TaggedObject;
			}
		}

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x060045AB RID: 17835 RVA: 0x00191008 File Offset: 0x0018F208
		public Asn1Encodable ID
		{
			get
			{
				if (this.id is Asn1TaggedObject)
				{
					return Asn1OctetString.GetInstance((Asn1TaggedObject)this.id, false);
				}
				return IssuerAndSerialNumber.GetInstance(this.id);
			}
		}

		// Token: 0x060045AC RID: 17836 RVA: 0x00191034 File Offset: 0x0018F234
		public override Asn1Object ToAsn1Object()
		{
			return this.id.ToAsn1Object();
		}

		// Token: 0x04002D6F RID: 11631
		private Asn1Encodable id;
	}
}
