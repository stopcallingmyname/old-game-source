using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200078C RID: 1932
	public class Evidence : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06004527 RID: 17703 RVA: 0x0018FDFF File Offset: 0x0018DFFF
		public Evidence(TimeStampTokenEvidence tstEvidence)
		{
			this.tstEvidence = tstEvidence;
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x0018FE0E File Offset: 0x0018E00E
		private Evidence(Asn1TaggedObject tagged)
		{
			if (tagged.TagNo == 0)
			{
				this.tstEvidence = TimeStampTokenEvidence.GetInstance(tagged, false);
			}
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x0018FE2B File Offset: 0x0018E02B
		public static Evidence GetInstance(object obj)
		{
			if (obj is Evidence)
			{
				return (Evidence)obj;
			}
			if (obj is Asn1TaggedObject)
			{
				return new Evidence(Asn1TaggedObject.GetInstance(obj));
			}
			throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x0600452A RID: 17706 RVA: 0x0018FE6A File Offset: 0x0018E06A
		public virtual TimeStampTokenEvidence TstEvidence
		{
			get
			{
				return this.tstEvidence;
			}
		}

		// Token: 0x0600452B RID: 17707 RVA: 0x0018FE72 File Offset: 0x0018E072
		public override Asn1Object ToAsn1Object()
		{
			if (this.tstEvidence != null)
			{
				return new DerTaggedObject(false, 0, this.tstEvidence);
			}
			return null;
		}

		// Token: 0x04002D45 RID: 11589
		private TimeStampTokenEvidence tstEvidence;
	}
}
